using System;
using System.Collections.Generic;
using System.Text;

namespace RPC_Bot.Modules
{
    using System;
    using System.Collections.Generic;
    using Microsoft.VisualBasic;

    public class ItemClass
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string link { get; set; }
        public string Rarity { get; set; }
        public string Image { get; set; }
        public string Slot { get; set; }
        public int? base_armor { get; set; }
        public int? base_magic_armor { get; set; }
        public string socket1 { get; set; }
        public int? socket1value { get; set; }
        public string socket2 { get; set; }
        public int? socket2value { get; set; }
        public string Special_Ability { get; set; }
        public string Special_Ability_Element { get; set; }
        public string Special_Ability_Damage_Type { get; set; }
        public string Special_Ability_Description { get; set; }
        public string AlternativeName
        {
            get
            {
                if (maltname == null | maltname == "")
                {
                    maltname = link.Replace("https://www.roshpit.ca//items/item_rpc_", "");
                    return maltname;
                }
                else
                    return maltname;
            }
            set
            {
                maltname = value;
            }
        }
        public string Required_level { get; set; }
        public List<string> Rolls = new List<string>();

        private string maltname;
        public ItemClass()
        {
        }
        public ItemClass(string mName, string mRarity, string mSlot, string mImage)
        {
            Name = mName;
            Slot = mSlot;
            Rarity = mRarity;
            Image = mImage;
        }
        public ItemClass(string mName, string mRarity, string mSlot, string mImage, string mSpecialdesr, string mAlternativeName, string mlevel, string mhero)
        {
            Name = mName;
            Slot = mSlot;
            Rarity = mRarity;
            Image = mImage;
            Special_Ability = mhero;
            Special_Ability_Description = mSpecialdesr;
            AlternativeName = mAlternativeName;
            Required_level = mlevel;
        }
        public ItemClass(string mName, string mRarity, string mSlot, string mImage, string altName, string mSpecialName, string mSpecialDescription, string mType, string mElement)
        {
            Name = mName;
            Slot = mSlot;
            Rarity = mRarity;
            Image = mImage;
            Special_Ability = mSpecialName;
            Special_Ability_Description = mSpecialDescription.Replace(Constants.vbTab, "");
            Special_Ability_Element = mElement;
            Special_Ability_Damage_Type = mType;
            AlternativeName = altName;
        }
    }


}
