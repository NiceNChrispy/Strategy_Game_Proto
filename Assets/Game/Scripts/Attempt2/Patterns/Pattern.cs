using System.Collections.Generic;
using UnityEngine;

namespace Prototype
{
    public abstract class Pattern : ScriptableObject
    {
        public abstract Vector2Int Size { get; }
        public abstract List<Vector2Int> GetPositions();
    }
}