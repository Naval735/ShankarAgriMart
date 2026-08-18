using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShankarAgriMart.Application.DTOs.Request;

public class CreateInventoryTransactionRequest
{
    public string TransactionType { get; set; } = string.Empty;

    public int Quantity { get; set; }

    public string? Remarks { get; set; }
}