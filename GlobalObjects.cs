using Discord.Commands;
using MushicordRemake.Modules;

namespace MushicordRemake
{
    public static class GlobalObjects
    {
        public static CommandService Commands = new();
        public static MushicordManager MushicordManager;
    }
}