using UnityEngine;

namespace ParagorGames.PainkillerTools.Importers.PKMDL
{
    public class SkeletalMesh
    {
        public string Name { get; }
        public int[] Triangles { get; set; }
        public Vector3[] Vertexes { get; set; }
        public Vector3[] Normals { get; set; }
        public Vector2[] UVs { get; set; }
        public Material[] ColorMaterial  { get; set; }
        public Material BumpMaterial  { get; set; }
        public VertexWeight[] Weights { get; set; }
        
        public SkeletalMesh(string name)
        {
            Name = name;
        }
    }
}