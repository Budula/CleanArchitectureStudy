using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Movies.Application.Handlers;
using Movies.Core.Repositories;
using Movies.Core.Repositories.Base;
using Movies.Infraestructure;
using Movies.Infraestructure.Data;
using Movies.Infraestructure.Repositories.Base;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddApiVersioning();
builder.Services.AddDbContext<MovieContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("MovieDBConnection")), ServiceLifetime.Singleton);

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo{ Title = "MovieAPI Review", Version = "v1" });
});
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddMediatR(c=>c.RegisterServicesFromAssemblies(typeof(CreateMovieCommandHandler).GetTypeInfo().Assembly));

builder.Services.AddScoped(typeof(IRepository<>),typeof(Repository<>));
builder.Services.AddTransient<IMovieRepository, MovieRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseEndpoints(endpoint =>
{
    endpoint.MapControllers();
});
app.UseSwagger();
app.UseSwaggerUI(c=>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json","Movie API V1");
});
await CreateAndSeed(app);
app.Run();

static async Task CreateAndSeed(IHost host)
{
    using(var scope = host.Services.CreateScope())
    {
        var service = scope.ServiceProvider;
        var loggerFactory = service.GetRequiredService<ILoggerFactory>();
        try
        {
            var movieContext = service.GetRequiredService<MovieContext>();
            await MovieContextSeed.SeedAsync(movieContext, loggerFactory);
        }
        catch (Exception ex)
        {
            var logger = loggerFactory.CreateLogger<Program>();
            logger.LogError($"Exception occured in API: {ex.Message}");
            
        }

    }
}