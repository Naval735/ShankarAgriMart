using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentValidation;
using ShankarAgriMart.Application.DTOs.Request;

namespace ShankarAgriMart.Application.Validators;

public class CreateProductValidator : AbstractValidator<CreateProductRequest>
{
    public CreateProductValidator()
    {
        RuleFor(x => x.CategoryId)
            .GreaterThan(0)
            .WithMessage("Category is required.");

        RuleFor(x => x.BrandId)
            .GreaterThan(0)
            .WithMessage("Brand is required.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Product name is required.")
            .MaximumLength(200);

        RuleFor(x => x.MRP)
            .GreaterThan(0)
            .WithMessage("MRP must be greater than 0.");

        RuleFor(x => x.SellingPrice)
            .GreaterThan(0)
            .WithMessage("Selling price must be greater than 0.");

        RuleFor(x => x.SellingPrice)
            .LessThanOrEqualTo(x => x.MRP)
            .WithMessage("Selling price cannot be greater than MRP.");

        RuleFor(x => x.GST)
            .InclusiveBetween(0, 100)
            .WithMessage("GST must be between 0 and 100.");

        RuleFor(x => x.Stock)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Stock cannot be negative.");

        RuleFor(x => x.Weight)
            .GreaterThan(0)
            .When(x => x.Weight.HasValue)
            .WithMessage("Weight must be greater than 0.");

        RuleFor(x => x.Unit)
            .MaximumLength(20)
            .When(x => !string.IsNullOrWhiteSpace(x.Unit));

        RuleFor(x => x.ActiveIngredient)
            .MaximumLength(500)
            .When(x => !string.IsNullOrWhiteSpace(x.ActiveIngredient));

        RuleFor(x => x.Dosage)
            .MaximumLength(1000)
            .When(x => !string.IsNullOrWhiteSpace(x.Dosage));

        RuleFor(x => x.ApplicationMethod)
            .MaximumLength(1000)
            .When(x => !string.IsNullOrWhiteSpace(x.ApplicationMethod));

        RuleFor(x => x.Benefits)
            .MaximumLength(2000)
            .When(x => !string.IsNullOrWhiteSpace(x.Benefits));

        RuleFor(x => x.UsageInstructions)
            .MaximumLength(3000)
            .When(x => !string.IsNullOrWhiteSpace(x.UsageInstructions));

        RuleFor(x => x.SafetyPrecautions)
            .MaximumLength(3000)
            .When(x => !string.IsNullOrWhiteSpace(x.SafetyPrecautions));

        RuleFor(x => x.Manufacturer)
            .MaximumLength(200)
            .When(x => !string.IsNullOrWhiteSpace(x.Manufacturer));

        RuleFor(x => x.CountryOfOrigin)
            .MaximumLength(100)
            .When(x => !string.IsNullOrWhiteSpace(x.CountryOfOrigin));

        RuleFor(x => x.ExpiryDate)
            .GreaterThan(DateTime.UtcNow)
            .When(x => x.ExpiryDate.HasValue)
            .WithMessage("Expiry date must be in the future.");
    }
}