using Microsoft.AspNetCore.Mvc;
using Pux.Dto;
using Pux.Providers;
using Pux.Services;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IFileProvider, FileProvider>();
builder.Services.AddSingleton<IHistoryProvider, HistoryProvider>();
builder.Services.AddTransient<IFileService, FileService>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder.WithOrigins("*")
            .AllowAnyHeader()
            .AllowAnyMethod();
        });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(builder =>
{
    builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader();
});

app.MapGet("/list", (string path, [FromServices] IFileService service) =>
{
    if (string.IsNullOrEmpty(path))
    {
        return Results.BadRequest("Fill the directory please");
    }

    try
    {
        var files = service.GetDirectoryContent(path);
        return Results.Ok(files);
    } 
    catch (Exception ex)
    {
        var errorResponse = new ErrorResponse()
        {
            Directory = path,
            Message = ex.Message
        };

        return Results.BadRequest(errorResponse);
    }
})
.Produces<IList<FileInDirectoryDto>>((int)HttpStatusCode.OK)
.Produces<ErrorResponse>((int)HttpStatusCode.BadRequest)
.WithName("List")
.WithOpenApi();

app.MapGet("/difference", (string path, [FromServices] IFileService service) =>
{
    if (string.IsNullOrEmpty(path))
    {
        return Results.BadRequest("Fill the directory please");
    }

    try
    {
        var entries = service.Compare(path);
        return Results.Ok(entries);
    }
    catch (Exception ex)
    {
        var errorResponse = new ErrorResponse()
        {
            Directory = path,
            Message = ex.Message
        };

        return Results.BadRequest(errorResponse);
    }
})
.Produces<IList<FileInDirectoryDto>>((int)HttpStatusCode.OK)
.Produces<ErrorResponse>((int)HttpStatusCode.BadRequest)
.WithName("Difference")
.WithOpenApi();

app.Run();
