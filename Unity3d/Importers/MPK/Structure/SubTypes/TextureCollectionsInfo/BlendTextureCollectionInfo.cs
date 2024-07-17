using System.Collections.Generic;

namespace ParagorGames.PainkillerTools.Importers.MPK
{
    public class BlendTextureCollectionInfo : BaseTextureCollectionInfo
    {
        public readonly HashSet<string> Textures;

        public BlendTextureCollectionInfo()
        {
            Textures = new HashSet<string>();
        }

        public override void HandleTexture(Material pkMaterial, string meshName)
        {
            string textureName = pkMaterial.BlendMap.Name.Replace("\0", "");
            
            if (string.IsNullOrEmpty(textureName)) return;
            
            Textures.Add(textureName);
        }
    }
}