namespace ParagorGames.PainkillerTools.Importers.ANI
{
    public class PainkillerAnimation
    {
        public string Name { get; }
        public JointAnimation[] JointAnimations { get; }

        public PainkillerAnimation(string name, JointAnimation[] jointAnimations)
        {
            Name = name;
            JointAnimations = jointAnimations;
        }
    }
}