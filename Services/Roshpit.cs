using System;
using System.Collections.Generic;
using System.Text;

namespace RPC_Bot.Services
{
    using System;
    //using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Security;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.VisualBasic;
    using System.Text.RegularExpressions;
    using System.Net;
    using HtmlAgilityPack;
    using Newtonsoft.Json;
    using System.Runtime;
    using RPC_Bot.Modules;
    using static RPC_Bot.Modules.Serenguard_Module;
    using static RPC_Bot.Modules.Global_Variables;


        public class RoshpitStatsClass
        {
        public bool ListShowingAuctions;
        public Dictionary<string, ItemClass> ItemList = new Dictionary<string, ItemClass>();
        public Dictionary<int, UserNewClass> Users = new Dictionary<int, UserNewClass>();
        public  Dictionary<string, string> Heroes = new Dictionary<string, string>();
        public int auction_counter;
        public string Global_Stats_String;
        public List<UserNewClass> Leaderboard;
         public Serenguard_Top Seren_Top;
            public const string SPLITTER_CHAR = "¿";
        public Dictionary<string, List<string>> RollsList = new Dictionary<string, List<string>>();


            public void Load()
            {
                Heroes.Clear();
                Heroes.Add("npc_dota_hero_dragon_knight", "<:flamewaker:422449987105062914>");
                Heroes.Add("npc_dota_hero_phantom_assassin", "<:voltex:422449987197206529>");
                Heroes.Add("npc_dota_hero_necrolyte", "<:venomort:422449987390275595>");
                Heroes.Add("npc_dota_hero_axe", "<:theredgeneral:422449987419635742>");
                Heroes.Add("npc_dota_hero_drow_ranger", "<:astralranger:422449900521914372>");
                Heroes.Add("npc_dota_hero_obsidian_destroyer", "<:epochguardian:422449987180560384>");
                Heroes.Add("npc_dota_hero_omniknight", "<:paladin:422449986962456579>");
                Heroes.Add("npc_dota_hero_crystal_maiden", "<:sorceress:422449987373367297>");
                Heroes.Add("npc_dota_hero_invoker", "<:conjuror:422449987209658407>");
                Heroes.Add("npc_dota_hero_juggernaut", "<:seinaru:422449987256057856>");
                Heroes.Add("npc_dota_hero_beastmaster", "<:elementalwarlord:422449987281223680>");
                Heroes.Add("npc_dota_hero_leshrac", "<:bahamut:422449900907659264>");
                Heroes.Add("npc_dota_hero_spirit_breaker", "<:duskbringer:422449987192881162>");
                Heroes.Add("npc_dota_hero_zuus", "<:auriun:422449900668846117>");
                Heroes.Add("npc_dota_hero_huskar", "<:spiritwarrior:422449987344007168>");
                Heroes.Add("npc_dota_hero_legion_commander", "<:mountainprotector:422449987297869834>");
                Heroes.Add("npc_dota_hero_night_stalker", "<:chernobog:422449986677112864>");
                Heroes.Add("npc_dota_hero_vengefulspirit", "<:solunia:422449987457253376>");
                Heroes.Add("npc_dota_hero_visage", "<:ekkan:422449987054731266>");
                Heroes.Add("npc_dota_hero_dark_seer", "<:zhonik:422449987218046977>");
                Heroes.Add("npc_dota_hero_skywrath_mage", "<:sephyr:422449987314647041>");
                Heroes.Add("npc_dota_hero_slark", "<:slipfinn:422449987398664192>");
                Heroes.Add("npc_dota_hero_antimage", "<:arkimus:422449900685492236>");
                Heroes.Add("npc_dota_hero_monkey_king", "<:djanghor:422449986861793291>");
                Heroes.Add("npc_dota_hero_slardar", "<:hydroxis:426817021309616139>");
                Heroes.Add("npc_dota_hero_templar_assassin", "<:trapper:422449987474030592>");
                Heroes.Add("npc_dota_hero_winter_wyvern", "<:dinath:478217808925294592>");
                Heroes.Add("npc_dota_hero_arc_warden", "<:jex:538154780304998410>");
                Heroes.Add("npc_dota_hero_faceless_void", "<:omniro:575083345206050842>");
                Heroes.Add("npc_dota_hero_grimstroke", "<:rubilash:673287838602887179>");
                Heroes.Add("AkaShrug", "<:AkaShrug:280432292395089920>");
                Heroes.Add("Doge", "<:Doge:280432351425593345>");
                Heroes.Add("Catgasm", "<:Catgasm:280432329627926540>");
                Heroes.Add("mithril_shard", "<:mithril_shard:422449779016990730>");
                Heroes.Add("prismatic_gemstone", "<:prismatic_gemstone:664282137721307156>");
                // Heroes.Add("medivhCmonBruh", "<:medivhCmonBruh:422102804228866061>")
                Heroes.Add("arcane_crystal", "<:arcane_crystal:422449737002778625>");
                Heroes.Add("deckard", "<:deckard:422449840601956372>");
                Heroes.Add("hodor", "<:hodor:426817021141975042>");
                Heroes.Add("chalkybrush", "<:chalkybrush:422449812148060169>");
                GetSerenTop();
                AddSellersToList();
                AddItemsToList();
                Global_Stats_String = $"Roshpit Champions Market Stats : Loaded `{auction_counter}` auctions made by `{Users.Count}` sellers.";
                try
                {
                ///LastModified = DateTime.Now;
                LastModified = FileSystem.FileDateTime(LOG_FILE);
            }
                catch
                {
                    LastModified = DateTime.Now;
                }
            }

