using System;
using System.IO;
using ParagorGames.PainkillerTools.Utils;


namespace ParagorGames.PainkillerTools.Importers.PKMDL
{
    public static class PainkillerSkeletalModelImporter
    {
        public static PainkillerModel Import(string filePath)
        {
            if (File.Exists(filePath) == false)
            {
                throw new FileNotFoundException();
            }
            
            using var fileStream = File.OpenRead(filePath);
            
            return ImportPainkillerModel(fileStream, Path.GetFileNameWithoutExtension(filePath));
        }
        
        public static PainkillerModel Import(byte[] mapBinaryData, string filePath)
        {
            using var stream = new MemoryStream(mapBinaryData);

            return ImportPainkillerModel(stream, Path.GetFileNameWithoutExtension(filePath));
        }
        
        private static PainkillerModel ImportPainkillerModel(Stream stream, string fileName)
        {
            var pkModel = new PainkillerModel(Path.GetFileNameWithoutExtension(fileName));
            using var reader = new BinaryReader(stream);

            var startFile = reader.ReadInt32(); // 3
            var fileNameInside = reader.ReadNullTerminatedString();
            var exportPath = reader.ReadNullTerminatedString();
            var identifier = reader.ReadNullTerminatedString();

            float[] unknown = new float[6];

            for (int i = 0; i < 6; i++)
            {
                unknown[i] = reader.ReadSingle();
            }

            reader.ReadNullTerminatedString();
            reader.ReadInt16();

            var (rootJoint, bindposes) = reader.ReadRootJoint();

            var skeletalMeshes = reader.ReadSkeletalMeshes();

            pkModel.RootJoint = rootJoint;
            pkModel.Meshes = skeletalMeshes;
            pkModel.BindPoses = bindposes;
            
            return pkModel;
        }
        
        private static (Joint, UnityEngine.Matrix4x4[]) ReadRootJoint(this BinaryReader reader)
        {
            var bonesCount = reader.ReadInt32();

            var bones = new string[bonesCount];
            var childCount = new int[bonesCount];
            var transforms = new UnityEngine.Matrix4x4[bonesCount];

            for (var i = 0; i < bonesCount; i++)
            {
                bones[i] = reader.ReadNullTerminatedString();

                transforms[i] = new UnityEngine.Matrix4x4
                {
                    m00 = reader.ReadSingle(),
                    m01 = -reader.ReadSingle(),
                    m02 = -reader.ReadSingle(),
                    m03 = reader.ReadSingle(),
                    m10 = -reader.ReadSingle(),
                    m11 = reader.ReadSingle(),
                    m12 = reader.ReadSingle(),
                    m13 = reader.ReadSingle(),
                    m20 = -reader.ReadSingle(),
                    m21 = reader.ReadSingle(),
                    m22 = reader.ReadSingle(),
                    m23 = reader.ReadSingle(),
                    m30 = -reader.ReadSingle(),
                    m31 = reader.ReadSingle(),
                    m32 = reader.ReadSingle(),
                    m33 = reader.ReadSingle()
                };

                transforms[i] = transforms[i].transpose;
                childCount[i] = reader.ReadByte();
            }
            
            var jointParams = new JointParams
            {
                Names = bones,
                ChildrenCount = childCount,
                Transforms = transforms
            };
            
            var iterator = 0;
            var jointName = jointParams.Names[0];
            var jointTransform = jointParams.Transforms[0];
            var childrenJoints = GetChildrenJoints(jointParams, jointName, ref iterator);
            return (new Joint(jointName, jointName, jointTransform, childrenJoints), transforms);
        }

        private static Joint[] GetChildrenJoints(JointParams jointParams, string path, ref int iterator)
        {
            var childCount = jointParams.ChildrenCount[iterator];
            
            if (childCount < 1)
            {
                return Array.Empty<Joint>();
            }

            var children = new Joint[childCount];
            

            for (int i = 0; i < childCount; i++)
            {
                iterator++;
                var childPath = path + "/" + jointParams.Names[iterator];
                children[i] = new Joint(jointParams.Names[iterator], childPath, jointParams.Transforms[iterator],
                    GetChildrenJoints(jointParams,childPath, ref iterator));
            }
            
            return children;
        }

