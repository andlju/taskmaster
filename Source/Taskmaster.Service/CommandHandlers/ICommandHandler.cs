namespace Taskmaster.Service.CommandHandlers
{
    public interface ICommandHandler<T>
    {
        void Handle(T command);
    }
}