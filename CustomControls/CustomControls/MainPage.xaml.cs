using System.Collections.ObjectModel;

namespace CustomControls;

public partial class MainPage : ContentPage
{
    private bool _isValidForm = false;
    public string Contraseña { get; set; }
    public int  Numero { get; set; }    
    
    public class Opcion
    {
        public string Nombre { get; set; }
    }
    
    public ObservableCollection<Opcion> ListaDeOpciones { get; set; }

    public Opcion? OpcionSeleccionada { get; set; }

    public MainPage()
    {
        InitializeComponent();
        
        ListaDeOpciones = new ObservableCollection<Opcion>
        {
            new Opcion { Nombre = "Opción 1" },
            new Opcion { Nombre = "Opción 2" },
            new Opcion { Nombre = "Opción 3" }
        };
        
        
        //EntryNombre   .IsValidChanged += (s, ok) => UpdateEnviarState();
        //EntryCorreo   .IsValidChanged += (s, ok) => UpdateEnviarState();
        //EntryCelular  .IsValidChanged += (s, ok) => UpdateEnviarState();
        

        // (Opcional) fuerza comprobación inicial
        //UpdateEnviarState();
        
        
        BindingContext = this;
    }
    
    
    void UpdateEnviarState()
    {
        _isValidForm = EntryNombre.IsValid &&
                      EntryCorreo.IsValid &&
                      EntryCelular.IsValid &&
                      MiPicker.IsValid;
        //BtnInscribir.IsEnabled = _isValidForm;
    }

    private bool FormValidate()
    {
        var v1 = EntryCorreo.Validate();
        var v2 = EntryNombre.Validate();
        return (v1 && v2);
    }

    private async void BtnInscribir_OnClicked(object? sender, EventArgs e)
    {
        if (!FormValidate())
            return;
            
        await DisplayAlert(
            "¡Atención!",          
            "Este es un mensaje.",
            "OK"                   
        );
    }
}