using System;

namespace UsersVoice.Infrastructure.Mongo.Queries.Entities
{
    public class IdeaComment
    {
        public Guid Id { get; set; }
        public Guid IdeaId { get; set; }
        public Guid AuthorId { get; set; }
        public string AuthorCompleteName { get; set; }
        public DateTime CreationDate { get; set; }
        public string Text { get; set; }
    }
}