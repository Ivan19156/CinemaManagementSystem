using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.DTOs.FilmDTOs;

public class UpdateFilmDto
{
    public int Id { get; set; }  // Або без Id, якщо передається окремо як параметр
    public string Title { get; set; } = null!;
    public string Genre { get; set; } = null!;
    public string Director { get; set; } = null!;
    public DateTime ReleaseDate { get; set; }
    public string Description { get; set; } = null!;
}
