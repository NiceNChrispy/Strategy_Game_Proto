using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SquadViewer : MonoBehaviour {

    public Transform[] squadViewerPos;
    public float _angle = 360 / 7;
    public int activeUnit;

    public Button[] navButtons;

	// Use this for initialization
	void Start () {
        Setup();
    }
	
	// Update is called once per frame
	void Update () {
        if (iTween.Count() > 0)
        {
            navButtons[0].interactable = false;
            navButtons[1].interactable = false;
        }
        else
        {
            navButtons[0].interactable = true;
            navButtons[1].interactable = true;
        }
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
