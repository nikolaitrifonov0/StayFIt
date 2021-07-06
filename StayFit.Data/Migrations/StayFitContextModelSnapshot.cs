﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StayFit.Data;

namespace StayFit.Data.Migrations
{
    [DbContext(typeof(StayFitContext))]
    partial class StayFitContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.7")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BodyPartExercise", b =>
                {
                    b.Property<int>("BodyPartsId")
                        .HasColumnType("int");

                    b.Property<string>("ExercisesId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("BodyPartsId", "ExercisesId");

                    b.HasIndex("ExercisesId");

                    b.ToTable("BodyPartExercise");
                });

            modelBuilder.Entity("ExerciseWorkDay", b =>
                {
                    b.Property<string>("ExercisesId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("WorkDaysId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("ExercisesId", "WorkDaysId");

                    b.HasIndex("WorkDaysId");

                    b.ToTable("ExerciseWorkDay");
                });

            modelBuilder.Entity("StayFit.Data.Models.BodyPart", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("BodyParts");
                });

            modelBuilder.Entity("StayFit.Data.Models.Exercise", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Equipment")
                        .HasColumnType("int");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("VideoUrl")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Exercises");
                });

            modelBuilder.Entity("StayFit.Data.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CurrentWorkoutId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("Id");

                    b.HasIndex("CurrentWorkoutId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("StayFit.Data.Models.UserExerciseLog", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("WorkDayId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ExerciseId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "WorkDayId", "ExerciseId");

                    b.HasIndex("ExerciseId");

                    b.HasIndex("WorkDayId");

                    b.ToTable("UserExerciseLogs");
                });

            modelBuilder.Entity("StayFit.Data.Models.WorkDay", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("NextWorkout")
                        .HasColumnType("datetime2");

                    b.Property<string>("WorkoutId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("WorkoutId");

                    b.ToTable("WorkDays");
                });

            modelBuilder.Entity("StayFit.Data.Models.Workout", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CreatorId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int?>("CycleDays")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("WorkoutCycleType")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.ToTable("Workouts");
                });

            modelBuilder.Entity("BodyPartExercise", b =>
                {
                    b.HasOne("StayFit.Data.Models.BodyPart", null)
                        .WithMany()
                        .HasForeignKey("BodyPartsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StayFit.Data.Models.Exercise", null)
                        .WithMany()
                        .HasForeignKey("ExercisesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ExerciseWorkDay", b =>
                {
                    b.HasOne("StayFit.Data.Models.Exercise", null)
                        .WithMany()
                        .HasForeignKey("ExercisesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StayFit.Data.Models.WorkDay", null)
                        .WithMany()
                        .HasForeignKey("WorkDaysId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("StayFit.Data.Models.User", b =>
                {
                    b.HasOne("StayFit.Data.Models.Workout", "CurrentWorkout")
                        .WithMany("Users")
                        .HasForeignKey("CurrentWorkoutId");

                    b.Navigation("CurrentWorkout");
                });

            modelBuilder.Entity("StayFit.Data.Models.UserExerciseLog", b =>
                {
                    b.HasOne("StayFit.Data.Models.Exercise", "Exercise")
                        .WithMany("UserExerciseLogs")
                        .HasForeignKey("ExerciseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StayFit.Data.Models.User", "User")
                        .WithMany("UserExerciseLogs")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StayFit.Data.Models.WorkDay", "WorkDay")
                        .WithMany("UserExerciseLogs")
                        .HasForeignKey("WorkDayId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Exercise");

                    b.Navigation("User");

                    b.Navigation("WorkDay");
                });

            modelBuilder.Entity("StayFit.Data.Models.WorkDay", b =>
                {
                    b.HasOne("StayFit.Data.Models.Workout", "Workout")
                        .WithMany("WorkDays")
                        .HasForeignKey("WorkoutId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Workout");
                });

            modelBuilder.Entity("StayFit.Data.Models.Workout", b =>
                {
                    b.HasOne("StayFit.Data.Models.User", "Creator")
                        .WithMany("UploadedWorkouts")
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Creator");
                });

            modelBuilder.Entity("StayFit.Data.Models.Exercise", b =>
                {
                    b.Navigation("UserExerciseLogs");
                });

            modelBuilder.Entity("StayFit.Data.Models.User", b =>
                {
                    b.Navigation("UploadedWorkouts");

                    b.Navigation("UserExerciseLogs");
                });

            modelBuilder.Entity("StayFit.Data.Models.WorkDay", b =>
                {
                    b.Navigation("UserExerciseLogs");
                });

            modelBuilder.Entity("StayFit.Data.Models.Workout", b =>
                {
                    b.Navigation("Users");

                    b.Navigation("WorkDays");
                });
#pragma warning restore 612, 618
        }
    }
}
