namespace VoidBot.Services
{
    using System.IO;
    using Newtonsoft.Json;
    using VoidBot.Models;

    public static class FileService
    {
        public static Config GetConfig()
        {
            const string file = "./Config/Config.json";
            string data = File.ReadAllText(file);
            return JsonConvert.DeserializeObject<Config>(data);
        }
    }
}
