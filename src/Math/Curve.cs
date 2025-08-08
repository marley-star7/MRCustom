/*
namespace MRCustom.Math;

public class Curve
{
    public const string SignalRangeChanged = "range_changed";
    public const string SignalDomainChanged = "domain_changed";

    public enum TangentMode
    {
        Free,
        Linear,
        Count
    }

    public struct Point
    {
        public Vector2 Position;
        public float LeftTangent;
        public float RightTangent;
        public TangentMode LeftMode;
        public TangentMode RightMode;

        public Point(Vector2 position, float leftTangent, float rightTangent, TangentMode leftMode, TangentMode rightMode)
        {
            Position = position;
            LeftTangent = leftTangent;
            RightTangent = rightTangent;
            LeftMode = leftMode;
            RightMode = rightMode;
        }
    }

    private List<Point> _points = new List<Point>();
    private float _minValue = 0;
    private float _maxValue = 1;
    private float _minDomain = 0;
    private float _maxDomain = 1;
    private int _bakeResolution = 100;
    private bool _bakedCacheDirty = true;
    private float[] _bakedCache;
    private float _bakedMaxOfs;

    public int PointCount => _points.Count;

    public float MinValue
    {
        get => _minValue;
        set
        {
            _minValue = Mathf.Min(value, _maxValue - 0.01f);
            foreach (var p in _points)
            {
                _minValue = Mathf.Min(_minValue, p.Position.y);
            }
        }
    }

    public float MaxValue
    {
        get => _maxValue;
        set
        {
            _maxValue = Mathf.Max(value, _minValue + 0.01f);
            foreach (var p in _points)
            {
                _maxValue = Mathf.Max(_maxValue, p.Position.y);
            }
        }
    }

    public float MinDomain
    {
        get => _minDomain;
        set
        {
            _minDomain = Mathf.Min(value, _maxDomain - 0.01f);
            if (_points.Count > 0 && _minDomain > _points[0].Position.x)
            {
                _minDomain = _points[0].Position.x;
            }
            _bakedCacheDirty = true;
        }
    }

    public float MaxDomain
    {
        get => _maxDomain;
        set
        {
            _maxDomain = Mathf.Max(value, _minDomain + 0.01f);
            if (_points.Count > 0 && _maxDomain < _points[_points.Count - 1].Position.x)
            {
                _maxDomain = _points[_points.Count - 1].Position.x;
            }
            _bakedCacheDirty = true;
        }
    }

    public float DomainRange => _maxDomain - _minDomain;
    public float ValueRange => _maxValue - _minValue;

    public int BakeResolution
    {
        get => _bakeResolution;
        set
        {
            if (value < 1 || value > 1000)
                throw new ArgumentOutOfRangeException(nameof(value), "Bake resolution must be between 1 and 1000");
            _bakeResolution = value;
            _bakedCacheDirty = true;
        }
    }

    public void SetPointCount(int count)
    {
        if (count < 0)
            throw new ArgumentOutOfRangeException(nameof(count), "Point count cannot be negative");

        if (_points.Count == count)
            return;

        if (_points.Count > count)
        {
            _points.RemoveRange(count, _points.Count - count);
            MarkDirty();
        }
        else
        {
            for (int i = count - _points.Count; i > 0; i--)
            {
                _AddPoint(Vector2.zero);
            }
        }
    }

    private int _AddPoint(Vector2 position, float leftTangent = 0, float rightTangent = 0,
                         TangentMode leftMode = TangentMode.Free, TangentMode rightMode = TangentMode.Free,
                         bool markDirty = true)
    {
        // Clamp position to valid range
        position.x = Mathf.Clamp(position.x, _minDomain, _maxDomain);
        position.y = Mathf.Clamp(position.y, _minValue, _maxValue);

        int index = -1;

        if (_points.Count == 0)
        {
            _points.Add(new Point(position, leftTangent, rightTangent, leftMode, rightMode));
            index = 0;
        }
        else if (_points.Count == 1)
        {
            float diff = position.x - _points[0].Position.x;
            if (diff > 0)
            {
                _points.Add(new Point(position, leftTangent, rightTangent, leftMode, rightMode));
                index = 1;
            }
            else
            {
                _points.Insert(0, new Point(position, leftTangent, rightTangent, leftMode, rightMode));
                index = 0;
            }
        }
        else
        {
            int i = GetIndex(position.x);
            if (i == 0 && position.x < _points[0].Position.x)
            {
                _points.Insert(0, new Point(position, leftTangent, rightTangent, leftMode, rightMode));
                index = 0;
            }
            else
            {
                i++;
                _points.Insert(i, new Point(position, leftTangent, rightTangent, leftMode, rightMode));
                index = i;
            }
        }

        UpdateAutoTangents(index);

        if (markDirty)
        {
            MarkDirty();
        }

        return index;
    }

    public int AddPoint(Vector2 position, float leftTangent = 0, float rightTangent = 0,
                       TangentMode leftMode = TangentMode.Free, TangentMode rightMode = TangentMode.Free)
    {
        int index = _AddPoint(position, leftTangent, rightTangent, leftMode, rightMode);
        
        return index;
    }

    public int GetIndex(float offset)
    {
        if (_points.Count == 0)
            return -1;

        int imin = 0;
        int imax = _points.Count - 1;

        while (imax - imin > 1)
        {
            int m = (imin + imax) / 2;
            float a = _points[m].Position.x;
            float b = _points[m + 1].Position.x;

            if (a < offset && b < offset)
            {
                imin = m;
            }
            else if (a > offset)
            {
                imax = m;
            }
            else
            {
                return m;
            }
        }

        if (offset > _points[imax].Position.x)
        {
            return imax;
        }
        return imin;
    }

    public void CleanDupes()
    {
        bool dirty = false;

        for (int i = 1; i < _points.Count; i++)
        {
            float diff = _points[i - 1].Position.x - _points[i].Position.x;
            if (diff <= Mathf.Epsilon)
            {
                _points.RemoveAt(i);
                i--;
                dirty = true;
            }
        }

        if (dirty)
        {
            MarkDirty();
        }
    }

    public void SetPointLeftTangent(int index, float tangent)
    {
        if (index < 0 || index >= _points.Count)
            throw new IndexOutOfRangeException();

        var point = _points[index];
        point.LeftTangent = tangent;
        point.LeftMode = TangentMode.Free;
        _points[index] = point;
        MarkDirty();
    }

    public void SetPointRightTangent(int index, float tangent)
    {
        if (index < 0 || index >= _points.Count)
            throw new IndexOutOfRangeException();

        var point = _points[index];
        point.RightTangent = tangent;
        point.RightMode = TangentMode.Free;
        _points[index] = point;
        MarkDirty();
    }

    public void SetPointLeftMode(int index, TangentMode mode)
    {
        if (index < 0 || index >= _points.Count)
            throw new IndexOutOfRangeException();

        var point = _points[index];
        point.LeftMode = mode;
        if (index > 0 && mode == TangentMode.Linear)
        {
            Vector2 v = (_points[index - 1].Position - _points[index].Position).normalized;
            point.LeftTangent = v.y / v.x;
        }
        _points[index] = point;
        MarkDirty();
    }

    public void SetPointRightMode(int index, TangentMode mode)
    {
        if (index < 0 || index >= _points.Count)
            throw new IndexOutOfRangeException();

        var point = _points[index];
        point.RightMode = mode;
        if (index < _points.Count - 1 && mode == TangentMode.Linear)
        {
            Vector2 v = (_points[index + 1].Position - _points[index].Position).normalized;
            point.RightTangent = v.y / v.x;
        }
        _points[index] = point;
        MarkDirty();
    }

    public float GetPointLeftTangent(int index)
    {
        if (index < 0 || index >= _points.Count)
            return 0;
        return _points[index].LeftTangent;
    }

    public float GetPointRightTangent(int index)
    {
        if (index < 0 || index >= _points.Count)
            return 0;
        return _points[index].RightTangent;
    }

    public TangentMode GetPointLeftMode(int index)
    {
        if (index < 0 || index >= _points.Count)
            return TangentMode.Free;
        return _points[index].LeftMode;
    }

    public TangentMode GetPointRightMode(int index)
    {
        if (index < 0 || index >= _points.Count)
            return TangentMode.Free;
        return _points[index].RightMode;
    }

    private void _RemovePoint(int index, bool markDirty = true)
    {
        if (index < 0 || index >= _points.Count)
            throw new IndexOutOfRangeException();

        _points.RemoveAt(index);
        if (markDirty)
        {
            MarkDirty();
        }
    }

    public void RemovePoint(int index)
    {
        _RemovePoint(index);
    }

    public void ClearPoints()
    {
        if (_points.Count == 0)
            return;

        _points.Clear();
        MarkDirty();
    }

    public void SetPointValue(int index, float position)
    {
        if (index < 0 || index >= _points.Count)
            throw new IndexOutOfRangeException();

        var point = _points[index];
        point.Position.y = position;
        _points[index] = point;
        UpdateAutoTangents(index);
        MarkDirty();
    }

    public int SetPointOffset(int index, float offset)
    {
        if (index < 0 || index >= _points.Count)
            return -1;

        Point p = _points[index];
        _RemovePoint(index, false);
        int newIndex = _AddPoint(new Vector2(offset, p.Position.y), p.LeftTangent, p.RightTangent, p.LeftMode, p.RightMode, false);
        if (index != newIndex)
        {
            UpdateAutoTangents(index);
        }
        UpdateAutoTangents(newIndex);
        MarkDirty();
        return newIndex;
    }

    public Vector2 GetPointPosition(int index)
    {
        if (index < 0 || index >= _points.Count)
            return Vector2.zero;
        return _points[index].Position;
    }

    public Point GetPoint(int index)
    {
        if (index < 0 || index >= _points.Count)
            return new Point();
        return _points[index];
    }

    public void UpdateAutoTangents(int index)
    {
        if (index < 0 || index >= _points.Count)
            return;

        Point p = _points[index];

        if (index > 0)
        {
            if (p.LeftMode == TangentMode.Linear)
            {
                Vector2 v = (_points[index - 1].Position - p.Position).normalized;
                p.LeftTangent = v.y / v.x;
            }
            if (_points[index - 1].RightMode == TangentMode.Linear)
            {
                Vector2 v = (_points[index - 1].Position - p.Position).normalized;
                var prevPoint = _points[index - 1];
                prevPoint.RightTangent = v.y / v.x;
                _points[index - 1] = prevPoint;
            }
        }

        if (index < _points.Count - 1)
        {
            if (p.RightMode == TangentMode.Linear)
            {
                Vector2 v = (_points[index + 1].Position - p.Position).normalized;
                p.RightTangent = v.y / v.x;
            }
            if (_points[index + 1].LeftMode == TangentMode.Linear)
            {
                Vector2 v = (_points[index + 1].Position - p.Position).normalized;
                var nextPoint = _points[index + 1];
                nextPoint.LeftTangent = v.y / v.x;
                _points[index + 1] = nextPoint;
            }
        }

        _points[index] = p;
    }

    public float Sample(float offset)
    {
        if (_points.Count == 0)
            return 0;
        if (_points.Count == 1)
            return _points[0].Position.y;

        int i = GetIndex(offset);

        if (i == _points.Count - 1)
        {
            return _points[i].Position.y;
        }

        float local = offset - _points[i].Position.x;

        if (i == 0 && local <= 0)
        {
            return _points[0].Position.y;
        }

        return SampleLocalNoCheck(i, local);
    }

    public float SampleLocalNoCheck(int index, float localOffset)
    {
        Point a = _points[index];
        Point b = _points[index + 1];

        float d = b.Position.x - a.Position.x;
        if (Mathf.IsZeroApprox(d))
        {
            return b.Position.y;
        }
        localOffset /= d;
        d /= 3.0f;
        float yac = a.Position.y + d * a.RightTangent;
        float ybc = b.Position.y - d * b.LeftTangent;

        return Mathf.BezierInterpolate(a.Position.y, yac, ybc, b.Position.y, localOffset);
    }

    public void MarkDirty()
    {
        _bakedCacheDirty = true;
    }

    public void Bake()
    {
        _Bake();
    }

    private void _Bake()
    {
        if (!_bakedCacheDirty)
            return;

        _bakedCacheDirty = false;

        if (_points.Count == 0)
        {
            _bakedCache = Array.Empty<float>();
            return;
        }

        _bakedCache = new float[_bakeResolution];

        for (int i = 1; i < _bakeResolution - 1; i++)
        {
            float x = DomainRange * i / (float)(_bakeResolution - 1) + _minDomain;
            float y = Sample(x);
            _bakedCache[i] = y;
        }

        if (_points.Count != 0)
        {
            _bakedCache[0] = _points[0].Position.y;
            _bakedCache[_bakedCache.Length - 1] = _points[_points.Count - 1].Position.y;
        }
    }

    public float SampleBaked(float offset)
    {
        if (!Mathf.IsFinite(offset))
            return 0;

        if (_bakedCacheDirty)
        {
            _Bake();
        }

        if (_bakedCache.Length == 0)
            return 0;
        if (_bakedCache.Length == 1)
            return _bakedCache[0];

        float fi = (offset - _minDomain) / DomainRange * (_bakedCache.Length - 1);
        int i = (int)Mathf.Floor(fi);
        if (i < 0)
        {
            i = 0;
            fi = 0;
        }
        else if (i >= _bakedCache.Length)
        {
            i = _bakedCache.Length - 1;
            fi = 0;
        }

        if (i + 1 < _bakedCache.Length)
        {
            float t = fi - i;
            return Mathf.Lerp(_bakedCache[i], _bakedCache[i + 1], t);
        }
        else
        {
            return _bakedCache[_bakedCache.Length - 1];
        }
    }

    public void EnsureDefaultSetup(float min, float max)
    {
        if (_points.Count == 0 && Mathf.IsEqualApprox(_minValue, 0) && Mathf.IsEqualApprox(_maxValue, 1))
        {
            AddPoint(new Vector2(0, 1));
            AddPoint(new Vector2(1, 1));
            MinValue = min;
            MaxValue = max;
        }
    }
}

public class Curve2D : Resource
{
    public struct Point
    {
        public Vector2 Position;
        public Vector2 In;
        public Vector2 Out;
    }

    private List<Point> _points = new List<Point>();
    private float _bakeInterval = 20.0f;
    private bool _bakedCacheDirty = true;
    private Vector2[] _bakedPointCache;
    private float[] _bakedDistCache;
    private Vector2[] _bakedForwardVectorCache;
    private float _bakedMaxOfs;

    public int PointCount => _points.Count;

    public float BakeInterval
    {
        get => _bakeInterval;
        set
        {
            _bakeInterval = value;
            MarkDirty();
        }
    }

    public float BakedLength
    {
        get
        {
            if (_bakedCacheDirty)
                _Bake();
            return _bakedMaxOfs;
        }
    }

    public void SetPointCount(int count)
    {
        if (count < 0)
            throw new ArgumentOutOfRangeException(nameof(count), "Point count cannot be negative");

        if (_points.Count == count)
            return;

        if (_points.Count > count)
        {
            _points.RemoveRange(count, _points.Count - count);
            MarkDirty();
        }
        else
        {
            for (int i = count - _points.Count; i > 0; i--)
            {
                _AddPoint(Vector2.zero);
            }
        }
        
    }

    private void _AddPoint(Vector2 position, Vector2 inDir, Vector2 outDir, int atPos = -1)
    {
        Point n = new Point
        {
            Position = position,
            In = inDir,
            Out = outDir
        };

        if (atPos >= 0 && atPos < _points.Count)
        {
            _points.Insert(atPos, n);
        }
        else
        {
            _points.Add(n);
        }

        MarkDirty();
    }

    public void AddPoint(Vector2 position, Vector2 inDir = default, Vector2 outDir = default, int atPos = -1)
    {
        _AddPoint(position, inDir, outDir, atPos);
        
    }

    public void SetPointPosition(int index, Vector2 position)
    {
        if (index < 0 || index >= _points.Count)
            throw new IndexOutOfRangeException();

        var point = _points[index];
        point.Position = position;
        _points[index] = point;
        MarkDirty();
    }

    public Vector2 GetPointPosition(int index)
    {
        if (index < 0 || index >= _points.Count)
            return Vector2.zero;
        return _points[index].Position;
    }

    public void SetPointIn(int index, Vector2 inDir)
    {
        if (index < 0 || index >= _points.Count)
            throw new IndexOutOfRangeException();

        var point = _points[index];
        point.In = inDir;
        _points[index] = point;
        MarkDirty();
    }

    public Vector2 GetPointIn(int index)
    {
        if (index < 0 || index >= _points.Count)
            return Vector2.zero;
        return _points[index].In;
    }

    public void SetPointOut(int index, Vector2 outDir)
    {
        if (index < 0 || index >= _points.Count)
            throw new IndexOutOfRangeException();

        var point = _points[index];
        point.Out = outDir;
        _points[index] = point;
        MarkDirty();
    }

    public Vector2 GetPointOut(int index)
    {
        if (index < 0 || index >= _points.Count)
            return Vector2.zero;
        return _points[index].Out;
    }

    private void _RemovePoint(int index)
    {
        if (index < 0 || index >= _points.Count)
            throw new IndexOutOfRangeException();

        _points.RemoveAt(index);
        MarkDirty();
    }

    public void RemovePoint(int index)
    {
        _RemovePoint(index);
        
    }

    public void ClearPoints()
    {
        if (_points.Count == 0)
            return;

        _points.Clear();
        MarkDirty();
        
    }

    public Vector2 Sample(int index, float t)
    {
        if (_points.Count == 0)
            return Vector2.zero;

        if (index >= _points.Count - 1)
        {
            return _points[_points.Count - 1].Position;
        }
        else if (index < 0)
        {
            return _points[0].Position;
        }

        Vector2 p0 = _points[index].Position;
        Vector2 p1 = p0 + _points[index].Out;
        Vector2 p3 = _points[index + 1].Position;
        Vector2 p2 = p3 + _points[index + 1].In;

        return p0.BezierInterpolate(p1, p2, p3, t);
    }

    public Vector2 Samplef(float findex)
    {
        if (findex < 0)
        {
            findex = 0;
        }
        else if (findex >= _points.Count)
        {
            findex = _points.Count;
        }

        return Sample((int)findex, Mathf.PosMod(findex, 1.0f));
    }

    public void MarkDirty()
    {
        _bakedCacheDirty = true;
    }

    private void _Bake()
    {
        if (!_bakedCacheDirty)
            return;

        _bakedCacheDirty = false;
        _bakedMaxOfs = 0;

        if (_points.Count == 0)
        {
            _bakedPointCache = Array.Empty<Vector2>();
            _bakedDistCache = Array.Empty<float>();
            _bakedForwardVectorCache = Array.Empty<Vector2>();
            return;
        }

        if (_points.Count == 1)
        {
            _bakedPointCache = new Vector2[] { _points[0].Position };
            _bakedDistCache = new float[] { 0.0f };
            _bakedForwardVectorCache = new Vector2[] { new Vector2(0.0f, 0.1f) };
            return;
        }

        // Tessellate curve to (almost) even length segments
        var midpoints = _TessellateEvenLength(10, _bakeInterval);

        int pc = 1;
        for (int i = 0; i < _points.Count - 1; i++)
        {
            pc++;
            pc += midpoints[i].Count;
        }

        _bakedPointCache = new Vector2[pc];
        _bakedDistCache = new float[pc];
        _bakedForwardVectorCache = new Vector2[pc];

        _bakedPointCache[0] = _points[0].Position;
        _bakedForwardVectorCache[0] = _CalculateTangent(
            _points[0].Position,
            _points[0].Position + _points[0].Out,
            _points[1].Position + _points[1].In,
            _points[1].Position,
            0.0f);

        int pidx = 0;

        for (int i = 0; i < _points.Count - 1; i++)
        {
            foreach (var kv in midpoints[i])
            {
                pidx++;
                _bakedPointCache[pidx] = kv.Value;
                _bakedForwardVectorCache[pidx] = _CalculateTangent(
                    _points[i].Position,
                    _points[i].Position + _points[i].Out,
                    _points[i + 1].Position + _points[i + 1].In,
                    _points[i + 1].Position,
                    kv.Key);
            }

            pidx++;
            _bakedPointCache[pidx] = _points[i + 1].Position;
            _bakedForwardVectorCache[pidx] = _CalculateTangent(
                _points[i].Position,
                _points[i].Position + _points[i].Out,
                _points[i + 1].Position + _points[i + 1].In,
                _points[i + 1].Position,
                1.0f);
        }

        // Calculate distances
        _bakedDistCache[0] = 0.0f;
        for (int i = 0; i < pc - 1; i++)
        {
            _bakedDistCache[i + 1] = _bakedDistCache[i] + _bakedPointCache[i].DistanceTo(_bakedPointCache[i + 1]);
        }
        _bakedMaxOfs = _bakedDistCache[pc - 1];
    }

    private Vector2 _CalculateTangent(Vector2 begin, Vector2 control1, Vector2 control2, Vector2 end, float t)
    {
        if (Mathf.IsZeroApprox(t - 0.0f))
        {
            if (control1.IsEqualApprox(begin))
            {
                if (control1.IsEqualApprox(control2))
                {
                    return (end - begin).normalized;
                }
                return (control2 - begin).normalized;
            }
        }
        else if (Mathf.IsZeroApprox(t - 1.0f))
        {
            if (control2.IsEqualApprox(end))
            {
                if (control2.IsEqualApprox(control1))
                {
                    return (end - begin).normalized;
                }
                return (end - control1).normalized;
            }
        }

        if (control1.IsEqualApprox(end) && control2.IsEqualApprox(begin))
        {
            return (end - begin).normalized;
        }

        return begin.BezierDerivative(control1, control2, end, t).normalized;
    }

    public Vector2 SampleBaked(float offset, bool cubic = false)
    {
        if (!Mathf.IsFinite(offset))
            return Vector2.zero;

        if (_bakedCacheDirty)
            _Bake();

        if (_bakedPointCache.Length == 0)
            return Vector2.zero;
        if (_bakedPointCache.Length == 1)
            return _bakedPointCache[0];

        offset = Mathf.Clamp(offset, 0.0f, BakedLength);

        var interval = _FindInterval(offset);
        return _SampleBaked(interval, cubic);
    }

    public Transform2D SampleBakedWithRotation(float offset, bool cubic = false)
    {
        if (!Mathf.IsFinite(offset))
            return Transform2D.Identity;

        if (_bakedCacheDirty)
            _Bake();

        if (_bakedPointCache.Length == 0)
            return Transform2D.Identity;
        if (_bakedPointCache.Length == 1)
            return new Transform2D(0, _bakedPointCache[0]);

        offset = Mathf.Clamp(offset, 0.0f, BakedLength);

        var interval = _FindInterval(offset);
        Vector2 pos = _SampleBaked(interval, cubic);
        Transform2D frame = _SamplePosture(interval);
        frame.Origin = pos;
        return frame;
    }

    private struct Interval
    {
        public int Index;
        public float Fraction;
    }

    private Interval _FindInterval(float offset)
    {
        if (_bakedCacheDirty)
            throw new InvalidOperationException("Baked cache is dirty");

        int pc = _bakedPointCache.Length;
        if (pc < 2)
            throw new InvalidOperationException("Less than two points in cache");

        int start = 0;
        int end = pc;
        int idx = (end + start) / 2;

        while (start < idx)
        {
            float currentOffset = _bakedDistCache[idx];
            if (offset <= currentOffset)
            {
                end = idx;
            }
            else
            {
                start = idx;
            }
            idx = (end + start) / 2;
        }

        float offsetBegin = _bakedDistCache[idx];
        float offsetEnd = _bakedDistCache[idx + 1];
        float interval = offsetEnd - offsetBegin;

        if (offset < offsetBegin || offset > offsetEnd)
            throw new ArgumentOutOfRangeException(nameof(offset), "Offset out of range");

        return new Interval
        {
            Index = idx,
            Fraction = (offset - offsetBegin) / interval
        };
    }

    private Vector2 _SampleBaked(Interval interval, bool cubic)
    {
        int idx = interval.Index;
        float frac = interval.Fraction;

        if (cubic)
        {
            Vector2 pre = idx > 0 ? _bakedPointCache[idx - 1] : _bakedPointCache[idx];
            Vector2 post = idx < _bakedPointCache.Length - 2 ? _bakedPointCache[idx + 2] : _bakedPointCache[idx + 1];
            return _bakedPointCache[idx].CubicInterpolate(_bakedPointCache[idx + 1], pre, post, frac);
        }
        else
        {
            return _bakedPointCache[idx].Lerp(_bakedPointCache[idx + 1], frac);
        }
    }

    private Transform2D _SamplePosture(Interval interval)
    {
        int idx = interval.Index;
        float frac = interval.Fraction;

        Vector2 forwardBegin = _bakedForwardVectorCache[idx];
        Vector2 forwardEnd = _bakedForwardVectorCache[idx + 1];

        Vector2 forward = forwardBegin.Slerp(forwardEnd, frac).normalized;
        Vector2 side = new Vector2(-forward.y, forward.x);

        return new Transform2D(forward, side, Vector2.zero);
    }

    public Vector2[] GetBakedPoints()
    {
        if (_bakedCacheDirty)
            _Bake();
        return _bakedPointCache;
    }

    public Vector2 GetClosestPoint(Vector2 toPoint)
    {
        if (_bakedCacheDirty)
            _Bake();

        if (_bakedPointCache.Length == 0)
            return Vector2.zero;
        if (_bakedPointCache.Length == 1)
            return _bakedPointCache[0];

        Vector2 nearest = Vector2.zero;
        float nearestDist = -1.0f;

        for (int i = 0; i < _bakedPointCache.Length - 1; i++)
        {
            float interval = _bakedDistCache[i + 1] - _bakedDistCache[i];
            Vector2 origin = _bakedPointCache[i];
            Vector2 direction = (_bakedPointCache[i + 1] - origin) / interval;

            float d = Mathf.Clamp((toPoint - origin).Dot(direction), 0.0f, interval);
            Vector2 proj = origin + direction * d;

            float dist = proj.DistanceSquaredTo(toPoint);

            if (nearestDist < 0.0f || dist < nearestDist)
            {
                nearest = proj;
                nearestDist = dist;
            }
        }

        return nearest;
    }

    public float GetClosestOffset(Vector2 toPoint)
    {
        if (_bakedCacheDirty)
            _Bake();

        if (_bakedPointCache.Length == 0)
            return 0.0f;
        if (_bakedPointCache.Length == 1)
            return 0.0f;

        float nearest = 0.0f;
        float nearestDist = -1.0f;
        float offset = 0.0f;

        for (int i = 0; i < _bakedPointCache.Length - 1; i++)
        {
            offset = _bakedDistCache[i];
            float interval = _bakedDistCache[i + 1] - _bakedDistCache[i];
            Vector2 origin = _bakedPointCache[i];
            Vector2 direction = (_bakedPointCache[i + 1] - origin) / interval;

            float d = Mathf.Clamp((toPoint - origin).Dot(direction), 0.0f, interval);
            Vector2 proj = origin + direction * d;

            float dist = proj.DistanceSquaredTo(toPoint);

            if (nearestDist < 0.0f || dist < nearestDist)
            {
                nearest = offset + d;
                nearestDist = dist;
            }
        }

        return nearest;
    }

    private Dictionary<float, Vector2>[] _TessellateEvenLength(int maxStages, float length)
    {
        var midpoints = new Dictionary<float, Vector2>[_points.Count - 1];
        for (int i = 0; i < _points.Count - 1; i++)
        {
            midpoints[i] = new Dictionary<float, Vector2>();
            _BakeSegment2dEvenLength(midpoints[i], 0, 1,
                _points[i].Position, _points[i].Out,
                _points[i + 1].Position, _points[i + 1].In,
                0, maxStages, length);
        }
        return midpoints;
    }

    private void _BakeSegment2dEvenLength(Dictionary<float, Vector2> bake, float begin, float end,
                                        Vector2 a, Vector2 outDir, Vector2 b, Vector2 inDir,
                                        int depth, int maxDepth, float length)
    {
        Vector2 beg = a.BezierInterpolate(a + outDir, b + inDir, b, begin);
        Vector2 endPt = a.BezierInterpolate(a + outDir, b + inDir, b, end);

        float segmentLength = beg.DistanceTo(endPt);

        if (segmentLength > length && depth < maxDepth)
        {
            float mp = (begin + end) * 0.5f;
            Vector2 mid = a.BezierInterpolate(a + outDir, b + inDir, b, mp);
            bake[mp] = mid;

            _BakeSegment2dEvenLength(bake, begin, mp, a, outDir, b, inDir, depth + 1, maxDepth, length);
            _BakeSegment2dEvenLength(bake, mp, end, a, outDir, b, inDir, depth + 1, maxDepth, length);
        }
    }

    public Vector2[] Tessellate(int maxStages = 5, float toleranceDegrees = 4)
    {
        if (_points.Count == 0)
            return Array.Empty<Vector2>();

        var midpoints = new Dictionary<float, Vector2>[_points.Count - 1];
        for (int i = 0; i < _points.Count - 1; i++)
        {
            midpoints[i] = new Dictionary<float, Vector2>();
            _BakeSegment2d(midpoints[i], 0, 1,
                _points[i].Position, _points[i].Out,
                _points[i + 1].Position, _points[i + 1].In,
                0, maxStages, toleranceDegrees);
        }

        int pc = 1;
        for (int i = 0; i < _points.Count - 1; i++)
        {
            pc++;
            pc += midpoints[i].Count;
        }

        var tess = new Vector2[pc];
        tess[0] = _points[0].Position;
        int pidx = 0;

        for (int i = 0; i < _points.Count - 1; i++)
        {
            foreach (var kv in midpoints[i])
            {
                pidx++;

                */