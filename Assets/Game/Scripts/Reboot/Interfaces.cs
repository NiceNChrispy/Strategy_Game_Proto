using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reboot
{
    public interface ISelectable
    {
        bool IsSelectable { get; set; }
        void Select();
        void Deselect();
    }

    public interface IGraphNode<T>
    {
        List<IGraphNode<T>> Connected { get; set; }
        T Data { get; set; }
    }

    public interface ITilable
    {
        void SetPosition(float x, float y, float z);
    }
}