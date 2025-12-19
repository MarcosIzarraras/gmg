using GMG.Application.Common.Interfaces;
using GMG.Application.Common.Persistence;
using MediatR;

namespace GMG.Application.Common.Behaviours
{
    public class TenantContextBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContext _currentUserService;

        public TenantContextBehaviour(IUnitOfWork unitOfWork, IUserContext currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_currentUserService.IsAuthenticated)
            {
                _unitOfWork.SetTenantContext(
                    _currentUserService.OwnerId, 
                    _currentUserService.BranchId, 
                    _currentUserService.BranchUserId, 
                    _currentUserService.IsOwner);
            }

            return await next();
        }
    }
}
