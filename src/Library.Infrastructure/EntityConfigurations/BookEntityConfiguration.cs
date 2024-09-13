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
    public class BookEntityConfiguration: IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.HasKey(b => b.Id);
            builder.HasAlternateKey(b => b.ISBN);

            builder.Property(b=>b.ISBN).IsRequired();
            builder.Property(b=>b.Name).IsRequired().HasMaxLength(100);
            builder.Property(b=>b.Genre).IsRequired().HasMaxLength(50);
            builder.Property(b=>b.Description).IsRequired().HasMaxLength(150);


        }
    }
}
