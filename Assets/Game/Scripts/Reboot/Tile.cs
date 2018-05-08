using System;
using System.Collections.Generic;
using UnityEngine;

namespace Reboot
{
    public class Tile : MonoBehaviour, ISelectableComponent<Hex>
    {
        public NavNode<Hex> HexNode;

        public Hex SelectableComponent
        {
            get
            {
                return HexNode.Data;
            }
        }

        public bool IsSelectable { get; set; }
        public bool IsSelected { get; set; }

        private void OnEnable()
        {
            IsSelectable = true;
        }

        public void Deselect() {}

        public void OnCursorEnter() {}

        public void OnCursorExit() {}

        public void Select()
        {
            Debug.Log(name);
        }
    }
}