            public async Task<bool> Update()
            {
                UpdateIsRunning = true;
                Dictionary<int, AuctionNewClass> NewAuctionlist = new Dictionary<int, AuctionNewClass>();
                StreamWriter writer = new StreamWriter(LOG_FILE);
                try
                {
                    int counter = 1000;
                    int multiplier = 0;
                    string a;
                    WebClient wc = new WebClient() { Encoding = System.Text.Encoding.UTF8 };
                    int start;
                    using (AuctionContext dbAucContext = new AuctionContext())
                    {
                        try
                        {
                            start = dbAucContext.Auctions.First(c => c.status == 0 & c.id > 1730000).id;
                        }
                        catch
                        {
                            // start = dbAucContext.Auctions.Last(Function(c) c.id > 1730000).id
                            start = 1730000;
                        }
                        writer.WriteLine(start);
                        while (counter == 1000)
                        {
                            counter = 0;
                            System.Threading.Thread.Sleep(2000);
                            writer.WriteLine(multiplier);
                            try
                            {
                                a = wc.DownloadString($"http://www.roshpit.ca/champions/API/get_auctions?id={start + (1000 * multiplier)}");
                            }
                            catch (Exception ex)
                            {
                                counter = 1000;
                                writer.WriteLine(ex.Message);
                                continue;
                            }
                            try
                            {
                                foreach (AuctionNewClass auc in JsonConvert.DeserializeObject<AuctionNewClass[]>(a))
                                {
                                    try
                                    {
                                        if (NewAuctionlist.ContainsKey(auc.id) == false)
                                            NewAuctionlist.Add(auc.id, auc);
                                    }
                                    catch (Exception ex)
                                    {
                                        writer.WriteLine(ex.Message);
                                    }
                                    counter += 1;
                                }
                                multiplier += 1;
                            }
                            catch (Exception ex)
                            {
                                writer.WriteLine(ex.Message);
                                counter = 0;
                            }
                        }
                        dbAucContext.Auctions.RemoveRange(((IEnumerable<AuctionNewClass>)dbAucContext.Auctions).Where(c => c.id >= start));
                        dbAucContext.SaveChanges();
                        writer.WriteLine("removed");
                    }

                    List<int> remid = new List<int>();
                    using (AuctionContext dbAucContext = new AuctionContext())
                    {
                        dbAucContext.AddRange(NewAuctionlist.Values);
                        writer.WriteLine("saved " + NewAuctionlist.Count);
                        dbAucContext.SaveChanges();
                    }
                    writer.WriteLine(DateTime.Now);
                    writer.Close();
                    writer.Dispose();
                    LastModified = DateTime.Now;
                    GC.Collect();
                }
                catch (Exception ex)
                {
                    UpdatesEnabled = false;
                    writer.WriteLine(ex.Message);
                    writer.Close();
                    writer.Dispose();
                }
                writer.Close();
                writer.Dispose();
                UpdateIsRunning = false;
                return true;
            }


