namespace ChurchManagementSystem.Core.Mediator
{
    public class AddResponse
    {
        /// <summary>
        /// The unique identifier of the entity that was added.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// An optional message for the Add response
        /// </summary>
        public string Message { get; set; }
    }
}