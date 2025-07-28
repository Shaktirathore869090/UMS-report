using Antlr.Runtime;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI.WebControls;
using UMSReportsAPI.Extenders;

namespace UMSReportsAPI.BO
{
    public class Print
    {
        public const string _page = "~~Pg";
        public bool pageSkipped = false;
        public string printFileName = "";
        public string reportPath = "";
        int printMode = 2;

        List<string> reportLines;
        List<string> headerLines;
        List<int> skipHeaderLines;
        int maxHeaderLines;
        List<string> footerLines;
        List<int> skipFooterLines;
        int maxFooterLines;
        int totalFooterLines;
        int lineCount;
        int pageCount;

        int printLeft;
        int printTop;
        int printBottom;
        int maxLines;

        string errorMessage;

        int i, j;

        public Print()
        {
            reportLines = new List<string>();
            headerLines = new List<string>();
            skipHeaderLines = new List<int>();
            maxHeaderLines = 0;
            footerLines = new List<string>();
            skipFooterLines = new List<int>();
            maxFooterLines = 0;
            lineCount = 0;
            pageCount = 0;
            printLeft = 0;
            printTop = 0;
            printBottom = 0;
            maxLines = 0;
            errorMessage = string.Empty;

        }
        public bool PrepareAndPrint(int functionType, int skipCount, string dataLine)
        {
            try
            {
                //FileSystemObject fileSystemObject = new FileSystemObject();
                //File file = new File();
                //TextStream textStream;
                if (reportLines == null || reportLines.Count <= 0)
                    reportLines = new List<string>();

                int printLeftPosition;
                int printTopPosition;
                int printBottomPosition;



                bool success = true;
                errorMessage = "";

                //   0 Initialise        Width               ReportName(50) + Path(50)
                //                       1/2 -Print / file

                if (functionType == 0)
                {
                    if (skipCount == 1 || skipCount == 2)
                    {
                        printFileName = dataLine;
                        printMode = skipCount;
                    }
                    else
                    {
                        printFileName = dataLine;
                        //frmPrintOptions.ReportSize = skipCount;
                        //frmPrintOptions.ShowDialog();
                        //if (gintPrintMode == 0)
                        //{
                        //    pfPrintErrMesg = "Printing Cancelled";
                        //    goto pfPrintError;
                        //}
                    }

                    //if (gintPrintMode == 1)
                    //{
                    //    intPrintLeft = 6;
                    //    intPrintTop = 3;
                    //    intPrintBottom = 3;
                    //    Printer.CurrentY = 0;
                    //    Printer.Print("");
                    //    intMaxLines = Convert.ToInt32(Printer.Height / Printer.CurrentY);
                    //    Printer.CurrentY = 0;
                    //}
                    //else
                    //{
                    printLeft = 0;
                    printTop = 0;
                    printBottom = 0;
                    maxLines = 66;
                    //fso = new FileSystemObject();
                    //fso.CreateTextFile(gstrPrintFileName, true);
                    //txtfile = fso.GetFile(gstrPrintFileName);
                    //ts = txtfile.OpenAsTextStream(ForWriting);
                    //}
                    pageCount = 1;
                    lineCount = 0;
                    InitializeHeaders(functionType);
                }

                //   100 init headers
                //   101,102.. header 1,2..  SkipLines           Header
                if (functionType.ToString().StartsWith("1"))
                {
                    if (functionType == 100)
                    {
                        InitializeHeaders(functionType);
                    }
                    else if (functionType < 100 || functionType > 160)
                    {
                        errorMessage = "Invalid Header Lines";
                        PrintError();
                    }
                    else
                    {
                        i = functionType - 100;
                        if (i > maxHeaderLines)
                            maxHeaderLines = i;
                        if (headerLines.Count < i)
                            headerLines.Add(dataLine);
                        else
                            headerLines[i - 1] = dataLine;

                        if (skipHeaderLines.Count < i)
                            skipHeaderLines.Add(skipCount);
                        else
                            skipHeaderLines[i - 1] = skipCount;
                    }
                }
                //
                //   2 NewPage/Header    0 - Print Headers
                //                       1-59 - Check balance line for NewPage & Header
                //                       > 59 - NewPage & Header
                if (functionType == 2)
                {
                    pageSkipped = false;
                    if (skipCount == 0)
                        Header(functionType, dataLine);
                    if (skipCount < 60 && maxLines - (lineCount + totalFooterLines + 6) >= skipCount)
                    {
                        return true;
                    }
                    NewPage(functionType, dataLine);
                }
                //
                //   3 Print detail      SkipLines           Details
                if (functionType == 3)
                {
                    pageSkipped = false;
                    if (maxLines - (lineCount + skipCount + totalFooterLines + printBottom) <= 0)
                        NewPage(functionType, dataLine);
                    if (skipCount > 0)
                        WriteBlankLines(skipCount);
                    lineCount += skipCount;
                    Detail(dataLine);
                }
                //
                //   4 SET Print Margins Value               L-Left 6, T-Top 0, B-Bottom 3
                if (functionType == 4)
                {
                    //Not applicable for Web Application
                }
                //
                //   5 Text MaxLines     Value               Def 66
                if (functionType == 5)
                {
                    maxLines = skipCount;
                    return true;
                }
                //
                //   600 init Footers    SkipLines           Footer
                //   601,602.. Footer 1,2..
                if (functionType.ToString().SubstringWithCorrection(0, 1) == "6")
                {
                    if (functionType == 600)
                        InitializeFooter();
                    if (functionType > 660)
                    {
                        errorMessage = "Invalid Footer Lines";
                        PrintError();
                        return false;
                    }
                    i = functionType - 600;
                    if (i > maxFooterLines)
                        maxFooterLines = i;
                    if (footerLines.Count < i)
                        footerLines.Add(dataLine);
                    if (skipFooterLines.Count < i)
                        skipFooterLines.Add(skipCount);
                    totalFooterLines = 0;
                    for (i = 0; i < maxFooterLines; i++)
                    {
                        totalFooterLines += 1 + skipFooterLines[i];
                    }
                }
                //
                //   7 Adjust Controls       to New Number      Headers / Footers / PageCount
                if (functionType == 7)
                {
                    switch (dataLine)
                    {
                        case "H":
                            maxHeaderLines = skipCount;
                            break;
                        case "F":
                            maxFooterLines = skipCount;
                            totalFooterLines = 0;
                            for (i = 0; i < maxFooterLines; i++)
                            {
                                totalFooterLines += 1 + skipFooterLines[i];
                            }
                            break;
                        case "P":
                            pageCount = skipCount;
                            break;
                    }
                }
                //
                //   9 EndDoc            0 EndDoc            Show meaasge
                //                       1 Cancel Print
                if (functionType == 9)
                {
                    if (skipCount == 1)
                    {
                        WriteReport();
                        return true;
                    }
                    NewPage(functionType, dataLine);
                }
                return success;
            }
            catch (Exception ex)
            {
                // Handle exceptions and errors.
                return false;
            }
        }
        private void WriteReport()
        {
            string filePath = Path.Combine(reportPath, printFileName);
            File.WriteAllLines(printFileName, reportLines, System.Text.Encoding.UTF8);
        }
        private void InitializeHeaders(int functionType)
        {
            headerLines = new List<string>();
            skipHeaderLines = new List<int>();
            maxHeaderLines = 0;

            if (functionType == 0)
                InitializeFooter();
        }
        private void Header(int functionType, string dataLine)
        {
            lineCount = printTop;
            if (maxHeaderLines > 0)
            {
                //for (i = 1; i <= maxHeaderLines; i++)
                for (i = 1; i <= maxHeaderLines + 1; i++)
                {
                    if (skipHeaderLines.Count > i - 1 && skipHeaderLines[i - 1] > 0)
                    {
                        WriteBlankLines(skipHeaderLines[i - 1]);
                        lineCount += skipHeaderLines[i - 1];
                    }
                    string tempString = headerLines.Count > i - 1 ? headerLines[i - 1] : "";
                    j = tempString.IndexOf(_page) + 1;
                    if (j > 0)
                    {
                        while (j > 0)
                        {
                            tempString = tempString.SubstringWithCorrection(0, j - 1) + FormatPrintString(pageCount, 4) + tempString.SubstringWithCorrection(j + 4, 200);
                            j = tempString.IndexOf(_page) + 1;
                        }
                    }
                    if (!tempString.IsNullOrWhiteSpace())
                    {
                        reportLines.Add(tempString);
                        lineCount++;
                    }
                }
            }
            //if (functionType == 3)
            //    ReportBody(dataLine);
        }
        private void ReportBody(string dataLine)
        {
            reportLines.Add(dataLine);
            lineCount++;
        }
        private void InitializeFooter()
        {
            footerLines = new List<string>();
            skipFooterLines = new List<int>();
            maxFooterLines = 0;
            totalFooterLines = 0;
        }
        private void NewPage(int functionType, string dataLine)
        {
            if (footerLines.Count > 0)
            {
                i = maxFooterLines - lineCount - totalFooterLines - printBottom - 1;
                if (i > 0)
                {
                    WriteBlankLines(i);
                }
            }
            for (i = 1; i <= footerLines.Count; i++)
            {
                if (skipFooterLines[i - 1] > 0)
                {
                    WriteBlankLines(skipFooterLines[i - 1]);
                }
            }
            string tempString = footerLines.Count > 0 && footerLines.Count >= i ? footerLines[i - 1] : "";
            j = tempString.IndexOf(_page);
            if (j > 0)
            {
                while (j > 0)
                {
                    tempString = tempString.SubstringWithCorrection(0, j) + FormatPrintString(pageCount, 4) + tempString.SubstringWithCorrection(j + 4, 200); ;
                }
            }
            reportLines.Add(tempString);
            if (functionType == 2 || functionType == 3)
            {
                // Manual Page Break in WordPad
                reportLines.Add("^m");
                pageSkipped = true;
                pageCount++;
                lineCount = 0;
                Header(functionType, dataLine);
            }
            if (functionType == 9)
            {
                WriteReport();
            }
        }
        private void Detail(string dataLine)
        {
            reportLines.Add(dataLine);
            lineCount++;
        }
        private void WriteBlankLines(int noOfLines)
        {
            for (int p = 0; p < noOfLines; p++)
            {
                reportLines.Add("");
            }
        }
        public string FormatPrintString(object dataField, int width, int dataLength = 0, string decimalFormat = "0", bool rightAlign = false)
        {
            try
            {
                int decimalPrecision;
                bool includeZero;
                string numberFormat;
                int i;

                switch (Type.GetTypeCode(dataField.GetType()))
                {
                    case TypeCode.String:
                        if (width >= 0)
                        {
                            if (dataLength == 0)
                            {
                                return (dataField.ToString() + new string(' ', width)).SubstringWithCorrection(0, width);
                            }
                            else
                            {
                                if (rightAlign)
                                {
                                    string tmp = (dataField.ToString().SubstringWithCorrection(0, dataLength) + new string(' ', width)).SubstringWithCorrection(0, width);
                                    int whiteSpace = tmp.Length - tmp.TrimEnd().Length;
                                    if (whiteSpace > 0)
                                        return new string(' ', whiteSpace) + tmp.TrimEnd();
                                    else
                                        return tmp;
                                }
                                else
                                    return (dataField.ToString().SubstringWithCorrection(0, dataLength) + new string(' ', width)).SubstringWithCorrection(0, width);
                            }
                        }
                        break;

                    case TypeCode.Decimal:
                    case TypeCode.Double:
                    case TypeCode.Single:
                    case TypeCode.Int16:
                    case TypeCode.Int32:
                    case TypeCode.Int64:
                        decimalPrecision = int.Parse(decimalFormat);
                        includeZero = (decimalFormat.Contains("z") || decimalFormat.Contains("Z"));
                        numberFormat = (decimalFormat.Contains(",")) ? "####,##,##,##0" : "#0";

                        dataField = Math.Round(Convert.ToDecimal(dataField), decimalPrecision);
                        if (includeZero && Convert.ToDecimal(dataField) == 0)
                        {
                            return new string(' ', width);
                        }
                        else
                        {
                            string result = "";
                            if (decimalPrecision > 0)
                            {
                                numberFormat += ".";
                                for (i = 1; i <= decimalPrecision; i++)
                                {
                                    numberFormat += "0";
                                }
                            }
                            if (dataLength == 0)
                            {
                                result = (new string(' ', width) + string.Format("{0:" + numberFormat + "}", dataField)).Substring(Math.Max(0, ((new string(' ', width) + string.Format("{0:" + numberFormat + "}", dataField)).Length) - width));
                            }
                            else
                            {
                                result = (string.Format("{0:" + numberFormat + "}", dataField).SubstringWithCorrection(0, dataLength) + new string(' ', width)).SubstringWithCorrection(0, width);
                            }
                            if (rightAlign)
                                return RightAlign(result, width, dataLength);
                            else
                                return result;
                        }

                    case TypeCode.DateTime:
                        if (Convert.ToDateTime(dataField) == DateTime.MinValue)
                        {
                            return new string(' ', width);
                        }
                        else if (dataLength == 8)
                        {
                            return (Convert.ToDateTime(dataField).ToString("dd/MM/yy") + new string(' ', width)).SubstringWithCorrection(0, width);
                        }
                        else
                        {
                            if (dataLength == 0)
                            {
                                dataLength = 10;
                            }
                            return (Convert.ToDateTime(dataField).ToString("dd/MM/yyyy").SubstringWithCorrection(0, dataLength) + new string(' ', width)).SubstringWithCorrection(0, width);
                        }

                    default:
                        return "";
                }
            }
            catch (Exception ex)
            {
                // Handle error if needed
            }
            return "";
        }
        private void PrintError()
        {
            Console.WriteLine(errorMessage);
        }

