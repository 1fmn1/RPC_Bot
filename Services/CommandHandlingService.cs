using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using Microsoft.Extensions.DependencyInjection;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using RPC_Bot.Modules;
using System.Text.RegularExpressions;
using System.Linq;
using static RPC_Bot.Modules.Global_Variables;
using System.Collections.Generic;

namespace RPC_Bot.Services
{
    public class CommandHandlingService
    {
        private readonly CommandService _commands;
        private readonly DiscordSocketClient _discord;
        private readonly IServiceProvider _services;

        public CommandHandlingService(IServiceProvider services)
        {
            _commands = services.GetRequiredService<CommandService>();
            _discord = services.GetRequiredService<DiscordSocketClient>();
            _services = services;
            _discord.Ready += bot_Ready;
            // Hook CommandExecuted to handle post-command-execution logic.
            _commands.CommandExecuted += CommandExecutedAsync;
            // Hook MessageReceived so we can process each message to see
            // if it qualifies as a command.
            _discord.MessageReceived += MessageReceivedAsync;
        }

        public async Task InitializeAsync()
        {
            // Register modules that are public and inherit ModuleBase<T>.
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }

        public async Task MessageReceivedAsync(SocketMessage rawMessage)
        {
            // Ignore system messages, or messages from other bots
            if (!(rawMessage is SocketUserMessage message)) return;
            if (message.Source != MessageSource.User) return;
            if (message.MentionedRoles.Count > 0) await returnPing(message);

            // This value holds the offset where the prefix ends
            var argPos = 0;
            // Perform prefix check. You may want to replace this with
            // (!message.HasCharPrefix('!', ref argPos))
            // for a more traditional command format like !help.
            if (message.Content.Contains("roshpit.ca/market/auction/"))
            {
                AuctionNewClass new_auc;
                string helpstring;
                ItemClass helpitem = new ItemClass();
                helpstring = Regex.Matches(message.Content, @"(http|ftp|https)://([\w_-]+(?:(?:\.[\w_-]+)+))([\w.,@?^=%&:/~+#-]*[\w@?^=%&/~+#-])?")[0].Value;
                new_auc = RoshpitStats.GetOngoingAuction2(helpstring);
                helpitem = RoshpitStats.Search(new_auc.item_name_en, true);
                if (helpitem == null)
                {
                    UpdateTablesClass a = new UpdateTablesClass();
                    helpitem = a.LoadItem($"https://www.roshpit.ca/items/{new_auc.roshpititem_variant}");
                    using (AuctionContext acontext = new AuctionContext())
                    {
                        int mid = 0;
                        foreach (KeyValuePair<string,ItemClass> item in RoshpitStats.ItemList)
                        {
                            if (mid < item.Value.id) mid = item.Value.id;
                        }
                        helpitem.id = mid + 1;
                        acontext.Items.Add(helpitem);
                        acontext.SaveChanges();
                    }
                    RoshpitStats.ItemList.Add(helpitem.Name.ToLower(), helpitem);
                }
                if (helpitem == null) return;
                EmbedBuilder builder = new EmbedBuilder();
                builder.WithAuthor(new EmbedAuthorBuilder { IconUrl = message.Author.GetAvatarUrl(), Name = message.Author.Username });
                builder.WithTitle("Auction for **" + helpitem.Name + "**");
                builder.WithThumbnailUrl(helpitem.Image);
                builder.WithUrl(helpstring);
                //Rarity Color
                if (helpitem.Rarity == "Immortal") builder.WithColor(228, 174, 51);
                else if (helpitem.Rarity == "Arcana") builder.WithColor(173, 229, 92);
                else
                {
                    System.Drawing.Color col;
                    col = System.Drawing.ColorTranslator.FromHtml(helpitem.Rarity);
                    builder.WithColor(col.R, col.G, col.B);
                    builder.AddField(helpitem.Special_Ability, "Lv. " + helpitem.Required_level, true);
                }
                if (new_auc.level_req.HasValue) helpstring = $"Lv. {new_auc.level_req}\t";
                else helpstring = "";
                //Base Armor
                if ((new_auc.base_armor.HasValue) && (new_auc.base_magic_armor.HasValue) && (new_auc.base_armor > 0) && (new_auc.base_magic_armor > 0))
                    helpstring = helpstring + $"{emoArmor} {new_auc.base_armor}\t{emoMagicArmor} {new_auc.base_magic_armor}\r\n";
                else if (new_auc.base_armor.HasValue && new_auc.base_armor > 0)
                    helpstring = helpstring + $"{emoArmor} {new_auc.base_armor}\r\n";
                else if (new_auc.base_magic_armor.HasValue && new_auc.base_magic_armor > 0)
                    helpstring = helpstring + $"{emoMagicArmor} {new_auc.base_magic_armor}\r\n";

                //Sockets
                if (new_auc.socket1 == "none" || new_auc.socket1 == "") ;
                else if (new_auc.socket1 == "open")
                    helpstring = helpstring + $"{PingedEmotes.Find(c => c.ToString().Contains(new_auc.socket1))}";
                else
                    helpstring = helpstring + $"{PingedEmotes.Find(c => c.ToString().Contains($"{new_auc.socket1}{new_auc.socket1value}"))}";
                if (new_auc.socket2 == "none" || new_auc.socket2 == "") ;
                else if (new_auc.socket2 == "open")
                    helpstring = helpstring + $"{PingedEmotes.Find(c => c.ToString().Contains(new_auc.socket2))}";
                else
                    helpstring = helpstring + $"{PingedEmotes.Find(c => c.ToString().Contains($"{new_auc.socket2}{new_auc.socket2value}"))}";
                //Properties
                //Property1
                if (new_auc.property1name != "")
                {
                    if (new_auc.property1name.Contains("immortal*weapon"))
                    {
                        string hero;
                        hero = new_auc.roshpititem_variant.Split("_")[2];
                        if (hero == "axe") hero = "redgeneral";
                        helpstring = helpstring + $"★ {RoshpitEmotes.Find(c => c.ToString().Contains(hero))} **Weapon**\r\n";
                    }
                }
                else if (new_auc.property1name.Contains("arcana"))
                {
                    string hero;
                    hero = new_auc.roshpititem_variant.Split("_")[2];
                                if (hero == "axe") hero = "redgeneral";
                    helpstring = helpstring + $"★ {RoshpitEmotes.Find(c => c.ToString().Contains(hero))} **Arcana**\r\n";
                            }
                else if (new_auc.property1name.Contains ("immortal"))
                        helpstring = helpstring + $"★**{new_auc.property1name.Substring(21).Replace("_", " ")}**\r\n";
                else
                    helpstring = helpstring + $"**{new_auc.property1name.Replace("_", " ")}** \t{new_auc.property1}\r\n";
            //Property2 3 4 
            if (new_auc.property2name != "")
                helpstring = helpstring + $"**{new_auc.property2name.Replace("_", " ")}**\t{new_auc.property2}\r\n";
            if (new_auc.property3name != "")
                helpstring = helpstring + $"**{new_auc.property3name.Replace("_", " ")}**\t{new_auc.property3}\r\n";
            if (new_auc.property4name != "")
                helpstring = helpstring + $"**{new_auc.property4name.Replace("_", " ")}**\t{new_auc.property4}\r\n";
                if (new_auc.property1name != "")
                    builder.AddField("Properties:", helpstring, true);

                //Bids
                helpstring = "";
                string emote="";
                if (new_auc.resource_type == "Mithril Shards") emote = emoShard;
                if (new_auc.resource_type == "Arcane Crystals") emote = emoCrystal;
                if (new_auc.resource_type == "Prismatic Gemstones") emote = emoGem;

                if (new_auc.minimum_bid > 0)
                    helpstring = helpstring + $"\r\nStarting Bid:\t\t{emote}{new_auc.minimum_bid.ToString("#,##0")}";
                if (new_auc.current_bid > 0)
                    helpstring = helpstring + $"\r\nHighest Bid:\t\t{emote}{new_auc.current_bid.ToString("#,##0")}";
                        if (new_auc.buyout > 0)
                            helpstring = helpstring + $"\r\nBuyout:\t\t{emote}{new_auc.buyout.ToString("#,##0")}";
                        helpstring = helpstring +"\r\n\\_\\_\\_\\_\\_\\_\\_\\_";
                //Time

                double hours;
                hours = (new_auc.expiry_time - DateTime.Today).TotalHours - 12;
                helpstring = helpstring + $"\r\nExpiry:\t\t{hours.ToString("#,##0")} Hours\r\n";
                builder.AddField("Auction Details", helpstring, true);
                builder.Description = Regex.Replace(message.Content, @"(http|ftp|https)://([\w_-]+(?:(?:\.[\w_-]+)+))([\w.,@?^=%&:/~+#-]*[\w@?^=%&/~+#-])?", "");
                //
                //await message.DeleteAsync();
                await message.Channel.SendMessageAsync("", embed: builder.Build());
            }
            if ((!message.HasMentionPrefix(_discord.CurrentUser, ref argPos)) && (!message.HasCharPrefix('!', ref argPos))) return;
            if ((message.Channel.Id == 391268932696145920) || (message.Channel.Id == 391268981446803466) || (message.Channel.Id == 331554376181219350) || (message.Channel.Id == 299731610683572224))
            {
                await message.Channel.SendMessageAsync("<:MonkaS:663460636490989597> 👉 <#431938235267284992>");
                return;
            }
            var context = new SocketCommandContext(_discord, message);
            // Perform the execution of the command. In this method,
            // the command service will perform precondition and parsing check
            // then execute the command if one is matched.
            await _commands.ExecuteAsync(context, argPos, _services); 
            // Note that normally a result will be returned by this format, but here
            // we will handle the result in CommandExecutedAsync,
        }

