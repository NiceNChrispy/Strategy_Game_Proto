using DataStructures;
using System.Collections.Generic;
using UnityEngine;

namespace Reboot
{
    public interface ISelectableComponent<MonoBehaviour>
    {
        MonoBehaviour Component { get; }
        bool IsSelectable { get; set; }
        bool IsSelected { get; set; }
        void OnCursorEnter();
        void OnCursorExit();
        void Select();
        void Deselect();
    }

    public interface INavAgent<T>
    {
        AStarNode<T> OccupiedNode { get; set; }
        NavGraph<T> NavGraph { get; set; }
        void Move(AStarNode<T> to);
    }
}