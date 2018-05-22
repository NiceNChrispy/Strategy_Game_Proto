using System;
using System.Collections.Generic;
using UnityEngine;

namespace Reboot
{
    public class Tile : MonoBehaviour, ISelectableComponent<Tile>
    {
        public enum State { NONE = 0, MOVEABLE = 1, ATTACKABLE = 2, FRIENDLY = 3, HOSTILE = 4 }

        private Material m_Material;

        [SerializeField] Color[] m_StateColors;

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
            m_Material = GetComponent<Renderer>().material;
            m_StateColors[0] = m_Material.color;
        }

        public void Deselect() { }

        public void OnCursorEnter() { }

        public void OnCursorExit() { }

        public void Select()
        {
            Debug.Log(name);
        }

        public void SetState(State state)
        {
            m_Material.color = m_StateColors[(int)state];
        }
    }
}