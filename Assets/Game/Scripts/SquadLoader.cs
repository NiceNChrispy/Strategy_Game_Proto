using UnityEngine;
using System.IO;
using System;

public class SquadLoader : MonoBehaviour {

    public Unit[] unitPrefabs;

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
            Load();
        }
	}

    [NaughtyAttributes.Button("LOAD SQUAD")]
    public void Load()
    {
        builder.ClearSquad();
        StreamReader streamReader;
        //sr = new StreamReader(path + "/" + fileName + ".txt");
        streamReader = new StreamReader(saver.file);

        string fileData = streamReader.ReadToEnd();
        string[] squadData = fileData.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

        for (int i = 0; i < squadData.Length; i++)
        {
            if(squadData[i] != string.Empty)
            {
                UnitData unitData = JsonUtility.FromJson<UnitData>(squadData[i]);
                Unit temp = Instantiate(unitPrefabs[(int)unitData._class]);
                builder.squadList.Add(temp);
                Debug.Log(string.Format("LOADED {0} SUCCESSFULLY", unitData._class.ToString()));
            }
        }
        streamReader.Close();

        Debug.Log((string.Format("{0} UNITS LOADED INTO SQUAD", builder.squadList.Count)));
    }
}
