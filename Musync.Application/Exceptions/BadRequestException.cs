using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;

namespace Musync.Application.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string message) : base(message) { }

        public BadRequestException(string message, ValidationResult validationResult) : base(message)
        {
            ValidationErrors = validationResult.ToDictionary();
        }

        public BadRequestException(string message, IEnumerable<IdentityError> errors) : base(message)
        {
            IDictionary<string, string[]> errorsDict = errors
                .GroupBy(e => e.Code)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.Description).ToArray());
            ValidationErrors = errorsDict;
        }

        public IDictionary<string, string[]>? ValidationErrors { get; set; }
    }
}
