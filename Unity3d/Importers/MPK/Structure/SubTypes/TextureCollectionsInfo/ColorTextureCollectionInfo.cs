using System.Collections.Generic;
using UnityEditor.Build.Pipeline;

namespace ParagorGames.PainkillerTools.Importers.MPK
{
    public class ColorTextureCollectionInfo : BaseTextureCollectionInfo
    {
        public readonly HashSet<string> Textures;

        public ColorTextureCollectionInfo()
        {
            Textures = new HashSet<string>();
        }

        public override void HandleTexture(Material pkMaterial, string meshName)
        {
            string textureName = pkMaterial.ColorMap.Name.Replace("\0", "");
            
            if (string.IsNullOrEmpty(textureName)) return;
            
            Textures.Add(textureName);
        }
    }
}