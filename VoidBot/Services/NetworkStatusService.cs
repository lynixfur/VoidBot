namespace VoidBot.Services
{
    using System.Collections.Generic;
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
            Task.WhenAll(UpdateStatusTile());
        }

        private async Task CreateStatusTile()
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
                    "Operational" => DiscordEmoji.FromUnicode(bot, ":cloud_check:"),
                    "MinorOutage" => DiscordEmoji.FromUnicode(bot, ":cloud_error~1:"),
                    "MajorOutage" => DiscordEmoji.FromUnicode(bot, ":cloud_error:"),
                    _ => DiscordEmoji.FromUnicode(bot, ":cloud_error:")
                };

                DiscordEmbedBuilder statusEmbed = new DiscordEmbedBuilder
                {
                    Title = "Voidwyrm Server Status",
                    Color = DiscordColor.Black,
                    Description = $"{typeEmoji}**__{serverStatus.Type}__**{typeEmoji}\n\n" +
                                  $"{serverStatus.Reason}"
                };

                DiscordEmoji webHostEmoji = serverStatus.Type switch
                {
                    "Operational" => DiscordEmoji.FromName(bot, ":cloud_check:"),
                    "Issues" => DiscordEmoji.FromName(bot, ":cloud_error~1:"),
                    "Outage" => DiscordEmoji.FromName(bot, ":cloud_error:"),
                    _ => DiscordEmoji.FromName(bot, ":cloud_error:")
                };

                statusEmbed.AddField($"{webHostEmoji}Voidwyrm WebHost", "");

                DiscordEmoji dbCEmoji = serverStatus.Type switch
                {
                    "Operational" => DiscordEmoji.FromName(bot, ":cloud_check:"),
                    "Issues" => DiscordEmoji.FromName(bot, ":cloud_error~1:"),
                    "Outage" => DiscordEmoji.FromName(bot, ":cloud_error:"),
                    _ => DiscordEmoji.FromName(bot, ":cloud_error:")
                };

                statusEmbed.AddField($"{dbCEmoji}Voidwyrm DB_CA", "");

                DiscordEmoji dbUEmoji = serverStatus.Type switch
                {
                    "Operational" => DiscordEmoji.FromName(bot, ":cloud_check:"),
                    "Issues" => DiscordEmoji.FromName(bot, ":cloud_error~1:"),
                    "Outage" => DiscordEmoji.FromName(bot, ":cloud_error:"),
                    _ => DiscordEmoji.FromName(bot, ":cloud_error:")
                };

                statusEmbed.AddField($"{dbUEmoji}Voidwyrm DB_CA", "");

                DiscordEmoji dbEEmoji = serverStatus.Type switch
                {
                    "Operational" => DiscordEmoji.FromName(bot, ":cloud_check:"),
                    "Issues" => DiscordEmoji.FromName(bot, ":cloud_error~1:"),
                    "Outage" => DiscordEmoji.FromName(bot, ":cloud_error:"),
                    _ => DiscordEmoji.FromName(bot, ":cloud_error:")
                };

                statusEmbed.AddField($"{dbEEmoji}Voidwyrm DB_CA", "");

                DiscordEmoji cdnCEmoji = serverStatus.Type switch
                {
                    "Operational" => DiscordEmoji.FromName(bot, ":cloud_check:"),
                    "Issues" => DiscordEmoji.FromName(bot, ":cloud_error~1:"),
                    "Outage" => DiscordEmoji.FromName(bot, ":cloud_error:"),
                    _ => DiscordEmoji.FromName(bot, ":cloud_error:")
                };

                statusEmbed.AddField($"{cdnCEmoji}Voidwyrm DB_CA", "");

                DiscordEmoji cdnUEmoji = serverStatus.Type switch
                {
                    "Operational" => DiscordEmoji.FromName(bot, ":cloud_check:"),
                    "Issues" => DiscordEmoji.FromName(bot, ":cloud_error~1:"),
                    "Outage" => DiscordEmoji.FromName(bot, ":cloud_error:"),
                    _ => DiscordEmoji.FromName(bot, ":cloud_error:")
                };

                statusEmbed.AddField($"{cdnUEmoji}Voidwyrm DB_CA", "");

                DiscordEmoji cdnEEmoji = serverStatus.Type switch
                {
                    "Operational" => DiscordEmoji.FromName(bot, ":cloud_check:"),
                    "Issues" => DiscordEmoji.FromName(bot, ":cloud_error~1:"),
                    "Outage" => DiscordEmoji.FromName(bot, ":cloud_error:"),
                    _ => DiscordEmoji.FromName(bot, ":cloud_error:")
                };

                statusEmbed.AddField($"{cdnEEmoji}Voidwyrm DB_CA", "");

                statusMessage.WithEmbed(statusEmbed);
            }

            statusMessage.AddComponents(new DiscordButtonComponent
                                        {
                                            Label = "Report Outage",
                                            Style = ButtonStyle.Danger
                                        });

            statusMessage.AddComponents(new DiscordButtonComponent
                                        {
                                            Label = "Website",
                                            Style = ButtonStyle.Primary
                                        });

            await statusMessage.SendAsync(status);
        }

        private async Task UpdateStatusTile()
        {

        }
    }
}
