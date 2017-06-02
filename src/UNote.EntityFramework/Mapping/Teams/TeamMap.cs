using UNote.Domain.Teams;
namespace UNote.EntityFramework.Mapping.Teams
{
    public partial class TeamMap : UNoteEntityTypeConfiguration<Team>
    {
        public TeamMap()
        {
            this.ToTable(DbConsts.DbTableName.Teams_Teams);
            this.HasKey(x => x.Id);

            this.HasRequired(x => x.Commander).WithMany().HasForeignKey(x => x.CommanderId);
        }
    }
}
