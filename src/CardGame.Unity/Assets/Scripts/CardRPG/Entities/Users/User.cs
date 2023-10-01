using Core.Entities;

namespace CardRPG.Entities.Users
{
    public class User
    {
        public User(UserId id)
        {
            Id = id;
        }

        public UserId Id { get; private set; }
    }

    public class UserId : EntityId { public UserId(string value) : base(value) {} }
}
