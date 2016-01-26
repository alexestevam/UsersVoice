using System;
using MediatR;

namespace UsersVoice.Services.API.CQRS.Commands
{
    public class CreateArea : IAsyncNotification
    {
        public CreateArea(Guid areaId, Guid authorId, string title, string description)
        {
            if (areaId == Guid.Empty) throw new ArgumentNullException("areaId");
            if (string.IsNullOrWhiteSpace(title)) throw new ArgumentNullException("title");

            this.Title = title;
            this.Description = description ??string.Empty;

            this.AuthorId = authorId;
            this.AreaId = areaId;
        }

        public string Title { get; private set; }
        public string Description { get; set; }

        public Guid AuthorId { get; private set; }
        public Guid AreaId { get; private set; }
    }
}