        public async Task CommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            // command is unspecified when there was a search failure (command not found); we don't care about these errors
            if (!command.IsSpecified)
                return;

            // the command was successful, we don't care about this result, unless we want to log that a command succeeded.
            if (result.IsSuccess)
                return;

            // the command failed, let's notify the user that something happened.
            await context.Channel.SendMessageAsync($"error: {result}");
        }
        public async Task returnPing(SocketMessage rawMessage)
        {
            int a = rn.Next(0, PingedEmotes.Count);
            if (a > PingedEmotes.Count - 1)
                a = PingedEmotes.Count - 1;
            await rawMessage.Channel.SendMessageAsync(PingedEmotes[a].ToString());
        }
        private Task bot_Ready()
        {
            SocketGuild b;
            b = _discord.GetGuild(503160168482340865);
            Dictionary<string, GuildEmote> a;
            a = b.Emotes.ToDictionary(x => ":" + x.Name + ":");
            b = _discord.GetGuild(299728973380976651);
            RoshpitEmotes = b.Emotes.ToList();
            b = _discord.GetGuild(510201275468611584);
            PingedEmotes = b.Emotes.ToList();
            return Task.CompletedTask;
            //HangManGame = new HangManClass(RoshpitStats.ItemList.Keys.ToList, RoshpitStats.Heroes.Values.ToList, a);
        }
    }
}
