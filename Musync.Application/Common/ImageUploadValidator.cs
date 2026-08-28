using Microsoft.AspNetCore.Http;

namespace Musync.Application.Common
{
    /// <summary>
    /// Shared image-upload validation rules (size, extension, magic-byte signature) used by
    /// every feature that accepts an uploaded image (post images, avatars).
    /// </summary>
    public static class ImageUploadValidator
    {
        public const long MaxFileSizeBytes = 5 * 1024 * 1024; // 5 MB

        private static readonly byte[] JpegSignature = { 0xFF, 0xD8, 0xFF };
        private static readonly byte[] PngSignature = { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };
        private static readonly byte[] GifSignature = { 0x47, 0x49, 0x46, 0x38 };
        private static readonly byte[] RiffSignature = { 0x52, 0x49, 0x46, 0x46 }; // "RIFF" - WEBP container
        private static readonly byte[] WebpMarker = { 0x57, 0x45, 0x42, 0x50 }; // "WEBP", at offset 8 of a RIFF file

        private static readonly HashSet<string> AllowedExtensions = new(StringComparer.OrdinalIgnoreCase)
        {
            ".jpg", ".jpeg", ".png", ".gif", ".webp"
        };

        public static bool HasAllowedExtension(string fileName) =>
            AllowedExtensions.Contains(Path.GetExtension(fileName));

        /// <summary>
        /// Confirms the file's actual bytes match a known image format, regardless of what its
        /// extension/content-type claim - rejects e.g. an .svg/.html file renamed to .jpg.
        /// </summary>
        public static async Task<bool> HasValidImageSignatureAsync(IFormFile file, CancellationToken cancellationToken)
        {
            byte[] header = new byte[12];
            await using Stream stream = file.OpenReadStream();
            int bytesRead = await stream.ReadAsync(header.AsMemory(0, header.Length), cancellationToken);

            if (bytesRead >= WebpMarker.Length && header.AsSpan(0, RiffSignature.Length).SequenceEqual(RiffSignature)
                && bytesRead >= 12 && header.AsSpan(8, WebpMarker.Length).SequenceEqual(WebpMarker))
                return true;

            return StartsWith(header, bytesRead, JpegSignature)
                || StartsWith(header, bytesRead, PngSignature)
                || StartsWith(header, bytesRead, GifSignature);
        }

        private static bool StartsWith(byte[] header, int bytesRead, byte[] signature) =>
            bytesRead >= signature.Length && header.AsSpan(0, signature.Length).SequenceEqual(signature);

        /// <summary>
        /// Generates a save-safe file name: a fresh GUID plus the (already validated) extension only -
        /// never any part of the client-supplied file name, which closes the path-traversal vector.
        /// </summary>
        public static string GenerateSafeFileName(string originalFileName) =>
            $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
    }
}
