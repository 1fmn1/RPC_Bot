using System;
using System.IO;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using RPC_Bot.Services;
using static RPC_Bot.Modules.Serenguard_Module;
using static RPC_Bot.Modules.Global_Variables;
using System.Text.RegularExpressions;

namespace RPC_Bot.Modules
{
    // Modules must be public and inherit from an IModuleBase
    public class Commands : ModuleBase<SocketCommandContext>
    {
        DateTime lastModifiedSeren = new DateTime();
        // Dependency Injection will fill this value in for us
        public PictureService PictureService { get; set; }

        [Command("faq")]
        public Task FaqAsync()
    => ReplyAsync("https://roshpit.fandom.com/wiki/FAQ");

        [Command("seren7")]
        public Task Seren7Async()
        {
            if (lastModifiedSeren.AddMinutes(5) < DateTime.Now)
                lastModifiedSeren = RoshpitStats.GetSerenTop();


            EmbedBuilder builder = new EmbedBuilder()
            {
                Title = "Serengaard this week Top 10"
            };

            builder.WithColor(Color.DarkGreen);
            builder.WithFooter($"Requested by {Context.User.Username}. Updated  { (DateTime.Now - lastModifiedSeren).TotalSeconds} seconds ago.");
            int counter = 0;
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
                    strNames = strNames + $"{RoshpitStats.Heroes[item.hero1]} [{item.steam_name1}](http://www.roshpit.ca/players/{item.steam_id1})" + "\r\n";
                if ((item.steam_name2) == null)
                {
                }
                else
                    strNames = strNames + $"{RoshpitStats.Heroes[item.hero2]} [{item.steam_name2}](http://www.roshpit.ca/players/{item.steam_id2})" + "\r\n";
                if ((item.steam_name3) == null)
                {
                }
                else
                    strNames = strNames + $"{RoshpitStats.Heroes[item.hero3]} [{item.steam_name3}](http://www.roshpit.ca/players/{item.steam_id3})" + "\r\n";
                if ((item.steam_name4) == null)
                {
                }
                else
                    strNames = strNames + $"{RoshpitStats.Heroes[item.hero4]} [{item.steam_name4}](http://www.roshpit.ca/players/{item.steam_id4})" + "\r\n";
                if (strHeroes == "")
                    strHeroes = "Unknown Heroes";
                if (strNames == "")
                    strNames = "Unknown Team";
                builder.AddField(strHeroes, strNames, true);
            }
            // builder.WithDescription(strNames)
            return ReplyAsync("", embed: builder.Build());
        }


