using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class SquadBuilder : MonoBehaviour {

    public List<Unit> squadList;

    public Unit[] unitPrefabs;

    public string fileName;
    public string _path;
    public string file;
    public bool overwrite;

    public SquadViewer squadViewer;

    private void Awake()
    {
        _path = Application.persistentDataPath + "/" + "Squads";
        file = _path + "/" + fileName + ".txt";
        CheckDirectory();
        CheckFile();
    }

    public void DisplaySquad(Unit unit, Transform pos)
    {

    }

    void CheckDirectory()
    {
        if (!Directory.Exists(_path))
        {
            //Debug.Log("Creating Path" + _path);
            Directory.CreateDirectory(_path);
        }
    }

    [NaughtyAttributes.Button("DELETE FILE")]
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

    public void ClearSquad()
    {
        squadList.Clear();
    }

    #region LOAD
    [NaughtyAttributes.Button("LOAD SQUAD")]
    public void Load()
    {
        ClearSquad();
        StreamReader streamReader;
        //sr = new StreamReader(path + "/" + fileName + ".txt");
        streamReader = new StreamReader(file);

        string fileData = streamReader.ReadToEnd();
        string[] squadData = fileData.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

        for (int i = 0; i < squadData.Length; i++)
        {
            if (squadData[i] != string.Empty)
            {
                Vector3 spawn = new Vector3(squadViewer.squadViewerPos[i].transform.position.x, squadViewer.squadViewerPos[i].transform.position.y, squadViewer.squadViewerPos[i].transform.position.z);
                UnitData unitData = JsonUtility.FromJson<UnitData>(squadData[i]);
                Unit temp = Instantiate(unitPrefabs[(int)unitData._class], spawn, Quaternion.identity, squadViewer.gameObject.transform);
                temp.gameObject.transform.rotation = new Quaternion(temp.gameObject.transform.rotation.x, temp.gameObject.transform.rotation.y + 180, temp.gameObject.transform.rotation.z, 0);
                temp.gameObject.transform.localScale *= 5;
                squadList.Add(temp);
                Debug.Log(string.Format("LOADED {0} SUCCESSFULLY", unitData._class.ToString()));
            }
        }
        streamReader.Close();

        Debug.Log((string.Format("{0} UNITS LOADED INTO SQUAD", squadList.Count)));
    }
    #endregion

    #region SAVE
    [NaughtyAttributes.Button("SAVE SQUAD")]
    public void Save()
    {
        StreamWriter streamWriter;
        streamWriter = new StreamWriter(file, true);

        if (squadList != null)
        {
            foreach (Unit unit in squadList)
            {
                streamWriter.WriteLine(JsonUtility.ToJson(unit.UnitData));
            }
        }
        streamWriter.Close();
    }
    #endregion
}
