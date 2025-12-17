using GMG.Domain.Common;
using GMG.Domain.Common.Result;

namespace GMG.Domain.Suppliers.Entities;

public class Supplier : OwnMultipleBranches
{
    public string Name { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string Phone { get; private set; } = string.Empty;
    public string Address { get; private set; } = string.Empty;

    // Private constructor enforces factory pattern
    private Supplier() { }

    private Supplier(string name, string email, string phone, string address)
    {
        Id = Guid.NewGuid();
        Name = name;
        Email = email;
        Phone = phone;
        Address = address;
    }

    // Factory method with Result pattern
    public static Result<Supplier> Create(string name, string email, string phone, string address)
    {
        // Validations
        if (string.IsNullOrWhiteSpace(name))
            return Result<Supplier>.Failure("Supplier name cannot be null or empty.");

        if (string.IsNullOrWhiteSpace(email))
            return Result<Supplier>.Failure("Supplier email cannot be null or empty.");

        if (!IsValidEmail(email))
            return Result<Supplier>.Failure("Supplier email format is invalid.");

        if (string.IsNullOrWhiteSpace(phone))
            return Result<Supplier>.Failure("Supplier phone cannot be null or empty.");

        if (string.IsNullOrWhiteSpace(address))
            return Result<Supplier>.Failure("Supplier address cannot be null or empty.");

        return Result<Supplier>.Success(new Supplier(name, email, phone, address));
    }

    // Domain method for updates
    public Result<Supplier> Update(string name, string email, string phone, string address)
    {
        // Validations
        if (string.IsNullOrWhiteSpace(name))
            return Result<Supplier>.Failure("Supplier name cannot be null or empty.");

        if (string.IsNullOrWhiteSpace(email))
            return Result<Supplier>.Failure("Supplier email cannot be null or empty.");

        if (!IsValidEmail(email))
            return Result<Supplier>.Failure("Supplier email format is invalid.");

        if (string.IsNullOrWhiteSpace(phone))
            return Result<Supplier>.Failure("Supplier phone cannot be null or empty.");

        if (string.IsNullOrWhiteSpace(address))
            return Result<Supplier>.Failure("Supplier address cannot be null or empty.");

        // State changes
        Name = name;
        Email = email;
        Phone = phone;
        Address = address;

        return Result<Supplier>.Success(this);
    }

    // Private validation helper
    private static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
}
