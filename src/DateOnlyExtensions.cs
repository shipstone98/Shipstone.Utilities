using System;

namespace Shipstone.Utilities;

/// <summary>
/// Provides a set of <c>static</c> methods (<c>Shared</c> in Visual Basic) methods for querying instances of <see cref="DateOnly" />.
/// </summary>
public static class DateOnlyExtensions
{
    /// <summary>
    /// Gets the age of a person born on the specified date.
    /// </summary>
    /// <param name="date">The date the person was born.</param>
    /// <param name="today">The current date.</param>
    /// <returns>The age of a person born on the specified date, expressed as years.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><c><paramref name="today" /></c> is less than <c><paramref name="date" /></c>.</exception>
    public static int GetAgeYears(this DateOnly date, DateOnly today)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(today, date);
        int age = today.Year - date.Year;
        int todayMonth = today.Month;
        int dateMonth = date.Month;

        if (
            todayMonth < dateMonth
            || (todayMonth == dateMonth && today.Day < date.Day)
        )
        {
            -- age;
        }

        return age;
    }
}
