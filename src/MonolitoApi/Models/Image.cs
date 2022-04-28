using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MonolitoApi.Models
{
    public class Image
    {
        [NotMapped]
        public int Id { get; set; }
        public string Name { get; set; }
        [NotMapped]
        [Required(ErrorMessage = "Es Obligatorio el cargue de un archivo de imagen")]
        public string fileImageBase64 { get; set; }
        [JsonIgnore]
        public string? UUID { get; set; }
        [Required(ErrorMessage = "Dato Obligatorio Id Relacionado con Persona")]
        public int PersonId { get; set; }
        [JsonIgnore]
        public virtual Person? Person { get; set; }
    }
}