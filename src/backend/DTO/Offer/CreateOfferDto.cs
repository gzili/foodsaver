using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace backend.DTO.Offer
{
    public class CreateOfferDto : IValidatableObject
    {
        public string Description { get; set; }
        public DateTime ExpiresAt { get; set; }
        public decimal FoodMinQuantity { get; set; }
        public string FoodName { get; set; }
        public IFormFile FoodPhoto { get; set; }
        public string FoodUnit { get; set; }
        public decimal Quantity { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ExpiresAt <= DateTime.UtcNow)
            {
                yield return new ValidationResult(
                    "Expiration date must be in the future",
                    new[] { nameof(ExpiresAt) });
            }

            if (Quantity <= 0)
            {
                yield return new ValidationResult(
                    "Quantity must be positive",
                    new[] { nameof(Quantity) });
            }

            if (FoodMinQuantity is <= 0 or > 1000)
            {
                yield return new ValidationResult(
                    "Minimum Quantity must be positive and not greater than 1000",
                    new[] { nameof(FoodMinQuantity) });
            }
            
            if (FoodMinQuantity != 0 && Quantity % FoodMinQuantity != 0)
            {
                yield return new ValidationResult(
                    "Quantity must be a factor of Minimum Quantity",
                    new[] { nameof(Quantity) });
            }
        }
    }
}