using System.Net.Mime;

namespace CC.HelpDesk.Api;

public static class FileEndpoints
{
    public static WebApplication MapFileEndpoints(this WebApplication app)
    {
        app.MapGet("api/download", (string filename) =>
        {
            string path = Path.Combine(app.Environment.ContentRootPath, "downloads", filename);

            if (!File.Exists(path))
            {
                return Results.NotFound();
            }

            return Results.File(path, MediaTypeNames.Application.Pdf);
        }
        );

        app.MapGet("api/upload", (HttpRequest request) =>
        {
            if (!request.HasFormContentType)
            {
                return Results.BadRequest();
            }

            var form = request.Form;
            var file = form.Files["file"];

            if (file is null)
            {
                return Results.BadRequest();
            }

            var uploads = Path.Combine("uploads", file.FileName);
            using var uploadStream = file.OpenReadStream();
            using var fileStream = File.OpenWrite(uploads);

            uploadStream.CopyTo(fileStream);

            return Results.NoContent();

        });


        return app;
    }
}
