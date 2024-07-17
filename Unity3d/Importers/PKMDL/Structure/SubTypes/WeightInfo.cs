namespace ParagorGames.PainkillerTools.Importers.PKMDL
{
    public class WeightInfo
    {
        public short BoneId { get; set; }
        public float Weight { get; set; }

        public WeightInfo(short boneId, float weight)
        {
            BoneId = boneId;
            Weight = weight;
        }
    }
}