using System.ComponentModel.DataAnnotations;

namespace SunRise.Models
{
    public class CityDtoModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage="Это обязательное поле")]
        public string Name { get; set; }
        
        [Required(ErrorMessage="Это обязательное поле")]
        public string Latitude { get; set; } 
        [Required(ErrorMessage="Это обязательное поле")]
        public string Longitude { get; set; } 
    }
}
