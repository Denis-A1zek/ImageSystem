using ImageSystem.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ImageSystem.Infrastructure.Configuration;

internal class ImageTypeConfiguration : IdentityTypeConfiguration<Image>
{
    protected override void AddConfigure(EntityTypeBuilder<Image> builder)
    {
       
    }
}
