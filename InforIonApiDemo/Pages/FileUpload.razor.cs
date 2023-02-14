using System;
using System.Collections.Generic;
using System.Linq;

namespace InforIonApiDemo.Pages
{
    public partial class FileUpload
    {
        private long maxFileSize = 1024 * 1024 * 1; // represents 1MB
        private int maxAllowedFiles = 1;
        private List<string> errors = new();
        private async Task LoadFiles(InputFileChangeEventArgs e)
        {
            errors.Clear();
            if (e.FileCount > maxAllowedFiles)
            {
                errors.Add($"Error: Attempting to upload {e.FileCount} files, but only {maxAllowedFiles} files are allowed");
                return;
            }

            foreach (var file in e.GetMultipleFiles(maxAllowedFiles))
            {
                // move the file to storage
                try
                {
                    //string newFileName = Path.ChangeExtension(Path.GetRandomFileName(), Path.GetExtension(file.Name));
                    string newFileName = Path.ChangeExtension(file.Name, ".json");
                    string path = Path.Combine(config.GetValue<string>("FileStorage")!, "UploadedFiles", newFileName);
                    Directory.CreateDirectory(Path.Combine(config.GetValue<string>("FileStorage")!, "UploadedFiles"));
                    await using FileStream fs = new(path, FileMode.Create);
                    await file.OpenReadStream(maxFileSize).CopyToAsync(fs);
                }
                catch (Exception ex)
                {
                    errors.Add($"File: {file.Name} Error: {ex.Message}");
                }
            }
        }
    }
}