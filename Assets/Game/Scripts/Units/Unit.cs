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
    public string unitName;
    public int _health; // Health of the unit
    public int _damage; // Attack of the unit
    public float _critChance; //Chance of performing a critical hit
    public int _movement; //How far can the unit move
    public int _armour; //Helps defend against melee
    public int _sight; //How far can the Unit see
    public int _shield; //Helps defend against magic
    public int _resistMelee; //Resistance against melee attacks
    public int _resistRanged; //Resistance against ranged attacks
    public int _resistMagic; //Resistance against magic attacks
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
