using IdentityHub.IdentityService.Domain.ValueObjects.Status;
using Shared.Kernel.Primitives;
using Shared.Kernel.Results;

namespace IdentityHub.IdentityService.Domain.Models
{
    public sealed class Status : AggregateRoot<StatusId>
    {
        public StatusName Name { get; private set; }

        private Status() { }
        private Status(StatusId id, StatusName statusName) : base(id)
            => Name = statusName;

        public static Result<Status> Create(StatusId id, StatusName statusName)
        {
            var statusIdResult = StatusId.Create(id);
            var statusNameResult = StatusName.Create(statusName);

            if (statusIdResult.IsFailure || statusNameResult.IsFailure)
            {
                var errors = new List<Error>();

                errors.AddRange(statusIdResult.Errors);
                errors.AddRange(statusNameResult.Errors);

                return Result<Status>.Failure(errors);
            }

            var status = new Status
                (
                    statusIdResult.Value,
                    statusNameResult.Value
                );

            return Result<Status>.Success(status);
        }

        public void UpdateStatusName(StatusName statusName)
            => Name = statusName;
    }
}
