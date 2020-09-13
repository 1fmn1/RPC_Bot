using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RPC_Bot.Modules
{
    /// <summary>
    /// RegisteredUserClass entity
    /// </summary>
    public class RegisteredUserClass
    {
        [Key]
        public ulong DiscordID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public ulong DotaID { get; set; }
        [Required]
        public int HangManScore { get; set; }
    }

    public class channelTimerstruct
    {
        public int Current_delay { get; set; }
        public int Channel_delay { get; set; }
        public void Set_delay()
        {
            Current_delay = Channel_delay;
        }
        public void Decrement()
        {
            if (Current_delay > 0)
                Current_delay = Current_delay - 1;
        }
    }
}
