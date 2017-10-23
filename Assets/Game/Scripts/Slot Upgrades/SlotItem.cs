using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "SlotItem", menuName = "LeetStratsScriptables/SlotItem")]
public class SlotItem : ScriptableObject {

    public Image slotIcon;
    public string buffName;
    public int _cost;

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
}
