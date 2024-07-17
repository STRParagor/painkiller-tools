using UnityEngine;

namespace ParagorGames.PainkillerTools.Importers.PKMDL
{
    public class PainkillerModel
    {
        public string Name { get; }
        public Joint RootJoint { get; set; }
        public Matrix4x4[] BindPoses { get; set; }
        public SkeletalMesh[] Meshes { get; set; }

        public PainkillerModel(string name)
        {
            Name = name;
        }

    }
}