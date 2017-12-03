using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Unit Stats", menuName = "Unit Stats/ Create New Stats")]
public class Stats : ScriptableObject {

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
