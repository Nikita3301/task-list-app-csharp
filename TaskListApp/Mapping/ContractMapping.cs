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
            ListName = request.ListName,
            OwnerId = request.UserId
        };
    }


    public static TaskList MapToTaskList(this UpdateTaskListRequest request, Guid listId)
    {
        return new TaskList
        {
            ListId = listId,
            ListName = request.ListName,
            OwnerId = request.UserId
        };
    }


    public static TaskList MapToTaskListAndTasks(this UpdateFullTaskListRequest request, Guid listId, DateTime createdAt)
    {
        List<TaskItem> tasks = [];
        if (request.Tasks != null)
        {
            tasks = request.Tasks.Select(item => MapToTask(item, listId)).ToList();
        }


        return new TaskList
        {
            ListId = listId,
            ListName = request.ListName,
            OwnerId = request.UserId,
            Tasks = tasks,
            CreatedAt = createdAt
        };
    }


    public static TaskItem MapToTask(this CreateTaskRequest request, Guid listId)
    {
        return new TaskItem
        {
            ListId = listId,
            Description = request.Description,
            Completed = request.Completed,
        };
    }

    public static List<TaskItem> MapToTasks(this List<CreateTaskRequest> request, Guid listId)
    {
        return request.Select(item => MapToTask(item, listId)).ToList();
    }
    


    public static TaskListResponse MapToTaskListResponse(this TaskList taskList)
    {
        return new TaskListResponse
        {
            ListId = taskList.ListId,
            ListName = taskList.ListName,
            OwnerId = taskList.OwnerId,
            Tasks = taskList.Tasks?.Select(MapToTaskItemResponse).ToList(),
            CreatedAt = taskList.CreatedAt  
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