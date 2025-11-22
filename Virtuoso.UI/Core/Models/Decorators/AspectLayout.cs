using System.Windows;
using System.Windows.Controls;

namespace Virtuoso.UI.Core.Models.Decorators;

/// <summary>
/// Decorator layout to enforce an aspect-locked size.
/// </summary>
public sealed class AspectLayout
    : Decorator
{
#region Variables

    /// <summary>
    /// Dependency property for <see cref="AspectRatio"/>
    /// </summary>
    public static readonly DependencyProperty AspectRatioProperty =
        DependencyProperty.Register(
            nameof(AspectRatio),
            typeof(double),
            typeof(AspectLayout),
            new FrameworkPropertyMetadata(1.0, FrameworkPropertyMetadataOptions.AffectsMeasure));


    /// <summary>
    /// Gets or sets the width-to-height ratio.
    /// Use 1.0 for squares.
    /// </summary>
    public double AspectRatio
    {
        get => (double) GetValue(AspectRatioProperty);
        set => SetValue(AspectRatioProperty, value);
    }

#endregion

#region Overrides

    /// <inheritdoc />
    protected override Size MeasureOverride(Size constraint) {
        if (Child is null)
            return new Size(0, 0);

        var targetSize = CoerceSize(constraint);
        Child.Measure(targetSize);
        return targetSize;
    }


    /// <inheritdoc />
    protected override Size ArrangeOverride(Size arrangeSize) {
        if (Child is null)
            return arrangeSize;

        var targetSize = CoerceSize(arrangeSize);
        var offsetX = (arrangeSize.Width - targetSize.Width) / 2;
        var offsetY = (arrangeSize.Height - targetSize.Height) / 2;

        Child.Arrange(new Rect(
            offsetX,
            offsetY,
            targetSize.Width,
            targetSize.Height
        ));

        return arrangeSize;
    }

#endregion

#region Internals

    /// <summary>
    /// Calculates the relevant size at the respective aspect ratio.
    /// </summary>
    private Size CoerceSize(Size available) {
        var ratio = AspectRatio <= 0 ? 1.0 : AspectRatio;

        var maxW = double.IsInfinity(available.Width)
            ? available.Height * ratio
            : available.Width;

        var maxH = double.IsInfinity(available.Height)
            ? available.Width / ratio
            : available.Height;

        var sizeW = Math.Min(maxW, maxH * ratio);
        var sizeH = sizeW / ratio;

        if (!double.IsNaN(sizeW) && !double.IsNaN(sizeH)) {
            return new Size(
                Math.Max(0, sizeW),
                Math.Max(0, sizeH)
            );
        }

        sizeW = 0;
        sizeH = 0;
        return new Size(
            Math.Max(0, sizeW),
            Math.Max(0, sizeH)
        );
    }

#endregion
}
