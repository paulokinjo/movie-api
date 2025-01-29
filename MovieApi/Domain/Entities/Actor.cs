
using System.Collections.ObjectModel;

namespace MovieApi.Domain.Entities;

public sealed class Actor
{
    public int Id { get; private set; }

    public string Name { get; private set; }

    public ICollection<Movie> Movies { get; private set; }

    public Actor(string name, ICollection<Movie> movies = null!)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Actor name cannot be empty", nameof(name));
        }

        Name = name;
        Movies = movies ?? new List<Movie>();
    }

    public bool IsValid() => !string.IsNullOrWhiteSpace(Name) && Name.Length <= 100;

    public override bool Equals(object? obj)
    {
        if (obj is Actor otherActor)
        {
            return Id == otherActor.Id;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    #region Required for EF
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    protected Actor()
    {
        Movies = new List<Movie>();
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    #endregion
}
