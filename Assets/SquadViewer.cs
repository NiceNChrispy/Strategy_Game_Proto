using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadViewer : MonoBehaviour {

    public Transform[] squadViewerPos;
    public float _angle = 360 / 7;

	// Use this for initialization
	void Start () {
        Setup();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Setup()
    {
        //for(int i = 0; i < squadViewerPos.Length; i++)
        //{
        //    squadViewerPos[i].position += new Vector3(0, 0, 5);
        //    transform.rotation = new Quaternion(0, _angle, 0, 0);
        //}
    }

    [NaughtyAttributes.Button("Next")]
    public void CharacterSelect(float value)
    {
        iTween.MoveBy(gameObject, iTween.Hash("x", value, "easeType", "easeInOutExpo", "delay", .1));
    }
}
