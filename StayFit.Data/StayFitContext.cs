using Microsoft.EntityFrameworkCore;
using StayFit.Data.Models;

namespace StayFit.Data
{
    public class StayFitContext : DbContext
    {
        public StayFitContext()
        {
        }

        public StayFitContext(DbContextOptions<StayFitContext> options)
        {            
        }

        public DbSet<BodyPart> BodyParts { get; init; }
        public DbSet<Exercise> Exercises { get; init; }
        public DbSet<User> Users { get; init; }
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
            modelBuilder.Entity<User>()
                .HasMany(u => u.UploadedWorkouts)
                .WithOne(w => w.Creator)
                .HasForeignKey(w => w.CreatorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasOne(u => u.CurrentWorkout)
                .WithMany(w => w.Users)
                .HasForeignKey(u => u.CurrentWorkoutId);

           
            modelBuilder.Entity<UserExerciseLog>()
                .HasKey(uel => new { uel.UserId, uel.WorkDayId, uel.ExerciseId });

            base.OnModelCreating(modelBuilder);
        }
    }
}
