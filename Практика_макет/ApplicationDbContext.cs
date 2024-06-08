using Microsoft.EntityFrameworkCore;

namespace Практика_макет
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Type> Types { get; set; }
        public DbSet<UserCredential> UserCredentials { get; set; }
        public DbSet<UserType> UserTypes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<RequestStatus> RequestStatus { get; set; }
        public DbSet<HomeTech> HomeTech { get; set; }
        public DbSet<RequestDetail> RequestsDetails { get; set; }
        public DbSet<RequestsDetailsStatus> RequestsDetailsStatus { get; set; }
        public DbSet<RequestDetailTech> RequestsDetailsTech { get; set; }
        public DbSet<Request> Requests { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=ООО_ТехСервис;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasKey(u => u.UserID);

            modelBuilder.Entity<UserCredential>()
                .HasKey(uc => uc.UserID);
            modelBuilder.Entity<UserCredential>()
                .HasOne(uc => uc.User)
                .WithMany(u => u.UserCredentials)
                .HasForeignKey(uc => uc.UserID);

            modelBuilder.Entity<Type>()
                .HasKey(t => t.TypeID);

            modelBuilder.Entity<UserType>()
                .HasKey(ut => new { ut.UserID, ut.TypeID });
            modelBuilder.Entity<UserType>()
                .HasOne(ut => ut.User)
                .WithMany(u => u.UserTypes)
                .HasForeignKey(ut => ut.UserID);
            modelBuilder.Entity<UserType>()
                .HasOne(ut => ut.Type)
                .WithMany(t => t.UserTypes)
                .HasForeignKey(ut => ut.TypeID);

            modelBuilder.Entity<Comment>()
                .HasKey(c => c.CommentID);
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserID);

            modelBuilder.Entity<RequestStatus>()
                .HasKey(rs => rs.RequestStatusID);

            modelBuilder.Entity<HomeTech>()
                .HasKey(ht => ht.TechID);

            modelBuilder.Entity<RequestDetail>()
                .HasKey(rd => rd.RequestID);
            modelBuilder.Entity<RequestDetail>()
                .HasOne(rd => rd.Requests)
                .WithOne(r => r.RequestDetail)
                .HasForeignKey<Request>(r => r.RequestID);

            modelBuilder.Entity<RequestsDetailsStatus>()
                .HasKey(rds => new { rds.RequestID, rds.RequestStatusID });
            modelBuilder.Entity<RequestsDetailsStatus>()
                .HasOne(rds => rds.RequestDetail)
                .WithMany(rd => rd.RequestsDetailsStatus)
                .HasForeignKey(rds => rds.RequestID);
            modelBuilder.Entity<RequestsDetailsStatus>()
                .HasOne(rds => rds.RequestStatus)
                .WithMany(rs => rs.RequestsDetailsStatus)
                .HasForeignKey(rds => rds.RequestStatusID);

            modelBuilder.Entity<RequestDetailTech>()
                .HasKey(rdt => new { rdt.RequestID, rdt.TechID });
            modelBuilder.Entity<RequestDetailTech>()
                .HasOne(rdt => rdt.RequestDetail)
                .WithMany(rd => rd.RequestsDetailsTech)
                .HasForeignKey(rdt => rdt.RequestID);
            modelBuilder.Entity<RequestDetailTech>()
                .HasOne(rdt => rdt.HomeTech)
                .WithMany(ht => ht.RequestsDetailsTech)
                .HasForeignKey(rdt => rdt.TechID);

            modelBuilder.Entity<Request>()
                .HasKey(r => r.RequestID);
            modelBuilder.Entity<Request>()
                .HasOne(r => r.Client)
                .WithMany(u => u.ClientRequests)
                .HasForeignKey(r => r.ClientID)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Request>()
                .HasOne(r => r.Master)
                .WithMany(u => u.MasterRequests)
                .HasForeignKey(r => r.MasterID)
                .OnDelete(DeleteBehavior.Restrict);
        }

    }
}
