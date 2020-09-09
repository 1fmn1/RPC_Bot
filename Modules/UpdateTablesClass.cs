using System;
using System.Collections.Generic;
using System.Text;

namespace RPC_Bot.Modules
{
    using System;
    using System.Collections.Generic;
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
    using HtmlAgilityPack;
    using System.Web;
    using RPC_Bot.Services;
    using System.Reflection.Metadata.Ecma335;

    public class UpdateTablesClass
    {
        // Dim itemlist As Dictionary(Of String, ItemClass)
        private string SPLITTER = "¿";
        private List<string> HeroList;

        public void Load()
        {
        }
        public ItemClass LoadItem(string link)
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc;
            ItemClass NewItem = new ItemClass();
            doc = web.Load(link);
            HtmlNode item = doc.DocumentNode.SelectSingleNode($"//div[@id={Strings.Chr(34)}main-item-container{Strings.Chr(34)}]");
            NewItem.Name = System.Net.WebUtility.HtmlDecode(item.ChildNodes[1].InnerText);

            NewItem.link = link;
            NewItem.Image = item.SelectSingleNode("img").GetAttributeValue("src", "=");
            NewItem.Rarity = (item.ChildNodes[12].InnerText);

            try
            {
                NewItem.Slot = (item.ChildNodes[16].InnerText.Replace("Slot: ", ""));
            }
            catch
            {
            }
            if (NewItem.Slot == "")
                NewItem.Slot = "Glyph";
            try
            {
                NewItem.Special_Ability_Description = System.Net.WebUtility.HtmlDecode(item.InnerText);
                if (NewItem.Special_Ability_Description.Contains("Damage Type:"))
                {
                    int st = NewItem.Special_Ability_Description.IndexOf("Type:") + 5;
                    int len = NewItem.Special_Ability_Description.IndexOf("Property") - st;
                    string mystring;
                    string[] s;
                    mystring = NewItem.Special_Ability_Description.Substring(st, len).Trim().Replace("\r\n", "");
                    s = mystring.Split();
                    NewItem.Special_Ability_Damage_Type = s[0];
                    for (int i = 1; i <= s.Length - 1; i++)
                    {
                        if (s[i] == "Element:")
                        {
                            NewItem.Special_Ability_Element = s[i + 1];
                            if (s[20] == "/") NewItem.Special_Ability_Element += " / " + s[28];
                            break;
                        }
                    }
                }
                NewItem.Special_Ability = System.Net.WebUtility.HtmlDecode(item.SelectSingleNode($"//tbody[@id={Strings.Chr(34)}propertyData1{Strings.Chr(34)}]").SelectNodes("tr/td").First().InnerText);
                NewItem.Special_Ability_Description = System.Net.WebUtility.HtmlDecode((item.SelectSingleNode($"//tbody[@id={Strings.Chr(34)}propertyData1{Strings.Chr(34)}]").SelectNodes("tr").Last().InnerText.Replace(Constants.vbTab, "").Replace(Constants.vbLf, "")).Trim());
            }
            catch
            {
            }
            return NewItem;
        }
        public void UpdateItems()
        {
            HtmlWeb web = new HtmlWeb();
            List<string> NewItemList = new List<string>();
            List<ItemClass> NewItems = new List<ItemClass>();
            string[] a = new[] { "head", "body", "weapon", "hands", "feet", "amulet", "consumable" };
            HtmlDocument doc;
            HtmlEntity util;
            foreach (var i in a)
            {
                doc = web.Load($"https://www.roshpit.ca/items?slot={i}");
                HtmlNodeCollection items = doc.DocumentNode.SelectNodes($"//div[@id={Strings.Chr(34)}item-table-container{Strings.Chr(34)}]//a");
                foreach (var it in items)
                    NewItemList.Add(it.GetAttributeValue("href", "none"));
            }
            foreach (var it in NewItemList)
            {
                NewItems.Add(LoadItem($"https://www.roshpit.ca/{it}"));
                System.Threading.Thread.Sleep(500);
            }
            using (AuctionContext myx = new AuctionContext())
            {
                myx.Items.AddRange(NewItems);
                myx.SaveChanges();
            }
        }
        public void LoadHeroes(ref List<string> link)
        {
            link = new List<string>();
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load("http://www.roshpit.ca/heroes");
            HtmlNodeCollection imgs = doc.DocumentNode.SelectNodes($"//img[@class={Strings.Chr(34)}hero_image{Strings.Chr(34)}]");
            foreach (HtmlNode img in imgs)
                link.Add(img.GetAttributeValue("src", "").Replace("https://s3-us-west-2.amazonaws.com/roshpit-assets/heroes/", "").Replace(".png", ""));
        }
        public void LoadGlyphsData(string link, string hero)
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(link);
            HtmlNodeCollection imgs = doc.DocumentNode.SelectNodes($"//div[@class={Strings.Chr(34)}glyph_container{Strings.Chr(34)}]");

