using Microsoft.VisualBasic;
using System.Collections.ObjectModel;

namespace MovieApi.Domain.Entities;

public sealed class Movie
{
    public int Id { get; private set; }
    public string Title { get; private set; }
    public int Year { get; private set; }
    public ICollection<Actor> Actors { get; private set; }
    public ICollection<MovieRating> Ratings { get; private set; }

    public Movie(string title, int year)
    {
        Id = 0;
        Title = title;
        Year = year;

        Actors = new Collection<Actor>();
        Ratings = new Collection<MovieRating>();
    }

    public void SetTitle(string title) => Title = title;

    public void SetYear(int year) => Year = year;

    public bool IsValid() => !string.IsNullOrWhiteSpace(Title) && Year > 1800 && Year <= DateTime.Now.Year;

    public override bool Equals(object? obj)
    {
        if (obj is Movie otherMovie)
        {
            return Id == otherMovie.Id;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    #region Required for EF
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    protected Movie() 
    {
        Actors = new Collection<Actor>();
        Ratings = new Collection<MovieRating>();
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    #endregion
}
