using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JoeBot.Bots
{
    public class TrackConversation
    {
        // Tracking what users have said to the bot
        public List<string> ConversationList { get; } = new List<string>();

        // The number of conversational turns that have occurred          
        public int TurnNumber { get; set; } = 0;

        // Create concurrency control where this is used.  
        public string ETag { get; set; } = "*";
    }
}
