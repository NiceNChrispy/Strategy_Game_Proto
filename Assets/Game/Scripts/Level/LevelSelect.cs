using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour {

    LevelLoader loader;
    public List<Level> _levels;

    public Text levelName;

    public int chosenLevel;

    public void Awake()
    {
        loader = GetComponent<LevelLoader>();
        levelName.text = _levels[chosenLevel].name;
    }

    public void Next()
    {
            chosenLevel++;
            if (chosenLevel > _levels.Count - 1)
            {
                chosenLevel = 0;
                levelName.text = _levels[chosenLevel].name;
            }
            levelName.text = _levels[chosenLevel].name;
    }

    public void Previous()
    {
            chosenLevel--;
            if (chosenLevel < 0)
            {
                chosenLevel = _levels.Count - 1;
                levelName.text = _levels[chosenLevel].name;
            }
            levelName.text = _levels[chosenLevel].name;
    }

    public void SelectLevel()
    {
        loader._levelAsset = _levels[chosenLevel];
    }
}
