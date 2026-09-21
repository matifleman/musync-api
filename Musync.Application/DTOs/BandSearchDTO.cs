namespace Musync.Application.DTOs
{
    public class BandSearchDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int MemberCount { get; set; }
        // Nullable because a band may have no picture yet. Added for the discover cards;
        // search results carry it too so the two band lists stay one shape.
        public string? ProfilePicture { get; set; }
    }
}
