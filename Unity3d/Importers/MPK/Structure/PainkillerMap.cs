using System;
using ParagorGames.UltimatePainkiller.PainkillerContent;

namespace ParagorGames.PainkillerTools.Importers.MPK
{
    [Serializable]
    public class PainkillerMap
    {
        public readonly string Name;
        public Mesh[] Meshes;
        public TextureCollections TextureCollections;
        public PKLightMapData LightmapData;
        //public List<PKLightMapInfo> LightmapData;

        public PainkillerMap(string mapName)
        {
            Name = mapName;
        }
    }
}