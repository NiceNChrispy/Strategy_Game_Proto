using System.Collections.ObjectModel;

namespace DataStructures
{
    public class NodeList<T> : Collection<Node<T>>
    {
        public NodeList() : base() { }

        public NodeList(int size)
        {
            // Add the specified number of items
            for (int i = 0; i < size; i++)
            {
                base.Items.Add(default(Node<T>));
            }
        }

        public Node<T> FindByValue(T value)
        {
            foreach (Node<T> node in Items)
            {
                if (node.Data.Equals(value))
                {
                    return node;
                }
            }
            return null;
        }
    }
}