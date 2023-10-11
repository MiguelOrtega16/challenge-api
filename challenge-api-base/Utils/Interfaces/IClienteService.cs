using challenge_api_base.Models;

public interface IClienteService
{
    Task<bool> ExisteIdentificadorAsync(string identificador, int? clienteId = null);
    Task<bool> ExisteTelefonoAsync(string telefonoFijo, string telefonoCelular, int? clienteId = null);
    bool TieneMinimoUnaSucursal(Cliente cliente);
    bool CodigoSucursalRepetido(Cliente cliente);
    bool DatosValidosParaTipo(Cliente cliente);
    Task<bool> ExisteTelefonoEnSucursalAsync(string telefonoFijo, string telefonoCelular, int? clienteId = null, int? sucursalId = null);
}