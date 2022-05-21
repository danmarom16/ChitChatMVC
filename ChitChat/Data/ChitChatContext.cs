using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ChitChat.Models;

namespace ChitChat.Data
{
    public class ChitChatContext : DbContext
    {
        public ChitChatContext (DbContextOptions<ChitChatContext> options)
            : base(options)
        {
        }

        public DbSet<ChitChat.Models.User>? User { get; set; }

        public DbSet<ChitChat.Models.Chat>? Chat { get; set; }
    }
}
