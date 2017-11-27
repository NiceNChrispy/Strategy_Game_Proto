using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class LevelLoader : MonoBehaviour {

    public HexSpawner spawner;

    public enum LoadType {file, asset}
    public LoadType _loadType;

    //From file
    public string[] mapData;
    public string fileName;
    public string _path;
    public string file;

    public Level _levelAsset;

    public void Awake()
    {
        _path = Application.persistentDataPath + "/" + "Maps";
        file = _path + "/" + fileName + ".txt";
    }

    public void ChooseMap()
    {

        if (_loadType == LoadType.file)
        {
            StreamReader streamReader;
            streamReader = new StreamReader(file);

            string fileData = streamReader.ReadToEnd();
            mapData = fileData.Split(',');
        }
        else if (_loadType == LoadType.asset)
        {
            mapData = _levelAsset.levelInfo;
        }
    }
}