            List<ItemClass> Itemlist = new List<ItemClass>();
            foreach (HtmlNode img in imgs)
                Itemlist.Add(new ItemClass() { 
                    Name = System.Net.WebUtility.HtmlDecode(img.ChildNodes[1].GetAttributeValue("data-glyph-name", "Unknown")), 
                    Rarity = System.Net.WebUtility.HtmlDecode(img.ChildNodes[1].GetAttributeValue("data-rarity-color", "Unknown")), 
                    Slot = "Glyph", 
                    Image = img.ChildNodes[3].GetAttributeValue("src", "Unknown"), 
                    Special_Ability_Description = System.Net.WebUtility.HtmlDecode(img.ChildNodes[1].GetAttributeValue("data-glyph-description", "Unknown").Replace("&lt;font color=&#39;#CCFF66&#39;&gt;", "**").Replace("&lt;/font&gt;", "**").Replace("&#39", "'").Replace("&lt;font color=&quot;#EF4126&quot;&gt;", "**").Replace("&lt;font color=&quot;#87D9FF&quot;&gt;", "**").Replace("&lt;font color=&quot;#C25DFC&quot;&gt;", "**").Replace("&lt;font color=&quot;#5CCDF9&quot;&gt;", "**").Replace("&lt;font color=&quot;#69BC71&quot;&gt;", "**").Replace("color=&quot;#DDDDDD&quot;&gt;", "**").Replace("&lt;font color=&quot;#B5FFB7&quot;&gt;", "**")), 
                    AlternativeName = img.ChildNodes[3].GetAttributeValue("src", "Unknown").Replace("https://s3-us-west-2.amazonaws.com/roshpit-assets/glyphs/", "").Replace(".png", ""), 
                    Required_level = img.ChildNodes[1].GetAttributeValue("data-required-level", "Unknown"), 
                    Special_Ability = hero });
            using (AuctionContext a = new AuctionContext())
            {
                a.Items.AddRange(Itemlist);
                a.SaveChanges();
            }
        }

        public void UpdateGlyphs()
        {
            LoadHeroes(ref HeroList);
            foreach (string item in HeroList)
            {
                Console.WriteLine(item);
                LoadGlyphsData($"https://www.roshpit.ca/heroes/npc_dota_hero_{item}/getGlyphs", item);
                System.Threading.Thread.Sleep(500);
            }
        }

        public void UpdateNames(ref List<RegisteredUserClass> RegUsersList)
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc;
            foreach (RegisteredUserClass reguser in RegUsersList)
            {
                doc = web.Load($"https://www.roshpit.ca/players/{reguser.DotaID}");
                HtmlNode a = doc.DocumentNode.SelectSingleNode($"//span[@id={Strings.Chr(34)}index-title{Strings.Chr(34)}]");
                reguser.Name = a.InnerText.Replace(Constants.vbLf, "").Replace(Constants.vbTab, "").Trim();
                System.Threading.Thread.Sleep(1000);
            }
        }
    }

}
