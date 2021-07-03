namespace VoidBot.Services
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using System.Timers;
    using DSharpPlus;
    using DSharpPlus.Entities;
    using Newtonsoft.Json;
    using Void.Core.Entities;

    public class NetworkStatusService
    {
        private readonly Config config;
        private readonly DiscordShardedClient shardedBot;

        private DiscordMessage statusTile;
        public NetworkStatusService(Config _config, DiscordShardedClient _bot)
        {
            config = _config;
            shardedBot = _bot;
        }

        public async Task InitialiseAsync()
        {
            await CreateStatusTile();

            Timer updateTimer = new Timer(900000);

            updateTimer.Elapsed += OnUpdateStatus;
            updateTimer.AutoReset = true;
            updateTimer.Enabled = true;
        }

        private void OnUpdateStatus(object _sender, ElapsedEventArgs _e)
        {
            Task.WhenAll(CreateStatusTile());
        }

        private async Task CreateStatusTile()
        {
            await SetTileContent();
        }

        private async Task SetTileContent()
        {
            DiscordClient bot = shardedBot.GetShard(config.VoidwyrmDc.ServerId);
            DiscordGuild server = await bot.GetGuildAsync(config.VoidwyrmDc.ServerId);
            DiscordChannel status = server.GetChannel(config.VoidwyrmDc.StatusChannel);

            string json = new WebClient().DownloadString("https://voidwyrmapi.com/api/network-status");

            if (json.Equals(""))
            {
                return;
            }

            VoidwyrmStatus serverStatus = JsonConvert.DeserializeObject<VoidwyrmStatus>(json);


            IReadOnlyList<DiscordMessage> messages = await status.GetMessagesAsync();

            if (messages.Count != 0)
            {
                await status.DeleteMessagesAsync(messages);
            }

            DiscordMessageBuilder statusMessage = new DiscordMessageBuilder();

            if (serverStatus != null)
            {

                DiscordEmoji typeEmoji = serverStatus.Type switch
                {
                    "Operational" => DiscordEmoji.FromName(bot, ":white_check_mark:"),
                    "MinorOutage" => DiscordEmoji.FromName(bot, ":warning:"),
                    "MajorOutage" => DiscordEmoji.FromName(bot, ":x:"),
                    _ => DiscordEmoji.FromName(bot, ":x:")
                };



                DiscordEmbedBuilder statusEmbed = new DiscordEmbedBuilder
                {
                    Title = "Voidwyrm Server Status",
                    Color = DiscordColor.Black,
                    Description = $"{typeEmoji} **__{serverStatus.Type}__** {typeEmoji}\n\n" +
                                  $"{serverStatus.Reason}"
                };

                DiscordEmoji webHostEmoji = serverStatus.Servers.WebHost switch
                {
                    "Operational" => DiscordEmoji.FromName(bot, ":white_check_mark:"),
                    "Issues" => DiscordEmoji.FromName(bot, ":warning:"),
                    "Outage" => DiscordEmoji.FromName(bot, ":x:"),
                    _ => DiscordEmoji.FromName(bot, ":x:")
                };

                statusEmbed.AddField($"{webHostEmoji} Voidwyrm WebHost", "\u200B");

                DiscordEmoji dbCEmoji = serverStatus.Servers.DB_CA switch
                {
                    "Operational" => DiscordEmoji.FromName(bot, ":white_check_mark:"),
                    "Issues" => DiscordEmoji.FromName(bot, ":warning:"),
                    "Outage" => DiscordEmoji.FromName(bot, ":x:"),
                    _ => DiscordEmoji.FromName(bot, ":x:")
                };

                statusEmbed.AddField($"{dbCEmoji} Voidwyrm DB_CA", "\u200B");

                DiscordEmoji dbUEmoji = serverStatus.Servers.DB_US switch
                {
                    "Operational" => DiscordEmoji.FromName(bot, ":white_check_mark:"),
                    "Issues" => DiscordEmoji.FromName(bot, ":warning:"),
                    "Outage" => DiscordEmoji.FromName(bot, ":x:"),
                    _ => DiscordEmoji.FromName(bot, ":x:")
                };

                statusEmbed.AddField($"{dbUEmoji} Voidwyrm DB_US", "\u200B");

                DiscordEmoji dbEEmoji = serverStatus.Servers.DB_EU switch
                {
                    "Operational" => DiscordEmoji.FromName(bot, ":white_check_mark:"),
                    "Issues" => DiscordEmoji.FromName(bot, ":warning:"),
                    "Outage" => DiscordEmoji.FromName(bot, ":x:"),
                    _ => DiscordEmoji.FromName(bot, ":x:")
                };

                statusEmbed.AddField($"{dbEEmoji} Voidwyrm DB_EU", "\u200B");

                DiscordEmoji cdnCEmoji = serverStatus.Servers.CDN_CA switch
                {
                    "Operational" => DiscordEmoji.FromName(bot, ":white_check_mark:"),
                    "Issues" => DiscordEmoji.FromName(bot, ":warning:"),
                    "Outage" => DiscordEmoji.FromName(bot, ":x:"),
                    _ => DiscordEmoji.FromName(bot, ":x:")
                };

                statusEmbed.AddField($"{cdnCEmoji} Voidwyrm CDN_CA", "\u200B");

                DiscordEmoji cdnUEmoji = serverStatus.Servers.CDN_US switch
                {
                    "Operational" => DiscordEmoji.FromName(bot, ":white_check_mark:"),
                    "Issues" => DiscordEmoji.FromName(bot, ":warning:"),
                    "Outage" => DiscordEmoji.FromName(bot, ":x:"),
                    _ => DiscordEmoji.FromName(bot, ":x:")
                };

                statusEmbed.AddField($"{cdnUEmoji} Voidwyrm CDN_US", "\u200B");

                DiscordEmoji cdnEEmoji = serverStatus.Servers.CDN_EU switch
                {
                    "Operational" => DiscordEmoji.FromName(bot, ":white_check_mark:"),
                    "Issues" => DiscordEmoji.FromName(bot, ":warning:"),
                    "Outage" => DiscordEmoji.FromName(bot, ":x:"),
                    _ => DiscordEmoji.FromName(bot, ":x:")
                };

                statusEmbed.AddField($"{cdnEEmoji}Voidwyrm CDN_EU", "\u200B");

                statusEmbed.WithFooter($"Updated at {DateTime.Now}", "https://voidwyrmapi.com/assets/VoidwyrmIco.png");

                statusMessage.WithEmbed(statusEmbed);
            }

            //DiscordActionRowComponent buttonRow = new DiscordActionRowComponent(new DiscordComponent[]
            //                                                                    {
            //                                                                        new DiscordButtonComponent(ButtonStyle.Danger, "Status-Report_Outage", "Report Outage"),
            //                                                                        new DiscordLinkButtonComponent("https://voidwyrmapi.com/", "Voidwyrm Website")
            //                                                                    });



            statusMessage.AddComponents(new DiscordLinkButtonComponent("https://voidwyrmapi.com/", "Voidwyrm Website"));

            statusTile = await statusMessage.SendAsync(status);
        }
    }
}
