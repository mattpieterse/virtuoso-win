using Wpf.Ui.Controls;

namespace Virtuoso.UI.Core.Services.Appearance.Toasts;

public interface IToastService
{
#region Contracts

    public void Success(
        string heading,
        string message,
        SymbolIcon? icon = null
    );

    public void Warning(
        string heading,
        string message,
        SymbolIcon? icon = null
    );

    public void Failure(
        string heading,
        string message,
        SymbolIcon? icon = null
    );

    public void Details(
        string heading,
        string message,
        SymbolIcon? icon = null
    );

    public void Default(
        string heading,
        string message,
        SymbolIcon? icon = null
    );

    public void Primary(
        string heading,
        string message,
        SymbolIcon? icon = null
    );

#endregion
}
