namespace GLMS.Services;

public class FileService
{
    public async Task<string?> SavePdfAsync(IFormFile? file)
    {
        if (file == null || file.Length == 0)
            return null;

        // STRICT PDF VALIDATION
        if (file.ContentType != "application/pdf")
            return null;

        // CREATE UNIQUE FILE NAME
        var uniqueFileName =
            Guid.NewGuid().ToString() +
            Path.GetExtension(file.FileName);

        // CREATE FOLDER
        var folderPath = Path.Combine(
            Directory.GetCurrentDirectory(),
            "wwwroot/files");

        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        // FINAL PATH
        var filePath = Path.Combine(folderPath, uniqueFileName);

        // SAVE FILE
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        // RETURN RELATIVE PATH
        return "/files/" + uniqueFileName;
    }
}