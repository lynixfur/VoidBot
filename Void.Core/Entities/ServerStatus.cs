namespace Void.Core.Entities
{
    public class ServerStatus
    {
        public string WebHost { get; set; }
        public string DB_CA { get; set; }
        public string DB_US { get; set; }
        public string DB_EU { get; set; }
        public string CDN_CA { get; set; }
        public string CDN_US { get; set; }
        public string CDN_EU { get; set; }
    }
}