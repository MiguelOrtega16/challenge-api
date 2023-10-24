using challenge_api_base.Models;
using challenge_api_base.Repositories.Interfaces;
using challenge_api_base.Utils.Interfaces;

namespace challenge_api_base.Utils
{
    public class SucursalService : ISucursalService
    {
        private readonly ISucursalRepository _repository;

        public SucursalService(ISucursalRepository repository)
        {
            _repository = repository;
        }

        public Task<Sucursal> GetSucursalAsync(string clienteId, int sucursalId)
        {
            return _repository.GetSucursalByIdAsync(clienteId, sucursalId);
        }

        public Task AddSucursalAsync(string clienteId, Sucursal sucursal)
        {
            return _repository.AddSucursalAsync(clienteId, sucursal);
        }

        public Task UpdateSucursalAsync(string clienteId, Sucursal sucursal)
        {
            return _repository.UpdateSucursalAsync(clienteId, sucursal);
        }

        public Task DeleteSucursalAsync(string clienteId, int sucursalId)
        {
            return _repository.DeleteSucursalAsync(clienteId, sucursalId);
        }

        public Task<bool> ExisteTelefonoEnSucursalAsync(string telefonoFijo, string telefonoCelular, int? clienteId = null, int? sucursalId = null)
        {
            return _repository.ExisteTelefonoEnSucursalAsync(telefonoFijo, telefonoCelular, clienteId, sucursalId);
        }

        public async Task<bool> ValidarCodigoUnicoParaCliente(string clienteId, string codigoSucursal)
        {
            return !await _repository.CodigoSucursalYaExiste(clienteId, codigoSucursal);
        }
    }
}