using CesiZen.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class CesiZenDbContext : IdentityDbContext<UserModel, IdentityRole<int>, int>
{
    public virtual DbSet<InformationModel> InformationModels { get; set; }
    public virtual DbSet<StressEventModel> StressEvents { get; set; }
    public virtual DbSet<StressTestResultModel> StressTestResults { get; set; }
    public virtual DbSet<StressQuestionModel> StressQuestions { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<StressQuestionModel>()
            .HasOne(q => q.StressEvent)
            .WithMany(e => e.StressQuestions)
            .HasForeignKey(q => q.StressEventModelId)
            .OnDelete(DeleteBehavior.Cascade);


        base.OnModelCreating(builder);

        // Roles
        builder.Entity<IdentityRole<int>>().HasData(
            new IdentityRole<int> { Id = 1, Name = "Admin", NormalizedName = "ADMIN" },
            new IdentityRole<int> { Id = 2, Name = "User", NormalizedName = "USER" }
        );

        // Event
        builder.Entity<StressEventModel>().HasData(
            new StressEventModel { Id = 1, Title = "Questionnaire Holmes et Rahe" }
        );

        // Questions
        builder.Entity<StressQuestionModel>().HasData(
            new StressQuestionModel { Id = 1, Title = "Décès du conjoint", Point = 100, StressEventModelId = 1 },
            new StressQuestionModel { Id = 2, Title = "Divorce", Point = 73, StressEventModelId = 1 },
            new StressQuestionModel { Id = 3, Title = "Séparation conjugale", Point = 65, StressEventModelId = 1 },
            new StressQuestionModel { Id = 4, Title = "Peine d’emprisonnement", Point = 63, StressEventModelId = 1 },
            new StressQuestionModel { Id = 5, Title = "Décès d’un proche parent", Point = 63, StressEventModelId = 1 },
            new StressQuestionModel { Id = 6, Title = "Blessure ou maladie personnelle grave", Point = 53, StressEventModelId = 1 },
            new StressQuestionModel { Id = 7, Title = "Mariage", Point = 50, StressEventModelId = 1 },
            new StressQuestionModel { Id = 8, Title = "Perte d’emploi", Point = 47, StressEventModelId = 1 },
            new StressQuestionModel { Id = 9, Title = "Réconciliation conjugale", Point = 45, StressEventModelId = 1 },
            new StressQuestionModel { Id = 10, Title = "Retraite", Point = 45, StressEventModelId = 1 },
            new StressQuestionModel { Id = 11, Title = "Changement de santé d’un membre de la famille", Point = 44, StressEventModelId = 1 },
            new StressQuestionModel { Id = 12, Title = "Grossesse", Point = 40, StressEventModelId = 1 },
            new StressQuestionModel { Id = 13, Title = "Difficultés sexuelles", Point = 39, StressEventModelId = 1 },
            new StressQuestionModel { Id = 14, Title = "Arrivée d’un nouveau membre dans la famille", Point = 39, StressEventModelId = 1 },
            new StressQuestionModel { Id = 15, Title = "Réorganisation professionnelle", Point = 39, StressEventModelId = 1 },
            new StressQuestionModel { Id = 16, Title = "Changement dans la situation financière", Point = 38, StressEventModelId = 1 },
            new StressQuestionModel { Id = 17, Title = "Décès d’un ami proche", Point = 37, StressEventModelId = 1 },
            new StressQuestionModel { Id = 18, Title = "Changement de métier", Point = 36, StressEventModelId = 1 },
            new StressQuestionModel { Id = 19, Title = "Dispute conjugale", Point = 35, StressEventModelId = 1 },
            new StressQuestionModel { Id = 20, Title = "Crédit important", Point = 31, StressEventModelId = 1 },
            new StressQuestionModel { Id = 21, Title = "Hypothèque ou emprunt", Point = 30, StressEventModelId = 1 },
            new StressQuestionModel { Id = 22, Title = "Changement de responsabilités au travail", Point = 29, StressEventModelId = 1 },
            new StressQuestionModel { Id = 23, Title = "Départ d’un enfant du foyer", Point = 29, StressEventModelId = 1 },
            new StressQuestionModel { Id = 24, Title = "Problèmes avec la belle-famille", Point = 29, StressEventModelId = 1 },
            new StressQuestionModel { Id = 25, Title = "Réussite personnelle remarquable", Point = 28, StressEventModelId = 1 },
            new StressQuestionModel { Id = 26, Title = "Épouse commence ou arrête de travailler", Point = 26, StressEventModelId = 1 },
            new StressQuestionModel { Id = 27, Title = "Début ou fin d’études", Point = 26, StressEventModelId = 1 },
            new StressQuestionModel { Id = 28, Title = "Changement de conditions de vie", Point = 25, StressEventModelId = 1 },
            new StressQuestionModel { Id = 29, Title = "Révision d’habitudes personnelles", Point = 24, StressEventModelId = 1 },
            new StressQuestionModel { Id = 30, Title = "Problèmes avec un supérieur", Point = 23, StressEventModelId = 1 },
            new StressQuestionModel { Id = 31, Title = "Changement d’horaires ou de conditions de travail", Point = 20, StressEventModelId = 1 },
            new StressQuestionModel { Id = 32, Title = "Changement de résidence", Point = 20, StressEventModelId = 1 },
            new StressQuestionModel { Id = 33, Title = "Changement d’école", Point = 20, StressEventModelId = 1 },
            new StressQuestionModel { Id = 34, Title = "Changement dans les loisirs", Point = 19, StressEventModelId = 1 },
            new StressQuestionModel { Id = 35, Title = "Changement d’activités religieuses", Point = 19, StressEventModelId = 1 },
            new StressQuestionModel { Id = 36, Title = "Changement d’activités sociales", Point = 18, StressEventModelId = 1 },
            new StressQuestionModel { Id = 37, Title = "Petit emprunt ou dette", Point = 17, StressEventModelId = 1 },
            new StressQuestionModel { Id = 38, Title = "Changement dans les habitudes de sommeil", Point = 16, StressEventModelId = 1 },
            new StressQuestionModel { Id = 39, Title = "Changement dans la fréquence des réunions de famille", Point = 15, StressEventModelId = 1 },
            new StressQuestionModel { Id = 40, Title = "Vacances", Point = 13, StressEventModelId = 1 }
        );
    }

    public CesiZenDbContext(DbContextOptions<CesiZenDbContext> options)
        : base(options)
    {
    }
}
