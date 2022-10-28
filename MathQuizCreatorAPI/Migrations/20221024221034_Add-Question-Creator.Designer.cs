﻿// <auto-generated />
using System;
using MathQuizCreatorAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MathQuizCreatorAPI.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20221024221034_Add-Question-Creator")]
    partial class AddQuestionCreator
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("MathQuizCreatorAPI.Models.NormalDistribution", b =>
                {
                    b.Property<Guid>("NormalDistributionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("Area")
                        .HasColumnType("float");

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DistributionType")
                        .HasColumnType("int");

                    b.Property<double>("Max")
                        .HasColumnType("float");

                    b.Property<double>("Mean")
                        .HasColumnType("float");

                    b.Property<double>("Min")
                        .HasColumnType("float");

                    b.Property<Guid?>("QuestionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("StandardDeviation")
                        .HasColumnType("float");

                    b.Property<bool>("Values")
                        .HasColumnType("bit");

                    b.HasKey("NormalDistributionId");

                    b.HasIndex("QuestionId");

                    b.ToTable("NormalDistributions");
                });

            modelBuilder.Entity("MathQuizCreatorAPI.Models.Parameter", b =>
                {
                    b.Property<Guid>("ParameterId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Order")
                        .HasColumnType("int");

                    b.Property<Guid?>("QuestionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ParameterId");

                    b.HasIndex("QuestionId");

                    b.ToTable("Parameters");
                });

            modelBuilder.Entity("MathQuizCreatorAPI.Models.Question", b =>
                {
                    b.Property<Guid>("QuestionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Answer")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("CreationTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid?>("CreatorUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("LastModifiedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("TopicId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("QuestionId");

                    b.HasIndex("CreatorUserId");

                    b.HasIndex("TopicId");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("MathQuizCreatorAPI.Models.Quiz", b =>
                {
                    b.Property<Guid>("QuizId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreationTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid?>("CreatorUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("HasUnlimitedMode")
                        .HasColumnType("bit");

                    b.Property<bool>("IsPublic")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset>("LastModifiedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("TopicId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("QuizId");

                    b.HasIndex("CreatorUserId");

                    b.HasIndex("TopicId");

                    b.ToTable("Quizzes");
                });

            modelBuilder.Entity("MathQuizCreatorAPI.Models.QuizQuestion", b =>
                {
                    b.Property<Guid>("QuizQuestionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Order")
                        .HasColumnType("int");

                    b.Property<Guid?>("QuestionId")
                        .IsRequired()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("QuizId")
                        .IsRequired()
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("QuizQuestionId");

                    b.HasIndex("QuestionId");

                    b.HasIndex("QuizId");

                    b.ToTable("QuizQuestions");
                });

            modelBuilder.Entity("MathQuizCreatorAPI.Models.Role", b =>
                {
                    b.Property<Guid>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreationTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset>("LastModifiedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("RoleId");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("MathQuizCreatorAPI.Models.SolvedQuiz", b =>
                {
                    b.Property<Guid>("SolvedQuizId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("CorrectResponses")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("CreationTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<int>("IncorrectResponses")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("LastModifiedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid?>("QuizId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("Score")
                        .HasColumnType("float");

                    b.Property<int>("TotalQuestions")
                        .HasColumnType("int");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("SolvedQuizId");

                    b.HasIndex("QuizId");

                    b.HasIndex("UserId");

                    b.ToTable("SolvedQuizzes");
                });

            modelBuilder.Entity("MathQuizCreatorAPI.Models.Topic", b =>
                {
                    b.Property<Guid>("TopicId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreationTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset>("LastModifiedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TopicId");

                    b.ToTable("Topics");
                });

            modelBuilder.Entity("MathQuizCreatorAPI.Models.User", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreationTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("LastModifiedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.HasIndex("RoleId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("MathQuizCreatorAPI.Models.NormalDistribution", b =>
                {
                    b.HasOne("MathQuizCreatorAPI.Models.Question", "Question")
                        .WithMany()
                        .HasForeignKey("QuestionId");

                    b.Navigation("Question");
                });

            modelBuilder.Entity("MathQuizCreatorAPI.Models.Parameter", b =>
                {
                    b.HasOne("MathQuizCreatorAPI.Models.Question", "Question")
                        .WithMany()
                        .HasForeignKey("QuestionId");

                    b.Navigation("Question");
                });

            modelBuilder.Entity("MathQuizCreatorAPI.Models.Question", b =>
                {
                    b.HasOne("MathQuizCreatorAPI.Models.User", "Creator")
                        .WithMany()
                        .HasForeignKey("CreatorUserId");

                    b.HasOne("MathQuizCreatorAPI.Models.Topic", "Topic")
                        .WithMany("Questions")
                        .HasForeignKey("TopicId");

                    b.Navigation("Creator");

                    b.Navigation("Topic");
                });

            modelBuilder.Entity("MathQuizCreatorAPI.Models.Quiz", b =>
                {
                    b.HasOne("MathQuizCreatorAPI.Models.User", "Creator")
                        .WithMany()
                        .HasForeignKey("CreatorUserId");

                    b.HasOne("MathQuizCreatorAPI.Models.Topic", "Topic")
                        .WithMany("Quizzes")
                        .HasForeignKey("TopicId");

                    b.Navigation("Creator");

                    b.Navigation("Topic");
                });

            modelBuilder.Entity("MathQuizCreatorAPI.Models.QuizQuestion", b =>
                {
                    b.HasOne("MathQuizCreatorAPI.Models.Question", "Question")
                        .WithMany("QuizQuestions")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MathQuizCreatorAPI.Models.Quiz", "Quiz")
                        .WithMany("QuizQuestions")
                        .HasForeignKey("QuizId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Question");

                    b.Navigation("Quiz");
                });

            modelBuilder.Entity("MathQuizCreatorAPI.Models.SolvedQuiz", b =>
                {
                    b.HasOne("MathQuizCreatorAPI.Models.Quiz", "Quiz")
                        .WithMany()
                        .HasForeignKey("QuizId");

                    b.HasOne("MathQuizCreatorAPI.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("Quiz");

                    b.Navigation("User");
                });

            modelBuilder.Entity("MathQuizCreatorAPI.Models.User", b =>
                {
                    b.HasOne("MathQuizCreatorAPI.Models.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("MathQuizCreatorAPI.Models.Question", b =>
                {
                    b.Navigation("QuizQuestions");
                });

            modelBuilder.Entity("MathQuizCreatorAPI.Models.Quiz", b =>
                {
                    b.Navigation("QuizQuestions");
                });

            modelBuilder.Entity("MathQuizCreatorAPI.Models.Topic", b =>
                {
                    b.Navigation("Questions");

                    b.Navigation("Quizzes");
                });
#pragma warning restore 612, 618
        }
    }
}