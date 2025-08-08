using MRCustom;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

public sealed class StringName : IEquatable<StringName>, IEquatable<string>
{
    private static bool configured = false;
    private static bool debugStringName = false;

    // Table structure to hold shared strings
    private static class Table
    {
        public const int TableBits = 16;
        public const int TableLen = 1 << TableBits;
        public const int TableMask = TableLen - 1;

        public static readonly Data[] table = new Data[TableLen];
        public static readonly object mutex = new object();
    }

    private class Data
    {
        public string name;
        public int refCount;
        public int staticCount;
        public uint hash;
        public Data next;
        public Data prev;

#if DEBUG
        public int debugReferences;
#endif
    }

    private Data data;

    // Static constructor for initialization
    static StringName()
    {
        Setup();
    }

    public static void Setup()
    {
        if (configured) return;

        for (int i = 0; i < Table.TableLen; i++)
        {
            Table.table[i] = null;
        }

        configured = true;
    }

    public static void Cleanup()
    {
        lock (Table.mutex)
        {
#if DEBUG
            if (debugStringName)
            {
                List<Data> allData = new List<Data>();
                for (int i = 0; i < Table.TableLen; i++)
                {
                    Data d = Table.table[i];
                    while (d != null)
                    {
                        allData.Add(d);
                        d = d.next;
                    }
                }

                allData.Sort((a, b) => b.debugReferences.CompareTo(a.debugReferences));

                int unreferenced = 0;
                int rarelyReferenced = 0;
                for (int i = 0; i < allData.Count; i++)
                {
                    Plugin.LogDebug($"{i + 1}: {allData[i].name} - {allData[i].debugReferences}");
                    if (allData[i].debugReferences == 0) unreferenced++;
                    else if (allData[i].debugReferences < 5) rarelyReferenced++;
                }

                Plugin.LogDebug($"\nOut of {allData.Count} StringNames, {unreferenced} were never referenced (0 times) ({(unreferenced / (float)allData.Count) * 100:F2}%).");
                Plugin.LogDebug($"Out of {allData.Count} StringNames, {rarelyReferenced} were rarely referenced (1-4 times) ({(rarelyReferenced / (float)allData.Count) * 100:F2}%).");
            }
#endif

            int lostStrings = 0;
            for (int i = 0; i < Table.TableLen; i++)
            {
                while (Table.table[i] != null)
                {
                    Data d = Table.table[i];
                    if (d.staticCount != d.refCount)
                    {
                        lostStrings++;
                        Plugin.LogDebug($"Orphan StringName: {d.name} (static: {d.staticCount}, total: {d.refCount})");
                    }

                    Table.table[i] = d.next;
                }
            }

            if (lostStrings > 0)
            {
                Plugin.LogDebug($"StringName: {lostStrings} unclaimed string names at exit.");
            }

            configured = false;
        }
    }

    private void Unref()
    {
        if (!configured) throw new InvalidOperationException("StringName system not configured");

        if (data != null && Interlocked.Decrement(ref data.refCount) == 0)
        {
            lock (Table.mutex)
            {
                if (data.prev != null)
                {
                    data.prev.next = data.next;
                }
                else
                {
                    int idx = (int)(data.hash & Table.TableMask);
                    Table.table[idx] = data.next;
                }

                if (data.next != null)
                {
                    data.next.prev = data.prev;
                }
            }

            data = null;
        }
    }

    public bool Equals(string other)
    {
        if (data != null)
        {
            return data.name == other;
        }
        return string.IsNullOrEmpty(other);
    }

    public bool Equals(StringName other)
    {
        if (ReferenceEquals(this, other)) return true;
        if (other is null) return false;
        return ReferenceEquals(data, other.data);
    }

    public override bool Equals(object obj)
    {
        return Equals(obj as StringName);
    }

    public override int GetHashCode()
    {
        return data?.name.GetHashCode() ?? 0;
    }

    public char this[int index]
    {
        get
        {
            if (data != null)
            {
                return data.name[index];
            }
            throw new IndexOutOfRangeException();
        }
    }

    public int Length => data?.name.Length ?? 0;

    public StringName()
    {
        if (!configured) throw new InvalidOperationException("StringName system not configured");
    }

    public StringName(StringName other)
    {
        if (!configured) throw new InvalidOperationException("StringName system not configured");

        if (other.data != null && Interlocked.Increment(ref other.data.refCount) > 0)
        {
            data = other.data;
        }
    }

    public StringName(string name, bool isStatic = false)
    {
        if (!configured) throw new InvalidOperationException("StringName system not configured");

        if (string.IsNullOrEmpty(name))
        {
            return;
        }

        uint hash = (uint)name.GetHashCode();
        int idx = (int)(hash & Table.TableMask);

        lock (Table.mutex)
        {
            Data existing = Table.table[idx];
            while (existing != null)
            {
                if (existing.hash == hash && existing.name == name)
                {
                    break;
                }
                existing = existing.next;
            }

            if (existing != null)
            {
                if (Interlocked.Increment(ref existing.refCount) > 0)
                {
                    data = existing;
                    if (isStatic)
                    {
                        Interlocked.Increment(ref existing.staticCount);
                    }
#if DEBUG
                    if (debugStringName)
                    {
                        Interlocked.Increment(ref existing.debugReferences);
                    }
#endif
                    return;
                }
            }

            data = new Data
            {
                name = name,
                refCount = 1,
                staticCount = isStatic ? 1 : 0,
                hash = hash,
                next = Table.table[idx],
                prev = null
            };

#if DEBUG
            if (debugStringName)
            {
                Interlocked.Increment(ref data.refCount);
                Interlocked.Increment(ref data.staticCount);
            }
#endif

            if (Table.table[idx] != null)
            {
                Table.table[idx].prev = data;
            }
            Table.table[idx] = data;
        }
    }

    ~StringName()
    {
        Unref();
    }

    public static bool operator ==(StringName left, StringName right)
    {
        if (ReferenceEquals(left, right)) return true;
        if (left is null || right is null) return false;
        return left.Equals(right);
    }

    public static bool operator !=(StringName left, StringName right)
    {
        return !(left == right);
    }

    public static bool operator ==(StringName left, string right)
    {
        if (left is null) return string.IsNullOrEmpty(right);
        return left.Equals(right);
    }

    public static bool operator !=(StringName left, string right)
    {
        return !(left == right);
    }

    public static bool operator ==(string left, StringName right)
    {
        if (right is null) return string.IsNullOrEmpty(left);
        return right.Equals(left);
    }

    public static bool operator !=(string left, StringName right)
    {
        return !(left == right);
    }

    public static implicit operator StringName(string str)
    {
        return new StringName(str);
    }

    public override string ToString()
    {
        return data?.name ?? string.Empty;
    }
}