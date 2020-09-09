using System;
using System.Collections.Generic;
using System.Text;

namespace RPC_Bot.Modules
{
    public class RegisteredUserClass
    {
        public ulong DotaID;
        public string Name;
        public ulong DiscordID;
        public int HangManScore = 0;
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
