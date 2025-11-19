using ReactiveUI;

namespace Virtuoso.UI.Views.Home;

/// <summary>
/// View model for <see cref="HomeView"/>
/// </summary>
public sealed partial class HomeViewModel
    : ReactiveObject, IActivatableViewModel
{
#region Variables

    public ViewModelActivator Activator { get; } = new();

#endregion
}
