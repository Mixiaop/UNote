using UNote.Domain.Notes;

namespace UNote.EntityFramework.Mapping.Notes
{

    public partial class ContentFollowerMap : UNoteEntityTypeConfiguration<ContentFollower>
    {
        public ContentFollowerMap()
        {
            this.ToTable(DbConsts.DbTableName.Notes_ContentFollowers);
            this.HasKey(x => x.Id);
            this.HasRequired(x => x.Content).WithMany().HasForeignKey(x => x.ContentId);
            this.HasRequired(x => x.User).WithMany().HasForeignKey(x => x.UserId);
        }
    }
}
