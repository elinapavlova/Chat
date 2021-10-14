using Models.Base;

namespace Models.Dtos.Room
{
    public class RoomDto : BaseModel
    {
        public string Title { get; set; }
        public int IdUser { get; set; }
    }
}