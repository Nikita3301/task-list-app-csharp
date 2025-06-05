using TaskLists.Application.Models;
using TaskLists.Contracts.Requests;
using TaskLists.Contracts.Responses;

namespace TaskListApp.Mapping;

public static class ContractMapping
{
    public static TaskList MapToTaskList(this CreateTaskListRequest request)
    {
        return new TaskList
        {
            Name = request.Name,
            OwnerId = request.UserId,
            ConnectedUsers = request.ConnectedUsers ?? [],
            Tasks = request.Tasks?.MapToTasks()
        };
    }


    public static TaskList MapToTaskList(this UpdateTaskListRequest request, Guid listId)
    {
        return new TaskList
        {
            Id = listId,
            Name = request.ListName,
            OwnerId = request.UserId,
        };
    }


    private static TaskItem MapToTask(this CreateTaskRequest request)
    {
        return new TaskItem
        {
            Description = request.Description,
            Completed = request.Completed,
        };
    }

    private static List<TaskItem> MapToTasks(this List<CreateTaskRequest> request)
    {
        return request.Select(MapToTask).ToList();
    }
    


    public static TaskListResponse MapToTaskListResponse(this TaskList taskList)
    {
        return new TaskListResponse
        {
            Id = taskList.Id,
            Name = taskList.Name,
            OwnerId = taskList.OwnerId,
            Tasks = taskList.Tasks?.Select(MapToTaskItemResponse).ToList(),
            ConnectedUsers = taskList.ConnectedUsers,
            CreatedAt = taskList.CreatedAt  
        };
    }
    

    public static PageOptions MapToOptions(this GetAllTaskListsRequest request)
    {
        return new PageOptions()
        {
            Page = request.Page,
            PageSize = request.PageSize,
        };
    }

    public static PagedResponse<TaskListShortResponse> MapToPagedResponse(this PagedResult<TaskList> taskList)
    {
        return new PagedResponse<TaskListShortResponse>
        {
            Items = from item in taskList.Items
                select new TaskListShortResponse
                {
                    Id = item.Id,
                    Name = item.Name,
                    CreatedAt = item.CreatedAt
                },
            Page = taskList.Page,
            PageSize = taskList.PageSize,
            TotalItemsCount = taskList.TotalItemsCount,
            TotalPages = taskList.TotalPages,
            HasNextPage = taskList.HasNextPage,
        };
    }


    public static TaskItemResponse MapToTaskItemResponse(this TaskItem taskItem)
    {
        return new TaskItemResponse
        {
            Description = taskItem.Description,
            Completed = taskItem.Completed,
        };
    }
    
}