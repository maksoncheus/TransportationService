using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;
using TransportationService.WEB.Data.Entities;

namespace TransportationService.WEB.Models
{
    public class CustomPasswordValidator : IPasswordValidator<User>
    {
        private const int MIN_LENGTH = 8;
        private const int MAX_LENGTH = 30;
        private const string PATTERN = "^[A-Za-z0-9]+$";
        public Task<IdentityResult> ValidateAsync(UserManager<User> manager, User user, string? password)
        {
            List<IdentityError> errors = new List<IdentityError>();

            if (password == null)
            {
                errors.Add(new IdentityError
                {
                    Description = $"Пароль не указан"
                });
            }
            if (password?.Length < MIN_LENGTH)
            {
                errors.Add(new IdentityError
                {
                    Description = $"Минимальная длина пароля: {MIN_LENGTH} символов"
                });
            }
            if (password?.Length > MAX_LENGTH)
            {
                errors.Add(new IdentityError
                {
                    Description = $"Максимальная длина пароля: {MAX_LENGTH} символов"
                });
            }

            if (!Regex.IsMatch(password ?? "", PATTERN))
            {
                errors.Add(new IdentityError
                {
                    Description = "Пароль должен состоять только из букв латинского алфавита и цифр"
                });
            }
            if (password != null && !password.Any(char.IsUpper))
            {
                errors.Add(new IdentityError
                {
                    Description = "Пароль должен содержать как минимум одну заглавную букву"
                });
            }
            if (password != null && !password.Any(char.IsDigit))
            {
                errors.Add(new IdentityError
                {
                    Description = "Пароль должен содержать как минимум одну цифру"
                });
            }
            return Task.FromResult(errors.Count == 0 ?
                IdentityResult.Success : IdentityResult.Failed(errors.ToArray()));
        }
    }
}
