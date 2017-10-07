using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "RaceSo", menuName = "LeetStratsScriptables/RaceSo")]
public class RaceSO : ScriptableObject {

    public Image slotIcon;
    public enum UnitRace { Human, Orc, Elven, Undead, Dwarf };
    public UnitRace _unitRace;
    public string racePassiveName;

    [Header("Stats")]
    public int _health; // Health of the unit
    public int _Damage; // Attack of the unit
    public float _CritChance; //Chance of performing a critical hit
    public int _Movement; //How far can the unit move
    public int _Armour; //Helps defend against melee
    public int _Sight; //How far can the Unit see
    public int _Shield; //Helps defend against magic
    public int _ResistMelee; //Resistance against melee attacks
    public int _ResistRanged; //Resistance against ranged attacks
    public int _ResistMagic; //Resistance against magic attacks

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
