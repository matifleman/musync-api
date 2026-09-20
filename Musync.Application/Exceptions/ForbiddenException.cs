namespace Musync.Application.Exceptions
{
    /// <summary>
    /// The caller is authenticated but isn't allowed to act on this resource - e.g. editing a
    /// post they didn't write. Distinct from BadRequestException so ownership failures map to
    /// 403 instead of 400, and from UnauthorizedAccessException so the client isn't told to
    /// log in again over something a fresh token wouldn't fix.
    /// </summary>
    public sealed class ForbiddenException : Exception
    {
        public ForbiddenException(string message) : base(message) { }
    }
}
