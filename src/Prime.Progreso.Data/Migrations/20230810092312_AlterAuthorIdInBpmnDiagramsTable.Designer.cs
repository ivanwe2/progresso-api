﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Prime.Progreso.Data;

#nullable disable

namespace Prime.Progreso.Data.Migrations
{
    [DbContext(typeof(ProgresoDbContext))]
    [Migration("20230810092312_AlterAuthorIdInBpmnDiagramsTable")]
    partial class AlterAuthorIdInBpmnDiagramsTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Prime.Progreso.Data.Entities.Activity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Subject")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Activities");
                });

            modelBuilder.Entity("Prime.Progreso.Data.Entities.Answer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsCorrect")
                        .HasColumnType("bit");

                    b.Property<Guid>("QuestionId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("QuestionId");

                    b.ToTable("Answers");
                });

            modelBuilder.Entity("Prime.Progreso.Data.Entities.AnswerChoice", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ChoiceId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("QuestionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("QuizExecutionId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ChoiceId");

                    b.HasIndex("QuestionId");

                    b.HasIndex("QuizExecutionId");

                    b.ToTable("AnswerChoices");
                });

            modelBuilder.Entity("Prime.Progreso.Data.Entities.BpmnDiagram", b =>
                {
                    b.Property<Guid>("FileId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AuthorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("FileName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FilePath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UploadDate")
                        .HasColumnType("datetime2");

                    b.HasKey("FileId");

                    b.ToTable("BpmnDiagrams");
                });

            modelBuilder.Entity("Prime.Progreso.Data.Entities.CategorizedQuestion", b =>
                {
                    b.Property<Guid>("QuestionCategoryId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("QuestionId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("QuestionCategoryId", "QuestionId");

                    b.HasIndex("QuestionId");

                    b.ToTable("CategorizedQuestions");
                });

            modelBuilder.Entity("Prime.Progreso.Data.Entities.CodingChallenge", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Codebase")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("CodingChallenges");
                });

            modelBuilder.Entity("Prime.Progreso.Data.Entities.Curriculum", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Duration")
                        .HasColumnType("int");

                    b.Property<Guid>("TechnologyId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("TechnologyId");

                    b.ToTable("Curriculums");
                });

            modelBuilder.Entity("Prime.Progreso.Data.Entities.CurriculumItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ActivityId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CurriculumId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("DayOfInternship")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ActivityId");

                    b.HasIndex("CurriculumId");

                    b.ToTable("CurriculumItems");
                });

            modelBuilder.Entity("Prime.Progreso.Data.Entities.Keyword", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("LanguageId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Word")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("LanguageId");

                    b.ToTable("Keywords");
                });

            modelBuilder.Entity("Prime.Progreso.Data.Entities.KeywordDescription", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("KeywordId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("KeywordId");

                    b.ToTable("KeywordDescriptions");
                });

            modelBuilder.Entity("Prime.Progreso.Data.Entities.Language", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Languages");
                });

            modelBuilder.Entity("Prime.Progreso.Data.Entities.Milestone", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Duration")
                        .HasColumnType("int");

                    b.Property<int>("Order")
                        .HasColumnType("int");

                    b.Property<Guid?>("ProjectId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("Milestones");
                });

            modelBuilder.Entity("Prime.Progreso.Data.Entities.Project", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("Prime.Progreso.Data.Entities.Question", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("Prime.Progreso.Data.Entities.QuestionCategory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("QuestionCategories");
                });

            modelBuilder.Entity("Prime.Progreso.Data.Entities.Quiz", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Duration")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Quizzes");
                });

            modelBuilder.Entity("Prime.Progreso.Data.Entities.QuizExecution", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("QuizId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("QuizId");

                    b.ToTable("QuizExecutions");
                });

            modelBuilder.Entity("Prime.Progreso.Data.Entities.QuizQuestionLink", b =>
                {
                    b.Property<Guid>("QuestionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("QuizId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("QuestionId", "QuizId");

                    b.HasIndex("QuizId");

                    b.ToTable("QuizQuestionLinks");
                });

            modelBuilder.Entity("Prime.Progreso.Data.Entities.Technology", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Technologies");
                });

            modelBuilder.Entity("Prime.Progreso.Data.Entities.Answer", b =>
                {
                    b.HasOne("Prime.Progreso.Data.Entities.Question", null)
                        .WithMany("Answers")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Prime.Progreso.Data.Entities.AnswerChoice", b =>
                {
                    b.HasOne("Prime.Progreso.Data.Entities.Answer", "Choice")
                        .WithMany()
                        .HasForeignKey("ChoiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Prime.Progreso.Data.Entities.Question", "Question")
                        .WithMany()
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Prime.Progreso.Data.Entities.QuizExecution", "QuizExecution")
                        .WithMany()
                        .HasForeignKey("QuizExecutionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Choice");

                    b.Navigation("Question");

                    b.Navigation("QuizExecution");
                });

            modelBuilder.Entity("Prime.Progreso.Data.Entities.CategorizedQuestion", b =>
                {
                    b.HasOne("Prime.Progreso.Data.Entities.QuestionCategory", "QuestionCategory")
                        .WithMany()
                        .HasForeignKey("QuestionCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Prime.Progreso.Data.Entities.Question", "Question")
                        .WithMany("CategorizedQuestions")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Question");

                    b.Navigation("QuestionCategory");
                });

            modelBuilder.Entity("Prime.Progreso.Data.Entities.Curriculum", b =>
                {
                    b.HasOne("Prime.Progreso.Data.Entities.Technology", "Technology")
                        .WithMany()
                        .HasForeignKey("TechnologyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Technology");
                });

            modelBuilder.Entity("Prime.Progreso.Data.Entities.CurriculumItem", b =>
                {
                    b.HasOne("Prime.Progreso.Data.Entities.Activity", "Activity")
                        .WithMany()
                        .HasForeignKey("ActivityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Prime.Progreso.Data.Entities.Curriculum", "Curriculum")
                        .WithMany("CurriculumItems")
                        .HasForeignKey("CurriculumId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Activity");

                    b.Navigation("Curriculum");
                });

            modelBuilder.Entity("Prime.Progreso.Data.Entities.Keyword", b =>
                {
                    b.HasOne("Prime.Progreso.Data.Entities.Language", "Language")
                        .WithMany("Keywords")
                        .HasForeignKey("LanguageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Language");
                });

            modelBuilder.Entity("Prime.Progreso.Data.Entities.KeywordDescription", b =>
                {
                    b.HasOne("Prime.Progreso.Data.Entities.Keyword", "Keyword")
                        .WithMany("KeywordDescriptions")
                        .HasForeignKey("KeywordId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Keyword");
                });

            modelBuilder.Entity("Prime.Progreso.Data.Entities.Milestone", b =>
                {
                    b.HasOne("Prime.Progreso.Data.Entities.Project", null)
                        .WithMany("Milestones")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Prime.Progreso.Data.Entities.QuizExecution", b =>
                {
                    b.HasOne("Prime.Progreso.Data.Entities.Quiz", "Quiz")
                        .WithMany()
                        .HasForeignKey("QuizId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Quiz");
                });

            modelBuilder.Entity("Prime.Progreso.Data.Entities.QuizQuestionLink", b =>
                {
                    b.HasOne("Prime.Progreso.Data.Entities.Question", "Question")
                        .WithMany()
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Prime.Progreso.Data.Entities.Quiz", "Quiz")
                        .WithMany("QuizQuestionLinks")
                        .HasForeignKey("QuizId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Question");

                    b.Navigation("Quiz");
                });

            modelBuilder.Entity("Prime.Progreso.Data.Entities.Curriculum", b =>
                {
                    b.Navigation("CurriculumItems");
                });

            modelBuilder.Entity("Prime.Progreso.Data.Entities.Keyword", b =>
                {
                    b.Navigation("KeywordDescriptions");
                });

            modelBuilder.Entity("Prime.Progreso.Data.Entities.Language", b =>
                {
                    b.Navigation("Keywords");
                });

            modelBuilder.Entity("Prime.Progreso.Data.Entities.Project", b =>
                {
                    b.Navigation("Milestones");
                });

            modelBuilder.Entity("Prime.Progreso.Data.Entities.Question", b =>
                {
                    b.Navigation("Answers");

                    b.Navigation("CategorizedQuestions");
                });

            modelBuilder.Entity("Prime.Progreso.Data.Entities.Quiz", b =>
                {
                    b.Navigation("QuizQuestionLinks");
                });
#pragma warning restore 612, 618
        }
    }
}
