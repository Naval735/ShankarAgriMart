using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using ShankarAgriMart.Application.Common.Exceptions;
using ShankarAgriMart.Application.DTOs.Request;
using ShankarAgriMart.Application.DTOs.Response;
using ShankarAgriMart.Application.Interfaces.Repositories;
using ShankarAgriMart.Application.Interfaces.Services;
using ShankarAgriMart.Domain.Entities;
using ShankarAgriMart.Infrastructure.Data;

namespace ShankarAgriMart.Application.Services;

public class InventoryService : IInventoryService
{
    private readonly AppDbContext _context;
    private readonly IProductRepository _productRepository;
    private readonly IInventoryTransactionRepository _transactionRepository;

    public InventoryService(
        AppDbContext context,
        IProductRepository productRepository,
        IInventoryTransactionRepository transactionRepository)
    {
        _context = context;
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

        return transactions.Select(Map).ToList();
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

        await using var transaction =
            await _context.Database.BeginTransactionAsync();

        try
        {
            product.Stock = newStock;
            product.UpdatedAt = DateTime.UtcNow;

            _context.Products.Update(product);

            var inventoryTransaction = new InventoryTransaction
            {
                ProductId = productId,
                TransactionType = transactionType,
                Quantity = request.Quantity,
                Remarks = request.Remarks?.Trim()
            };

            await _context.InventoryTransactions
                .AddAsync(inventoryTransaction);

            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            return new InventoryTransactionResponse
            {
                Id = inventoryTransaction.Id,
                ProductId = productId,
                ProductName = product.Name,
                TransactionType = transactionType,
                Quantity = request.Quantity,
                Remarks = inventoryTransaction.Remarks,
                CreatedAt = inventoryTransaction.CreatedAt
            };
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
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