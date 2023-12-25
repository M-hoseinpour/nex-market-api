using market.Extensions;
using market.Models.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace market.Models.Domain;

public class Customer : Entity
{
    public int UserId { get; set; }
    public virtual required User User { get; set; }
    public virtual required ICollection<Address> Addresses { get; set; }
    public virtual ICollection<Review>? Reviews { get; set; }
    public virtual FinancialAccount FinancialAccount { get; set; }
    public virtual ICollection<BankTransaction> BankTransactions { get; set; }
}

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {

        builder.HasTimestamps();
        builder.HasSoftDelete();
        builder.HasUuid();

        builder.HasKey(x => x.Id);

        builder
         .HasOne(x => x.User)
         .WithOne(x => x.Customer)
         .HasForeignKey<Customer>(x => x.UserId);

        builder
         .HasMany(x => x.Addresses)
         .WithOne(x => x.Customer)
         .HasForeignKey(x => x.CustomerId);
    }
}