using UNote.Domain.Users;

namespace UNote.EntityFramework.Mapping.Users
{
    public partial class UserBriefMap : UNoteEntityTypeConfiguration<UserBrief>
    {
        public UserBriefMap()
        {
            this.ToTable(DbConsts.DbTableName.Users_Users);
            this.HasKey(x => x.Id);

            this.Ignore(x => x.FormatNickName);
        }
    }
}
