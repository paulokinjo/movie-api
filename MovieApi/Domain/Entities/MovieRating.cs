namespace MovieApi.Domain.Entities;

public sealed class MovieRating
{
    public int Id { get; private set; }

    public double Rating { get; private set; }

    public string Review { get; private set; }

    public int MovieId { get; private set; }

    public Movie? Movie { get; private set; }

    public MovieRating(double rating, string review)
    {
        if (rating < 0 || rating > 10)
            throw new ArgumentException("Rating must be between 0 and 10.", nameof(rating));

        if (string.IsNullOrWhiteSpace(review))
            throw new ArgumentException("Review cannot be empty.", nameof(review));

        Rating = rating;
        Review = review;
    }
}
