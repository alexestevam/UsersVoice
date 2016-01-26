using System;

namespace UsersVoice.Infrastructure.Mongo.Commands.Entities
{
    public class IdeaComment
    {
        public Guid Id { get; set; }
        public Guid IdeaId { get; set; }
        public Guid AuthorId { get; set; }
        public DateTime CreationDate { get; set; }
        public string Text { get; set; }
    }
}