using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadViewer : MonoBehaviour {

    public Transform[] squadViewerPos;
    public float _angle = 360 / 7;
    public int activeUnit;

    [ReadOnly]
    public int squad; 

	// Use this for initialization
	void Start () {
        Setup();
        squad = squadViewerPos.Length;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Setup()
    {
        activeUnit = 0;

    }

    [NaughtyAttributes.Button("Next")]
    public void CharacterSelect(float value)
    {

        if (value < 0)
        {
            if (activeUnit == squadViewerPos.Length)
            {
                iTween.MoveBy(gameObject, iTween.Hash("x", 78, "easeType", "easeInOutExpo"));
                activeUnit = 0;
            }
            else
            {
                iTween.MoveBy(gameObject, iTween.Hash("x", value, "easeType", "easeInOutExpo"));
                activeUnit++;
            }
        }
        else
        {
            if (activeUnit == 0)
            {
                iTween.MoveBy(gameObject, iTween.Hash("x", -78, "easeType", "easeInOutExpo"));
                activeUnit = 7;
            }
            else
            {
                iTween.MoveBy(gameObject, iTween.Hash("x", value, "easeType", "easeInOutExpo"));
                activeUnit--;
            }

        }
        
    }
}
