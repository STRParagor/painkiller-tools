using System.Collections.Generic;

namespace ParagorGames.PainkillerTools.Importers.MPK
{
    public class LightMapInfo
    {
        public int GroupId;
        public List<string> MeshCollection;

        public LightMapInfo(int groupId)
        {
            MeshCollection = new List<string>();
            GroupId = groupId;
        }

        public void AddMeshName(string pkMeshName)
        {
            MeshCollection.Add(pkMeshName);
        }
    }
}