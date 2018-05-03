using DataStructures;
using System.Collections.Generic;
using UnityEngine;

namespace Reboot
{
    public interface ISelectableComponent<MonoBehaviour>
    {
        MonoBehaviour SelectableComponent { get; }
        bool IsSelectable { get; set; }
        bool IsSelected { get; set; }
        void OnCursorEnter();
        void OnCursorExit();
        void Select();
        void Deselect();
    }

    public interface NavNode<T>
    {
        T Data { get; set; }
        bool IsTraversible { get; set; }
        float Cost { get; set; }
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