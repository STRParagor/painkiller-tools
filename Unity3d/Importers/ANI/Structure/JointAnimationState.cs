namespace ParagorGames.PainkillerTools.Importers.ANI
{
    public class JointAnimationState
    {
        public int Key { get; }
        public float Time { get; }
        public UnityEngine.Matrix4x4 Transform { get; }

        public JointAnimationState(int key, float time, UnityEngine.Matrix4x4 transform)
        {
            Key = key;
            Time = time;
            Transform = transform;
        }
    }
}