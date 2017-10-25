using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

public class SquadSaver : MonoBehaviour {

    public string fileName;
    public string _path;
    public string file;
    public bool overwrite;

    public SquadBuilder builder;

    private void Awake()
    {
        _path = Application.persistentDataPath + "/" + "Squads";
        file = _path + "/" + fileName + ".txt";
        CheckDirectory();
        CheckFile();
    }

    private void Update()
    {
        file = _path + "/" + fileName + ".txt";

        if (Input.GetKeyDown(KeyCode.S))
        {
            ReadSquadAndSave();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            DeleteFile();
        }
    }


    void CheckDirectory()
    {
        if (!Directory.Exists(_path))
        {
            //Debug.Log("Creating Path" + _path);
            Directory.CreateDirectory(_path);
        }
        else
        {
            //Debug.Log("Path Exists" + _path);
        }
    }

    public void DeleteFile()
    {
        if (CheckFile())
        {
            File.Delete(file);
        }
    }

    bool CheckFile()
    {
        if (!File.Exists(file))
        {
            Debug.Log("Creating file" + file);
            File.Create(file);
            return false;
        }
        else
        {
            Debug.Log("File Exists" + file);
            return true;
        }
    }

    [NaughtyAttributes.Button("SAVE SQUAD")]
    public void ReadSquadAndSave()
    {
        //Debug.Log("Knobs");
        StreamWriter sw;
        sw = new StreamWriter(file, true);
        if (builder.squadList != null)
        {
            foreach (Unit unit in builder.squadList)
            {
                //print("Bitches");
                sw.WriteLine(JsonUtility.ToJson(unit));
                //print(JsonUtility.ToJson(unit));
            }
        }
        sw.Close();
    }
}
