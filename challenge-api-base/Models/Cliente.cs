using System.ComponentModel.DataAnnotations;
using NSwag.Annotations;

namespace challenge_api_base.Models
{
    // Enumeración para el tipo de cliente
    public enum TipoCliente
    {
        PersonaNatural,
        PersonaJuridica
    }

    public class Cliente
    {
        [SwaggerIgnore] public int Id { get; set; }
        [Required] public TipoCliente TipoCliente { get; set; }
        public string NombresYApellidos { get; set; } // Obligatorio si es PersonaNatural
        public string RazonSocial { get; set; } // Obligatorio si es PersonaJuridica
        [Required(ErrorMessage = "El Nit o Cédula es requerido.")]
        public string Identificador { get; set; } // Obligatorio

        public InformacionContacto InfoContacto { get; set; }
        [Required] public List<Sucursal> Sucursales { get; set; } = new();
    }
}