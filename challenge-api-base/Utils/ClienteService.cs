using challenge_api_base.Data;
using challenge_api_base.Models;
using Microsoft.EntityFrameworkCore;

public class ClienteService : IClienteService
{
    private readonly AppDbContext _context;

    public ClienteService(AppDbContext context)
    {
        _context = context;
    }

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
                       (c.InfoContacto.TelefonoFijo == telefonoFijo || c.InfoContacto.TelefonoCelular == telefonoCelular) && c.Id != clienteId.Value);
        }

        return await _context.Clientes.AnyAsync(c =>
                   c.InfoContacto.TelefonoFijo == telefonoFijo || c.InfoContacto.TelefonoCelular == telefonoCelular);
    }

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

    public async Task<bool> ExisteTelefonoEnSucursalAsync(string telefonoFijo, string telefonoCelular, int? clienteId = null, int? sucursalId = null)
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
}