using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LintCode.Basic
{
    /// <summary>
    /// 单向链表的操作接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISingleListOperate<T> where T : IComparable<T>
    {
        void Insert(int index, T t);
        void RemoveAt(int index);
        void AddFirst(T t);
        void AddLast(T t);
        void RemoveFirst();
        void RemoveLast();
        void Clear();
        int Count { get; }
        SingleDirectionNode<T> this[int index] { get; }
        bool IsEmpty { get; }
        SingleDirectionNode<T> Head { get; set; }
        SingleDirectionNode<T> Tail { get; set; }
    }
}
