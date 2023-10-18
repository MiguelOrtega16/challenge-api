using System.ComponentModel.DataAnnotations;
using NSwag.Annotations;

namespace challenge_api_base.Models
{
    public class Sucursal
    {
        [SwaggerIgnore] public int Id { get; set; }
        [Required]
        [StringLength(5, MinimumLength = 3, ErrorMessage = "El código de la sucursal debe tener entre 3 y 5 dígitos.")]
        public string CodigoSucursal { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "El nombre de la sucursal debe tener entre 5 y 100 caracteres.")]
        public string NombreSucursal { get; set; }
        [Required]
        [StringLength(5, MinimumLength = 3, ErrorMessage = "El código de vendedor debe tener entre 3 y 5 dígitos.")]
        public string CodigoVendedor { get; set; }
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "El cupo de crédito debe ser un valor positivo y diferente de cero.")]
        public decimal CupoCredito { get; set; } // Debe ser diferente de cero y no negativo.
        public string ClienteId { get; set; }

        public InformacionContactoSucursal InfoContactoSucursal { get; set; }
    }
}