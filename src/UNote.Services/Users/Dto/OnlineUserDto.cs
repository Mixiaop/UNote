
namespace UNote.Services.Users.Dto
{
    public class OnlineUserDto : U.Application.Services.Dto.IDto
    {
        public int UserId { get; set; }
        public string NickName { get; set; }
    }
}
