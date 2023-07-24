using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Sieve.Attributes;

namespace AreaAccountData.Models;

[Index(nameof(AccountNumber), IsUnique = true)]
[Index(nameof(ActiveFrom), nameof(ActiveUntil))]
[Index(nameof(Address))]
[Index(nameof(Area), IsDescending = new[] { true })]
public class AreaAccount
{
    [Key]
    [Required]
    [Sieve(CanFilter = true, CanSort = true)]
    public Guid Id { get; set; }

    [Sieve(CanFilter = true, CanSort = true)]
    [Required]
    public string AccountNumber { get; set; }

    [Sieve(CanFilter = true, CanSort = true)]
    [Required]
    public DateTime ActiveFrom { get; set; }

    [Sieve(CanFilter = true, CanSort = true)]
    public DateTime ActiveUntil { get; set; }

    [Sieve(CanFilter = true, CanSort = true)]
    [Required]
    public string Address { get; set; }

    [Sieve(CanFilter = true, CanSort = true)]
    [Required]
    public double Area { get; set; }

    [Sieve(CanFilter = true, CanSort = true)]
    public List<Person> Tenants { get; set; }
}