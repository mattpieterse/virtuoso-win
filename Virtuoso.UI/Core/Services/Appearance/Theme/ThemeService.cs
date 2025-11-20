using System.Windows;
using System.Windows.Media;
using Wpf.Ui.Appearance;

namespace Virtuoso.UI.Core.Services.Appearance.Theme;

public sealed class ThemeService
    : IThemeService
{
#region Variables

    private const string ThemeFilePrefix = "Theme.";
    private const string LightThemeFilename = "Theme.Light.xaml";
    private const string NightThemeFilename = "Theme.Night.xaml";


    private readonly string _assembly =
        System.Reflection.Assembly.GetExecutingAssembly().GetName().Name!
        ?? throw new InvalidOperationException("App assembly is missing");


    /// <summary>
    /// Lock to prevent multiple subscriptions to the event stream.
    /// </summary>
    private bool _isSubscribed;

#endregion

#region Extension

    /// <summary>
    /// Listen for and act upon WPF-UI theme changing events.
    /// </summary>
    public void Listen() {
        if (_isSubscribed) return;
        ApplicationThemeManager.Changed += OnThemeChanged;
        _isSubscribed = true;
    }


    /// <summary>
    /// Ignore WPF-UI theme change events until resubscribed.
    /// </summary>
    /// <remarks>
    /// Warning: If any theme changes occur while the listener is ignored, the
    /// service will not automatically re-apply the correct theme dictionaries
    /// upon resubscription.
    /// </remarks>
    public void Ignore() {
        if (!_isSubscribed) return;
        ApplicationThemeManager.Changed -= OnThemeChanged;
        _isSubscribed = false;
    }

#endregion

#region Internals

    /// <summary>
    /// Iterates through the defined application resource dictionaries (merged)
    /// and removes any matching the prefix convention for custom dictionaries.
    /// This is a safe call if none are defined.
    /// </summary>
    private static void ClearThemeDictionaries() {
        var resources = Application.Current.Resources.MergedDictionaries;
        var oldThemeDictionaries = resources
            .Where(d => d.Source?.OriginalString.Contains(ThemeFilePrefix) == true)
            .ToList();

        if (oldThemeDictionaries.Count == 0) return;
        foreach (var dictionary in oldThemeDictionaries) {
            resources.Remove(dictionary);
        }
    }


    /// <summary>
    /// Event handler for <see cref="ApplicationThemeManager.Changed"/> events.
    /// </summary>
    private void OnThemeChanged(
        ApplicationTheme theme,
        Color accent = default
    ) {
        ClearThemeDictionaries();

        var customThemeFilename = (theme == ApplicationTheme.Light)
            ? LightThemeFilename
            : NightThemeFilename;

        var updatedDictionary = new ResourceDictionary {
            Source = new Uri(
                $"pack://application:,,,/{_assembly};component/Assets/Styles/{customThemeFilename}",
                UriKind.Absolute
            )
        };

        var resources = Application.Current.Resources.MergedDictionaries;
        resources.Insert(index: 3, updatedDictionary);
    }

#endregion
}
