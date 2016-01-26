using System;
using System.Collections.Generic;

namespace UsersVoice.Infrastructure.Mongo.Commands.Entities
{
    public class Idea
    {
        public Idea()
        {
            this.Votes = new List<IdeaVote>();
            this.Status = IdeaStatus.Nothing;
        }

        public Guid Id { get; set; }
        public Guid AreaId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid AuthorId { get; set; }
        public DateTime CreationDate { get; set; }
        public ICollection<IdeaVote> Votes { get; private set; }

        public IdeaStatus Status { get; set; }
        public enum IdeaStatus
        {
            Nothing = 0,
            Cancelled = 1,
            Approved = 2,
            Implemented = 3
        }
    }

    public class IdeaVote
    {
        public Guid VoterId { get; set; }
        public short Points { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
