using Microsoft.EntityFrameworkCore;
using SchoolProject.Data;
using SchoolProject.Models;
using SchoolProject.Repositories;

namespace SchoolProject
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
            // Add in-memory database
            services.AddDbContext<SchoolDbContext>(options =>
                options.UseInMemoryDatabase(databaseName: "SchoolDatabase"));

            // Add repositories
            services.AddScoped<IStudentRepository, StudentRepository>();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<SchoolDbContext>();
                context.Database.EnsureCreated();
                if (!context.Students.Any())
                {
                    context.Students.AddRange(
                        new Student
                        {
                            Id = 1,
                            Name = "Alice",
                            Age = 20,
                            Hobbies = new List<string> { "reading", "swimming", "coding" }.ToArray()
                        },
                        new Student
                        {
                            Id = 2,
                            Name = "Bob",
                            Age = 22,
                            Hobbies = new List<string> { "painting", "dancing", "singing" }.ToArray()
                        }
                    );
                    context.SaveChanges();
                }
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

