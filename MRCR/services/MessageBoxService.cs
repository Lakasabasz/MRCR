using System.Windows;

namespace MRCR.services;

public enum MessageMode
{
    Error,
    Warning,
    Info,
    Success
}

public interface IMessageBoxService
{
    public void Show(string message);
    public void Show(string message, string caption, MessageBoxButton button, MessageBoxImage icon);
    public void Show(string message, string caption, MessageMode mode);
}

public class MessageBoxService : IMessageBoxService
{
    public void Show(string message)
    {
        MessageBox.Show(message);
    }

    public void Show(string message, string caption, MessageBoxButton button, MessageBoxImage icon)
    {
        MessageBox.Show(message, caption, button, icon);
    }

    public void Show(string message, string caption, MessageMode mode)
    {
        MessageBoxImage icon = MessageBoxImage.None;
        MessageBoxButton button = MessageBoxButton.OK;

        switch (mode)
        {
            case MessageMode.Error:
                icon = MessageBoxImage.Error;
                break;
            case MessageMode.Warning:
                icon = MessageBoxImage.Warning;
                break;
            case MessageMode.Info:
                icon = MessageBoxImage.Information;
                break;
            case MessageMode.Success:
                icon = MessageBoxImage.Information;
                break;
        }

        MessageBox.Show(message, caption, button, icon);
    }
}