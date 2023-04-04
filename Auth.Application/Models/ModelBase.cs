namespace Auth.Application.Models
{
    public class ModelBase
    {
        public ModelBase()
        {
            CreatedDate =DateTime.UtcNow;
        }
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        
    }
}
