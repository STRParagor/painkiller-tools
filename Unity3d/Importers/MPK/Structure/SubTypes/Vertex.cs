using System;
using System.Text;
using UnityEngine;

namespace ParagorGames.PainkillerTools.Importers.MPK
{
    [Serializable]
    public struct Vertex
    {
        public Vector3 Position;
        public Vector3 Normal;
        public UV[] UVs;

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append("Pos: ");
            builder.Append(Position);
            builder.Append(" | Normal: ");
            builder.Append(Normal);
            builder.Append(" | UVs: ");
            builder.AppendLine(UVs.Length.ToString());
            builder.Append("UVs: ");
            for (int i = 0; i < UVs.Length; i++)
            {
                builder.Append(i.ToString());
                builder.Append(": ");
                builder.Append(UVs[i].ToString());
            }

            return builder.ToString();
        }
    }
}