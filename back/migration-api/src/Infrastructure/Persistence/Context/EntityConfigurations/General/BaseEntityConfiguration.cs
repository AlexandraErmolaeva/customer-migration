using Domain.Entities.General;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Context.EntityConfigurations.General;

public abstract class GuidBaseEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
  where TEntity : EntityBase
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
               .HasDefaultValueSql("NEWSEQUENTIALID()")
               .ValueGeneratedOnAdd();

        builder.HasQueryFilter(e => !e.IsDeleted);
        builder.Property(e => e.IsDeleted)
            .HasDefaultValue(false);

        builder.Property(e => e.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
    }
}
