using System;
using MediatR;

namespace UsersVoice.Services.API.CQRS.Commands
{
    public class CreateUser : IAsyncNotification
    {
        public CreateUser(Guid userId, string firstName, string lastName, string email)
            :this(userId, firstName, lastName, email, false)
        {
        }

        public CreateUser(Guid userId, string firstName, string lastName, string email, bool isAdmin)
        {
            if (userId == Guid.Empty) throw new ArgumentNullException("userId");
            if (string.IsNullOrWhiteSpace(firstName)) throw new ArgumentNullException("firstName");
            if (string.IsNullOrWhiteSpace(lastName)) throw new ArgumentNullException("lastName");
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentNullException("email");
            UserId = userId;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            IsAdmin = isAdmin;
        }

        public Guid UserId { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public bool IsAdmin { get; private set; }
    }
}
