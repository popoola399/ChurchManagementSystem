namespace ChurchManagementSystem.Core.Mediators
{
    public interface IStaticDispatcher
    {
        object Dispatch(object request);
    }
}