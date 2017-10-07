using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class Interactor_Behaviour : MonoBehaviour {

    public LevelCube tile;
    public Unit _unit;

    RaycastHit _hit;


    void Start () {
		
	}
	
	void FixedUpdate ()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out _hit, 100.0f))
            {
                if (_hit.transform.gameObject.GetComponent<LevelCube>())
                {
                    if (tile != null)
                    {
                        tile.TileDeselect();
                    }

                    if (_unit != null)
                    {
                        _unit.UnitDeselect();
                    }

                    tile = _hit.transform.gameObject.GetComponent<LevelCube>();
                    tile.TileSelected();
                    Debug.Log("Tile - " + "Grid Pos " + _hit.transform.position.x + "," + _hit.transform.position.z + " Status - " + tile.status);
                    
                }
            }

            if (Physics.Raycast(ray, out _hit, 100.0f))
            {
                if (_hit.transform.gameObject.GetComponent<Unit>())
                {
                    if (_unit != null)
                    {
                        _unit.UnitDeselect();
                    }

                    if (tile != null)
                    {
                        tile.TileDeselect();
                    }

                    _unit = _hit.transform.gameObject.GetComponent<Unit>();
                    _unit.UnitSelected();
                    Debug.Log("Unit - " + _unit.unitName + " | Slot1 - " + _unit.slot1.buffName + " | Slot2 - " + _unit.slot1.buffName + " | Race - " + _unit.raceType._unitRace + " | Class - " + _unit.classType._unitClass);
                }
            }
        }
        
	}
}
