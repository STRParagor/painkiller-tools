using System.IO;
using ParagorGames.PainkillerTools.Utils;


namespace ParagorGames.PainkillerTools.Importers.ANI
{
    public static class PainkillerAnimationImporter
    {
        public static PainkillerAnimation Import(string filePath)
        {
            if (File.Exists(filePath) == false)
            {
                throw new FileNotFoundException();
            }
            
            using var fileStream = File.OpenRead(filePath);
            
            return ImportPainkillerModel(fileStream, filePath);
        }
        
        public static PainkillerAnimation Import(byte[] aniBinaryData, string filePath)
        {
            using var stream = new MemoryStream(aniBinaryData);
            
            return ImportPainkillerModel(stream, filePath);
        }
        
        private static PainkillerAnimation ImportPainkillerModel(Stream stream, string filePath)
        {
            using var binaryReader = new BinaryReader(stream);

            binaryReader.ReadInt32(); // always 1818585971 or skel
            binaryReader.ReadInt32(); // ?
            
            var jointsCount = binaryReader.ReadInt32();

            var jointAnimations = new JointAnimation[jointsCount];

            for (int i = 0; i < jointsCount; i++)
            {
                var jointName = binaryReader.ReadNullTerminatedString();
                var keysCount = binaryReader.ReadInt32();
                
                var transforms = new UnityEngine.Matrix4x4[keysCount];
                var jointAnimationStates = new JointAnimationState[keysCount];

                for (var j = 0; j < keysCount; j++)
                {
                    var time = binaryReader.ReadSingle();
                    transforms[j] = new UnityEngine.Matrix4x4
                    {
                        m00 = binaryReader.ReadSingle(),
                        m01 = -binaryReader.ReadSingle(),
                        m02 = -binaryReader.ReadSingle(),
                        m03 = binaryReader.ReadSingle(),
                        m10 = -binaryReader.ReadSingle(),
                        m11 = binaryReader.ReadSingle(),
                        m12 = binaryReader.ReadSingle(),
                        m13 = binaryReader.ReadSingle(),
                        m20 = -binaryReader.ReadSingle(),
                        m21 = binaryReader.ReadSingle(),
                        m22 = binaryReader.ReadSingle(),
                        m23 = binaryReader.ReadSingle(),
                        m30 = -binaryReader.ReadSingle(),
                        m31 = binaryReader.ReadSingle(),
                        m32 = binaryReader.ReadSingle(),
                        m33 = binaryReader.ReadSingle()
                    };

                    transforms[j] = transforms[j].transpose;
                    jointAnimationStates[j] = new JointAnimationState(j, time, transforms[j]);
                }

                jointAnimations[i] = new JointAnimation(jointName, jointAnimationStates);
            }

            var animName = Path.GetFileNameWithoutExtension(filePath).Replace('.', '@');

            return new PainkillerAnimation(animName, jointAnimations);
        }
    }
}