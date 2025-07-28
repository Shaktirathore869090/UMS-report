using System;
using System.IO;
using Newtonsoft.Json;

namespace MiscUtil
{
    public partial class App : Form
    {
        public App()
        {
            InitializeComponent();
        }

        private void btnJsonPayload_Click(object sender, EventArgs e)
        {
            int numberOfFiles = 4440; // Set the number of JSON files you want to create
            Random random = new Random();
            int counter = 0;
            for (int fileCounter = 1; fileCounter <= numberOfFiles; fileCounter++)
            {
                var marksData = new List<Dictionary<string, object>>();

                for (int id = 1; id <= 48; id++)
                {
                    counter++;
                    marksData.Add(new Dictionary<string, object>
                    {
                        ["id"] = counter,
                        ["Marks"] = random.Next(1, 21).ToString("00"),
                        ["VaildYN"] = null,
                        ["SystemErr"] = null,
                        ["UpdatedBy"] = "12"
                    });
                }

                var payload = new
                {
                    marks = marksData,
                    MakerChecker = "M",
                    MakrAppFlag = "F"
                };

                string json = JsonConvert.SerializeObject(payload, Formatting.Indented);
                string folderPath = Path.Combine(Application.StartupPath, "json");
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);
                string filePath = Path.Combine(folderPath, $"payload-{fileCounter}.json");
                File.WriteAllText(filePath, json);
            }
            MessageBox.Show("Done");
        }

        private void btnVBtoCSharp_Click(object sender, EventArgs e)
        {
            frmVBtoCSharp frm = new frmVBtoCSharp();
            frm.ShowDialog();
        }

        private void btnDocX_Click(object sender, EventArgs e)
        {
            frmDocXReporter frm = new frmDocXReporter();
            frm.ShowDialog();
        }
    }
}
