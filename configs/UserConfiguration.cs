using BikesTest.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikesTest.configs
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(o => o.id);

            builder.HasIndex(o => o.username).IsUnique(true).HasDatabaseName("Ix_unique_username");
            builder.HasIndex(o => o.email).IsUnique(true).HasDatabaseName("Ix_unique_email");

            builder.HasData(new User
            {
                id = 1,
                birthday = DateTime.Parse("05/05/2000"),
                email = "test1@test.com",
                firstName = "test1",
                lastName = "test1",
                username = "test1",
                password = Service.LoginServices.HashPassword("test1", DateTime.Parse("05/05/2000").ToString("MM/dd/yyyy"))
            });

            builder.HasData(new User
            {
                id = 2,
                birthday = DateTime.Parse("05/05/2000"),
                email = "admin@admin.com",
                firstName = "admin",
                lastName = "admin",
                username = "admin",
                password = Service.LoginServices.HashPassword("admin", DateTime.Parse("05/05/2000").ToString("MM/dd/yyyy"))
            });


        }


    }
}
