using challenge_api_base.Models;

namespace challenge_api_base.Repositories.Interfaces
{
    public interface ISucursalRepository
    {
        Task<Sucursal> GetSucursalByIdAsync(string clienteId, int sucursalId);
        Task AddSucursalAsync(string clienteId, Sucursal sucursal);
        Task UpdateSucursalAsync(string clienteId, Sucursal sucursal);
        Task DeleteSucursalAsync(string clienteId, int sucursalId);
        Task<bool> ExisteTelefonoEnSucursalAsync(string telefonoFijo, string telefonoCelular, int? clienteId = null, int? sucursalId = null);
        Task<bool> CodigoSucursalYaExiste(string clienteId, string codigoSucursal);
    }
}