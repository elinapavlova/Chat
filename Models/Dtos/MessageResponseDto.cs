using Models.Base;

namespace Models.Dtos
{
    public class MessageResponseDto : BaseModel
    {
        public string Text { get; set; }
        public int IdUser { get; set; }
        public int IdRoom { get; set; }
    }
}