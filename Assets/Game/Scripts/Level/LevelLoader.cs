using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour {

    public GameObject levelCube;
    public Transform levelHolder;
    public int width;
    public int depth;

	// Use this for initialization
	void Start () {
        StartCoroutine(GenerateLevel(width, depth));
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(GenerateLevel(width, depth));
        }
	}

    public IEnumerator GenerateLevel(int _width, int _depth)
    {
        for (int x = 0; x < _width; x++)
        {
            yield return new WaitForSeconds(0.005f);

            for (int z = 0; z < _depth; z++)
            {
                yield return new WaitForSeconds(0.005f);
                GameObject levelObj = Instantiate(levelCube, new Vector3(x, 0, z), Quaternion.identity);
                levelObj.transform.parent = levelHolder;
            }
        //yield return new WaitForSeconds(0.1f);
        }
    }
}
