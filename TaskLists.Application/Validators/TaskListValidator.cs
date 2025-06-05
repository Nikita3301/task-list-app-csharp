using FluentValidation;
using TaskLists.Application.Models;


namespace TaskLists.Application.Validators;

public class TaskListValidator : AbstractValidator<TaskList>
{
    public TaskListValidator()
    {
        RuleFor(taskList => taskList.Name).Length(1, 255).WithMessage("List name should have from 1 to 255 characters.");
    }
}