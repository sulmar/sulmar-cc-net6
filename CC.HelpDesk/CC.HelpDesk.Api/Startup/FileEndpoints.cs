namespace CC.HelpDesk.Api;

public static class FileEndpoints
{
    public static WebApplication MapFileEndpoints(this WebApplication app)
    {
        app.MapGet("upload", (HttpRequest request) =>
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
