using System;
using System.Reflection;
using System.Linq;
using System.Data.Entity;
using U;
using U.EntityFramework;
using UNote.Configuration;
using UNote.EntityFramework.Mapping;
using UNote.Domain.Notes;
using UNote.Domain.Users;
using UNote.Domain.Teams;

namespace UNote.EntityFramework
{
    public class UNodeDbContext : UDbContext
    {
        //public virtual IDbSet<Node> Nodes { get; set; }
        //public virtual IDbSet<Content> Contents { get; set; }
        //public virtual IDbSet<Tag> Tags { get; set; }

        //public virtual IDbSet<User> Users { get; set; }

        //public virtual IDbSet<Team> Teams { get; set; }

        //public virtual IDbSet<TeamMember> TeamMembers { get; set; }

        public UNodeDbContext(string nameOrConnectionString)
            : base(UPrimeEngine.Instance.Resolve<DatabaseSettings>().SqlConnectionString)
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var typesToRegister = Assembly.GetExecutingAssembly().GetTypes()
            .Where(type => !String.IsNullOrEmpty(type.Namespace))
            .Where(type => type.BaseType != null && type.BaseType.IsGenericType &&
                type.BaseType.GetGenericTypeDefinition() == typeof(UNoteEntityTypeConfiguration<>));
            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.Configurations.Add(configurationInstance);
            }

            base.OnModelCreating(modelBuilder);
        }
    }
}
