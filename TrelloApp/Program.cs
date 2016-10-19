namespace TrelloApp
{
    using System;
    using System.Linq;
    using System.Threading;

    using Manatee.Trello;
    using Manatee.Trello.ManateeJson;
    using Manatee.Trello.WebApi;

    internal class Program
    {
        private static void Main()
        {
            Initialize();
            var notifications = Member.Me.Notifications;
            while (true)
            {
                if (IsNotificationsUnread(notifications))
                {
                    foreach (var notification in notifications)
                    {
                        if (notification.IsUnread == null || !notification.IsUnread.Value) continue;
                        Console.WriteLine(notification);
                        notification.IsUnread = false;
                    }
                }

                Thread.Sleep(1000);
            }

            // ReSharper disable once FunctionNeverReturns
        }

        private static bool IsNotificationsUnread(ReadOnlyNotificationCollection notifications)
        {
            if (!notifications.Any()) return false;
            foreach (var notification in notifications)
            {
                if (notification.IsUnread != null && notification.IsUnread.Value) return true;
            }

            return false;
        }

        private static void Initialize()
        {
            var serializer = new ManateeSerializer();
            TrelloConfiguration.Serializer = serializer;
            TrelloConfiguration.Deserializer = serializer;
            TrelloConfiguration.JsonFactory = new ManateeFactory();
            TrelloConfiguration.RestClientProvider = new WebApiClientProvider();
            TrelloAuthorization.Default.AppKey = "0b27c6aa5adb2a59af4260e014de24d8";
            TrelloAuthorization.Default.UserToken = "3b18fd40230e3a4be83f2e1dca58101bfbd1325560a9c500794083f0eb52077e";
            TrelloConfiguration.ExpiryTime = TimeSpan.FromSeconds(0.5);
        }
    }
}