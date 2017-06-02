using UNote.Domain.Users;

namespace UNote.EntityFramework.Mapping.Users
{
    public partial class UserMap : UNoteEntityTypeConfiguration<User>
    {
        public UserMap()
        {
            this.ToTable(DbConsts.DbTableName.Users_Users);
            this.HasKey(x => x.Id);

            //this.HasRequired(x => x.LastEditUser).WithMany().HasForeignKey(x => x.LastEditUserId);

            this.Ignore(x => x.UserType);
            this.Ignore(x => x.FormatNickName);
        }
    }
}
