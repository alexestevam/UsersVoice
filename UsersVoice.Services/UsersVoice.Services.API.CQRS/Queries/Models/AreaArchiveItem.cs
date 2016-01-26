using System;

namespace UsersVoice.Services.API.CQRS.Queries.Models
{
    public class AreaArchiveItem
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid AuthorId { get; set; }

        public string AuthorCompleteName { get; set; }
    }
}
