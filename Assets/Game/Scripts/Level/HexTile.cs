using UnityEngine;
using UnityEngine.UI;

public class HexTile : MonoBehaviour
{
    [SerializeField] private int x, y;
    [SerializeField] private TextMesh m_Cost;

    public int X
    {
        get
        {
            return x;
        }

        set
        {
            x = value;
        }
    }
    public int Y
    {
        get
        {
            return y;
        }

        set
        {
            y = value;
        }
    }

    public void SetPosition(int x, int y)
    {
        this.x = x;
        this.y = y;

        name = string.Format("{0},{1}", x, y);
    }

    public void Select()
    {
        Game_Manager.Instance.UpdateTileUnderCursor(this);
    }

    public void Interact()
    {
        Game_Manager.Instance.TileAction(this);
    }

    public void Deselect()
    {
        Game_Manager.Instance.ClearTile();
    }

    private void Update()
    {
        //m_Cost.text = Game_Manager.Instance.GetCost(X, Y);
    }
}