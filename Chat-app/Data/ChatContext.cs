using Chat_app.Models;
using Microsoft.EntityFrameworkCore;

namespace Chat_app.Data
{
    public class ChatContext : DbContext
    {
        public ChatContext(DbContextOptions<ChatContext> options) : base(options) { }

        public DbSet<ChatGroup> ChatGroups => Set<ChatGroup>();
        public DbSet<Message> Messages => Set<Message>();
    }
}
