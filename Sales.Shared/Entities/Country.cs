﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sales.Shared.Entities
{
    public class Country
    {
        public int Id { get; set; }

        [Display(Name= "País")]
        [Required(ErrorMessage ="El cambo {0} es necesario.")]
        [MaxLength(100, ErrorMessage ="El campo {0} no puede tener más de {1} caractéres")]
        public string Name { get; set; } = null!;

        public ICollection<State>? States { get; set; }
   
        public int StatesNumber => States == null ? 0 : States.Count;

    }
}
