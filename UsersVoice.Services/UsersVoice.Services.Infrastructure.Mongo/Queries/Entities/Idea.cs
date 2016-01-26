using System;
using System.Collections.Generic;

namespace UsersVoice.Infrastructure.Mongo.Queries.Entities
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
        public string AreaTitle { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid AuthorId { get; set; }
        public string AuthorCompleteName { get; set; }
        public DateTime CreationDate { get; set; }
        public IList<IdeaVote> Votes { get; private set; }
        public int TotalPoints { get; set; }
        public int TotalComments { get; set; }
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
        public string VoterCompleteName { get; set; }
        public Guid VoterId { get; set; }
        public short Points { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
