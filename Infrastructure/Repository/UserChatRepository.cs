using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database;
using Infrastructure.Contracts;
using Infrastructure.Filter;
using Infrastructure.Options;
using Infrastructure.Repository.Base;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Infrastructure.Repository
{
    public class UserChatRepository : BaseRepository<UserChat, BaseFilter>,  IUserChatRepository
    {
        private readonly AppDbContext _context;

        public UserChatRepository(AppDbContext context, PagingOptions options) : base(context, options)
        {
            _context = context;
        }

        public async Task<List<Chat>> GetChatsByUserId(int userId, BaseFilterDto filter)
        {
            var chats = new List<Chat>();
            var chatsUserIn = _context.UsersChats
                .Where(ur => ur.UserId == userId && ur.DateComeOut == null);

            var filteredUsersChats = await GetFilteredSource(chatsUserIn, filter);

            foreach (var userChat in filteredUsersChats)
            {
                userChat.Chat = await _context.Chats.FirstOrDefaultAsync(r => r.Id == userChat.ChatId);
                chats.Add(userChat.Chat);
            }

            return chats;
        }

        public async Task<List<User>> GetUsersInChat(int chatId)
        {
            var users = new List<User>();
            var usersInChat = await _context.UsersChats
                .Where(ur => ur.ChatId == chatId && ur.DateComeOut == null)
                .OrderByDescending(ur => ur.Id)
                .ToListAsync();

            foreach (var user in usersInChat)
            {
                user.User = await _context.Users.FirstOrDefaultAsync(u => u.Id == user.UserId);
                users.Add(user.User);
            }

            return users;
        }

        public async Task<UserChat> CheckUserInChat(int userId, int chatId)
        {
            var userChat = await _context.UsersChats
                .OrderByDescending(ur => ur.Id)
                .FirstOrDefaultAsync(ur => 
                    ur.UserId == userId && 
                    ur.ChatId == chatId && 
                    ur.DateComeOut == null);

            return userChat;
        }

        public async Task<UserChat> ComeOutOfChat(int userId, int chatId)
        {
            var userChat = await CheckUserInChat(userId, chatId);

            if (userChat == null) 
                return null;
            
            userChat.DateComeOut = DateTime.Now;
            await _context.SaveChangesAsync();

            return userChat;
        }
    }
}