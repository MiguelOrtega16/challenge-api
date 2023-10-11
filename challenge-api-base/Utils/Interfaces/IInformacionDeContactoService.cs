using challenge_api_base.Models;

public interface IInformacionDeContactoService
{
    bool TieneTelefonoValido(InformacionContacto infoContacto);
}