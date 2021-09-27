using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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
            string JsonString = ReadAnyFileToString(filePath);
            return DeserializeJSON(JsonString);
        }

        public static string ReadAnyFileToString(string filePath)
        {
            return File.ReadAllText(filePath);
        }
        public static T DeserializeJSON(string jsonString)
        {
            return JsonConvert.DeserializeObject<T>(jsonString);
        }



        public static void WriteJson(string filePath, T value)
        {
            string jsonString = SerializeJson(value);
            WriteStringToAnyFile(jsonString, filePath);
        }

        public static void WriteStringToAnyFile(string filePath, string text)
        {
            File.WriteAllText(filePath, text);
        }

        public static string SerializeJson(T value)
        {
            return JsonConvert.SerializeObject(value);
        }
    }
}
