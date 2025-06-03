using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using TaskListApp.Mapping;
using TaskLists.Application;
using TaskLists.Application.Database;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
builder.Services.AddControllers();
builder.Services.AddApplication();

//mongoDb
builder.Services.AddDatabase(config["MongoDb:ConnectionString"]!, config["MongoDb:DatabaseName"]!);
BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

builder.Services.AddSwaggerGen();


var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();


app.UseMiddleware<ValidationMappingMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.MapControllers();

app.UseHttpsRedirection();

app.Run();
