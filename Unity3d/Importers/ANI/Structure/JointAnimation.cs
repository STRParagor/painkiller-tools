namespace ParagorGames.PainkillerTools.Importers.ANI
{
    public class JointAnimation
    {
        public string JointName { get; private set; }
        public JointAnimationState[] JointAnimationStates { get; private set; }

        public JointAnimation(string jointName, JointAnimationState[] animationStates)
        {
            JointName = jointName;
            JointAnimationStates = animationStates;
        }
    }
}