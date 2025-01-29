using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MovieApi;
using MovieApi.Data;
using MovieApi.Domain.Entities;
using MovieApi.Service;
using MovieApi.Service.Contracts;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Bind AppSettings to the configuration section
builder.Services.Configure<AppSettings>(builder.Configuration);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("MovieDb")
);
builder.Services.AddScoped<IMovieService, MovieService>();
builder.Services.AddScoped<IActorService, ActorService>();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Use IOptions<AppSettings> to get the ApiSecret in the JWT Bearer authentication configuration
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false,
            IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>(nameof(AppSettings.ApiSecret)))
            ),
        };
    });

// Configure Swagger to accept JWT token
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            Description = "Please enter your Bearer token",
        }
    );

    c.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer",
                    },
                },
                new string[] { }
            },
        }
    );
});

// Configure CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseCors("AllowAllOrigins");

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    if (!context.Movies.Any())
    {
        // Create a Faker instance for Movie, Actor, and MovieRating
        var movieFaker = new Faker<Movie>()
            .CustomInstantiator(f => new Movie(f.Random.Word(), f.Date.Past(20).Year)) // Custom constructor
            .RuleFor(m => m.Actors, f => f.Make(1, () => new Actor(f.Person.FullName)))
            .RuleFor(m => m.Ratings, f => f.Make(f.Random.Int(2, 5), () => new MovieRating(f.Random.Double(1, 10), f.Lorem.Sentence())));

        // Generate 20 movies
        var movies = movieFaker.Generate(20);

        context.Movies.AddRange(movies);
        context.SaveChanges();
    }
}

app.Run();
