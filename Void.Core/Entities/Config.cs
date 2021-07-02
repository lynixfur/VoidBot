namespace Void.Core.Entities
{
    using System.Collections.Generic;

    public class Config
    {
        public DiscordConfig DiscordConfig { get; set; }
        public List<MySql> MySql { get; set; }
    }
}
