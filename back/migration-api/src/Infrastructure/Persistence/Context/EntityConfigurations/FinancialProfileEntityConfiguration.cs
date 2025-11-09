using Domain.Entities;
using Infrastructure.Persistence.Context.EntityConfigurations.General;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Context.EntityConfigurations;

public class FinancialProfileEntityConfiguration : GuidBaseEntityConfiguration<FinancialProfileEntity>
{
    public override void Configure(EntityTypeBuilder<FinancialProfileEntity> builder)
    {
        base.Configure(builder);

        builder.HasOne(profile => profile.Customer)
            .WithOne(customer => customer.FinancialProfile)
            .OnDelete(DeleteBehavior.NoAction);
    }
}