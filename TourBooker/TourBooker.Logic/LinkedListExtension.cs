//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace TourBooker.Logic
//{
//	public static class LinkedListExtension
//	{
//		public static LinkedListNode<T> GetNthNode<T>(this LinkedList<T> lst, int n)
//		{
//			LinkedListNode<T> currentNode = lst.First;
//			for (int i = 0; i < n; i++)
//				currentNode = currentNode.Next;
//			return currentNode;
//		}
//	}
//}


using System.Collections.Generic;

namespace TourBooker.Logic
{
    public static class LinkedListExtension
    {
        public static LinkedListNode<T> GetNthNode<T>(this ConcurrentLinkedList<T> list, int n)
        {
            return list.GetNthNodeSafe(n);
        }
    }
}
