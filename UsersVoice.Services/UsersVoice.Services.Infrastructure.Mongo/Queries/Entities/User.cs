using System;

namespace UsersVoice.Infrastructure.Mongo.Queries.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompleteName
        {
            get { return string.Format("{0} {1}", this.FirstName, this.LastName); }
        }

        public string Email { get; set; }
        public DateTime RegistrationDate { get; set; }
        public int AvailablePoints{ get; set; }

        public bool IsAdmin { get; set; }
    }
}
