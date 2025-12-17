using GMG.Application.Common;
using System.Security.Claims;

namespace GMGv2.Common
{
    public class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
    {
        // Propiedades de IUserContext
        public Guid UserId => GetCurrentUserId();
        public Guid OwnerId => GetCurrentOwnerId();
        public Guid? BranchId => GetCurrentBranchId();
        public Guid? BranchUserId => GetCurrentBranchUserId();
        public bool IsOwner => IsOwnerUser();
        public bool IsBranchUser => IsBranchUserType();
        public bool IsAuthenticated => httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

        public string UserName => GetUserName();

        public string UserType => GetUserType();

        public string Email => GetUserEmail();

        // Métodos auxiliares
        private bool IsDesignTimeOrStartup() => httpContextAccessor.HttpContext == null;

        public Guid? GetCurrentBranchId()
        {
            if (IsDesignTimeOrStartup())
                return null;

            var branchIdClaim = httpContextAccessor.HttpContext?.User?
                .FindFirst("BranchId")?.Value;

            return string.IsNullOrEmpty(branchIdClaim) ? null : Guid.Parse(branchIdClaim);
        }

        public Guid? GetCurrentBranchUserId()
        {
            if (IsDesignTimeOrStartup())
                return null;

            var branchUserIdClaim = httpContextAccessor.HttpContext?.User
                .FindFirst("BranchUserId")?.Value;

            return string.IsNullOrEmpty(branchUserIdClaim) ? null : Guid.Parse(branchUserIdClaim);
        }

        public Guid GetCurrentUserId()
        {
            if (IsDesignTimeOrStartup())
                return Guid.Empty;

            var userIdClaim = httpContextAccessor.HttpContext?.User?
                .FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
                throw new UnauthorizedAccessException("User not authenticated.");

            return Guid.Parse(userIdClaim);
        }

        private Guid GetCurrentOwnerId()
        {
            if (IsDesignTimeOrStartup())
                return Guid.Empty;

            // Primero intentar obtener el claim OwnerId
            var ownerIdClaim = httpContextAccessor.HttpContext?.User?
                .FindFirst("OwnerId")?.Value;

            if (!string.IsNullOrEmpty(ownerIdClaim))
            {
                return Guid.Parse(ownerIdClaim);
            }

            // Si no existe el claim OwnerId y es Owner, usar su propio UserId
            if (IsOwnerUser())
            {
                return GetCurrentUserId();
            }

            // Si es BranchUser y no tiene claim OwnerId, lanzar excepción
            throw new InvalidOperationException("OwnerId not found for current user.");
        }

        public string GetUserEmail()
        {
            if (IsDesignTimeOrStartup())
                return string.Empty;

            var emailClaim = httpContextAccessor.HttpContext?.User?
                .FindFirst(ClaimTypes.Email)?.Value;

            return emailClaim ?? string.Empty;
        }

        public string GetUserName()
        {
            if (IsDesignTimeOrStartup())
                return string.Empty;

            var nameClaim = httpContextAccessor.HttpContext?.User?
                .FindFirst(ClaimTypes.Name)?.Value;

            return nameClaim ?? string.Empty;
        }

        public string GetUserType()
        {
            if (IsDesignTimeOrStartup())
                return "Owner"; // Por defecto Owner para migraciones

            var userTypeClaim = httpContextAccessor.HttpContext?.User?
                .FindFirst("UserType")?.Value;

            return userTypeClaim ?? "Unknown";
        }

        private bool IsBranchUserType()
        {
            var userType = GetUserType();
            return userType.Equals("BranchUser", StringComparison.OrdinalIgnoreCase);
        }

        private bool IsOwnerUser()
        {
            var userType = GetUserType();
            return userType.Equals("Owner", StringComparison.OrdinalIgnoreCase);
        }
    }
}