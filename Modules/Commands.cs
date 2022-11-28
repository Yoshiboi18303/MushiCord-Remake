using Discord;
using Discord.Commands;
using Discord.WebSocket;
using SharpLink;

namespace MushicordRemake.Modules
{
    public class Commands : ModuleBase<SocketCommandContext>
    {

        // TODO: Finish this
        [Command("help")]
        public async Task Help(string? command = null)
        {
            if(command is not null)
            {
                bool commandExists = false;
                foreach(var info in GlobalObjects.Commands.Commands)
                {
                    if(info.Name == command) 
                    {
                        commandExists = true;
                        break;
                    }
                }
            }
        }

        [Command("play")]
        public async Task Play(string? query = null)
        {
            IVoiceChannel? voiceChannel = null;
            foreach(SocketVoiceChannel channel in Context.Guild.VoiceChannels)
            {
                if(channel.Users.Contains(Context.User))
                {
                    voiceChannel = channel;
                    break;
                }
            }

            if(voiceChannel is null)
            {
                Embed noVoiceEmbed = new EmbedBuilder()
                    .WithColor(255, 0, 0)
                    .WithDescription("Please join a voice channel to use this command!")
                    .Build();
                
                await ReplyAsync(embed: noVoiceEmbed);
                return;
            }

            LavalinkPlayer player = await GlobalObjects.MushicordManager.GetPlayerAsync(Context.Guild.Id, voiceChannel);

            LoadTracksResponse res = await GlobalObjects.MushicordManager.GetTracksAsync($"ytsearch:{query}");
            
            LavalinkTrack track = res.Tracks.First();
            await player.PlayAsync(track);
        }
    }
}