using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShankarAgriMart.Application.DTOs.Response;

public class InventoryTransactionResponse
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public string ProductName { get; set; } = string.Empty;

    public string TransactionType { get; set; } = string.Empty;

    public int Quantity { get; set; }

    public string? Remarks { get; set; }

    public DateTime CreatedAt { get; set; }
}