        public static string RightAlign( string value, int width, int dataLength = 0)
        {
            string tmp = (value.ToString().SubstringWithCorrection(0, dataLength) + new string(' ', width)).SubstringWithCorrection(0, width);
            int whiteSpace = tmp.Length - tmp.TrimEnd().Length;
            if (whiteSpace > 0)
                return new string(' ', whiteSpace) + tmp.TrimEnd();
            else
                return tmp;
        }
        public string[] ReadReport()
        {
            return reportLines.ToArray();
        }

        internal string AlignAndPrint(string printLine, int printWidth, TextAlignment textAlignment)
        {
            string result = printLine;
            switch (textAlignment)
            {
                case TextAlignment.Center:
                    result = FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
                    break;
            }
            return result;
        }

        public List<string> BreakString(string wholeString, int length, int parts)
        {
            List<string> strList = new List<string>();

            try
            {
                for (int i = 0; i < parts; i++)
                {
                    string currentPart = "";
                    while (wholeString.Length > 0)
                    {
                        if (wholeString.Length <= length - currentPart.Length)
                        {
                            currentPart += wholeString;
                            wholeString = "";
                        }
                        else
                        {
                            int k = wholeString.IndexOf(" ");
                            if (k != -1 && k <= length - currentPart.Length)
                            {
                                currentPart += wholeString.Substring(0, k + 1);
                                wholeString = wholeString.Substring(k + 1);
                            }
                            else
                            {
                                if (currentPart == "")
                                {
                                    currentPart = wholeString.Substring(0, length);
                                    wholeString = wholeString.Substring(length);
                                }
                                break;
                            }
                        }
                    }
                    strList.Add(currentPart);
                }
            }
            catch (Exception ex)
            {

            }

            return strList;
        }
    }

    public enum TextAlignment
    {
        Center,
        Left,
        Right
    }
}