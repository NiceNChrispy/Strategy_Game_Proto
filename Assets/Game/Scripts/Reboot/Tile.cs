using System;
using System.Collections.Generic;
using UnityEngine;

namespace Reboot
{
    public class Tile : MonoBehaviour, ISelectableComponent<Tile>
    {
        public NavNode<Hex> HexNode;

        public Tile Data
        {
            get
            {
                return this;
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