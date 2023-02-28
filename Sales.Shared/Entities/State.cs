
using System.ComponentModel.DataAnnotations;



namespace Sales.Shared.Entities
{
    public class State
    {
        public int Id { get; set; }

        [Display(Name = "Estado/Departamento")]
        [Required(ErrorMessage = "El cambo {0} es necesario.")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener más de {1} caractéres")]
        public string Name { get; set; } = null!;
        public Country? Country { get; set; }
        public ICollection<City>? Cities { get; set; }
        public int CountryId { get; set; }

        public int CitiesNumber => Cities == null ? 0 : Cities.Count;

    }
}
