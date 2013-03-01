using System;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// ScoringIdentifiers identify values that can be used in city scoring. 
/// The values may be scalar or may be obtained through the use of a function with an input parameter.
/// </summary>
public class ScoringIdentifier
{
    /// <summary>
    /// Scoring identifier name; identifiers are referred to by name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// A shorter (theoretically more user-friendly) identifier name.
    /// </summary>
    public string ShortName { get; set; }

    /// <summary>
    /// A short description of the identifier, for display to user.
    /// </summary>
    public string Descrition { get; set; }

    /// <summary>
    /// The name of the corresponding <see cref="CityInfo"/> object property.
    /// </summary>
    public string PropertyName { get; set; }

}