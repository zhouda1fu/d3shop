using FluentValidation;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.DepartmentAggregate;
using NetCorePal.D3Shop.Infrastructure.Repositories.Identity.Admin;
using NetCorePal.Extensions.Primitives;

namespace NetCorePal.D3Shop.Web.Application.Commands.Identity.VueAdmin
{
    public record VueUpdateDepartmentStatusCommand(
        DeptId DepartmentId,
        int Status) : ICommand;

    public class VueUpdateDepartmentStatusCommandValidator : AbstractValidator<VueUpdateDepartmentStatusCommand>
    {
        public VueUpdateDepartmentStatusCommandValidator()
        {
            RuleFor(x => x.DepartmentId).NotEmpty().WithMessage("部门ID不能为空");
            RuleFor(x => x.Status).InclusiveBetween(0, 1).WithMessage("状态值必须为0或1");
        }
    }

    public class VueUpdateDepartmentStatusCommandHandler : ICommandHandler<VueUpdateDepartmentStatusCommand>
    {
        private readonly IDepartmentRepository _departmentRepository;

        public VueUpdateDepartmentStatusCommandHandler(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public async Task Handle(VueUpdateDepartmentStatusCommand request, CancellationToken cancellationToken)
        {
            var department = await _departmentRepository.GetAsync(request.DepartmentId, cancellationToken) ??
                             throw new KnownException($"未找到部门，DepartmentId = {request.DepartmentId}");

            department.UpdateStatus(request.Status);
        }
    }
}