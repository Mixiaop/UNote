using UNote.Domain.Notes;

namespace UNote.EntityFramework.Mapping.Notes
{

    public partial class NodeMap : UNoteEntityTypeConfiguration<Node>
    {
        public NodeMap()
        {
            this.ToTable(DbConsts.DbTableName.Notes_Nodes);
            this.HasKey(x => x.Id);

            this.HasRequired(x => x.Team).WithMany().HasForeignKey(x => x.TeamId);

            this.Ignore(x => x.Parent);
            this.Ignore(x => x.NodeType);
            this.Ignore(x => x.ListShowType);
            //this.HasRequired(x => x.Parent)
                //.WithMany()
                //.HasForeignKey(x => x.ParentId)
                //.WillCascadeOnDelete(false);
            
        }
    }
}
