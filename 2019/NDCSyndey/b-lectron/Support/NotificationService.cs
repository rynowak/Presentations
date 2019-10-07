using System.Threading.Tasks;
using ElectronNET.API;
using ElectronNET.API.Entities;

namespace b_lectron
{
    public class NotificationService
    {
        public Task ShowAsync(string title, string body)
        {
            return ShowAsync(new NotificationOptions(title, body));
        }

        public Task ShowAsync(NotificationOptions options)
        {
            Electron.Notification.Show(options);
            return Task.CompletedTask;
        }
    }
}
