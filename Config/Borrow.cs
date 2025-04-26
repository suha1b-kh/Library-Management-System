using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Library_Management_System.Models
{
    public class BorrowConfig : IEntityTypeConfiguration<Borrow>
    {
        public void Configure(EntityTypeBuilder<Borrow> builder) {
            builder.HasKey(x => x.BorrowId);

            builder.HasOne(b => b.User)
           .WithMany(u => u.Borrows)
           .HasForeignKey(b => b.UserId)
           .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(b => b.Book)
                .WithMany()
                .HasForeignKey(b => b.BookId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
