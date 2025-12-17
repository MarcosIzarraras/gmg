using GMG.Domain.Common;
using GMG.Domain.Common.Result;

namespace GMG.Domain.Customers.Entities;

public class Customer : OwnMultipleBranches
{
    public string Name { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string Phone { get; private set; } = string.Empty;
    public string Address { get; private set; } = string.Empty;

    // Private constructor enforces factory pattern
    private Customer() { }

    private Customer(string name, string email, string phone, string address)
    {
        Id = Guid.NewGuid();
        Name = name;
        Email = email;
        Phone = phone;
        Address = address;
    }

    // Factory method with Result pattern
    public static Result<Customer> Create(string name, string email, string phone, string address)
    {
        // Validations
        if (string.IsNullOrWhiteSpace(name))
            return Result<Customer>.Failure("Customer name cannot be null or empty.");

        if (string.IsNullOrWhiteSpace(email))
            return Result<Customer>.Failure("Customer email cannot be null or empty.");

        if (!IsValidEmail(email))
            return Result<Customer>.Failure("Customer email format is invalid.");

        if (string.IsNullOrWhiteSpace(phone))
            return Result<Customer>.Failure("Customer phone cannot be null or empty.");

        if (string.IsNullOrWhiteSpace(address))
            return Result<Customer>.Failure("Customer address cannot be null or empty.");

        return Result<Customer>.Success(new Customer(name, email, phone, address));
    }

    // Domain method for updates
    public Result<Customer> Update(string name, string email, string phone, string address)
    {
        // Validations
        if (string.IsNullOrWhiteSpace(name))
            return Result<Customer>.Failure("Customer name cannot be null or empty.");

        if (string.IsNullOrWhiteSpace(email))
            return Result<Customer>.Failure("Customer email cannot be null or empty.");

        if (!IsValidEmail(email))
            return Result<Customer>.Failure("Customer email format is invalid.");

        if (string.IsNullOrWhiteSpace(phone))
            return Result<Customer>.Failure("Customer phone cannot be null or empty.");

        if (string.IsNullOrWhiteSpace(address))
            return Result<Customer>.Failure("Customer address cannot be null or empty.");

        // State changes
        Name = name;
        Email = email;
        Phone = phone;
        Address = address;

        return Result<Customer>.Success(this);
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
