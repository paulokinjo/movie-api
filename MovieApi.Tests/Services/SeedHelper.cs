using MovieApi.Data;
using MovieApi.Domain.Entities;

namespace MovieApi.Tests
{
    internal static class SeedHelper
    {

        public static void SeedDatabase(ApplicationDbContext context)
        {
            // Add initial movies and actors to the in-memory database
            var actors = new List<Actor>
            {
                new Actor("Test Actor 1"),
                new Actor("Test Actor 2"),
                new Actor("Test Actor 3")
            };

            var movie = new Movie("Test Movie", 2025);
            movie.Actors.Add(actors[0]);
            movie.Actors.Add(actors[1]);

            var movie2 = new Movie("Test Movie 2", 2024);
            movie2.Actors.Add(actors[0]);
            movie2.Actors.Add(actors[2]);

            context.Actors.AddRange(actors);
            context.Movies.Add(movie);
            context.Movies.Add(movie2);
            context.SaveChanges();
        }
    }
}