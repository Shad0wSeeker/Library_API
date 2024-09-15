using Library.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Infrastructure.EntityConfigurations
{
    public class UserEntityConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);

            builder.HasMany(u => u.BorrowedBooks)
                .WithOne();
           
            builder.HasIndex(u => u.Email)
                .IsUnique();

            builder.Property(u => u.Role).IsRequired();
            builder.Property(u=>u.Password).IsRequired();   
            builder.Property(u => u.Email).IsRequired();

        }
    }
}
