using challenge_api_base.Models;

namespace challenge_api_base.Utils.Interfaces
{
    public interface ISucursalService
    {
        Task<Sucursal> GetSucursalAsync(string clienteId, int sucursalId);
        Task AddSucursalAsync(string clienteId, Sucursal sucursal);
        Task UpdateSucursalAsync(string clienteId, Sucursal sucursal);
        Task DeleteSucursalAsync(string clienteId, int sucursalId);
        Task<bool> ExisteTelefonoEnSucursalAsync(string telefonoFijo, string telefonoCelular, int? clienteId = null, int? sucursalId = null);
        Task<bool> ValidarCodigoUnicoParaCliente(string clienteId, string codigoSucursal);
    }
}