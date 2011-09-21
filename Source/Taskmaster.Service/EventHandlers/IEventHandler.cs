namespace Taskmaster.Service.EventHandlers
{
    public interface IEventHandler<T>
    {
        void Handle(T evt);
    }
}