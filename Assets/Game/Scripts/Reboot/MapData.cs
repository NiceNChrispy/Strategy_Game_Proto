using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Reboot
{
    [System.Serializable]
    public class MapData : ICollection<Hex>
    {
        public List<Hex> Hexes;

        public MapData(HashSet<Hex> hashSet)
        {
            Hexes = new List<Hex>(hashSet);
        }

        public int Count { get { return ((ICollection<Hex>)Hexes).Count; } }

        public bool IsReadOnly { get { return ((ICollection<Hex>)Hexes).IsReadOnly; } }

        public void Add(Hex item)
        {
            ((ICollection<Hex>)Hexes).Add(item);
        }

        public void Clear()
        {
            ((ICollection<Hex>)Hexes).Clear();
        }

        public bool Contains(Hex item)
        {
            return ((ICollection<Hex>)Hexes).Contains(item);
        }

        public void CopyTo(Hex[] array, int arrayIndex)
        {
            ((ICollection<Hex>)Hexes).CopyTo(array, arrayIndex);
        }

        public IEnumerator<Hex> GetEnumerator()
        {
            return ((ICollection<Hex>)Hexes).GetEnumerator();
        }

        public bool Remove(Hex item)
        {
            return ((ICollection<Hex>)Hexes).Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((ICollection<Hex>)Hexes).GetEnumerator();
        }

        public static bool Load(string path, out MapData loadedMap)
        {
            if (File.Exists(path))
            {
                StreamReader sr = new StreamReader(path);
                loadedMap = JsonUtility.FromJson<MapData>(sr.ReadToEnd());
                sr.Close();
                return true;
            }
            loadedMap = null;
            return false;
        }

        public static bool Save(string path, MapData saveMapData)
        {
            if (File.Exists(path))
            {
                StreamWriter sw = new StreamWriter(path, false);
                string data = JsonUtility.ToJson(saveMapData, true);
                sw.Write(data);
                sw.Close();
                return true;
            }
            return false;
        }
    }
}