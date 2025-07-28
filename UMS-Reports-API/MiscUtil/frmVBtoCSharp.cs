using System.Text.RegularExpressions;

namespace MiscUtil
{
    public partial class frmVBtoCSharp : Form
    {
        public frmVBtoCSharp()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            RePaint();
        }
        private void frmMain_Resize(object sender, EventArgs e)
        {
            RePaint();
        }
        private void RePaint()
        {
            splitContainer3.SplitterDistance = splitContainer3.Width / 2;
            btnConvert.Left = (this.Width - btnConvert.Width) / 2;

            this.Refresh();
            Application.DoEvents();
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            ConvertVBtoCS();
        }

        private void ConvertVBtoCS()
        {
            string vbCode = txtSource.Text;

            // Replace simple VB constructs with C# equivalents
            string csCode = vbCode;

            foreach (var mapping in VBtoCSMappings())
            {
                csCode = csCode.Replace(mapping.Key, mapping.Value);
            }

            // Replace .Fields with Regex
            string pattern = @"\.Fields\(""(?<fieldName>[^""]+)""\)";
            csCode = Regex.Replace(csCode, pattern, m => $"dataRow[\"{m.Groups["fieldName"].Value}\"].ToString()");

            pattern = @"Iif\s*\(\s*(?<field1>[^,]+)\s*,\s*(?<field2>[^,]+)\s*,\s*(?<field3>[^\)]+)\s*\)";
            csCode = Regex.Replace(csCode, pattern, m => $"({m.Groups["field1"].Value.Trim()} ? {m.Groups["field2"].Value.Trim()} : {m.Groups["field3"].Value.Trim()})");

            pattern = @"pfRepl\s*\(\s*(?<character>\""[^\""]+\""|'.'|\w+)\s*,\s*(?<field>\w+)\s*\)";
            csCode = Regex.Replace(csCode, pattern, m => $"{m.Groups["character"].Value}.RepeatForLeftPadding({m.Groups["field"].Value})");


            csCode = Regex.Replace(csCode, @"If (.+) Then", "if ($1) {");
            csCode = Regex.Replace(csCode, @"As (\w+)", m => $"= new {TypeMapper(m.Groups[1].Value)}()");

            txtOutput.Text = csCode;
        }

        private string TypeMapper(string vbType)
        {
            switch (vbType.ToLower())
            {
                case "integer": return "int";
                case "string": return "string";
                case "double": return "double";
                // Add more type mappings as needed
                default: return vbType; // If not mapped, return the original type
            }
        }

        private Dictionary<string, string> VBtoCSMappings()
        {
            return new Dictionary<string, string>
            {
                {"Dim", "var"},
                {"Then", "{" },
                {"If gPrtRet = False Then GoTo EndReport:", "if (!printResult) goto endReport" },
                {"ElseIf", "else if" },
                {"End If", "}" },
                {"If", "if " },
                {"End Sub", "}" },
                {"End Function", "}" },
                {"Private Sub", "private void" },

                //{"", "" },
                {"strCentName", "center" },
                {"gPrtWidth", "printWidth" },
                {"gPrtLine", "printLine" },
                {"gPrtRet", "printResult" },
                {"pfPrint", "print.PrepareAndPrint" },
                //{"GoTo EndReport:", "goto endReport" },
                {"RptName", "reportName" },
                {"\r\n", ";\r\n" },
                {"PrtTotals(0)", "printTotals[0]" },
                {"PrtTotals(1)", "printTotals[1]" },
                {"PrtTotals(2)", "printTotals[2]" },
                {"PrtTotals(3)", "printTotals[3]"},
                {"gPrtSrNo", "printSrNo" },
                {"gstrVldtMesg", "validationMessage" },
                {"glnI", "gLnI" },
                {"pfPrStr", "print.FormatPrintString" },
                {" & ", " + " },
                {"'", "//" },
                {"Exit Sub", "return" },
                {"gPrtTotCtr", "printTotalCounter" },
                {"PrtGrps", "printGrps" },
                {"gPg4", "pageNumber" },
                {"strEmName","examName" },
                {"gstrSnYear","sessionYear" },
                {"intRptCode","reportCode" },
                {"strInst","instituteName" },
                {"Len(printLine)","printLine.Length" },
                {"gblSchool","isSchool" },
                {"gblInst","isInstitute" },
                {" Or "," || " },
                {"_;", ""},
                {"strHeldIn", "examHeldIn"},
                {"gstrSnName", "sessionName"},
                {"strTown", "town"},
                {"<>", "!="},
                {"strBreak", "pageBreak"},
                {"pfRepl(\"-\", printWidth)", "\"-\".RepeatForLeftPadding(printWidth)"},
                {"strRegnRoll", "registrationRoll"},
                {"intInst", "instituteId"},
                {"strAuthName", "authorityName"},
                {"strAuthDsgn", "authorityDesignation" },
                {"StrResultDate", "resultDate" },
                {"strBrName", "streamName" },
                {"Format(gdtSysDate, gDtFormat)", "systemDate.ToString(dateFormat)" },
                {"MDIMain.Caption", "reportName" },
                {"strTitle", "reportTitle" },
                {"strSubTitle", "reportSubTitle" },
                {"strOptions", "options.Coalesce()" },
                {"gregistrationRoll", "registrationRoll" },
                {"printGrps(", "printGrps[" }
            };
        }
    }
}