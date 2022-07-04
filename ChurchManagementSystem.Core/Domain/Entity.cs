namespace ChurchManagementSystem.Core.Domain
{
    public interface IEntity
    {
        int Id { get; }
    }

    public class Entity : IEntity
    {
        public int Id { get; set; }
    }

    public interface IActive
    {
        bool IsActive { get; }
    }
}