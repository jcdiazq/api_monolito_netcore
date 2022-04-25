using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MonolitoApi.Models
{
    public class Person
    {
        [NotMapped]
        public int Id { get; set; }
        public string FirtName { get; set; }
        public string LastName { get; set; }
        [JsonIgnore]
        public virtual Image? Image { get; set; }
    }
}