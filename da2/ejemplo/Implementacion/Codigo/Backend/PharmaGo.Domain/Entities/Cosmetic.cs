using System.Text.RegularExpressions;

namespace PharmaGo.Domain.Entities;

public class Cosmetic : Product
{
    public string Description { get; set; }

    public void ValidateCosmetic()
    {
        ValidateCode();
        ValidateName();
        ValidateDescription();
        ValidatePrice();
    }

    private void ValidateCode()
    {
        if (!Regex.IsMatch(Code, @"^\d{5}$"))
        {
            throw new ArgumentException("Code must be a numeric value of 5 digits.");
        }
    }

    private void ValidateName()
    {
        if (Name.Length > 30 || !Regex.IsMatch(Name, @"^[a-zA-Z0-9\s]+$"))
        {
            throw new ArgumentException("Name must be alphanumeric and cannot exceed 30 characters.");
        }
    }

    private void ValidateDescription()
    {
        if (Description.Length > 70 || !Regex.IsMatch(Description, @"^[a-zA-Z0-9\s]+$"))
        {
            throw new ArgumentException("Description must be alphanumeric and cannot exceed 70 characters.");
        }
    }

    private void ValidatePrice()
    {
        if (Price < 0 || decimal.Round(Price, 2) != Price)
        {
            throw new ArgumentException("Price must be a positive value with two decimal places.");
        }
    }
}
