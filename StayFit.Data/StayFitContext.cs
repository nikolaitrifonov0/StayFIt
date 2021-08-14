using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StayFit.Data.Models;

namespace StayFit.Data
{
    public class StayFitContext : IdentityDbContext
    {
        public StayFitContext()
        {
        }

        public StayFitContext(DbContextOptions<StayFitContext> options) : base(options)
        {            
        }

        public DbSet<BodyPart> BodyParts { get; init; }
        public DbSet<Equipment> Equipments { get; init; }
        public DbSet<Exercise> Exercises { get; init; }
        public DbSet<WorkDay> WorkDays { get; init; }
        public DbSet<Workout> Workouts { get; init; }
        public DbSet<UserExerciseLog> UserExerciseLogs { get; init; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer(@"Server=.\SQLEXPRESS;Database=StayFit;Integrated Security=True;");
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Workout>()
                .HasOne<IdentityUser>(w => w.Creator)
                .WithMany()
                .HasForeignKey(w => w.CreatorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Workout>()
                .HasMany<IdentityUser>()
                .WithOne();


            modelBuilder.Entity<UserExerciseLog>()
                .HasOne<IdentityUser>()
                .WithMany()
                .HasForeignKey(uel => uel.UserId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