            public DateTime GetSerenTop()
            {
                WebClient wc = new WebClient();
                string a="";
                bool topflag = true;
                wc.Encoding = System.Text.Encoding.UTF8;
                // Dim reader As New StreamReader("D:\result.txt")
                // a = reader.ReadToEnd
                while (topflag)
                {
                    try
                    {
                        a = wc.DownloadString("https://www.roshpit.ca/champions/get_serengaard?");

                        // reader.Close()
                        // reader.Dispose()
                        topflag = false;
                    }
                    catch
                    {
                        System.Threading.Thread.Sleep(4000);
                        continue;
                    }
                }
                // reader.Close()
                // reader.Dispose()
                Seren_Top = JsonConvert.DeserializeObject<Serenguard_Top>(a);
                return DateTime.Now;
            }

            public void AddItemsToList()
            {
                List<ItemClass> itemlisthelp;
                ItemList.Clear();
                RollsList.Clear();
                using (AuctionContext a = new AuctionContext())
                {
                    itemlisthelp = a.Items.ToList();
                }
                ItemClassComparer Comparer = new ItemClassComparer();
                ItemList = itemlisthelp.Distinct(Comparer).ToDictionary(c =>(c.Name).ToLower());
                return;
                LoadFromWiki("http://roshpit.wikia.com/wiki/World_Drops");
                LoadFromWikiSpecific("http://roshpit.wikia.com/wiki/Specific_Drops%27_Rolls");
                LoadFromWikiArcana("http://roshpit.wikia.com/wiki/Arcana_Rolls");
                LoadFromWeapon("http://roshpit.wikia.com/wiki/Weapons%27_Rolls");
                foreach (KeyValuePair<string, List<string>> i in RollsList)
                {
                    ItemClass it = Search(i.Key.Replace("\n", ""));
                    if (ItemList.ContainsKey((it.Name).ToLower()))
                    {
                        ItemList[it.Name.ToLower()].Rolls = i.Value;

                        if (it.Name.ToLower().Contains("lv2"))
                        if (ItemList.ContainsKey((it.Name.Substring(0, it.Name.Length - 1) + "1").ToLower()))
                            ItemList[((it.Name.Substring(0, it.Name.Length - 1)) + "1").ToLower()].Rolls = i.Value;
                        if (it.Name.ToLower().Contains("lv3"))
                                {
                        if (ItemList.ContainsKey((it.Name.Substring(0, it.Name.Length - 1) + "1").ToLower()))
                            ItemList[((it.Name.Substring(0, it.Name.Length - 1)) + "1").ToLower()].Rolls = i.Value;
                        if (ItemList.ContainsKey((it.Name.Substring(0, it.Name.Length - 1) + "2").ToLower()))
                            ItemList[((it.Name.Substring(0, it.Name.Length - 1)) + "2").ToLower()].Rolls = i.Value;
                    }

                    }
                }
            }
            private void AddSellersToList()
            {
                auction_counter = 0;
                Users.Clear();
                using (AuctionContext MyAuctionContext = new AuctionContext())
                {
                    List<AuctionNewClass> auctions;
                    auctions = (((IEnumerable<AuctionNewClass>)MyAuctionContext.Auctions).Where(c => (int)c.final_price > 0))
                                                        .ToList();

                    foreach (AuctionNewClass auc in auctions)
                    {
                        if (Users.ContainsKey(auc.steam_id) == false)
                            Users.Add(auc.steam_id, new UserNewClass((ulong)auc.steam_id));
                        Users[auc.steam_id].Add_To_list(auc);
                        if (auc.final_price > 0)
                        {
                            if (Users.ContainsKey((int)auc.buyer_id) == false)
                                Users.Add((int)auc.buyer_id, new UserNewClass((ulong)auc.buyer_id));
                            Users[(int)auc.buyer_id].Add_To_list_Buy(auc);
                        }
                    }
                }

                foreach (UserNewClass Au in Users.Values)
                    Au.CalculateStats();
                Leaderboard = null;
                Leaderboard = Users.Values.ToList();
                Leaderboard.Sort((x, y) => y.Total_Income.CompareTo(x.Total_Income));
                for (var i = 0; i <= Leaderboard.Count - 1; i++)
                    Users[(int)Leaderboard[i].id].Global_Top_Position = i + 1;
                Leaderboard.Sort((x, y) => y.Pure_Profit.CompareTo(x.Pure_Profit));
                for (var i = 0; i <= Leaderboard.Count - 1; i++)
                    Users[(int)Leaderboard[i].id].Global_Top_Pure = i + 1;
                GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
                GC.Collect();
            }

