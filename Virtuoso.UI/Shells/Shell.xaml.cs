using System.Diagnostics.CodeAnalysis;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using CommunityToolkit.Mvvm.DependencyInjection;
using ReactiveUI;
using Virtuoso.UI.Core.Models.Intents;
using Virtuoso.UI.Views.Decks;
using Virtuoso.UI.Views.Home;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace Virtuoso.UI.Shells;

public sealed partial class Shell
    : IViewFor<ShellViewModel>
{
#region Variables

    [AllowNull]
    public ShellViewModel ViewModel { get; set; }


    [NotNullIfNotNull(nameof(ViewModel))]
    object? IViewFor.ViewModel
    {
        get => ViewModel;
        set => ViewModel = (ShellViewModel?) value;
    }


    public ViewModelActivator Activator { get; } = new();

#endregion

#region Lifecycle

    /// <summary>
    /// Constructor for <see cref="Shell"/>
    /// </summary>
    public Shell(
        ShellViewModel viewModel
    ) {
        ViewModel = viewModel;
        DataContext = ViewModel;

        InitializeComponent();
        InitializeUiHosters();
        this.WhenActivated((disposables) => {
            this.OneWayBind(
                ViewModel,
                bind => bind.NavigationHeaderItems,
                view => view.NavigationHost.MenuItemsSource
            ).DisposeWith(disposables);

            this.OneWayBind(
                ViewModel,
                bind => bind.NavigationFooterItems,
                view => view.NavigationHost.FooterMenuItemsSource
            ).DisposeWith(disposables);

            var onSidebarNavigationComplete = Observable
                .FromEvent<TypedEventHandler<NavigationView, NavigatedEventArgs>, NavigatedEventArgs>(
                    handler => (_, args) => handler(args),
                    h => NavigationHost.Navigated += h,
                    h => NavigationHost.Navigated -= h
                );

            onSidebarNavigationComplete
                .Where((args) => args.Page is DeckView)
                .Delay(TimeSpan.FromMilliseconds(50))
                .ObserveOn(RxSchedulers.MainThreadScheduler)
                .Subscribe((_) => {
                    var selectedItem = NavigationHost.SelectedItem as NavigationViewItem;
                    var argumentPage = selectedItem?.TargetPageTag ?? string.Empty;
                    Ioc.Default
                        .GetRequiredService<IMessageBus>()
                        .SendMessage(new DeckNavigationIntent(argumentPage));
                })
                .DisposeWith(disposables);

            StartNavigation();
        });
    }

#endregion

#region Internals

    /// <summary>
    /// Initializes services with their UI dependencies.
    /// </summary>
    /// <remarks>
    /// This is for <see cref="Wpf.Ui"/> services that require controls such as
    /// the <see cref="NavigationView"/> and others to host/display information
    /// throughout the application.
    /// </remarks>
    /// <seealso cref="IContentDialogService"/>
    /// <seealso cref="INavigationService"/>
    /// <seealso cref="ISnackbarService"/>
    private void InitializeUiHosters() {
        Ioc.Default
            .GetRequiredService<IContentDialogService>()
            .SetDialogHost(DialogHost);
        Ioc.Default
            .GetRequiredService<INavigationService>()
            .SetNavigationControl(NavigationHost);
        Ioc.Default
            .GetRequiredService<ISnackbarService>()
            .SetSnackbarPresenter(Toaster);
    }


    /// <summary>
    /// Calls the initial screen for the window.
    /// </summary>
    private static void StartNavigation() {
        Ioc.Default
            .GetRequiredService<INavigationService>()
            .Navigate(typeof(HomeView));
    }

#endregion
}