        private static SkeletalMesh[] ReadSkeletalMeshes(this BinaryReader reader)
        {
            var meshCount = reader.ReadInt32();
            var meshes = new SkeletalMesh[meshCount];

            for (var i = 0; i < meshCount; i++)
            {
                var name = reader.ReadNullTerminatedString();
                meshes[i] = new SkeletalMesh(name);
                
                reader.ReadInt32();
                reader.ReadInt32();

                var bumpTextureName = reader.ReadNullTerminatedString();
                
                if (!string.IsNullOrEmpty(bumpTextureName))
                {
                    meshes[i].BumpMaterial = new Material(bumpTextureName);
                }

                var materials = reader.ReadColorMaterials();
                var triangles = reader.ReadTriangles();
                
                reader.ReadInt32(); // 0

                var vertexesInfo = reader.ReadVertexes();
                
                reader.ReadInt32(); // 0
                reader.ReadInt32(); // 0
                
                meshes[i].ColorMaterial = materials;
                meshes[i].Triangles = triangles;
                meshes[i].Vertexes = vertexesInfo.Positions;
                meshes[i].Normals = vertexesInfo.Normals;
                meshes[i].UVs = vertexesInfo.UVs;
                meshes[i].Weights = reader.ReadWeights();
            }

            return meshes;
        }
        
        private static Material[] ReadColorMaterials(this BinaryReader reader)
        {
            var texturesCount = reader.ReadInt32();
                    
            var materials = new Material[texturesCount];
                
            for (var i = 0; i < texturesCount; i++)
            {
                var textureName = reader.ReadNullTerminatedString();
                var offset = reader.ReadUInt32();
                var size = reader.ReadUInt32();

                materials[i] = new Material(textureName, offset, size);
            }

            return materials;
        }

        private static int[] ReadTriangles(this BinaryReader reader)
        {
            var trianglesCount = reader.ReadInt32();

            var triangles = new int[trianglesCount];

            for (var index = 0; index < trianglesCount; index++)
            {
                switch (index % 3)
                {
                    case 1: triangles[index + 1] = reader.ReadUInt16(); break;
                    case 2: triangles[index - 1] = reader.ReadUInt16(); break;
                    default: triangles[index] = reader.ReadUInt16(); break;
                }
            }

            return triangles;
        }
        
        private static VertexesInfo ReadVertexes(this BinaryReader reader)
        {
            var vertices = reader.ReadInt32();

            var vertexesInfo = new VertexesInfo
            {
                Positions = new UnityEngine.Vector3[vertices],
                Normals = new UnityEngine.Vector3[vertices],
                UVs = new UnityEngine.Vector2[vertices]
            };

            for (var index = 0; index < vertices; index++)
            {
                vertexesInfo.Positions[index] = new UnityEngine.Vector3(-reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                vertexesInfo.Normals[index] = new UnityEngine.Vector3(-reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                vertexesInfo.UVs[index] = new UnityEngine.Vector2(reader.ReadSingle(), 1 - reader.ReadSingle());
            }

            return vertexesInfo;
        }
        
        private static VertexWeight[] ReadWeights(this BinaryReader reader)
        {
            var vertexCount = reader.ReadInt32();

            var vertexesBlend = new VertexWeight[vertexCount];
                
            for (var j = 0; j < vertexCount; j++)
            {
                var numBones = reader.ReadInt32();
                var weightInfos = new WeightInfo[numBones];

                for (var k = 0; k < numBones; k++)
                {
                    weightInfos[k] = new WeightInfo(reader.ReadInt16(), reader.ReadSingle());
                }

                Array.Sort(weightInfos, (x, y) => (x.Weight <= y.Weight ? 1 : -1));
                    
                vertexesBlend[j] = new VertexWeight(weightInfos);
            }

            return vertexesBlend;
        }

        
        private class VertexesInfo
        {
            public UnityEngine.Vector3[] Positions;
            public UnityEngine.Vector3[] Normals;
            public UnityEngine.Vector2[] UVs;
        }

        private class JointParams
        {
            public string[] Names;
            public int[] ChildrenCount;
            public UnityEngine.Matrix4x4[] Transforms;
        }
    }
}