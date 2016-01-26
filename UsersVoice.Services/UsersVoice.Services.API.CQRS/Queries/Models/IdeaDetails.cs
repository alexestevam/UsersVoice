using System;

namespace UsersVoice.Services.API.CQRS.Queries.Models
{
    public class IdeaDetails
    {
        public Guid Id { get; set; }
        public DateTime CreationDate { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public Guid AuthorId { get; set; }

        public string AuthorCompleteName { get; set; }

        public Guid AreaId { get; set; }
        public string AreaTitle { get; set; }

        public int TotalPoints { get; set; }
        public int TotalComments { get; set; }

        public IdeaStatus Status { get; set; }
    }

    public enum IdeaStatus
    {
        Pending,
        Cancelled,
        Approved,
        Implemented
    }
}
