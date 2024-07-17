using System;

namespace ParagorGames.PainkillerTools.Importers.MPK
{
    [Serializable]
    public class Material
    {
        public ushort Offset;
        public ushort Size;
        public TextureMap ColorMap;
        public TextureMap LightMap;
        public TextureMap BlendMap;
        public TextureMap AlphaMap;
    }
}