using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AreaAccountApi.Validators;

namespace AreaAccountApi.DTOs;

public class CreateAreaAccountDto
{
    [Required] public Guid Id { get; set; }

    [Required]
    [RegularExpression(@"^\d+$")]
    public string AccountNumber { get; set; }

    [Required]
    [EarlierThan(nameof(ActiveUntil))]
    public DateTime ActiveFrom { get; set; }

    [Required] public DateTime ActiveUntil { get; set; }
    [Required] public string Address { get; set; }
    [Required] [Range(0, double.MaxValue)] public double Area { get; set; }
    public List<CreatePersonDTO> Tenants { get; set; }
}