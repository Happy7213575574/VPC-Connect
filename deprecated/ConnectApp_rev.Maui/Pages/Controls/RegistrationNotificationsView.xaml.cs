using System.Windows.Input;

namespace ConnectApp.Maui.Pages.Controls;

public partial class RegistrationNotificationsView : ContentView
{
    public RegistrationNotificationsView()
    {
        InitializeComponent();
    }

    public static readonly BindableProperty ShowSuccessProperty = BindableProperty.Create(nameof(ShowSuccess), typeof(bool), typeof(RegistrationNotificationsView), false);
    public static readonly BindableProperty ShowInstructionProperty = BindableProperty.Create(nameof(ShowInstruction), typeof(bool), typeof(RegistrationNotificationsView), false);
    public static readonly BindableProperty TapCommandProperty = BindableProperty.Create(nameof(TapCommand), typeof(ICommand), typeof(RegistrationNotificationsView), null);

    public bool ShowSuccess
    {
        get => (bool)GetValue(ShowSuccessProperty);
        set => SetValue(ShowSuccessProperty, value);
    }

    public bool ShowInstruction
    {
        get => (bool)GetValue(ShowInstructionProperty);
        set => SetValue(ShowInstructionProperty, value);
    }

    public ICommand TapCommand
    {
        get => (ICommand)GetValue(TapCommandProperty);
        set => SetValue(TapCommandProperty, value);
    }
}
