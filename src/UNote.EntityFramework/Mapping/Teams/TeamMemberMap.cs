using UNote.Domain.Teams;
namespace UNote.EntityFramework.Mapping.Teams
{
    public partial class TeamMemberMap : UNoteEntityTypeConfiguration<TeamMember>
    {
        public TeamMemberMap()
        {
            this.ToTable(DbConsts.DbTableName.Teams_TeamMembers);
            this.HasKey(x => x.Id);

            this.HasRequired(x => x.User).WithMany().HasForeignKey(x => x.UserId);
            this.HasRequired(x => x.Team).WithMany().HasForeignKey(x => x.TeamId);
        }
    }
}
