using challenge_api_base.Models;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class ClientesController : ControllerBase
{
    private readonly IClienteService _clienteService;
    private readonly IInformacionDeContactoService _infoContactoService;

    public ClientesController(IClienteService clienteService, IInformacionDeContactoService infoContactoService)
    {
        _clienteService = clienteService;
        _infoContactoService = infoContactoService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Cliente>>> GetAllClientes()
    {
        return Ok(await _clienteService.GetAllClientesAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Cliente>> GetCliente(string id)
    {
        Cliente cliente = await _clienteService.GetClienteByIdAsync(id);
        if (cliente == null)
        {
            return NotFound();
        }

        return Ok(cliente);
    }

    [HttpGet("by-ciudad/{ciudad}")]
    public async Task<ActionResult<IEnumerable<Cliente>>> GetClientesByCiudad(string ciudad)
    {
        IEnumerable<Cliente> clientes = await _clienteService.GetClientesByCiudad(ciudad);
        return Ok(clientes);
    }

    [HttpGet("by-vendedor/{codigoVendedor}")]
    public async Task<ActionResult<List<Cliente>>> GetClientesByCodigoVendedor(string codigoVendedor)
    {
        List<Cliente>? clientes = await _clienteService.GetClientesByCodigoVendedorAsync(codigoVendedor);
        if (clientes == null || !clientes.Any())
        {
            return NotFound();
        }

        return Ok(clientes);
    }

    [HttpPost]
    public async Task<ActionResult<Cliente>> CrearCliente([FromBody] Cliente cliente)
    {
        ActionResult validationResult = await ValidarInformacionCliente(cliente, false);
        if (validationResult != null)
        {
            return validationResult;
        }

        foreach (Sucursal sucursal in cliente.Sucursales)
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

        await _clienteService.AddClienteAsync(cliente);

        return CreatedAtAction(nameof(CrearCliente), new { id = cliente.Id }, cliente);
    }

    [HttpPut("{identificador}")]
    public async Task<ActionResult> UpdateCliente(string identificador, Cliente cliente)
    {
        if (identificador != cliente.Identificador)
        {
            return BadRequest("El Nit/Cedula proporcionado no coincide con el ID del cliente en el cuerpo de la solicitud.");
        }

        ActionResult validationResult = await ValidarInformacionCliente(cliente, true);
        if (validationResult != null)
        {
            return validationResult;
        }

        bool result = await _clienteService.UpdateClienteAsync(cliente);

        if (!result)
        {
            return StatusCode(500, "Error interno al actualizar el cliente.");
        }

        return NoContent();
    }

    [HttpDelete("{identificador}")]
    public async Task<ActionResult> DeleteCliente(string identificador)
    {
        bool result = await _clienteService.DeleteClienteAsync(identificador);

        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }

    #region Métodos Privados

    private async Task<ActionResult> ValidarInformacionCliente(Cliente cliente, bool esActualizacion)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

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

        if (!esActualizacion && !_clienteService.TieneMinimoUnaSucursal(cliente))
        {
            return BadRequest("El cliente debe tener al menos una sucursal.");
        }

        if (!_infoContactoService.TieneTelefonoValido(cliente.InfoContacto))
        {
            return BadRequest("Debe proporcionar al menos un número de teléfono (fijo o celular).");
        }

        return null;
    }

    #endregion
}