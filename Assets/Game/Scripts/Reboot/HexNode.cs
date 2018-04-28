using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reboot
{
    [System.Serializable]
    public class HexNode
    {
        public HexNode(Hex value)
        {
            Value = value;
        }

        public Hex Value
        {
            get; set;
        }
    }
}