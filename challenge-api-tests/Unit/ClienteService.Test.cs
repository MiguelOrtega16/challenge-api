using challenge_api_base.Models;
using challenge_api_base.Repositories.Interfaces;
using Moq;
using Xunit;

public class ClienteServiceTests
{
    private readonly ClienteService _clienteService;
    private readonly Mock<IClienteRepository> _mockClienteRepository;

    public ClienteServiceTests()
    {
        _mockClienteRepository = new Mock<IClienteRepository>();
        _clienteService = new ClienteService(_mockClienteRepository.Object);
    }

    [Fact]
    public async Task AddClienteAsync_ShouldReturnTrue_WhenRepositoryReturnsTrue()
    {
        // Arrange
        var cliente = new Cliente
        {
            Identificador = "123456",
            TipoCliente = TipoCliente.PersonaNatural,
            NombresYApellidos = "Miguel Ortega",
            Sucursales = new List<Sucursal>()
        };

        _mockClienteRepository.Setup(repo => repo.AddClienteAsync(It.IsAny<Cliente>())).ReturnsAsync(true);

        // Act
        var result = await _clienteService.AddClienteAsync(cliente);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void TieneMinimoUnaSucursal_ShouldReturnTrue_WhenClienteHasOneOrMoreSucursales()
    {
        // Arrange
        var cliente = new Cliente
        {
            Identificador = "123456",
            Sucursales = new List<Sucursal>
            {
                new()
                {
                    ClienteId = "123456",
                    CodigoSucursal = "ABCD123",
                    CupoCredito = 0,
                    NombreSucursal = "TEST-EXAMPLE"
                }
            }
        };

        // Act
        var result = _clienteService.TieneMinimoUnaSucursal(cliente);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void TieneMinimoUnaSucursal_ShouldReturnFalse_WhenClienteHaLessThanOneSucursal()
    {
        // Arrange
        var cliente = new Cliente
        {
            Identificador = "123456"
        };

        // Act
        var result = _clienteService.TieneMinimoUnaSucursal(cliente);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CodigoSucursalRepetido_ReturnsTrue_WhenCodigoIsRepeated()
    {
        // Arrange
        var cliente = new Cliente
        {
            Sucursales = new List<Sucursal>
            {
                new() { CodigoSucursal = "TEST-CODIGO-SUCURSAL-123" },
                new() { CodigoSucursal = "TEST-CODIGO-SUCURSAL-123" }
            }
        };

        // Act
        var result = _clienteService.CodigoSucursalRepetido(cliente);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void DatosValidosParaTipo_ReturnsTrue_WhenDatosAreValidForTipoCliente()
    {
        // Arrange
        var cliente = new Cliente
        {
            TipoCliente = TipoCliente.PersonaNatural,
            NombresYApellidos = "TEST-NOMBRES-PERSONAL-NATURAL"
        };

        // Act
        var result = _clienteService.DatosValidosParaTipo(cliente);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void DatosValidosParaTipo_ReturnsFalse_WhenDatosAreNotValidForTipoCliente()
    {
        // Arrange
        var cliente = new Cliente
        {
            TipoCliente = TipoCliente.PersonaNatural,
            NombresYApellidos = string.Empty
        };

        // Act
        var result = _clienteService.DatosValidosParaTipo(cliente);

        // Assert
        Assert.False(result);
    }

    // ... (más pruebas para otros métodos)
}