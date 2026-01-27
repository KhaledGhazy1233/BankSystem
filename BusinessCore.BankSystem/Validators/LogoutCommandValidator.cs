using BusinessCore.BankSystem.Features.User.Commands.Requests;
using FluentValidation;

namespace BusinessCore.BankSystem.Validators
{
    public class LogoutCommandValidator : AbstractValidator<LogoutCommand>
    {
        public LogoutCommandValidator()
        {

            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("UserId must be a valid positive integer.");

            // 2. التحقق الشرطي للـ RefreshToken
            // لو المستخدم عاوز يخرج من جهاز واحد (RevokeAllTokens = false)
            // يبقى لازم يبعت الـ RefreshToken اللي عاوز يلغيه
            RuleFor(x => x.RefreshToken)
                .NotEmpty().WithMessage("Refresh token is required to logout from this device.")
                .When(x => x.RevokeAllTokens == false)
                .WithName("Security Token");

            // 3. في حالة الـ Logout All
            // بنسمح إن الـ RefreshToken يكون فاضي لأننا هنلغي كل حاجة بناءً على الـ UserId
            RuleFor(x => x.RefreshToken)
                .Empty().WithMessage("Refresh token should be empty when revoking all sessions.")
                .When(x => x.RevokeAllTokens == true);


        }
    }
}
