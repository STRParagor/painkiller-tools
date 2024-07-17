using System;
using UnityEngine;

namespace ParagorGames.PainkillerTools.Importers.MPK
{
    [Serializable]
    public struct UV
    {
        public float U;
        public float V;

        public UV(float u, float v)
        {
            U = u;
            V = v;
        }
        
        public UV(Vector2 uv)
        {
            U = uv.x;
            V = uv.y;
        }
        
        public override string ToString()
        {
            return $"U: {U} V: {V}";
        }
    }
}