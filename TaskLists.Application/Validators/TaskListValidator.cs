using FluentValidation;
using TaskLists.Application.Models;


namespace TaskLists.Application.Validators;

public class TaskListValidator : AbstractValidator<TaskList>
{
    public TaskListValidator()
    {
        RuleFor(taskList => taskList.ListName).Length(1, 255).WithMessage("List Name should have from 1 to 255 characters.");
    }
}