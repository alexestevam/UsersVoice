using System;
using MediatR;

namespace UsersVoice.Services.API.CQRS.Commands
{
    public class CreateIdea : IAsyncNotification
    {
        public CreateIdea(Guid ideaId, Guid areaId, Guid authorId, string title, string description)
        {
            if (ideaId == Guid.Empty) throw new ArgumentNullException("ideaId");
            if (areaId == Guid.Empty) throw new ArgumentNullException("areaId");
            if (authorId == Guid.Empty) throw new ArgumentNullException("authorId");
            if (string.IsNullOrWhiteSpace(title)) throw new ArgumentNullException("title");

            this.Title = title;

            this.Description = description ??string.Empty;

            this.AuthorId = authorId;
            this.AreaId = areaId;
            this.IdeaId = ideaId;
        }

        public string Title { get; private set; }
        public string Description { get; set; }

        public Guid AuthorId { get; private set; }
        public Guid AreaId { get; private set; }
        public Guid IdeaId { get; private set; }
    }
}
