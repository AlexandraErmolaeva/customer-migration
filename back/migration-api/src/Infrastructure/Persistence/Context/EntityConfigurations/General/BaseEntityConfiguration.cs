using Domain.Entities.General;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Context.EntityConfigurations.General;

/// <summary>
/// Конфигурация базовой сущности. Устанавливаем последовательный гуид.
/// Устанавливает фильтр, чтобы не забирать удаленные записи в IQueryable.
/// Дефолтно устанавливает для CreatedAt время UTC.
/// </summary>
/// <typeparam name="TEntity"></typeparam>
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
            .HasDefaultValueSql("SYSUTCDATETIME()");
    }
}
