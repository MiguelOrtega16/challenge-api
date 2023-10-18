using challenge_api_base.Models;

public interface IClienteService
{
    #region CRUD operaciones

    Task<bool> AddClienteAsync(Cliente cliente);
    Task<bool> UpdateClienteAsync(Cliente cliente);
    Task<bool> DeleteClienteAsync(string identificador);

    #endregion

    #region GET operaciones

    Task<List<Cliente>> GetAllClientesAsync();
    Task<Cliente> GetClienteByIdAsync(string id);
    Task<List<Cliente>> GetClientesByCiudad(string ciudad);
    Task<List<Cliente>> GetClientesByCodigoVendedorAsync(string codigoVendedor);

    #endregion

    #region Metodos utilitarios

    Task<bool> ExisteIdentificadorAsync(string identificador);
    Task<bool> ExisteTelefonoAsync(string telefonoFijo, string telefonoCelular, int? clienteId = null);
    Task<bool> ExisteTelefonoEnSucursalAsync(string telefonoFijo, string telefonoCelular, int? clienteId = null, int? sucursalId = null);
    bool TieneMinimoUnaSucursal(Cliente cliente);
    bool CodigoSucursalRepetido(Cliente cliente);
    bool DatosValidosParaTipo(Cliente cliente);

    #endregion
}