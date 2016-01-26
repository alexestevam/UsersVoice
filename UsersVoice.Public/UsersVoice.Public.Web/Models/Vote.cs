using System;

namespace UsersVoice.Public.Web.Models
{
    public class Vote
    {
        public Guid IdeaId { get; set; }

        public int Points { get; set; }

        public Guid VoterId { get; set; }

        public DateTime CreationDate { get; set; }
    }
}