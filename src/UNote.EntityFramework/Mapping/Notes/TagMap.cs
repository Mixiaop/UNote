using UNote.Domain.Notes;

namespace UNote.EntityFramework.Mapping.Notes
{

    public partial class TagMap : UNoteEntityTypeConfiguration<Tag>
    {
        public TagMap()
        {
            this.ToTable(DbConsts.DbTableName.Notes_Tags);
            this.HasKey(x => x.Id);

            this.HasRequired(x => x.Node).WithMany().HasForeignKey(x => x.NodeId);
        }
    }
}
