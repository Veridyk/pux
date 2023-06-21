using Microsoft.AspNetCore.Mvc;
using Pux.Providers;
using Pux.Services;

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
            builder.WithOrigins("https://localhost/", "http://localhost:7273")
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
    catch
    {
        return Results.BadRequest("Invalid directory: " + path);
    }
})
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
    catch
    {
        return Results.BadRequest("Invalid directory: " + path);
    }
})
.WithName("Difference")
.WithOpenApi();

app.Run();
