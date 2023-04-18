using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Auth.Application.Models;

namespace Auth.Persistence.Data
{
    /// <summary>
    /// Configures the entity framework mappings for the User model.
    /// </summary>
    internal class UserEntityConfig : IEntityTypeConfiguration<UserModel>
    {
        /// <summary>
        /// Configures the entity framework mappings for the User model.
        /// </summary>
        /// <param name="builder">The entity type builder used to configure the model.</param>
        public void Configure(EntityTypeBuilder<UserModel> builder)
        {
            // Defines the primary key.
            builder.HasKey(x => x.Id);

            // Defines required fields and their maximum length.
            builder.Property(x => x.Email).IsRequired().HasMaxLength(225);
            builder.Property(x => x.Role).IsRequired();
            builder.Property(x => x.UserName).IsRequired().HasMaxLength(100);

            // Defines a unique index on the email field.
            builder.HasIndex(x => x.Email).IsUnique();

            // Defines required password fields.
            builder.Property(x => x.PasswordHash).IsRequired();
            builder.Property(x => x.PasswordSalt).IsRequired();

            // Sets up a global query filter to exclude deleted users.
            builder.HasQueryFilter(x => x.IsFalseDeleted == false);
        }
    }
}
