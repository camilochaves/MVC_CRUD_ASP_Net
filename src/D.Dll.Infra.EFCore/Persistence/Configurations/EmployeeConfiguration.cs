using System.Security.Cryptography.X509Certificates;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace Infra.EFCore.Configurations
{
  internal class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
  {
    public void Configure(EntityTypeBuilder<Employee> b)
    {
      b.HasKey(x=>x.Id);

      b.Property(x=>x.Id)
                       .ValueGeneratedOnAdd();

      b.Property(x=>x.Email)
          .IsRequired()
          .HasMaxLength(50);

      b.Property(x=>x.EmployeeNumber)
          .IsRequired();

      b.Property(x=>x.FirstName)
          .IsRequired()
          .HasMaxLength(10);

      b.Property(x=>x.LastName)
          .IsRequired()
          .HasMaxLength(150);

      b.Property(x=>x.LeaderId).IsRequired(false);

      b.Property(x=>x.Phone)
          .HasMaxLength(15);

      b.Property(x=>x.MobilePhone)
          .HasMaxLength(15);

      b.Property(x=>x.Password)
          .IsRequired()
          .HasMaxLength(100);

      b.Property(x=>x.Status).IsRequired(true);

      b.HasIndex(x=>x.LeaderId);

      b.HasOne(x => x.Leader)
        .WithOne()
        .HasForeignKey<Employee>(b=>b.LeaderId)
        .OnDelete(DeleteBehavior.Cascade);

      b.Navigation(o=>o.Leader).IsRequired(false);
    }

  }

}