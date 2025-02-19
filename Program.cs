
using Labb3_API.Data;
using Labb3_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Labb3_API
{
    public class Program
    {
       
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //Connection
            builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            // ------- Endpoints för Projects --------

            //Post
            app.MapPost("/api/projects", async (Project project, AppDbContext db) =>
            {
                db.Projects.Add(project);
                await db.SaveChangesAsync();

                return Results.Ok(project);
            });

            //Get all
            app.MapGet("/api/projects", async (AppDbContext db) =>
                await db.Projects.ToListAsync());

            //GetById
            app.MapGet("/api/projects/{id}", async (int id, AppDbContext db) =>
            {
                var project = await db.Projects.FindAsync(id);

                if(project == null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(project);
            });

            //Put
            app.MapPut("/api/projects/{id}", async (int id, Project updatedProject, AppDbContext db) =>
            {
                var existingProject = await db.Projects.FindAsync(id);

                if ( existingProject == null)
                {
                    return Results.NotFound();
                }

                existingProject.Title = updatedProject.Title;
                existingProject.Description = updatedProject.Description;
                existingProject.TechnologiesUsed = updatedProject.TechnologiesUsed;

                await db.SaveChangesAsync();

                return Results.Ok(updatedProject);
            });

            //Delete
            app.MapDelete("/api/projects/{id}", async (int id, AppDbContext db) =>
            {
                var project = await db.Projects.FindAsync(id);

                if (project == null)
                {
                    return Results.NotFound();
                }

                db.Projects.Remove(project);

                await db.SaveChangesAsync();
                return Results.NoContent();
            });

            // ------- Endpoints för Technology --------

            //Post
            app.MapPost("/api/technologies", async (Technology tech, AppDbContext db) =>
            {
                db.Technologies.Add(tech);

                await db.SaveChangesAsync();
                return Results.Ok(tech);
            });

            //Get all
            app.MapGet("/api/technologies", async (AppDbContext db) =>
                await db.Technologies.ToListAsync());

            //GetById
            app.MapGet("/api/technologies/{id}", async (int id, AppDbContext db) =>
            {
                var tech = await db.Technologies.FindAsync(id);

                if (tech == null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(tech);
            });

            //Put
            app.MapPut("/api/technologies/{id}", async (int id, AppDbContext db, Technology updatedTech) =>
            {
                var existingTech = await db.Technologies.FindAsync(id);
                
                if (existingTech == null)
                {
                    return Results.NotFound();
                }

                existingTech.Name = updatedTech.Name;
                existingTech.YearsOfExperience = updatedTech.YearsOfExperience;
                existingTech.SkillLevel = updatedTech.SkillLevel;

                await db.SaveChangesAsync();

                return Results.Ok(existingTech);
            });

            //Delete
            app.MapDelete("/api/technologies/{id}", async (int id, AppDbContext db) =>
            {
                var tech = await db.Technologies.FindAsync(id);

                if (tech == null)
                {
                    return Results.NotFound();
                }

                db.Technologies.Remove(tech);

                await db.SaveChangesAsync();
                return Results.NoContent();
            });





            app.Run();
        }
    }
}
