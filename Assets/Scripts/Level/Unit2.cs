using Navigation;
using UnityEngine;

public class Unit2 : MonoBehaviour
{
    [SerializeField] private Navigation.NavAgent m_NavAgent;

    public NavAgent Agent
    {
        get
        {
            return m_NavAgent;
        }
    }

    public void Select()
    {
        Game_Manager.Instance.SelectUnit(this);
    }

    public void MoveTo(NavNode targetNavNode)
    {
        StartCoroutine(m_NavAgent.PathTo(targetNavNode));
    }
}