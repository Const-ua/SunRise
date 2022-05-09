using System.ComponentModel.DataAnnotations;


namespace Model
{
    public class City
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage="Это обязательное поле")]
        public string Name { get; set; }
        
        [Required(ErrorMessage="Это обязательное поле")]
        [Range(22.08, 40.13, ErrorMessage="Долгота должна быть между 22,08 и 40,13")]
        public string Latitude { get; set; } 
        [Required(ErrorMessage="Это обязательное поле")]
        public string Longitude { get; set; } 
    }
}
