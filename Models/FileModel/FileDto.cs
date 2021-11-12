namespace Models.FileModel
{
    public class FileDto
    {
        public byte[] File { get; init; }
        public string Name { get; set; }
        public string ContentType { get; set; }
    }
}