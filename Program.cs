using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

#region Minimal APIs to upload files (Single and multiple)
app.MapPost("/upload", async (IFormFile receivedFile) =>
{
    // Read the folder where the file is to be saved
    var folder = Path.Combine(Directory.GetCurrentDirectory(), "files");
    // REad the Uploaded File Name
    var postedFileName = ContentDispositionHeaderValue
        .Parse(receivedFile.ContentDisposition)
        .FileName.Trim('"');

    // set the file path as FolderName/FileName
    var finalPath = Path.Combine(folder, postedFileName);
    using var fs = File.OpenWrite(finalPath);
        
    await receivedFile.CopyToAsync(fs);
        
});


app.MapPost("/uploadfiles", async (IFormFileCollection receivedFiles) =>
{
    // Read the folder where the file is to be saved
    var folder = Path.Combine(Directory.GetCurrentDirectory(), "files");
    foreach (var file in receivedFiles)
    {
        // REad the Uploaded File Name
        var postedFileName = ContentDispositionHeaderValue
            .Parse(file.ContentDisposition)
            .FileName.Trim('"');

        // set the file path as FolderName/FileName
        var finalPath = Path.Combine(folder, postedFileName);
        using var fs = File.OpenWrite(finalPath);

        await file.CopyToAsync(fs);
    }
});


#endregion
app.Run();

 
