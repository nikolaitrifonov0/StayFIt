using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StayFit.Data.Models;

namespace StayFit.Data
{
    public class StayFitContext : IdentityDbContext<ApplicationUser>
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
        public DbSet<ChatLog> ChatLogs { get; init; }

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
                .HasOne<ApplicationUser>(w => w.Creator)
                .WithMany()
                .HasForeignKey(w => w.CreatorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Workout>()
                .HasMany<ApplicationUser>()
                .WithOne()
                .OnDelete(DeleteBehavior.Restrict); 


            modelBuilder.Entity<UserExerciseLog>()
                .HasOne<ApplicationUser>(uel => uel.User)
                .WithMany()
                .HasForeignKey(uel => uel.UserId);

            modelBuilder.Entity<ApplicationUser>()
                .HasOne<WorkDay>(wd => wd.NextWorkDay)
                .WithMany()
                .HasForeignKey(wd => wd.NextWorkDayId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}
