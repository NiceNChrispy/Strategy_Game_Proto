using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum UnitClass {Ranger, Knight, Rogue, Wizard, Cleric }

[System.Serializable]
public class UnitData
{
    public UnitClass _class;

    [Header("Stats")]
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
    public bool unitSelected;
    public string unitName;

    public GameObject unitPrefab;
    //public SlotItem slot1;
    //public SlotItem slot2;

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
        m_UnitData = new UnitData() { _class = (UnitClass)Random.Range(0,5), _armour = 10, _critChance = 0.2f, _health = 100, _movement = 2 };
        //_health += slot1._health + slot2._health;
        //_damage += slot1._health + slot2._health;
        //_critChance += slot1._CritChance + slot2._CritChance;
        //_movement += slot1._Movement + slot2._Movement;
        //_armour += slot1._Armour + slot2._Armour;
        //_sight += slot1._Sight + slot2._Sight;
        //_shield += slot1._Shield + slot2._Shield;
        //_resistMelee += slot1._ResistMelee + slot2._ResistMelee;
        //_resistRanged += slot1._ResistRanged + slot2._ResistRanged;
        //_resistMagic += slot1._ResistMagic + slot2._ResistMagic;
    }
	
	void Update ()
    {
        //unitName = _unitClass + "," + slot1.name + "," + slot2.name;
        unitName = UnitData._class.ToString();
    }
}