            public ItemClass Search(string Pattern, bool fullcompare = false)
            {
                if (fullcompare)
                {
                    if (ItemList.ContainsKey(Strings.LCase(Pattern)))
                        return ItemList[Strings.LCase(Pattern)];
                    else
                        return null/* TODO Change to default(_) if this is not a reference type */;
                }
                string[] patterns = (Strings.Trim(Strings.LCase(Pattern))).Split(" ");
                Dictionary<string, int> MatchList = new Dictionary<string, int>();
                for (var i = 0; i <= patterns.Count() - 1; i++)
                {
                    patterns[i] = patterns[i];
                foreach (string it in ItemList.Keys)
                {
                    if (it.Contains(patterns[i]))
                    {
                        if (MatchList.ContainsKey(it)) MatchList[it] += 3;
                        else MatchList.Add(it, 3);
                    }
                }
                    foreach (KeyValuePair<string, int> a in MatchList.ToList())
                    {
                        if (a.Value / (double)3 == patterns.Length)
                            MatchList[a.Key] += 3;
                        if ((Strings.Trim(Strings.LCase(Pattern))) == a.Key)
                            MatchList[a.Key] += 20;
                    }
                foreach (string it in ItemList.Keys)
                {
                    if (ItemList[it].AlternativeName.Contains(patterns[i]))
                    {
                        if (MatchList.ContainsKey(it)) MatchList[it] += 2;
                        else MatchList.Add(it, 2);
                    }
                }
                }

                List<KeyValuePair<string, int>> Matchlisting;

                Matchlisting = MatchList.ToList();

                if (Matchlisting.Count == 0)
                    return new ItemClass("", "", "", "");

                Matchlisting.Sort((x, y) => y.Value.CompareTo(x.Value));

                for (var i = 1; i <= Matchlisting.Count - 1; i++)
                {
                    if (Matchlisting[i].Value < Matchlisting[0].Value)
                    {
                        Matchlisting.RemoveAt(i);
                        if (i > Matchlisting.Count - 1)
                            break;
                        i = i - 1;
                    }
                }

                Matchlisting.Sort((x, y) => x.Key.Length.CompareTo(y.Key.Length));

                return ItemList[Matchlisting[0].Key];
            }

            public void LoadFromWeapon(string url)
            {
                HtmlWeb web = new HtmlWeb();
                HtmlDocument doc = new HtmlDocument();
                HtmlNodeCollection elems;
                int counter;
                doc = web.Load(url);
                elems = doc.DocumentNode.SelectNodes($"//table[@class={Strings.Chr(34)}article-table sortable{Strings.Chr(34)}]");
                counter = 0;
                foreach (HtmlNode elem in elems)
                {
                    HtmlNodeCollection rows = elem.SelectNodes("tr");
                    for (int i = 1; i <= rows.Count - 1; i++)
                    {
                        HtmlNodeCollection cols = rows[i].SelectNodes("th");
                        HtmlNodeCollection rows2 = cols[1].SelectNodes(".//tr");
                        if (RollsList.ContainsKey(rows2[0].InnerText))
                            continue;
                        if (RollsList.ContainsKey(rows2[0].InnerText) == false)
                        {
                            RollsList.Add(rows2[0].InnerText, new List<string>());
                            // If counter < 4 Then
                            HtmlNodeCollection rows1 = cols[2].SelectNodes(".//tr");
                            for (int j = 0; j <= rows1.Count - 1; j++)
                                RollsList.Values.Last().Add((rows1[j].InnerText.Replace(Constants.vbLf, "")).Trim());
                        }
                    }
                    counter = counter + 1;
                }
            }

