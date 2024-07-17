using System;

namespace ParagorGames.PainkillerTools.Importers.MPK
{
    [Serializable]
    public struct TextureMap
    {
        public string Name;
        public UV Offset;
        public UV Tiling;

        public TextureMap(string textureName, UV offset, UV tiling)
        {
            Name = textureName;
            Offset = offset;
            Tiling = tiling;
        }
    }
}