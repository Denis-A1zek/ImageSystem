using ImageSystem.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace ImageSystem.Infrastructure.Configuration;

internal class FriendshipTypeConfiguration : IEntityTypeConfiguration<Friendship>
{
    public void Configure(EntityTypeBuilder<Friendship> builder)
    {

        builder.HasKey(f => new { f.SenderId, f.RecieverId });

        builder.HasOne(f => f.Sender)
               .WithMany(u => u.SendFriendships)
               .HasForeignKey(f => f.SenderId)
               .OnDelete(DeleteBehavior.Cascade); 

        builder.HasOne(f => f.Reciever)
               .WithMany(u => u.ReceivedFriendships)
               .HasForeignKey(f => f.RecieverId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
