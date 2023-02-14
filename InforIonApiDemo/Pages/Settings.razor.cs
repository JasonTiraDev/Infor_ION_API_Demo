using InforIonApiDemo;
using InforIonApiDemo.Models;


namespace InforIonApiDemo.Pages
{

    public partial class Settings
    {
        // button
        private bool _processing = false;

        InforConnectionRoot inforConnectionRoot = new();
        InforConnection inforConnection = new();
        ProgramSettingsModel programSettings = new();
        FileUploadModel fileUploadModel = new();
        IBrowserFile? browserFile;
        private JsonSettings jsonSettings = new JsonSettings();
        //IConfiguration config;
        
        private long maxFileSize = 1024 * 1024 * 1; // represents 1MB
        private int maxAllowedFiles = 1;
        private List<string> errors = new();
        private string[] uploadedFiles;
        
        private string selectedFile;
        public string SelectedFile
        {
            get { return selectedFile; }   // get method
            set { selectedFile = value; 
            OnUploadedFileSelect(selectedFile);}  // set method            
        }

        private string selectedFileToDelete;
        public string SelectedFileToDelete
        {
            get { return selectedFileToDelete; }   // get method
            set { selectedFileToDelete = value; 
            //DeleteFileSelect(selectedFileToDelete);} // set method   
            OpenDeleteFileDialog(selectedFileToDelete);}         
        }

        protected override async void OnInitialized()
        {
            LoadAppSettings();
            GetUploadedFileList();
        }
        public async Task UpdateProgramSettings()
        {
            {
            _processing = true;
            await jsonSettings.UpdateProgramSettings(programSettings);
            _processing = false;
            }
        }

        private async Task SubmitForm()
        {
            try 
            {
                string relativePath = await CaptureFile();
                fileUploadModel.Filename = relativePath;
            }
            catch(Exception ex)
            {
                errors.Add($"Error: {ex.Message}");
            }
        }

        public void LoadAppSettings()
        {
            try
            {
                inforConnection = jsonSettings.GetInforSettings();
                programSettings = jsonSettings.GetProgramSettings();
            }
            catch (Exception e)
            {
                var exception = e.Message;
            }
        }

        public async Task GetUploadedFileList()
        {
            //uploadedFiles = Directory.GetFiles(config.GetValue<string>("FileStorage")!);
            uploadedFiles =  new DirectoryInfo(config.GetValue<string>("FileStorage")!).GetFiles().Select(o => o.Name).ToArray();
        }

        public async Task UpdateInforSettings()
        {
            _processing = true;
            await jsonSettings.UpdateInforSettings(inforConnection);
            _processing = false;
        }

        private void LoadFiles(InputFileChangeEventArgs e)
        {
            browserFile = e.File;
            
        }
        private async Task<string> CaptureFile()
        {
            if (browserFile is null)
            {
                return "";
            }

            errors.Clear();

            var configFileStorage = config.GetValue<string>("FileStorage");
            try
            {
                //string newFileName = Path.ChangeExtension(Path.GetRandomFileName(), Path.GetExtension(file.Name));
                
                string newFileName = Path.ChangeExtension(browserFile.Name, ".json");
                string path = Path.Combine(config.GetValue<string>("FileStorage")!, newFileName);
                
                string relativePath = Path.Combine("Uploaded", newFileName);

                Directory.CreateDirectory(Path.Combine(config.GetValue<string>("FileStorage")!));
                await using FileStream fs = new(path, FileMode.Create);
                await browserFile.OpenReadStream(maxFileSize).CopyToAsync(fs);

                
                return relativePath;
            }
            catch (Exception ex)
            {
                errors.Add($"File: {browserFile.Name} Error: {ex.Message}");
                throw;
            }
        }

        //TODO: Add method to delete files from uploaded
        //TODO: add parameter in program settings for upload path

        private async void OnUploadedFileSelect(string selectedFiles)
        {
            // update infor connect with file changes
            //uploadedFiles - deserialize
            try
            {                
            JsonSettings jsonSettings = new();
            string inforConnectionText = File.ReadAllText($"./Uploaded/{selectedFiles}");
            InforConnection? inforConnection = System.Text.Json.JsonSerializer.Deserialize<InforConnection>(inforConnectionText);
            jsonSettings.UpdateInforSettings(inforConnection);
            NavigationManager.NavigateTo("settings", true);
            }
            catch(Exception e)
            {
                Snackbar.Add("Error: " + e.Message + " Please check the file and try again.", severity: MudBlazor.Severity.Error );
            }
        }

        private async void DeleteFileSelect(string selectedFile)
        {
            try
            {
                File.Delete($"./Uploaded/{selectedFileToDelete}");
                NavigationManager.NavigateTo("settings", true);
            }
            catch(Exception e)
            {
                Snackbar.Add("Error: " + e.Message + " Please check the file and try again.", severity: MudBlazor.Severity.Error );
            }
        }

    private static string DefaultDragClass = "relative rounded-lg border-2 border-dashed pa-4 mt-4 mud-width-full mud-height-full z-10";
    private string DragClass = DefaultDragClass;
    private List<string> fileNames = new List<string>();
    IBrowserFile inputFile;

        private void OnInputFileChanged(InputFileChangeEventArgs e)
    {
        ClearDragClass();
        var files = e.File;
        
            fileNames.Add(files.Name);
            inputFile = files;
        }

    private async Task Clear()
    {
        fileNames.Clear();
        ClearDragClass();
        await Task.Delay(100);
    }
    private void Upload()
    {
        //Upload the files here
        //Snackbar.Configuration.PositionClass = Defaults.Classes.Position.TopCenter;
        Snackbar.Add("TODO: Upload your files!", severity: MudBlazor.Severity.Normal);
        
            browserFile = inputFile;
            CaptureFile();
            Clear();
            NavigationManager.NavigateTo("settings", true); // page wouldnt refresh, so forcing it so the new files will load. 
        }

        private void SetDragClass()
        {
            DragClass = $"{DefaultDragClass} mud-border-primary";
        }

        private void ClearDragClass()
        {
            DragClass = DefaultDragClass;
        }
         private async Task OpenDeleteFileDialog(string fileToDelete)
        {
            //var parameters = new MudBlazor.DialogParameters { ["file"]=fileToDelete };
            
            var dialog = await DialogService.ShowAsync<DeleteFileDialog>("Delete File");
            var result = await dialog.Result;

            if (!result.Canceled)
            {
                DeleteFileSelect(fileToDelete);
            }
        }
    }
}
