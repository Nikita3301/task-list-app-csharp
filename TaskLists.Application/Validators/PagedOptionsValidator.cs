using FluentValidation;
using TaskLists.Application.Models;

namespace TaskLists.Application.Validators;

public class PagedOptionsValidator : AbstractValidator<PageOptions>
{
    public PagedOptionsValidator()
    {
        RuleFor(request => request.Page).GreaterThan(0);
        RuleFor(request => request.PageSize).InclusiveBetween(1, 25).WithMessage("You can only get from 1 to 25 movies per page");
    }
}