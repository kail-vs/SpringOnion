using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpringOnion.Data.Entities
{
    public class Conversation
    {
        public string ConversationId { get; set; } = null!;
        public string Type { get; set; } = "Direct"; 
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
        public DateTime? LastActivityUtc { get; set; }

        public ICollection<ConversationParticipant> Participants { get; set; } = new List<ConversationParticipant>();
        public ICollection<Message> Messages { get; set; } = new List<Message>();
    }
}
