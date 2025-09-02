using Chat_app.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Chat_app.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly ChatContext _context;

        public ChatController(ChatContext context)
        {
            _context = context;
        }

        // ✅ Get all groups
        // GET: api/chat/groups
        [HttpGet("groups")]
        public async Task<IActionResult> GetAllGroups()
        {
            try
            {
                var groups = await _context.ChatGroups
                    .Select(g => new
                    {
                        g.Id,
                        g.Name,
                        MessageCount = g.Messages.Count
                    })
                    .ToListAsync();

                return Ok(groups);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // ✅ Get messages by group name
        // GET: api/chat/group/TestRoom
        [HttpGet("group/{groupName}")]
        public async Task<IActionResult> GetMessagesByGroup(string groupName)
        {
            try
            {
                var group = await _context.ChatGroups
                    .Include(g => g.Messages)
                    .FirstOrDefaultAsync(g => g.Name == groupName);

                if (group == null)
                    return NotFound(new { message = "Group not found" });

                return Ok(group.Messages.Select(m => new
                {
                    m.Id,
                    m.Sender,
                    m.Content,
                    m.ChatGroupId
                }));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
