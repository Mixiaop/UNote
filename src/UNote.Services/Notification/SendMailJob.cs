using U;
using U.Net.Mail;
using U.BackgroundJobs;

namespace UNote.Services.Notification
{
    public class SendMailJob:  BackgroundJob<SendMailJobArgs>, U.Dependency.ITransientDependency
    {
        public override void Execute(SendMailJobArgs args)
        {
            var emailSender = UPrimeEngine.Instance.Resolve<IEmailSender>();
            emailSender.SendAsync(args.Email, args.Title, args.Body);
        }
    }
}
