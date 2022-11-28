using Discord;
using Discord.WebSocket;

using SharpLink;
using SharpLink.Stats;

namespace MushicordRemake.Modules
{
    /// <summary>
    /// A module for managing Lavalink.
    /// </summary>
    public class MushicordManager
    {
        public LavalinkManager Manager { get; private set; }

        public LavalinkManagerConfig Config { get; set; }

        public MushicordManager(DiscordSocketClient client, bool verbose = true)
        {
            Manager = new(client);
        }

        public bool IsStarted { get; private set; } = false;

        public MushicordManager(DiscordSocketClient client, LavalinkManagerConfig configuration, bool verbose = true)
        {
            Manager = new(client, configuration);
            Config = configuration;
            if(verbose) 
            {
                Manager.Log += OnLog;
                Manager.Stats += OnStats;
            }
        }

        private Task OnStats(LavalinkStats updatedStats)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("STATS UPDATE RECEIVED: ");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write($"\nCPU: {Console.ForegroundColor = ConsoleColor.Green} {updatedStats.CPU} {Console.ForegroundColor = ConsoleColor.Blue}\nMemory: {Console.ForegroundColor = ConsoleColor.Green} {updatedStats.Memory} {Console.ForegroundColor = ConsoleColor.Blue}\nPlaying Players: {Console.ForegroundColor = ConsoleColor.Green} {updatedStats.PlayingPlayers} {Console.ForegroundColor = ConsoleColor.Blue}\nUptime: {Console.ForegroundColor = ConsoleColor.Green} {updatedStats.Uptime} {Console.ForegroundColor = ConsoleColor.Blue}");
            Console.ResetColor();
            return Task.CompletedTask;
        }

        private Task OnLog(LogMessage log)
        {
            Console.WriteLine(log);
            return Task.CompletedTask;
        }

        public async Task StartAsync()
        {
            IsStarted = true;
            await Manager.StartAsync();
        }

        public async Task StopAsync()
        {
            IsStarted = false;
            await Manager.StopAsync();
        }

        public async Task<LavalinkPlayer> GetPlayerAsync(ulong guildId, IVoiceChannel voiceChannel)
        {
            return Manager.GetPlayer(guildId) ?? await Manager.JoinAsync(voiceChannel);
        }

        public async Task<LoadTracksResponse> GetTracksAsync(string query)
        {
            return await Manager.GetTracksAsync(query);
        }
    }
}