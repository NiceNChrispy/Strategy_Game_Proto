namespace DataStructures
{
    public class Node<T>
    {
        public T Data { get; set; }

        public NodeList<T> Neighbors { get; protected set; }

        public Node(T data)
        {
            Data = data;
            Neighbors = new NodeList<T>();
        }
    }
}