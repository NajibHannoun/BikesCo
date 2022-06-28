using BikesTest.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikesTest.configs
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(o => o.id);

            builder.HasOne(o => o.user)
                   .WithOne(p => p.customer)
                   .HasForeignKey<Customer>(o => o.user_id)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasData(new Customer
            {
                id = 1,
                user_id = 1,
                isCurrentlyBiking = false,
                numberOfBikesRented = 0,
                timeBiked = 0,
            }) ;

        }
    }
}
