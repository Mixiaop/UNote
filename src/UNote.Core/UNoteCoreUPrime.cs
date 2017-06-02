using System.Reflection;
using U.UPrimes;

namespace UNote
{

    public class UNoteCoreUPrime : UPrime
    {
        public override void Initialize()
        {
            Engine.IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}

