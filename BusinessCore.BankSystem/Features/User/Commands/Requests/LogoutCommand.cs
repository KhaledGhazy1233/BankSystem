using MediatR;

namespace BusinessCore.BankSystem.Features.User.Commands.Requests
{
    public class LogoutCommand : IRequest<Response<string>>
    {
        public int UserId { get; set; }
        public string RefreshToken { get; set; }
        public bool RevokeAllTokens { get; set; } = false;
    }
}
