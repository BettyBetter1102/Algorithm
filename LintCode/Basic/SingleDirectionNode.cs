using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LintCode.Basic
{
    /// <summary>
    /// 单向链表的结点
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SingleDirectionNode<T> where T : IComparable<T>
    {
        public T Value { get; set; }

        public SingleDirectionNode<T> Next { get; set; }

        public SingleDirectionNode()
        {
            Value = default(T);
            Next = null;
        }

        public SingleDirectionNode(T value) : this()
        {
            Value = value;
        }

        public SingleDirectionNode(T value, SingleDirectionNode<T> next) : this(value)
        {
            Next = next;
        }

        public override string ToString()
        {
            T nextValue = Next == null ? default(T) : Next.Value;

            string str = string.Format("当前结点值:{0};后一节点值:{2}", Value, nextValue);

            return str;
        }
    }
}
