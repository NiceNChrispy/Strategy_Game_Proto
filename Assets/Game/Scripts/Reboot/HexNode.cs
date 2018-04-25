using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reboot
{
    [System.Serializable]
    public class HexNode : IVertex<Hex>
    {
        public HexNode(Hex hex)
        {
            Value = hex;
        }

        public Hex Value
        {
            get;
            set;
        }
    }
}