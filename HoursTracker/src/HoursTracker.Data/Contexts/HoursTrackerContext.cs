using HoursTracker.Domain.Aggregates.Campuses;
using HoursTracker.Domain.Aggregates.Careers;
using HoursTracker.Domain.Aggregates.Classes;
using HoursTracker.Domain.Aggregates.Bot;
using HoursTracker.Domain.Aggregates.ExternalOrganizations;
using HoursTracker.Domain.Aggregates.Faculties;
using HoursTracker.Domain.Aggregates.Periods;
using HoursTracker.Domain.Aggregates.Professors;
using HoursTracker.Domain.Aggregates.Projects;
using HoursTracker.Domain.Aggregates.Sections;
using HoursTracker.Domain.Aggregates.Students;
using HoursTracker.Domain.Aggregates.VinculationTypes;
using HoursTracker.Domain.Shared;
using Microsoft.EntityFrameworkCore;
using HoursTracker.Domain.Aggregates.ProjectHours;

namespace HoursTracker.Data.Contexts
{
    public class HoursTrackerContext : DbContext
    {
        public HoursTrackerContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Professor> Professors { get; set; }

        public DbSet<Campus> Campuses { get; set; }
        
        public DbSet<Career> Careers { get; set;  } 
        
        public DbSet<Class> Classes { get; set; }

        public DbSet<Student> Students { get; set; }

        public DbSet<Faculty> Faculties { get; set; }

        public DbSet<Period> Periods { get; set; }

        public DbSet<Section> Sections { get; set; }

        public DbSet<Project> Projects { get; set; }

        public DbSet<VinculationType> VinculationTypes { get; set; }

        public DbSet<ExternalOrganization> ExternalOrganizations { get; set; }

        public DbSet<DataBot> DataBot { get; set; }

        public DbSet<ProjectHour> Projecthours { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Campus>().HasIndex(c=> c.Code).IsUnique();
            modelBuilder.Entity<Career>().HasIndex(c=> c.Code).IsUnique();
            modelBuilder.Entity<Class>().HasIndex(c => c.ClassCode).IsUnique();
            modelBuilder.Entity<ClassCareer>()
                .HasKey(classCareer => new { classCareer.ClassId, classCareer.CareerId });

            modelBuilder.Entity<ClassCareer>()
                .HasOne(classCareer => classCareer.Class)
                .WithMany(@class => @class.ClassCareers)
                .HasForeignKey(classCareer => classCareer.ClassId);

            modelBuilder.Entity<ClassCareer>()
                .HasOne(classCareer => classCareer.Career)
                .WithMany(career => career.ClassCareers)
                .HasForeignKey(classCareer => classCareer.CareerId);

            modelBuilder.Entity<StudentCareer>()
                .HasKey(studentCareer => new { studentCareer.StudentId, studentCareer.CareerId });

            modelBuilder.Entity<StudentCareer>()
                .HasOne(studentCareer => studentCareer.Student)
                .WithMany(@student => @student.StudentCareers)
                .HasForeignKey(studentCareer => studentCareer.StudentId);

            modelBuilder.Entity<StudentCareer>()
               .HasOne(studentCareer => studentCareer.Career)
               .WithMany(career => career.StudentCareers)
               .HasForeignKey(studentCareer => studentCareer.CareerId);

            modelBuilder.Entity<StudentCareer>()
                .HasKey(studentCareer => new { studentCareer.StudentId, studentCareer.CareerId });

            modelBuilder.Entity<StudentCareer>()
                .HasOne(studentCareer => studentCareer.Student)
                .WithMany(@student => @student.StudentCareers)
                .HasForeignKey(studentCareer => studentCareer.StudentId);

            modelBuilder.Entity<StudentCareer>()
               .HasOne(studentCareer => studentCareer.Career)
               .WithMany(career => career.StudentCareers)
               .HasForeignKey(studentCareer => studentCareer.CareerId);

            modelBuilder.Entity<CampusCareer>()
               .HasKey(campusCareer => new { campusCareer.CampusId, campusCareer.CareerId });

            modelBuilder.Entity<CampusCareer>()
                .HasOne(campusCareer => campusCareer.Campus)
                .WithMany(@campus => @campus.CampusCareer)
                .HasForeignKey(campusCareer => campusCareer.CampusId);

            modelBuilder.Entity<CampusCareer>()
               .HasOne(campusCareer => campusCareer.Career)
               .WithMany(career => career.CampusCareer)
               .HasForeignKey(campusCareer => campusCareer.CareerId);

            modelBuilder.Entity<Faculty>()
                .HasMany(f => f.Careers)
                .WithOne(c => c.Faculty);

            modelBuilder.Entity<StudentSection>()
               .HasKey(studentSection => new { studentSection.StudentId, studentSection.SectionId });

            modelBuilder.Entity<StudentSection>()
                .HasOne(studentSection => studentSection.Student)
                .WithMany(student => student.StudentSections)
                .HasForeignKey(StudentSection => StudentSection.StudentId);

            modelBuilder.Entity<StudentSection>()
                .HasOne(studentSection => studentSection.Section)
                .WithMany(section => section.StudentSections)
                .HasForeignKey(StudentSection => StudentSection.SectionId);

            modelBuilder.Entity<VinculationType>()
                .HasMany(v => v.Project)
                .WithOne(c => c.VinculationType);

            modelBuilder.Entity<SectionProject>()
               .HasKey(sectionProject => new { sectionProject.SectionId, sectionProject.ProjectId });

            modelBuilder.Entity<SectionProject>()
                .HasOne(sectionProject => sectionProject.Section)
                .WithMany(section => section.SectionProjects)
                .HasForeignKey(SectionProject => SectionProject.SectionId);

            modelBuilder.Entity<SectionProject>()
                .HasOne(sectionProject => sectionProject.Project)
                .WithMany(project => project.SectionProjects)
                .HasForeignKey(SectionProject => SectionProject.ProjectId);

            modelBuilder.Entity<ProjectOrganization>()
               .HasKey(projectOrganization => new { projectOrganization.ProjectId, projectOrganization.OrganizationId });

            modelBuilder.Entity<ProjectOrganization>()
                .HasOne(projectOrganization => projectOrganization.Project)
                .WithMany(project => project.ProjectOrganizations)
                .HasForeignKey(ProjectOrganization => ProjectOrganization.ProjectId);

            modelBuilder.Entity<ProjectOrganization>()
                .HasOne(projectOrganization => projectOrganization.ExternalOrganization)
                .WithMany(project => project.ProjectOrganizations)
                .HasForeignKey(ProjectOrganizations => ProjectOrganizations.OrganizationId);

            modelBuilder.Entity<Campus>()
                .HasMany(f => f.Professor)
                .WithOne(c => c.Campus);

            modelBuilder.Entity<Student>()
                .HasOne(s => s.Data)
                .WithOne(d => d.Student)
                .HasForeignKey<DataBot>(b => b.StudentRef);

            modelBuilder.Entity<Project>()
               .HasMany(f => f.ProjectHours)
               .WithOne(c => c.Project);

            modelBuilder.Entity<Section>()
               .HasMany(f => f.ProjectHours)
               .WithOne(c => c.Section);

            modelBuilder.Entity<Student>()
              .HasMany(f => f.ProjectHours)
              .WithOne(c => c.Student);

            modelBuilder.Entity<Class>()
              .HasMany(f => f.Sections)
              .WithOne(c => c.Class);




        }
    }
}