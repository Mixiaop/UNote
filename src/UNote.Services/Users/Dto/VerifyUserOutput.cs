using UNote.Domain.Users;

namespace UNote.Services.Users.Dto
{
    public class VerifyUserOutput : U.Application.Services.Dto.IOutputDto
    {
        public bool Success { get; set; }

        public string ErrorMessage { get; set; }

        public User User { get; set; }
    }
}
