using System.Windows.Input;

namespace ConnectApp.Maui.Pages.Controls;

public partial class FramedTile : ContentView
{
    public FramedTile()
    {
        InitializeComponent();
    }

    //public static readonly BindableProperty ContentIconProperty, ContentTitleProperty, ContentTextProperty, ButtonTextProperty;
    //public static readonly BindableProperty ShowIconProperty, ShowTitleProperty, ShowTextProperty, RepeatTextHackProperty, ShowButtonProperty;
    //public static readonly BindableProperty ContentColourProperty, TapCommandProperty, TapParameterProperty;

    public static readonly BindableProperty ContentIconProperty = BindableProperty.Create(nameof(ContentIcon), typeof(string), typeof(FramedTile), null, propertyChanged: OnContentChanged);
    public static readonly BindableProperty ContentTitleProperty = BindableProperty.Create(nameof(ContentTitle), typeof(string), typeof(FramedTile), string.Empty, propertyChanged: OnContentChanged);
    public static readonly BindableProperty ContentTextProperty = BindableProperty.Create(nameof(ContentText), typeof(string), typeof(FramedTile), string.Empty, propertyChanged: OnContentChanged);
    public static readonly BindableProperty ButtonTextProperty = BindableProperty.Create(nameof(ButtonText), typeof(string), typeof(FramedTile), string.Empty, propertyChanged: OnContentChanged);

    public static readonly BindableProperty ShowIconProperty = BindableProperty.Create(nameof(ShowIcon), typeof(bool), typeof(FramedTile), false);
    public static readonly BindableProperty ShowTitleProperty = BindableProperty.Create(nameof(ShowTitle), typeof(bool), typeof(FramedTile), false);
    public static readonly BindableProperty ShowTextProperty = BindableProperty.Create(nameof(ShowText), typeof(bool), typeof(FramedTile), false);
    public static readonly BindableProperty RepeatTextHackProperty = BindableProperty.Create(nameof(RepeatTextHack), typeof(bool), typeof(FramedTile), false);
    public static readonly BindableProperty ShowButtonProperty = BindableProperty.Create(nameof(ShowButton), typeof(bool), typeof(FramedTile), false);

    public static BindableProperty ContentColourProperty = BindableProperty.Create(nameof(ContentColour), typeof(Color), typeof(FramedTile));

    public static readonly BindableProperty TapCommandProperty = BindableProperty.Create(nameof(TapCommand), typeof(ICommand), typeof(FramedTile), null);
    public static readonly BindableProperty TapParameterProperty = BindableProperty.Create(nameof(TapParameter), typeof(string), typeof(FramedTile), null);

    private static void OnContentChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var tile = (FramedTile)bindable;
        tile.SetValue(ShowIconProperty, tile.ShowIcon);
        tile.SetValue(ShowTitleProperty, tile.ShowTitle);
        tile.SetValue(ShowTextProperty, tile.ShowText);
        tile.SetValue(RepeatTextHackProperty, tile.RepeatTextHack);
        tile.SetValue(ShowButtonProperty, tile.ShowButton);
    }

    public string ContentIcon
    {
        get => (string)GetValue(ContentIconProperty);
        set => SetValue(ContentIconProperty, value);
    }

    public string ContentTitle
    {
        get => (string)GetValue(ContentTitleProperty);
        set => SetValue(ContentTitleProperty, value);
    }

    public string ContentText
    {
        get => (string)GetValue(ContentTextProperty);
        set => SetValue(ContentTextProperty, value);
    }

    public string ButtonText
    {
        get => (string)GetValue(ButtonTextProperty);
        set => SetValue(ButtonTextProperty, value);
    }

    public bool ShowIcon
    {
        get => !string.IsNullOrWhiteSpace(ContentIcon);
    }

    public bool ShowTitle
    {
        get => !string.IsNullOrWhiteSpace(ContentTitle);
    }

    public bool ShowText
    {
        get => !string.IsNullOrWhiteSpace(ContentText);
    }

    public bool RepeatTextHack
    {
        get => ShowText && !ShowButton;
    }

    public bool ShowButton
    {
        get => !string.IsNullOrWhiteSpace(ButtonText);
    }

    public Color ContentColour
    {
        get => (Color)(GetValue(ContentColourProperty));
        set => SetValue(ContentColourProperty, value);
    }

    public ICommand TapCommand
    {
        get => (ICommand)GetValue(TapCommandProperty);
        set => SetValue(TapCommandProperty, value);
    }

    public string TapParameter
    {
        get => (string)GetValue(TapParameterProperty);
        set => SetValue(TapParameterProperty, value);
    }

}
