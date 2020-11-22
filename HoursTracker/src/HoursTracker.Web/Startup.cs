using HoursTracker.Core.Campuses;
using HoursTracker.Core.Careers;
using HoursTracker.Core.Classes;
using HoursTracker.Core.Faculties;
using HoursTracker.Core.Professors;
using HoursTracker.Core.ProjectHours;
using HoursTracker.Core.Projects;
using HoursTracker.Core.Sections;
using HoursTracker.Core.Students;
using HoursTracker.Core.Periods;
using HoursTracker.Data.Contexts;
using HoursTracker.Data.Repositories;
using HoursTracker.Data.Repositories.Bot;
using HoursTracker.Data.Repositories.Campuses;
using HoursTracker.Data.Repositories.Periods;
using HoursTracker.Data.Repositories.Careers;
using HoursTracker.Data.Repositories.Classes;
using HoursTracker.Data.Repositories.Faculties;
using HoursTracker.Data.Repositories.Professors;
using HoursTracker.Data.Repositories.ProjectHours;
using HoursTracker.Data.Repositories.Projects;
using HoursTracker.Data.Repositories.Sections;
using HoursTracker.Data.Repositories.Students;
using HoursTracker.Data.Repositories.Projects;
using HoursTracker.Domain.Aggregates.Campuses;
using HoursTracker.Domain.Aggregates.VinculationTypes;
using HoursTracker.Domain.Aggregates.Careers;
using HoursTracker.Domain.Aggregates.Periods;
using HoursTracker.Domain.Aggregates.Classes;
using HoursTracker.Domain.Aggregates.Faculties;
using HoursTracker.Domain.Aggregates.Professors;
using HoursTracker.Domain.Aggregates.ProjectHours;
using HoursTracker.Domain.Aggregates.Projects;
using HoursTracker.Domain.Aggregates.Sections;
using HoursTracker.Domain.Aggregates.Students;
using HoursTracker.Domain.Aggregates.Projects;
using HoursTracker.Domain.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using HoursTracker.Data.Repositories.VinculationTypes;
using HoursTracker.Core.VinculationTypes;
using HoursTracker.Web.Areas.Identity.Data;
using HoursTracker.Web.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using HoursTracker.Web.Models;

namespace HoursTracker.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddDbContext<HoursTrackerContext>(options =>
                    options
                        .UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = new PathString("/Identity/Account/AccessDenied");
            });

            services
                .AddControllersWithViews()
                .AddRazorRuntimeCompilation();

            services.AddRazorPages();

            ConfigureDependencies(services);
        }
        
        public static readonly ILoggerFactory MyLoggerFactory
            = LoggerFactory.Create(builder => { builder.AddConsole(); });

        private static void ConfigureDependencies(IServiceCollection services)
        {
            services.AddScoped<DbContext, HoursTrackerContext>();
            services.AddScoped(typeof(IBaseRepository<>), typeof(EfRepository<>));

            services.AddScoped<IProfessorRepository, ProfessorRepository>();
            services.AddScoped<IProfessorService, ProfessorService>();

            services.AddScoped<ICampusRepository, CampusRepository>();
            services.AddScoped<ICampusService, CampusService>();

            services.AddScoped<ICareerRepository, CareerRepository>();
            services.AddScoped<ICareerService, CareerService>();

            services.AddScoped<IClassRepository, ClassRepository>();
            services.AddScoped<IClassService, ClassService>();

            services.AddScoped<IStudentRepository, StudentsRepository>();
            services.AddScoped<IStudentService, StudentService>();

            services.AddScoped<IFacultyRepository, FacultyRepository>();
            services.AddScoped<IFacultyService, FacultyService>();

            services.AddScoped<IDataBotRepository, DataBotRepository>();

            services.AddScoped<IProjectHourRepository, ProjectHourRepository>();
            services.AddScoped<IProjectHourService, ProjectHourService>();

            services.AddScoped<ISectionRepository, SectionRepository>();
            services.AddScoped<ISectionService, SectionService>();

            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IProjectService, ProjectService>();

            services.AddScoped<IPeriodRepository, PeriodRepository>();
            services.AddScoped<IPeriodService, PeriodService>();

            services.AddScoped<IVinculationTypeRepository, VinculationTypesRepository>();
            services.AddScoped<IVinculationTypeService, VinculationTypeService>();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}")
                .RequireAuthorization();
                endpoints.MapRazorPages();
            });
        }
    }
}