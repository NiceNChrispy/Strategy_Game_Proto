using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour {

    public bool unitSelected;
    public string unitName;
    public GameObject unitPrefab;
    public SlotItem slot1;
    public SlotItem slot2;

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

    // Use this for initialization
    void Awake ()
    {
        _health += slot1._health + slot2._health;
        _damage += slot1._health + slot2._health;
        _critChance += slot1._CritChance + slot2._CritChance;
        _movement += slot1._Movement + slot2._Movement;
        _armour += slot1._Armour + slot2._Armour;
        _sight += slot1._Sight + slot2._Sight;
        _shield += slot1._Shield + slot2._Shield;
        _resistMelee += slot1._ResistMelee + slot2._ResistMelee;
        _resistRanged += slot1._ResistRanged + slot2._ResistRanged;
        _resistMagic += slot1._ResistMagic + slot2._ResistMagic;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UnitSelected()
    {
        unitSelected = true;
    }

    public void UnitDeselect()
    {
        unitSelected = false;
    }
}
