using DataStructures;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Reboot
{
    public class LevelBuilder : MonoBehaviour
    {
        [SerializeField] private LevelData m_LevelData;
        [SerializeField] private Tile m_TilePrefab;
        [SerializeField] private string m_LevelName = "LEVEL.txt";
        string Path(string file) { return Application.persistentDataPath + "/Maps/" + file; }

        private bool Load<T>(string filePath, out T loaded)
        {
            if (File.Exists(filePath))
            {
                StreamReader sr = new StreamReader(filePath);
                loaded = JsonUtility.FromJson<T>(sr.ReadToEnd());
                sr.Close();
                return true;
            }
            loaded = default(T);
            return false;
        }

        [NaughtyAttributes.Button]
        public void BuildLevel()
        {
            Map<Hex> map;
            if (Load(Path(m_LevelName), out map))
            {
                NavGraph<Hex> navGraph = new NavGraph<Hex>();
                foreach (Hex hex in map.Contents)
                {
                    Tile tile = Instantiate(m_TilePrefab);
                    tile.transform.parent = this.transform;
                    tile.transform.localPosition = m_LevelData.Layout.HexToPixel(hex);
                    tile.Position = hex;
                    tile.name = string.Format("{0},{1},{2}", hex.q, hex.r, hex.s);
                    navGraph.Nodes.Add(tile);
                    m_LevelData.Tiles.Add(tile);
                }
                foreach (var navNode in navGraph.Nodes)
                {
                    navNode.Connected = new List<INavNode<Hex>>();
                    foreach (Hex hex in navNode.Position.AllNeighbors())
                    {
                        if (map.Contains(hex))
                        {
                            navNode.Connected.Add(navGraph.Nodes.Single(x => x.Position == hex));
                        }
                    }
                }
            }
        }
    }
}