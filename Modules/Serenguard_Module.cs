using System;
using System.Collections.Generic;
using System.Text;

namespace RPC_Bot.Modules 
{
static class Serenguard_Module
{
    public class Seven
    {
        public int id { get; set; }
        public int? steam_id1 { get; set; }
        public int? steam_id2 { get; set; }
        public int? steam_id3 { get; set; }
        public int? steam_id4 { get; set; }
        public string steam_id_long1 { get; set; }
        public string steam_id_long2 { get; set; }
        public string steam_id_long3 { get; set; }
        public string steam_id_long4 { get; set; }
        public string steam_name1 { get; set; }
        public string steam_name2 { get; set; }
        public string steam_name3 { get; set; }
        public string steam_name4 { get; set; }
        public string hero1 { get; set; }
        public string hero2 { get; set; }
        public string hero3 { get; set; }
        public string hero4 { get; set; }
        public int wave_number { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }

    public class ThisMonth
    {
        public int id { get; set; }
        public int? steam_id1 { get; set; }
        public int? steam_id2 { get; set; }
        public int? steam_id3 { get; set; }
        public int? steam_id4 { get; set; }
        public string steam_id_long1 { get; set; }
        public string steam_id_long2 { get; set; }
        public string steam_id_long3 { get; set; }
        public string steam_id_long4 { get; set; }
        public string steam_name1 { get; set; }
        public string steam_name2 { get; set; }
        public string steam_name3 { get; set; }
        public string steam_name4 { get; set; }
        public string hero1 { get; set; }
        public string hero2 { get; set; }
        public string hero3 { get; set; }
        public string hero4 { get; set; }
        public int wave_number { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }

    public class AllTime
    {
        public int id { get; set; }
        public int? steam_id1 { get; set; }
        public int? steam_id2 { get; set; }
        public int? steam_id3 { get; set; }
        public int? steam_id4 { get; set; }
        public string steam_id_long1 { get; set; }
        public string steam_id_long2 { get; set; }
        public string steam_id_long3 { get; set; }
        public string steam_id_long4 { get; set; }
        public string steam_name1 { get; set; }
        public string steam_name2 { get; set; }
        public string steam_name3 { get; set; }
        public string steam_name4 { get; set; }
        public string hero1 { get; set; }
        public string hero2 { get; set; }
        public string hero3 { get; set; }
        public string hero4 { get; set; }
        public int wave_number { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }

    public class Serenguard_Top
    {
        public Seven[] seven { get; set; }
        public ThisMonth[] thisMonth { get; set; }
        public AllTime[] allTime { get; set; }
    }
}
}

