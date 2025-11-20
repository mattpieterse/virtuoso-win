namespace Virtuoso.UI.Core.Models.Intents;

/// <summary>
/// Contract to define a messenger intent for navigation with parameters.
/// </summary>
public interface INavigationIntent
{
#region Contract

    string Argument { get; }

#endregion
}
