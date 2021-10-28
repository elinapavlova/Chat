﻿using System.Collections.Generic;
using Models.Base;

namespace Models
{
    public class Room : BaseModel
    {
        public string Title { get; set; }
        public int UserId { get; set; }
        
        public User User { get; set; }
        public List<Chat> Chats { get; set; }
    }
}