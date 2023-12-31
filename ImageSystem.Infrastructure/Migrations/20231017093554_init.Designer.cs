﻿// <auto-generated />
using System;
using ImageSystem.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ImageSystem.Infrastructure.Migrations
{
    [DbContext(typeof(PostgreContext))]
    [Migration("20231017093554_init")]
    partial class init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ImageSystem.Domain.Friendship", b =>
                {
                    b.Property<Guid>("SenderId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("RecieverId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("SenderId", "RecieverId");

                    b.HasIndex("RecieverId");

                    b.ToTable("Friendship");
                });

            modelBuilder.Entity("ImageSystem.Domain.Image", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Image", (string)null);
                });

            modelBuilder.Entity("ImageSystem.Domain.User", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("User", (string)null);
                });

            modelBuilder.Entity("ImageSystem.Domain.Friendship", b =>
                {
                    b.HasOne("ImageSystem.Domain.User", "Reciever")
                        .WithMany("ReceivedFriendships")
                        .HasForeignKey("RecieverId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ImageSystem.Domain.User", "Sender")
                        .WithMany("SendFriendships")
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Reciever");

                    b.Navigation("Sender");
                });

            modelBuilder.Entity("ImageSystem.Domain.Image", b =>
                {
                    b.HasOne("ImageSystem.Domain.User", "User")
                        .WithMany("Images")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("ImageSystem.Domain.User", b =>
                {
                    b.HasOne("ImageSystem.Domain.User", null)
                        .WithMany("Friends")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("ImageSystem.Domain.User", b =>
                {
                    b.Navigation("Friends");

                    b.Navigation("Images");

                    b.Navigation("ReceivedFriendships");

                    b.Navigation("SendFriendships");
                });
#pragma warning restore 612, 618
        }
    }
}
