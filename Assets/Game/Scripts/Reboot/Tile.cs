using System.Collections.Generic;
using UnityEngine;

namespace Reboot
{
    public class Tile : MonoBehaviour, ISelectableComponent<Tile>, INavNode<Hex>
    {
        public int Height;

        //TODO: REMOVE ALL THIS JUNK
        public Tile Data
        {
            get
            {
                return this;
            }
        }

        public bool IsSelectable { get; set; }
        public bool IsSelected { get; set; }

        public Hex Position
        {
            get; set;
        }

        public bool IsTraversible { get; set; }
        public float Cost { get; set; }
        public List<INavNode<Hex>> Connected { get; set; }

        private void OnEnable()
        {
            IsSelectable = true;
            IsTraversible = true;
            Connected = new List<INavNode<Hex>>();
        }

        public void Deselect() { }
        public void OnCursorEnter() { }
        public void OnCursorExit() { }
        public void Select() {}
        //END TODO
    }
}