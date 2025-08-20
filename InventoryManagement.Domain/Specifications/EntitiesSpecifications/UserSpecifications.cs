using InventoryManagement.Domain.Constants;
using InventoryManagement.Domain.Entities;
using System.Linq.Expressions;

namespace InventoryManagement.Domain.Specifications.EntitiesSpecifications
{
    public class UserSpecifications : BaseSpecification<User>
    {
        public UserSpecifications(int pageNumber, int pageSize, string? sortBy, SortDirection sortDirection)
        {
            AddIncludes($"{nameof(User.UserRoles)}.{nameof(UserRole.Role)}");
            ApplySorting(sortBy, sortDirection);
            ApplyPagination(pageSize * (pageNumber - 1), pageSize);
        }

        private void ApplySorting(string? sortBy, SortDirection sortDirection)
        {
            var key = sortBy?.Trim().ToLowerInvariant();

            var sortSelectors = new Dictionary<string, Expression<Func<User, object>>>
            {
                [nameof(User.Id).ToLower()] = x => x.Id,
                [nameof(User.FirstName).ToLower()] = x => x.FirstName,
                [nameof(User.LastName).ToLower()] = x => x.LastName,
                [nameof(User.Email).ToLower()] = x => x.Email!,
                [nameof(User.UserName).ToLower()] = x => x.UserName!
            };

            if (string.IsNullOrEmpty(key) || !sortSelectors.ContainsKey(key))
                key = nameof(User.Id).ToLower();

            var selector = sortSelectors[key];

            if (sortDirection == SortDirection.Descending)
                ApplyOrderByDescending(selector);
            else
                ApplyOrderBy(selector);
        }
    }
}
