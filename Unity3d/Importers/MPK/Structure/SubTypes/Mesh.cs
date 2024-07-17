using System;

namespace ParagorGames.PainkillerTools.Importers.MPK
{
    [Serializable]
    public class Mesh
    {
        public string Name;
        public UnityEngine.Vector3 PositionOffset;
        public Vertex[] Vertexes;
        public int[] Faces;
        public Material[] Materials;
        public UnityEngine.Bounds Bounds;
        public int UVChannels = 1;

        public Mesh(string meshName)
        {
            Name = meshName;
        }
    }
}