using CustomControls.Helpers;

namespace CustomControls.Controls;

public partial class ReusableFloatingEntry : ContentView
{
    public ReusableFloatingEntry()
    {
        InitializeComponent();
        BindingContext = this;
    }
    
    bool _isFocused;
    public static readonly BindableProperty InputTypeProperty =
        BindableProperty.Create(nameof(InputType), typeof(Constants.EntryInputType),
            typeof(ReusableFloatingEntry), 
            Constants.EntryInputType.None);
    public Constants.EntryInputType InputType
    {
        get => (Constants.EntryInputType)GetValue(InputTypeProperty);
        set => SetValue(InputTypeProperty, value);
    }
    
    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(nameof(Text), 
            typeof(string), typeof(ReusableFloatingEntry), 
            default(string), BindingMode.TwoWay);
    
    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    
    public static readonly BindableProperty PlaceholderProperty =
        BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(ReusableFloatingEntry), string.Empty);

    public string Placeholder
    {
        get => (string)GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }
    
    void InputEntry_Focused(object sender, FocusEventArgs e)
    {
        _isFocused = true;
        UpdateFloatingState(animate:true);
    }

    void InputEntry_Unfocused(object sender, FocusEventArgs e)
    {
        _isFocused = false;
        UpdateFloatingState(animate:true);
    }

    void InputEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        UpdateFloatingState(animate:false);
    }

    void UpdateFloatingState(bool animate)
    {
        bool shouldFloat = _isFocused || !string.IsNullOrEmpty(Text);
        if (animate)
        {
            uint duration = 100;
            if (shouldFloat)
            {
                FloatingLabel.TranslateTo(0, -18, duration);
                FloatingLabel.FontSize = 10;
            }
            else
            {
                FloatingLabel.TranslateTo(0, 0, duration);
                FloatingLabel.FontSize = 14;
            }
        }
        else
        {
            FloatingLabel.TranslationY = shouldFloat ? -18 : 0;
            FloatingLabel.FontSize     = shouldFloat ? 10  : 14;
        }
    }
}