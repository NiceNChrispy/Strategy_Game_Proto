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
        DontDestroyOnLoad(this);
        _path = Application.persistentDataPath + "/" + "Squads";
        file = _path + "/" + fileName + ".txt";
        CheckDirectory();
        CheckFile();
        //Load();
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
                temp.gameObject.transform.localScale *= 4;
                temp.UnitData.startX = unitData.startX;
                temp.UnitData.startY = unitData.startY;
                squadList.Add(temp);
            }
        }
        streamReader.Close();
        LoadUnitInfo();
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

    #region DeleteUnit
    public void DeleteUnit()
    {
        Destroy(squadList[squadViewer.activeUnit].gameObject);
        squadList.RemoveAt(squadViewer.activeUnit);
    }
    #endregion

    #region AddInUnit
    public void AddUnit()
    {
        if (squadList[squadViewer.activeUnit] == null)
        {

        }
    }
    #endregion

    #region LoadUnitData
    public void LoadUnitInfo()
    {
        squadViewer.unitClass.text = squadList[squadViewer.activeUnit].UnitData._class.ToString();


        squadViewer.health.text = "Health - " + squadList[squadViewer.activeUnit].UnitData._stats._health.ToString();
        squadViewer.damage.text = "Damage - " + squadList[squadViewer.activeUnit].UnitData._stats._damage.ToString();
        squadViewer.crit.text = "Crit Chance - " + squadList[squadViewer.activeUnit].UnitData._stats._critChance.ToString();
        squadViewer.movement.text = "Movement - " + squadList[squadViewer.activeUnit].UnitData._stats._movement.ToString();
        squadViewer.armour.text = "Armour - " + squadList[squadViewer.activeUnit].UnitData._stats._armour.ToString();
        squadViewer.shield.text = "Shield - " + squadList[squadViewer.activeUnit].UnitData._stats._shield.ToString();
        squadViewer.resistMelee.text = "Resist Melee - " + squadList[squadViewer.activeUnit].UnitData._stats._resistMelee.ToString();
        squadViewer.resistRange.text = "Resist Ranged - " + squadList[squadViewer.activeUnit].UnitData._stats._resistRanged.ToString();
        squadViewer.resistMagic.text = "Resist Magic - " + squadList[squadViewer.activeUnit].UnitData._stats._resistMagic.ToString();
        squadViewer.unitX.value = squadList[squadViewer.activeUnit].UnitData.startX;
        squadViewer.unitY.value = squadList[squadViewer.activeUnit].UnitData.startY;
    }
    #endregion


    public void SetX()
    {
        squadList[squadViewer.activeUnit].UnitData.startX = squadViewer.unitX.value;
        
    }

    public void SetY()
    {
        squadList[squadViewer.activeUnit].UnitData.startY = squadViewer.unitY.value;
    }
}
