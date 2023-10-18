using System.ComponentModel.DataAnnotations;
using NSwag.Annotations;

namespace challenge_api_base.Models
{
    public class InformacionContacto
    {
        [SwaggerIgnore] public int Id { get; set; }
        [Required] public string Direccion { get; set; }
        [Required] public string Ciudad { get; set; }
        [Required] public string Pais { get; set; }
        [StringLengthIfNotEmpty(7, 12, ErrorMessage = "El teléfono fijo debe tener entre 7 y 12 dígitos.")]
        public string TelefonoFijo { get; set; }

        [StringLengthIfNotEmpty(10, 15, ErrorMessage = "El teléfono celular debe tener entre 10 y 15 dígitos.")]
        public string TelefonoCelular { get; set; }
        [EmailAddress(ErrorMessage = "El correo electrónico no es válido.")]
        public string CorreoElectronico { get; set; }
    }
}