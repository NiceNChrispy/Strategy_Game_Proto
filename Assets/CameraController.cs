using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public Camera playerCam;
    public float rotateBy;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            StartCoroutine(RotateCam());
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            StartCoroutine(RotateCam());
        }
    }

    public IEnumerator RotateCam()
    {
        iTween.RotateBy(gameObject, iTween.Hash("y", .25, "easeType", "easeInOutBack" , "delay", .4));
        yield return null;
    }
}
