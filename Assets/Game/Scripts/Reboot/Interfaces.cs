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

    public interface INavNode<T>
    {
        T Position { get; set; }
        bool IsTraversible { get; set; }
        float Cost { get; set; }
        List<INavNode<T>> Connected { get; set; }
    }

    public interface IHealth<T>
    {
        int Health { get; set; }
        int MaxHealth { get; set; }

        void Heal(int amount);
        void Heal();
        void Damage(int amount);
        void Kill();
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