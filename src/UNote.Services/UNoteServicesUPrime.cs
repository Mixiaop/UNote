using System.Reflection;
using U;
using U.UPrimes;
using U.Hangfire;
using U.BackgroundJobs;
using UNote.Services.Mapping;

namespace UNote.Services
{
    public class UNoteServicesUPrime : UPrime
    {
        public override void Initialize()
        {
            Engine.IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
            CustomDtoMapper.CreateMappings();

            Engine.Configuration.BackgroundJob.UseHangfire();
        }
    }
}
