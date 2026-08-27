using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ShankarAgriMart.Domain.Entities;

namespace ShankarAgriMart.Application.Interfaces.Repositories;

public interface IPaymentRepository
{
    Task<Payment?> GetByOrderIdAsync(int orderId);

    Task<Payment?> GetByRazorpayOrderIdAsync(
        string razorpayOrderId);

    Task<Payment> CreateAsync(Payment payment);

    Task UpdateAsync(Payment payment);
}