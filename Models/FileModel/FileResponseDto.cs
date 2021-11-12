namespace Models.FileModel
{
    public class FileResponseDto : BaseModel
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public int MessageId { get; set; }
        public string ContentType { get; set; }
    }
}