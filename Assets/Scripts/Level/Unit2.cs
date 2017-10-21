using UnityEngine;

public class Unit2 : MonoBehaviour
{
    [SerializeField] private Navigation.NavAgent m_NavAgent;

    public NavNode ActiveNode
    {
        get { return m_NavAgent.ActiveNode; }
    }

    public bool HasPath
    {
        get { return m_NavAgent.HasPath; }
    }

    public void Select()
    {
        Game_Manager.Instance.SelectUnit(this);
    }

    public void MoveTo(NavNode targetNavNode)
    {
        print("MOVING");
        StartCoroutine(m_NavAgent.PathTo(targetNavNode));
    }
}