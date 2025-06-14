﻿namespace TaskListApp;

public static class ApiEndpoints
{
    private const string ApiBase = "api";
    

    public static class TaskListEndpoints
    {
        private const string Base = $"{ApiBase}/tasklist";
        public const string Create = Base;
        
        public const string GetByListId = $"{Base}/by-list/{{listId:guid}}";
        public const string GetAll = $"{Base}/all";
        public const string Update = $"{Base}/{{listId:guid}}";
        public const string Delete = $"{Base}/{{listId:guid}}";
        
    }

    
    public static class TaskItemEndpoints
    {
        private const string Base = $"{ApiBase}/task";
        
        public const string Create = $"{Base}/{{listId:guid}}";
        
    }

    public static class UsersEndpoints
    {
        private const string Base = $"{ApiBase}/users";
        public const string Create = Base;
        
    }
    
    
    public static class TaskListConnectionsEndpoints
    {
        private const string Base = $"{ApiBase}/connection";
        
        public const string CreateConnection = $"{Base}/{{listId:guid}}";
        public const string GetAllConnections =  $"{Base}/all";
        public const string Delete = $"{Base}/{{listId:guid}}";
        
    }
    
}