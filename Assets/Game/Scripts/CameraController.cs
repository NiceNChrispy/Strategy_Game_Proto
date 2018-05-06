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
            //if (iTween.Count() == 0)
            {
                StartCoroutine(RotateCam(0.25f));
            }
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            //if (iTween.Count() == 0)
            {
                StartCoroutine(RotateCam(-0.25f));
            }
        }
    }

    public IEnumerator RotateCam(float val)
    {
        //iTween.RotateBy(gameObject, iTween.Hash("y", val, "easeType", "easeInOutQuad" , "delay", .1));
        yield return null;
    }
}
