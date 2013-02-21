using System;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// ScoringIdentifiers identify values that can be used in city scoring. 
/// The values may be scalar or may be obtained through the use of a function with an input parameter.
/// </summary>
public class ScoringIdentifier
{
    public string Name { get; set; }

    /// <summary>
    /// A short description of the identifier, for display to user.
    /// </summary>
    public string Descrition { get; set; }

    /// <summary>
    /// The data type of the value refereced by this identifier. 
    /// </summary>
    public Type DataType { get; set; }

    /// <summary>
    /// Set to true if this scoring identifier is a function requiring an input parameter.
    /// </summary>
    public bool IsFunction { get; set; }

    /// <summary>
    /// The type expected as an input parameter.
    /// Only applies if <see cref="IsFunction"/> is true.
    /// </summary>
    public Type InputParameterType { get; set; }
}