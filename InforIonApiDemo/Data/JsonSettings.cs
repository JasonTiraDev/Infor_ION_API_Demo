namespace InforIonApiDemo.Data
{
    public class JsonSettings
    {

        public InforConnectionRoot GetInforSettingsRoot()
        {
            // get inforConnection information
            string inforConnectionText = File.ReadAllText(@"./inforConnection.json");
            InforConnectionRoot? inforConnectionRoot = System.Text.Json.JsonSerializer.Deserialize<InforConnectionRoot>(inforConnectionText);
            var inforCon = inforConnectionRoot;

            return inforConnectionRoot;
        }
        public InforConnection GetInforSettings()
        {

            // get settings information
            string inforConnectionText = File.ReadAllText(@"./inforConnection.json");
            InforConnection? inforConnection = System.Text.Json.JsonSerializer.Deserialize<InforConnection>(inforConnectionText);

            return inforConnection;
        }
        // update infor connection settings
        public async Task UpdateInforSettings(InforConnection inforConnection)
        {
            var jsonSerial = JsonConvert.SerializeObject(inforConnection, Formatting.Indented);
            File.WriteAllText(@"./inforConnection.json", jsonSerial);

        }
        // get settings
        public ProgramSettingsModel GetProgramSettings()
        {
            // get settings information
            string programSettingsText = File.ReadAllText(@"./Settings.json");
            ProgramSettingsModel? programSettings = System.Text.Json.JsonSerializer.Deserialize<ProgramSettingsModel>(programSettingsText);
            var program = programSettings;

            return programSettings;
        }
        // update settings
        public async Task UpdateProgramSettings(ProgramSettingsModel programSettings)
        {
            var jsonSerial = JsonConvert.SerializeObject(programSettings, Formatting.Indented);
            File.WriteAllText(@"./Settings.json", jsonSerial);

        }
    }
}

