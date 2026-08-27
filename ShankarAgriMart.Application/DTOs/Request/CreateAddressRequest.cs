using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShankarAgriMart.Application.DTOs.Request;

public class CreateAddressRequest
{
    public string FullName { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string AddressLine1 { get; set; } = string.Empty;

    public string? AddressLine2 { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public string? Pincode { get; set; }

    public bool IsDefault { get; set; }
}