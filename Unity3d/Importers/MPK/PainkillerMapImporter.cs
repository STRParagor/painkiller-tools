using System;
using System.IO;
using ParagorGames.PainkillerTools.Utils;

namespace ParagorGames.PainkillerTools.Importers.MPK
{
    public static class PainkillerMapImporter
    {
        public static PainkillerMap Import(string mapPath)
        {
            if (File.Exists(mapPath) == false)
            {
                throw new FileNotFoundException();
            }
            
            using var fileStream = File.OpenRead(mapPath);
            
            return ImportMpk(fileStream, mapPath);
        }
        
        public static PainkillerMap Import(byte[] mapBinaryData, string mapPath)
        {
            using var stream = new MemoryStream(mapBinaryData);

            return ImportMpk(stream, mapPath);
        }
        
        private static PainkillerMap ImportMpk(Stream stream, string mapPath)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            
            using var reader = new BinaryReader(stream);

            string mapName = Path.GetFileNameWithoutExtension(mapPath);
            var painkillerMap = new PainkillerMap(mapName);

            int meshCount = reader.ReadMeshCount();
            int[] meshAddresses = reader.ReadMeshAddresses(meshCount);
            
            var meshes = new Mesh[meshCount];

            for (var i = 0; i < meshAddresses.Length; i++)
            {
                meshes[i] = reader.ReadMesh(meshAddresses[i]);
            }

            reader.Close();
            painkillerMap.Meshes = meshes;

            painkillerMap.TextureCollections = new TextureCollections();
            painkillerMap.TextureCollections.FetchTextures(meshes);

            return painkillerMap;
        }

        private static int ReadMeshCount(this BinaryReader reader)
        {
            reader.BaseStream.Seek(-8, SeekOrigin.End);
            
            return reader.ReadInt32();
        }
        
        private static int[] ReadMeshAddresses(this BinaryReader reader, int meshCount)
        {
            var meshAddresses = new int[meshCount];
            
            reader.BaseStream.Seek(-8 - (meshCount * 4), SeekOrigin.End);
            
            for (var i = 0; i < meshCount; i++)
            {
                meshAddresses[i] = reader.ReadInt32();
            }

            return meshAddresses;
        }

        private static Mesh ReadMesh(this BinaryReader reader, int meshAddress)
        {
            reader.BaseStream.Seek(meshAddress, SeekOrigin.Begin);
            uint start = reader.ReadUInt32();

            string meshName = reader.ReadNullTerminatedString();
            var mesh = new Mesh(meshName);

            // Skip matrix

            var matrix = new float[16];
            for (int i = 0; i < 16; i++)
            {
                matrix[i] = reader.ReadSingle();
            }
            
            mesh.Vertexes = reader.ReadVertexes();

            var bounds = new UnityEngine.Bounds(mesh.Vertexes[0].Position, UnityEngine.Vector3.zero);
            
            for (var i = 1; i < mesh.Vertexes.Length; i++)
            {
                var meshVertex = mesh.Vertexes[i];
                bounds.Encapsulate(meshVertex.Position);
            }

            mesh.PositionOffset = bounds.center;

            // Skip Bounding Box
            
            var bound = new float[6];
            for (int i = 0; i < 6; i++)
            {
                bound[i] = reader.ReadSingle();
            }
            
            mesh.Faces = reader.ReadFaces();
            mesh.Materials = reader.ReadMaterials();

            return mesh;
        }
        
        private static Vertex[] ReadVertexes(this BinaryReader reader)
        {
            int numUVs = reader.ReadInt32();
            int numVerts = reader.ReadInt32();
            
            var vertexes = new Vertex[numVerts];

            for (var j = 0; j < numVerts; j++)
            {
                vertexes[j] = reader.ReadVertex(numUVs);
            }

            if (numUVs == 2)
            {
                numVerts = reader.ReadInt32();
                
                for (var j = 0; j < numVerts; j++)
                {
                    vertexes[j].Normal = reader.ReadVertexNormal();
                }
            }
            else
            {
                reader.BaseStream.Seek(4, SeekOrigin.Current);
            }

            return vertexes;
        }
        
        private static Vertex ReadVertex(this BinaryReader reader, int numUVs)
        {
            var vertex = new Vertex();
            
            vertex.Position = new UnityEngine.Vector3
            {
                x = -reader.ReadSingle(),
                y = reader.ReadSingle(),
                z = reader.ReadSingle(),
            };

            if (numUVs == 2)
            {
                var uVs = new UV[2];
                reader.BaseStream.Seek(4, SeekOrigin.Current);
                uVs[0].U = reader.ReadSingle();
                uVs[0].V = 1 - reader.ReadSingle();
                uVs[1].U = reader.ReadSingle();
                uVs[1].V = 1 - reader.ReadSingle();

                vertex.UVs = uVs;
            }
            else
            {
                var normal = new UnityEngine.Vector3
                {
                    x = reader.ReadSingle(),
                    y = reader.ReadSingle(),
                    z = reader.ReadSingle(),
                };

                vertex.Normal = normal;
                vertex.UVs = new UV[1]
                {
                    new(reader.ReadSingle(), 1 - reader.ReadSingle())
                };
            }

            return vertex;
        }
        
        private static UnityEngine.Vector3 ReadVertexNormal(this BinaryReader reader)
        {
            return new UnityEngine.Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
        }

        private static int[] ReadFaces(this BinaryReader reader)
        {
            int facesCount = reader.ReadInt32();
            var faces = new int[facesCount];

            for (var j = 0; j < facesCount; j++)
            {
                faces[j] = reader.ReadUInt16();
            }

            return faces;
        }

        private static Material[] ReadMaterials(this BinaryReader reader)
        {

            var numMaterials = reader.ReadInt32();
            
            var materials = new Material[numMaterials];

            for (var j = 0; j < numMaterials; j++)
            {
                var material = new Material();
                material.Offset = reader.ReadUInt16();
                material.Size = reader.ReadUInt16();
                material.ColorMap = reader.ReadTextureMap();
                material.LightMap = reader.ReadTextureMap();
                material.BlendMap = reader.ReadTextureMap();
                material.AlphaMap = reader.ReadTextureMap();

                materials[j] = material;
            }

            return materials;
        }
        
        private static TextureMap ReadTextureMap(this BinaryReader reader)
        {
            return new TextureMap
            {
                Name = reader.ReadNullTerminatedString(),
                Offset = new UV(reader.ReadSingle(), reader.ReadSingle()),
                Tiling = new UV(reader.ReadSingle(), reader.ReadSingle())
            };
        }
    }
}