namespace 辅助代码.Generic
{
    interface IPair<T>
    {
        T First { get; set; }
        T Second { get; set; }
    }

    public struct Pair<T> : IPair<T>
    {
        public T First { get; set; }
        public T Second { get; set; }

        public Pair(T frist, T second)
        {
            First = frist;
            Second = second;
        }
    }
}

namespace 辅助代码.Class
{
    public class PdaItem
    {
        public string Name { get; set; }
        public PdaItem(string name)
        {
            Name = name;
        }
    }

    class Contact : PdaItem
    {
        public int Phone { get; set; }
        public Contact(string name, int phone) : base(name)
        {
            Phone = phone;
        }
    }
}