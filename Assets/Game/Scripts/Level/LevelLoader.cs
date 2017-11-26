using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class LevelLoader : MonoBehaviour {

    public HexSpawner spawner;

    public string[] mapData;
    public string fileName;
    public string _path;
    public string file;

    public void Awake()
    {
        _path = Application.persistentDataPath + "/" + "Maps";
        file = _path + "/" + fileName + ".txt";
        ChooseMap();
    }

    public void ChooseMap()
    {
        StreamReader streamReader;
        streamReader = new StreamReader(file);

        string fileData = streamReader.ReadToEnd();
        mapData = fileData.Split(',');
    }
}
