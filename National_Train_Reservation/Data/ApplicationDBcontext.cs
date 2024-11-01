using Microsoft.EntityFrameworkCore;
using National_Train_Reservation.Models;
using System.Collections.Generic;
using static NuGet.Packaging.PackagingConstants;
namespace National_Train_Reservation.Data
{
    public class ApplicationDBcontext : DbContext
    {
        public ApplicationDBcontext(DbContextOptions<ApplicationDBcontext> options) : base(options)
        { }
        public DbSet<Users> Users { get; set; }
        public DbSet<Trains> Trains { get; set; }
        public DbSet<Tickets> Tickets { get; set; }
        public DbSet<Payment> Payment { get; set; }
        public DbSet<Suggestions_Complaints> Suggestions_Complaints { get; set; }
        public DbSet<AvailableTickets>AvailableTickets { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tickets>().HasOne(U => U.Users).WithMany(T => T.Tickets).HasForeignKey(U => U.User_ID);
            modelBuilder.Entity<Payment>().HasOne(U => U.Users).WithMany(P => P.Payment).HasForeignKey(U => U.User_ID);
            modelBuilder.Entity<Tickets>().HasOne(Tr => Tr.Trains).WithMany(Ti => Ti.Tickets).HasForeignKey(Tr => Tr.Journey_ID);
            modelBuilder.Entity<Suggestions_Complaints>().HasOne(U => U.Users).WithMany(s => s.Suggestions_Complaints).HasForeignKey(U => U.UserID);
            modelBuilder.Entity<AvailableTickets>().HasOne(Tr => Tr.Trains).WithMany(At => At.AvailableTickets).HasForeignKey(Tr => Tr.Journey_ID);
            
        }
    }
}