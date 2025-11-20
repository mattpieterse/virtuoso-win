using Wpf.Ui;
using Wpf.Ui.Controls;

namespace Virtuoso.UI.Core.Services.Appearance.Toasts;

public class ToastService(
    ISnackbarService snackbarService
) : IToastService
{
#region Internals

    /// <summary>
    /// Constructs a <see cref="ISnackbarService"/> snackbar and displays it
    /// with <see cref="ISnackbarService.Show"/>. This is an internal method for
    /// the convenience extensions to use for construction.
    /// </summary>
    private void Show(
        string heading,
        string message,
        ControlAppearance appearance,
        SymbolRegular defaultSymbol,
        SymbolIcon? icon = null
    ) {
        snackbarService.Show(
            title: heading,
            message: message,
            timeout: TimeSpan.FromSeconds(10),
            appearance: appearance,
            icon: icon ?? new SymbolIcon() {
                Symbol = defaultSymbol
            }
        );
    }

#endregion

#region Functions

    /// <summary>
    /// Invokes a toast styled as <see cref="ControlAppearance.Success"/>.
    /// </summary>
    public void Success(
        string heading,
        string message,
        SymbolIcon? icon = null
    ) {
        Show(
            heading,
            message,
            ControlAppearance.Success,
            SymbolRegular.CheckmarkCircle24,
            icon
        );
    }


    /// <summary>
    /// Invokes a toast styled as <see cref="ControlAppearance.Danger"/>.
    /// </summary>
    public void Failure(
        string heading,
        string message,
        SymbolIcon? icon = null
    ) {
        Show(
            heading,
            message,
            ControlAppearance.Danger,
            SymbolRegular.DismissCircle24,
            icon
        );
    }


    /// <summary>
    /// Invokes a toast styled as <see cref="ControlAppearance.Caution"/>.
    /// </summary>
    public void Warning(
        string heading,
        string message,
        SymbolIcon? icon = null
    ) {
        Show(
            heading,
            message,
            ControlAppearance.Caution,
            SymbolRegular.ErrorCircle24,
            icon
        );
    }


    /// <summary>
    /// Invokes a toast styled as <see cref="ControlAppearance.Info"/>.
    /// </summary>
    public void Details(
        string heading,
        string message,
        SymbolIcon? icon = null
    ) {
        Show(
            heading,
            message,
            ControlAppearance.Info,
            SymbolRegular.Info24,
            icon
        );
    }


    /// <summary>
    /// Invokes a toast styled as <see cref="ControlAppearance.Primary"/>.
    /// </summary>
    public void Primary(
        string heading,
        string message,
        SymbolIcon? icon = null
    ) {
        Show(
            heading,
            message,
            ControlAppearance.Primary,
            SymbolRegular.Sparkle24,
            icon
        );
    }


    /// <summary>
    /// Invokes a toast styled as <see cref="ControlAppearance.Secondary"/>.
    /// </summary>
    public void Default(
        string heading,
        string message,
        SymbolIcon? icon = null
    ) {
        Show(
            heading,
            message,
            ControlAppearance.Secondary,
            SymbolRegular.Sparkle24,
            icon
        );
    }

#endregion
}
