using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class SquadLoader : MonoBehaviour {

    public GameObject[] characterPrefabs;

    //public List<string> squad;

    public string path;

    public SquadSaver saver;
    public SquadBuilder builder;

    // Use this for initialization
    void Start () {
        path = saver._path;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.U))
        {
            LoadSquad();
        }
	}

    [NaughtyAttributes.Button("LOAD SQUAD")]
    public void LoadSquad()
    {
        builder.ClearSquad();
        StreamReader sr;
        //sr = new StreamReader(path + "/" + fileName + ".txt");
        sr = new StreamReader(saver.file);
        for (int i = 0; i < 7; i++)
        {
            Unit temp = JsonUtility.FromJson<Unit>(sr.ReadLine());
            builder.squadList.Add(temp);
        }
    }
}
