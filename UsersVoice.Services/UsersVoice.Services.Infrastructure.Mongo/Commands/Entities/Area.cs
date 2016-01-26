using System;

namespace UsersVoice.Infrastructure.Mongo.Commands.Entities
{
    public class Area
    {
        public Guid Id { get; set; }
        public Guid AuthorId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
    }
}