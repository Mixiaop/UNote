using System.Reflection;
using U.UPrimes;

namespace UNote.EntityFramework
{
    public class UNoteEntityFrameworkUPrime : UPrime
    {
        public override void Initialize()
        {
            Engine.IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
