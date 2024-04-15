using Microsoft.EntityFrameworkCore;
using Prime.Progreso.Data.Entities;

namespace Prime.Progreso.Data
{
    public class ProgresoDbContext : DbContext
    {
        public ProgresoDbContext(DbContextOptions<ProgresoDbContext> options) : base(options)
        {
        }

        public DbSet<Milestone> Milestones { get; set; }

        public DbSet<Activity> Activities { get; set; }

        public DbSet<CurriculumItem> CurriculumItems { get; set; }

        public DbSet<Curriculum> Curriculums{ get; set; }

        public DbSet<Project> Projects { get; set; }

        public DbSet<QuestionCategory> QuestionCategories { get; set; }

        public DbSet<Question> Questions { get; set; }

        public DbSet<Answer> Answers { get; set; }

        public DbSet<CategorizedQuestion> CategorizedQuestions { get; set; }
        
        public DbSet<CodingChallenge> CodingChallenges { get; set; }

        public DbSet<KeywordDescription> KeywordDescriptions { get; set; }

        public DbSet<Keyword> Keywords { get; set; }

        public DbSet<Language> Languages { get; set; }

        public DbSet<Quiz> Quizzes { get; set; }

        public DbSet<QuizQuestionLink> QuizQuestionLinks { get; set; }
        
        public DbSet<Technology> Technologies { get; set; }

        public DbSet<QuizExecution> QuizExecutions { get; set; }

        public DbSet<QuizAssignment> QuizAssignments { get; set; }

        public DbSet<AnswerChoice> AnswerChoices { get; set; }

        public DbSet<BpmnDiagram> BpmnDiagrams { get; set; }

        public DbSet<AssignmentToCodingChallenge> Assignments { get; set; }

        public DbSet<TestCase> TestCases { get; set; }

        public DbSet<KeywordSinglePlayerResult> KeywordSinglePlayerResults { get; set; }

        public DbSet<KeywordDescriptionSinglePlayerResult> KeywordDescriptionSinglePlayerResults { get; set; }

        public DbSet<KeywordDescriptionMultiPlayerResult> KeywordDescriptionMultiPlayerResults { get; set; }

        public DbSet<KeywordMultiPlayerResult> KeywordMultiPlayerResults { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Activity>()
                .Property(p => p.Type)
                .HasDefaultValue(null);

            modelBuilder.Entity<Project>()
                .HasMany(p => p.Milestones)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CurriculumItem>()
                .HasOne(x => x.Activity)
                .WithMany();

            modelBuilder.Entity<Question>(eb =>
            {
                eb.HasMany(q => q.Answers)
                .WithOne()
                .HasForeignKey(a => a.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);

                eb.HasMany(e => e.QuestionCategories)
                .WithMany()
                .UsingEntity<CategorizedQuestion>(l => l.HasOne(e => e.QuestionCategory).WithMany(),
                r => r.HasOne(e => e.Question).WithMany(e => e.CategorizedQuestions));
            });
            
            modelBuilder.Entity<CurriculumItem>()
                .HasOne(x => x.Curriculum)
                .WithMany(x => x.CurriculumItems)
                .HasForeignKey(x => x.CurriculumId);

            modelBuilder.Entity<Quiz>()
                .HasMany(p => p.Questions)
                .WithMany()
                .UsingEntity<QuizQuestionLink>(l => l.HasOne(e => e.Question).WithMany(),
                                               r => r.HasOne(e => e.Quiz).WithMany(e => e.QuizQuestionLinks));
            
            modelBuilder.Entity<Curriculum>()
                .HasOne(x => x.Technology)
                .WithMany()
                .HasForeignKey(x => x.TechnologyId);

            modelBuilder.Entity<QuizAssignment>()
                .HasOne(x => x.Quiz)
                .WithMany()
                .HasForeignKey(x => x.QuizId);

            modelBuilder.Entity<QuizExecution>()
                .HasOne(x => x.Quiz)
                .WithMany()
                .HasForeignKey(x => x.QuizId);

            modelBuilder.Entity<AnswerChoice>(ac =>
            {
                ac.HasOne(x => x.QuizExecution)
                .WithMany()
                .HasForeignKey(x => x.QuizExecutionId);

                ac.HasOne(x => x.Question)
                .WithMany()
                .HasForeignKey(x => x.QuestionId);

                ac.HasOne(x => x.Choice)
                .WithMany()
                .HasForeignKey(x => x.ChoiceId);
            });

            modelBuilder.Entity<BpmnDiagram>()
                .HasKey(e => e.FileId);

            modelBuilder.Entity<AssignmentToCodingChallenge>(a =>
            {
                a.HasOne(x => x.CodingChallenge)
                .WithMany()
                .HasForeignKey(x => x.CodingChallengeId);

                a.HasIndex(ia => new { ia.InternId, ia.CodingChallengeId })
                .IsUnique();
            });
                
            modelBuilder.Entity<TestCase>()
                .HasOne(x => x.CodingChallenge)
                .WithMany()
                .HasForeignKey(x => x.CodingChallengeId);

            modelBuilder.Entity<CodingChallenge>()
                .HasOne(x => x.Technology)
                .WithMany()
                .HasForeignKey(x => x.TechnologyId);

            modelBuilder.Entity<KeywordSinglePlayerResult>()
                .HasOne(x => x.Keyword)
                .WithMany() 
                .HasForeignKey(x => x.KeywordId);

            modelBuilder.Entity<KeywordDescriptionSinglePlayerResult>()
                .HasOne(x => x.KeywordDescription)
                .WithMany()
                .HasForeignKey(x => x.KeywordDescriptionId);

            modelBuilder.Entity<KeywordDescriptionMultiPlayerResult>()
               .HasOne(x => x.KeywordDescription)
               .WithMany()
               .HasForeignKey(x => x.KeywordDescriptionId);

            modelBuilder.Entity<KeywordMultiPlayerResult>()
                .HasOne(x => x.Keyword)
                .WithMany()
                .HasForeignKey(x => x.KeywordId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
