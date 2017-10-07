using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelCube : MonoBehaviour {

    public bool selected;
    public enum Status {Normal, Burning, Poisoned, Water}
    public Status status;

    public Image _higlighter;
    public Image _blob;

    public GameObject _fog;

    // Use this for initialization
    void Start () {
        _higlighter.enabled = false;
        _blob.enabled = false;
        StartCoroutine(GoToStart());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public IEnumerator GoToStart()
    {
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
            }
        }
    }

    public void TileSelected()
    {
        _higlighter.enabled = true;
    }

    public void TileDeselect()
    {
        _higlighter.enabled = false;
    }
}
