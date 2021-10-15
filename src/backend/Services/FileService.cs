using Newtonsoft.Json;
using System.IO;

namespace backend.Services
{
    /// <summary>
    /// NEEDS A COMMENT ABOUT EXCEPTIONS!
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class FileService<T> where T : class
    {
        public static T ReadJson(string filePath)
        {
            var jsonString = ReadAnyFileToString(filePath);
            return DeserializeJson(jsonString);
        }

        private static string ReadAnyFileToString(string filePath)
        {
            using var streamReader = new StreamReader(filePath);
            var allText = streamReader.ReadToEnd();
            return allText;
        }

        private static T DeserializeJson(string jsonString)
        {
            return JsonConvert.DeserializeObject<T>(jsonString);
        }
        
        public static void WriteJson(string filePath, T value)
        {
            var jsonString = SerializeJson(value);
            WriteStringToAnyFile(filePath, jsonString);
        }

        private static void WriteStringToAnyFile(string filePath, string text)
        {
            using var streamWriter = new StreamWriter(filePath);
            streamWriter.Write(text);
        }

        private static string SerializeJson(T value)
        {
            return JsonConvert.SerializeObject(value, Formatting.Indented,
                new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
        }
    }
}