using System;
using System.Collections.Generic;

namespace MRCustom.Collections
{
    // Explanation to argue this types consistent existance and use can be found here: https://youtu.be/WwkuAqObplU?t=2595
    // Basically, deleting elements from a list is expensive due to re-orienting, so this functions when that is not needed to be less so by... not re-orienting.

    /// <summary>
    /// A form of lists that does not preserve position on removal of elements.
    /// On removal of an element, the last elements in the count will override the freed space, before reducing the count to the new size.
    /// More performant than a list for when the positioning of a stored element does not matter, often when there is some other identifier for it, or when it is used purely for iterating.
    /// </summary>
    public class SwapbackArray<T>
    {
        private List<T> _list;

        /// <summary>
        /// Gets the number of elements contained in the array.
        /// </summary>
        public Int32 Count
        {
            get { return _list.Count; }
        }

        /// <summary>
        /// Construct an empty array.
        /// </summary>
        public SwapbackArray()
        {
            _list = new List<T>();
        }

        /// <summary>
        /// Construct an array with the listed elements.
        /// </summary>
        /// <param name="elements"></param>
        public SwapbackArray(params T[] elements)
        {
            _list = new List<T>(elements);
        }

        public T this[Int32 index]
        {
            get
            {
                // Check if the index is valid
                if (index < 0 || index >= _list.Count)
                {
                    throw new IndexOutOfRangeException("Index is out of bounds.");
                }
                return _list[index];
            }
            set
            {
                // Check if the index is valid
                if (index < 0 || index >= _list.Count)
                {
                    throw new IndexOutOfRangeException("Index is out of bounds.");
                }
                _list[index] = value;
            }
        }

        /// <summary>
        /// Adds an object to the end of the array.
        /// </summary>
        /// <param name="item"></param>
        public void Add(T item)
        {
            _list.Add(item);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the array.
        /// </summary>
        public void Remove(T item)
        {
            var found = _list.FindIndex(i => i.Equals(item));
            if (found != -1)
                this.RemoveAt(found);
        }

        /// <summary>
        /// Removes the element at the specified index of the Swapback Array.
        /// This will also swap the value of that index to be the furthest back element of the array.
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(Int32 index)
        {
            var back = _list.Count - 1;
            _list[index] = _list[back];
            _list.RemoveAt(back);
        }

        /// <summary>
        /// Removes all the elements that match the conditions defined by the specified predicate.
        /// </summary>
        public void RemoveAll(Predicate<T> match)
        {
            for (int i = _list.Count - 1; i >= 0; i--)
            {
                if (match(_list[i]))
                {
                    RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Removes a range of elements from the SwapbackArray.
        /// </summary>
        /// <param name="start">The zero-based starting index of the range to remove.</param>
        /// <param name="count">The number of elements to remove.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when start or count is negative, or when start and count exceed the list's bounds.</exception>
        public void RemoveRange(int start, int count)
        {
            if (start < 0)
                throw new ArgumentOutOfRangeException(nameof(start), "Start index cannot be negative.");
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), "Count cannot be negative.");
            if (start + count > _list.Count)
                throw new ArgumentException("Start index and count exceed the list's bounds.");

            if (count == 0)
                return;

            int totalCount = _list.Count;
            int end = start + count;

            // Directly remove if the range is at the end
            if (end == totalCount)
            {
                _list.RemoveRange(start, count);
                return;
            }

            int swapStart = totalCount - count;

            // Swap elements in the target range with the last 'count' elements
            for (int i = 0; i < count; i++)
            {
                int sourceIndex = start + i;
                int targetIndex = swapStart + i;

                T temp = _list[sourceIndex];
                _list[sourceIndex] = _list[targetIndex];
                _list[targetIndex] = temp;
            }

            // Remove the last 'count' elements
            _list.RemoveRange(swapStart, count);
        }
    }
}