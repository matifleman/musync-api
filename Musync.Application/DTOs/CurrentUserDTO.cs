namespace Musync.Application.DTOs
{
    public record CurrentUserDTO : UserDTO
    {
        public required string Email { get; init; }
        // Self-facing only, like Email: whether the first-run flow has been finished or skipped.
        public bool OnboardingCompleted { get; init; }
    };
}
