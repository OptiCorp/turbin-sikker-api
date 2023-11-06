namespace turbin.sikker.core.Services
{
    public interface ISocketService
    {
        Task SendMessage(string notification);
    }
}