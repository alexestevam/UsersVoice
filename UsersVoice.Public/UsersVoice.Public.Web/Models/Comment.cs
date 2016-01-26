using System;

namespace UsersVoice.Public.Web.Models
{
    public class Comment
    {
        public Guid CommentId { get; set; }
        public Guid IdeaId { get; set; }
        public string Text { get; set; }
        public Guid AuthorId { get; set; }
        public string AuthorCompleteName { get; set; }
        public DateTime CreationDate { get; set; }
    }
}