using DataStructures;
using System.Collections.Generic;
using UnityEngine;

namespace Reboot
{
    public interface ISelectableComponent<T>
    {
        T Data { get; }
        bool IsSelectable { get; set; }
        bool IsSelected { get; set; }
        void OnCursorEnter();
        void OnCursorExit();
        void Select();
        void Deselect();
    }

    public class NavNode<T>
    {
        public T Data { get; set; }
        public bool IsTraversible { get; set; }
        public float Cost { get; set; }
        public List<NavNode<T>> Connected { get; set; }

        public NavNode(T data, bool isTraversible, float cost)
        {
            Data = data;
            IsTraversible = isTraversible;
            Cost = cost;
            Connected = new List<NavNode<T>>();
        }
    }
}

namespace DataStructures
{
    public interface IHeuristic<T>
    {
        float Heuristic(T from, T to);
        float NeighborDistance(T from, T to);
    }
}