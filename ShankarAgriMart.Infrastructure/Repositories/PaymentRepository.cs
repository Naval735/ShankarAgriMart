using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using ShankarAgriMart.Application.Interfaces.Repositories;
using ShankarAgriMart.Domain.Entities;
using ShankarAgriMart.Infrastructure.Data;

namespace ShankarAgriMart.Infrastructure.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly AppDbContext _context;

    public PaymentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Payment?> GetByOrderIdAsync(int orderId)
    {
        return await _context.Payments
            .FirstOrDefaultAsync(x =>
                x.OrderId == orderId &&
                !x.IsDeleted);
    }

    public async Task<Payment?> GetByRazorpayOrderIdAsync(
        string razorpayOrderId)
    {
        return await _context.Payments
            .FirstOrDefaultAsync(x =>
                x.RazorpayOrderId == razorpayOrderId &&
                !x.IsDeleted);
    }

    public async Task<Payment> CreateAsync(Payment payment)
    {
        await _context.Payments.AddAsync(payment);
        await _context.SaveChangesAsync();

        return payment;
    }

    public async Task UpdateAsync(Payment payment)
    {
        payment.UpdatedAt = DateTime.UtcNow;

        _context.Payments.Update(payment);

        await _context.SaveChangesAsync();
    }
}