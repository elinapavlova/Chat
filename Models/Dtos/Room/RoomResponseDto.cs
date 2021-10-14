using System.Collections.Generic;
using Models.Base;

namespace Models.Dtos.Room
{
    public class RoomResponseDto : BaseModel
    {
        public string Title { get; set; }
        public int IdUser { get; set; }

        public ICollection<MessageResponseDto> Messages { get; set; }
    }
}