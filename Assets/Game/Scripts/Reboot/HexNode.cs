using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reboot
{
    public class HexNode : INavigationNode<Hex>
    {
        public Hex Data
        {
            get;
            set;
        }

        public IEnumerable<INavigationNode<Hex>> Connections
        {
            get;
            set;
        }
    }
}