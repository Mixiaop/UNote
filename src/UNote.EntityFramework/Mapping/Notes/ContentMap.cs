using UNote.Domain.Notes;

namespace UNote.EntityFramework.Mapping.Notes
{

    public partial class ContentMap : UNoteEntityTypeConfiguration<Content>
    {
        public ContentMap()
        {
            this.ToTable(DbConsts.DbTableName.Notes_Contents);
            this.HasKey(x => x.Id);
            this.HasRequired(x => x.Node).WithMany().HasForeignKey(x => x.NodeId);
            this.HasRequired(x => x.User).WithMany().HasForeignKey(x => x.UserId);
            this.HasRequired(x => x.Team).WithMany().HasForeignKey(x => x.TeamId);

            this.Ignore(x => x.FormatPreviewUrl);
            this.Ignore(x => x.FormatFileSize);
            this.Ignore(x => x.FormatCreationTime);
            this.Ignore(x => x.FormatLastModificationTime);
            this.Ignore(x => x.FormatTags);
            this.Ignore(x => x.FormatNodeType);
            this.Ignore(x => x.Followers);
            this.Ignore(x => x.ContentItems);
        }
    }
}
