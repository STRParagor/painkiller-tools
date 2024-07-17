using System;
using UnityEngine;

namespace ParagorGames.PainkillerTools.Importers.PKMDL
{
    public class Joint
    {
        public string Name { get; }
        public string Path { get; }
        public Joint[] Children { get; }
        public Matrix4x4 TransformMatrix { get; }

        public Joint(string name, string path, Matrix4x4 transformMatrix)
        {
            Name = name;
            Path = path;
            TransformMatrix = transformMatrix;
            Children = Array.Empty<Joint>();
        }
        
        public Joint(string name, string path, Matrix4x4 transformMatrix, Joint[] children)
        {
            Name = name;
            Path = path;
            TransformMatrix = transformMatrix;
            Children = children;
        }

        public Joint FindJointByName(string name)
        {
            if (Name == name) return this;

            foreach (var child in Children)
            {
                var joint = child.FindJointByName(name);
                if (joint != null)
                {
                    return joint;
                }
            }

            return null;
        }
    }
}