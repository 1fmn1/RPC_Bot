using System;
using System.Collections.Generic;
using System.Text;
using RPC_Bot.Services;

namespace RPC_Bot.Modules
{
     static class Global_Variables
    {
        public static Config conf;
        public const string SPLITTER = "¿";
        public static bool Running = true;
        public static Dictionary<ulong, DateTime> ignorelist = new Dictionary<ulong, DateTime>();



        public static RoshpitStatsClass RoshpitStats = new RoshpitStatsClass();
        public const ulong MedivhsGuild = 279649221517246464;
        public const string emoAkaShrug = "<:AkaShrug:280432292395089920>";
        public const string emoDoge = "<:Doge:280432351425593345>";
        public const string emoLeader = "<:2_:423135710724489223>";
        public const string emoShard = "<:mithril_shard:422449779016990730>";
        public const string emoCrystal = "<:arcane_crystal:422449737002778625>";
        public const string emoGem = "<:prismatic_gemstone:664282137721307156>";
        public const string emoArmor = "<:armor:662687696208265229>";
        public const string emoMagicArmor = "<:magic_armor:662687762914213888>";
        public static DateTime LastModified;
        public static bool UpdatesEnabled = true;
        public static bool UpdateIsRunning;
        public static Dictionary<ulong, RegisteredUserClass> RegisteredList = new Dictionary<ulong, RegisteredUserClass>();
        public static HangManClass HangManGame;
        public static bool renewRegisterList;
        public static Random rn = new Random();
        public static List<Discord.GuildEmote> PingedEmotes;
        public static List<Discord.GuildEmote> RoshpitEmotes = new List<Discord.GuildEmote>();


        //public const string RegisteredFile = @"d:\DZHosts\LocalUser\medivh015\www.Curator.somee.com\registrations.txt";
        //public const string IGNORE_FILENAME = @"d:\DZHosts\LocalUser\medivh015\www.Curator.somee.com\ignorelist.txt";
        //public const string LOG_FILE = @"d:\DZHosts\LocalUser\medivh015\www.Curator.somee.com\log.txt";
        //public const string MinersFile = @"d:\DZHosts\LocalUser\medivh015\www.Curator.somee.com\Miners.txt";
        //public const string statusFile = @"d:\DZHosts\LocalUser\medivh015\www.Curator.somee.com\Status.txt";

        //public const string MinersFile = @"h:\programming\RPC_BOT_WEB\RPC_BOT_WEB\Tables\miners.txt";
        //public const string statusFile = @"h:\programming\RPC_BOT_WEB\RPC_BOT_WEB\Tables\Status.txt";
        public const string LOG_FILE = @"Log.txt";
        //public const string RegisteredFile = @"h:\programming\RPC_BOT_WEB\RPC_BOT_WEB\Tables\registrations.txt";
        //public const string IGNORE_FILENAME = @"h:\programming\RPC_BOT_WEB\RPC_BOT_WEB\Tables\ignorelist.txt";
    }
}
