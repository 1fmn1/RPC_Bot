using System;
using System.IO;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using RPC_Bot.Services;
using static RPC_Bot.Modules.Serenguard_Module;

namespace RPC_Bot.Modules
{
    // Modules must be public and inherit from an IModuleBase
    public class Commands : ModuleBase<SocketCommandContext>
    {
        DateTime lastModifiedSeren= new DateTime();
        // Dependency Injection will fill this value in for us
        public PictureService PictureService { get; set; }

        [Command("faq")]
        public Task FaqAsync()
    => ReplyAsync("https://roshpit.fandom.com/wiki/FAQ");

        [Command("seren7")]
        public Task Seren7Async()
        {
    if (lastModifiedSeren.AddMinutes(5) < DateTime.Now)
        lastModifiedSeren = RoshpitStats.GetSerenTop;


    EmbedBuilder builder = new EmbedBuilder()
    {
        Title = "Serengaard Weekly Top 10"
    };

        builder.WithColor(Color.DarkGreen);
    builder.WithFooter($"Requested by {Context.User.Username}. Updated  { (DateTime.Now - lastModifiedSeren).TotalSeconds} seconds ago.");
    int counter=0;
    foreach (Seven item in RoshpitStats.Seren_Top.seven)
    {
        string strNames;
        string strHeroes;
        strNames = "";
        strHeroes = "";
        // If counter = 9 Then Exit For
        counter += 1;
        strHeroes = $"{counter} place. Wave {item.wave_number}";
        if ((item.steam_name1) == null)
        {
        }
        else
            strNames = strNames + $"{RoshpitStats.Heroes(item.hero1)} [{item.steam_name1}](http://www.roshpit.ca/players/{item.steam_id1})" + "\r\n";
        if ((item.steam_name2) == null)
                {
        }
        else
            strNames = strNames + $"{RoshpitStats.Heroes(item.hero2)} [{item.steam_name2}](http://www.roshpit.ca/players/{item.steam_id2})" + "\r\n";
        if ((item.steam_name3) == null)
                {
        }
        else
            strNames = strNames + $"{RoshpitStats.Heroes(item.hero3)} [{item.steam_name3}](http://www.roshpit.ca/players/{item.steam_id3})" + "\r\n";
        if ((item.steam_name4) == null)
                {
        }
        else
            strNames = strNames + $"{RoshpitStats.Heroes(item.hero4)} [{item.steam_name4}](http://www.roshpit.ca/players/{item.steam_id4})" + "\r\n";
        if (strHeroes == "")
            strHeroes = "Unknown Heroes";
        if (strNames == "")
            strNames = "Unknown Team";
        builder.AddField(strHeroes, strNames, true);
    }
    // builder.WithDescription(strNames)
    return ReplyAsync("", embed: builder.Build());
}


[Command("ping")]
        [Alias("pong", "hello")]
        public Task PingAsync()
            => ReplyAsync("pong!");

        [Command("cat")]
        public async Task CatAsync()
        {
            // Get a stream containing an image of a cat
            var stream = await PictureService.GetCatPictureAsync();
            // Streams must be seeked to their beginning before being uploaded!
            stream.Seek(0, SeekOrigin.Begin);
            await Context.Channel.SendFileAsync(stream, "cat.png");
        }

        // Get info on a user, or the user who invoked the command if one is not specified
        [Command("userinfo")]
        public async Task UserInfoAsync(IUser user = null)
        {
            user = user ?? Context.User;

            await ReplyAsync(user.ToString());
        }

        // Ban a user
        [Command("ban")]
        [RequireContext(ContextType.Guild)]
        // make sure the user invoking the command can ban
        [RequireUserPermission(GuildPermission.BanMembers)]
        // make sure the bot itself can ban
        [RequireBotPermission(GuildPermission.BanMembers)]
        public async Task BanUserAsync(IGuildUser user, [Remainder] string reason = null)
        {
            await user.Guild.AddBanAsync(user, reason: reason);
            await ReplyAsync("ok!");
        }

        // [Remainder] takes the rest of the command's arguments as one argument, rather than splitting every space
        [Command("echo")]
        public Task EchoAsync([Remainder] string text)
            // Insert a ZWSP before the text to prevent triggering other bots!
            => ReplyAsync('\u200B' + text);

        // 'params' will parse space-separated elements into a list
        [Command("list")]
        public Task ListAsync(params string[] objects)
            => ReplyAsync("You listed: " + string.Join("; ", objects));

        // Setting a custom ErrorMessage property will help clarify the precondition error
        [Command("guild_only")]
        [RequireContext(ContextType.Guild, ErrorMessage = "Sorry, this command must be ran from within a server, not a DM!")]
        public Task GuildOnlyCommand()
            => ReplyAsync("Nothing to see here!");
    }
}