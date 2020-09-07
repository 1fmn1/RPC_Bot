using System;
using System.Collections.Generic;
using System.Text;

namespace RPC_Bot.Modules
{
    using System;

    public class AuctionNewClass
    {
        public int id { get; set; }
        public int steam_id { get; set; }
        public int championitem_id { get; set; }
        public string roshpititem_variant { get; set; }
        public int roshpititem_id { get; set; }
        public int minimum_bid { get; set; }
        public int buyout { get; set; }
        public string resource_type { get; set; }
        public DateTime expiry_time { get; set; }
        public int? final_price
        {
            get
            {
                return mfinalprice;
            }
            set
            {
                if (value == null)
                    mfinalprice = 0;
                else
                    mfinalprice = (int)value;
            }
        }
        public int? buyer_id
        {
            get
            {
                return mbuyer_id;
            }
            set
            {
                if (value == null)
                    mbuyer_id = 0;
                else
                    mbuyer_id = (int)value;
            }
        }
        public int status { get; set; }
        public int current_bid { get; set; }
        public string item_image_url { get; set; }
        public string item_name_en
        {
            get
            {
                if (mitem_name_en == null)
                    return roshpititem_variant;
                return mitem_name_en;
            }
            set
            {
                if (value == null)
                    mitem_name_en = roshpititem_variant;
                else
                    mitem_name_en = value;
            }
        }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public string property1name { get; set; }
        public string property2name { get; set; }
        public string property3name { get; set; }
        public string property4name { get; set; }
        public int? property1 { get; set; }
        public int? property2 { get; set; }
        public int? property3 { get; set; }
        public int? property4 { get; set; }
        public int? level_req { get; set; }
        public int? current_level { get; set; }
        public int? max_level { get; set; }
        public int? base_armor { get; set; }
        public int? base_magic_armor { get; set; }
        public string socket1 { get; set; }
        public int? socket1value { get; set; }
        public string socket2 { get; set; }
        public int? socket2value { get; set; }

        private int mfinalprice;
        private int mbuyer_id;
        private string mitem_name_en;
    }

}
