using Database;
using Infrastructure.Contracts;
using Infrastructure.Filter;
using Infrastructure.Repository.Base;
using Models;

namespace Infrastructure.Repository
{
    public class UserChatRepository : BaseRepository<UserChat, BaseFilter>,  IUserChatRepository
    {
        private readonly AppDbContext _context;

        public UserChatRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}