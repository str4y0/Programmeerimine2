using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Data
{
    [Index(nameof(Name), IsUnique = true)]
    public class Tournament
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }
    }
}
