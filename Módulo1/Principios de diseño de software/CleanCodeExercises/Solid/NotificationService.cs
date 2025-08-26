
namespace Solid
{
    public class NotificationService
    {
        public void SendEmail(string email, string message)
        {
            Console.WriteLine($"Sending Email to {email}: {message}");
        }

        public void sendSMS(string phoneNumber, string message)
        {
            Console.WriteLine($"Sending SMS to {phoneNumber}: {message}");
        }




    }

    public class LogNofication
    {
        public void LogNotifiacation(string message)
        {
            Console.WriteLine($"Logging notification: {message}");
        }
    }
}
