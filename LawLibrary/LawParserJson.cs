using System.Text.Json;

namespace LawLibrary
{
    /// <summary>
    /// Representa o objeto Law em um formato Json.
    /// </summary>
    internal static class LawParserJson
    {
        public static void Save(LawParser parser, string outputfile, bool indent)
        {
            JsonSerializerOptions options = new JsonSerializerOptions();            
            options.WriteIndented = indent;
            string json = JsonSerializer.Serialize(parser.Law, options);

            using FileStream fs = File.Create(outputfile);
            fs.Close();

            File.AppendAllText(outputfile, json);
        }

        public static Law? Load(string outputfile)
        {
            //return JsonSerializer.Deserialize<Law>(outputfile);

            if (!File.Exists(outputfile))
                return null;

            string fileContent;
            using StreamReader sr = new StreamReader(outputfile);

            fileContent = sr.ReadToEnd();

            return JsonSerializer.Deserialize<Law>(fileContent);
        }

        public static async Task<Law?> LoadAsync(string outputfile)
        {
            if (!File.Exists(outputfile))
                return null;
            
            using Stream sr = new FileStream(outputfile, FileMode.Open, FileAccess.Read, FileShare.Read);
            return await JsonSerializer.DeserializeAsync<Law>(sr);
        }
    }
}