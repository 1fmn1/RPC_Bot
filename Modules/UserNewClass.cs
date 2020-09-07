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


namespace RPC_Bot.Modules
{

    public class UserNewClass
    {
        public ulong id { get; set; }
        public List<AuctionNewClass> Auctionlist { get; set; }
        public List<AuctionNewClass> AuctionlistBuy { get; set; }
        public int Items_Sold { get; set; }
        public string Success { get; set; }
        public string First_Entry { get; set; }
        public string Last_Entry { get; set; }
        public int Highest_Earning { get; set; }
        public string Highest_Earning_Item { get; set; }
        public int Total_Income { get; set; }

        public int Income_Shard { get; set; }
        public int Income_Crystal { get; set; }
        public int Income_Gemstone { get; set; }

        public int Outcome_Shard { get; set; }
        public int Outcome_Crystal { get; set; }
        public int Outcome_Gemstone { get; set; }

        public int Profit_Shard { get; set; }
        public int Profit_Crystal { get; set; }
        public int Profit_Gemstone { get; set; }

        public string Common_Item { get; set; }
        public int Common_Item_Copies_Sold { get; set; }
        public string Regular_Customer { get; set; }
        public int Regular_Customer_Bought { get; set; }
        public int Global_Top_Position { get; set; }
        public int Highest_Spending { get; set; }
        public string Highest_Spending_Item { get; set; }
        public int Total_Spent { get; set; }
        public int Items_Bought { get; set; }
        public int Pure_Profit { get; set; }
        public int Global_Top_Pure { get; set; }

        public UserNewClass(ulong IDs)
        {
            id = IDs;
            Auctionlist = new List<AuctionNewClass>();
            AuctionlistBuy = new List<AuctionNewClass>();
        }

        public void ClearALL()
        {
            Auctionlist.Clear();
            Auctionlist.TrimExcess();
            Auctionlist = null;
            AuctionlistBuy.Clear();
            AuctionlistBuy.TrimExcess();
            AuctionlistBuy = null;
        }
        public void Add_To_list(AuctionNewClass Auc)
        {
            if (Auc != null)
                Auctionlist.Add(Auc);
        }
        public void Add_To_list_Buy(AuctionNewClass Auc)
        {
            if (Auc != null)
                AuctionlistBuy.Add(Auc);
        }

