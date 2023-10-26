using challenge_api_base.Models;
using challenge_api_base.Repositories.Interfaces;

public class ClienteService : IClienteService
{
    private readonly IClienteRepository _clienteRepository;

    public ClienteService(IClienteRepository clienteRepository)
    {
        _clienteRepository = clienteRepository;
    }

    #region CRUD operaciones

    public async Task<bool> AddClienteAsync(Cliente cliente)
    {
        return await _clienteRepository.AddClienteAsync(cliente);
    }

    public async Task<bool> UpdateClienteAsync(Cliente cliente)
    {
        return await _clienteRepository.UpdateClienteAsync(cliente);
    }

    public async Task<bool> DeleteClienteAsync(string identificador)
    {
        return await _clienteRepository.DeleteClienteAsync(identificador);
    }

    #endregion

    #region GET operaciones

    public async Task<List<Cliente>> GetAllClientesAsync()
    {
        return await _clienteRepository.GetAllClientesAsync();
    }

    public async Task<List<Cliente>> GetClientesByCiudad(string ciudad)
    {
        return await _clienteRepository.GetClientesByCiudad(ciudad);
    }

    public async Task<Cliente> GetClienteByIdAsync(string id)
    {
        return await _clienteRepository.GetClienteByIdAsync(id);
    }

    public async Task<List<Cliente>> GetClientesByCodigoVendedorAsync(string codigoVendedor)
    {
        return await _clienteRepository.GetClientesByCodigoVendedorAsync(codigoVendedor);
    }

    #endregion

    #region "Métodos Utilitarios/Compartidos

    public bool TieneMinimoUnaSucursal(Cliente cliente)
    {
        return cliente.Sucursales != null && cliente.Sucursales.Any();
    }

    public bool CodigoSucursalRepetido(Cliente cliente)
    {
        return cliente.Sucursales.GroupBy(s => s.CodigoSucursal).Any(g => g.Count() > 1);
    }

    public bool DatosValidosParaTipo(Cliente cliente)
    {
        if (cliente.TipoCliente == TipoCliente.PersonaNatural && string.IsNullOrWhiteSpace(cliente.NombresYApellidos))
        {
            return false;
        }

        if (cliente.TipoCliente == TipoCliente.PersonaJuridica && string.IsNullOrWhiteSpace(cliente.RazonSocial))
        {
            return false;
        }

        return true;
    }

    public async Task<bool> ExisteIdentificadorAsync(string identificador)
    {
        return await _clienteRepository.ExisteIdentificadorAsync(identificador);
    }

    public async Task<bool> ExisteTelefonoAsync(string telefonoFijo, string telefonoCelular, int? clienteId = null)
    {
        return await _clienteRepository.ExisteTelefonoAsync(telefonoFijo, telefonoCelular, clienteId);
    }

    public async Task<bool> ExisteTelefonoEnSucursalAsync(string telefonoFijo, string telefonoCelular, int? clienteId = null, int? sucursalId = null)
    {
        return await _clienteRepository.ExisteTelefonoEnSucursalAsync(telefonoFijo, telefonoCelular, clienteId, sucursalId);
    }

    #endregion
}