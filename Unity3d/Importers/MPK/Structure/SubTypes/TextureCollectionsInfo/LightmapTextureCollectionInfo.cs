using System.Collections.Generic;

namespace ParagorGames.PainkillerTools.Importers.MPK
{
    public class LightmapTextureCollectionInfo : BaseTextureCollectionInfo
    {
        public readonly Dictionary<string, LightMapInfo> LightmapsInfo;
        public readonly HashSet<string> Textures;
        private int _lastLightmapIndex = 0;

        public LightmapTextureCollectionInfo()
        {
            LightmapsInfo = new Dictionary<string, LightMapInfo>();
            Textures = new HashSet<string>();
        }

        public override void HandleTexture(Material pkMaterial, string meshName)
        {
            var lightmapName = pkMaterial.LightMap.Name.ToLower();

            if (string.IsNullOrEmpty(lightmapName) || lightmapName is "lightmap" or "water_bake") return;
            
            if (LightmapsInfo.TryGetValue(lightmapName, out var collection))
            {
                collection.AddMeshName(meshName);
            }
            else
            {
                Textures.Add(lightmapName);
                LightmapsInfo.Add(lightmapName, new LightMapInfo(_lastLightmapIndex++));
                LightmapsInfo[lightmapName].AddMeshName(lightmapName);
            }
        }
    }
}