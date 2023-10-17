using ImageSystem.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ImageSystem.Infrastructure.Configuration;

internal abstract class IdentityTypeConfiguration<T> : IEntityTypeConfiguration<T> where T : Identity
{
    protected virtual string TableName => typeof(T).Name;
    protected abstract void AddConfigure(EntityTypeBuilder<T> builder);

    public void Configure(EntityTypeBuilder<T> builder)
    {
        builder.ToTable(TableName);

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        
        AddConfigure(builder);
    }

}
