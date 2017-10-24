using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

public class SquadSaver : MonoBehaviour {

    public string squadData;
    public string fileName;
    public string _path;
    public string file;
    public bool overwrite;

    public SquadBuilder builder;

    private string newLine = "\r\n";

    private void Awake()
    {
        builder = GetComponentInChildren<SquadBuilder>();
        _path = Application.persistentDataPath + "/" + "Squads";
    }

    private void Update()
    {
        file = _path + "/" + fileName + ".txt";
        if (Input.GetKeyDown(KeyCode.S))
        {
            CreateSquad();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ReadSquadAndSave();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            DeleteFile();
        }
    }

    public void CreateSquad()
    {
         CheckDirectory();
        if (CheckFile())
        {
            //ReadSquadAndSave();
            File.WriteAllText(file, squadData);
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

     public void ReadSquadAndSave()
    {
        foreach (GameObject unit in builder.squadList)
        {
            squadData += unit.GetComponent<Unit>().unitName + newLine;
        }
    }
}
