using challenge_api_base.Data;
using challenge_api_base.Models;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class ClientesController : ControllerBase
{
    private readonly IClienteService _clienteService;
    private readonly AppDbContext _context;
    private readonly IInformacionDeContactoService _infoContactoService;

    public ClientesController(AppDbContext context, IClienteService clienteService, IInformacionDeContactoService infoContactoService)
    {
        _context = context;
        _clienteService = clienteService;
        _infoContactoService = infoContactoService;
    }

    [HttpPost]
    public async Task<ActionResult<Cliente>> CrearCliente([FromBody] Cliente cliente)
    {
        if (!_clienteService.DatosValidosParaTipo(cliente))
        {
            return BadRequest("Los datos proporcionados no son consistentes con el tipo de cliente.");
        }

        if (await _clienteService.ExisteIdentificadorAsync(cliente.Identificador))
        {
            return BadRequest("El Identificador ya está en uso.");
        }

        if (await _clienteService.ExisteTelefonoAsync(cliente.InfoContacto.TelefonoFijo, cliente.InfoContacto.TelefonoCelular))
        {
            return BadRequest("El número de teléfono fijo o celular ya está en uso.");
        }

        if (!_clienteService.TieneMinimoUnaSucursal(cliente))
        {
            return BadRequest("El cliente debe tener al menos una sucursal.");
        }

        if (!_infoContactoService.TieneTelefonoValido(cliente.InfoContacto))
        {
            return BadRequest("Debe proporcionar al menos un número de teléfono (fijo o celular).");
        }

        foreach (var sucursal in cliente.Sucursales)
        {
            if (await _clienteService.ExisteTelefonoEnSucursalAsync(sucursal.InfoContactoSucursal.TelefonoFijo,
                    sucursal.InfoContactoSucursal.TelefonoCelular, cliente.Id, sucursal.Id))
            {
                return BadRequest($"El teléfono fijo o celular de la sucursal {sucursal.CodigoSucursal} ya está en uso.");
            }
        }

        if (_clienteService.CodigoSucursalRepetido(cliente))
        {
            return BadRequest("El código de la sucursal debe ser único para el mismo cliente.");
        }

        // Guardar el cliente
        _context.Clientes.Add(cliente);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(CrearCliente), new { id = cliente.Id }, cliente);
    }
}