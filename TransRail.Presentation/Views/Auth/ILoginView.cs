namespace TransRail.Presentation.Views;

public interface ILoginView
{
    string Correo { get; }
    string Contrasena { get; }
    event EventHandler? LoginRequested;

    void ShowMessage(string message);
    void OpenAdminMenu();
    void OpenEmployeeMenu();
    void OpenPassengerMenu();
}
