using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum UnitClass {Ranger, Knight, Rogue, Wizard, Druid }

[System.Serializable]
public class UnitData
{
    public UnitClass _class;

    [Header("Stats")]
    public Stats _stats;

    //public int prefabNum;
    public int startX;
    public int startY;
}

[System.Serializable]
public class Unit : MonoBehaviour
{

    public GameObject unitPrefab;
    //public SlotItem slot1;
    //public SlotItem slot2;
    [SerializeField]

    private UnitData m_UnitData;
    public UnitData UnitData
    {
        get
        {
            return m_UnitData;
        }

        set
        {
            m_UnitData = value;
        }
    }

    void Awake()
    {
    }
	
	void Update ()
    {
        
    }
}
