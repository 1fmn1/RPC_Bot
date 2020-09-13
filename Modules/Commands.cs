using System;
using System.IO;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using RPC_Bot.Services;
using static RPC_Bot.Modules.Serenguard_Module;
using static RPC_Bot.Modules.Global_Variables;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using Microsoft.VisualBasic;
using System.Linq;
using System.Collections.Generic;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Globalization;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Net;

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

        [Command("register")]
        public async Task registerAsync([Remainder] string text)
        {
            bool result;
            uint Userid;

            if (text.Contains("roshpit.ca") == false)
            {
                Discord.Rest.RestUserMessage mes;
                mes = (Discord.Rest.RestUserMessage)(await Context.Channel.GetMessageAsync(Context.Message.Id));
                await mes.AddReactionAsync(await Context.Client.GetGuild(279649221517246464).GetEmoteAsync(425332435773816842));
            }
            Uri link = new Uri("http://roshpit.ca");
            try
            {
                link = new Uri(text, UriKind.Absolute);
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
            }
            Userid = 0;
            result = uint.TryParse(link.Segments.Last(), out Userid);
            if (result)
            {
                if (RegisteredList.ContainsKey(Context.Message.Author.Id) == false)
                {
                    HtmlWeb web = new HtmlWeb();
                    HtmlDocument doc;
                    doc = web.Load($"https://www.roshpit.ca/players/{Userid}");
                    HtmlNode a = doc.DocumentNode.SelectSingleNode($"//span[@id={Strings.Chr(34)}index-title{Strings.Chr(34)}]");
                    if (a == null)
                        return;
                    RegisteredList.Add(Context.Message.Author.Id, new RegisteredUserClass() { DotaID = Userid, DiscordID = Context.Message.Author.Id, Name = (System.Net.WebUtility.HtmlDecode((a.InnerText.Replace(Constants.vbLf, "").Replace(Constants.vbTab, "")))).Trim() });
                    renewRegisterList = true;

                    using (UserContext cont = new UserContext())
                    {
                        cont.Users.UpdateRange(RegisteredList.Values.ToList());
                        await cont.SaveChangesAsync();
                    }
                    Discord.Rest.RestUserMessage mes;
                    mes = (Discord.Rest.RestUserMessage)(await Context.Message.Channel.GetMessageAsync(Context.Message.Id));
                    await mes.AddReactionAsync(await Context.Client.GetGuild(279649221517246464).GetEmoteAsync(425332394422173706));
                    return;
                }
                else
                {
                    Discord.Rest.RestUserMessage mes;
                    mes = (Discord.Rest.RestUserMessage)(await Context.Message.Channel.GetMessageAsync(Context.Message.Id));
                    await mes.AddReactionAsync(await Context.Client.GetGuild(279649221517246464).GetEmoteAsync(425332435773816842));
                    // Dim dmchan As SocketDMChannel = Await arg.Author.GetOrCreateDMChannelAsync()
                    await ReplyAsync($"Sorry {Context.Message.Author.Mention}, you already registered as [{RegisteredList[Context.Message.Author.Id].Name}]({RegisteredList[Context.Message.Author.Id].DotaID}). If you made a mistake in ID, send PM to {(await Context.Message.Channel.GetUserAsync(216861458196201484)).Mention}");
                    return;
                }
            }
            else
            {
                Discord.Rest.RestUserMessage mes;
                mes = (Discord.Rest.RestUserMessage)(await Context.Message.Channel.GetMessageAsync(Context.Message.Id));
                await mes.AddReactionAsync(await Context.Client.GetGuild(279649221517246464).GetEmoteAsync(425332435773816842));
                // Dim dmchan As SocketDMChannel = Await arg.Author.GetOrCreateDMChannelAsync
                await ReplyAsync($"You are trying to register wrong roshpit.ca profile link. Check your information And try again.");
                return;
            }
        }

        [Command("hangman")]
        public async Task HangmanAsync()
        {
            HangManGame.NewGame(Context.Message);
        }
        [Command("htop")]
        public async Task HtopAsync()
        {
            string helpstring;
            List<KeyValuePair<ulong, RegisteredUserClass>> alist;
            List<SocketUser> blist = new List<SocketUser>();

            EmbedBuilder builder = new EmbedBuilder();
            builder.WithTitle($"Hangman top.");
            builder.WithColor(Discord.Color.DarkOrange);
            helpstring = "";
            alist = RegisteredList.ToList();
            alist.Sort((y, x) => x.Value.HangManScore.CompareTo(y.Value.HangManScore));
            for (var i = 0; i <= 9; i++)
                helpstring = helpstring + $"{i + 1}. <@{alist[i].Key}>" + Constants.vbCrLf;
            builder.AddField("Name", helpstring, true);
            helpstring = "";
            for (var i = 0; i <= 9; i++)
                helpstring = helpstring + alist[i].Value.HangManScore + Constants.vbCrLf;
            builder.AddField("Points", helpstring, true);
            await (ReplyAsync("", embed: builder.Build()));

        }

        [Command("guess")]
        public async Task GuessAsync([Remainder] string text)
        {
            if (HangManGame.GameInstances.ContainsKey(Context.Message.Author))
            {
                if (HangManGame.GameInstances[Context.Message.Author].GameEnded)
                {
                    await ReplyAsync("Your game has ended.");
                    return;
                }
                await HangManGame.GameInstances[Context.Message.Author].Guess(text.First());
                if (Context.IsPrivate == false)
                    await Context.Message.DeleteAsync();
            }
            else
                await ReplyAsync("Use ?hangman to start a game first.");

        }

        [Command("word")]
        public async Task WordAsync([Remainder] string text)
        {
            if (HangManGame.GameInstances.ContainsKey(Context.Message.Author))
            {
                if (HangManGame.GameInstances[Context.Message.Author].GameEnded)
                {
                    await ReplyAsync("Your game has ended.");
                    return;
                }
                await HangManGame.GameInstances[Context.Message.Author].GuessWord(text);
                if (Context.IsPrivate == false)
                    await Context.Message.DeleteAsync();
            }
            else
                await ReplyAsync("Use ?hangman to start a game first.");
        }
        [Command("top")]
        public async Task AsyncTop()
        {
            WebClient wc = new WebClient();
            string a;
            TopClass[] mytop;
            EmbedBuilder builder = new EmbedBuilder();
            CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");
            string helpstring = "";

            a= wc.DownloadString("https://www.roshpit.ca/champions/API/richest_players");
            mytop = Newtonsoft.Json.JsonConvert.DeserializeObject<TopClass[]>(a);
            builder.WithTitle($"TOP RICHEST PLAYERS");
            builder.WithColor(Discord.Color.Blue);
            builder.WithThumbnailUrl("https://b.catgirlsare.sexy/ceO7.png");
            string[] aname = new string [9];
            for (int i = 0; i <= 4; i++)
            {
                foreach (RegisteredUserClass reguser in RegisteredList.Values)
                {
                    if (reguser.DotaID == (ulong)mytop[i].steam_id)
                        aname[i] = reguser.Name;
                }


                if (aname[i] == "")
                    aname[i] = mytop[i].steam_id.ToString();
                string s;
                s = aname[i];
                helpstring = helpstring + $"{i + 1}. {$"[{s}](http://www.roshpit.ca/players/{mytop[i].steam_id})"}{Constants.vbTab}{RoshpitStats.Heroes["prismatic_gemstone"]}{mytop[i].prismatic_gemstones.ToString("N0", culture)}{Constants.vbTab}{RoshpitStats.Heroes["mithril_shard"]}{mytop[i].mithril_shards.ToString("N0", culture)}{Constants.vbTab}{RoshpitStats.Heroes["arcane_crystal"]}{mytop[i].arcane_crystals.ToString("N0", culture)}" + Constants.vbCrLf + Constants.vbCrLf;

            }
            builder.WithDescription(helpstring);
            builder.WithFooter($"Requested by {Context.Message.Author.Username}");
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
