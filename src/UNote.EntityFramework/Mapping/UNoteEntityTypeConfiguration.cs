using System;
using System.Data.Entity.ModelConfiguration;

namespace UNote.EntityFramework.Mapping
{
    public abstract class UNoteEntityTypeConfiguration<T> : EntityTypeConfiguration<T> where T : class
    {
        protected UNoteEntityTypeConfiguration()
        {
            PostInitialize();
        }

        protected virtual void PostInitialize()
        {

        }
    }
}
