using System;

namespace ParagorGames.PainkillerTools.Importers.PKMDL
{
    [Serializable]
    public class Material
    {
        public uint Offset;
        public uint Size;
        public string Texture;

        public Material(string textureName, uint offset = 0, uint size = 0)
        {
            Texture = textureName;
            Offset = offset;
            Size = size;
        }
    }
}