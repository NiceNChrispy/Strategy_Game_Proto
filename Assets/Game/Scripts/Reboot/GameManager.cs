using DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Reboot
{
    public class GameManager : Singleton<GameManager>
    {
        private List<Player> m_Players;
        private int m_PlayerTurn;
        private int m_StartingPlayerTurn;
        //private Map<Hex> m_Map;
        private MapData m_Map;

        NavGraph<Hex> m_NavGraph;
        [SerializeField] private Tile m_TilePrefab;

        public List<Tile> Tiles { get; private set; }

        [SerializeField, Range(0, 180)] int m_TurnLength = 30;
        [SerializeField, ReadOnly] int m_TurnCount = 0;
        private HexHeuristic m_Heuristic;
        public Player PlayerWithTurn { get { return m_Players[m_PlayerTurn]; } }
        public List<Player> PlayersWithoutTurn { get { return m_Players.Where(x => x != PlayerWithTurn).ToList(); } }

        [SerializeField] private string m_LevelName = "LEVEL.txt";
        string Path(string file) { return Application.persistentDataPath + "/Maps/" + file; }

        public float TimeBeforeNextPlayersTurn { get { return PlayerWithTurn.RemainingTime; } }

        public event Action OnPlayerUpdateTiles = delegate { };
        public event Action OnGameBegin = delegate { };
        public event Action OnTurnEnd = delegate { };

        private void Start()
        {
            m_Players = GetComponentsInChildren<Player>().ToList();
            if(m_Players.Count == 0)
            {
                throw new Exception("At least one player must join");
            }
            Debug.Log(string.Format("{0} players joined", m_Players.Count));
            if (!MapData.Load(Path(m_LevelName), out m_Map))
            {
                throw new System.Exception("FAILED TO LOAD LEVEL");
            }
            else
            {
                Debug.Log(string.Format("Loaded Map With {0} Hexes", m_Map.Count));
                m_NavGraph = new NavGraph<Hex>();
                Tiles = new List<Tile>();
                foreach (Hex hex in m_Map)
                {
                    Tile tile = Instantiate(m_TilePrefab);
                    tile.transform.parent = this.transform;
                    tile.transform.localPosition = (Vector3)HexToWorld(hex);
                    tile.Position = hex;
                    tile.name = string.Format("{0},{1},{2}", hex.q, hex.r, hex.s);
                    m_NavGraph.Nodes.Add(tile);
                    Tiles.Add(tile);
                }
                foreach (var navNode in m_NavGraph.Nodes)
                {
                    foreach(Hex hex in navNode.Position.AllNeighbors())
                    {
                        if (m_Map.Contains(hex))
                        {
                            navNode.Connected.Add(m_NavGraph.Nodes.Single(x => x.Position == hex));
                        }
                    }
                }
            }
            Begin();
        }

        public void Begin()
        {
            foreach (Player player in m_Players)
            {
                player.Init();
                player.OnTilesUpdated += OnPlayerUpdateTiles;
                foreach (Unit unit in player.Units)
                {
                    if(!unit.enabled)
                    {
                        continue;
                    }
                    Hex nearestHex = WorldToHex(unit.transform.position);
                    unit.OccupiedNode = m_NavGraph.Nodes.SingleOrDefault(x => x.Position == nearestHex);

                    if (unit.OccupiedNode == null)
                    {
                        throw new System.Exception("Unit on inaccessible node");
                    }
                    unit.OccupiedNode.IsTraversible = false;
                    unit.transform.position = HexToWorld(nearestHex);
                }
            }
            m_PlayerTurn = UnityEngine.Random.Range(0, m_Players.Count);
            m_StartingPlayerTurn = m_PlayerTurn;
            m_TurnCount = 0;
            StartCoroutine(PlayerWithTurn.CountDown((float)m_TurnLength, OnTurnComplete));
            OnGameBegin.Invoke();
        }

        public void OnTurnComplete()
        {
            m_PlayerTurn = (m_PlayerTurn + 1) % m_Players.Count;
            if (m_PlayerTurn == m_StartingPlayerTurn)
            {
                m_TurnCount++;
            }

            StartCoroutine(PlayerWithTurn.CountDown((float)m_TurnLength, OnTurnComplete));
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                PlayerWithTurn.EndTurn();
            }
        }

        public List<Tile> GetPath(Hex from, Hex to)
        {
            List<INavNode<Hex>> pathNodes = m_NavGraph.GetPath(from, to, m_Heuristic);
            List<Tile> pathTiles = new List<Tile>();
            foreach (INavNode<Hex> node in pathNodes)
            {
                pathTiles.Add(Tiles.Single(x => (INavNode<Hex>)x == node));
            }
            return pathTiles;
        }

        public List<Tile> GetTilesInAttackRange(Hex from, int range)
        {
            List<INavNode<Hex>> nodesInRange = m_NavGraph.Nodes.Where(x => x.Position.Distance(from) <= range).ToList();
            List<Tile> tilesInRange = new List<Tile>();
            foreach (INavNode<Hex> node in nodesInRange)
            {
                tilesInRange.Add(Tiles.Single(x => (INavNode<Hex>)x == node));
            }
            return tilesInRange;
        }

        public List<Tile> GetTilesInMovementRange(Hex from, int range)
        {
            List<INavNode<Hex>> nodesInRange = m_NavGraph.GetNodesInRange(from, range, m_Heuristic);
            List<Tile> tilesInRange = new List<Tile>();
            foreach (INavNode<Hex> node in nodesInRange)
            {
                tilesInRange.Add(Tiles.Single(x => (INavNode<Hex>)x == node));
            }

            return tilesInRange;
        }

        public Vector2 HexToWorld(Hex hex)
        {
            return Layout.Default.HexToPixel(hex);
        }

        public Hex WorldToHex(Vector2 worldPosition)
        {
            return Layout.Default.PixelToHex(worldPosition).HexRound();
        }

        public void Attack(INavNode<Hex> targetHex, AttackData attackData)
        {
            foreach (Player player in PlayersWithoutTurn)
            {
                foreach (Unit unit in player.Units)
                {
                    if (unit.Position == targetHex.Position)
                    {
                        unit.Damage(attackData.Damage);
                    }
                }
            }
        }
    }
}