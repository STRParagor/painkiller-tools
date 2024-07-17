using System;

namespace ParagorGames.PainkillerTools.Importers.MPK
{
    [Serializable]
    public class TextureCollections
    {
        public ColorTextureCollectionInfo ColorTextureCollection;
        public LightmapTextureCollectionInfo LightmapTextureCollection;
        public BlendTextureCollectionInfo BlendTextureCollection;
        public AlphaTextureCollectionInfo AlphaTextureCollection;


        public void FetchTextures(Mesh[] meshes)
        {
            ColorTextureCollection = new ColorTextureCollectionInfo();
            LightmapTextureCollection = new LightmapTextureCollectionInfo();
            BlendTextureCollection = new BlendTextureCollectionInfo();
            AlphaTextureCollection = new AlphaTextureCollectionInfo();

            var lightmapIndex = 0;

            foreach (var pkMesh in meshes)
            {
                if (pkMesh.Materials.Length == 0) continue;
                
                foreach (var pkMeshMaterial in pkMesh.Materials)
                {
                    if (string.IsNullOrEmpty(pkMeshMaterial.LightMap.Name))
                    {
                        break;
                    }
                    
                    ColorTextureCollection.HandleTexture(pkMeshMaterial, pkMesh.Name);
                    LightmapTextureCollection.HandleTexture(pkMeshMaterial, pkMesh.Name);
                    BlendTextureCollection.HandleTexture(pkMeshMaterial, pkMesh.Name);
                    AlphaTextureCollection.HandleTexture(pkMeshMaterial, pkMesh.Name);
                }
            }
        }
    }
}