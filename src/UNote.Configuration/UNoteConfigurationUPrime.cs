using System.Reflection;
using U.UPrimes;

namespace UNote.Configuration
{

    public class UNoteConfigurationUPrime : UPrime
    {
        public override void Initialize()
        {
            Engine.IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}

