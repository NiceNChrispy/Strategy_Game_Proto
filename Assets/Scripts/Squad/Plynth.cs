using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plynth : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(GoToStart());

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public IEnumerator GoToStart()
    {
        //iTween.MoveBy(gameObject, iTween.Hash("y", -5, "easeType", "easeInOutQuad"));
        int step = 0;
        while (step != -1)
        {
            if (transform.position.y != 0)
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, 0f, transform.position.z), 20f * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            else
            {
                step = -1;
                StopCoroutine(GoToStart());
            }
        }
        //yield return null;
    }
}
