using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TourBooker.Logic
{
    public class ConcurrentLinkedList<T> : IEnumerable<T>
    {
        private readonly LinkedList<T> _list = new LinkedList<T>();
        private readonly object _lock = new object();

        public void AddLast(T item)
        {
            lock (_lock)
            {
                _list.AddLast(item);
            }
        }

        public void AddBefore(LinkedListNode<T> node, T item)
        {
            lock (_lock)
            {
                if (node != null)
                    _list.AddBefore(node, item);
            }
        }

        public void Remove(LinkedListNode<T> node)
        {
            lock (_lock)
            {
                if (node != null)
                    _list.Remove(node);
            }
        }

        public void Clear()
        {
            lock (_lock)
            {
                _list.Clear();
            }
        }

        public T[] ToArray()
        {
            lock (_lock)
            {
                return _list.ToArray();
            }
        }

        public List<T> ToListSafe()
        {
            lock (_lock)
            {
                return _list.ToList();   
            }
        }

        public LinkedListNode<T> GetNthNodeSafe(int n)
        {
            lock (_lock)
            {
                var current = _list.First;

                for (int i = 0; i < n; i++)
                {
                    if (current == null)
                        return null;
                    current = current.Next;
                }
                return current;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            lock (_lock)
            {
                return _list.ToList().GetEnumerator(); 
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
