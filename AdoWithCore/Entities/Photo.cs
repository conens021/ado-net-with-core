namespace AdoWithCore.Entities
{
    public class Photo
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int GalleryId { get; set; }
    }
}
