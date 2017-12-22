using UNote.Domain.Notes;

namespace UNote.EntityFramework.Mapping.Notes
{

    public partial class ContentColumnMap : UNoteEntityTypeConfiguration<ContentColumn>
    {
        public ContentColumnMap()
        {
            this.ToTable(DbConsts.DbTableName.Notes_ContentColumns);
            this.HasKey(x => x.Id);
            this.HasRequired(x => x.Team).WithMany().HasForeignKey(x => x.TeamId);
            this.HasRequired(x => x.Node).WithMany().HasForeignKey(x => x.NodeId);
        }
    }
}
