namespace PhotoStudio.app.Services
{
    public class ToastService
    {
        public event Action<string, ToastType>? OnShow;

        public void Success(string message)
            => OnShow?.Invoke(message, ToastType.Success);

        public void Error(string message)
            => OnShow?.Invoke(message, ToastType.Error);

        public void Info(string message)
            => OnShow?.Invoke(message, ToastType.Info);

        public void Warning(string message)
            => OnShow?.Invoke(message, ToastType.Warning);
    }

    public enum ToastType
    {
        Success,
        Error,
        Info,
        Warning
    }
}
