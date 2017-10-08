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

    public ClassSo classType;
    public RaceSO raceType;

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
        _health += classType._health + raceType._health + slot1._health + slot2._health;
        _damage += classType._Damage + raceType._Damage + slot1._health + slot2._health;
        _critChance += classType._CritChance + raceType._CritChance + slot1._CritChance + slot2._CritChance;
        _movement += classType._Movement + raceType._Movement + slot1._Movement + slot2._Movement;
        _armour += classType._Armour + raceType._Armour + slot1._Armour + slot2._Armour;
        _sight += classType._Sight + raceType._Sight + slot1._Sight + slot2._Sight;
        _shield += classType._Shield + raceType._Shield + slot1._Shield + slot2._Shield;
        _resistMelee += classType._ResistMelee + raceType._ResistMelee + slot1._ResistMelee + slot2._ResistMelee;
        _resistRanged += classType._ResistRanged + raceType._ResistRanged + slot1._ResistRanged + slot2._ResistRanged;
        _resistMagic += classType._ResistMagic + raceType._ResistMagic + slot1._ResistMagic + slot2._ResistMagic;
        unitName = raceType._unitRace.ToString() + " " + classType._unitClass.ToString();
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
