using System.Collections.Generic;

namespace Reboot
{
    [System.Serializable]
    public class Map<T>
    {
        public List<T> Contents;

        public Map()
        {
            Contents = new List<T>();
        }

        public void Add(T item)
        {
            if(!Contents.Contains(item))
            {
                Contents.Add(item);
            }
        }

        public void Remove(T item)
        {
            if (Contents.Contains(item))
            {
                Contents.Remove(item);
            }
        }

        public bool Contains(T item)
        {
            return Contents.Contains(item);
        }
    }
}