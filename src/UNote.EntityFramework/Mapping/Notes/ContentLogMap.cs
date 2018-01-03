using UNote.Domain.Notes;

namespace UNote.EntityFramework.Mapping.Notes
{

    public partial class ContentLogMap : UNoteEntityTypeConfiguration<ContentLog>
    {
        public ContentLogMap()
        {
            this.ToTable(DbConsts.DbTableName.Notes_ContentLogs);
            this.HasKey(x => x.Id);
            this.HasRequired(x => x.User).WithMany().HasForeignKey(x => x.UserId);
        }
    }
}
