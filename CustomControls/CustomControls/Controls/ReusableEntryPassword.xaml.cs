﻿using System.Text.RegularExpressions;
using CustomControls.Helpers;

namespace CustomControls.Controls;

public partial class ReusableEntryPassword : ContentView
{
    bool _isPassword = true;
    
    public ReusableEntryPassword()
    {
        InitializeComponent();
        BindingContext = this;
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
    
    public static readonly BindableProperty IsRequiredProperty =
        BindableProperty.Create(nameof(IsRequired), typeof(bool), typeof(ReusablePicker), false);

    public bool IsRequired
    {
        get => (bool)GetValue(IsRequiredProperty);
        set => SetValue(IsRequiredProperty, value);
    }
   
    private void InputEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        var text = e.NewTextValue ?? string.Empty;
        bool isValid = true;
        string error = string.Empty;

        if (IsRequired && isValid)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                isValid = false;
                error = "Este campo es obligatorio";
            }
        }

        if (!isValid)
        {
            ErrorLabelPassword.Text = error ?? "Inalid input";
            ErrorLabelPassword.IsVisible = true;
            MainBorderPassword.Stroke = Colors.Red;
        }
        else
        {
            ErrorLabelPassword.IsVisible = false;
            MainBorderPassword.Stroke = Color.FromArgb("#D1D5DB");
        }
    }

    private void OnViewChanged(object? sender, EventArgs e)
    {
        _isPassword = !_isPassword;

        InputEntryPassword.IsPassword = _isPassword;

        ViewToggleButton.Text = _isPassword
            ? FaSolidIcons.EyeSlash
            : FaSolidIcons.Eye;
    }
}