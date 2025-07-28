using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PdfSharp;

namespace UMSReportsAPI.BO
{
    public class PrintSettings
    {
        public PageSize PageSize { get; set; }
        public PageOrientation PageOrientation { get; set; }
        public int LeftMargin { get; set; }
        public int RightMargin { get; set; }
        public int TopMargin { get; set; }
        public int BottomMargin { get; set;}
        public string ReportHeader{ get; set; }
        public string ReportFooter { get; set; }
        public string PageHeader { get; set; }
        public string PageFooter { get; set; }
        public bool PageNumbering { get; set; }
        public int FontSize { get; set; }

        public PrintSettings() {
            this.PageSize = PageSize.A4;
            this.PageOrientation = PageOrientation.Portrait;
            this.PageNumbering = true;
            this.ReportHeader = string.Empty;
            this.ReportFooter = string.Empty;
            this.PageHeader = string.Empty; 
            this.PageFooter = string.Empty;
            this.TopMargin = 50;
            this.BottomMargin = 50;
            this.LeftMargin = 50;
            this.RightMargin = 50;
            this.FontSize = 8;
        }
    }
}