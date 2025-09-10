
using Microsoft.EntityFrameworkCore;
using PlatformSchool.Application.Contracts;
using PlatformSchool.Domain.Repositories;
using PlatformSchool.Persistence.Repositories;
using PlatformSchool.Application.Services;
using PlatformSchool.Persistence.Context;

namespace PlatformSchool.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configuración de Entity Framework
            builder.Services.AddDbContext<SchoolDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Registro de repositorios
            builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            builder.Services.AddScoped<ICourseRepository, CourseRepository>();
            builder.Services.AddScoped<IStudentRepository, StudentRepository>();

            // Registro de servicios
            builder.Services.AddTransient<IDepartmentService, DepartmentService>();
            builder.Services.AddTransient<ICourseService, CourseService>();
            builder.Services.AddTransient<IStudentService, StudentService>();

            // Configuración de controladores y Swagger
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new() { Title = "PlatformSchool API", Version = "v1" });
            });

            // Configuración de CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            var app = builder.Build();

            // Configuración del pipeline de la aplicación
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "PlatformSchool API v1");
                });
            }

            app.UseCors("AllowAll");
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
