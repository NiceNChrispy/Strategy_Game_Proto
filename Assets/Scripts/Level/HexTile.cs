using UnityEngine;

public class HexTile : MonoBehaviour
{
    [SerializeField] private int x, y;

    public void SetPosition(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}