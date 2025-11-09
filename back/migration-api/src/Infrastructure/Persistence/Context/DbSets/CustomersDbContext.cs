using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Context;

public partial class CustomersDbContext
{
    public virtual DbSet<CustomerEntity> Customers { get; set; } = null!;
    public virtual DbSet<ContactsEntity> Contacts { get; set; } = null!;
    public virtual DbSet<FinancialProfileEntity> FinancialProfiles { get; set; } = null!;
}
