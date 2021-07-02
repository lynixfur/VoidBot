namespace VoidBot.Commands
{
    using System.Threading.Tasks;
    using DSharpPlus;
    using DSharpPlus.Entities;
    using DSharpPlus.SlashCommands;

    public class SetupCommands : SlashCommandModule
    {
        [SlashCommand("Hello", "Say Hello to the Bot")]
        public async Task TestCommand(InteractionContext _ctx)
        {
            await _ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder { Content = "Hello", IsEphemeral = true });


        }
    }
}
