using System.Text.RegularExpressions;
using CustomControls.Helpers;

namespace CustomControls.Controls;

public partial class ReusableEntry : ContentView
{
    public ReusableEntry()
    {
        InitializeComponent();
        BindingContext = this;
    }
    
    public static readonly BindableProperty IsRequiredProperty =
        BindableProperty.Create(nameof(IsRequired), typeof(bool), typeof(ReusableEntry), false);

    public bool IsRequired
    {
        get => (bool)GetValue(IsRequiredProperty);
        set => SetValue(IsRequiredProperty, value);
    }
    
    public static readonly BindableProperty AllowClearProperty =
        BindableProperty.Create(nameof(AllowClear), typeof(bool), typeof(ReusableEntry), false);

    public bool AllowClear
    {
        get => (bool)GetValue(AllowClearProperty);
        set => SetValue(AllowClearProperty, value);
    }
    
    public static readonly BindableProperty IsValidProperty =
        BindableProperty.Create(
            nameof(IsValid),
            typeof(bool),
            typeof(ReusableEntry),
            false,
            BindingMode.OneWayToSource,
            propertyChanged: OnIsValidChanged);

    public bool IsValid
    {
        get => (bool)GetValue(IsValidProperty);
        private set => SetValue(IsValidProperty, value);
    }
    
    public event EventHandler<bool>? IsValidChanged;

    static void OnIsValidChanged(BindableObject bindable, object oldVal, object newVal)
    {
        var ctl = (ReusableEntry)bindable;
        ctl.IsValidChanged?.Invoke(ctl, (bool)newVal);
    }
    
    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(nameof(Text), typeof(string), typeof(ReusableEntry), default(string), BindingMode.TwoWay);

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public static readonly BindableProperty PlaceholderProperty =
        BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(ReusableEntry), string.Empty);

    public string Placeholder
    {
        get => (string)GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }

    public static readonly BindableProperty IsPasswordProperty =
        BindableProperty.Create(nameof(IsPassword), typeof(bool), typeof(ReusableEntry), false);

    public bool IsPassword
    {
        get => (bool)GetValue(IsPasswordProperty);
        set => SetValue(IsPasswordProperty, value);
    }
    
    public static readonly BindableProperty MinLengthProperty =
        BindableProperty.Create(nameof(MinLength), typeof(int), typeof(ReusableEntry), 0);

    public int MinLength
    {
        get => (int)GetValue(MinLengthProperty);
        set => SetValue(MinLengthProperty, value);
    }
    
    public static readonly BindableProperty InputTypeProperty =
        BindableProperty.Create(nameof(InputType), 
            typeof(Constants.EntryInputType), 
            typeof(ReusableEntry), 
            Constants.EntryInputType.None);
    public Constants.EntryInputType InputType
    {
        get => (Constants.EntryInputType)GetValue(InputTypeProperty);
        set => SetValue(InputTypeProperty, value);
    }

    private void InputEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        var text = e.NewTextValue ?? string.Empty;
        bool isValid = true;
        string error = string.Empty;

        switch (InputType)
        {
            case Constants.EntryInputType.Numeric:
                if (!Regex.IsMatch(text, @"^\d*$"))
                {
                    isValid = false;
                    error = "Sólo números";
                }
                break;
            case Constants.EntryInputType.Email:
                if (!Regex.IsMatch(text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                {
                    isValid = false;
                    error = "Correo inválido";
                }
                break;
            case Constants.EntryInputType.Alphabetic:
                if (!Regex.IsMatch(text ?? string.Empty, @"^[a-zA-Z ]*$"))
                {
                    isValid = false;
                    error = "Sólo letras y espacios";
                }
                break;
            case Constants.EntryInputType.Alphanumeric:
                if (!Regex.IsMatch(text, @"^[a-zA-Z0-9]*$"))
                {
                    isValid = false;
                    error = "Sólo letras y números";
                }
                break;
        }

        if (isValid && IsRequired)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                isValid = false;
                error = "Este campo es obligatorio";
            }
        }
        
        if (MinLength > 0 && text.Length < MinLength)
        {
            isValid = false;
            error = $"Debe tener al menos {MinLength} caracteres";
        }

        if (!isValid)
        {
            ErrorLabel.Text = error ?? "Inalid input";
            ErrorLabel.IsVisible = true;
            MainBorder.Stroke = Colors.Red;
        }
        else
        {
            ErrorLabel.IsVisible = false;
            MainBorder.Stroke = Color.FromArgb("#D1D5DB");
        }
        
        IsValid = isValid;
    }
    
    public bool Validate()
    {
        InputEntry_TextChanged(InputEntry, new TextChangedEventArgs(Text, Text));
        return IsValid;
    }
    
    void OnClearClicked(object sender, EventArgs e)
    {
        Text = string.Empty;
    }
}