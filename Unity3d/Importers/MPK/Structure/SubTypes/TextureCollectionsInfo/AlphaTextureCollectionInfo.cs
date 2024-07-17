using System.Collections.Generic;

namespace ParagorGames.PainkillerTools.Importers.MPK
{
    public class AlphaTextureCollectionInfo : BaseTextureCollectionInfo
    {
        public readonly HashSet<string> Textures;

        public AlphaTextureCollectionInfo()
        {
            Textures = new HashSet<string>();
        }

        public override void HandleTexture(Material pkMaterial, string meshName)
        {
            string textureName = pkMaterial.AlphaMap.Name.Replace("\0", "");
            
            if (string.IsNullOrEmpty(textureName)) return;
            
            Textures.Add(textureName);
        }
    }
}