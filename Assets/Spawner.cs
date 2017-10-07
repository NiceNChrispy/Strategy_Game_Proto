using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour {

    public Dropdown raceDropdown;
    public SquadBuilder _builder;

    public GameObject[] humanPrefabs;
    public GameObject[] orcPrefabs;
    public GameObject[] elvenPrefabs;
    public GameObject[] undeadPrefabs;
    public GameObject[] dwarfPrefabs;

    public GameObject squadPlynths;

    public int maxPlayers;
    public int currentPlynth;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SpawnCharacter(int classType)
    {
        switch (raceDropdown.value)
        {
            case 0:
                //Instantiate(humanPrefabs[classType], Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
                _builder.squadList.Add(humanPrefabs[classType]);
                break;

            case 1:
                //Instantiate(orcPrefabs[classType], Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
                _builder.squadList.Add(orcPrefabs[classType]);
                break;

            case 2:
                //Instantiate(elvenPrefabs[classType], Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
                _builder.squadList.Add(elvenPrefabs[classType]);
                break;

            case 3:
                //Instantiate(dwarfPrefabs[classType], Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
                _builder.squadList.Add(dwarfPrefabs[classType]);
                break;

            case 4:
                //Instantiate(undeadPrefabs[classType], Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
                _builder.squadList.Add(undeadPrefabs[classType]);
                break;

            default:
                break;
        }
    }

    public void RotatePlynths(float val)
    {
        iTween.RotateBy(squadPlynths, iTween.Hash("y", val, "easeType", "easeInOutQuad", "delay", .1));
        if (val < 0)
        {
            currentPlynth--;
            
        }
        else
        {
            currentPlynth++;
            
        }
    }
}
