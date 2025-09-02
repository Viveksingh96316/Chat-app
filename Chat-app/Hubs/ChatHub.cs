using Chat_app.Data;
using Chat_app.Models;
using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;

namespace Chat_app.Hubs
{
    public class ChatHub: Hub
    {
        private readonly ChatContext _context;

        public ChatHub(ChatContext context)
        {
            _context = context;
        }

        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            // Create group if it doesn’t exist
            var group = _context.ChatGroups.FirstOrDefault(g => g.Name == groupName);
            if (group == null)
            {
                group = new ChatGroup { Name = groupName };
                _context.ChatGroups.Add(group);
                await _context.SaveChangesAsync();
            }

            await Clients.Group(groupName).SendAsync("ReceiveMessage", "System", $"{Context.ConnectionId} joined {groupName}");
        }   

        public async Task SendMessage(string groupName, string sender, string message)
        {
            var group = _context.ChatGroups.FirstOrDefault(g => g.Name == groupName);
            if (group == null) return;

            var msg = new Message { Sender = sender, Content = message, ChatGroupId = group.Id };
            _context.Messages.Add(msg);
            await _context.SaveChangesAsync();

            await Clients.Group(groupName).SendAsync("ReceiveMessage", sender, message);
        }
    }
}
