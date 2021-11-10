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
    public class UserRoomRepository : BaseRepository<UserRoom, BaseFilter>,  IUserRoomRepository
    {
        private readonly AppDbContext _context;

        public UserRoomRepository(AppDbContext context, PagingOptions options) : base(context, options)
        {
            _context = context;
        }

        public async Task<List<Room>> GetRoomsByUserId(int userId, int page, int pageSize)
        {
            var rooms = new List<Room>();
            var roomsUserIn = _context.UsersRooms
                .Where(ur => ur.UserId == userId && ur.DateComeOut == null);
            
            var filter = new BaseFilter
            {
                Paging = new FilterPagingDto {PageNumber = page, PageSize = pageSize}
            };
            
            var filteredUsersRooms = await GetFilteredSource(roomsUserIn, filter);

            foreach (var room in filteredUsersRooms)
            {
                room.Room = await _context.Rooms.FirstOrDefaultAsync(r => r.Id == room.RoomId);
                rooms.Add(room.Room);
            }

            return rooms;
        }
        
        public async Task<List<User>> GetUsersByRoomId(int roomId, int page, int pageSize)
        {
            var users = new List<User>();
            var usersInRoom = _context.UsersRooms.Where(ur => ur.RoomId == roomId && ur.DateComeOut == null);
            
            var filter = new BaseFilter
            {
                Paging = new FilterPagingDto {PageNumber = page, PageSize = pageSize},
                Sort = new FilterSortDto {ColumnName = "Id", IsDescending = true}
            };
            
            var filteredUsersChats = await GetFilteredSource(usersInRoom, filter);
            
            foreach (var user in filteredUsersChats)
            {
                user.User = await _context.Users.FirstOrDefaultAsync(u => u.Id == user.UserId);
                users.Add(user.User);
            }

            return users;
        }

        public async Task<UserRoom> CheckUserInRoom(int userId, int roomId)
        {
            var userRoom = await _context.UsersRooms
                .OrderByDescending(ur => ur.Id)
                .FirstOrDefaultAsync(ur => 
                    ur.UserId == userId && 
                    ur.RoomId == roomId && 
                    ur.DateComeOut == null);

            return userRoom;
        }
        
        public async Task<UserRoom> ComeOutOfRoom(int userId, int roomId)
        {
            var userRoom = await CheckUserInRoom(userId, roomId);

            if (userRoom == null) 
                return null;
            
            userRoom.DateComeOut = DateTime.Now;
            await _context.SaveChangesAsync();

            return userRoom;
        }
    }
}