using System.Collections;

namespace CustomControls.Controls;

public partial class ReusablePicker : ContentView
{
    public ReusablePicker()
    {
        InitializeComponent();
        //BindingContext = this;
        this.BindingContextChanged += (s, e) =>
        {
            System.Diagnostics.Debug.WriteLine("🚀 BindingContextChanged disparado");
            TryApplyItemDisplayBinding();
        };
    }

    public Picker InnerPicker => MainPicker;
    
    public static readonly BindableProperty IsValidProperty =
        BindableProperty.Create(
            nameof(IsValid),
            typeof(bool),
            typeof(ReusablePicker),
            false,
            BindingMode.OneWayToSource,
            propertyChanged: OnIsValidChanged);

    public bool IsValid
    {
        get => (bool)GetValue(IsValidProperty);
        private set => SetValue(IsValidProperty, value);
    }

    public event EventHandler<bool> IsValidChanged;
    
    static void OnIsValidChanged(BindableObject bindable, object oldVal, object newVal)
    {
        var ctl = (ReusablePicker)bindable;
        ctl.IsValidChanged?.Invoke(ctl, (bool)newVal);
    }
    
    // Título (placeholder)
    public static readonly BindableProperty TitleProperty =
        BindableProperty.Create(nameof(Title), typeof(string), typeof(ReusablePicker), string.Empty);

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public static readonly BindableProperty ItemsSourceProperty =
        BindableProperty.Create(
            nameof(ItemsSource),
            typeof(IEnumerable),
            typeof(ReusablePicker),
            null);

    public IEnumerable ItemsSource
    {
        get => (IEnumerable)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }
    
    private static void OnItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is ReusablePicker control)
        {
            System.Diagnostics.Debug.WriteLine("🔄 ItemsSource cambiado");
            control.TryApplyItemDisplayBinding();
        }
    }
    
    private void TryApplyItemDisplayBinding()
    {
        
        System.Diagnostics.Debug.WriteLine("🎯 TryApplyItemDisplayBinding() ejecutado");
        System.Diagnostics.Debug.WriteLine("DisplayMemberPath: " + DisplayMemberPath);
        System.Diagnostics.Debug.WriteLine("ItemsSource.Count: " + ItemsSource?.OfType<object>().Count());
        
        if (string.IsNullOrWhiteSpace(DisplayMemberPath)) return;

        var firstItem = ItemsSource?.OfType<object>().FirstOrDefault();
        if (firstItem != null)
        {
            var prop = firstItem.GetType().GetProperty(DisplayMemberPath);
            if (prop != null)
            {
                MainPicker.ItemDisplayBinding = new Binding(DisplayMemberPath);
                System.Diagnostics.Debug.WriteLine("✅ Binding aplicado con éxito");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("❌ Propiedad no encontrada: " + DisplayMemberPath);
            }
        }
        else
        {
            System.Diagnostics.Debug.WriteLine("❌ ItemsSource vacío o nulo");
        }
    }
    

    // Elemento seleccionado
    public static readonly BindableProperty SelectedItemProperty =
        BindableProperty.Create(nameof(SelectedItem), typeof(object), typeof(ReusablePicker), null, BindingMode.TwoWay);

    public static readonly BindableProperty DisplayMemberPathProperty =
        BindableProperty.Create(nameof(DisplayMemberPath), typeof(string), typeof(ReusablePicker), default(string), propertyChanged: OnDisplayMemberPathChanged);

    public string DisplayMemberPath
    {
        get => (string)GetValue(DisplayMemberPathProperty);
        set => SetValue(DisplayMemberPathProperty, value);
    }

    private static void OnDisplayMemberPathChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is ReusablePicker control)
            control.TryApplyItemDisplayBinding();
    }
    
    public static readonly BindableProperty IsRequiredProperty =
        BindableProperty.Create(nameof(IsRequired), typeof(bool), typeof(ReusablePicker), false);

    public bool IsRequired
    {
        get => (bool)GetValue(IsRequiredProperty);
        set => SetValue(IsRequiredProperty, value);
    }
    
    
    public object? SelectedItem
    {
        get => GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }

    // Validación al cambiar
    private void MainPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (IsRequired && SelectedItem == null)
        {
            ErrorLabel.Text = "Debe seleccionar un valor";
            ErrorLabel.IsVisible = true;
            MainBorder.Stroke = Colors.Red;
        }
        else
        {
            ErrorLabel.IsVisible = false;
            MainBorder.Stroke = Color.FromArgb("#D1D5DB");
        }
        
        var valid = !ErrorLabel.IsVisible;
        IsValid = valid;
    }
    
    void OnClearClicked(object sender, EventArgs e)
    {
        SelectedItem = null;
        MainPicker.SelectedIndex = -1;
    }
}