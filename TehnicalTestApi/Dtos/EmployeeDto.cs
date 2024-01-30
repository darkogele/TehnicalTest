using System.Text.Json.Serialization;

namespace TechnicalTestApi.Dtos;

public class EmployeeDto
{
    public int? Id { get; set; }

    [JsonPropertyName("date_of_birth")]
    public DateTime? DateOfBirth { get; set; }

    public string? Image { get; set; }

    public string? Email { get; set; }

    [JsonPropertyName("first_name")]
    public string? FirstName { get; set; }

    [JsonPropertyName("last_name")]
    public string? LastName { get; set; }

    public string? Title { get; set; }

    public string? Address { get; set; }

    public string? Country { get; set; }

    public string? Bio { get; set; }

    public double? Rating { get; set; }
}