using challenge_api_base.Data;
using challenge_api_base.Models;
using challenge_api_base.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace challenge_api_base.Repositories
{
    public class SucursalRepository : ISucursalRepository
    {
        private readonly AppDbContext _context;

        public SucursalRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddSucursalAsync(string clienteId, Sucursal sucursal)
        {
            if (sucursal.ClienteId == string.Empty)
            {
                sucursal.ClienteId = clienteId;
            }

            _context.Sucursales.Add(sucursal);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateSucursalAsync(string clienteId, Sucursal sucursal)
        {
            var existingSucursal = await GetSucursalByIdAsync(clienteId, sucursal.Id);
            if (existingSucursal == null)
            {
                return;
            }

            _context.Entry(existingSucursal).CurrentValues.SetValues(sucursal);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSucursalAsync(string clienteId, int sucursalId)
        {
            var sucursal = await GetSucursalByIdAsync(clienteId, sucursalId);
            if (sucursal == null)
            {
                return;
            }

            _context.Sucursales.Remove(sucursal);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExisteTelefonoEnSucursalAsync(string telefonoFijo, string telefonoCelular, int? clienteId = null,
            int? sucursalId = null)
        {
            IQueryable<Sucursal> query = _context.Sucursales.Include(s => s.InfoContactoSucursal);

            // Si se está actualizando una sucursal, excluir esa sucursal en la validación.
            if (sucursalId.HasValue)
            {
                query = query.Where(s => s.Id != sucursalId.Value);
            }

            if (await query.AnyAsync(
                    s => s.InfoContactoSucursal.TelefonoFijo == telefonoFijo || s.InfoContactoSucursal.TelefonoCelular == telefonoCelular))
            {
                return true; // El número de teléfono ya está en uso en otra sucursal.
            }

            return false;
        }

        public async Task<Sucursal> GetSucursalByIdAsync(string clienteId, int sucursalId)
        {
            return await _context.Sucursales
                                 .FirstOrDefaultAsync(s => s.ClienteId == clienteId && s.Id == sucursalId);
        }

        public async Task<bool> CodigoSucursalYaExiste(string clienteId, string codigoSucursal)
        {
            return await _context.Sucursales
                                 .AnyAsync(s => s.ClienteId == clienteId && s.CodigoSucursal == codigoSucursal);
        }
    }
}