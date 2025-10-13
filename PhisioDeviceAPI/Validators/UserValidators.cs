using FluentValidation;
using PhisioDeviceAPI.DTOs.Auth;
using PhisioDeviceAPI.DTOs.User;

namespace PhisioDeviceAPI.Validators
{
    public class RegisterUserValidator : AbstractValidator<UserDTO>
    {
        public RegisterUserValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MinimumLength(2)
                .MaximumLength(100)
                .Must(name => name == name.Trim())
                .WithMessage("Name must not start or end with spaces")
                .Matches("^[\\p{L} .'\\-]+$")
                .WithMessage("Name can contain letters, spaces, apostrophes, dots and hyphens only");

            RuleFor(x => x.Email)
                .NotEmpty()
                .MaximumLength(255)
                .Must(email => email == email.Trim())
                .WithMessage("Email must not start or end with spaces")
                .Matches("^[A-Za-z0-9._%+-]+@gmail\\.com$")
                .WithMessage("Email must be a valid Gmail address (e.g., name@gmail.com)");

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(6);
        }
    }

    public class LoginValidator : AbstractValidator<LoginDTO>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}


