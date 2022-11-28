using System.Reflection;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

using Microsoft.Extensions.DependencyInjection;

using MushicordRemake.Modules;

using SharpLink;

namespace MushicordRemake
{
    class Program
    {
        private DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;

        private DiscordSocketConfig _clientConfig = new()
        {
            AlwaysDownloadUsers = true,
            LogGatewayIntentWarnings = false,
            GatewayIntents = GatewayIntents.All,
            MessageCacheSize = 100
        };

        public static void Main(string[] args)
        {
            Console.Clear();
            new Program().MainAsync().GetAwaiter().GetResult();
        }

        private async Task MainAsync()
        {
            _client = new(_clientConfig);
            _commands = new();

            GlobalObjects.MushicordManager = new(_client, false);

            _services = new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_commands)
                .BuildServiceProvider();

            await RegisterCommandsAsync();

            _client.Log += OnClientLog;
            _client.Ready += OnClientReady;

            string token = await File.ReadAllTextAsync(Path.Join(new string[]
            {
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                "Discord Bot",
                "token.txt",
            }));

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            // Don't exit until the user exits the program (for example, hitting the KeyboardInterrupt keybind)
            await Task.Delay(-1);
        }

        private async Task OnClientReady()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("The client is ready!");
            Console.ResetColor();

            // Start manager so we can use Lavalink
            await GlobalObjects.MushicordManager.StartAsync();
        }

        private Task OnClientLog(LogMessage arg)
        {
            Console.WriteLine(arg);
            return Task.CompletedTask;
        }

        private async Task RegisterCommandsAsync()
        {
            _client.MessageReceived += HandleCommandAsync;
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
            await GlobalObjects.Commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }

        private async Task HandleCommandAsync(SocketMessage arg)
        {
            SocketUserMessage? message = arg as SocketUserMessage;
            if(message is null) return;

            SocketCommandContext context = new(_client, message);

            int argPos = 0;

            if(!message.HasStringPrefix("mushi!", ref argPos)) return;

            IResult result = await _commands.ExecuteAsync(context, argPos, _services);

            if(!result.IsSuccess)
            {
                if(result.Error.Equals(CommandError.UnmetPrecondition))
                {
                    await message.ReplyAsync(result.ErrorReason);
                    return;
                }
                else
                {
                    await message.ReplyAsync($"An error occurred, please report this to the developer(s)!\n**Error:** ||{result.ErrorReason}||");
                    return;
                }
            }
        }
    }
}