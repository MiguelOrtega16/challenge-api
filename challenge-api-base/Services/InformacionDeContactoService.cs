using challenge_api_base.Models;

public class InformacionDeContactoService : IInformacionDeContactoService
{
    public bool TieneTelefonoValido(InformacionContacto infoContacto)
    {
        if (string.IsNullOrWhiteSpace(infoContacto.TelefonoFijo) && string.IsNullOrWhiteSpace(infoContacto.TelefonoCelular))
        {
            return false; // Ninguno de los dos números fue proporcionado.
        }

        return true;
    }
}