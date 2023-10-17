using ImageSystem.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ImageSystem.Infrastructure.Configuration;

internal class UserTypeConfiguration : IdentityTypeConfiguration<User>
{
    protected override void AddConfigure(EntityTypeBuilder<User> builder)
    {

        builder.Metadata.FindNavigation("Images")
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}
