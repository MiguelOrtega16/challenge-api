using challenge_api_base.Models;
using challenge_api_base.Utils.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/clientes/{clienteId}/sucursales")]
public class SucursalesController : ControllerBase
{
    private readonly IClienteService _clienteService;
    private readonly IInformacionDeContactoService _infoContactoService;
    private readonly ISucursalService _service;

    public SucursalesController(ISucursalService service, IClienteService clienteService, IInformacionDeContactoService infoContactoService)
    {
        _service = service;
        _clienteService = clienteService;
        _infoContactoService = infoContactoService;
    }

    [HttpGet("{sucursalId}")]
    public async Task<ActionResult<Sucursal>> GetSucursal(string clienteId, int sucursalId)
    {
        Sucursal? sucursal = await _service.GetSucursalAsync(clienteId, sucursalId);
        if (sucursal == null)
        {
            return NotFound();
        }

        return sucursal;
    }

    [HttpPost]
    public async Task<ActionResult> AddSucursal(string clienteId, [FromBody] Sucursal sucursal)
    {
        ActionResult validationResult = await ValidarInformacionSucursal(clienteId, sucursal, false);
        if (validationResult != null)
        {
            return validationResult;
        }

        await _service.AddSucursalAsync(clienteId, sucursal);
        return CreatedAtAction(nameof(GetSucursal), new { clienteId, sucursalId = sucursal.Id }, sucursal);
    }

    [HttpPut("{sucursalId}")]
    public async Task<ActionResult> UpdateSucursal(string clienteId, int sucursalId, [FromBody] Sucursal sucursal)
    {
        if (sucursalId != sucursal.Id)
        {
            return BadRequest("El ID proporcionado no coincide con el ID de la sucursal del cuerpo de la solicitud.");
        }

        ActionResult validationResult = await ValidarInformacionSucursal(clienteId, sucursal, true);
        if (validationResult != null)
        {
            return validationResult;
        }

        await _service.UpdateSucursalAsync(clienteId, sucursal);
        return NoContent();
    }

    [HttpDelete("{sucursalId}")]
    public async Task<ActionResult> DeleteSucursal(string clienteId, int sucursalId)
    {
        await _service.DeleteSucursalAsync(clienteId, sucursalId);
        return NoContent();
    }

    #region Métodos Privados

    private async Task<ActionResult> ValidarInformacionSucursal(string clienteId, Sucursal sucursal, bool esActualizacion)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        Cliente cliente = await _clienteService.GetClienteByIdAsync(clienteId);

        if (!esActualizacion && !_clienteService.TieneMinimoUnaSucursal(cliente))
        {
            return BadRequest("El cliente debe tener al menos una sucursal.");
        }

        if (!await _service.ValidarCodigoUnicoParaCliente(clienteId, sucursal.CodigoSucursal))
        {
            return BadRequest("El código de la sucursal debe ser único para el mismo cliente.");
        }

        if (await _service.ExisteTelefonoEnSucursalAsync(sucursal.InfoContactoSucursal.TelefonoFijo,
                sucursal.InfoContactoSucursal.TelefonoCelular, cliente.Id, sucursal.Id))
        {
            return BadRequest($"El teléfono fijo o celular de la sucursal {sucursal.CodigoSucursal} ya está en uso.");
        }

        return null;
    }

    #endregion
}