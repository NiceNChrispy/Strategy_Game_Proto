using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadBuilderConstructor : MonoBehaviour {

    public GameObject plynthPrefab;
    public Transform spawnPoint;
    public float rotateVal;
    GameObject obj;

    // Use this for initialization
    void Start () {
        rotateVal = 360 / 7;
        StartCoroutine(PlynthsOpen());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public IEnumerator PlynthsOpen()
    {
        for (int i = 0; i < 7; i++)
        {
            float rotate = rotateVal * i;
            obj = Instantiate(plynthPrefab, new Vector3(spawnPoint.position.x, 5, spawnPoint.position.z), Quaternion.identity);
            spawnPoint.eulerAngles = new Vector3(obj.transform.rotation.x, rotate, obj.transform.rotation.z);
            
            obj.transform.localPosition += new Vector3(0,0,-1.5f);
            obj.transform.parent = spawnPoint;
            GetComponent<SquadBuilder>().plynthList.Add(obj);
            yield return new WaitForSeconds(0.5f);
        }

        spawnPoint.eulerAngles = new Vector3(obj.transform.rotation.x, rotateVal, obj.transform.rotation.z);
        yield return null;
    }
}
