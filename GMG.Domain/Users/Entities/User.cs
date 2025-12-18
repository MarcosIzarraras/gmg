using GMG.Domain.Branches.Entities;
using GMG.Domain.Common.Result;
using GMG.Domain.Users.Enums;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Text;

namespace GMG.Domain.Users.Entities
{
    public class User
    {
        public Guid Id { get; private set; }
        public string Username { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string PasswordHash { get; private set; } = string.Empty;
        public string FirstName { get; private set; } = string.Empty;
        public string LastName { get; private set; } = string.Empty;
        public UserRole UserRole { get; private set; }
        public bool IsActive { get; private set; }


        public DateTime CreatedAt { get; private set; }
        public DateTime UpdateAt { get; private set; }
        public DateTime? LastLoginAt { get; private set; }

        private List<Branch> _branches = new();
        public IReadOnlyCollection<Branch> Branches
            => _branches.AsReadOnly();

        private User() { }
        private User(
            string username,
            string email,
            string passwordHash,
            string firstName,
            string lastName,
            UserRole userRole,
            bool isActive)
        {
            Id = Guid.NewGuid();
            Username = username;
            Email = email;
            PasswordHash = passwordHash;
            FirstName = firstName;
            LastName = lastName;
            UserRole = userRole;
            IsActive = isActive;
            CreatedAt = DateTime.UtcNow;
            UpdateAt = DateTime.UtcNow;
            LastLoginAt = DateTime.UtcNow;
        }

        public void SetLastLogin()
        {
            LastLoginAt = DateTime.UtcNow;
        }

        public static Result<User> Create(
            string username,
            string email,
            string passwordHash,
            string firstName,
            string lastName,
            UserRole userRole)
        {
            return new Validation()
                .Required(!string.IsNullOrEmpty(username), "Username cant be empty.")
                .Required(!string.IsNullOrEmpty(email) && email.Contains("@"), "Email cant be empty.")
                .Required(!string.IsNullOrEmpty(passwordHash), "Password cant be empty.")
                .Required(!string.IsNullOrEmpty(firstName), "First name cant be empty.")
                .Required(!string.IsNullOrEmpty(lastName), "Last name cant be empty.")
                .Build(() => new User(
                    username,
                    email,
                    passwordHash,
                    firstName,
                    lastName,
                    userRole,
                    true));
        }

        public Result<Branch> AddBranch(string name, string code)
        {
            var branch = Branch.CreateDefaultBranchForUser(this.Id, name, code);
            if (branch.IsSuccess)
                _branches.Add(branch.Value);

            return branch;
        }
    }
}
