using BikesTest.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikesTest.configs
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.HasKey(o => o.id);

            builder.Property(o => o.rentalDate).IsRequired();

            builder.Property(o => o.bicycle_Id).IsRequired(); //will be removed cuz front to back
            builder.Property(o => o.customer_Id).IsRequired(); //will be removed cuz front to back
            builder.Property(o => o.admin_Id).IsRequired(); //will be removed cuz front to back


            builder.HasOne(o => o.bicycle)
                   .WithMany(p => p.transactions)
                   .HasForeignKey(o => o.bicycle_Id)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(o => o.admin)
                   .WithMany(p => p.transactions)
                   .HasForeignKey(o => o.admin_Id)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(o => o.customer)
                   .WithMany(p => p.transactions)
                   .HasForeignKey(o => o.customer_Id)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
