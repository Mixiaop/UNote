using UNote.Domain.Users;

namespace UNote.EntityFramework.Mapping.Users
{
    public partial class UserVisitLogMap : UNoteEntityTypeConfiguration<UserVisitLog>
    {
        public UserVisitLogMap()
        {
            this.ToTable(DbConsts.DbTableName.Users_UserVisitLogs);
            this.HasKey(x => x.Id);

            this.HasRequired(x => x.Content).WithMany().HasForeignKey(x => x.ContentId);
            this.HasRequired(x => x.Node).WithMany().HasForeignKey(x => x.NodeId);
            this.HasRequired(x => x.User).WithMany().HasForeignKey(x => x.UserId);

            this.Ignore(x => x.FormatLastVisitTime);
        }
    }
}
