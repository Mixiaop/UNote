using System.Reflection;
using U.UPrimes;
using U.Hangfire;
using UNote.Configuration;

namespace UNote.Web.Infrastructure
{
    [DependsOn(
        typeof(UHangfireUPrime),
        typeof(UNoteConfigurationUPrime))]
    public class UNoteUPrime : UPrime
    {
        public override void PreInitialize()
        {
            base.PreInitialize();

            Engine.Configuration.BackgroundJob.IsJobExecutionEnabled = true;
        }
        public override void Initialize()
        {
            Engine.IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            
        }
    }
}