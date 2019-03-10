using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Reboot
{
    public static class BorderDetector
    {
        public class Edge
        {
            public Hex Hex;
            public int Index;

            public Edge(Hex hex, int index)
            {
                Hex = hex;
                Index = index;
            }

            public override bool Equals(object obj)
            {
                var edge = obj as Edge;
                return Hex.Equals(edge.Hex) && Index.Equals(edge.Index);
            }

            public Edge Inverse()
            {
                return new Edge(Hex.Neighbor(Index), (Index + 2) % 6);
            }

        }

        public static List<Edge> edges;
        public static LinkedList<KeyValuePair<Hex, int>> linkedList;
        public static Edge currentEdge;
        /*
        3. Set Q to the empty queue.
        4. Add all corners of node to List of corners
        5. Add node to the end of Q.
        6. While Q is not empty:
        7.     n Q.Dequeue.
        9.     If the color of the node to the west of n is target-color,
        for (int i = 0;i < 6; i++)
        {
        if(neighbor(i) && edge has not been visited)
        {
            set edge visited and add node to queue
        }
        }
                    set the color of that node to replacement-color and add that node to the end of Q.
        10.     If the color of the node to the east of n is target-color,
                    set the color of that node to replacement-color and add that node to the end of Q.
        11.     If the color of the node to the north of n is target-color,
                    set the color of that node to replacement-color and add that node to the end of Q.
        12.     If the color of the node to the south of n is target-color,
                    set the color of that node to replacement-color and add that node to the end of Q.
        13. Continue looping until Q is exhausted.
        14. Return.
             
        */
        public static IEnumerator GetBorder(List<Tile> tiles)
        {
            if (tiles == null && tiles.Count < 2)
            {
                throw new System.Exception("OUT OF RANGE");
            }

            List<Hex> hexes = tiles.Select(x => x.Position).ToList();
            Queue<Edge> openSet = new Queue<Edge>();
            HashSet<Hex> closedSet = new HashSet<Hex>();

            edges = new List<Edge>();

            Hex firstHex = hexes.First();

            for (int i = 0; i < 6; i++)
            {
                Edge e = new Edge(firstHex, i);
                openSet.Enqueue(e);
                edges.Add(e);
            }

            int previousIndex = 0;

            while (openSet.Count > 0)
            {
                do
                {
                    currentEdge = openSet.Dequeue();
                } while (closedSet.Contains(currentEdge.Hex));

                Hex nextHex = currentEdge.Hex.Neighbor(currentEdge.Index);
                if (hexes.Contains(nextHex))
                {
                    int indexOfEdge = edges.IndexOf(currentEdge);
                    previousIndex = ((currentEdge.Index + 3 + 1) % 6);
                    for (int j = previousIndex + 4; j >= previousIndex; j--)
                    {
                        Edge edge = new Edge(nextHex, (j % 6));
                        Hex neighborHex = nextHex.Neighbor(j % 6);
                        if (closedSet.Contains(neighborHex))
                        {
                           edges.Remove(new Edge(neighborHex, (j + 3) % 6));
                        }
                        else
                        {
                            openSet.Enqueue(edge);
                            if(indexOfEdge >= 0 && indexOfEdge < edges.Count)
                            {
                                edges.Insert(indexOfEdge, edge);
                            }
                            else
                            {
                                Debug.Log("INDEX IS WRONG");
                            }
                        }
                        yield return new WaitForEndOfFrame();
                    }
                    closedSet.Add(nextHex);
                    edges.Remove(currentEdge);
                }
            }
        }
    }
}