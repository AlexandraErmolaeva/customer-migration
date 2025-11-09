using Domain.Entities;
using Infrastructure.Persistence.Context.EntityConfigurations.General;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Context.EntityConfigurations;

public class CustomerEntityConfiguration : GuidBaseEntityConfiguration<CustomerEntity>
{
    public override void Configure(EntityTypeBuilder<CustomerEntity> builder)
    {
        base.Configure(builder);

        builder.Property(customer => customer.Gender)
            .HasConversion(t => t.ToString(),
            s => (Gender)Enum.Parse(typeof(Gender), s));

        builder.HasOne(customer => customer.Contacts)
            .WithOne(contacts => contacts.Customer)
            .HasForeignKey<CustomerEntity>(customer => customer.ContactsId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(customer => customer.FinancialProfile)
            .WithOne(financial => financial.Customer)
            .HasForeignKey<CustomerEntity>(customer => customer.FinancialProfileId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
