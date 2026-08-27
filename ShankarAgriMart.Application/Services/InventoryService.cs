using ShankarAgriMart.Application.Common.Exceptions;
using ShankarAgriMart.Application.DTOs.Request;
using ShankarAgriMart.Application.DTOs.Response;
using ShankarAgriMart.Application.Interfaces.Repositories;
using ShankarAgriMart.Application.Interfaces.Services;
using ShankarAgriMart.Domain.Entities;

namespace ShankarAgriMart.Application.Services;

public class InventoryService : IInventoryService
{
    private readonly IProductRepository _productRepository;
    private readonly IInventoryTransactionRepository _transactionRepository;

    public InventoryService(
        IProductRepository productRepository,
        IInventoryTransactionRepository transactionRepository)
    {
        _productRepository = productRepository;
        _transactionRepository = transactionRepository;
    }

    public async Task<List<InventoryTransactionResponse>> GetHistoryAsync(
        int productId)
    {
        var product = await _productRepository.GetByIdAsync(productId);

        if (product == null)
            throw new NotFoundException("Product not found.");

        var transactions = await _transactionRepository
            .GetByProductIdAsync(productId);

        return transactions
            .Select(Map)
            .ToList();
    }

    public async Task<InventoryTransactionResponse> AddTransactionAsync(
        int productId,
        CreateInventoryTransactionRequest request)
    {
        var product = await _productRepository.GetByIdAsync(productId);

        if (product == null)
            throw new NotFoundException("Product not found.");

        var transactionType = request.TransactionType.Trim();

        if (!new[] { "StockIn", "StockOut", "Adjustment" }
            .Contains(transactionType, StringComparer.OrdinalIgnoreCase))
        {
            throw new ArgumentException(
                "Invalid transaction type. Allowed types: StockIn, StockOut, Adjustment.");
        }

        int stockChange;

        switch (transactionType.ToLowerInvariant())
        {
            case "stockin":

                if (request.Quantity <= 0)
                    throw new ArgumentException(
                        "StockIn quantity must be greater than 0.");

                stockChange = request.Quantity;
                transactionType = "StockIn";
                break;

            case "stockout":

                if (request.Quantity <= 0)
                    throw new ArgumentException(
                        "StockOut quantity must be greater than 0.");

                stockChange = -request.Quantity;
                transactionType = "StockOut";
                break;

            case "adjustment":

                if (request.Quantity == 0)
                    throw new ArgumentException(
                        "Adjustment quantity cannot be zero.");

                stockChange = request.Quantity;
                transactionType = "Adjustment";
                break;

            default:
                throw new ArgumentException(
                    "Invalid transaction type.");
        }

        var newStock = product.Stock + stockChange;

        if (newStock < 0)
            throw new ArgumentException(
                "Insufficient stock. Stock cannot become negative.");

        product.Stock = newStock;
        product.UpdatedAt = DateTime.UtcNow;

        var inventoryTransaction = new InventoryTransaction
        {
            ProductId = productId,
            TransactionType = transactionType,
            Quantity = request.Quantity,
            Remarks = request.Remarks?.Trim()
        };

        await _productRepository.UpdateAsync(product);

        var created = await _transactionRepository
            .AddAsync(inventoryTransaction);

        return new InventoryTransactionResponse
        {
            Id = created.Id,
            ProductId = productId,
            ProductName = product.Name,
            TransactionType = created.TransactionType,
            Quantity = created.Quantity,
            Remarks = created.Remarks,
            CreatedAt = created.CreatedAt
        };
    }

    private static InventoryTransactionResponse Map(
        InventoryTransaction transaction)
    {
        return new InventoryTransactionResponse
        {
            Id = transaction.Id,
            ProductId = transaction.ProductId,
            ProductName = transaction.Product?.Name ?? string.Empty,
            TransactionType = transaction.TransactionType,
            Quantity = transaction.Quantity,
            Remarks = transaction.Remarks,
            CreatedAt = transaction.CreatedAt
        };
    }
}