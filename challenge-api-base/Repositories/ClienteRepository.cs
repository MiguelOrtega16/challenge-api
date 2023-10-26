using challenge_api_base.Data;
using challenge_api_base.Models;
using challenge_api_base.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace challenge_api_base.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly AppDbContext _context;

        public ClienteRepository(AppDbContext context)
        {
            _context = context;
        }

        #region CRUD operaciones

        public async Task<bool> AddClienteAsync(Cliente cliente)
        {
            await _context.Clientes.AddAsync(cliente);
            await _context.InformacionContactos.AddAsync(cliente.InfoContacto);
            foreach (Sucursal sucursal in cliente.Sucursales)
            {
                sucursal.ClienteId = cliente.Identificador;
                _context.InformacionContactoSucursales.Add(sucursal.InfoContactoSucursal);
                _context.Sucursales.Add(sucursal);
            }

            // TO-DO: Se requiere regresar algun identificador? validar
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateClienteAsync(Cliente cliente)
        {
            _context.Clientes.Update(cliente);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteClienteAsync(string identificador)
        {
            Cliente? cliente = await GetClienteByIdAsync(identificador);
            if (cliente == null)
            {
                return false;
            }

            _context.Clientes.Remove(cliente);
            return await _context.SaveChangesAsync() > 0;
        }

        #endregion

        #region GET operaciones

        public async Task<List<Cliente>> GetClientesByCiudad(string ciudad)
        {
            return await _context.Clientes
                                 .Include(c => c.InfoContacto)
                                 .Where(c => c.InfoContacto.Ciudad == ciudad)
                                 .ToListAsync();
        }

        public async Task<Cliente> GetClienteByIdAsync(string id)
        {
            return await _context.Clientes.Include(c => c.Sucursales).FirstOrDefaultAsync(c => c.Identificador == id);
        }

        public async Task<List<Cliente>> GetAllClientesAsync()
        {
            return await _context.Clientes.Include(c => c.Sucursales).ToListAsync();
        }

        public async Task<List<Cliente>> GetClientesByCodigoVendedorAsync(string codigoVendedor)
        {
            return await _context.Clientes
                                 .Include(c => c.Sucursales)
                                 .Where(c => c.Sucursales.Any(s => s.CodigoVendedor == codigoVendedor))
                                 .ToListAsync();
        }

        #endregion

        #region Metodos utilitarios

        public async Task<bool> ExisteIdentificadorAsync(string identificador, int? clienteId = null)
        {
            if (clienteId.HasValue)
            {
                return await _context.Clientes.AnyAsync(c => c.Identificador == identificador && c.Id != clienteId.Value);
            }

            return await _context.Clientes.AnyAsync(c => c.Identificador == identificador);
        }

        public async Task<bool> ExisteTelefonoAsync(string telefonoFijo, string telefonoCelular, int? clienteId = null)
        {
            if (clienteId.HasValue)
            {
                return await _context.Clientes.AnyAsync(c =>
                           (c.InfoContacto.TelefonoFijo == telefonoFijo || c.InfoContacto.TelefonoCelular == telefonoCelular)
                           && c.Id != clienteId.Value);
            }

            return await _context.Clientes.AnyAsync(c =>
                       c.InfoContacto.TelefonoFijo == telefonoFijo || c.InfoContacto.TelefonoCelular == telefonoCelular);
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

        #endregion
    }
}