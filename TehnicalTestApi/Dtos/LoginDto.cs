using System.ComponentModel.DataAnnotations;

namespace TechnicalTestApi.Dtos;

public record LoginDto(
    [Required(ErrorMessage = "Please enter Email")]
    string UserName,

    [Required(ErrorMessage = "Please enter Password")]
    [StringLength(30, MinimumLength = 6)]
    string Password);