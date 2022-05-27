using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using FileUtility;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello Oat World!");

app.MapGet("/Files", async context =>
{
    string path = builder.Configuration["MetaDataFileLoc"];
    string readText = File.ReadAllText(path);

    await context.Response.WriteAsJsonAsync(readText);
});

app.MapPost("/Files/MetaData", async context =>
{
    if (!context.Request.HasJsonContentType())
    {
        context.Response.StatusCode = (int)HttpStatusCode.UnsupportedMediaType;
        return;
    }

    var jsonData = await context.Request.ReadFromJsonAsync<FileMetaData>();
    JsonSerializerOptions _options =
        new() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };
    var jsonString = JsonSerializer.Serialize(jsonData, _options);

    var outputPath = builder.Configuration["MetaDataFileLoc"];
    await File.WriteAllTextAsync(outputPath, jsonString);

    context.Response.StatusCode = (int)HttpStatusCode.Created;
});

app.MapPost("/Files/UploadFiles", (HttpRequest request) =>
{
    if(!request.Form.Files.Any())
    {
        return Results.BadRequest("No File");
    }

    foreach(var file in request.Form.Files)
    {
        using (var stream = new FileStream(builder.Configuration["ChunkFilesLoc"] + file.FileName, FileMode.Create))
        {
            file.CopyTo(stream);
        }
    }

    var chunckMeta = request.Form["chunckMeta"];

    return Results.Ok($"File Upload Successfully - {chunckMeta}");
});

app.Run();