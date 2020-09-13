using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Text;

namespace RPC_Bot.Modules
{
    class Config
    {
        public string token { get; set; }
        public char prefix { get; set;}
    
        public Config()
        {

        }
    }
}
