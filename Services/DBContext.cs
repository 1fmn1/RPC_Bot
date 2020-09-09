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
using Microsoft.EntityFrameworkCore;
using RPC_Bot.Modules;
using Microsoft.EntityFrameworkCore.Storage;

namespace RPC_Bot.Services
{
    public class AuctionContext : DbContext
    {
        // Public Const DataFile As String = "G:\\programming\RPC_BOT_WEB\RPC_BOT_WEB\Tables\Auctions.db"
        // Public Const DataFile As String = "h:\root\home\medivh015-001\www\site1\Auctions.db"
        const string DataFile = @"H:\Programming\RPC_BOT_WEB\RPC_BOT_WEB\Tables\Auctions.db";
        //const string DataFile = @"Au.db";
        //const string DataFile = @"d:\DZHosts\LocalUser\medivh015\www.Curator.somee.com\Auctions.db";
        // Public Const DataFile As String =
        // Public Property Companies As DbSet(Of Company)
        // Public Property ItemsNew As DbSet(Of ItemClassNew)
        public DbSet<ItemClass> Items { get; set; }
        public DbSet<AuctionNewClass> Auctions { get; set; }

        public AuctionContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={DataFile}");
        }
    }

}
