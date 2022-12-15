using chatgpt.Services;
using Microsoft.Toolkit.Uwp.Notifications;

namespace chatgpt.Platforms.Windows;

public class NotificationService : INotificationService
{
    public void ShowNotification(string title, string body)
    {
        new ToastContentBuilder()
            .AddToastActivationInfo(null, ToastActivationType.Foreground)
            .AddAppLogoOverride(new Uri("ms-appx:///Assets/dotnet_bot.png"))
            .AddText(title, hintStyle: AdaptiveTextStyle.Header)
            .AddText(body, hintStyle: AdaptiveTextStyle.Body)
            .Show();
    }
}
