namespace UNote.Services.Users.Dto
{
    public class UpdateProfileInput : U.Application.Services.Dto.IInputDto
    {
        public int UserId { get; set; }

        public string NickName { get; set; }

        public int AvatarId { get; set; }

        public int PicX { get; set; }

        public int PicY { get; set; }

        public int PicW { get; set; }

        public int PicH { get; set; }
    }
}
