using System.IO;

namespace ParagorGames.PainkillerTools.Utils
{
    public static class BinaryReaderExtensions
    {
        public static string ReadNullTerminatedString(this BinaryReader reader, int stringLenght)
        {
            var chars = new char[stringLenght];

            chars = reader.ReadChars(stringLenght);
            
            return new string(chars).Replace("\0", "");
        }
        
        public static string ReadNullTerminatedString(this BinaryReader reader)
        {
            var stringLenght = reader.ReadInt32();
            var chars = new char[stringLenght];

            chars = reader.ReadChars(stringLenght);
            
            return new string(chars).Replace("\0", "");
        }

        public static void WriteNullTerminatedString(this BinaryWriter writer, string text)
        {
            writer.Write(text.Length + 1);
            
            var chars = new char[text.Length + 1];
            for (var i = 0; i < text.Length; i++)
            {
                chars[i] = text[i];
            }
            chars[^1] = '\0';

            writer.Write(chars);
        }
    }
}