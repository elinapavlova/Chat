namespace Models.Dtos.File
{
    public class FileRequestDto
    {
        public byte[] File { get; init; }
        public string Name { get; set; }
        public string ContentType { get; set; }
    }
}