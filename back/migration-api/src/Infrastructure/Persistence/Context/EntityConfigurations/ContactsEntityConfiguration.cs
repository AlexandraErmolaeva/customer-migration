using Domain.Entities;
using Infrastructure.Persistence.Context.EntityConfigurations.General;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Context.EntityConfigurations;

public class ContactsEntityConfiguration : GuidBaseEntityConfiguration<ContactsEntity>
{
    public override void Configure(EntityTypeBuilder<ContactsEntity> builder)
    {
        base.Configure(builder);

        builder.HasOne(contscts => contscts.Customer)
            .WithOne(customer => customer.Contacts)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