        [Command("seren30")]
        public Task Seren30Async()
        {
            if (lastModifiedSeren.AddMinutes(5) < DateTime.Now)
                lastModifiedSeren = RoshpitStats.GetSerenTop();


            EmbedBuilder builder = new EmbedBuilder()
            {
                Title = "Serengaard this month Top 10"
            };

            builder.WithColor(Color.DarkGreen);
            builder.WithFooter($"Requested by {Context.User.Username}. Updated  { (DateTime.Now - lastModifiedSeren).TotalSeconds} seconds ago.");
            int counter = 0;
            foreach (ThisMonth item in RoshpitStats.Seren_Top.thisMonth)
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
                    strNames = strNames + $"{RoshpitStats.Heroes[item.hero1]} [{item.steam_name1}](http://www.roshpit.ca/players/{item.steam_id1})" + "\r\n";
                if ((item.steam_name2) == null)
                {
                }
                else
                    strNames = strNames + $"{RoshpitStats.Heroes[item.hero2]} [{item.steam_name2}](http://www.roshpit.ca/players/{item.steam_id2})" + "\r\n";
                if ((item.steam_name3) == null)
                {
                }
                else
                    strNames = strNames + $"{RoshpitStats.Heroes[item.hero3]} [{item.steam_name3}](http://www.roshpit.ca/players/{item.steam_id3})" + "\r\n";
                if ((item.steam_name4) == null)
                {
                }
                else
                    strNames = strNames + $"{RoshpitStats.Heroes[item.hero4]} [{item.steam_name4}](http://www.roshpit.ca/players/{item.steam_id4})" + "\r\n";
                if (strHeroes == "")
                    strHeroes = "Unknown Heroes";
                if (strNames == "")
                    strNames = "Unknown Team";
                builder.AddField(strHeroes, strNames, true);
            }
            // builder.WithDescription(strNames)
            return ReplyAsync("", embed: builder.Build());
        }


        [Command("seren")]
        public Task SerenAsync()
        {
            if (lastModifiedSeren.AddMinutes(5) < DateTime.Now)
                lastModifiedSeren = RoshpitStats.GetSerenTop();


            EmbedBuilder builder = new EmbedBuilder()
            {
                Title = "Serengaard all time Top 10"
            };

            builder.WithColor(Color.DarkGreen);
            builder.WithFooter($"Requested by {Context.User.Username}. Updated  { (DateTime.Now - lastModifiedSeren).TotalSeconds} seconds ago.");
            int counter = 0;
            foreach (AllTime item in RoshpitStats.Seren_Top.allTime)
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
                    strNames = strNames + $"{RoshpitStats.Heroes[item.hero1]} [{item.steam_name1}](http://www.roshpit.ca/players/{item.steam_id1})" + "\r\n";
                if ((item.steam_name2) == null)
                {
                }
                else
                    strNames = strNames + $"{RoshpitStats.Heroes[item.hero2]} [{item.steam_name2}](http://www.roshpit.ca/players/{item.steam_id2})" + "\r\n";
                if ((item.steam_name3) == null)
                {
                }
                else
                    strNames = strNames + $"{RoshpitStats.Heroes[item.hero3]} [{item.steam_name3}](http://www.roshpit.ca/players/{item.steam_id3})" + "\r\n";
                if ((item.steam_name4) == null)
                {
                }
                else
                    strNames = strNames + $"{RoshpitStats.Heroes[item.hero4]} [{item.steam_name4}](http://www.roshpit.ca/players/{item.steam_id4})" + "\r\n";
                if (strHeroes == "")
                    strHeroes = "Unknown Heroes";
                if (strNames == "")
                    strNames = "Unknown Team";
                builder.AddField(strHeroes, strNames, true);
            }
            // builder.WithDescription(strNames)
            return ReplyAsync("", embed: builder.Build());
        }

        [Command("rpc")]
        public async Task rpcAsync([Remainder] string text)
        {
            ItemClass helpItem;
            String helpstring = "";

            if (text.Length < 3) return;
            helpItem = RoshpitStats.Search(text);
            if (helpItem.Name == null)
                await ReplyAsync($"Sorry, I don't know anything about this.{emoAkaShrug}");
            EmbedBuilder builder = new EmbedBuilder();
            builder.WithFooter($"Requested by {Context.User.Username}.");
            builder.WithTitle("**" + helpItem.Name + "**");
            builder.WithThumbnailUrl(helpItem.Image);
            if (helpItem.AlternativeName.Contains("https:") || helpItem.AlternativeName.Contains("http:"))
                builder.WithUrl(helpItem.AlternativeName);
            else
                builder.WithUrl("http://www.roshpit.ca/items/item_rpc_" + helpItem.AlternativeName);

            if (helpItem.Rarity == "Immortal")
            {
                builder.WithColor(228, 174, 51);
                builder.AddField(helpItem.Rarity, helpItem.Slot, true);
            }
            else if (helpItem.Rarity == "Arcana")
            {
                builder.WithColor(173, 229, 92);
                builder.AddField(helpItem.Rarity, helpItem.Slot, true);
            }
            else
            {
                System.Drawing.Color col;
                col = System.Drawing.ColorTranslator.FromHtml(helpItem.Rarity);
                builder.WithColor(col.R, col.G, col.B);
                builder.AddField("Lv. " + helpItem.Required_level, helpItem.Slot, true);
            }
            if (helpItem.Rolls.Count > 0)
            {
                foreach (string roll in helpItem.Rolls)
                {
                    if (roll.Contains("Property"))
                    {
                        Match mat = Regex.Match(roll, "\\w+Property\\d");
                        string roll2;
                        roll2 = Regex.Replace(roll, mat.Value, $"[{mat.Value}](http://roshpit.wikia.com/wiki/{mat.Value})");
                        helpstring = helpstring + "■    " + roll2 + "\r\n";
                    }
                    else helpstring = helpstring + "■    " + roll + "\r\n";
                }
                builder.AddField("Rolls", helpstring, true);
            }
            if (helpItem.Special_Ability_Description != "")
            {
                if (helpItem.Special_Ability_Element == "")
                    builder.AddField("★" + helpItem.Special_Ability, helpItem.Special_Ability_Description, false);
                else
                    builder.AddField("★" + helpItem.Special_Ability, $"__Damage type__         **{helpItem.Special_Ability_Damage_Type}**\r\n\t"
                                  + $"__Element__               **{helpItem.Special_Ability_Element}**\r\n\r\n" + helpItem.Special_Ability_Description, true);
             }
            await ReplyAsync("", embed: builder.Build());
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
