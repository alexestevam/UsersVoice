using System;

namespace UsersVoice.Services.API.CQRS.Queries.Models
{
    public class IdeaCommentArchiveItem
    {
        public Guid Id { get; set; }
        public string Text { get; set; }

        public Guid AuthorId { get; set; }
        public string AuthorCompleteName { get; set; }

        public Guid IdeaId { get; set; }
        public string IdeaTitle { get; set; }
    }
}