        public void CalculateStats()
        {
            int ItemCounter;
            int Income;
            string Item = "";
            try
            {
                if (Auctionlist.Count > 0)
                {
                    try
                    {
                        foreach (var au in Auctionlist.Where(x => x.resource_type == "Mithril Shards"))
                        {
                            Income = Income + au.final_price;
                            if (au.final_price > 0)
                                ItemCounter += 1;
                        }
                    }
                    catch
                    {
                    }
                    Income_Shard = Income;
                    Income = 0;
                    try
                    {
                        foreach (var au in Auctionlist.Where(x => x.resource_type == "Arcane Crystals"))
                        {
                            Income = Income + au.final_price;
                            if (au.final_price > 0)
                                ItemCounter += 1;
                        }
                    }
                    catch
                    {
                    }
                    Income_Crystal = Income;
                    Income = 0;
                    try
                    {
                        foreach (var au in Auctionlist.Where(x => x.resource_type == "Prismatic Gemstones"))
                        {
                            Income = Income + au.final_price;
                            if (au.final_price > 0)
                                ItemCounter += 1;
                        }
                    }
                    catch
                    {
                    }
                    Income_Gemstone = Income;
                    Income = 0;
                    Auctionlist.Sort((x, y) => System.Convert.ToInt32(y.final_price).CompareTo(x.final_price));
                    foreach (AuctionNewClass Au in Auctionlist)
                    {
                        Income = Income + Au.final_price;
                        if (Au.final_price > 0)
                            ItemCounter += 1;
                    }
                    Items_Sold = ItemCounter;
                    Total_Income = Income;
                    Highest_Earning = Auctionlist[0].final_price;
                    Highest_Earning_Item = $"[{Auctionlist[0].item_name_en}](http://www.roshpit.ca/market/auction/{Auctionlist[0].id})";
                    // Auctionlist.Sort(Function(x, y) y.AucDate.CompareTo(x.AucDate))
                    // For i = Auctionlist.Count - 1 To 0 Step -1
                    // If Auctionlist(i).AucDate > New Date(2017, 1, 1) Then
                    // First_Entry = Auctionlist(i).AucDate.ToShortDateString
                    // Exit For
                    // End If
                    // Next
                    // Last_Entry = Auctionlist(0).AucDate.ToShortDateString
                    Success = ItemCounter * 100 / Auctionlist.Count + "%";

                    Auctionlist.Sort((x, y) => y.item_name_en.CompareTo(x.item_name_en));
                    ItemCounter = 0;
                    int helpcounter;
                    helpcounter = 0;
                    for (var i = 0; i <= Auctionlist.Count - 1; i++)
                    {
                        if (i > 0)
                        {
                            if (Auctionlist[i].item_name_en == Auctionlist[i - 1].item_name_en)
                            {
                                if (Auctionlist[i].final_price > 0)
                                    helpcounter += 1;
                            }
                            else if (helpcounter >= ItemCounter)
                            {
                                ItemCounter = helpcounter;
                                Item = Auctionlist[i - 1].item_name_en;
                                helpcounter = 1;
                            }
                            else
                                helpcounter = 1;
                        }
                        else
                        {
                            Item = Auctionlist[i].item_name_en;
                            helpcounter = 1;
                            ItemCounter = helpcounter;
                        }
                    }

                    Common_Item = Item;
                    Common_Item_Copies_Sold = ItemCounter;

                    Auctionlist.Sort((x, y) => System.Convert.ToInt32(y.buyer_id).CompareTo(x.buyer_id));
                    ItemCounter = 0;
                    helpcounter = 0;
                    for (var i = 0; i <= Auctionlist.Count - 1; i++)
                    {
                        if (i > 0)
                        {
                            if (Auctionlist[i].buyer_id == Auctionlist[i - 1].buyer_id)
                                helpcounter += 1;
                            else if (helpcounter >= ItemCounter)
                            {
                                ItemCounter = helpcounter;
                                Item = Auctionlist[i - 1].buyer_id;
                                helpcounter = 1;
                            }
                            else
                                helpcounter = 1;
                        }
                        else
                            helpcounter = 1;
                    }
                    Regular_Customer = Item;
                    Regular_Customer_Bought = ItemCounter;
                }
                Income = 0; ItemCounter = 0; Item = "";
                if (AuctionlistBuy.Count > 0)
                {
                    try
                    {
                        foreach (var au in AuctionlistBuy.Where(x => x.resource_type == "Mithril Shards"))
                        {
                            Income = Income + au.final_price;
                            if (au.final_price > 0)
                                ItemCounter += 1;
                        }
                    }
                    catch
                    {
                    }
                    Outcome_Shard = Income;
                    Income = 0;
                    try
                    {
                        foreach (var au in AuctionlistBuy.Where(x => x.resource_type == "Arcane Crystals"))
                        {
                            Income = Income + au.final_price;
                            if (au.final_price > 0)
                                ItemCounter += 1;
                        }
                    }
                    catch
                    {
                    }
                    Outcome_Crystal = Income;
                    Income = 0;
                    try
                    {
                        foreach (var au in AuctionlistBuy.Where(x => x.resource_type == "Prismatic Gemstones"))
                        {
                            Income = Income + au.final_price;
                            if (au.final_price > 0)
                                ItemCounter += 1;
                        }
                    }
                    catch
                    {
                    }
                    Outcome_Gemstone = Income;
                    Income = 0;
                    Profit_Crystal = Income_Crystal - Outcome_Crystal;
                    Profit_Shard = Income_Shard - Outcome_Shard;
                    Profit_Gemstone = Income_Gemstone - Outcome_Gemstone;

                    AuctionlistBuy.Sort((x, y) => System.Convert.ToInt32(y.final_price).CompareTo(x.final_price));
                    foreach (AuctionNewClass Au in AuctionlistBuy)
                    {
                        Income = Income + Au.final_price;
                        if (Au.final_price > 0)
                            ItemCounter += 1;
                    }
                    Items_Bought = ItemCounter;
                    Total_Spent = Income;
                    Highest_Spending = AuctionlistBuy[0].final_price;
                    Highest_Spending_Item = $"[{AuctionlistBuy[0].item_name_en}](http://www.roshpit.ca/market/auction/{AuctionlistBuy[0].id})";
                }

                Profit_Crystal = Income_Crystal - Outcome_Crystal;
                Profit_Shard = Income_Shard - Outcome_Shard;
                Profit_Gemstone = Income_Gemstone - Outcome_Gemstone;
                Pure_Profit = Total_Income - Total_Spent;

                // For Each auc In AuctionlistBuy
                // auc = Nothing
                // Next
                // For Each auc In Auctionlist
                // auc = Nothing
                // Next
                AuctionlistBuy.Clear();
                Auctionlist.Clear();
            }
            catch
            {
            }
        }
    }

}
