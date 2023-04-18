namespace Auth.Application.Models
{
    /// <summary>
    /// Represents a base model with common properties.
    /// </summary>
    public class ModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModelBase"/> class.
        /// </summary>
        public ModelBase()
        {
            CreatedDate = DateTime.UtcNow;
        }

        /// <summary>
        /// Gets or sets the unique identifier of the model.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the creation date of the model in UTC time.
        /// </summary>
        public DateTime CreatedDate { get; set; }
    }
}
