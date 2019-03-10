using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Assets/Reboor/Level", order = 1)]
public class LevelData : ScriptableObject, ICollection<TileData>
{
    public List<TileData> TileData;

    public int Count { get { return ((ICollection<TileData>)TileData).Count; } }

    public bool IsReadOnly { get { return ((ICollection<TileData>)TileData).IsReadOnly; } }

    public void Add(TileData item)
    {
        ((ICollection<TileData>)TileData).Add(item);
    }

    public void Clear()
    {
        ((ICollection<TileData>)TileData).Clear();
    }

    public bool Contains(TileData item)
    {
        return ((ICollection<TileData>)TileData).Contains(item);
    }

    public void CopyTo(TileData[] array, int arrayIndex)
    {
        ((ICollection<TileData>)TileData).CopyTo(array, arrayIndex);
    }

    public IEnumerator<TileData> GetEnumerator()
    {
        return ((ICollection<TileData>)TileData).GetEnumerator();
    }

    public bool Remove(TileData item)
    {
        return ((ICollection<TileData>)TileData).Remove(item);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((ICollection<TileData>)TileData).GetEnumerator();
    }

    public void DrawGizmos()
    {
        foreach (TileData tileData in TileData)
        {
            Gizmos.DrawSphere(tileData.WorldPosition, 0.2f);
        }
    }
}