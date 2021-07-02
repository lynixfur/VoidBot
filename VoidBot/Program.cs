namespace VoidBot
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using DSharpPlus;
    using DSharpPlus.Interactivity;
    using DSharpPlus.Interactivity.Extensions;
    using DSharpPlus.SlashCommands;
    using Microsoft.Extensions.DependencyInjection;
    using Void.Core.Entities;
    using VoidBot.Commands;
    using VoidBot.Services;

    internal class Program
    {
        private DiscordShardedClient bot;
        private Config config;

        private static void Main()
        {
            Program program = new Program();
            program.CreateBotAsync().GetAwaiter().GetResult();
        }

        private async Task CreateBotAsync()
        {
            // Get Config
            config = FileService.GetConfig();

            // Initialise Bot
            bot = new DiscordShardedClient(new DiscordConfiguration
            {
                Token = config.DiscordConfig.Token,
                TokenType = TokenType.Bot
            });

            // Initialise Interactivity
            await bot.UseInteractivityAsync(new InteractivityConfiguration
            {
                Timeout = TimeSpan.FromMinutes(5)
            });

            // Register Services
            IServiceProvider services = ConfigureServices();

            // Create Commands
            IReadOnlyDictionary<int, SlashCommandsExtension> commands = await bot.UseSlashCommandsAsync(new SlashCommandsConfiguration
                                                                                                        {
                                                                                                            Services = services
                                                                                                        });

            // Register Commands
            foreach (SlashCommandsExtension slashCommandsExtension in commands.Values)
            {
                slashCommandsExtension.RegisterCommands<SetupCommands>(851958001739759677);
            }

            //login
            await bot.StartAsync();
            await Task.Delay(Timeout.Infinite);
        }

        private IServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                   .AddSingleton(bot)
                   .AddSingleton(config)
                   .BuildServiceProvider();
        }
    }
}
