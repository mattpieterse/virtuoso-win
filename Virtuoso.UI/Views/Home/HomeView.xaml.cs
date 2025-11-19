using System.Diagnostics.CodeAnalysis;
using ReactiveUI;
using Virtuoso.UI.Shells;
using Wpf.Ui.Abstractions.Controls;

namespace Virtuoso.UI.Views.Home;

/// <summary>
/// Backing class for a view within the <see cref="Shell"/>
/// </summary>
public sealed partial class HomeView
    : IViewFor<HomeViewModel>, INavigableView<HomeViewModel>
{
#region Variables

    [AllowNull]
    public HomeViewModel ViewModel { get; set; }


    [NotNullIfNotNull(nameof(ViewModel))]
    object? IViewFor.ViewModel
    {
        get => ViewModel;
        set => ViewModel = (HomeViewModel?) value;
    }

#endregion

#region Lifecycle

    /// <summary>
    /// Constructor for <see cref="HomeView"/>
    /// </summary>
    public HomeView(
        HomeViewModel viewModel
    ) {
        ViewModel = viewModel;
        DataContext = viewModel;

        InitializeComponent();
        this.WhenActivated((disposables) => {
            // ...
        });
    }

#endregion
}