            public void LoadFromWiki(string url)
            {
                HtmlWeb web = new HtmlWeb();
                HtmlDocument doc = new HtmlDocument();
                HtmlNodeCollection elems;
                int counter;
                doc = web.Load(url);
                elems = doc.DocumentNode.SelectNodes($"//table[@class={Strings.Chr(34)}article-table sortable{Strings.Chr(34)}]");
                counter = 0;
                foreach (HtmlNode elem in elems)
                {
                    HtmlNodeCollection rows = elem.SelectNodes("tr");
                    for (int i = 1; i <= rows.Count - 1; i++)
                    {
                        HtmlNodeCollection cols = rows[i].SelectNodes("td");
                        if (RollsList.ContainsKey(cols[1].InnerText) == false)
                        {
                            RollsList.Add(cols[1].InnerText, new List<string>());
                            // If counter < 4 Then
                            HtmlNodeCollection rows1 = cols[2].SelectNodes(".//tr");
                            for (int j = 0; j <= rows1.Count - 1; j++)
                                RollsList.Values.Last().Add((rows1[j].InnerText.Replace(Constants.vbLf, "")).Trim());
                        }
                    }
                    counter = counter + 1;
                }
            }
            public void LoadFromWikiSpecific(string url)
            {
                HtmlWeb web = new HtmlWeb();
                HtmlDocument doc = new HtmlDocument();
                HtmlNodeCollection elems;
                int counter;
                doc = web.Load(url);
                elems = doc.DocumentNode.SelectNodes($"//table[@class={Strings.Chr(34)}article-table sortable{Strings.Chr(34)}]");
                counter = 0;
                foreach (HtmlNode elem in elems)
                {
                    HtmlNodeCollection rows = elem.SelectNodes("tr");
                    for (int i = 1; i <= rows.Count - 1; i++)
                    {
                        HtmlNodeCollection cols = rows[i].SelectNodes("td");
                        if (RollsList.ContainsKey(cols[1].InnerText) == false)
                        {
                            RollsList.Add(cols[1].InnerText, new List<string>());
                            // If counter < 4 Then
                            HtmlNodeCollection rows1 = cols[3].SelectNodes(".//tr");
                            for (int j = 0; j <= rows1.Count - 1; j++)
                                RollsList.Values.Last().Add((rows1[j].InnerText.Replace(Constants.vbLf, "")).Trim());
                        }
                    }
                    counter = counter + 1;
                }
            }
            public void LoadFromWikiArcana(string url)
            {
                HtmlWeb web = new HtmlWeb();
                HtmlDocument doc = new HtmlDocument();
                HtmlNodeCollection elems;
                int counter;
                doc = web.Load(url);
                elems = doc.DocumentNode.SelectNodes($"//table[@class={Strings.Chr(34)}article-table sortable{Strings.Chr(34)}]");
                counter = 0;
                foreach (HtmlNode elem in elems)
                {
                    HtmlNodeCollection rows = elem.SelectNodes("tr");
                    for (int i = 1; i <= rows.Count - 1; i++)
                    {
                        HtmlNodeCollection cols = rows[i].SelectNodes("td");
                        if (RollsList.ContainsKey(cols[1].InnerText) == false)
                        {
                            RollsList.Add(cols[1].InnerText, new List<string>());
                            if (counter < 4)
                            {
                                HtmlNodeCollection rows1 = cols[4].SelectNodes(".//tr");
                                for (int j = 0; j <= rows1.Count - 1; j++)
                                    RollsList.Values.Last().Add(rows1[j].InnerText.Replace(Constants.vbLf, "").Trim());
                            }
                            else
                                RollsList.Values.Last().Add(cols[2].InnerText.Replace(Constants.vbLf, ""));
                        }
                    }
                    counter = counter + 1;
                }
            }

            public new AuctionNewClass GetOngoingAuction2(string URL)
            {
                WebClient wc = new WebClient();
                Uri U = new Uri(URL);
                string a;
                AuctionNewClass new_auc;
                a = wc.DownloadString($"Http://www.roshpit.ca/champions/API/get_auctions?limit=1&id={System.Convert.ToInt64(U.Segments.Last())}");
                // new_auc = JsonConvert.DeserializeObject(Of AuctionNewClass())(a).First
                return JsonConvert.DeserializeObject<AuctionNewClass>(a);
            }
        }


        class ItemClassComparer : IEqualityComparer<ItemClass>
        {
            public bool Equals(ItemClass x, ItemClass y)
            {
                if (object.ReferenceEquals(x, y))
                    return true;
                if (object.ReferenceEquals(x, null/* TODO Change to default(_) if this is not a reference type */) || object.ReferenceEquals(y, null/* TODO Change to default(_) if this is not a reference type */))
                    return false;
                return x.Name.ToLower() == y.Name.ToLower();
            }

            public int GetHashCode(ItemClass ItemClass)
            {
                if (object.ReferenceEquals(ItemClass, null/* TODO Change to default(_) if this is not a reference type */))
                    return 0;
                int hashItemClassName = ItemClass.Name == null ? 0 : ItemClass.Name.GetHashCode();
                int hashItemClassCode = ItemClass.Name.GetHashCode();
                return hashItemClassName ^ hashItemClassCode;
            }
        }
    }
