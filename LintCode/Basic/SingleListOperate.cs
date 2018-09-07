using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LintCode.Basic
{
    public class SingleListOperate<T> : ISingleListOperate<T> where T : IComparable<T>
    {
        int size = 0;
        SingleDirectionNode<T> head;
        SingleDirectionNode<T> tail;
        public SingleDirectionNode<T> this[int index]
        {
            get
            {
                if (IsEmpty)
                {
                    throw new Exception("链表是空的");
                }
                if (index < 0)
                {
                    throw new OutOfMemoryException();
                }
                if (index >= Count)
                {
                    throw new OutOfMemoryException();
                }
                if (index == 0)
                    return head;
                if (index == Count - 1)
                    return tail;
                int i = 1;
                SingleDirectionNode<T> node = head.Next;

                while (true)
                {
                    if (i == index)
                    {
                        return node;
                    }
                    i++;
                    node = node.Next;

                }
            }
        }

        public int Count
        {
            get
            {
                return size;
            }
        }

        public SingleDirectionNode<T> Head
        {
            get
            {
                return head;
            }

            set
            {
                head = value;
            }
        }

        public bool IsEmpty
        {
            get
            {
                return head == null;
            }
        }

        public SingleDirectionNode<T> Tail
        {
            get
            {
                if (tail == null)
                    tail = this[size - 1];
                return tail;
            }

            set
            {
                tail = value;
            }
        }

        public void AddFirst(T t)
        {
            SingleDirectionNode<T> node = new Basic.SingleDirectionNode<T>(t);
            node.Next = head;
            head = node;
            size += 1;
        }

        public void AddLast(T t)
        {
            SingleDirectionNode<T> node = new Basic.SingleDirectionNode<T>(t);
            if (tail == null)
                tail = this[size - 1];
            tail.Next = node;
            size += 1;
        }

        public void Clear()
        {
            head = null;
            size = 0;
        }

        public void Insert(int index, T t)
        {
            if (IsEmpty && index > 0)
            {
                throw new IndexOutOfRangeException();
            }
            if (index >= Count)
            {
                throw new IndexOutOfRangeException();
            }
            if (index < 0)
            {
                throw new IndexOutOfRangeException();
            }
            if (index == 0)
            {
                AddFirst(t);
                return;
            }


            SingleDirectionNode<T> node = new Basic.SingleDirectionNode<T>(t);
            SingleDirectionNode<T> preNode = this[index];
            SingleDirectionNode<T> nextNode = this[index + 1];
            node.Next = nextNode;
            preNode.Next = node;
            size += 1;
        }

        public void RemoveAt(int index)
        {
            if (IsEmpty && index > 0)
            {
                throw new IndexOutOfRangeException();
            }
            if (index > Count)
            {
                throw new IndexOutOfRangeException();
            }
            if (index < 0)
            {
                throw new IndexOutOfRangeException();
            }
            if (index == Count)
            {
                RemoveLast();
                return;
            }
            if (index == 0)
            {
                RemoveFirst();
                return;
            }
            SingleDirectionNode<T> preNode = this[index - 1];
            SingleDirectionNode<T> nextNode = this[index + 1];
            preNode.Next = nextNode;
            size += 1;
        }

        public void RemoveFirst()
        {
            SingleDirectionNode<T> nextNode = head.Next.Next;
            head.Next = nextNode;
            size -= 1;

        }

        public void RemoveLast()
        {
            SingleDirectionNode<T> preNode = this[size - 1 - 1];
            preNode.Next = null;
            tail = preNode;
            size -= 1;
        }
    }
}
