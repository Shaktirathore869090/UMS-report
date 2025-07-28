using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using UMSReportsAPI.DAL;
using UMSReportsAPI.Extenders;

using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json.Linq;

using log4net;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.Net.Http.Headers;
using System.Data.SqlClient;
using log4net.Util;
using System.Text.RegularExpressions;
using System.Linq;
using CrystalDecisions.ReportAppServer.ClientDoc;
using UMSReportsAPI.BO;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.VisualBasic.Devices;
using System.Drawing;
using CrystalDecisions.Web.HtmlReportRender;
using CrystalDecisions.ReportAppServer.Controllers;
using UMSReportsAPI.Utilities;
using System.Web.Http.Results;
using Swashbuckle.Swagger;
using Microsoft.Extensions.Options;
using System.Drawing.Drawing2D;
using System.Runtime.Remoting.Messaging;
using System.Collections;
using System.Runtime.CompilerServices;
using WebGrease.Activities;
using CrystalDecisions.Shared.Json;


using CrystalDecisions.ReportAppServer.DataDefModel;

using System.Web.Mvc;
using DataSet = System.Data.DataSet;
using Table = CrystalDecisions.CrystalReports.Engine.Table;
using ParameterField = CrystalDecisions.Shared.ParameterField;

//using CrystalDecisions.ReportAppServer.DataDefModel;
//using CrystalDecisions.ReportAppServer.ReportDefModel;

namespace UMSReportsAPI.BO
{
    public class LegacyReports
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(LegacyReports));

        private string _reportPath = "";
        private string dateFormat = "dd/MM/yyyy";
        int gLnI = 0;
        string validationMessage = "";
        bool cancelPrintJob = false;
        int printTotalCounter = 0;      // gPrtTotCtr
        DateTime systemDate;
        //int scheduleId = 0;
        //int branch = 0;
        //string center = "";
        //string dsCode = "";

        int loginId;
        int reportCode;
        string reportType;
        string reportName;              // RptName
        string reportTitle;             // strTitle
        string reportSubTitle;          // strSubTitle
        string options;                 // strOptions
        string footer;                  // strFooter
        string reportPath;
        string reportStoredProcedure;
        string reportImage;             // RptImg
        string logoPath;                // Logo Path
        string instituteName;           // strInst
        string institutePlace;
        int instituteId;                // intInst
        string instituteCode;           // institute Code
        string registrationRoll;        // gstrRegnRoll
        int scheduleId;                 // EcShId
        string collegeCode;
        int rollNo;
        string center;
        string tnCode;
        int esId;                       // lnEsId
        int csNo;
        string pageBreak;
        int streamId;
        string streamCode;
        string registartionNo;
        string uc;
        string ledger;
        string examCode;
        string examName;
        string examShortName;
        string examMarkList;
        string examEvaluation;
        string imagePath;
        int medium;
        string resultDate;          // StrResultDate
        string examHeldIn;          // strHeldIn
        string examDate;            // strEmdate
        string examNo;
        int examSrNo;
        string authorityName;
        string authorityDesignation;
        string dsCode;
        string courseCode;
        int srNo;
        string extra1;
        string examCourseCode;
        string examCourseName;
        //int DivPc;

        int sessionNo;
        string cnCpde;
        string loginName;
        string loginAccess;
        int loginLevel;
        string sessionYear;         // gstrSnYear
        string sessionCode;         // gstrSnCode
        string sessionName;
        string sessionStatus;
        string streamName;          // streamName (branch name)

        string rollNoPrefix;        // strShPrefix

        string connectionCode;      // CnCode
        string examFormShow;        // EfShow
        string branchName;          // StrName
        string town;                // StrTown

        string coursePattern;       // strCoPattern
        int scheduleExamId;         // lnEmId

        bool isSchool;              // gblSchool
        bool isInstitute;           // gblInst
        bool isTownAvailable;       // gblTowns

        bool dibrugarhCovidFlag;  // strDibCovidFlag

        string rep;                 // strRep
        int passFail;               // intPassFail
        int footerSize;             // intFooterSize
        int centerCollege;          // intCentCg

        string singleLine;          // strSingleLine
        string details;             // strDetails

        int inCode;                 // In Code
        string change;              // Change
        int ecId;                   // EcId

        string subjectCode;         // pfEditSubCode
        string headCode;            // pfEditHdCode
        string editFlag;            // pfEditFlag
        string editCaCent;          // pfEditCaCent
        string editCandidate;       // PfEdit_Cand
        string editApplication;     // pfEditApp
        string editChange;     // pfEditChng
        string editValid;      // pfEditValid
        int registrationId;         // Student Registration Id
        int examId;                 // Exam Id

        string resultType = "";     // ResultType from Extra1
        string paper = "";          // Paper from Extra1 (a.k.a. EsCode)

        int studentId = 0;          // Student Id

        public HttpResponseMessage PrintReport(int _loginId = 0, string _reportPath = "", JObject filters = null)
        {
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.InternalServerError);

            //if (filters == null || !filters["test"].Coalesce().ToBoolean())
            //{
            //    result = PrintLineReport(Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath(_reportPath), "test.txt"));
            //    return result;
            //}

            try
            {
                loginId = _loginId;
                reportCode = 0;
                reportType = "";
                reportName = "";
                reportPath = _reportPath;
                reportStoredProcedure = "";
                reportImage = "";
                instituteName = "";
                institutePlace = "";
                instituteId = 0;
                instituteCode = "";
                registrationRoll = "";
                scheduleId = 0;
                collegeCode = "";
                rollNo = 0;
                center = "";
                tnCode = "";
                esId = 0;
                csNo = 0;
                pageBreak = "";
                streamId = 0;       // branchId
                streamCode = "";    // branchCode
                registartionNo = "";
                uc = "U";
                ledger = "";
                examCode = "";
                examName = "";
                examSrNo = 0;
                examShortName = "";
                examMarkList = "";
                examEvaluation = "";
                imagePath = "";
                medium = 0;
                resultDate = "";
                examHeldIn = "";
                examNo = "";
                authorityName = "";
                authorityDesignation = "";
                dsCode = "";
                courseCode = "";
                srNo = 0;
                extra1 = "";
                examCourseCode = "";
                examCourseName = "";

                sessionNo = 0;
                cnCpde = "";
                loginName = "";
                loginAccess = "";
                loginLevel = 0;
                sessionYear = "";
                sessionCode = "";
                sessionName = "";
                sessionStatus = "";
                streamName = "";

                rollNoPrefix = "";      // strShPrefix - SRTM - Prefix for RollNo

                connectionCode = "";
                examFormShow = "";
                branchName = "";
                town = "";

                inCode = 0;
                change = "";
                ecId = 0;

                subjectCode = "";
                headCode = "";
                editFlag = "";
                editCaCent = "";
                editCandidate = "";
                options = "";
                footer = "   ****End of Report****";
                editApplication = "";
                editValid = "";
                editChange = "";
                //DivPc = 0;

                resultType = "";

                LegacyReportFacade reportFacade = new LegacyReportFacade(filters);
                reportFacade.GetReportParameters(loginId, ref reportCode, ref reportType, ref reportName,
                    ref reportStoredProcedure, ref reportImage, ref instituteName, ref instituteId,
                    ref registrationRoll, ref scheduleId, ref collegeCode, ref rollNo, ref center,
                    ref tnCode, ref esId, ref csNo, ref pageBreak, ref streamId, ref streamCode,
                    ref registartionNo, ref uc, ref ledger, ref examCode, ref examName,
                    ref examShortName, ref examMarkList, ref examEvaluation, ref imagePath, ref medium,
                    ref resultDate, ref examHeldIn, ref examNo, ref authorityName, ref authorityDesignation,
                    ref dsCode, ref courseCode, ref sessionNo, ref extra1, ref examCourseCode, ref examCourseName, ref institutePlace, ref instituteCode);

                JObject json = null;
                try
                {
                    json = JObject.Parse(extra1);
                }
                catch { }
                if (json != null)
                {
                    try { resultType = json.SelectToken("ResultType").Coalesce(); } catch { }
                    try { centerCollege = json.SelectToken("CenterCollege").Coalesce("1").ToInt32(); } catch { }

                    try { studentId = json.SelectToken("StuId").Coalesce("0").ToInt32(); } catch { }

                }

                // Golbal Parameters
                reportFacade.InitializeSystemParameters(loginId, sessionNo, streamId, ref systemDate
                    , ref connectionCode, ref loginName, ref loginAccess, ref loginLevel, ref sessionYear
                    , ref sessionCode, ref sessionName, ref sessionStatus, ref examFormShow, ref branchName, ref town);

                // Misc parameters
                reportFacade.InitializeMiscParameters(courseCode, scheduleId, instituteId, streamId
                    , ref coursePattern, ref scheduleExamId, ref resultDate, ref examHeldIn, ref rollNoPrefix, ref examDate, ref examSrNo);

                isSchool = examFormShow.IndexOf("S") == 0;
                isInstitute = examFormShow.IndexOf("I") == 0;
                isTownAvailable = examFormShow != "N";


                if (isSchool || isInstitute)
                    reportName = reportName.Replace("College", "Institute");

                // Dibrugarh Reguar 2020 (Covid-19) session - Exam/Calc marks flag
                if ((instituteId == 3 || instituteId == 18) & sessionYear == "2020" && sessionCode == "R")
                    dibrugarhCovidFlag = true;
                else
                    dibrugarhCovidFlag = false;

                log.Debug("ReportCode: " + reportCode);
                string reportFileName = GetReportFileName(reportCode, filters);

                switch (reportCode)
                {
                    // Centre/Colg List
                    case 2:
                    case 21:
                        if (instituteId == 8)
                            result = PrintCenterSchoolList(filters);
                        else
                            result = PrintCollageCenterList(filters);
                        break;
                    // Pre Assesment - Paper Count
                    case 3:
                        result = PreparePaperCount(filters);
                        break;
                    // Pre - Colg/Centre RollList
                    case 4:
                    case 8:
                        switch (instituteId)
                        {
                            case 5:
                                result = PrintRollList_5(filters);
                                break;
                            case 6:
                                result = PrintRollList_6(filters);
                                break;
                            case 9:
                                result = PrintCrystalReport("RollList_9.Rpt", null, filters);
                                break;
                            case 11:
                                result = PrintRollList_11(filters);
                                break;
                            case 13:
                                result = PrintRollList_13(filters);
                                break;
                            case 16:
                                result = PrintRollList_16(filters);
                                break;
                            default:
                                result = PrintRollList(filters);
                                break;
                        }
                        break;
                    // Pre - Admit card
                    case 5:
                        Print print = new Print();
                        if (!reportImage.IsNullOrWhiteSpace() && "2,5,6,9,14,15,17,19".ToCSV().Contains(instituteId.ToString()))
                        {
                            reportFacade.PrepareAdmitCard("Rpt_AdmitCardsI", loginId, scheduleId, collegeCode, rollNo, registartionNo, instituteId, center, streamId, "Y");
                            reportFacade.PrepareImages(loginId, reportImage, imagePath);
                        }
                        else if (!reportImage.IsNullOrWhiteSpace() && instituteId == 11)
                        {
                            reportFacade.PrepareAdmitCard("rpt_AdmitCardsI_11", loginId, scheduleId, collegeCode, rollNo, registartionNo, instituteId, center, streamId, "Y");
                            reportFacade.PrepareImages(loginId, reportImage, imagePath);
                        }
                        switch (instituteId)
                        {
                            //case 2:
                            //    result = PrintCrystalReport("AdmitCardI_02.Rpt", null, filters);
                            //    break;
                            //case 5:
                            //    result = PrintCrystalReport("AdmitCardI_05.Rpt", null, filters);
                            //    break;
                            //case 6:
                            //    result = PrintCrystalReport("AdmitCardI_06.Rpt", null, filters);
                            //    break;
                            //case 9:
                            //    result = PrintCrystalReport("AdmitCardI_9.Rpt", null, filters);
                            //    break;
                            //case 14:
                            //case 19:
                            //    result = PrintCrystalReport("AdmitCardI_14.Rpt", null, filters);
                            //    break;
                            //case 15:
                            //    result = PrintCrystalReport("AdmitCardI_15.Rpt", null, filters);
                            //    break;
                            //case 17:
                            //    result = PrintCrystalReport("AdmitCardI_17.Rpt", null, filters);
                            //    break;
                            //case 3:
                            //case 18:
                            //    //throw new NotImplementedException();
                            //    result = PrintCrystalReport("AdmitCardsDib.Rpt", null, filters);
                            //    break;
                            //case 8:
                            //    PrintAdmitCard_8();
                            //    break;
                            //case 11:
                            //    throw new NotImplementedException();
                            //    if (1 == 2)
                            //        result = PrintCrystalReport("AdmitCardI_11_A4.Rpt", null, filters);
                            //    else
                            //        result = PrintCrystalReport("AdmitCardI_11_B5.Rpt", null, filters);
                            //    break;
                            //case 13:
                            //    PrintAdmitCard_13(filters);
                            //    break;
                            //case 16:
                            //    PrintAdmitCard_16(filters);
                            //    break;
                            default:
                                result = PrintCrystalReport("AdmitCardI_Generic.rpt", null, filters);
                                break;
                        }
                        reportFacade.ClearImages(loginId);
                        break;
                    // Post - Colg/Cent Result Summary
                    case 6:
                    case 16:
                        switch (instituteId)
                        {
                            case 2:
                                if (reportCode == 6)
                                    PrintCollageSummary_02(filters);
                                else
                                    PrintSummary(filters);
                                break;
                            case 3:
                                result = PrintCrystalReport("Summary_DIB.Rpt", null, filters);
                                break;
                            case 9:
                                result = PrintCrystalReport("Summary_DMI.Rpt", null, filters);
                                break;
                            case 14:
                            case 19:
                                result = PrintCenterSummary_14(filters);
                                break;
                            default:
                                result = PrintSummary(filters);
                                break;
                        }
                        break;
                    // Pre/Post - Colg/Cent dispatch list - not used
                    case 7:
                    case 12:
                        result = PrintPreDispatch(filters);
                        break;
                    // Pre - Centre Paper list
                    case 9:
                        if (instituteId == 16)
                            result = PrintCenterPaper_16(filters);
                        else
                            result = PrintCenterPaper(filters);
                        break;
                    // Pre - Attendance Sheet
                    case 10:
                        switch (instituteId)
                        {
                            case 6:
                            case 9:
                            case 17:
                                if (!reportImage.IsNullOrWhiteSpace())
                                {
                                    reportFacade.PrepareAdmitCard("Rpt_AdmitCardsI", loginId, scheduleId, collegeCode, rollNo, registartionNo, instituteId, center, streamId, "Y");
                                    reportFacade.PrepareImages(loginId, reportImage, imagePath);
                                }
                                if (instituteId == 6)
                                    result = PrintCrystalReport("AttenSheetI.Rpt", null, filters);
                                else if (instituteId == 9)
                                    result = PrintCrystalReport("AttenSheetIDMI.Rpt", null, filters);
                                else if (instituteId == 17)
                                    result = PrintCrystalReport("AttenSheetIKKH.Rpt", null, filters);
                                break;
                            case 5:
                                PrintAttendanceSheet_05(filters);
                                break;
                            case 3:
                            case 8:
                            case 15:
                                //result = PrintAttendanceSheetAll(filters);
                                if (!reportImage.IsNullOrWhiteSpace())
                                {
                                    reportFacade.PrepareAdmitCard("Rpt_AdmitCardsI", loginId, scheduleId, collegeCode, rollNo, registartionNo, instituteId, center, streamId, "Y");
                                    reportFacade.PrepareImages(loginId, reportImage, imagePath);
                                }
                                result = PrintCrystalReport("AttenSheetI.Rpt", null, filters);
                                break;
                            //case 14:
                            //case 19:
                            //    result = PrintCrystalReport("TopSheet_14.rpt", null, filters);
                            //    break;
                            //case 18:
                            //    if (uc.ToUpper() == "C")
                            //        result = PrintCrystalReport("AttenSheetDibD.rpt", null, filters);
                            //    else
                            //        result = PrintAttendanceSheetAll(filters);
                            //    break;
                            default:
                                result = PrintAttendanceSheetAll(filters);
                                break;
                        }
                        break;
                    // Pre- Packing List
                    case 11:
                        PrintPackingList(filters);
                        break;
                    // EditList - ExamForms (KLE)
                    case 13:
                        if (!reportImage.IsNullOrWhiteSpace())
                        {
                            reportFacade.PrepareExamForms(loginId, esId, collegeCode, "Y");
                            reportFacade.PrepareImages(loginId, "", imagePath);
                        }
                        switch (instituteId)
                        {
                            case 6:
                                result = PrintCrystalReport("ExamFormsI_06.rpt", null, filters);
                                break;
                            default:
                                result = PrintCrystalReport("ExamFormsI.rpt", null, filters);
                                break;
                        }
                        reportFacade.ClearImages(loginId);
                        break;
                    // Pre - Declaration (KLE)
                    case 14:
                        result = PrintCrystalReport("Declaration.rpt", null, filters);
                        break;
                    // Post - Colg/Cent Ledger
                    case 17:
                    case 31:
                        if (dibrugarhCovidFlag)
                        {
                            dsCode = filters["examMode"].ToString().Coalesce();
                            if (!"E,P".ToCSV().Contains(dsCode)) dsCode = "";
                        }
                        if (ledger == "S")
                        {
                            reportFileName = "";
                            int subjects = 0;

                            int centCollege = 0;
                            string colgCode = "";
                            if (reportCode == 17)
                            {
                                centCollege = 2;
                                colgCode = collegeCode;
                            }
                            else
                            {
                                centCollege = 1;
                                colgCode = center;
                            }

                            reportFacade.CrystalReportCheck(loginId, scheduleId, streamId, colgCode, centCollege, dsCode, instituteId, pageBreak);
                            DataSet dsReportHeaders = reportFacade.GetCrystalReportHeaders(loginId);
                            if (dsReportHeaders.HasData())
                                subjects = dsReportHeaders.Tables[0].Rows.Count;

                            if (subjects <= 16)
                                reportFileName = instituteId == 9 ? "Ledger_DMI.rpt" : (instituteId == 2 ? "Ledger_RTM.rpt" : "Ledger.rpt");
                            else if (subjects <= 31)
                                reportFileName = instituteId == 9 ? "Ledger1_DMI.rpt" : (instituteId == 2 ? "Ledger1_RTM.rpt" : "Ledger1.rpt");
                            else if (subjects <= 43)
                                reportFileName = instituteId == 9 ? "Ledger2_DMI.rpt" : (instituteId == 2 ? "Ledger2_RTM.rpt" : "Ledger2.rpt");
                            else
                                reportFileName = "Ledger2.rpt"; // Need to verify if all subjects are covering

                            //result = PrintCrystalReport("", dsReportHeaders, filters);
                            result = PrintCrystalReport(reportFileName, dsReportHeaders, filters);
                        }
                        else
                        {
                            if (examEvaluation == "GPA") rep = "GPA";
                            switch (instituteId)
                            {
                                case 5:
                                    result = PrintCrystalReport("Ledger3_LNM.rpt", null, filters);
                                    break;
                                case 6:
                                    result = PrintCrystalReport("Ledger3_KLE.rpt", null, filters);
                                    break;
                                    //case 9:
                                    //    throw new NotImplementedException();
                                    break;
                                case 11:
                                    result = PrintLedger_11(filters);
                                    break;
                                case 13:
                                    result = PrintLedger_13(filters);
                                    break;
                                case 14:
                                case 19:
                                    result = PrintLedger_14(filters);
                                    break;
                                case 16:
                                    result = PrintLedger_16(filters);
                                    break;
                                //case 17:
                                //    throw new NotImplementedException();
                                //    break;
                                default:
                                    //result = PrintCrystalReport("Ledger3.rpt", null, filters);
                                    result = PrintCrystalReport("Ledger3_Multiline_Generic.rpt", null, filters);
                                    break;
                            }
                        }
                        break;
                    //Post - MarkList
                    case 18:
                        switch (instituteId)
                        {
                            case 2:
                                return PrintCrystalReport("MarkLRTM.rpt", null, filters);
                                break;
                            case 3:
                            case 18:
                                if (dibrugarhCovidFlag)
                                {
                                    dsCode = filters["examMode"].ToString().Coalesce();
                                    if (!"E,P".ToCSV().Contains(dsCode)) dsCode = "";
                                }
                                if (examEvaluation.ToUpper().Equals("GPA"))
                                {
                                    reportFacade.PrepareMarkListImages(loginId, scheduleId, streamId, collegeCode, rollNo, registartionNo, pageBreak, instituteId);
                                    reportFacade.PrepareImages(loginId, reportImage, imagePath);
                                }
                                string stationary = filters["stationary"].Coalesce();
                                if (stationary.ToUpper().Equals("PLAIN"))
                                {
                                    if (examEvaluation.ToUpper().Equals("GPA"))
                                        result = PrintCrystalReport("MarkLDib_GPA_PDF.rpt", null, filters);
                                    else
                                        result = PrintCrystalReport("MarkLDib_PDF.rpt", null, filters);
                                }
                                else
                                {
                                    if (examEvaluation.ToUpper().Equals("GPA"))
                                        result = PrintCrystalReport("MarkLDib_GPA.rpt", null, filters);
                                    else
                                        result = PrintCrystalReport("MarkLDib.rpt", null, filters);
                                }
                                break;
                            case 5:
                                if (!reportImage.IsNullOrWhiteSpace())
                                {
                                    reportFacade.PrepareMarkListWithImages(loginId, scheduleId, streamId, collegeCode, rollNo, registartionNo, "", "Y", instituteId);
                                    reportFacade.PrepareImages(loginId, reportImage, imagePath);
                                }
                                result = PrintCrystalReport("MarkList_LNM.rpt", null, filters);
                                break;
                            case 6:
                                if (examEvaluation.ToUpper().Equals("GPA"))
                                    result = PrintCrystalReport("MarkLKLE_GPA.rpt", null, filters);
                                else if (examEvaluation.ToUpper().Equals("GPAM"))
                                    result = PrintCrystalReport("MarkList_GPA_and_M.rpt", null, filters);
                                else
                                {
                                    //// Commented on 2024-10-21 to fix  Medical Marklist error
                                    //if (examMarkList.ToUpper().Equals("MED"))
                                    //    result = PrintCrystalReport("CryMarkLKLE.rpt", null, filters);
                                    //else if (examMarkList.ToUpper().Equals("GEN") || examMarkList.IsNullOrWhiteSpace())
                                    //    //result = PrintCrystalReport("MarkLKLE.rpt", null, filters);
                                    result = PrintCrystalReport("MarkLKLE_Formatted.rpt", null, filters);

                                }
                                break;
                            case 8:
                                result = PrintMarkList_AHS(filters);
                                break;
                            case 9:
                                reportFacade.PrepareMarkListImages(loginId, scheduleId, streamId, collegeCode, rollNo, registartionNo, pageBreak, instituteId);
                                reportFacade.PrepareImages(loginId, reportImage, imagePath);
                                if (examCourseCode.ToUpper().Equals("MBBSN"))
                                    result = PrintCrystalReport("MarkLDMI_MBBS.rpt", null, filters);
                                else
                                {
                                    if (examMarkList.ToUpper().Equals("MED"))
                                        result = PrintCrystalReport("CryMarkLDMI.rpt", null, filters);
                                    else if (examMarkList.ToUpper().Equals("GEN") || examMarkList.ToUpper().Equals("PG"))
                                        result = PrintCrystalReport("MarkLDMI.rpt", null, filters);
                                    else if (examMarkList.ToUpper().Equals("GPA"))
                                        result = PrintCrystalReport("MarkLDMI_GPA.rpt", null, filters);
                                }
                                break;
                            case 11:
                                if (rollNo != 0 || pageBreak == "F")
                                    throw new NotImplementedException();
                                //All including not eligible
                                int _passFail = filters["includeAll"].Coalesce("1").ToInt32();
                                reportFileName = reportFacade.GetMarksheetReportFileName(examCourseCode);
                                string _reportType = examMarkList;

                                reportFacade.PrepareImages_RptLedgerCo(loginId, scheduleId, streamId, collegeCode, center, gLnI, instituteId, _passFail, pageBreak, _reportType, rollNo, "Y", dsCode);
                                reportFacade.PrepareImages(loginId, "P", imagePath);

                                if (!reportFileName.IsNullOrWhiteSpace())
                                    result = PrintCrystalReport(reportFileName, null, filters);
                                else
                                {
                                    if (examMarkList.ToUpper().Equals("EXIN"))
                                        result = PrintCrystalReport("MarkLNmuExIn.rpt", null, filters);
                                    else if (examMarkList.ToUpper().Equals("EXINP"))
                                        result = PrintCrystalReport("MarkLNmuExInP.rpt", null, filters);
                                    else if (examMarkList.ToUpper().Equals("GEN"))
                                        result = PrintCrystalReport("MarkLNmuCo.rpt", null, filters);
                                    else if (examMarkList.ToUpper().Equals("GPA"))
                                        if (srNo >= 19)
                                            result = PrintCrystalReport("MarkLNmu_GPAN.rpt", null, filters);
                                        else
                                            result = PrintCrystalReport("MarkLNmu_GPA.rpt", null, filters);
                                }
                                break;
                            case 13:
                                if (examEvaluation.ToUpper().Equals("GPA"))
                                {
                                    if (examMarkList.ToUpper().Equals("ME")) result = PrintMarkList_13GPA_ME(filters);
                                    else result = PrintMarkList_13GPA(filters);
                                }
                                else
                                    result = PrintMarkList_13(filters);
                                break;
                            case 14:
                                if (examCourseCode.ToUpper().Equals("MTECH") || examEvaluation.ToUpper().Equals("M"))
                                    result = PrintCrystalReport("MarkLTU_MTech.rpt", null, filters);
                                else if (examCourseCode.ToUpper().Equals("BSCN") || examCourseCode.ToUpper().Equals("PBNS"))
                                    result = PrintCrystalReport("MarkLTU.rpt", null, filters);
                                else
                                    result = PrintMarkList_14(filters);
                                break;
                            case 15:
                                result = PrintCrystalReport("MarkLTBSE.rpt", null, filters);
                                break;
                            case 16:
                                PrintMarkList_16(filters);
                                break;
                            case 17:
                                if (examEvaluation.ToUpper().Equals("GPA"))
                                    result = PrintCrystalReport("MarkLKLE_GPA.rpt", null, filters);
                                else if (examEvaluation.ToUpper().Equals("GPAM"))
                                    result = PrintCrystalReport("MarkList_GPA_and_M.rpt", null, filters);
                                else
                                {
                                    reportFacade.PrepareMarkListImages(loginId, scheduleId, streamId, collegeCode, rollNo, registartionNo, pageBreak, instituteId);
                                    reportFacade.PrepareImages(loginId, reportImage, imagePath);
                                    result = PrintCrystalReport("MarkLKKH_Formatted.rpt", null, filters);
                                }
                                break;
                            case 19:
                                if (examCourseCode.Equals("VET", StringComparison.OrdinalIgnoreCase))
                                    result = PrintCrystalReport("MarkLTU_Vet.rpt", null, filters);
                                else if (examCourseCode.Equals("MBBS", StringComparison.OrdinalIgnoreCase))
                                    result = PrintCrystalReport("MarkLTUMarks.rpt", null, filters);
                                else if (examCourseCode.Equals("BSCN", StringComparison.OrdinalIgnoreCase) || examCourseCode.Equals("PBNS", StringComparison.OrdinalIgnoreCase))
                                    result = PrintCrystalReport("MarkLTU.rpt", null, filters);
                                else
                                    result = PrintMarkList_14(filters);
                                break;
                        }
                        break;
                    // Post- Result declaration
                    case 20:
                        switch (instituteId)
                        {
                            case 2:
                                result = PrintCrystalReport("ResultRTM.rpt", null, filters);
                                break;
                            //case 3:
                            //    throw new NotImplementedException();
                            //    break;
                            case 3:
                            case 17:
                                //result = PrintResult_17(filters);
                                result = PrintCrystalReport("Result_v2.rpt", null, filters);
                                break;
                            case 6:
                                //result = PrintResult_06(filters);
                                result = PrintCrystalReport("Result_v2.rpt", null, filters);
                                break;
                            case 9:
                                result = PrintCrystalReport("ResultDMI.rpt", null, filters);
                                break;
                            case 11:
                                result = PrintResult_11(filters);
                                break;
                            case 13:
                                throw new NotImplementedException();
                                break;
                            case 14:
                            case 19:
                                throw new NotImplementedException();
                                break;
                            case 15:
                                throw new NotImplementedException();
                                break;
                            case 16:
                                throw new NotImplementedException();
                                break;
                            case 18:
                                throw new NotImplementedException();
                                break;
                        }
                        break;
                    // Pre- Paper Centre count
                    case 22:
                        result = PrintPaperCenter(filters);
                        break;
                    case 23:
                        result = PrintCenterPaperCount(filters);
                        break;
                    case 24:
                        switch (instituteId)
                        {
                            // Pre- Numeric analyses (RTM)
                            case 2:
                                result = PrintNumericAnalysis(filters);
                                break;
                            case 13:
                                result = PrintNumericAnalysis_13(filters);     //same as PrintCenterPaperCount
                                break;
                        }
                        break;
                    // Pre- No information
                    case 25:
                        result = NoInfo(filters);
                        break;
                    // Post- Pass Cenrtificates
                    case 28:
                        switch (instituteId)
                        {
                            //case 9:
                            //    result = PrintCrystalReport("PassCert_9.rpt", null, filters);
                            //    break;
                            //case 11:
                            //    throw new NotImplementedException();
                            //    break;
                            //case 13:
                            //    result = PrintPassCertificate_13(filters);
                            //    break;
                            //case 14:
                            //case 19:
                            //    result = PrintCrystalReport("PassCert_14.rpt", null, filters);
                            //    break;
                            //case 15:
                            //    result = PrintCrystalReport("PassCert_15.rpt", null, filters);
                            //    break;
                            //case 17:
                            //    result = PrintCrystalReport("PassCert_17.rpt", null, filters);
                            //    break;
                            default:
                                result = PrintCrystalReport("PassCert_Generic.rpt", null, filters);
                                break;
                        }
                        break;
                    // Post- Colg Result
                    case 29:
                        switch (instituteId)
                        {
                            //case 11:
                            //    result = PrintResult_11(filters);
                            //    break;
                            //case 14:
                            //case 19:
                            //    result = PrintResult_14(filters);
                            //    break;
                            //case 15:
                            //    result = PrintResult_15(filters);
                            //    break;
                            //case 17:
                            //    result = PrintResult_17_College(filters);
                            //    break;
                            default:
                                //result = PrintResult_College(filters);
                                result = PrintCrystalReport("Result_v2_Cg.rpt", null, filters);
                                break;
                        }
                        break;
                    // Grace List
                    case 30:
                        result = PrintGraceList(filters);
                        break;
                    case 32:
                        result = PrintCrystalReport("CtrSheet.rpt", null, filters);
                        break;
                    // Pre- Div of work (RTM)
                    case 33:
                        result = PrintDivisionOfWork(filters);
                        break;
                    case 34:
                        result = PrintCrystalReport("PreVal.rpt", null, filters);
                        break;
                    // Post- PreVal time sche (RTM)
                    case 35:
                        result = PrintCrystalReport("PostVal.rpt", null, filters);
                        break;
                    // Pre- Code list (KUVEMPU)
                    case 36:
                        switch (instituteId)
                        {
                            case 13:
                                result = PrintCodes_13(filters);
                                break;
                            default:
                                result = PrintCrystalReport("CodeList.rpt", null, filters);
                                break;
                        }
                        break;
                    // Pre- Marks Foil
                    case 37:
                        switch (instituteId)
                        {
                            case 3:
                            case 18:
                                throw new NotImplementedException();
                                break;
                            case 6:
                                result = PrintCrystalReport("MarksFoilKle_PR.rpt", null, filters);
                                break;
                            case 8:
                                result = PrintCrystalReport("MarksFoilAHS.rpt", null, filters);
                                break;
                            case 14:
                            case 19:
                                throw new NotImplementedException();
                                break;
                            case 15:
                                result = PrintCrystalReport("MarksFoilTBSE.rpt", null, filters);
                                break;
                            case 17:
                                result = PrintCrystalReport("MarksFoilKKH.rpt", null, filters);
                                break;
                            case 9:
                                switch (uc.ToUpper())
                                {
                                    case "T":
                                        result = PrintCrystalReport("MarksFoilDMI_01.rpt", null, filters);
                                        break;
                                    case "P":
                                        result = PrintCrystalReport("MarksFoilDMI_10.rpt", null, filters);
                                        break;
                                    default:
                                        result = PrintCrystalReport("MarksFoilDMI_Int.rpt", null, filters);
                                        break;
                                }
                                break;
                            case 11:
                                result = PrintMarksFoil_NMU(filters);
                                break;
                            case 13:
                                if (uc.ToUpper() == "U")
                                    result = PrintMarksFoil_13U(filters);
                                else
                                    result = PrintMarksFoil_13C(filters);
                                break;
                        }
                        break;
                    // Post- Extract Result for Net
                    case 38:
                        switch (instituteId)
                        {
                            case 11:
                                throw new NotImplementedException();
                                break;
                            case 13:
                                result = PrintNetResult_11(filters);
                                break;
                            case 14:
                            case 19:
                                result = PrintNetResult_14(filters);
                                break;
                            case 15:
                                result = PrintNetResult_15(filters);
                                break;
                            case 17:
                                result = PrintNetResult_17(filters);
                                break;
                            default:
                                result = PrintNetResult(filters);
                                break;
                        }
                        break;
                    // Pre- Extract RollList for Net
                    case 39:
                        result = PrintNetRollList(filters);
                        break;
                    case 40:
                        singleLine = uc;
                        switch (instituteId)
                        {
                            case 5:
                            case 9:
                                throw new NotImplementedException();
                                break;
                            default:
                                result = PrintRegistrationList(filters);
                                break;
                        }
                        break;
                    // Course- Regn Card
                    case 41:
                        if (registartionNo != "" && registartionNo != "?")
                            sessionNo = 0;  //remove session for sigle cert.
                        switch (instituteId)
                        {
                            case 3:
                            case 5:
                            case 9:
                            case 17:
                            case 18:
                                throw new NotImplementedException();
                                break;
                            case 8:
                                throw new NotImplementedException();
                                break;
                            case 14:
                            case 19:
                                throw new NotImplementedException();
                                break;
                            case 15:
                                result = PrintCrystalReport("RegnTBSE.rpt", null, filters);
                                break;
                        }
                        break;
                    // Merit List
                    case 42:
                        //throw new NotImplementedException();

                        if (reportFileName.IsNullOrWhiteSpace())

                            reportFileName = "Merit_Kle.rpt";

                        result = PrintCrystalReport(reportFileName, null, filters);

                        break;

                    //throw new NotImplementedException();
                    //break;
                    case 43:
                        if (instituteId == 14)
                            result = PrintDistrictAssessmentMarks(filters);
                        else
                            result = PrintSectionMarks(filters);
                        break;
                    // Check List
                    case 44:
                        result = PrintCrystalReport("CheckList.rpt", null, filters);
                        break;
                    // Marks Discrepancy Report
                    case 46:
                        throw new NotImplementedException();
                        break;
                    // Subject-Town-Centre Exempt/App count
                    case 47:
                        throw new NotImplementedException();
                        break;
                    // Subject-Town-Centre Exempt/App count
                    case 48:
                        throw new NotImplementedException();
                        break;
                    // College Paper Count
                    case 49:
                        result = PrintCollegePaperCount(filters);
                        break;
                    // Summary of PassCert
                    case 50:
                        throw new NotImplementedException();
                        break;
                    // Course- Regn Count
                    case 51:
                        result = PrintRegistrationCountAll(filters);
                        break;
                    // Extract Convocation List
                    case 52:
                        result = ExportConvocationList(filters);
                        break;
                    // Attempts Over List (TBSE)
                    case 53:
                        throw new NotImplementedException();
                        break;
                    // Excp Result Schools (TBSE)
                    case 54:
                        throw new NotImplementedException();
                        break;
                    // Result Summary (TBSE)
                    case 55:
                        throw new NotImplementedException();
                        break;
                    // Degree
                    case 56:
                        if (instituteId == 6)
                            result = PrintCrystalReport("ProvCert_6.rpt", null, filters);
                        break;
                    case 57:
                        throw new NotImplementedException();
                        break;
                    // NMU RollList for all courses
                    case 59:
                        result = PrepareRollList_11(filters);
                        break;
                    // NMU Course Setup Extract
                    case 60:
                        throw new NotImplementedException();
                        break;
                    // Subject Statistics
                    case 61:
                        throw new NotImplementedException();
                        break;
                    // Result Summary
                    case 62:
                        if (instituteId == 6)
                            result = PrepareResultSummary_06(filters);
                        else
                            result = new HttpResponseMessage(HttpStatusCode.NoContent);
                        break;
                    // Consolidated Marklist
                    case 63:
                        if (pageBreak == "L")
                            result = PrintConsolidated_MarkList(filters);
                        else
                        {
                            if (!reportImage.IsNullOrWhiteSpace())
                            {
                                reportFacade.Prepare_MarkList_KKH_C(loginId, scheduleId, streamId, collegeCode, registartionNo, instituteId, "", "Y");
                                reportFacade.PrepareImages(loginId, reportImage, imagePath);
                            }
                            result = PrintCrystalReport("MarkLKKH_C.rpt", null, filters);
                        }
                        break;
                    // Result Summary
                    case 64:
                        throw new NotImplementedException();
                        break;
                    // Lower Cases Cleared
                    case 65:
                        result = PrintLowerCasesCleared(filters);
                        break;
                    // Course Pass Certificate
                    case 66:
                        result = PrintCrystalReport("PassCert_17.rpt", null, filters);
                        break;
                    // Consolidated Ledger
                    case 67:
                        throw new NotImplementedException();
                        break;
                    // Transcript
                    case 68:
                        result = PrintCrystalReport("Transcript.rpt", null, filters);
                        break;
                    // Exam Form for Student
                    case 99:
                        result = PrintCrystalReport("ExamFormsI_Student.rpt", null, filters);
                        break;
                    // Exam Subjects
                    case 101:
                        result = PrintExamSubject(filters);
                        break;
                    // Exam Schedule
                    case 102:
                        result = PrintExamSchedule(filters);
                        break;
                    // Exam Edit Control
                    case 103:
                        result = PrintExamEditControl(filters);
                        break;
                    // Course Schedule
                    case 104:
                        result = PrintCourseSchedule(filters);
                        break;
                    // Course Edit Schedule
                    case 105:
                        result = PrintCourseEditSchedule(filters);
                        break;
                    // Faculty List
                    case 106:
                        result = PrintFacultyList(filters);
                        break;
                    // Colleges List
                    case 107:
                        result = PrintCollegesList(filters);
                        break;
                    // Course College Mapping
                    case 108:
                        result = PrintCourseCollegeMapping(filters);
                        break;
                    // Courses
                    case 109:
                        result = PrintCourseList(filters);
                        break;
                    // Exams
                    case 110:
                        result = PrintExamList(filters);
                        break;
                    // Country
                    case 111:
                        result = PrintCountryList(filters);
                        break;
                    // State
                    case 112:
                        result = PrintStateList(filters);
                        break;
                    // District
                    case 113:
                        result = PrintDistrictList(filters);
                        break;
                    // Division
                    case 114:
                        result = PrintDivisionList(filters);
                        break;
                    // Towns
                    case 115:
                        result = PrintTownsList(filters);
                        break;
                    // Tehasil
                    case 116:
                        result = PrintTehsilList(filters);
                        break;
                    case 132:
                        result = PrintExamEditControlList(filters);
                        break;

                        //// DocXReports
                        //case 301:
                        //case 302:
                        //    result = RenderDocxReport(filters);
                        //    break;
                }

                // DocXReports
                if (reportCode >= 300 && reportCode < 500)
                    result = RenderDocxReport(filters);

                // CSV Export Reports
                if (reportCode >= 500)
                    result = RenderCSVReports(filters);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                log.Error(ex.StackTrace, ex);
            }
            return result;
        }



        private string GetReportFileName(int reportCode, JObject filters)

        {

            string reportFileName = "";

            try

            {

                DataTable spData = ExecuteReportQuery("usp_GetReportSP", filters).FirstTable();

                if (spData.HasData())

                {

                    reportFileName = spData.FirstRow()["TemplateName"].ToString();

                }

            }

            catch { }

            return reportFileName;
        }

        private HttpResponseMessage PrintExamEditControlList(JObject filters)
        {
            long editEcId = 0;
            int editInCode = inCode;
            string editCgCode = "";
            string editPEA = "";
            string editOpt = "";

            JObject json;
            try
            {
                json = JObject.Parse(extra1);
                inCode = json.SelectToken("InCode").ToString().ToInt32();
                ecId = json.SelectToken("EcId").ToString().ToInt32();
                //ecShId = json.SelectToken("EcShId").ToString().ToInt32();
                reportName = json.SelectToken("StrTitle").Coalesce(reportName);
                reportSubTitle = json.SelectToken("StrSubTitle").Coalesce(reportSubTitle);
            }
            catch { }
            //If intInCode = 18 Then
            //    pfEditPEA = ""
            //    Set grsTemp2 = EnvDB.CnACDB.Execute("select InSpFlag from Inputs where InCode = 18")
            //    If Not grsTemp2.EOF Then pfEditPEA = grsTemp2.Fields(0)
            //    grsTemp2.Close
            //End If

            //If gintInst = 16 Then
            //    pfEditCaCent = "1"
            //    pfEditCaCent = InputBox("1. CentreWise" & vbCrLf & "2. CollegeWise", "Edit List", pfEditCaCent)
            //    pfEditCaCent = Trim(pfEditCaCent)
            //End If
            editOpt = "1";
            //If itxtStatus<> "U" And pfEditCaCent<> "1" Then
            //    strEditOpt = InputBox("1. All" & vbCrLf & "2. Changed" & vbCrLf & "3. Invalid", "Edit List", strEditOpt)
            //    strEditOpt = Trim(strEditOpt)
            //End If
            switch (editOpt)
            {
                case "1":
                    editChange = "";
                    editValid = "";
                    break;
                case "2":
                    editChange = "Y";
                    editValid = "";
                    break;
                case "3":
                    editChange = "";
                    editValid = "N";
                    break;
            }
            JObject parameters = null;
            return PrintEditList(filters, parameters);
        }

        private HttpResponseMessage PrintEditList(JObject filters, JObject parameters)
        {
            JObject json;
            try
            {
                json = JObject.Parse(extra1);
                inCode = json.SelectToken("InCode").ToString().ToInt32();
                ecId = json.SelectToken("EcId").ToString().ToInt32();
            }
            catch { }
            string emMarksD = "";
            LegacyReportFacade reportFacade = new LegacyReportFacade(filters);

            try
            {
                reportFacade.GetEditListTitle(ecId, inCode, ref reportTitle, ref reportSubTitle);
            }
            catch { }
            try
            {
                if (inCode < 50)
                {
                    reportFacade.GetEditMarksD(ecId, ref emMarksD);
                }
            }
            catch { }

            switch (inCode)
            {
                case 2:
                    return PrintNameSlips(filters, parameters);
                case 4:
                    throw new NotImplementedException();
                case 5:
                    return PrintMarkSlips(filters, parameters);
                case 6:
                    throw new NotImplementedException();
                case 7:
                case 8:
                case 11:
                case 14:
                case 15:
                case 16:
                    if (instituteId == 6 && inCode == 7 && editFlag != "")
                    {
                        switch (editFlag)
                        {
                            case "M":
                                return PrintCrystalReport("MaxInp_06.rpt", null, filters);
                                break;
                            case "A":
                                return PrintCrystalReport("AttenSh_06.rpt", null, filters);
                                break;
                            default:
                                return PrintRollMarks(filters, parameters, emMarksD);
                                break;
                        }
                    }
                    else
                    {
                        switch (instituteId)
                        {
                            case 6:
                            case 9:
                            case 12:
                                return PrintCrystalReport("EditMark.rpt", null, filters);
                                break;
                            default:
                                return PrintRollMarks(filters, parameters, emMarksD);
                                break;
                        }
                    }
                    break;
                case 9:
                    throw new NotImplementedException();
                case 10:
                    throw new NotImplementedException();
                case 12:
                    throw new NotImplementedException();
                case 13:
                    return PrintSubjectChange(filters, parameters);
                case 18:
                    throw new NotImplementedException();
                case 19:
                    switch (instituteId)
                    {
                        case 3:
                        case 18:
                            if (editCandidate == "C")
                                //sCandList03
                                throw new NotImplementedException();
                            else
                                return PrintExamFormsG(filters, parameters);
                            break;
                        case 14:
                        case 19:
                            if (editCandidate == "C")
                                //sCandList14
                                throw new NotImplementedException();
                            else
                                return PrintExamFormsG(filters, parameters);
                            break;
                        default:
                            return PrintExamFormsG(filters, parameters);
                            break;
                    }
                    break;
                case 20:
                case 55:
                    return PrintCenterAllocation(filters, parameters, emMarksD);
                case 21:
                case 24:
                    return PrintChangeResult(filters, parameters, emMarksD);
                case 25:
                    throw new NotImplementedException();
                case 26:
                    throw new NotImplementedException();
                case 27:
                    throw new NotImplementedException();
                case 51:
                case 61:
                    return PrintRegistrationForms(filters, parameters);
                case 52:
                    return PrintRegistrationStatus(filters, parameters);
                case 53:
                    throw new NotImplementedException();
                case 54:
                    throw new NotImplementedException();
                case 57:
                    throw new NotImplementedException();
                case 62:
                    throw new NotImplementedException();
                case 63:
                    throw new NotImplementedException();
                case 999:
                    throw new NotImplementedException();
            }

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        private HttpResponseMessage PrintRegistrationForms(JObject filters, JObject parameters)
        {
            int printWidth = 132;
            int printSrNo = 0;
            List<double> printTotals = new List<double>() { 0, 0, 0, 0 };
            List<string> printGrps = new List<string>() { "", "", "", "" }; //EfPress

            string printFileName = (inCode == 51 ? "EditRegn.txt" : "RegNewCo.txt");
            string reportName = "EditList Registration";
            Print print = new Print();
            int pageNumber = 1;
            string printLine = print.FormatPrintString(reportName + " - " + printWidth, 50) + print.FormatPrintString(_reportPath + printFileName, 50);
            bool printResult = false;
            printResult = print.PrepareAndPrint(0, printWidth, printLine);
            if (!printResult) return new HttpResponseMessage(HttpStatusCode.NoContent);

            // KKH Invalid List
            bool printInvalidList = false;
            if (instituteId == 17)
            {
                try
                {
                    JObject json;
                    json = JObject.Parse(extra1);
                    printInvalidList = json.SelectToken("InvalidList").Coalesce().ToBoolean();
                }
                catch { }
                if (printInvalidList)
                {
                    reportSubTitle = "Invalid Records";
                    options = "";
                }
            }

            LegacyReportFacade reportFacade = new LegacyReportFacade(filters);
            DataSet dataSet = new DataSet();
            if (printInvalidList)
                dataSet = reportFacade.Get_Rpt_InvdRegnForm(ecId, collegeCode);
            else
                dataSet = reportFacade.Get_SP_SearchEditRegnForm(ecId, collegeCode, editValid, editChange);

            if (!dataSet.HasData())
                return new HttpResponseMessage(HttpStatusCode.NoContent);

            // Store headers
            // 1
            printLine = instituteName;
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printLine = print.FormatPrintString(printLine, printWidth - 12);
            printResult = print.PrepareAndPrint(101, 0, printLine);
            if (!printResult) goto endReport;
            //2
            printLine = reportName;
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printLine = print.FormatPrintString(printLine, printWidth - 12) + systemDate.ToString(dateFormat);
            printResult = print.PrepareAndPrint(102, 0, printLine);
            if (!printResult) goto endReport;
            // 3
            printLine = (inCode == 51 ? "RegnForms - " : "Regn New Course - ") + reportTitle;
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printLine = print.FormatPrintString(printLine, printWidth - 12) + "Page: " + pageNumber;
            printResult = print.PrepareAndPrint(103, 0, printLine);
            if (!printResult) goto endReport;
            // 4
            printLine = reportSubTitle;
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printResult = print.PrepareAndPrint(104, 0, printLine);
            if (!printResult) goto endReport;

            if (printInvalidList)
            {
                DataRow firstRow = dataSet.FirstRow();
                printGrps[1] = firstRow["CgCode"].ToString();
                printLine = firstRow["CgCode"].ToString() + " " + firstRow["Addr1"].ToString() + firstRow["Addr2"].ToString();
                printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
                printResult = print.PrepareAndPrint(105, 0, printLine);
            }
            else
            {
                printLine = options.Coalesce();
                printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
                printResult = print.PrepareAndPrint(105, 0, printLine);
            }

            // 5
            printResult = print.PrepareAndPrint(106, 0, "-".RepeatForLeftPadding(printWidth));
            if (!printResult) goto endReport;
            // 6
            printLine = print.FormatPrintString("SrNo", 6, 5)
                    + print.FormatPrintString("FormNo/Batch", 20, 19)
                    + print.FormatPrintString("Name", 50, 49)
                    + print.FormatPrintString((isInstitute || isSchool ? "Inst" : "Colg"), 8, 7)
                    + print.FormatPrintString("Brch", 6, 5)
                    + print.FormatPrintString("Sex", 4, 3)
                    + print.FormatPrintString("BirthDate", 11, 10)
                    + print.FormatPrintString("Cate", 5, 4)
                    + print.FormatPrintString("Na", 3, 2)
                    + print.FormatPrintString("M", 2, 1)
                    + print.FormatPrintString("FExm", 5, 4)
                    + (printInvalidList ? print.FormatPrintString("ListNo", 7) : print.FormatPrintString("Vld", 4, 3) + print.FormatPrintString("Ch", 3, 2))
                    + print.FormatPrintString("Act", 4, 3);
            printResult = print.PrepareAndPrint(107, 0, printLine);
            if (!printResult) goto endReport;
            // 7
            printLine = print.FormatPrintString("", 5)
                    + print.FormatPrintString("RegnNo", 21, 20)
                    + print.FormatPrintString("Father", 41, 40) + "Sub";
            printResult = print.PrepareAndPrint(108, 0, printLine);
            if (!printResult) goto endReport;
            // 8
            printResult = print.PrepareAndPrint(109, 0, "=".RepeatForLeftPadding(printWidth));
            if (!printResult) goto endReport;

            // Headers
            printResult = print.PrepareAndPrint(2, 0, "");
            if (!printResult) goto endReport;

            printSrNo = 0;
            printGrps[0] = "";

            int recordCount = dataSet.Tables[0].Rows.Count;
            for (int rowCounter = 0; rowCounter < recordCount; rowCounter++)
            {
                DataRow dataRow = dataSet.Tables[0].Rows[rowCounter];
                //page break for Cg wise invalid list;
                if (printInvalidList && printGrps[1] != dataRow["CgCode"].ToString())
                {
                    printGrps[1] = dataRow["CgCode"].ToString();
                    printLine = dataRow["CgCode"].ToString() + " " + dataRow["Addr1"].ToString() + dataRow["Addr2"].ToString();
                    printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
                    printResult = print.PrepareAndPrint(104, 0, printLine);
                    printResult = print.PrepareAndPrint(2, 99, "");
                    printSrNo = 0;
                }

                printSrNo = printSrNo + 1;
                printLine = print.FormatPrintString(printSrNo, 6, 5)
                    + print.FormatPrintString(dataRow["FormNo"].ToString().Trim() + " " + dataRow["Batch"].ToString().Trim(), 20, 19)
                    + print.FormatPrintString(dataRow["Name"].ToString(), 50, 49)
                    + print.FormatPrintString(dataRow["CgCode"].ToString(), 8, 7)
                    + print.FormatPrintString(dataRow["StrCode"].ToString(), 6, 5)
                    + print.FormatPrintString(dataRow["Sex"].ToString(), 4, 3)
                    + print.FormatPrintString(dataRow["BirthDate"].ToString(), 11, 10)
                    + print.FormatPrintString(dataRow["Caste"].ToString(), 5, 4)
                    + print.FormatPrintString(dataRow["Nation"].ToString(), 3, 2)
                    + print.FormatPrintString(dataRow["Medium"].ToString(), 2, 1)
                    + print.FormatPrintString(dataRow["EmSrNoF"].ToString() + " " + dataRow["EmSkip"].ToString(), 5, 4)
                    + (printInvalidList ? print.FormatPrintString(dataRow["EcId"].ToString(), 7, 6) :
                        print.FormatPrintString(dataRow["Valid"].ToString(), 4, 3) + print.FormatPrintString(dataRow["Chng"].ToString(), 3, 2))
                    + print.FormatPrintString((dataRow["ProcFlag"].ToString() == "Y" ? "DEL" : (dataRow["RgId"].ToString() == "0" ? "ADD" : "MOD")), 4, 3);
                printResult = print.PrepareAndPrint(3, 0, printLine);

                if (instituteId == 12)
                {
                    printLine = print.FormatPrintString("", 5)
                        + print.FormatPrintString(dataRow["RgNo"].ToString(), 21, 20)
                        + print.FormatPrintString(dataRow["Sess"].ToString(), 5, 4)
                        + print.FormatPrintString(dataRow["CtCode"].ToString(), 6, 5)
                        + print.FormatPrintString(dataRow["ExSub"].ToString(), 10, 9)
                        + (dataRow["Mesg"].ToString().Trim() != "" ? "Er: " + dataRow["Mesg"].ToString() : "");
                }
                else
                {
                    printLine = print.FormatPrintString("", 6, 5)
                        + print.FormatPrintString(dataRow["RgNo"].ToString(), 20, 19)
                        + print.FormatPrintString(dataRow["Father"].ToString(), 41, 40)
                        + dataRow["Sub"].ToString() + " "
                        + (dataRow["Mesg"].ToString().Trim() != "" ? "Er: " + dataRow["Mesg"].ToString() : "");
                }
                printLine = print.FormatPrintString(printLine, 132, 131);
                printResult = print.PrepareAndPrint(3, 0, printLine);

                if (!printInvalidList)
                {
                    printLine = print.FormatPrintString("", 27) + "Addr: "
                            + dataRow["Addr1"].ToString() + " "
                            + dataRow["Addr2"].ToString() + " "
                            + dataRow["Addr3"].ToString() + " "
                            + dataRow["Place"].ToString() + " Perm: "
                            + dataRow["Addr1P"].ToString() + " "
                            + dataRow["Addr2P"].ToString() + " "
                            + dataRow["Addr3P"].ToString() + " "
                            + dataRow["PlaceP"].ToString();
                    printLine = print.FormatPrintString(printLine, 132, 131);
                    printResult = print.PrepareAndPrint(3, 0, printLine);
                }
                printResult = print.PrepareAndPrint(3, 0, "");
            }
            printResult = print.PrepareAndPrint(3, 1, footer);
            printResult = print.PrepareAndPrint(3, 0, "");

            gLnI = 0;
            validationMessage = "Printing Over";

        endReport:
            print.PrepareAndPrint(9, gLnI == 1 ? 1 : 0, validationMessage);

            PrintSettings printSettings = new PrintSettings();
            printSettings.PageSize = PdfSharp.PageSize.Ledger;
            printSettings.PageOrientation = PdfSharp.PageOrientation.Landscape;
            return PrintLineReport(print, printSettings);
        }
        private HttpResponseMessage PrintRegistrationStatus(JObject filters, JObject parameters)
        {
            int printWidth = 132;
            int printSrNo = 0;
            List<double> printTotals = new List<double>() { 0, 0, 0, 0 };
            List<string> printGrps = new List<string>() { "", "", "", "" }; //EfPress

            string printFileName = "EditRegnStatus.txt";
            string reportName = "EditList Student Status";
            Print print = new Print();
            int pageNumber = 1;
            string printLine = print.FormatPrintString(reportName + " - " + printWidth, 50) + print.FormatPrintString(_reportPath + printFileName, 50);
            bool printResult = false;
            printResult = print.PrepareAndPrint(0, printWidth, printLine);
            if (!printResult) return new HttpResponseMessage(HttpStatusCode.NoContent);

            LegacyReportFacade reportFacade = new LegacyReportFacade(filters);
            DataSet dataSet = new DataSet();

            dataSet = reportFacade.Get_SP_SearchEditStatus(ecId, collegeCode, editValid, editChange);

            if (!dataSet.HasData())
                return new HttpResponseMessage(HttpStatusCode.NoContent);

            // Store headers
            // 1
            printLine = instituteName;
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printLine = print.FormatPrintString(printLine, printWidth - 12);
            printResult = print.PrepareAndPrint(101, 0, printLine);
            if (!printResult) goto endReport;
            //2
            printLine = reportName;
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printLine = print.FormatPrintString(printLine, printWidth - 12) + systemDate.ToString(dateFormat);
            printResult = print.PrepareAndPrint(102, 0, printLine);
            if (!printResult) goto endReport;
            // 3
            printLine = "Student Status - " + reportTitle;
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printLine = print.FormatPrintString(printLine, printWidth - 12) + "Page: " + pageNumber;
            printResult = print.PrepareAndPrint(103, 0, printLine);
            if (!printResult) goto endReport;
            // 4
            printLine = reportSubTitle;
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printResult = print.PrepareAndPrint(104, 0, printLine);
            if (!printResult) goto endReport;

            // 5
            printResult = print.PrepareAndPrint(105, 0, "-".RepeatForLeftPadding(printWidth));
            if (!printResult) goto endReport;
            // 6
            printLine = print.FormatPrintString("SrNo", 3, 2)
                    + print.FormatPrintString("RegnNo", 21, 20)
                    + print.FormatPrintString("Name", 51, 50)
                    + print.FormatPrintString("Type", 16, 15)
                    + print.FormatPrintString("Deatils", 38, 37)
                    + print.FormatPrintString("Ch", 3, 2);

            printResult = print.PrepareAndPrint(106, 0, printLine);
            if (!printResult) goto endReport;
            // 7
            printResult = print.PrepareAndPrint(107, 0, "=".RepeatForLeftPadding(printWidth));
            if (!printResult) goto endReport;

            // Headers
            printResult = print.PrepareAndPrint(2, 0, "");
            if (!printResult) goto endReport;

            printSrNo = 0;
            printGrps[0] = "";

            int recordCount = dataSet.Tables[0].Rows.Count;
            for (int rowCounter = 0; rowCounter < recordCount; rowCounter++)
            {
                DataRow dataRow = dataSet.Tables[0].Rows[rowCounter];
                string tmpType = "";
                switch (dataRow["Type"].Coalesce())
                {
                    case "S":
                        tmpType = "Student Status";
                        break;
                    case "E":
                        tmpType = "Exam Status";
                        break;
                    case "N":
                        tmpType = "New Course";
                        break;
                    case "D":
                        tmpType = "Delete Record";
                        break;
                }

                printSrNo = printSrNo + 1;
                printLine = print.FormatPrintString(printSrNo, 3, 2)
                    + print.FormatPrintString(dataRow["RgNo"].ToString().Trim(), 21, 20)
                    + print.FormatPrintString(dataRow["RgName"].ToString(), 51, 50)
                    + print.FormatPrintString(tmpType, 16, 15)
                    + print.FormatPrintString(dataRow["Details"].ToString(), 38, 37)
                    + print.FormatPrintString(dataRow["Chng"].ToString(), 3, 2);
                printResult = print.PrepareAndPrint(3, 0, printLine);
                printResult = print.PrepareAndPrint(3, 0, "");
            }
            printResult = print.PrepareAndPrint(3, 1, footer);
            printResult = print.PrepareAndPrint(3, 0, "");

            gLnI = 0;
            validationMessage = "Printing Over";

        endReport:
            print.PrepareAndPrint(9, gLnI == 1 ? 1 : 0, validationMessage);

            PrintSettings printSettings = new PrintSettings();
            printSettings.PageSize = PdfSharp.PageSize.Ledger;
            printSettings.PageOrientation = PdfSharp.PageOrientation.Landscape;
            return PrintLineReport(print, printSettings);
        }

        private HttpResponseMessage PrintSubjectChange(JObject filters, JObject parameters)
        {
            int printWidth = 79;
            int printSrNo = 0;
            List<double> printTotals = new List<double>() { 0, 0, 0, 0 };
            List<string> printGrps = new List<string>() { "", "", "", "" }; //EfPress

            string printFileName = "EditSC.Txt";
            string reportName = "EditList Subject Change";
            Print print = new Print();
            int pageNumber = 1;
            string printLine = print.FormatPrintString(reportName + " - " + printWidth, 50) + print.FormatPrintString(_reportPath + printFileName, 50);
            bool printResult = false;
            printResult = print.PrepareAndPrint(0, printWidth, printLine);
            if (!printResult) return new HttpResponseMessage(HttpStatusCode.NoContent);

            LegacyReportFacade reportFacade = new LegacyReportFacade(filters);
            DataSet dataSet = new DataSet();
            dataSet = reportFacade.Get_Rpt_EditSubChng(ecId, collegeCode, editChange, editValid);
            if (!dataSet.HasData())
                return new HttpResponseMessage(HttpStatusCode.NoContent);


            // Store headers
            // 1
            printLine = reportName;
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printLine = print.FormatPrintString(printLine, printWidth - 12) + systemDate.ToString(dateFormat);
            printResult = print.PrepareAndPrint(101, 0, printLine);
            if (!printResult) goto endReport;
            // 2
            printLine = "Sub Change - " + reportTitle;
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printLine = print.FormatPrintString(printLine, printWidth - 12) + "Page: " + pageNumber;
            printResult = print.PrepareAndPrint(102, 0, printLine);
            if (!printResult) goto endReport;
            // 3
            printLine = reportSubTitle;
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printResult = print.PrepareAndPrint(103, 0, printLine);
            if (!printResult) goto endReport;
            // 4
            printLine = options.Coalesce();
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printResult = print.PrepareAndPrint(104, 0, printLine);
            if (!printResult) goto endReport;
            // 5
            printResult = print.PrepareAndPrint(105, 0, "-".RepeatForLeftPadding(printWidth));
            if (!printResult) goto endReport;
            // 6
            //Batch, RgNo, RollNo, SubCode, AddRem, Remark, RgName, RgCgCode, Valid, Mesg, Chng;
            printLine = print.FormatPrintString("Batch ", 8)
                    + print.FormatPrintString("Name", 25)
                    + print.FormatPrintString("Form", 5)
                    + print.FormatPrintString("Subject  Subject  Subject  Subject ", 35)
                    + print.FormatPrintString("Chg", 4, 3)
                    + print.FormatPrintString("Vld", 3);
            printResult = print.PrepareAndPrint(106, 0, printLine);
            if (!printResult) goto endReport;
            // 7
            printLine = print.FormatPrintString("", 8)
                    + print.FormatPrintString("   RollNo", 10)
                    + print.FormatPrintString("RegnNo", 20);
            printResult = print.PrepareAndPrint(107, 0, printLine);
            if (!printResult) goto endReport;
            // 8
            printResult = print.PrepareAndPrint(108, 0, "=".RepeatForLeftPadding(printWidth));
            if (!printResult) goto endReport;

            // Print Header
            printResult = print.PrepareAndPrint(2, 0, "");
            if (!printResult) goto endReport;

            // Print Details
            printSrNo = 0;
            printGrps[0] = "";

            int recordCount = dataSet.Tables[0].Rows.Count;
            string temp = "";
            string temp2 = "";
            string temp3 = "";
            for (int rowCounter = 0; rowCounter < recordCount; rowCounter++)
            {
                DataRow dataRow = dataSet.Tables[0].Rows[rowCounter];

                printResult = print.PrepareAndPrint(3, 0, "");
                printGrps[1] = dataRow["Batch"].ToString();
                temp = "";
                temp2 = print.FormatPrintString(" " + dataRow["Chng"].ToString(), 4, 3) + print.FormatPrintString(" " + dataRow["Valid"].ToString(), 3);
                temp3 = print.FormatPrintString("", 8)
                        + print.FormatPrintString(dataRow["RollNo"].ToString(), 10, 9)
                        + print.FormatPrintString(dataRow["RgNo"].ToString(), 20, 19);
                printLine = print.FormatPrintString(dataRow["Batch"].ToString(), 8, 7)
                        + print.FormatPrintString(dataRow["RgName"].ToString(), 25, 24)
                        + print.FormatPrintString((dataRow["ProcFlag"].ToString() == "D" ? "Del" : ""), 5, 4);
                printSrNo = 0;

            nextSubjectChange:
                dataRow = dataSet.Tables[0].Rows[rowCounter];
                temp = temp + print.FormatPrintString(dataRow["AddRem"].ToString(), 2) + print.FormatPrintString(dataRow["SubCode"].ToString(), 7, 6);
                printSrNo = printSrNo + 1;

                rowCounter++;
                printTotalCounter = 0;
                if (rowCounter == recordCount - 1)
                    printTotalCounter = 1;
                else if (printGrps[1] != dataRow["Batch"].ToString())
                    printTotalCounter = 1;
                else if (printSrNo == 4)
                    printTotalCounter = 2;

                if (printTotalCounter > 0)
                {
                    printLine = printLine + print.FormatPrintString(temp, 35) + temp2;
                    printResult = print.PrepareAndPrint(3, 0, printLine);
                    printLine = temp3;
                    temp = "";
                    temp2 = "";
                    temp3 = print.FormatPrintString("", 38);
                    printSrNo = 0;
                }

                if (printTotalCounter != 1)
                    goto nextSubjectChange;
                if (printLine.Trim() != "")
                    printResult = print.PrepareAndPrint(3, 0, printLine);
            }

            gLnI = 0;
            validationMessage = "Printing Over";

        endReport:
            print.PrepareAndPrint(9, gLnI == 1 ? 1 : 0, validationMessage);

            PrintSettings printSettings = new PrintSettings();
            printSettings.PageSize = PdfSharp.PageSize.A4;
            printSettings.PageOrientation = PdfSharp.PageOrientation.Portrait;
            printSettings.FontSize = 10;
            return PrintLineReport(print, printSettings);
        }

        private HttpResponseMessage PrintChangeResult(JObject filters, JObject parameters, string emMarksD)
        {
            int printWidth = 130;
            int printSrNo = 0;
            List<double> printTotals = new List<double>() { 0, 0, 0, 0 };
            List<string> printGrps = new List<string>() { "", "", "", "" };

            string printFileName = (inCode == 21 ? "EditChRe.txt" : "EditRevl.txt");
            string reportName = "EditList for " + (inCode == 21 ? "Change Result" : "Revaluation");

            Print print = new Print();
            int pageNumber = 1;
            string printLine = print.FormatPrintString(reportName + " - " + printWidth, 50) + print.FormatPrintString(_reportPath + printFileName, 50);
            bool printResult = false;
            printResult = print.PrepareAndPrint(0, printWidth, printLine);
            if (!printResult) return new HttpResponseMessage(HttpStatusCode.NoContent);

            LegacyReportFacade reportFacade = new LegacyReportFacade(filters);
            DataSet dataSet = new DataSet();
            dataSet = reportFacade.Get_SP_SearchEditChResult(ecId, editChange, editValid);
            if (!dataSet.HasData())
                return new HttpResponseMessage(HttpStatusCode.NoContent);

            string editControlStatus = "";
            DataSet dataSet1 = reportFacade.Get_SP_GetEditControl(ecId);
            if (dataSet1.HasData())
            {
                try { editControlStatus = dataSet1.FirstRow()["EcStatus"].Coalesce(); } catch { }
            }

            //Store headers
            // 1
            printLine = reportName;
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printLine = print.FormatPrintString(printLine, printWidth - 12) + systemDate.ToString(dateFormat);
            printResult = print.PrepareAndPrint(101, 0, printLine);
            if (!printResult) goto endReport;
            // 2
            printLine = (inCode == 21 ? "Change Result - " : "Revaluation - ") + reportTitle;
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printResult = print.PrepareAndPrint(102, 0, printLine);
            if (!printResult) goto endReport;
            // 3
            printLine = reportSubTitle;
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printLine = print.FormatPrintString(printLine, printWidth - 12) + "Page: " + pageNumber;
            printResult = print.PrepareAndPrint(103, 0, printLine);
            if (!printResult) goto endReport;
            // 4
            printLine = options.Coalesce();
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printResult = print.PrepareAndPrint(104, 0, printLine);
            if (!printResult) goto endReport;
            // 5
            printResult = print.PrepareAndPrint(105, 0, "-".RepeatForLeftPadding(printWidth));
            if (!printResult) goto endReport;
            // 6
            printLine = print.FormatPrintString("Batch ", 9)
                    + print.FormatPrintString((registrationRoll == "G" ? "RgNo" : " RollNo"), 17, 16)
                    + print.FormatPrintString("Subject", 18, 17)
                    + print.FormatPrintString("Marks", 7, 5)
                    + print.FormatPrintString("Stat", 5, 4)
                    + print.FormatPrintString("WithHold", 10, 9)
                    + print.FormatPrintString("PrvMark", 8, 7)
                    + print.FormatPrintString("PrvRes", 7, 7)
                    + print.FormatPrintString("Chg Vld Remarks", 52);
            printResult = print.PrepareAndPrint(106, 0, printLine);
            if (!printResult) goto endReport;
            // 7
            printResult = print.PrepareAndPrint(107, 0, "=".RepeatForLeftPadding(printWidth));
            if (!printResult) goto endReport;

            // Print Header
            printResult = print.PrepareAndPrint(2, 0, "");
            if (!printResult) goto endReport;

            printSrNo = 0;
            printGrps[0] = "";

            int recordCount = dataSet.Tables[0].Rows.Count;
            for (int rowCounter = 0; rowCounter < recordCount; rowCounter++)
            {
                DataRow dataRow = dataSet.Tables[0].Rows[rowCounter];
                string temp = "";
                if (editControlStatus == "O")
                    temp = "";
                else
                    temp = print.FormatPrintString(dataRow["NewForm"].ToString() + " " + dataRow["HigherExams"].ToString(), 15, 14);

                printSrNo = printSrNo + 1;
                printLine = print.FormatPrintString(dataRow["Batch"].ToString(), 9, 8)
                        + (registrationRoll == "G" ? print.FormatPrintString(dataRow["RgNo"].ToString(), 17, 16) :
                            print.FormatPrintString(dataRow["RollNo"].ToString(), 17, 10))
                        + print.FormatPrintString(dataRow["Exam"].ToString() + " " + dataRow["Sub"].ToString()
                            + (dataRow["Head"].ToString() == "" ? "" : "-" + dataRow["Head"].ToString()), 18, 17)
                        + print.FormatPrintString(dataRow["Grade"].ToString().Trim() == "" ? ((emMarksD == "Y" ? dataRow["MarksD"].ToString() : dataRow["Marks"].ToString())) : dataRow["Grade"].ToString(), 7)
                        + print.FormatPrintString(dataRow["Status"].ToString(), 5, 4)
                        + print.FormatPrintString(dataRow["ExcpCode"].ToString() + " " + dataRow["SetRem"].ToString(), 10, 9)
                        + print.FormatPrintString(dataRow["PrvMarks"].ToString(), 8, 7)
                        + print.FormatPrintString(" " + dataRow["PrvResult"].ToString(), 7, 6)
                        + print.FormatPrintString(" " + dataRow["Chng"].ToString(), 4, 3)
                        + print.FormatPrintString(" " + dataRow["Valid"].ToString(), 4, 3)
                        + print.FormatPrintString(" " + temp + dataRow["Comment"].ToString() + " " + dataRow["Mesg"].ToString(), 44);
                printResult = print.PrepareAndPrint(3, 0, printLine);
                printResult = print.PrepareAndPrint(3, 0, "");
            }

            gLnI = 0;
            validationMessage = "Printing Over";
        endReport:
            print.PrepareAndPrint(9, gLnI == 1 ? 1 : 0, validationMessage);

            PrintSettings printSettings = new PrintSettings();
            printSettings.PageSize = PdfSharp.PageSize.Ledger;
            printSettings.PageOrientation = PdfSharp.PageOrientation.Landscape;
            return PrintLineReport(print, printSettings);
        }

        private HttpResponseMessage PrintExamFormsG(JObject filters, JObject parameters)
        {
            List<string> studentNames = new List<string>();
            string line1 = "";
            string message = "";
            string centerRoll = "";
            string subjects = "";
            bool newForms = false;

            int printWidth = 130;
            int printSrNo = 0;
            List<double> printTotals = new List<double>() { 0, 0, 0, 0 };
            List<string> printGrps = new List<string>() { "", "", "", "" };

            string printFileName = "EditEF.Txt";
            string reportName = "EditList Exam Forms";
            if (editCandidate.ToUpper() == "C")
                reportName = "Candidate List";

            Print print = new Print();
            int pageNumber = 1;
            string printLine = print.FormatPrintString(reportName + " - " + printWidth, 50) + print.FormatPrintString(_reportPath + printFileName, 50);
            bool printResult = false;
            printResult = print.PrepareAndPrint(0, printWidth, printLine);
            if (!printResult) return new HttpResponseMessage(HttpStatusCode.NoContent);

            LegacyReportFacade reportFacade = new LegacyReportFacade(filters);
            DataSet dataSet = new DataSet();
            dataSet = reportFacade.Get_SP_SearchEditExamForm(ecId, collegeCode, editValid, editChange, editApplication, 0);
            if (!dataSet.HasData())
                return new HttpResponseMessage(HttpStatusCode.NoContent);

            // Store headers
            // 1
            printLine = instituteName;
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printLine = print.FormatPrintString(printLine, printWidth - 12) + systemDate.ToString(dateFormat);
            printResult = print.PrepareAndPrint(101, 0, printLine);
            if (!printResult) goto endReport;
            // 2
            printLine = (editCandidate == "C" ? "Candidate List - " : "ExamForms - ") + reportTitle;
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printLine = print.FormatPrintString(printLine, printWidth - 12) + "Page: " + pageNumber;
            printResult = print.PrepareAndPrint(102, 0, printLine);
            if (!printResult) goto endReport;
            // 3
            printLine = (editCandidate == "C" ? "" : reportSubTitle);
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printResult = print.PrepareAndPrint(103, 0, printLine);
            if (!printResult) goto endReport;
            // 4
            if (instituteId == 16 && editChange != "")
                options = options.Replace("Changed", "Appeared");

            printLine = options;
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printResult = print.PrepareAndPrint(104, 0, printLine);
            if (!printResult) goto endReport;

            DataRow firstRow = dataSet.FirstRow();
            // 5  -College
            printSrNo = 0;
            printGrps[0] = firstRow["CgCode"].ToString();
            printLine = "College: " + printGrps[0] + " " + firstRow["CgName"].ToString();
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printResult = print.PrepareAndPrint(105, 0, printLine);
            if (!printResult) goto endReport;
            // 6
            printGrps[1] = firstRow["StrCode"].ToString();
            printLine = (printGrps[1] == "" ? "" : "Branch: " + printGrps[1] + " " + firstRow["StrName"].ToString());
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printResult = print.PrepareAndPrint(106, 0, printLine);
            if (!printResult) goto endReport;
            // 7
            printResult = print.PrepareAndPrint(107, 0, "-".RepeatForLeftPadding(printWidth));
            if (!printResult) goto endReport;

            // 8
            printLine = print.FormatPrintString("SrNo", 5, 4)
                    + print.FormatPrintString("Regn No", 14, 13)
                    + print.FormatPrintString((instituteId == 5 ? "Roll" : "Caste"), 6, 5)
                    + print.FormatPrintString("M/F", 4, 3)
                    + print.FormatPrintString("App-Exam", 14, 11)
                    + print.FormatPrintString("(P-Pass Y-Appearing)", 70)
                    + print.FormatPrintString((instituteId == 13 ? "Venue" : "Centre"), 7)
                    + print.FormatPrintString("    RollNo", 10);
            printResult = print.PrepareAndPrint(108, 0, printLine);
            if (!printResult) goto endReport;

            // 9
            printLine = print.FormatPrintString("", 5)
                    + print.FormatPrintString("Name", 38, 37)
                    + print.FormatPrintString("Paper         ".RepeatForLeftPadding(5), 70)
                    + print.FormatPrintString((instituteId == 13 ? "Decl  Cent RPV " : ""), 17);
            printResult = print.PrepareAndPrint(109, 0, printLine);
            if (!printResult) goto endReport;

            printResult = print.PrepareAndPrint(110, 0, "=".RepeatForLeftPadding(printWidth));
            if (!printResult) goto endReport;

            // Print Header
            printResult = print.PrepareAndPrint(2, 0, "");
            if (!printResult) goto endReport;

            int recordCount = dataSet.Tables[0].Rows.Count;
            for (int rowCounter = 0; rowCounter < recordCount; rowCounter++)
            {
                DataRow dataRow = dataSet.Tables[0].Rows[rowCounter];

                if (newForms)
                {
                    if (dataRow["App"].ToString() == "N" || dataRow["EfId"].ToString() != "0")
                    {
                        goto nextExamForm;
                    }
                }
                if (printGrps[0] != dataRow["CgCode"].ToString()
                    || printGrps[1] != dataRow["StrCode"].ToString())
                {
                    printGrps[0] = dataRow["CgCode"].ToString();
                    printLine = "College: " + printGrps[0] + " " + dataRow["CgName"].ToString();
                    printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
                    printResult = print.PrepareAndPrint(105, 0, printLine);

                    printGrps[1] = dataRow["StrCode"].ToString();
                    printLine = (printGrps[1] == "" ? "" : "Branch: " + printGrps[1] + " " + dataRow["StrName"].ToString());
                    printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
                    printResult = print.PrepareAndPrint(106, 0, printLine);

                    printResult = print.PrepareAndPrint(2, 99, "");
                    if (!printResult) goto endReport;
                    printSrNo = 0;
                }

                if (printGrps[2] == dataRow["RgId"].ToString())
                {
                    line1 = print.FormatPrintString("", 31)
                        + print.FormatPrintString(dataRow["App"].ToString(), 2)
                        + print.FormatPrintString(dataRow["SrNo"].ToString()
                        + "-" + dataRow["EmCode"].ToString(), 10, 9);
                    studentNames = print.BreakString("", 25, 3);
                }
                else
                {
                    if (printGrps[2] != "")
                        printResult = print.PrepareAndPrint(3, 0, "-".RepeatForLeftPadding(printWidth));

                    printGrps[2] = dataRow["RgId"].ToString();
                    printSrNo = printSrNo + 1;
                    line1 = print.FormatPrintString(printSrNo, 5, 4)
                        + print.FormatPrintString(dataRow["RgNo"].ToString(), 16, 15)
                        + print.FormatPrintString((instituteId == 5 ? dataRow["ClassRoll"].ToString() : dataRow["Caste"].ToString()), 6, 5)
                        + print.FormatPrintString(dataRow["RgSex"].ToString(), 4, 3)
                        + print.FormatPrintString(dataRow["App"].ToString(), 2)
                        + print.FormatPrintString(dataRow["SrNo"].ToString()
                        + "-" + dataRow["EmCode"].ToString(), 10, 9);
                    studentNames = print.BreakString(dataRow["RgName"].ToString(), 25, 3);

                    message = dataRow["Mesg"].ToString();
                    centerRoll = print.FormatPrintString(dataRow["Cent"].ToString(), 7)
                                + print.FormatPrintString(dataRow["RollNo"].ToString() == "0" ? "" : dataRow["RollNo"].ToString(), 10, 10, "Z")
                                + print.FormatPrintString(dataRow["Decl"].ToString(), 6)
                                + (instituteId != 13 ? "" : print.FormatPrintString(dataRow["PrefCent"].ToString(), 5)
                                + print.FormatPrintString(dataRow["Excp"].ToString().IndexOf("RPV") > -1 ? " Y" : "", 6));
                    subjects = "";
                    int lineFixedLength = 70;
                    int nextLineCounter = 70;
                    DataSet dataSet1 = reportFacade.Get_SP_GetEditFormSub_Rpt(ecId, dataRow["RgId"].Coalesce("0").ToInt32());
                    if (dataSet1.HasData())
                    {
                        foreach (DataRow dataRow1 in dataSet1.Tables[0].Rows)
                        {
                            if (dataRow1["SSrNo"].ToString() == dataRow["SrNo"].ToString())
                            {

                                if (instituteId == 16 || instituteId == 14 || instituteId == 19 || instituteId == 6 || instituteId == 17)
                                {
                                    //only appearing subjects;
                                    if (dataRow1["ExmS"].ToString() != "Y" || dataRow1["SApp"].ToString() == "R")
                                    {
                                        string code = dataRow1["Code"].ToString();
                                        string subject = (code.Length < 8 ? print.FormatPrintString(code, 9, code.Length) : print.FormatPrintString(code, 9, 8))
                                            + print.FormatPrintString((dataRow1["SApp"].ToString() == "N" ? "" : "Y"), 5, 1);
                                        if (subjects.Length > 0 && subjects.Length % nextLineCounter == 0)
                                        {
                                            subjects += "\n";
                                            nextLineCounter += lineFixedLength + 1;
                                        }
                                        subjects = subjects + subject;
                                    }
                                    else
                                    {
                                        subjects = subjects
                                            + print.FormatPrintString(dataRow1["Code"].ToString(), 9, 8)
                                            + print.FormatPrintString((dataRow1["ExmS"].ToString() == "Y" && dataRow1["SApp"].ToString() != "R" ? "P" : (dataRow1["SApp"].ToString() == "N" ? "" : "Y")), 2, 1)
                                            + print.FormatPrintString((dataRow1["ExmS"].ToString() == "Y" && dataRow1["SApp"].ToString() != "R" ? dataRow1["Marks"].ToString() : ""), 3);
                                    }
                                }
                            }
                        }
                        printResult = print.PrepareAndPrint(2, 3, "");
                        if (!printResult) goto endReport;
                        while ((subjects + line1).Trim() != "")
                        {
                            printLine = print.FormatPrintString(line1, 43) + print.FormatPrintString(subjects, 70) + print.FormatPrintString(centerRoll, 17);
                            printResult = print.PrepareAndPrint(3, 0, printLine);

                            line1 = print.FormatPrintString("", 5) + studentNames[0];
                            studentNames[0] = studentNames[1];
                            studentNames[1] = studentNames[2];
                            studentNames[2] = "";
                            subjects = subjects.SubstringWithCorrection(71, 500);
                            centerRoll = centerRoll.SubstringWithCorrection(18, 500);
                        }
                        if (message != "")
                            printResult = print.PrepareAndPrint(3, 0, print.FormatPrintString("", 5) + "Error: " + message);

                    }
                }
            nextExamForm:
                // Loop
                int x = 0;
            }
        endReport:
            print.PrepareAndPrint(9, gLnI == 1 ? 1 : 0, validationMessage);

            PrintSettings printSettings = new PrintSettings();
            printSettings.PageSize = PdfSharp.PageSize.A4;
            printSettings.PageOrientation = PdfSharp.PageOrientation.Landscape;
            return PrintLineReport(print, printSettings);
        }

        private HttpResponseMessage PrintCenterAllocation(JObject filters, JObject parameters, string emMarksD)
        {
            if (instituteId == 8)
            {
                //PfPrtCentAllo_8;
                throw new NotImplementedException();
            }

            if (instituteId == 16 && editCaCent == "1")
            {
                // PfPrtCentAllo_Cent;
                throw new NotImplementedException();
            }

            int printWidth = 79;
            int printSrNo = 0;
            List<double> printTotals = new List<double>() { 0, 0, 0, 0 };
            List<string> printGrps = new List<string>() { "", "", "", "" }; //EfPress

            string printFileName = "EditCA.Txt";
            string reportName = "EditList Centre Allocation";
            Print print = new Print();
            int pageNumber = 1;
            string printLine = print.FormatPrintString(reportName + " - " + printWidth, 50) + print.FormatPrintString(_reportPath + printFileName, 50);
            bool printResult = false;
            printResult = print.PrepareAndPrint(0, printWidth, printLine);
            if (!printResult) return new HttpResponseMessage(HttpStatusCode.NoContent);

            LegacyReportFacade reportFacade = new LegacyReportFacade(filters);
            DataSet dataSet = new DataSet();
            dataSet = reportFacade.Get_SP_SearchCentreAllo(ecId, editChange, editValid);
            if (!dataSet.HasData())
                return new HttpResponseMessage(HttpStatusCode.NoContent);

            //Store headers;
            //1;
            printLine = instituteName;
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printLine = print.FormatPrintString(printLine, printWidth - 12) + systemDate.ToString(dateFormat);
            printResult = print.PrepareAndPrint(101, 0, printLine);
            if (!printResult) goto endReport;
            //2;
            printLine = "Centre Allocation";
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printLine = print.FormatPrintString(printLine, printWidth - 12) + "Page: " + pageNumber;
            printResult = print.PrepareAndPrint(102, 0, printLine);
            if (!printResult) goto endReport;
            //2;
            printLine = reportTitle;
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printResult = print.PrepareAndPrint(103, 0, printLine);
            if (!printResult) goto endReport;
            //4;
            printLine = reportSubTitle;
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printResult = print.PrepareAndPrint(104, 0, printLine);
            if (!printResult) goto endReport;
            //5;
            printLine = options;
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printResult = print.PrepareAndPrint(105, 0, printLine);
            if (!printResult) goto endReport;
            //6;
            printResult = print.PrepareAndPrint(106, 0, "-".RepeatForLeftPadding(printWidth));
            if (!printResult) goto endReport;
            //7;
            DataRow firstRow = dataSet.FirstRow();
            string temp = (firstRow["CaCgCode"].ToString().Trim() == "" ? "Town" : "College")
                        + "-Code and Name";
            printLine = print.FormatPrintString("SrNo", 5, 4)
                    + print.FormatPrintString(temp, 36, 35)
                    + print.FormatPrintString("Candidates", 11, 10)
                    + print.FormatPrintString(" Allocated", 11, 10)
                    + print.FormatPrintString("Valid", 6, 5);
            if (editChange == "")
            {
                printResult = print.PrepareAndPrint(107, 0, printLine);
                if (!printResult) goto endReport;
            }
            // 8
            printLine = print.FormatPrintString("", 8)
                    + print.FormatPrintString("Centre-Code and Name", 33, 32)
                    + print.FormatPrintString("", 11, 10)
                    + print.FormatPrintString("", 11, 10)
                    + print.FormatPrintString("", 6, 5)
                    + print.FormatPrintString("Chng", 5, 4);
            if (editValid == "")
            {
                printResult = print.PrepareAndPrint(108, 0, printLine);
                if (!printResult) goto endReport;
            }

            // 9
            printResult = print.PrepareAndPrint(109, 0, "=".RepeatForLeftPadding(printWidth));
            if (!printResult) goto endReport;

            // Print Header
            printResult = print.PrepareAndPrint(2, 0, "");
            if (!printResult) goto endReport;
            printSrNo = 0;
            printGrps[0] = "";

            int recordCount = dataSet.Tables[0].Rows.Count;
            for (int rowCounter = 0; rowCounter < recordCount; rowCounter++)
            {
                DataRow dataRow = dataSet.Tables[0].Rows[rowCounter];
                if (dataRow["CaType"].ToString() == "T")
                {
                    printSrNo = printSrNo + 1;
                    if (printGrps[0] == "A")
                        printResult = print.PrepareAndPrint(3, 0, "");
                    printLine = print.FormatPrintString(printSrNo, 5, 4)
                        + print.FormatPrintString(dataRow["CaCgCode"].ToString() + dataRow["CaTnCode"].ToString() + " "
                        + dataRow["CgName"].ToString() + dataRow["TnName"].ToString(), 36, 35)
                        + print.FormatPrintString(dataRow["CaCand"].ToString(), 11, 10, "Z")
                        + print.FormatPrintString(dataRow["CaAllo"].ToString(), 13, 7, "Z")
                        + print.FormatPrintString(dataRow["Valid"].ToString(), 6, 3)
                        + print.FormatPrintString("", 3, 1);
                    printResult = print.PrepareAndPrint(3, 0, printLine);
                }
                else
                {
                    printLine = print.FormatPrintString("", 8, 4)
                        + print.FormatPrintString(dataRow["CaCent"].ToString() + " " + dataRow["CnName"].ToString(), 33, 32)
                        + print.FormatPrintString("", 11, 10)
                        + print.FormatPrintString(dataRow["CaAllo"].ToString(), 13, 10, "Z")
                        + print.FormatPrintString("", 6, 3)
                        + print.FormatPrintString(dataRow["Chng"].ToString(), 3, 1);
                    printResult = print.PrepareAndPrint(3, 0, printLine);
                }
                printGrps[0] = dataRow["CaType"].ToString();
            }
            printResult = print.PrepareAndPrint(3, 1, footer);

            gLnI = 0;
            validationMessage = "Printing Over";

        endReport:
            print.PrepareAndPrint(9, gLnI == 1 ? 1 : 0, validationMessage);

            PrintSettings printSettings = new PrintSettings();
            printSettings.PageSize = PdfSharp.PageSize.A4;
            printSettings.PageOrientation = PdfSharp.PageOrientation.Landscape;
            return PrintLineReport(print, printSettings);
        }

        private HttpResponseMessage PrintRollMarks(JObject filters, JObject parameters, string emMarksD)
        {
            int printWidth = 131;
            int printSrNo = 0;
            int counter = 0;
            List<double> printTotals = new List<double>() { 0, 0, 0, 0 };
            List<string> printGrps = new List<string>() { "", "", "", "" }; //EfPress

            string printFileName = "EditMSLP.Txt";
            string reportName = "Edit List of Mark Slips ";

            switch (inCode)
            {
                case 7:
                    printFileName = "EditMark.Txt";
                    reportName = "Edit List of Marks By " + (registrationRoll == "G" ? "RegnNo" : (instituteId == 13 ? "SeatNo" : "RollNo"));
                    break;
                case 8:
                    printFileName = "EditCdMk.Txt";
                    reportName = "Edit List of Marks By Code";
                    break;
                case 11:
                    printFileName = "EditStat.Txt";
                    reportName = "Edit List of Status Report";
                    break;
                case 14:
                    printFileName = "EditChMk.Txt";
                    reportName = "Edit List of Marks Change";
                    break;
                case 15:
                    printFileName = "EditSecM.Txt";
                    reportName = "Edit List of Section Marks";
                    break;
                case 16:
                    printFileName = "EditAssM.Txt";
                    reportName = "Edit List of DistAss Marks";
                    break;
            }

            Print print = new Print();
            int pageNumber = 1;
            string printLine = print.FormatPrintString(reportName + " - " + printWidth, 50) + print.FormatPrintString(_reportPath + printFileName, 50);
            bool printResult = false;
            printResult = print.PrepareAndPrint(0, printWidth, printLine);
            if (!printResult) return new HttpResponseMessage(HttpStatusCode.NoContent);

            LegacyReportFacade reportFacade = new LegacyReportFacade(filters);
            DataSet dataSet = new DataSet();
            dataSet = reportFacade.Get_SP_SearchEditMarks(ecId, editChange, editValid, subjectCode, headCode);
            if (!dataSet.HasData())
                return new HttpResponseMessage(HttpStatusCode.NoContent);

            //Store headers;
            //1;
            printLine = instituteName;
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printLine = print.FormatPrintString(printLine, printWidth - 12) + systemDate.ToString(dateFormat);
            printResult = print.PrepareAndPrint(101, 0, printLine);
            if (!printResult) goto endReport;
            //2;
            printLine = reportName + " - " + reportTitle;
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printLine = print.FormatPrintString(printLine, printWidth - 12) + "Page: " + pageNumber;
            printResult = print.PrepareAndPrint(102, 0, printLine);
            if (!printResult) goto endReport;
            //3;
            printLine = reportSubTitle;
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printResult = print.PrepareAndPrint(103, 0, printLine);
            if (!printResult) goto endReport;
            //4;
            printLine = ""; // pEditEsName;
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printResult = print.PrepareAndPrint(104, 0, printLine);
            if (!printResult) goto endReport;

            //5;
            printResult = print.PrepareAndPrint(105, 0, "-".RepeatForLeftPadding(printWidth));
            if (!printResult) goto endReport;
            //6;
            string temp = "";
            if (inCode == 8)
            {
                //counter = 5;
                counter = 4;
                temp = "  Code Sub      Marks Vld ";
                printLine = temp.RepeatForLeftPadding(counter);
            }
            else
            {
                if (registrationRoll == "G")
                {
                    //counter = 4;
                    counter = 3;
                    temp = "RegnNo        Sub     "
                        + (inCode == 11 ? "Status Vld " : " Marks Vld ");
                    printLine = temp.RepeatForLeftPadding(counter);
                }
                else if (instituteId == 17)
                {
                    counter = 3;
                    temp = "RegnNo     Sub     "
                        + (inCode == 11 ? "Status  Vld " : "  Marks    Vld   ");
                    printLine = temp.RepeatForLeftPadding(counter);
                }
                else
                {
                    //counter = 4;
                    counter = 3;
                    if (instituteId == 13)
                        temp = "    SeatNo Sub       ";
                    else
                        temp = "    RollNo Sub       ";

                    temp = temp + (inCode == 11 ? "Status Vld " : " Marks  Vld ");
                    printLine = temp.RepeatForLeftPadding(counter);
                }
            }

            printResult = print.PrepareAndPrint(106, 0, printLine);
            if (!printResult) goto endReport;
            // 7
            printResult = print.PrepareAndPrint(107, 0, "=".RepeatForLeftPadding(printWidth));
            if (!printResult) goto endReport;

            // Print Header;
            printResult = print.PrepareAndPrint(2, 0, "");
            if (!printResult) goto endReport;
            printTotals[1] = 0;
            printSrNo = 0;
            printLine = "";
            printGrps[1] = "";
            printGrps[2] = "";

            // Print Details
            int recordCount = dataSet.Tables[0].Rows.Count;
            for (int rowCounter = 0; rowCounter < recordCount; rowCounter++)
            {
                DataRow dataRow = dataSet.Tables[0].Rows[rowCounter];
                if (printSrNo == counter)
                {
                    printSrNo = 0;
                    printLine = "";
                }

                if (instituteId == 11)
                {
                    if (printGrps[1] != dataRow["CsTotErr"].ToString()
                    || printGrps[2] != dataRow["RgCgCode"].ToString())
                    {
                        if (printLine != "")
                            printResult = print.PrepareAndPrint(3, 1, printLine);
                        printSrNo = 0;
                        printLine = "";
                        printGrps[1] = dataRow["CsTotErr"].ToString();
                        printGrps[2] = dataRow["RgCgCode"].ToString();
                        printResult = print.PrepareAndPrint(3, 1, "Branch: " + printGrps[1] + "  Colg:" + printGrps[2]);
                    }
                }

                printSrNo = printSrNo + 1;
                printTotals[1] = printTotals[1] + 1;

                temp = (inCode == 8 ? print.FormatPrintString(dataRow["BarCode"].ToString(), 7, 6) :
                            (registrationRoll == "G" ? print.FormatPrintString(dataRow["RgNo"].ToString(), 14, 13) :
                             instituteId == 17 ? print.FormatPrintString(dataRow["RgNo"].ToString(), 11, 10) : print.FormatPrintString((dataRow["RollNo"].ToString() == "0" ? "" : dataRow["RollNo"].ToString()), 11, 10)))
                          + print.FormatPrintString(dataRow["Sub"].ToString() + " " + dataRow["Head"].ToString(), 8, 8)
                          + print.FormatPrintString((dataRow["CsNo"].ToString() == "0" ? "" : dataRow["CsNo"].ToString()), 2, 1, "Z")
                          + print.FormatPrintString((inCode == 11 ? " " + dataRow["Status"].ToString() : (dataRow["Status"].ToString().Trim() != "" ? dataRow["Status"].ToString() :
                            dataRow["Grade"].ToString().Trim() + emMarksD == "Y" ? dataRow["MarksD"].ToString() : dataRow["Marks"].ToString())), 8, 6)
                          + (instituteId == 13 ? print.FormatPrintString(dataRow["QpCode"].ToString(), 1) : " ")
                          + print.FormatPrintString(dataRow["Valid"].ToString(), 6, 1);
                printLine = printLine + temp;
                if (rowCounter == recordCount - 1 || printSrNo == counter)
                    printResult = print.PrepareAndPrint(3, 1, printLine);
            }

            printLine = print.FormatPrintString(" ", 13) + "Total: " + printTotals[1].ToString();
            printResult = print.PrepareAndPrint(3, 1, printLine);
            gLnI = 0;
            validationMessage = "Printing Over";

        endReport:
            print.PrepareAndPrint(9, gLnI == 1 ? 1 : 0, validationMessage);

            PrintSettings printSettings = new PrintSettings();
            printSettings.PageSize = PdfSharp.PageSize.Legal;
            printSettings.PageOrientation = PdfSharp.PageOrientation.Landscape;
            return PrintLineReport(print, printSettings);
        }

        private HttpResponseMessage PrintMarkSlips(JObject filters, JObject parameters)
        {
            int printWidth = 131;
            int printSrNo = 0;
            int counter = 0;
            List<double> printTotals = new List<double>() { 0, 0, 0, 0 };
            List<string> printGrps = new List<string>() { "", "", "", "" }; //EfPress

            string printFileName = "EditMSLP.Txt";
            string reportName = "Edit List of Mark Slips ";
            Print print = new Print();
            int pageNumber = 1;
            string printLine = print.FormatPrintString(reportName + " - " + printWidth, 50) + print.FormatPrintString(_reportPath + printFileName, 50);
            bool printResult = false;
            printResult = print.PrepareAndPrint(0, printWidth, printLine);
            if (!printResult) return new HttpResponseMessage(HttpStatusCode.NoContent);

            LegacyReportFacade reportFacade = new LegacyReportFacade(filters);
            DataSet dataSet = new DataSet();
            dataSet = reportFacade.Get_SP_SearchEditMarks(ecId, editChange, editValid, subjectCode, headCode);
            if (!dataSet.HasData())
                return new HttpResponseMessage(HttpStatusCode.NoContent);

            //Store headers;
            //1;
            printLine = reportName;
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printLine = print.FormatPrintString(printLine, printWidth - 12) + systemDate.ToString(dateFormat);
            printResult = print.PrepareAndPrint(101, 0, printLine);
            if (!printResult) goto endReport;
            //2;
            printLine = reportName + " - " + reportTitle;
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printLine = print.FormatPrintString(printLine, printWidth - 12) + "Page: " + pageNumber;
            printResult = print.PrepareAndPrint(102, 0, printLine);
            if (!printResult) goto endReport;
            //3;
            printLine = reportSubTitle;
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printResult = print.PrepareAndPrint(103, 0, printLine);
            if (!printResult) goto endReport;
            //4;
            printLine = ""; // pEditEsName;
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printResult = print.PrepareAndPrint(104, 0, printLine);
            if (!printResult) goto endReport;
            //5;
            printResult = print.PrepareAndPrint(105, 0, "-".RepeatForLeftPadding(printWidth));
            if (!printResult) goto endReport;
            //6;
            //Batch, Sr, Marks, PacketNo;
            string temp = "";
            if (editFlag.Length > 0 && editFlag.SubstringWithCorrection(editFlag.Length - 1) == "1")
            {
                counter = 4;
                temp = print.FormatPrintString("Batch", 12, 11)
                         + print.FormatPrintString("Marks", 6, 5)
                         + print.FormatPrintString("Packet", 10, 9)
                         + print.FormatPrintString("Vld", 4, 3);
                printLine = temp.RepeatForLeftPadding(counter);
            }
            else
            {
                counter = 6;
                temp = print.FormatPrintString("Batch", 12, 11)
                         + print.FormatPrintString("Marks", 6, 5)
                         + print.FormatPrintString("Vld", 4, 3);
                printLine = temp.RepeatForLeftPadding(counter);
            }
            printResult = print.PrepareAndPrint(106, 0, printLine);
            if (!printResult) goto endReport;
            //7;
            printResult = print.PrepareAndPrint(107, 0, "=".RepeatForLeftPadding(printWidth));
            if (!printResult) goto endReport;

            //Print Header;
            printResult = print.PrepareAndPrint(2, 0, "");
            if (!printResult) goto endReport;
            printTotals[1] = 0;
            printSrNo = 0;
            printLine = "";

            int recordCount = dataSet.Tables[0].Rows.Count;
            for (int rowCounter = 0; rowCounter < recordCount; rowCounter++)
            {
                DataRow dataRow = dataSet.Tables[0].Rows[rowCounter];
                if (printSrNo == counter)
                {
                    printSrNo = 0;
                    printLine = "";
                }
                printSrNo = printSrNo + 1;
                printTotals[1] = printTotals[1] + 1;

                if (editFlag.Length > 0 && editFlag.SubstringWithCorrection(editFlag.Length - 1) == "1")
                {
                    temp = print.FormatPrintString(dataRow["Batch"].ToString() + " " + dataRow["Sr"].ToString(), 14, 13)
                         + print.FormatPrintString(dataRow["Marks"].ToString(), 4, 3)
                         + print.FormatPrintString(dataRow["PacketNo"].ToString(), 12, 11)
                         + print.FormatPrintString(dataRow["Valid"].ToString(), 2, 1);
                }
                else
                {
                    temp = print.FormatPrintString(dataRow["Batch"].ToString() + " " + dataRow["Sr"].ToString(), 14, 13)
                         + print.FormatPrintString(dataRow["Marks"].ToString(), 4, 3)
                         + print.FormatPrintString(" " + dataRow["Valid"].ToString(), 4, 3);
                }
                printLine = printLine + temp;
                if (rowCounter == recordCount - 1 || printSrNo == counter)
                {
                    printResult = print.PrepareAndPrint(3, 1, printLine);
                }
            }
            printLine = print.FormatPrintString(" ", 13) + "Total: " + printTotals[1].ToString();
            printResult = print.PrepareAndPrint(3, 1, printLine);
            gLnI = 0;
            validationMessage = "Printing Over";

        endReport:
            print.PrepareAndPrint(9, gLnI == 1 ? 1 : 0, validationMessage);

            PrintSettings printSettings = new PrintSettings();
            printSettings.PageSize = PdfSharp.PageSize.A4;
            printSettings.PageOrientation = PdfSharp.PageOrientation.Landscape;
            return PrintLineReport(print, printSettings);
        }

        private HttpResponseMessage PrintNameSlips(JObject filters, JObject parameters)
        {
            int printWidth = 79;
            int printSrNo = 0;
            List<double> printTotals = new List<double>() { 0, 0, 0, 0 };
            List<string> printGrps = new List<string>() { "", "", "", "" }; //EfPress

            string printFileName = "EditNSLP.Txt";
            string reportName = "Edit List of Name Slips ";
            Print print = new Print();
            int pageNumber = 1;
            string printLine = print.FormatPrintString(reportName + " - " + printWidth, 50) + print.FormatPrintString(_reportPath + printFileName, 50);
            bool printResult = false;
            printResult = print.PrepareAndPrint(0, printWidth, printLine);
            if (!printResult) return new HttpResponseMessage(HttpStatusCode.NoContent);

            LegacyReportFacade reportFacade = new LegacyReportFacade(filters);
            DataSet dataSet = new DataSet();
            dataSet = reportFacade.Get_SP_SearchEditMarks(ecId, editChange, editValid, subjectCode, headCode);
            if (!dataSet.HasData())
                return new HttpResponseMessage(HttpStatusCode.NoContent);

            // Store headers
            // 1
            printLine = reportName;
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printLine = print.FormatPrintString(printLine, printWidth - 12) + systemDate.ToString(dateFormat);
            printResult = print.PrepareAndPrint(101, 0, printLine);
            if (!printResult) goto endReport;
            // 2
            printLine = reportName + " - " + reportTitle;
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printLine = print.FormatPrintString(printLine, printWidth - 12) + "Page: " + pageNumber;
            printResult = print.PrepareAndPrint(102, 0, printLine);
            if (!printResult) goto endReport;
            // 3
            printLine = reportSubTitle;
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printResult = print.PrepareAndPrint(103, 0, printLine);
            if (!printResult) goto endReport;
            // 4
            printLine = ""; // pEditEsName;
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printResult = print.PrepareAndPrint(104, 0, printLine);
            if (!printResult) goto endReport;
            // 5
            printResult = print.PrepareAndPrint(105, 0, "-".RepeatForLeftPadding(printWidth));
            if (!printResult) goto endReport;
            // 6
            // Batch, Sr, RollNo/RgNo, Sub, Head, Disp: RgName, EfCgCode
            if (instituteId == 11)
            {
                printLine = print.FormatPrintString("Batch", 14, 13)
                         + print.FormatPrintString("     BarNo", 16, 15)
                         + print.FormatPrintString("Sub", 8, 7)
                         + print.FormatPrintString("Colg", 8, 7)
                         + print.FormatPrintString("Name", 30, 29)
                         + print.FormatPrintString("Vld", 4, 3);
            }
            else
            {
                printLine = print.FormatPrintString("Batch", 14, 13)
                         + print.FormatPrintString("RegnNo", 16, 15)
                         + print.FormatPrintString("Sub", 8, 7)
                         + print.FormatPrintString("Colg", 8, 7)
                         + print.FormatPrintString("Name", 30, 29)
                         + print.FormatPrintString("Vld", 4, 3);
            }
            printResult = print.PrepareAndPrint(106, 0, printLine);
            if (!printResult) goto endReport;
            // 7
            printResult = print.PrepareAndPrint(107, 0, "=".RepeatForLeftPadding(printWidth));
            if (!printResult) goto endReport;
            // Print Header
            printResult = print.PrepareAndPrint(2, 0, "");
            if (!printResult) goto endReport;
            printTotals[1] = 0;
            printSrNo = 0;

            int recordCount = dataSet.Tables[0].Rows.Count;
            for (int rowCounter = 0; rowCounter < recordCount; rowCounter++)
            {
                DataRow dataRow = dataSet.Tables[0].Rows[rowCounter];
                printSrNo = printSrNo + 1;
                printTotals[1] = printTotals[1] + 1;

                printLine = print.FormatPrintString(dataRow["Batch"].ToString() + " " + dataRow["Sr"].ToString(), 14, 13)
                     + (instituteId == 11 ? print.FormatPrintString(dataRow["CsNo"].ToString(), 16, 10) : print.FormatPrintString(dataRow["RgNo"].ToString(), 16, 15))
                     + print.FormatPrintString(dataRow["Sub"].ToString() + " " + dataRow["Head"].ToString(), 8, 7)
                     + print.FormatPrintString(dataRow["RgCgCode"].ToString(), 8, 7)
                     + print.FormatPrintString(dataRow["RgName"].ToString(), 30, 29)
                     + print.FormatPrintString(dataRow["Valid"].ToString(), 4, 3);
                printResult = print.PrepareAndPrint(3, 1, printLine);
            }
            printLine = print.FormatPrintString(" ", 13) + "Total: " + printTotals[1].ToString();
            printResult = print.PrepareAndPrint(3, 1, printLine);

            gLnI = 0;
            validationMessage = "Printing Over";

        endReport:
            print.PrepareAndPrint(9, gLnI == 1 ? 1 : 0, validationMessage);

            PrintSettings printSettings = new PrintSettings();
            printSettings.PageSize = PdfSharp.PageSize.A4;
            printSettings.PageOrientation = PdfSharp.PageOrientation.Landscape;
            return PrintLineReport(print, printSettings);
        }

        private HttpResponseMessage PrintResult_17(JObject filters)
        {
            int printWidth = 75;
            int printSrNo = 0;
            List<double> printTotals = new List<double>() { 0, 0, 0, 0 };
            List<string> printGrps = new List<string>() { "", "", "", "" }; //EfPress

            List<double> totalStudents = new List<double>() { 0, 0, 0 };
            List<double> absentStudents = new List<double>() { 0, 0, 0 };
            List<double> withHoldStudents = new List<double>() { 0, 0, 0 };
            List<double> passStudents = new List<double>() { 0, 0, 0 };
            List<double> failStudents = new List<double>() { 0, 0, 0 };

            string printFileName = "Result.txt";
            string reportName = "";
            Print print = new Print();
            int pageNumber = 1;
            string printLine = print.FormatPrintString(reportName + " - " + printWidth, 50) + print.FormatPrintString(_reportPath + printFileName, 50);
            bool printResult = false;
            printResult = print.PrepareAndPrint(0, printWidth, printLine);
            if (!printResult) return new HttpResponseMessage(HttpStatusCode.NoContent);

            LegacyReportFacade reportFacade = new LegacyReportFacade(filters);
            DataSet dataSet = new DataSet();
            dataSet = reportFacade.Result_17(scheduleId, streamId);
            if (!dataSet.HasData())
                return new HttpResponseMessage(HttpStatusCode.NoContent);

            // A4 height
            printResult = print.PrepareAndPrint(5, 64, "");

            // Store headers
            // 1
            printLine = instituteName;
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printResult = print.PrepareAndPrint(101, 4, printLine);
            if (!printResult) goto endReport;

            // 2
            printLine = "Result Date: " + resultDate;
            printLine = print.FormatPrintString("", printWidth - printLine.Length) + printLine;
            printResult = print.PrepareAndPrint(102, 0, printLine);
            if (!printResult) goto endReport;

            // 3
            printLine = ("RESULT OF THE " + examName).SubstringWithCorrection(0, 80);
            printLine = printLine.Trim();
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printResult = print.PrepareAndPrint(103, 0, printLine);
            if (!printResult) goto endReport;

            // 4
            printLine = " Exam Held in " + examHeldIn;
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printLine = print.FormatPrintString(printLine, printWidth - 12) + "Page: " + pageNumber;
            printResult = print.PrepareAndPrint(104, 0, printLine);
            if (!printResult) goto endReport;

            // 5
            printLine = "-".RepeatForLeftPadding(printWidth);
            printResult = print.PrepareAndPrint(105, 0, printLine);

            // 6
            DataRow firstRow = dataSet.FirstRow();
            printGrps[1] = firstRow["StrId"].ToString();
            printLine = firstRow["StrName"].ToString();
            printResult = print.PrepareAndPrint(106, 0, printLine);

            // 7
            printGrps[2] = firstRow["EfPress"].ToString();
            printLine = firstRow["RmRemarks"].ToString() + " : RegnNos";
            printResult = print.PrepareAndPrint(107, 1, printLine);
            //;
            printLine = "Utmost care has been taken in declaring the result. However, if any error";
            printResult = print.PrepareAndPrint(601, 2, printLine);
            printLine = "is detected, it will be corrected as per university rules.";
            printResult = print.PrepareAndPrint(602, 0, printLine);

            totalStudents[1] = firstRow["TotM"].ToString().ToDouble();
            totalStudents[2] = firstRow["TotF"].ToString().ToDouble();
            totalStudents[0] = totalStudents[1] + totalStudents[2];
            absentStudents[1] = firstRow["AbsM"].ToString().ToDouble();
            absentStudents[2] = firstRow["AbsF"].ToString().ToDouble();
            absentStudents[0] = absentStudents[1] + absentStudents[2];
            withHoldStudents[1] = firstRow["WhM"].ToString().ToDouble();
            withHoldStudents[2] = firstRow["WhF"].ToString().ToDouble();
            withHoldStudents[0] = withHoldStudents[1] + withHoldStudents[2];
            passStudents[1] = firstRow["PassM"].ToString().ToDouble();
            passStudents[2] = firstRow["PassF"].ToString().ToDouble();
            passStudents[0] = passStudents[1] + passStudents[2];
            failStudents[1] = firstRow["FailM"].ToString().ToDouble();
            failStudents[2] = firstRow["FailF"].ToString().ToDouble();
            failStudents[0] = failStudents[1] + failStudents[2];

            // Print Header
            printResult = print.PrepareAndPrint(2, 0, "");
            if (!printResult) goto endReport;

            printSrNo = 0;
            printTotals[1] = 0;
            printLine = "";

            int recordCount = dataSet.Tables[0].Rows.Count;
            for (int rowCounter = 0; rowCounter < recordCount; rowCounter++)
            {
                DataRow dataRow = dataSet.Tables[0].Rows[rowCounter];

                if (printGrps[1] != dataRow["StrId"].ToString()
                    || printGrps[2] != dataRow["EfPress"].ToString())
                {
                    // 6
                    printLine = dataRow["StrName"].ToString();
                    printResult = print.PrepareAndPrint(106, 0, printLine);
                    // 7
                    printLine = dataRow["RmRemarks"].ToString() + " : RegnNos";
                    printResult = print.PrepareAndPrint(107, 1, printLine);

                    printResult = print.PrepareAndPrint(2, 5, "");
                    if (!printResult) goto endReport;

                    if (!print.pageSkipped)
                    {
                        if (printGrps[1] != dataRow["StrId"].ToString())
                        {
                            printLine = dataRow["StrName"].ToString();
                            printResult = print.PrepareAndPrint(3, 2, printLine);
                        }
                        if (printGrps[2] != dataRow["EfPress"].ToString())
                        {
                            printLine = dataRow["RmRemarks"].ToString() + ": RegnNos";
                            printResult = print.PrepareAndPrint(3, 1, printLine);
                            printResult = print.PrepareAndPrint(3, 0, "");
                        }
                    }

                    printGrps[1] = dataRow["StrId"].ToString();
                    printGrps[2] = dataRow["EfPress"].ToString();

                    totalStudents[1] = dataRow["TotM"].ToString().ToDouble();
                    totalStudents[2] = dataRow["TotF"].ToString().ToDouble();
                    totalStudents[0] = totalStudents[1] + totalStudents[2];
                    absentStudents[1] = dataRow["AbsM"].ToString().ToDouble();
                    absentStudents[2] = dataRow["AbsF"].ToString().ToDouble();
                    absentStudents[0] = absentStudents[1] + absentStudents[2];
                    withHoldStudents[1] = dataRow["WhM"].ToString().ToDouble();
                    withHoldStudents[2] = dataRow["WhF"].ToString().ToDouble();
                    withHoldStudents[0] = withHoldStudents[1] + withHoldStudents[2];
                    passStudents[1] = dataRow["PassM"].ToString().ToDouble();
                    passStudents[2] = dataRow["PassF"].ToString().ToDouble();
                    passStudents[0] = passStudents[1] + passStudents[2];
                    failStudents[1] = dataRow["FailM"].ToString().ToDouble();
                    failStudents[2] = dataRow["FailF"].ToString().ToDouble();
                    failStudents[0] = failStudents[1] + failStudents[2];

                    printSrNo = 0;
                    printTotals[1] = 0;
                    printLine = "";
                }
                printSrNo = printSrNo + 1;
                printTotals[1] = printTotals[1] + 1;
                printLine = printLine + print.FormatPrintString(dataRow["RgNo"].ToString(), 15, 14);

                printTotalCounter = 0;
                //rowCounter++;
                //if (recordCount == rowCounter) // EOF
                if (recordCount - 1 == rowCounter)
                {
                    printTotalCounter = 4;
                }
                else if (printGrps[1] != dataRow["StrId"].ToString())
                {
                    printTotalCounter = 3;
                }
                else if (printGrps[2] != dataRow["EfPress"].ToString())
                {
                    printTotalCounter = 2;
                }
                else
                {
                    if (printSrNo == 5)
                        printTotalCounter = 1;
                }

                if (printTotalCounter > 0)
                {
                    printResult = print.PrepareAndPrint(3, 1, printLine);
                    printSrNo = 0;
                    printLine = "";
                }
                if (printTotalCounter > 1)
                {
                    printLine = "    Total: " + print.FormatPrintString(printTotals[1], 6);
                    printResult = print.PrepareAndPrint(3, 0, printLine);
                    printResult = print.PrepareAndPrint(3, 0, "");
                    printTotals[1] = 0;
                }

                if (printTotalCounter > 2)
                {
                    printResult = print.PrepareAndPrint(2, 10, "");
                    printLine = " ".RepeatForLeftPadding(10) + "Result Statistics  " + "Candidates " + "  Male " + "FeMale ";
                    printResult = print.PrepareAndPrint(3, 1, printLine);
                    printLine = " ".RepeatForLeftPadding(10) + "-----------------  " + "---------- " + "------ " + "------ ";
                    printResult = print.PrepareAndPrint(3, 1, printLine);
                    printLine = " ".RepeatForLeftPadding(10) + "Total              " + print.FormatPrintString(totalStudents[0], 11, 10, "0", true)
                         + print.FormatPrintString(totalStudents[1], 7, 6, "0", true) + print.FormatPrintString(totalStudents[2], 7, 6, "0", true);
                    printResult = print.PrepareAndPrint(3, 0, printLine);
                    printLine = " ".RepeatForLeftPadding(10) + "Absent             " + print.FormatPrintString(absentStudents[0], 11, 10, "0", true)
                                    + print.FormatPrintString(absentStudents[1], 7, 6, "0", true) + print.FormatPrintString(absentStudents[2], 7, 6, "0", true);
                    printResult = print.PrepareAndPrint(3, 0, printLine);

                    double gLnI = totalStudents[0] - absentStudents[0];
                    double glnJ = totalStudents[1] - absentStudents[1];
                    double glnK = totalStudents[2] - absentStudents[2];
                    printLine = " ".RepeatForLeftPadding(10) + "Appeared           " + print.FormatPrintString(gLnI, 11, 10, "0", true)
                         + print.FormatPrintString(glnJ, 7, 6, "0", true) + print.FormatPrintString(glnK, 7, 6, "0", true);
                    printResult = print.PrepareAndPrint(3, 0, printLine);

                    printLine = " ".RepeatForLeftPadding(10) + "Result Withheld    " + print.FormatPrintString(withHoldStudents[0], 11, 10, "0", true)
                         + print.FormatPrintString(withHoldStudents[1], 7, 6, "0", true) + print.FormatPrintString(withHoldStudents[2], 7, 6, "0", true);
                    printResult = print.PrepareAndPrint(3, 0, printLine);

                    printLine = " ".RepeatForLeftPadding(10) + "Clear              " + print.FormatPrintString(passStudents[0], 11, 10, "0", true)
                         + print.FormatPrintString(passStudents[1], 7, 6, "0", true) + print.FormatPrintString(passStudents[2], 7, 6, "0", true);
                    printResult = print.PrepareAndPrint(3, 0, printLine);

                    printLine = " ".RepeatForLeftPadding(10) + "Not Clear          " + print.FormatPrintString(failStudents[0], 11, 10, "0", true)
                        + print.FormatPrintString(failStudents[1], 7, 6, "0", true) + print.FormatPrintString(failStudents[2], 7, 6, "0", true);
                    printResult = print.PrepareAndPrint(3, 0, printLine);

                    double gdblTemp1 = 0;
                    double gdblTemp2 = 0;
                    double gdblTemp3 = 0;
                    if (passStudents[0] + failStudents[0] != 0)
                        gdblTemp1 = Math.Round(passStudents[0] / (passStudents[0] + failStudents[0]) * 0.01 * 10000, 2);
                    if (passStudents[1] + failStudents[1] != 0)
                        gdblTemp2 = Math.Round(passStudents[1] / (passStudents[1] + failStudents[1]) * 0.01 * 10000, 2);
                    if (passStudents[2] + failStudents[2] != 0)
                        gdblTemp3 = Math.Round(passStudents[2] / (passStudents[2] + failStudents[2]) * 0.01 * 10000, 2);
                    printLine = " ".RepeatForLeftPadding(10) + "Clear%             " + print.FormatPrintString(gdblTemp1, 11, 10, "2", true)
                         + print.FormatPrintString(gdblTemp2, 7, 6, "2", true) + print.FormatPrintString(gdblTemp3, 7, 6, "2", true);
                    printResult = print.PrepareAndPrint(3, 0, printLine);

                    printResult = print.PrepareAndPrint(2, 5, "");
                    printLine = print.FormatPrintString(instituteName, 50) + authorityName;
                    printResult = print.PrepareAndPrint(3, 3, printLine);
                    printLine = print.FormatPrintString("Result Date: " + resultDate, 50) + authorityDesignation;
                    printResult = print.PrepareAndPrint(3, 0, printLine);
                }
            }
            gLnI = 0;
            validationMessage = "Printing Over";

        endReport:
            print.PrepareAndPrint(9, gLnI == 1 ? 1 : 0, validationMessage);

            PrintSettings printSettings = new PrintSettings();
            printSettings.PageSize = PdfSharp.PageSize.A4;
            printSettings.PageOrientation = PdfSharp.PageOrientation.Landscape;
            return PrintLineReport(print, printSettings);
        }

        private HttpResponseMessage PrintTehsilList(JObject filters)
        {
            JObject json;
            string reportName = "";
            try
            {
                json = JObject.Parse(extra1);
                reportName = json.SelectToken("MenuName").Coalesce();
            }
            catch { }
            int printWidth = 66;
            int printSrNo = 0;
            List<double> printTotals = new List<double>() { 0, 0, 0, 0 };
            List<string> printGrps = new List<string>() { "", "", "", "" };
            string printFileName = "Tehsil.txt";
            //string reportName = "";
            Print print = new Print();
            int pageNumber = 1;
            string printLine = print.FormatPrintString(reportName + " - " + printWidth, 50) + print.FormatPrintString(_reportPath + printFileName, 50);
            bool printResult = false;
            printResult = print.PrepareAndPrint(0, printWidth, printLine);
            if (!printResult) return new HttpResponseMessage(HttpStatusCode.NoContent);
            LegacyReportFacade reportFacade = new LegacyReportFacade(filters);
            DataSet dataSet = new DataSet();

            dataSet = reportFacade.GetTehsilList();
            if (!dataSet.HasData())
            {
                gLnI = 1;
                return new HttpResponseMessage(HttpStatusCode.NoContent);
            }

            // Store headers
            // 1
            printLine = printFileName.Replace(".txt", "");
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printResult = print.PrepareAndPrint(101, 0, printLine);
            if (!printResult) goto endReport;

            // 1
            printLine = instituteName;
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printResult = print.PrepareAndPrint(101, 1, printLine);
            if (!printResult) goto endReport;

            // 2
            printLine = "TEHSIL LIST";
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printLine = print.FormatPrintString(printLine, printWidth - 12) + "Page: " + pageNumber;
            printResult = print.PrepareAndPrint(102, 0, printLine);
            if (!printResult) goto endReport;
            // 3
            printResult = print.PrepareAndPrint(103, 1, "-".RepeatForLeftPadding(printWidth));
            if (!printResult) goto endReport;
            // 7
            printLine = "     " + print.FormatPrintString("Code", 11) + "Name";
            printResult = print.PrepareAndPrint(104, 0, printLine);
            if (!printResult) goto endReport;
            // 8
            printResult = print.PrepareAndPrint(105, 0, "=".RepeatForLeftPadding(printWidth));
            if (!printResult) goto endReport;

            // Print Header
            printResult = print.PrepareAndPrint(2, 0, "");
            if (!printResult) goto endReport;

            // Print Details
            printSrNo = 0;
            int recordCount = dataSet.Tables[0].Rows.Count;
            for (int rowCounter = 0; rowCounter < recordCount; rowCounter++)
            {
                DataRow dataRow = dataSet.Tables[0].Rows[rowCounter];
                printSrNo = printSrNo + 1;

                printSrNo = printSrNo + 1;
                printLine = print.FormatPrintString(printSrNo, 5, 4)
                    + print.FormatPrintString(dataRow["TsCode"].ToString(), 11, 5)
                    + print.FormatPrintString(dataRow["TsName"].ToString(), 50, 50);
                printResult = print.PrepareAndPrint(3, 0, printLine);
            }
            gLnI = 0;
            validationMessage = "Printing Over";

        endReport:
            print.PrepareAndPrint(9, gLnI == 1 ? 1 : 0, validationMessage);
            PrintSettings printSettings = new PrintSettings();
            printSettings.PageSize = PdfSharp.PageSize.Ledger;
            printSettings.PageOrientation = PdfSharp.PageOrientation.Portrait;
            return PrintLineReport(print, printSettings);
        }

        private HttpResponseMessage PrintTownsList(JObject filters)
        {
            JObject json;
            string reportName = "";
            try
            {
                json = JObject.Parse(extra1);
                reportName = json.SelectToken("MenuName").Coalesce();
            }
            catch { }
            int printWidth = 66;
            int printSrNo = 0;
            List<double> printTotals = new List<double>() { 0, 0, 0, 0 };
            List<string> printGrps = new List<string>() { "", "", "", "" };
            string printFileName = "Towns.txt";
            //string reportName = "";
            Print print = new Print();
            int pageNumber = 1;
            string printLine = print.FormatPrintString(reportName + " - " + printWidth, 50) + print.FormatPrintString(_reportPath + printFileName, 50);
            bool printResult = false;
            printResult = print.PrepareAndPrint(0, printWidth, printLine);
            if (!printResult) return new HttpResponseMessage(HttpStatusCode.NoContent);
            LegacyReportFacade reportFacade = new LegacyReportFacade(filters);
            DataSet dataSet = new DataSet();

            dataSet = reportFacade.GetTownList();
            if (!dataSet.HasData())
            {
                gLnI = 1;
                return new HttpResponseMessage(HttpStatusCode.NoContent);
            }

            // Store headers
            // 1
            printLine = printFileName.Replace(".txt", "");
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printResult = print.PrepareAndPrint(101, 0, printLine);
            if (!printResult) goto endReport;

            // 1
            printLine = instituteName;
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printResult = print.PrepareAndPrint(101, 1, printLine);
            if (!printResult) goto endReport;

            // 2
            printLine = "TOWNS LIST";
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printLine = print.FormatPrintString(printLine, printWidth - 12) + "Page: " + pageNumber;
            printResult = print.PrepareAndPrint(102, 0, printLine);
            if (!printResult) goto endReport;
            // 3
            printResult = print.PrepareAndPrint(103, 1, "-".RepeatForLeftPadding(printWidth));
            if (!printResult) goto endReport;
            // 7
            printLine = "     " + print.FormatPrintString("Code", 11) + "Name";
            printResult = print.PrepareAndPrint(104, 0, printLine);
            if (!printResult) goto endReport;
            // 8
            printResult = print.PrepareAndPrint(105, 0, "=".RepeatForLeftPadding(printWidth));
            if (!printResult) goto endReport;

            // Print Header
            printResult = print.PrepareAndPrint(2, 0, "");
            if (!printResult) goto endReport;

            // Print Details
            printSrNo = 0;
            int recordCount = dataSet.Tables[0].Rows.Count;
            for (int rowCounter = 0; rowCounter < recordCount; rowCounter++)
            {
                DataRow dataRow = dataSet.Tables[0].Rows[rowCounter];
                printSrNo = printSrNo + 1;

                printSrNo = printSrNo + 1;
                printLine = print.FormatPrintString(printSrNo, 5, 4)
                    + print.FormatPrintString(dataRow["TnCode"].ToString(), 11, 5)
                    + print.FormatPrintString(dataRow["TnName"].ToString(), 50, 50);
                printResult = print.PrepareAndPrint(3, 0, printLine);
            }
            gLnI = 0;
            validationMessage = "Printing Over";

        endReport:
            print.PrepareAndPrint(9, gLnI == 1 ? 1 : 0, validationMessage);
            PrintSettings printSettings = new PrintSettings();
            printSettings.PageSize = PdfSharp.PageSize.Ledger;
            printSettings.PageOrientation = PdfSharp.PageOrientation.Portrait;
            return PrintLineReport(print, printSettings);
        }

        private HttpResponseMessage PrintDivisionList(JObject filters)
        {
            JObject json;
            string reportName = "";
            try
            {
                json = JObject.Parse(extra1);
                reportName = json.SelectToken("MenuName").Coalesce();
            }
            catch { }
            int printWidth = 66;
            int printSrNo = 0;
            List<double> printTotals = new List<double>() { 0, 0, 0, 0 };
            List<string> printGrps = new List<string>() { "", "", "", "" };
            string printFileName = "Division.txt";
            //string reportName = "";
            Print print = new Print();
            int pageNumber = 1;
            string printLine = print.FormatPrintString(reportName + " - " + printWidth, 50) + print.FormatPrintString(_reportPath + printFileName, 50);
            bool printResult = false;
            printResult = print.PrepareAndPrint(0, printWidth, printLine);
            if (!printResult) return new HttpResponseMessage(HttpStatusCode.NoContent);
            LegacyReportFacade reportFacade = new LegacyReportFacade(filters);
            DataSet dataSet = new DataSet();

            dataSet = reportFacade.GetDivisionList();
            if (!dataSet.HasData())
            {
                gLnI = 1;
                return new HttpResponseMessage(HttpStatusCode.NoContent);
            }

            // Store headers
            // 1
            printLine = printFileName.Replace(".txt", "");
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printResult = print.PrepareAndPrint(101, 0, printLine);
            if (!printResult) goto endReport;

            // 1
            printLine = instituteName;
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printResult = print.PrepareAndPrint(101, 1, printLine);
            if (!printResult) goto endReport;

            // 2
            printLine = "DIVISION LIST";
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printLine = print.FormatPrintString(printLine, printWidth - 12) + "Page: " + pageNumber;
            printResult = print.PrepareAndPrint(102, 0, printLine);
            if (!printResult) goto endReport;
            // 3
            printResult = print.PrepareAndPrint(103, 1, "-".RepeatForLeftPadding(printWidth));
            if (!printResult) goto endReport;
            // 7
            printLine = "     " + print.FormatPrintString("Code", 11) + "Name";
            printResult = print.PrepareAndPrint(104, 0, printLine);
            if (!printResult) goto endReport;
            // 8
            printResult = print.PrepareAndPrint(105, 0, "=".RepeatForLeftPadding(printWidth));
            if (!printResult) goto endReport;

            // Print Header
            printResult = print.PrepareAndPrint(2, 0, "");
            if (!printResult) goto endReport;

            // Print Details
            printSrNo = 0;
            int recordCount = dataSet.Tables[0].Rows.Count;
            for (int rowCounter = 0; rowCounter < recordCount; rowCounter++)
            {
                DataRow dataRow = dataSet.Tables[0].Rows[rowCounter];
                printSrNo = printSrNo + 1;

                printSrNo = printSrNo + 1;
                printLine = print.FormatPrintString(printSrNo, 5, 4)
                    + print.FormatPrintString(dataRow["DivisionCode"].ToString(), 11, 5)
                    + print.FormatPrintString(dataRow["DivisionName"].ToString(), 50, 50);
                printResult = print.PrepareAndPrint(3, 0, printLine);
            }
            gLnI = 0;
            validationMessage = "Printing Over";

        endReport:
            print.PrepareAndPrint(9, gLnI == 1 ? 1 : 0, validationMessage);
            PrintSettings printSettings = new PrintSettings();
            printSettings.PageSize = PdfSharp.PageSize.Ledger;
            printSettings.PageOrientation = PdfSharp.PageOrientation.Portrait;
            return PrintLineReport(print, printSettings);
        }

        private HttpResponseMessage PrintDistrictList(JObject filters)
        {
            JObject json;
            string reportName = "";
            try
            {
                json = JObject.Parse(extra1);
                reportName = json.SelectToken("MenuName").Coalesce();
            }
            catch { }
            int printWidth = 66;
            int printSrNo = 0;
            List<double> printTotals = new List<double>() { 0, 0, 0, 0 };
            List<string> printGrps = new List<string>() { "", "", "", "" };
            string printFileName = "District.txt";
            //string reportName = "";
            Print print = new Print();
            //int pageNumber = 1;
            string printLine = print.FormatPrintString(reportName + " - " + printWidth, 50) + print.FormatPrintString(_reportPath + printFileName, 50);
            bool printResult = false;
            printResult = print.PrepareAndPrint(0, printWidth, printLine);
            if (!printResult) return new HttpResponseMessage(HttpStatusCode.NoContent);
            LegacyReportFacade reportFacade = new LegacyReportFacade(filters);
            DataSet dataSet = new DataSet();

            dataSet = reportFacade.GetDistrictList();
            if (!dataSet.HasData())
            {
                gLnI = 1;
                return new HttpResponseMessage(HttpStatusCode.NoContent);
            }

            // Store headers
            // 1
            printLine = printFileName.Replace(".txt", "");
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printResult = print.PrepareAndPrint(101, 0, printLine);
            if (!printResult) goto endReport;

            // 1
            printLine = instituteName;
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printResult = print.PrepareAndPrint(101, 1, printLine);
            if (!printResult) goto endReport;

            // 2
            printLine = "DISTRICTS LIST";
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printLine = print.FormatPrintString(printLine, printWidth - 12) + "Page: ~~Pg";
            printResult = print.PrepareAndPrint(102, 0, printLine);
            if (!printResult) goto endReport;
            // 3
            printResult = print.PrepareAndPrint(103, 1, "-".RepeatForLeftPadding(printWidth));
            if (!printResult) goto endReport;
            // 7
            printLine = "Sr   " + print.FormatPrintString("Code", 11) + "Name";
            printResult = print.PrepareAndPrint(104, 0, printLine);
            if (!printResult) goto endReport;
            // 8
            printResult = print.PrepareAndPrint(105, 0, "=".RepeatForLeftPadding(printWidth));
            if (!printResult) goto endReport;

            // Print Header
            printResult = print.PrepareAndPrint(2, 0, "");
            if (!printResult) goto endReport;

            // Print Details
            printSrNo = 0;
            int recordCount = dataSet.Tables[0].Rows.Count;
            for (int rowCounter = 0; rowCounter < recordCount; rowCounter++)
            {
                DataRow dataRow = dataSet.Tables[0].Rows[rowCounter];
                printSrNo = printSrNo + 1;

                //printSrNo = printSrNo + 1;
                printLine = print.FormatPrintString(printSrNo, 5, 4)
                    + print.FormatPrintString(dataRow["DsCode"].ToString(), 11, 5)
                    + print.FormatPrintString(dataRow["DsName"].ToString(), 50, 50);
                printResult = print.PrepareAndPrint(3, 0, printLine);
            }
            gLnI = 0;
            validationMessage = "Printing Over";

        endReport:
            print.PrepareAndPrint(9, gLnI == 1 ? 1 : 0, validationMessage);
            PrintSettings printSettings = new PrintSettings();
            printSettings.PageSize = PdfSharp.PageSize.Ledger;
            printSettings.PageOrientation = PdfSharp.PageOrientation.Portrait;
            return PrintLineReport(print, printSettings);
        }

        private HttpResponseMessage PrintStateList(JObject filters)
        {
            JObject json;
            string reportName = "";
            try
            {
                json = JObject.Parse(extra1);
                reportName = json.SelectToken("MenuName").Coalesce();
            }
            catch { }
            int printWidth = 66;
            int printSrNo = 0;
            List<double> printTotals = new List<double>() { 0, 0, 0, 0 };
            List<string> printGrps = new List<string>() { "", "", "", "" };
            string printFileName = "State.txt";
            //string reportName = "";
            Print print = new Print();
            //int pageNumber = 1;
            string printLine = print.FormatPrintString(reportName + " - " + printWidth, 50) + print.FormatPrintString(_reportPath + printFileName, 50);
            bool printResult = false;
            printResult = print.PrepareAndPrint(0, printWidth, printLine);
            if (!printResult) return new HttpResponseMessage(HttpStatusCode.NoContent);
            LegacyReportFacade reportFacade = new LegacyReportFacade(filters);
            DataSet dataSet = new DataSet();

            dataSet = reportFacade.GetStateList();
            if (!dataSet.HasData())
            {
                gLnI = 1;
                return new HttpResponseMessage(HttpStatusCode.NoContent);
            }

            // Store headers
            // 1
            printLine = printFileName.Replace(".txt", "");
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printResult = print.PrepareAndPrint(101, 0, printLine);
            if (!printResult) goto endReport;

            // 1
            printLine = instituteName;
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printResult = print.PrepareAndPrint(101, 1, printLine);
            if (!printResult) goto endReport;

            // 2
            printLine = "STATES LIST";
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printLine = print.FormatPrintString(printLine, printWidth - 12) + "Page: ~~Pg";
            printResult = print.PrepareAndPrint(102, 0, printLine);
            if (!printResult) goto endReport;
            // 3
            printResult = print.PrepareAndPrint(103, 1, "-".RepeatForLeftPadding(printWidth));
            if (!printResult) goto endReport;
            // 7
            printLine = "Sr   " + print.FormatPrintString("Code", 11) + "Name";
            printResult = print.PrepareAndPrint(104, 0, printLine);
            if (!printResult) goto endReport;
            // 8
            printResult = print.PrepareAndPrint(105, 0, "=".RepeatForLeftPadding(printWidth));
            if (!printResult) goto endReport;

            // Print Header
            printResult = print.PrepareAndPrint(2, 0, "");
            if (!printResult) goto endReport;

            // Print Details
            printSrNo = 0;
            int recordCount = dataSet.Tables[0].Rows.Count;
            for (int rowCounter = 0; rowCounter < recordCount; rowCounter++)
            {
                DataRow dataRow = dataSet.Tables[0].Rows[rowCounter];
                printSrNo = printSrNo + 1;

                //printSrNo = printSrNo + 1;
                printLine = print.FormatPrintString(printSrNo, 5, 4)
                    + print.FormatPrintString(dataRow["StateCode"].ToString(), 11, 5)
                    + print.FormatPrintString(dataRow["StateName"].ToString(), 50, 50);
                printResult = print.PrepareAndPrint(3, 0, printLine);
            }
            gLnI = 0;
            validationMessage = "Printing Over";

        endReport:
            print.PrepareAndPrint(9, gLnI == 1 ? 1 : 0, validationMessage);
            PrintSettings printSettings = new PrintSettings();
            printSettings.PageSize = PdfSharp.PageSize.Ledger;
            printSettings.PageOrientation = PdfSharp.PageOrientation.Portrait;
            return PrintLineReport(print, printSettings);
        }

        private HttpResponseMessage PrintCountryList(JObject filters)
        {
            JObject json;
            string reportName = "";
            try
            {
                json = JObject.Parse(extra1);
                reportName = json.SelectToken("MenuName").Coalesce();
            }
            catch { }
            int printWidth = 66;
            int printSrNo = 0;
            List<double> printTotals = new List<double>() { 0, 0, 0, 0 };
            List<string> printGrps = new List<string>() { "", "", "", "" };
            string printFileName = "Country.txt";
            //string reportName = "";
            Print print = new Print();
            //int pageNumber = 1;
            string printLine = print.FormatPrintString(reportName + " - " + printWidth, 50) + print.FormatPrintString(_reportPath + printFileName, 50);
            bool printResult = false;
            printResult = print.PrepareAndPrint(0, printWidth, printLine);
            if (!printResult) return new HttpResponseMessage(HttpStatusCode.NoContent);
            LegacyReportFacade reportFacade = new LegacyReportFacade(filters);
            DataSet dataSet = new DataSet();

            dataSet = reportFacade.GetCountryList();
            if (!dataSet.HasData())
            {
                gLnI = 1;
                return new HttpResponseMessage(HttpStatusCode.NoContent);
            }

            // Store headers
            // 1
            printLine = printFileName.Replace(".txt", "");
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printResult = print.PrepareAndPrint(101, 0, printLine);
            if (!printResult) goto endReport;

            // 1
            printLine = instituteName;
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printResult = print.PrepareAndPrint(101, 1, printLine);
            if (!printResult) goto endReport;

            // 2
            printLine = "COUNTRY LIST";
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printLine = print.FormatPrintString(printLine, printWidth - 12) + "Page: ~~Pg";
            printResult = print.PrepareAndPrint(102, 0, printLine);
            if (!printResult) goto endReport;
            // 3
            printResult = print.PrepareAndPrint(103, 1, "-".RepeatForLeftPadding(printWidth));
            if (!printResult) goto endReport;
            // 7
            printLine = "Sr   " + print.FormatPrintString("Code", 11) + "Name";
            printResult = print.PrepareAndPrint(104, 0, printLine);
            if (!printResult) goto endReport;
            // 8
            printResult = print.PrepareAndPrint(105, 0, "=".RepeatForLeftPadding(printWidth));
            if (!printResult) goto endReport;

            // Print Header
            printResult = print.PrepareAndPrint(2, 0, "");
            if (!printResult) goto endReport;

            // Print Details
            printSrNo = 0;
            int recordCount = dataSet.Tables[0].Rows.Count;
            for (int rowCounter = 0; rowCounter < recordCount; rowCounter++)
            {
                DataRow dataRow = dataSet.Tables[0].Rows[rowCounter];
                printSrNo = printSrNo + 1;

                //printSrNo = printSrNo + 1;
                printLine = print.FormatPrintString(printSrNo, 5, 4)
                    + print.FormatPrintString(dataRow["CountryCode"].ToString(), 11, 5)
                    + print.FormatPrintString(dataRow["CountryName"].ToString(), 50, 50);
                printResult = print.PrepareAndPrint(3, 0, printLine);
            }
            gLnI = 0;
            validationMessage = "Printing Over";

        endReport:
            print.PrepareAndPrint(9, gLnI == 1 ? 1 : 0, validationMessage);
            PrintSettings printSettings = new PrintSettings();
            printSettings.PageSize = PdfSharp.PageSize.Ledger;
            printSettings.PageOrientation = PdfSharp.PageOrientation.Portrait;
            return PrintLineReport(print, printSettings);
        }

        private HttpResponseMessage PrintExamList(JObject filters)
        {
            JObject json;
            string reportName = "";
            try
            {
                json = JObject.Parse(extra1);
                reportName = json.SelectToken("MenuName").Coalesce();
            }
            catch { }
            int printWidth = 80;
            int printSrNo = 0;
            List<double> printTotals = new List<double>() { 0, 0, 0, 0 };
            List<string> printGrps = new List<string>() { "", "", "", "" };
            string printFileName = "Exams.txt";
            //string reportName = "";
            Print print = new Print();
            //int pageNumber = 1;
            string printLine = print.FormatPrintString(reportName + " - " + printWidth, 50) + print.FormatPrintString(_reportPath + printFileName, 50);
            bool printResult = false;
            printResult = print.PrepareAndPrint(0, printWidth, printLine);
            if (!printResult) return new HttpResponseMessage(HttpStatusCode.NoContent);
            LegacyReportFacade reportFacade = new LegacyReportFacade(filters);
            DataSet dataSet = new DataSet();

            dataSet = reportFacade.GetExamList(courseCode, 0);
            if (!dataSet.HasData())
            {
                gLnI = 1;
                return new HttpResponseMessage(HttpStatusCode.NoContent);
            }

            // Store headers
            // 1
            printLine = printFileName.Replace(".txt", "");
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printResult = print.PrepareAndPrint(101, 0, printLine);
            if (!printResult) goto endReport;

            // 1
            printLine = instituteName;
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printResult = print.PrepareAndPrint(101, 1, printLine);
            if (!printResult) goto endReport;

            // 2
            printLine = "EXAMS LIST";
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printLine = print.FormatPrintString(printLine, printWidth - 12) + "Page: ~~Pg";
            printResult = print.PrepareAndPrint(102, 0, printLine);
            if (!printResult) goto endReport;
            // 3
            printLine = courseCode == "" ? "" : "Course: " + courseCode + " " + examCourseName.SubstringWithCorrection(0, 70);
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printResult = print.PrepareAndPrint(103, 0, printLine);
            if (!printResult) goto endReport;
            // 4
            printResult = print.PrepareAndPrint(104, 1, "-".RepeatForLeftPadding(printWidth));
            if (!printResult) goto endReport;
            // 5
            printLine = "Sr  Code   " + print.FormatPrintString("ShortName", 21)
                    + "Eval Impr Clr Fail     Max Max  Pass Brch Lat";
            printResult = print.PrepareAndPrint(105, 0, printLine);
            if (!printResult) goto endReport;
            // 6
            printLine = print.FormatPrintString("", 11) + print.FormatPrintString("Name", 21) + "Meth Rej  Exm Sub      Sub Mark  %   Appl Entry";
            printResult = print.PrepareAndPrint(106, 0, printLine);
            if (!printResult) goto endReport;
            // 7
            printResult = print.PrepareAndPrint(107, 0, "=".RepeatForLeftPadding(printWidth));
            if (!printResult) goto endReport;

            // Print Header
            printResult = print.PrepareAndPrint(2, 0, "");
            if (!printResult) goto endReport;

            // Print Details
            printSrNo = 0;
            printTotals = new List<double>() { 0, 0, 0, 0 };
            printGrps = new List<string>() { "", "", "", "" };
            int recordCount = dataSet.Tables[0].Rows.Count;
            int serialNumber = 1;
            for (int rowCounter = 0; rowCounter < recordCount; rowCounter++)
            {
                DataRow dataRow = dataSet.Tables[0].Rows[rowCounter];
                printSrNo = printSrNo + 1;

                printLine = print.FormatPrintString(serialNumber, 4, 3)
                    + print.FormatPrintString(dataRow["EmCode"].ToString(), 7, 6)
                    + print.FormatPrintString(dataRow["EmShName"].ToString(), 21, 20)
                    + print.FormatPrintString(dataRow["EmEval"].ToString(), 5, 4)
                    + print.FormatPrintString(dataRow["EmRejImpr"].ToString(), 4, 3)
                    + print.FormatPrintString(dataRow["EmClrSrNo"].ToString(), 4, 3, "Z")
                    + print.FormatPrintString(dataRow["EmATKTSub"].ToString(), 4, 3, "Z")
                    + print.FormatPrintString(dataRow["EmElgblCd"].ToString(), 5)
                    + print.FormatPrintString(dataRow["EmTotSub"].ToString(), 4, 3, "Z")
                    + print.FormatPrintString(dataRow["EmMaxTot"].ToString(), 5, 4, "Z")
                    + print.FormatPrintString(dataRow["EmPassPC"].ToString(), 6, 5, "Z")
                    + print.FormatPrintString(dataRow["EmStreams"].ToString(), 5, 3)
                    + print.FormatPrintString(dataRow["EmLatEntry"].ToString(), 4, 3);
                printResult = print.PrepareAndPrint(3, 1, printLine);
                serialNumber++;
                List<string> stringParts = print.BreakString(dataRow["EmName"].ToString(), 65, 2);
                printLine = print.FormatPrintString("", 11) + stringParts[0];
                printResult = print.PrepareAndPrint(3, 0, printLine);

                if (stringParts[1] != "")
                {
                    printLine = print.FormatPrintString("", 11) + stringParts[1];
                    printResult = print.PrepareAndPrint(3, 0, printLine);
                }

                if (dataRow["EmDiv"].ToString() != "N")
                {
                    printLine = print.FormatPrintString("", 11) + "Cl: ";

                    DataSet data = reportFacade.GetExamDivision(dataRow["EmId"].ToString().ToInt32(), 0, "");
                    if (data.HasData())
                    {
                        foreach (DataRow dr in data.Tables[0].Rows)
                        {
                            printLine = printLine + dr["EdDiv"].ToString()
                                + (dr["EdEsThPr"].ToString() != "" ? "-" + dr["EdEsThPr"].ToString() : "")
                                + "-" + dr["EdMarksPC"].ToString() + " ";
                        }
                    }

                    DataSet dsExamAggr = reportFacade.GetExamAggregate(dataRow["EmId"].ToString().ToInt32());
                    string tempExamAggr = "";
                    if (dsExamAggr.HasData())
                    {
                        foreach (DataRow dr in dsExamAggr.Tables[0].Rows)
                        {
                            tempExamAggr += dr["EmCode"].ToString() + " ";
                        }
                    }
                    tempExamAggr = tempExamAggr.Length > 0 ? "Aggr: " + tempExamAggr : "";
                    tempExamAggr = tempExamAggr
                        + (dataRow["EmDivFirstAtt"].ToString() == "Y" ? "  FAtmpt " : "")
                        + (dataRow["EmDivFirstMrk"].ToString() == "Y" ? "FMarks " : "");
                    if (!tempExamAggr.IsNullOrWhiteSpace())
                    {
                        if ((printLine.Length + tempExamAggr.Length) > 79)
                        {
                            printResult = print.PrepareAndPrint(3, 0, printLine);
                            printLine = print.FormatPrintString("", 11) + tempExamAggr.SubstringWithCorrection(0, 69);
                        }
                        else
                        {
                            printLine = printLine + tempExamAggr;
                        }
                    }
                    printResult = print.PrepareAndPrint(3, 0, printLine);
                }
            }
            gLnI = 0;
            validationMessage = "Printing Over";

        endReport:
            print.PrepareAndPrint(9, gLnI == 1 ? 1 : 0, validationMessage);
            PrintSettings printSettings = new PrintSettings();
            printSettings.PageSize = PdfSharp.PageSize.Ledger;
            printSettings.PageOrientation = PdfSharp.PageOrientation.Portrait;
            return PrintLineReport(print, printSettings);
        }

        private HttpResponseMessage PrintCourseList(JObject filters)
        {
            JObject json;
            string reportName = "";
            try
            {
                json = JObject.Parse(extra1);
                reportName = json.SelectToken("MenuName").Coalesce();
            }
            catch { }
            int printWidth = 80;
            int printSrNo = 0;
            int printSubSrNo = 0;
            List<double> printTotals = new List<double>() { 0, 0, 0, 0 };
            List<string> printGrps = new List<string>() { "", "", "", "" };
            string printFileName = isInstitute || isSchool ? "Course Institute.txt" : "Course College";
            //string reportName = "";
            Print print = new Print();
            int pageNumber = 1;
            string printLine = print.FormatPrintString(reportName + " - " + printWidth, 50) + print.FormatPrintString(_reportPath + printFileName, 50);
            bool printResult = false;
            printResult = print.PrepareAndPrint(0, printWidth, printLine);
            if (!printResult) return new HttpResponseMessage(HttpStatusCode.NoContent);
            LegacyReportFacade reportFacade = new LegacyReportFacade(filters);
            DataSet dataSet = new DataSet();

            gLnI = instituteId == 11 ? loginId : 0;
            dataSet = reportFacade.GetCourseList(gLnI);
            if (!dataSet.HasData())
            {
                gLnI = 1;
                return new HttpResponseMessage(HttpStatusCode.NoContent);
            }

            // Store headers
            // 1
            printLine = printFileName.Replace(".txt", "");
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printResult = print.PrepareAndPrint(101, 0, printLine);
            if (!printResult) goto endReport;

            // 1
            printLine = instituteName;
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printResult = print.PrepareAndPrint(101, 1, printLine);
            if (!printResult) goto endReport;

            // 2
            printLine = "COURSES LIST";
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printLine = print.FormatPrintString(printLine, printWidth - 12) + "Page: " + pageNumber;
            printResult = print.PrepareAndPrint(102, 0, printLine);
            if (!printResult) goto endReport;
            // 3
            printResult = print.PrepareAndPrint(103, 1, "-".RepeatForLeftPadding(printWidth));
            if (!printResult) goto endReport;
            // 5
            printLine = " Sr " + print.FormatPrintString("Code", 6) + print.FormatPrintString("Name", 66) + "Prfx";
            printResult = print.PrepareAndPrint(104, 0, printLine);
            if (!printResult) goto endReport;
            // 6
            printLine = print.FormatPrintString("", 10) + print.FormatPrintString("", 27) + print.FormatPrintString("Pass%", 6) + print.FormatPrintString("Pattern", 8)
                + print.FormatPrintString("Yrs", 4) + print.FormatPrintString("Exms", 5) + print.FormatPrintString("MaxYr", 6)
                + print.FormatPrintString("MaxSn", 6) + print.FormatPrintString("Med", 4);
            printResult = print.PrepareAndPrint(105, 0, printLine);
            if (!printResult) goto endReport;
            // 7
            printResult = print.PrepareAndPrint(106, 0, "=".RepeatForLeftPadding(printWidth));
            if (!printResult) goto endReport;

            // Print Header
            printGrps[1] = "";
            printResult = print.PrepareAndPrint(2, 0, "");
            if (!printResult) goto endReport;

            // Print Details
            printSrNo = 0;
            printTotals = new List<double>() { 0, 0, 0, 0 };
            printGrps = new List<string>() { "", "", "", "" };
            int recordCount = dataSet.Tables[0].Rows.Count;
            for (int rowCounter = 0; rowCounter < recordCount; rowCounter++)
            {
                DataRow dataRow = dataSet.Tables[0].Rows[rowCounter];
                printSrNo = printSrNo + 1;
                if (printGrps[1] != dataRow["CoFcCode"].ToString())
                {
                    printGrps[1] = dataRow["CoFcCode"].ToString();
                    printLine = printSrNo + " " + dataRow["CoFcCode"].ToString() + " " + dataRow["FcDesc"].ToString();
                    printResult = print.PrepareAndPrint(3, 1, printLine);
                }

                if (printGrps[1] == dataRow["CoFcCode"].ToString())
                {
                    printSubSrNo++;
                }

                List<string> stringParts = print.BreakString(dataRow["CoName"].ToString(), 65, 2);
                printLine = print.FormatPrintString(printSubSrNo, 4, 2)
                    + print.FormatPrintString(dataRow["CoCode"].ToString(), 6, 5)
                    + print.FormatPrintString(stringParts[0], 66, 65)
                    + dataRow["CoRgPrfx"].ToString();
                printResult = print.PrepareAndPrint(3, 0, printLine);

                string courseMediums = dataRow["CoMediums"].ToString();
                if (courseMediums.Length > 1)
                {
                    courseMediums = courseMediums.SubstringWithCorrection(0, courseMediums.Length - 1);
                }
                if (courseMediums.Length > 1)
                {
                    courseMediums = courseMediums.SubstringWithCorrection(1);
                }

                printLine = print.FormatPrintString("", 10) + print.FormatPrintString(stringParts[1], 27, 26)
                    + print.FormatPrintString(dataRow["CoPassPc"].ToString(), 6, 5, "2Z")
                    + print.FormatPrintString(dataRow["CoPattern"].ToString(), 8, 7)
                    + print.FormatPrintString(dataRow["CoYears"].ToString(), 4, 3)
                    + print.FormatPrintString(dataRow["CoExams"].ToString(), 5, 4)
                    + print.FormatPrintString(dataRow["CoMaxYr"].ToString(), 6, 5, "Z")
                    + print.FormatPrintString(dataRow["CoMaxSn"].ToString(), 6, 5, "Z")
                    + print.FormatPrintString(courseMediums, 12, 11);
                printResult = print.PrepareAndPrint(3, 0, printLine);

                DataSet data = new DataSet();
                data = reportFacade.GetExamStream(dataRow["CoCode"].ToString(), 0);
                if (data.HasData())
                {
                    printLine = print.FormatPrintString("", 10) + "Branches: ";
                    foreach (DataRow dr in data.Tables[0].Rows)
                    {
                        printLine = printLine + print.FormatPrintString(dr["StrCode"].ToString(), 7, 5)
                        + print.FormatPrintString(dr["StrName"].ToString(), 49) + dr["StrSpFlag"].ToString();
                        printResult = print.PrepareAndPrint(3, 0, printLine);
                        printLine = print.FormatPrintString("", 20);
                    }
                }
                printResult = print.PrepareAndPrint(3, 0, "");
            }
            gLnI = 0;
            validationMessage = "Printing Over";

        endReport:
            print.PrepareAndPrint(9, gLnI == 1 ? 1 : 0, validationMessage);
            PrintSettings printSettings = new PrintSettings();
            printSettings.PageSize = PdfSharp.PageSize.Ledger;
            printSettings.PageOrientation = PdfSharp.PageOrientation.Portrait;
            return PrintLineReport(print, printSettings);
        }

        private HttpResponseMessage PrintCourseCollegeMapping(JObject filters)
        {
            JObject json;
            string reportName = "";
            int collegeEmId = 0;
            string courseEm = "C";
            string byCollege = "";
            try
            {
                json = JObject.Parse(extra1);
                reportName = json.SelectToken("MenuName").Coalesce();
                byCollege = json.SelectToken("ByOp").Coalesce("Y");
            }
            catch { }

            int printWidth = 80;
            int printSrNo = 0;
            List<double> printTotals = new List<double>() { 0, 0, 0, 0 };
            List<string> printGrps = new List<string>() { "", "", "", "" };
            string printFileName = isInstitute || isSchool ? "Course Institute.txt" : "Course College";
            //string reportName = "";
            Print print = new Print();
            //int pageNumber = 1;
            string printLine = print.FormatPrintString(reportName + " - " + printWidth, 50) + print.FormatPrintString(_reportPath + printFileName, 50);
            bool printResult = false;
            printResult = print.PrepareAndPrint(0, printWidth, printLine);
            if (!printResult) return new HttpResponseMessage(HttpStatusCode.NoContent);
            LegacyReportFacade reportFacade = new LegacyReportFacade(filters);
            DataSet dataSet = new DataSet();

            dataSet = reportFacade.GetCourseCollegeList(collegeCode, collegeEmId, courseCode, courseEm, byCollege);
            if (!dataSet.HasData())
                return new HttpResponseMessage(HttpStatusCode.NoContent);

            // Store headers
            // 1
            printLine = isInstitute || isSchool ? "Course Institute" : "Course College"; ;
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printResult = print.PrepareAndPrint(101, 0, printLine);
            if (!printResult) goto endReport;

            // 1
            printLine = instituteName;
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printResult = print.PrepareAndPrint(101, 1, printLine);
            if (!printResult) goto endReport;

            // 2
            printLine = reportName;
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printLine = print.FormatPrintString(printLine, printWidth - 12) + "Page: ~~Pg";
            printResult = print.PrepareAndPrint(102, 0, printLine);
            if (!printResult) goto endReport;
            // 3
            printResult = print.PrepareAndPrint(103, 1, "-".RepeatForLeftPadding(printWidth));
            if (!printResult) goto endReport;

            // Print Header
            printResult = print.PrepareAndPrint(2, 0, "");
            if (!printResult) goto endReport;

            // Print Details
            printSrNo = 0;
            printTotals = new List<double>() { 0, 0, 0, 0 };
            printGrps = new List<string>() { "", "", "", "" };

            int recordCount = dataSet.Tables[0].Rows.Count;
            for (int rowCounter = 0; rowCounter < recordCount; rowCounter++)
            {
                DataRow dataRow = dataSet.Tables[0].Rows[rowCounter];
                printSrNo = printSrNo + 1;

                if (byCollege == "Y")
                {
                    if (printGrps[1] != dataRow["CgName"].ToString())
                    {
                        printGrps[1] = dataRow["CgName"].ToString();
                        printLine = print.FormatPrintString(dataRow["CgCode"].ToString() + " " + dataRow["CgName"].ToString(), 76);
                        printResult = print.PrepareAndPrint(3, 1, printLine);
                    }
                    else
                    {
                        if (printGrps[1] != dataRow["Code"].ToString())
                        {
                            printGrps[1] = dataRow["Code"].ToString();
                            printLine = print.FormatPrintString(dataRow["Code"].ToString(), 6, 5) + print.FormatPrintString(dataRow["Name"].ToString(), 70);
                            printResult = print.PrepareAndPrint(3, 1, printLine);
                        }
                    }

                    if (byCollege == "Y")
                    {
                        printLine = "    " + print.FormatPrintString(dataRow["Code"].ToString(), 6, 5) + print.FormatPrintString(dataRow["Name"].ToString(), 66);
                    }
                    else
                    {
                        printLine = "    " + print.FormatPrintString(dataRow["CgCode"].ToString() + " " + dataRow["CgName"].ToString(), 72);
                    }
                    printResult = print.PrepareAndPrint(3, 0, printLine);

                }
            }
            gLnI = 0;
            validationMessage = "Printing Over";

        endReport:
            print.PrepareAndPrint(9, gLnI == 1 ? 1 : 0, validationMessage);
            PrintSettings printSettings = new PrintSettings();
            printSettings.PageSize = PdfSharp.PageSize.Ledger;
            printSettings.PageOrientation = PdfSharp.PageOrientation.Portrait;
            return PrintLineReport(print, printSettings);
        }

        private HttpResponseMessage PrintCollegesList(JObject filters)
        {
            JObject json;
            string reportName = "";
            string districtCode = "";
            string townCode = "";
            string collegeType = "";
            string collegeCenter = "";
            string onlyCenter = "";
            string withAddr = "";
            string collegeName = "";
            string hslcAhm = "";
            string searchString = "";
            int pageSize = 2000;
            int pageNumber = 1;
            try
            {
                json = JObject.Parse(extra1);
                reportName = json.SelectToken("MenuName").Coalesce();
                districtCode = json.SelectToken("Ds").Coalesce();
                townCode = json.SelectToken("Tn").Coalesce();
                collegeType = json.SelectToken("Type").Coalesce();
                collegeCenter = json.SelectToken("Cent").Coalesce();
                onlyCenter = json.SelectToken("OnlyCent").Coalesce();
                withAddr = json.SelectToken("WithAddr").Coalesce();
                hslcAhm = json.SelectToken("HslcAhm").Coalesce();
                searchString = json.SelectToken("SearchString").Coalesce();
                pageSize = json.SelectToken("PageSize").Coalesce("2000").ToInt32();
                pageNumber = json.SelectToken("PageNumber").Coalesce("1").ToInt32();
            }
            catch { }
            int printWidth = 132;
            int printSrNo = 0;

            List<double> printTotals = new List<double>() { 0, 0, 0, 0 };
            List<string> printGrps = new List<string>() { "", "", "", "" };
            string printFileName = isInstitute || isSchool ? "Institute.txt" : "Colleges.txt";
            Print print = new Print();
            //int pageNumber = 1;
            string printLine = print.FormatPrintString(reportName + " - " + printWidth, 50) + print.FormatPrintString(_reportPath + printFileName, 50);
            bool printResult = false;
            printResult = print.PrepareAndPrint(0, printWidth, printLine);
            if (!printResult) return new HttpResponseMessage(HttpStatusCode.NoContent);
            LegacyReportFacade reportFacade = new LegacyReportFacade(filters);
            DataSet dataSet = new DataSet();

            dataSet = reportFacade.GetCollegeList(collegeCode, collegeName, dsCode.Coalesce(districtCode), tnCode.Coalesce(townCode), collegeCenter, onlyCenter, collegeType, hslcAhm, searchString);
            if (!dataSet.HasData())
                return new HttpResponseMessage(HttpStatusCode.NoContent);
            //Store headers;
            // 1
            printLine = reportName;
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printResult = print.PrepareAndPrint(101, 0, printLine);
            if (!printResult) goto endReport;

            // 1
            printLine = instituteName;
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printResult = print.PrepareAndPrint(101, 1, printLine);
            if (!printResult) goto endReport;

            // 2
            printLine = (isSchool || isInstitute ? "INSTITUTE LIST" : "COLLEGE LIST");
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printLine = print.FormatPrintString(printLine, printWidth - 12) + "Page: " + pageNumber;
            printResult = print.PrepareAndPrint(102, 0, printLine);
            if (!printResult) goto endReport;
            // 3
            printLine = (hslcAhm == "M" ? "  Madrassa" : (hslcAhm == "H" ? "  HSLC" : ""))
                + (collegeCenter == "Y" ? "  Centers" : "")
                + (onlyCenter == "Y" ? "  Only Centres" : "")
                + (collegeType != "" ? "   Type: " + collegeType : "");
            if (printLine != "")
                printLine = "(" + printLine + ")";
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printResult = print.PrepareAndPrint(103, 0, printLine);
            // 4
            DataRow firstRow = dataSet.FirstRow();
            printGrps[1] = firstRow["DsCode"].ToString();
            printLine = "District: " + printGrps[1] + " " + firstRow["DsName"].ToString();
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printResult = print.PrepareAndPrint(104, 0, printLine);
            // 5
            printResult = print.PrepareAndPrint(105, 1, "-".RepeatForLeftPadding(printWidth));
            if (!printResult) goto endReport;
            // 6
            printLine = print.FormatPrintString("SrNo", 5)
                    + print.FormatPrintString("Code", 9, 8)
                    + print.FormatPrintString("Name", 81, 80)
                    + print.FormatPrintString("ShortName", 21, 20);
            ;
            ;
            printResult = print.PrepareAndPrint(106, 0, printLine);
            if (!printResult) goto endReport;
            // 8
            printResult = print.PrepareAndPrint(107, 0, "=".RepeatForLeftPadding(printWidth));
            if (!printResult) goto endReport;

            // Print Header
            printResult = print.PrepareAndPrint(2, 0, "");
            if (!printResult) goto endReport;

            // Print Details
            printSrNo = 0;
            printTotals = new List<double>() { 0, 0, 0, 0 };
            printGrps = new List<string>() { "", "", "", "" };
            int recordCount = dataSet.Tables[0].Rows.Count;
            for (int rowCounter = 0; rowCounter < recordCount; rowCounter++)
            {
                DataRow dataRow = dataSet.Tables[0].Rows[rowCounter];
                //printSrNo = printSrNo + 1;

                if (!printGrps[1].IsNullOrWhiteSpace() && printGrps[1] != dataRow["DsCode"].ToString())
                {
                    printGrps[1] = dataRow["DsCode"].ToString();
                    printLine = "District: " + printGrps[1] + " " + dataRow["DsName"].ToString();
                    printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
                    printResult = print.PrepareAndPrint(104, 0, printLine);
                    printResult = print.PrepareAndPrint(2, 99, "");

                    printGrps[2] = "";
                    printSrNo = 0;
                }

                printSrNo = printSrNo + 1;
                printTotals[1] = printTotals[1] + 1;
                printLine = print.FormatPrintString(printSrNo, 5, 4)
                    + print.FormatPrintString(dataRow["CgCode"].ToString(), 9, 8)
                    + print.FormatPrintString(dataRow["CgName"].ToString(), 81, 80)
                    + print.FormatPrintString(dataRow["CgShName"].ToString(), 21, 20);

                printLine = printLine + print.FormatPrintString(dataRow["CgS1Cent"].ToString(), 5, 4) + print.FormatPrintString(dataRow["CgOnlyCent"].ToString(), 9, 8);
                printResult = print.PrepareAndPrint(3, 0, printLine);

                if (withAddr == "Y")
                {
                    printLine = print.FormatPrintString("", 14) + dataRow["CgAddr1"].ToString() + ", "
                        + dataRow["CgAddr2"].ToString() + ", " + dataRow["CgAddr3"].ToString() + ", "
                        + dataRow["CgLoca"].ToString() + " Pin: " + (instituteId == 7 ? dataRow["CgWeb"].ToString() : dataRow["CgPin"].ToString());
                    if ((printLine + " " + " Phone: " + dataRow["CgPhone"].ToString()).Length <= 132)
                    {
                        printLine = printLine + " " + " Phone: " + dataRow["CgPhone"].ToString();
                    }
                    else
                    {
                        printResult = print.PrepareAndPrint(3, 0, printLine);
                        printLine = print.FormatPrintString("", 14) + " " + " Phone: " + dataRow["CgPhone"].ToString();
                    }
                    if ((printLine + " " + " Fax: " + dataRow["CgFax"].ToString()).Length <= 132)
                    {
                        printLine = printLine + " " + " Fax: " + dataRow["CgFax"].ToString();
                    }
                    else
                    {
                        printResult = print.PrepareAndPrint(3, 0, printLine);
                        printLine = print.FormatPrintString("", 14) + " " + " Fax: " + dataRow["CgFax"].ToString();
                    }
                    if ((printLine + " " + " EMail: " + dataRow["CgEmail"].ToString()).Length <= 132)
                    {
                        printLine = printLine + " " + " EMail: " + dataRow["CgEmail"].ToString();
                    }
                    else
                    {
                        printResult = print.PrepareAndPrint(3, 0, printLine);
                        printLine = print.FormatPrintString("", 14) + " " + " EMail: " + dataRow["CgEmail"].ToString();
                    }
                    if (instituteId != 7)
                    {
                        if ((printLine + " " + " Web: " + dataRow["CgWeb"].ToString()).Length <= 132)
                        {
                            printLine = printLine + " " + " Web: " + dataRow["CgWeb"].ToString();
                        }
                        else
                        {
                            printResult = print.PrepareAndPrint(3, 0, printLine);
                            printLine = print.FormatPrintString("", 14) + " " + " Web: " + dataRow["CgWeb"].ToString();
                        }
                    }
                    if (printLine != "")
                    {
                        printResult = print.PrepareAndPrint(3, 0, printLine);
                    }
                    printResult = print.PrepareAndPrint(3, 0, "");
                }

                printTotalCounter = 0;
                if (rowCounter == recordCount - 1)
                    printTotalCounter = 1;
                else if (printGrps[1] != dataRow["DsCode"].ToString())
                    printTotalCounter = 2;

                if (printTotalCounter == 1)
                {
                    printLine = " Total : " + printTotals[1];
                    printResult = print.PrepareAndPrint(3, 1, printLine);
                }
            }

            gLnI = 0;
            validationMessage = "Printing Over";

        endReport:
            print.PrepareAndPrint(9, gLnI == 1 ? 1 : 0, validationMessage);
            PrintSettings printSettings = new PrintSettings();
            printSettings.PageSize = PdfSharp.PageSize.Ledger;
            printSettings.PageOrientation = PdfSharp.PageOrientation.Portrait;
            return PrintLineReport(print, printSettings);
        }

        private HttpResponseMessage PrintFacultyList(JObject filters)
        {
            JObject json;
            string reportName = "";
            try
            {
                json = JObject.Parse(extra1);
                reportName = json.SelectToken("MenuName").Coalesce();
            }
            catch { }
            int printWidth = 66;
            int printSrNo = 0;

            List<double> printTotals = new List<double>() { 0, 0, 0, 0 };
            List<string> printGrps = new List<string>() { "", "", "", "" };
            string printFileName = "Faculty.txt";
            //string reportName = "";
            Print print = new Print();
            int pageNumber = 1;
            string printLine = print.FormatPrintString(reportName + " - " + printWidth, 50) + print.FormatPrintString(_reportPath + printFileName, 50);
            bool printResult = false;
            printResult = print.PrepareAndPrint(0, printWidth, printLine);
            if (!printResult) return new HttpResponseMessage(HttpStatusCode.NoContent);
            LegacyReportFacade reportFacade = new LegacyReportFacade(filters);
            DataSet dataSet = new DataSet();

            dataSet = reportFacade.GetFacultyList();
            if (!dataSet.HasData())
                return new HttpResponseMessage(HttpStatusCode.NoContent);
            // Store headers;
            // 1
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printResult = print.PrepareAndPrint(101, 0, printLine);
            if (!printResult) goto endReport;

            // 1
            printLine = instituteName;
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printResult = print.PrepareAndPrint(101, 1, printLine);
            if (!printResult) goto endReport;

            // 2
            printLine = "FACULTY LIST";
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            //printLine = print.FormatPrintString(printLine, printWidth - 12) + "Page: " + pageNumber;
            printLine = print.FormatPrintString(printLine, printWidth - 12) + "Page: " + pageNumber;
            printResult = print.PrepareAndPrint(102, 0, printLine);
            if (!printResult) goto endReport;
            // 3
            printResult = print.PrepareAndPrint(103, 1, "-".RepeatForLeftPadding(printWidth));
            if (!printResult) goto endReport;
            // 7
            printLine = "Sr   " + print.FormatPrintString("Code", 11) + "Name";
            printResult = print.PrepareAndPrint(104, 0, printLine);
            if (!printResult) goto endReport;
            // 8
            printResult = print.PrepareAndPrint(105, 0, "=".RepeatForLeftPadding(printWidth));
            if (!printResult) goto endReport;

            // Print Header
            printResult = print.PrepareAndPrint(2, 0, "");
            if (!printResult) goto endReport;

            // Print Details
            printSrNo = 0;
            int recordCount = dataSet.Tables[0].Rows.Count;
            for (int rowCounter = 0; rowCounter < recordCount; rowCounter++)
            {
                DataRow dataRow = dataSet.Tables[0].Rows[rowCounter];
                printSrNo = printSrNo + 1;

                //printSrNo = printSrNo + 1;
                printLine = print.FormatPrintString(printSrNo, 5, 4)
                    + print.FormatPrintString(dataRow["FcCode"].ToString(), 11, 5)
                    + print.FormatPrintString(dataRow["FcDesc"].ToString(), 50, 50);
                printResult = print.PrepareAndPrint(3, 0, printLine);
            }
            gLnI = 0;
            validationMessage = "Printing Over";

        endReport:
            print.PrepareAndPrint(9, gLnI == 1 ? 1 : 0, validationMessage);
            PrintSettings printSettings = new PrintSettings();
            printSettings.PageSize = PdfSharp.PageSize.Ledger;
            printSettings.PageOrientation = PdfSharp.PageOrientation.Portrait;
            return PrintLineReport(print, printSettings);
        }

        private HttpResponseMessage PrintCourseEditSchedule(JObject filters)
        {
            JObject json;
            string allLast = "A";
            string reportName = "";
            try
            {
                json = JObject.Parse(extra1);
                reportName = json.SelectToken("MenuName").Coalesce();
                allLast = json.SelectToken("AllLast").Coalesce("A");
                inCode = json.SelectToken("InCode").ToString().ToInt32();
                ecId = json.SelectToken("EcId").ToString().ToInt32();
            }
            catch { }
            int printWidth = 110;
            int printSrNo = 0;

            List<double> printTotals = new List<double>() { 0, 0, 0, 0 };
            List<string> printGrps = new List<string>() { "", "", "", "" };
            string printFileName = "CourseEditSchedule.txt";
            //string reportName = "";
            Print print = new Print();
            int pageNumber = 1;
            string printLine = print.FormatPrintString(reportName + " - " + printWidth, 50) + print.FormatPrintString(_reportPath + printFileName, 50);
            bool printResult = false;
            printResult = print.PrepareAndPrint(0, printWidth, printLine);
            if (!printResult) return new HttpResponseMessage(HttpStatusCode.NoContent);
            LegacyReportFacade reportFacade = new LegacyReportFacade(filters);
            DataSet dataSet = new DataSet();

            dataSet = reportFacade.GetCourseEditSchedules(ecId, inCode, allLast);
            if (!dataSet.HasData())
                return new HttpResponseMessage(HttpStatusCode.NoContent);

            //Store headers;
            //1;
            printLine = reportName;
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printResult = print.PrepareAndPrint(101, 0, printLine);
            if (!printResult) goto endReport;

            // 1
            printLine = instituteName;
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printResult = print.PrepareAndPrint(101, 1, printLine);
            if (!printResult) goto endReport;

            //2;
            printLine = scheduleId + " " + sessionName;
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printResult = print.PrepareAndPrint(102, 0, printLine);
            if (!printResult) goto endReport;
            //3;
            printLine = "INPUT LIST" + (allLast == "A" ? " (All)" : (allLast == "L" ? " (Last Inputs)" : " (" + inCode + ")"));
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printLine = print.FormatPrintString(printLine, printWidth - 12) + "Page: " + pageNumber;
            printResult = print.PrepareAndPrint(103, 0, printLine);
            if (!printResult) goto endReport;
            //4
            printResult = print.PrepareAndPrint(104, 1, "-".RepeatForLeftPadding(printWidth));
            if (!printResult) goto endReport;
            //5
            printLine = print.FormatPrintString("  Sr", 5, 4)
                    + print.FormatPrintString("Input/Process", 31, 30)
                    + print.FormatPrintString("List", 5, 4)
                    + print.FormatPrintString("Desc", 31, 30)
                    + print.FormatPrintString("Operator", 11, 10)
                    + print.FormatPrintString("OpenedBy", 11, 10)
                    + print.FormatPrintString("Stat", 5, 4)
                    + print.FormatPrintString("UpdatedBy", 11, 10);
            printResult = print.PrepareAndPrint(105, 0, printLine);
            if (!printResult) goto endReport;
            //6
            printResult = print.PrepareAndPrint(106, 0, "=".RepeatForLeftPadding(printWidth));
            if (!printResult) goto endReport;

            //Print Header
            printResult = print.PrepareAndPrint(2, 0, "");
            if (!printResult) goto endReport;

            // Print Details
            printSrNo = 0;
            int recordCount = dataSet.Tables[0].Rows.Count;
            for (int rowCounter = 0; rowCounter < recordCount; rowCounter++)
            {
                DataRow dataRow = dataSet.Tables[0].Rows[rowCounter];
                printSrNo = printSrNo + 1;
                printLine = print.FormatPrintString(printSrNo, 5, 4)
                    + print.FormatPrintString(dataRow["InDesc"].ToString(), 31, 30)
                    + print.FormatPrintString(dataRow["CEcListNo"].ToString(), 5, 4)
                    + print.FormatPrintString(dataRow["CEcDesc"].ToString(), 31, 30)
                    + print.FormatPrintString(dataRow["L1"].ToString(), 11, 10)
                    + print.FormatPrintString(dataRow["L2"].ToString(), 11, 10)
                    + print.FormatPrintString(dataRow["CEcStatus"].ToString(), 5, 4)
                    + print.FormatPrintString(dataRow["L3"].ToString(), 11, 10);
                printResult = print.PrepareAndPrint(3, 0, printLine);
            }
        endReport:
            print.PrepareAndPrint(9, gLnI == 1 ? 1 : 0, validationMessage);
            PrintSettings printSettings = new PrintSettings();
            printSettings.PageSize = PdfSharp.PageSize.Ledger;
            printSettings.PageOrientation = PdfSharp.PageOrientation.Portrait;
            return PrintLineReport(print, printSettings);
        }

        private HttpResponseMessage PrintCourseSchedule(JObject filters)
        {
            JObject json;
            bool onlyForCurrentUser = true;
            string reportName = "";
            try
            {
                json = JObject.Parse(extra1);
                reportName = json.SelectToken("MenuName").Coalesce();
                onlyForCurrentUser = json.SelectToken("AllSchedules").Coalesce("0").ToBoolean();
            }
            catch { }
            int printWidth = 80;
            int printSrNo = 0;

            List<double> printTotals = new List<double>() { 0, 0, 0, 0 };
            List<string> printGrps = new List<string>() { "", "", "", "" };
            string printFileName = "CourseSchedule.txt";
            //string reportName = "";
            Print print = new Print();
            int pageNumber = 1;
            string printLine = print.FormatPrintString(reportName + " - " + printWidth, 50) + print.FormatPrintString(_reportPath + printFileName, 50);
            bool printResult = false;
            printResult = print.PrepareAndPrint(0, printWidth, printLine);
            if (!printResult) return new HttpResponseMessage(HttpStatusCode.NoContent);
            LegacyReportFacade reportFacade = new LegacyReportFacade(filters);
            DataSet dataSet = new DataSet();

            dataSet = reportFacade.GetCourseSchedules(sessionNo, (onlyForCurrentUser ? loginId : 0));
            if (!dataSet.HasData())
                return new HttpResponseMessage(HttpStatusCode.NoContent);

            //Store headers;
            //1;
            printLine = reportName;
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printResult = print.PrepareAndPrint(101, 0, printLine);
            if (!printResult) goto endReport;

            // 1
            printLine = instituteName;
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printResult = print.PrepareAndPrint(101, 1, printLine);
            if (!printResult) goto endReport;

            //2;
            printLine = reportName + " - " + sessionName;
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printResult = print.PrepareAndPrint(102, 0, printLine);
            if (!printResult) goto endReport;
            //3;
            printLine = (onlyForCurrentUser ? " Supevisor: " + loginName : "");
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printLine = print.FormatPrintString(printLine, printWidth - 12) + "Page: " + pageNumber;
            printResult = print.PrepareAndPrint(103, 0, printLine);
            if (!printResult) goto endReport;
            //4;
            printResult = print.PrepareAndPrint(104, 1, "-".RepeatForLeftPadding(printWidth));
            if (!printResult) goto endReport;
            //5;
            printLine = print.FormatPrintString(" Sr", 4) + print.FormatPrintString("Code", 6)
                     + print.FormatPrintString("Name", 41) + print.FormatPrintString("Status", 11) + print.FormatPrintString("Image path", 28);
            printResult = print.PrepareAndPrint(105, 0, printLine);
            if (!printResult) goto endReport;
            //6;
            printResult = print.PrepareAndPrint(106, 0, "=".RepeatForLeftPadding(printWidth));
            if (!printResult) goto endReport;

            //Print Header;
            printResult = print.PrepareAndPrint(2, 0, "");
            if (!printResult) goto endReport;

            // Print Details
            printSrNo = 0;
            int recordCount = dataSet.Tables[0].Rows.Count;
            for (int rowCounter = 0; rowCounter < recordCount; rowCounter++)
            {
                DataRow dataRow = dataSet.Tables[0].Rows[rowCounter];
                printSrNo = printSrNo + 1;
                printLine = print.FormatPrintString(printSrNo, 4, 3)
                         + print.FormatPrintString(dataRow["CoCode"].ToString(), 6, 5)
                         + print.FormatPrintString(dataRow["CoName"].ToString(), 41, 20)
                         + print.FormatPrintString(dataRow["CShStatusDesc"].ToString(), 11, 10)
                         + print.FormatPrintString(dataRow["CShPath"].ToString(), 28, 28);
                printResult = print.PrepareAndPrint(3, 0, printLine);
            }
            gLnI = 0;
            validationMessage = "Printing Over";

        endReport:
            print.PrepareAndPrint(9, gLnI == 1 ? 1 : 0, validationMessage);
            PrintSettings printSettings = new PrintSettings();
            printSettings.PageSize = PdfSharp.PageSize.Ledger;
            printSettings.PageOrientation = PdfSharp.PageOrientation.Portrait;
            return PrintLineReport(print, printSettings);
        }

        private HttpResponseMessage PrintExamSchedule(JObject filters)
        {
            JObject json;
            bool onlyForCurrentUser = true;
            string reportName = "";
            try
            {
                json = JObject.Parse(extra1);
                reportName = json.SelectToken("MenuName").Coalesce();
                onlyForCurrentUser = json.SelectToken("AllSchedules").Coalesce("0").ToBoolean();
            }
            catch { }
            int printWidth = 80;
            int printSrNo = 0;

            List<double> printTotals = new List<double>() { 0, 0, 0, 0 };
            List<string> printGrps = new List<string>() { "", "", "", "" };
            string printFileName = "ExamSchedule.txt";
            //string reportName = "";
            Print print = new Print();
            int pageNumber = 1;
            string printLine = print.FormatPrintString(reportName + " - " + printWidth, 50) + print.FormatPrintString(_reportPath + printFileName, 50);
            bool printResult = false;
            printResult = print.PrepareAndPrint(0, printWidth, printLine);
            if (!printResult) return new HttpResponseMessage(HttpStatusCode.NoContent);
            LegacyReportFacade reportFacade = new LegacyReportFacade(filters);
            DataSet dataSet = new DataSet();

            dataSet = reportFacade.GetExamSchedules(sessionNo, (onlyForCurrentUser ? loginId : 0));
            if (!dataSet.HasData())
                return new HttpResponseMessage(HttpStatusCode.NoContent);

            //Store headers;
            //1;
            printLine = reportName;
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printResult = print.PrepareAndPrint(101, 0, printLine);
            if (!printResult) goto endReport;

            // 1
            printLine = instituteName;
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printResult = print.PrepareAndPrint(101, 1, printLine);
            if (!printResult) goto endReport;

            //2;
            printLine = reportName + " - " + sessionName;
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printResult = print.PrepareAndPrint(102, 0, printLine);
            if (!printResult) goto endReport;
            //3;
            printLine = (onlyForCurrentUser ? " Supevisor: " + loginName : "");
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printLine = print.FormatPrintString(printLine, printWidth - 12) + "Page: " + pageNumber;
            printResult = print.PrepareAndPrint(103, 0, printLine);
            if (!printResult) goto endReport;
            //4;
            printResult = print.PrepareAndPrint(104, 1, "-".RepeatForLeftPadding(printWidth));
            if (!printResult) goto endReport;
            //5;
            printLine = print.FormatPrintString(" Sr", 4) + print.FormatPrintString("Code", 6) + print.FormatPrintString("Name", 21)
                    + print.FormatPrintString("Status", 18) + print.FormatPrintString("Image Path", 27)
                    + (instituteId == 16 ? "Pref" : "");
            printResult = print.PrepareAndPrint(105, 0, printLine);
            if (!printResult) goto endReport;
            // 6
            printResult = print.PrepareAndPrint(106, 0, "=".RepeatForLeftPadding(printWidth));
            if (!printResult) goto endReport;

            //Print Header
            printResult = print.PrepareAndPrint(2, 0, "");
            if (!printResult) goto endReport;

            // Print Details
            printSrNo = 0;
            int recordCount = dataSet.Tables[0].Rows.Count;
            for (int rowCounter = 0; rowCounter < recordCount; rowCounter++)
            {
                DataRow dataRow = dataSet.Tables[0].Rows[rowCounter];
                printSrNo = printSrNo + 1;
                printLine = print.FormatPrintString(printSrNo, 4, 3)
                         + print.FormatPrintString(dataRow["EmCode"].ToString(), 6, 5)
                         + print.FormatPrintString(dataRow["EmShName"].ToString(), 21, 20)
                         + print.FormatPrintString(dataRow["ShStatusDesc"].ToString(), 18, 17)
                         + print.FormatPrintString(dataRow["ShPath"].ToString(), 27, 26)
                         + (instituteId == 16 ? dataRow["ShExtr1"].ToString() : "");
                printResult = print.PrepareAndPrint(3, 0, printLine);
            }
            gLnI = 0;
            validationMessage = "Printing Over";

        endReport:
            print.PrepareAndPrint(9, gLnI == 1 ? 1 : 0, validationMessage);
            PrintSettings printSettings = new PrintSettings();
            printSettings.PageSize = PdfSharp.PageSize.Ledger;
            printSettings.PageOrientation = PdfSharp.PageOrientation.Portrait;
            return PrintLineReport(print, printSettings);
        }

        private HttpResponseMessage PrintExamEditControl(JObject filters)
        {
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.InternalServerError);

            JObject json;
            try
            {
                json = JObject.Parse(extra1);
                inCode = json.SelectToken("InCode").ToString().ToInt32();
                ecId = json.SelectToken("EcId").ToString().ToInt32();
                //ecShId = json.SelectToken("EcShId").ToString().ToInt32();
                reportName = json.SelectToken("StrTitle").Coalesce(reportName);
                reportSubTitle = json.SelectToken("StrSubTitle").Coalesce(reportSubTitle);
            }
            catch { }

            switch (instituteId)
            {
                case 6:
                case 9:
                case 12:
                    result = PrintCrystalReport("ExInput.rpt", null, filters);
                    break;
                default:
                    int printWidth = 80;
                    int printSrNo = 0;
                    List<double> printTotals = new List<double>() { 0, 0, 0, 0 };
                    List<string> printGrps = new List<string>() { "", "", "", "" };
                    throw new NotImplementedException();
                    break;
            }
            return result;
        }

        private HttpResponseMessage PrintExamSubject(JObject filters)
        {
            int printWidth = 132;
            int printSrNo = 0;

            List<double> printTotals = new List<double>() { 0, 0, 0, 0 };
            List<string> printGrps = new List<string>() { "", "", "", "" };
            string printFileName = "ExamSub.txt";
            string reportName = "";
            Print print = new Print();
            //int pageNumber = 1;
            string printLine = print.FormatPrintString(reportName + " - " + printWidth, 50) + print.FormatPrintString(_reportPath + printFileName, 50);
            bool printResult = false;
            printResult = print.PrepareAndPrint(0, printWidth, printLine);
            if (!printResult) return new HttpResponseMessage(HttpStatusCode.NoContent);
            LegacyReportFacade reportFacade = new LegacyReportFacade(filters);
            DataSet dataSet = new DataSet();

            dataSet = reportFacade.GetExamSubjectSetup(esId, streamId);
            if (!dataSet.HasData())
                return new HttpResponseMessage(HttpStatusCode.NoContent);

            string options = "";
            DataSet dsOptions = new DataSet();
            dsOptions = reportFacade.GetExamSubjectOptions(esId, streamId);
            if (dsOptions.HasData())
            {
                foreach (DataRow drOptions in dsOptions.Tables[0].Rows)
                {
                    if (options.IsNullOrWhiteSpace())
                        options = "Options: ";
                    options += (options.IsNullOrWhiteSpace() ? "" : ",  ")
                        + drOptions["OpCode"].ToString() + " " + drOptions["OpCode"].ToString()
                        + "-" + drOptions["OpMax"].ToString();
                }
            }

            // 1
            printLine = instituteName;
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printResult = print.PrepareAndPrint(101, 1, printLine);
            if (!printResult) goto endReport;

            //2;
            printLine = "EXAM SUBJECTS - " + examCode + " " + examName;
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printResult = print.PrepareAndPrint(102, 0, printLine);
            if (!printResult) goto endReport;
            //3;
            printLine = "";
            if (streamId != 0)
            {
                printLine = streamCode + " " + streamName;
            }
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printLine = print.FormatPrintString(printLine, printWidth - 12) + "Page: ~~Pg";
            printResult = print.PrepareAndPrint(103, 0, printLine);
            if (!printResult) goto endReport;

            // 4
            printLine = options;
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printResult = print.PrepareAndPrint(104, 0, printLine);
            if (!printResult) goto endReport;
            // 4
            printResult = print.PrepareAndPrint(105, 1, "-".RepeatForLeftPadding(printWidth));
            if (!printResult) goto endReport;
            // 5
            printLine = print.FormatPrintString("                    <---------Marks---------> Uni        ", 60)
                        + print.FormatPrintString("Name", 51) + "ShortName";
            printResult = print.PrepareAndPrint(106, 0, printLine);
            if (!printResult) goto endReport;

            // 6
            printLine = print.FormatPrintString("Ty Opt Code         Max  Pass  Exempt  Dist   /Cg C/o CR ", 60) + "";
            printResult = print.PrepareAndPrint(107, 0, printLine);
            if (!printResult) goto endReport;
            // 7
            printResult = print.PrepareAndPrint(108, 0, "=".RepeatForLeftPadding(printWidth));
            if (!printResult) goto endReport;

            // Print Header
            printResult = print.PrepareAndPrint(2, 0, "");
            if (!printResult) goto endReport;

            // Print Details
            printSrNo = 0;
            int recordCount = dataSet.Tables[0].Rows.Count;
            for (int rowCounter = 0; rowCounter < recordCount; rowCounter++)
            {
                DataRow dataRow = dataSet.Tables[0].Rows[rowCounter];
                if (dataRow["EsType"].ToString() == "S")
                    printResult = print.PrepareAndPrint(3, 0, "");
                List<string> stringParts = print.BreakString(dataRow["EsName"].ToString(), 50, 2);
                printLine = print.FormatPrintString(dataRow["EsType"].ToString(), 3, 1)
                    + print.FormatPrintString(dataRow["EsOpCode"].ToString(), 4, 1)
                    + print.FormatPrintString(dataRow["EsCode"].ToString(), 13, 12
                    )
                    + print.FormatPrintString(dataRow["EsMaxMarks"].ToString() == "0" ? "" : dataRow["EsMaxMarks"].ToString(), 5, 4, "Z")
                    + (dataRow["EsPassPC"].ToString() == "0.00" ? print.FormatPrintString(dataRow["EsPassMarks"].ToString() == "0" ? "" : dataRow["EsPassMarks"].ToString(), 7, 5, "Z") :
                      print.FormatPrintString(dataRow["EsPassPC"].ToString(), 5, 5, "2Z") + "% ")
                    + (dataRow["EsExmPC"].ToString() == "0.00" ? print.FormatPrintString(dataRow["EsExmMarks"].ToString() == "0" ? "" : dataRow["EsExmMarks"].ToString(), 7, 5, "Z") :
                      print.FormatPrintString(dataRow["EsExmPC"].ToString(), 5, 5, "2Z") + "% ")
                    + (dataRow["EsDistPC"].ToString() == "0.00" ? print.FormatPrintString(dataRow["EsDistMarks"].ToString() == "0" ? "" : dataRow["EsDistMarks"].ToString(), 5, 5, "Z") + print.FormatPrintString(dataRow["EsDistRem"].ToString(), 3, 1) :
                      print.FormatPrintString(dataRow["EsDistPC"].ToString(), 5, 5, "2Z") + "%" + print.FormatPrintString(dataRow["EsDistRem"].ToString(), 2, 1))
                    + print.FormatPrintString(" " + dataRow["EsCollUni"].ToString(), 4, 2)
                    + (dataRow["EsACF"].ToString() == "Y" ? " Y  " : "    ")
                    + print.FormatPrintString(dataRow["EsCredit"].ToString() == "0.00" ? "" : dataRow["EsCredit"].ToString(), 6, 5, "2Z")
                    + print.FormatPrintString(stringParts[0], 51, 50) + print.FormatPrintString((dataRow["EsType"].ToString() == "S" ? dataRow["EsShName"].ToString() : ""), 20);
                printResult = print.PrepareAndPrint(3, 0, printLine);
                if (stringParts[1] != "")
                {
                    printLine = print.FormatPrintString("", 60) + stringParts[1];
                    printResult = print.PrepareAndPrint(3, 0, printLine);
                }

                string temp = "";
                temp = (dataRow["EsCombCode"].ToString() != "" ? "Must-Offer:" + dataRow["EsCombCode"].ToString() + " : " : "")
                    + (dataRow["EsNoComb"].ToString() != "" ? "Can't-Offer:" + dataRow["EsNoComb"].ToString() + " : " : "")
                    + (dataRow["EsLang"].ToString() == "Y" ? "Lang : " : "")
                    + (dataRow["EsPrint"].ToString() == "N" ? "No-Print : " : "")
                    + (dataRow["EsExclTot"].ToString() == "Y" ? "Excl-FromTotal : " : "")
                    + (dataRow["EsQMarks"].ToString() != "" ? "Sec:" + dataRow["EsQMarks"].ToString() + " : " : "")
                    + (dataRow["EsGrades"].ToString() != "" && dataRow["EsGrades"].ToString() != "---" ? "Grd:" + dataRow["EsGrades"].ToString() + " : " : "")
                    + (dataRow["EsGrdPts"].ToString() != "" ? "Grd-Points:" + dataRow["EsGrdPts"].ToString() + " : " : "")
                    + (dataRow["EsOldCode"].ToString() != "" ? "OldCode:" + dataRow["EsOldCode"].ToString() + " : " : "")
                    + (dataRow["EsSpFlag"].ToString().Trim() != "" ? "Scaling: Y " + " : " : "")
                    + (dataRow["EsThPr"].ToString().Trim() != "" ? "ThPr: " + dataRow["EsThPr"].ToString() + " : " : "")
                    + (dataRow["EsMustPass"].ToString().Trim() == "Y" ? "MustPass" + " : " : "")
                    + (dataRow["EsDeAct"].ToString().Trim() == "Y" ? "NOT ACTIVE" + " : " : "");

                if (temp.Trim() != "")
                {
                    //temp = temp.SubstringWithCorrection(1);
                    List<string> stringPart2 = print.BreakString(temp, 73, 2);

                    printResult = print.PrepareAndPrint(3, 0, print.FormatPrintString("", 60) + stringPart2[0]);
                    if (stringPart2[1] != "")
                        printResult = print.PrepareAndPrint(3, 0, print.FormatPrintString("", 60) + stringPart2[1]);
                }
            }

            gLnI = 0;
            validationMessage = "Printing Over";
        endReport:
            print.PrepareAndPrint(9, gLnI == 1 ? 1 : 0, validationMessage);
            PrintSettings printSettings = new PrintSettings();
            printSettings.PageSize = PdfSharp.PageSize.Ledger;
            printSettings.PageOrientation = PdfSharp.PageOrientation.Portrait;
            return PrintLineReport(print, printSettings);
        }

        private HttpResponseMessage ExportConvocationList(JObject filters)
        {
            LegacyReportFacade reportFacade = new LegacyReportFacade(filters);
            DataSet dataSet = reportFacade.GetConvocationList(sessionNo, courseCode);
            if (!dataSet.HasData())
                return new HttpResponseMessage(HttpStatusCode.NoContent);

            return dataSet.Tables[0].ToCSV_InHttpResponse();
        }

        private HttpResponseMessage PrintSectionMarks(JObject filters)
        {
            int printWidth = 80;
            int printSrNo = 0;

            List<double> printTotals = new List<double>() { 0, 0, 0, 0 };
            List<string> printGrps = new List<string>() { "", "", "", "" };
            string printFileName = "SectMark.txt";
            string reportName = "";
            Print print = new Print();
            //int pageNumber = 1;
            string printLine = print.FormatPrintString(reportName + " - " + printWidth, 50) + print.FormatPrintString(_reportPath + printFileName, 50);
            bool printResult = false;
            printResult = print.PrepareAndPrint(0, printWidth, printLine);
            if (!printResult) return new HttpResponseMessage(HttpStatusCode.NoContent);
            LegacyReportFacade reportFacade = new LegacyReportFacade(filters);
            DataSet dataSet = new DataSet();

            dataSet = reportFacade.GetSectionMarks(loginId, scheduleId, 1, collegeCode, center, esId);
            if (!dataSet.HasData())
                return new HttpResponseMessage(HttpStatusCode.NoContent);

            DataRow firstRow = dataSet.FirstRow();
            //Store headers
            // 1
            printLine = instituteName;
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printResult = print.PrepareAndPrint(101, 0, printLine);
            if (!printResult) goto endReport;
            // 2
            printLine = examShortName + " - " + sessionName;
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printLine = printLine.SubstringWithCorrection(0, 78);
            printResult = print.PrepareAndPrint(102, 0, printLine);
            if (!printResult) goto endReport;
            // 3
            printLine = " Section wise Marks";
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printLine = print.FormatPrintString(printLine, printWidth - 12) + "Page: ~~Pg";
            printResult = print.PrepareAndPrint(103, 0, printLine);
            if (!printResult) goto endReport;

            // 4
            printLine = firstRow["EsShName"].ToString();
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printLine = printLine.SubstringWithCorrection(0, 78);
            printResult = print.PrepareAndPrint(104, 0, printLine);
            if (!printResult) goto endReport;
            // 5
            printLine = print.FormatPrintString(firstRow["CgCode"].ToString() + ": " + firstRow["CgName"].ToString(), 78);
            printResult = print.PrepareAndPrint(105, 0, printLine);
            if (!printResult) goto endReport;
            // 6
            printResult = print.PrepareAndPrint(106, 0, "-".RepeatForLeftPadding(printWidth));
            if (!printResult) goto endReport;
            // 7
            string examScheduleQMarks = firstRow["EsQMarks"].ToString();
            string temp = "";
            int glnK = 0;

            while (!string.IsNullOrEmpty(examScheduleQMarks))
            {
                int glnI = examScheduleQMarks.IndexOf(",");
                if (glnI == -1)
                {
                    temp += print.FormatPrintString(examScheduleQMarks, 8, 7);
                    glnK += int.Parse(examScheduleQMarks);
                    examScheduleQMarks = "";
                }
                else
                {
                    temp += print.FormatPrintString(examScheduleQMarks.SubstringWithCorrection(0, glnI), 8, 7);
                    glnK += int.Parse(examScheduleQMarks.SubstringWithCorrection(0, glnI));
                    examScheduleQMarks = examScheduleQMarks.SubstringWithCorrection(glnI + 1);
                }
            }
            examScheduleQMarks = temp + glnK.ToString();
            printLine = print.FormatPrintString("RegnNo", 15)
                + print.FormatPrintString((registrationRoll == "G" ? "" : "RollNo"), 10)
                + examScheduleQMarks;
            printResult = print.PrepareAndPrint(107, 0, printLine);
            if (!printResult) goto endReport;

            // 8
            printResult = print.PrepareAndPrint(108, 0, "=".RepeatForLeftPadding(printWidth));
            if (!printResult) goto endReport;

            // Print Header
            printResult = print.PrepareAndPrint(2, 0, "");
            if (!printResult) goto endReport;

            printSrNo = 0;
            printGrps[1] = firstRow["Es"].ToString();
            printGrps[2] = firstRow["CgCode"].ToString();

            int recordCount = dataSet.Tables[0].Rows.Count;
            for (int rowCounter = 0; rowCounter < recordCount; rowCounter++)
            {
                DataRow dataRow = dataSet.Tables[0].Rows[rowCounter];

                if (printGrps[1] != dataRow["Es"].ToString()
                    || printGrps[2] != dataRow["CgCode"].ToString())
                {

                    printGrps[1] = dataRow["Es"].ToString();
                    printGrps[2] = dataRow["CgCode"].ToString();
                    // 4
                    printLine = dataRow["EsShName"].ToString().SubstringWithCorrection(0, 78);
                    printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
                    printResult = print.PrepareAndPrint(104, 0, printLine);
                    if (!printResult) goto endReport;
                    // 5 
                    printLine = print.FormatPrintString(dataRow["CgCode"].ToString() + ": " + dataRow["CgName"].ToString(), 78);
                    printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
                    printResult = print.PrepareAndPrint(105, 0, printLine);
                    if (!printResult) goto endReport;
                    // 7
                    examScheduleQMarks = firstRow["EsQMarks"].ToString();
                    temp = "";
                    glnK = 0;

                    while (!string.IsNullOrEmpty(examScheduleQMarks))
                    {
                        int glnI = examScheduleQMarks.IndexOf(",");
                        if (glnI == -1)
                        {
                            temp += print.FormatPrintString(examScheduleQMarks, 8, 7);
                            glnK += int.Parse(examScheduleQMarks);
                            examScheduleQMarks = "";
                        }
                        else
                        {
                            temp += print.FormatPrintString(examScheduleQMarks.SubstringWithCorrection(0, glnI), 8, 7);
                            glnK += int.Parse(examScheduleQMarks.SubstringWithCorrection(0, glnI));
                            examScheduleQMarks = examScheduleQMarks.SubstringWithCorrection(glnI + 1);
                        }
                    }
                    examScheduleQMarks = examScheduleQMarks + glnK.ToString();
                    printLine = print.FormatPrintString("RegnNo", 15)
                        + print.FormatPrintString((registrationRoll == "G" ? "" : "RollNo"), 10)
                        + examScheduleQMarks;
                    printResult = print.PrepareAndPrint(107, 0, printLine);
                    if (!printResult) goto endReport;

                    printResult = print.PrepareAndPrint(2, 99, "");
                    if (!printResult) goto endReport;

                    printSrNo = 0;
                }

                printSrNo = printSrNo + 1;
                gLnI = dataRow["M1"].ToString().ToInt32() + dataRow["M2"].ToString().ToInt32()
                    + dataRow["M3"].ToString().ToInt32() + dataRow["M4"].ToString().ToInt32()
                    + dataRow["M5"].ToString().ToInt32() + dataRow["M6"].ToString().ToInt32()
                    + dataRow["M7"].ToString().ToInt32() + dataRow["M8"].ToString().ToInt32();
                printLine = print.FormatPrintString(dataRow["RgNo"].ToString(), 15, 14)
                        + print.FormatPrintString((registrationRoll == "G" ? "" : dataRow["EfRollNo"].ToString()), 10, 9)
                        + (dataRow["M1"].ToString() == "" ? "" : print.FormatPrintString(dataRow["M1"].ToString(), 8, 7))
                        + (dataRow["M2"].ToString() == "" ? "" : print.FormatPrintString(dataRow["M2"].ToString(), 8, 7))
                        + (dataRow["M3"].ToString() == "" ? "" : print.FormatPrintString(dataRow["M3"].ToString(), 8, 7))
                        + (dataRow["M4"].ToString() == "" ? "" : print.FormatPrintString(dataRow["M4"].ToString(), 8, 7))
                        + (dataRow["M5"].ToString() == "" ? "" : print.FormatPrintString(dataRow["M5"].ToString(), 8, 7))
                        + (dataRow["M6"].ToString() == "" ? "" : print.FormatPrintString(dataRow["M6"].ToString(), 8, 7))
                        + (dataRow["M7"].ToString() == "" ? "" : print.FormatPrintString(dataRow["M7"].ToString(), 8, 7))
                        + (dataRow["M8"].ToString() == "" ? "" : print.FormatPrintString(dataRow["M8"].ToString(), 8, 7))
                        + print.FormatPrintString(gLnI.ToString(), 8, 7);

                printResult = print.PrepareAndPrint(3, 0, printLine);
                printResult = print.PrepareAndPrint(3, 0, "");

                printTotalCounter = 0;
                if (rowCounter == recordCount - 1)
                    printTotalCounter = 1;
                else if (printGrps[1] != dataRow["Es"].ToString() || printGrps[2] != dataRow["CgCode"].ToString())
                    printTotalCounter = 2;

                if (printTotalCounter > 0)
                    printResult = print.PrepareAndPrint(3, 1, "     Total: " + printSrNo.ToString());
            }

            gLnI = 0;
            validationMessage = "Printing Over";
        endReport:
            print.PrepareAndPrint(9, gLnI == 1 ? 1 : 0, validationMessage);
            PrintSettings printSettings = new PrintSettings();
            printSettings.PageSize = PdfSharp.PageSize.A4;
            printSettings.PageOrientation = PdfSharp.PageOrientation.Landscape;
            return PrintLineReport(print, printSettings);
        }

        private HttpResponseMessage PrintDistrictAssessmentMarks(JObject filters)
        {
            throw new NotImplementedException();
        }

        private HttpResponseMessage PrintLowerCasesCleared(JObject filters)
        {
            int printWidth = 85;
            int printSrNo = 0;

            List<double> printTotals = new List<double>() { 0, 0, 0, 0 };
            List<string> printGrps = new List<string>() { "", "", "", "" };
            string printFileName = "LowerClr.txt";
            string reportName = "";
            Print print = new Print();
            int pageNumber = 1;
            string printLine = print.FormatPrintString(reportName + " - " + printWidth, 50) + print.FormatPrintString(_reportPath + printFileName, 50);
            bool printResult = false;
            printResult = print.PrepareAndPrint(0, printWidth, printLine);
            if (!printResult) return new HttpResponseMessage(HttpStatusCode.NoContent);
            LegacyReportFacade reportFacade = new LegacyReportFacade(filters);
            DataSet dataSet = new DataSet();

            dataSet = reportFacade.GetLowerCasesCleared(courseCode, sessionNo);
            if (!dataSet.HasData())
                return new HttpResponseMessage(HttpStatusCode.NoContent);

            // Store headers
            // 1
            printLine = instituteName;
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printResult = print.PrepareAndPrint(101, 0, printLine);
            // 2
            printLine = "List of Lower exam cases, now cleared";
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printLine = print.FormatPrintString(printLine, printWidth - 12) + "Page: " + pageNumber;
            printResult = print.PrepareAndPrint(102, 1, printLine);
            if (!printResult) goto endReport;

            // 3
            printLine = (courseCode.ToUpper() + ", " + sessionName).SubstringWithCorrection(0, 78);
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printLine = print.FormatPrintString(printLine, printWidth - 12) + systemDate.ToString(dateFormat);
            printResult = print.PrepareAndPrint(103, 0, printLine);
            // 4
            printLine = "-".RepeatForLeftPadding(printWidth);
            printResult = print.PrepareAndPrint(104, 1, printLine);
            // 5
            printLine = print.FormatPrintString("RollNo", 11, 10)
                    + print.FormatPrintString("Name", 36, 35)
                    + print.FormatPrintString("Result", 7, 6)
                    + print.FormatPrintString("College", 30, 30);
            printResult = print.PrepareAndPrint(105, 0, printLine);
            // 6
            printLine = "=".RepeatForLeftPadding(printWidth);
            printResult = print.PrepareAndPrint(106, 1, printLine);

            // Print Header
            printResult = print.PrepareAndPrint(2, 0, "");
            if (!printResult) goto endReport;
            printSrNo = 0;
            printGrps[0] = "";

            int recordCount = dataSet.Tables[0].Rows.Count;
            for (int rowCounter = 0; rowCounter < recordCount; rowCounter++)
            {
                DataRow dataRow = dataSet.Tables[0].Rows[rowCounter];
                printSrNo = printSrNo + 1;
                if (printGrps[0] != dataRow["SnName"].ToString())
                {
                    printGrps[0] = dataRow["SnName"].ToString();
                    printLine = "Session: " + dataRow["SnName"].ToString();
                    printResult = print.PrepareAndPrint(3, 2, printLine);
                }
                printLine = print.FormatPrintString(dataRow["EfRollNo"].ToString(), 11, 10)
                    + print.FormatPrintString(dataRow["RgName"].ToString(), 36, 35)
                    + print.FormatPrintString(dataRow["Result"].ToString(), 7, 6)
                    + print.FormatPrintString(dataRow["CgName"].ToString(), 30, 30);
                printResult = print.PrepareAndPrint(3, 1, printLine);

                // write file;
                printLine = dataRow["EfRollNo"].ToString();
            }
            gLnI = 0;
            validationMessage = "Printing Over";
        endReport:
            print.PrepareAndPrint(9, gLnI == 1 ? 1 : 0, validationMessage);
            PrintSettings printSettings = new PrintSettings();
            printSettings.PageSize = PdfSharp.PageSize.A4;
            printSettings.PageOrientation = PdfSharp.PageOrientation.Landscape;
            return PrintLineReport(print, printSettings);
        }

        private HttpResponseMessage PrintCollegePaperCount(JObject filters)
        {
            //throw new NotImplementedException();
            //EsSeq, CgCode, EfRollNo, EsShName, SubCode, HdCode, CgName,
            //EtTime, EtTimeTo, CdSeq, Medium

            int printWidth = 80;
            int printSrNo = 0;

            List<double> printTotals = new List<double>() { 0, 0, 0 };
            List<string> printGrps = new List<string>()
            {
                "",
                "",
                ""
            };
            string printFileName = "CP_Count.txt";
            string reportName = "";

            if (reportCode == 49)
                reportName = "College Paper Count";

            Print print = new Print();
            int pageNumber = 1;
            string printLine = print.FormatPrintString(reportName + " - " + printWidth, 50) + print.FormatPrintString(_reportPath + printFileName, 50);
            bool printResult = false;
            printResult = print.PrepareAndPrint(0, printWidth, printLine);
            if (!printResult) return new HttpResponseMessage(HttpStatusCode.NoContent);
            LegacyReportFacade reportFacade = new LegacyReportFacade(filters);
            DataSet dataSet = new DataSet();
            string flag = "";
            if (dibrugarhCovidFlag)
            {
                flag = filters["examMode"].ToString().Coalesce();
                if (!"E,P".ToCSV().Contains(flag)) flag = "";
            }

            dataSet = reportFacade.GetCollegePaperSummary(scheduleId, uc, collegeCode, dsCode, reportCode, instituteId);
            if (!dataSet.HasData())
                return new HttpResponseMessage(HttpStatusCode.NoContent);

            // Store headers
            // 1
            printLine = instituteName;
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            //printLine = print.FormatPrintString(printLine, printWidth - 12) + "Page: " + pageNumber;
            printResult = print.PrepareAndPrint(101, 0, printLine);
            if (!printResult) goto endReport;

            // 2
            printLine = reportName + "  " + dsCode;
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printLine = print.FormatPrintString(printLine, printWidth - 12) + "Page: " + pageNumber;
            printResult = print.PrepareAndPrint(102, 0, printLine);
            if (!printResult) goto endReport;

            // 3
            printLine = examName + ", " + (examHeldIn != "" ? examHeldIn : sessionName);
            if (instituteId == 14 && dsCode == "TDP")
            {
                printLine = "TDP SEM" + examSrNo + ", " + sessionName;
            }
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printResult = print.PrepareAndPrint(103, 0, printLine);
            if (!printResult) goto endReport;

            //3;
            DataRow firstRow = dataSet.FirstRow();
            printGrps[1] = firstRow["EfCgCode"].ToString();
            printLine = firstRow["EfCgCode"].ToString() + ": " + firstRow["CgName"].ToString().Trim();
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printResult = print.PrepareAndPrint(104, 0, printLine);
            if (!printResult) goto endReport;

            // 4
            printResult = print.PrepareAndPrint(105, 0, "-".RepeatForLeftPadding(printWidth));
            if (!printResult) goto endReport;
            // 5
            printLine = print.FormatPrintString("Subject", 58) + "Appearing Candidates";
            printResult = print.PrepareAndPrint(106, 0, printLine);
            if (!printResult) goto endReport;
            // 6
            printResult = print.PrepareAndPrint(107, 0, "=".RepeatForLeftPadding(printWidth));
            if (!printResult) goto endReport;

            //Print Header
            printResult = print.PrepareAndPrint(2, 0, "");
            if (!printResult) goto endReport;

            // Print Details
            int rowCounter = 0;
            printTotals[1] = 0;
            printTotals[2] = 0;
            foreach (DataRow dataRow in dataSet.Tables[0].Rows)
            {
                rowCounter++;
                //Check Headers for Grp1;
                if (printGrps[1] != dataRow["EfCgCode"].ToString())
                {
                    printGrps[1] = dataRow["EfCgCode"].ToString();
                    printLine = dataRow["EfCgCode"].ToString() + ": " + dataRow["CgName"].ToString().Trim();
                    printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
                    printResult = print.PrepareAndPrint(104, 0, printLine);
                    if (!printResult) goto endReport;

                    printResult = print.PrepareAndPrint(2, 99, "");
                    if (!printResult) goto endReport;
                }

                string temp = dataRow["StrCode"].ToString() + " " + dataRow["SubCode"].ToString().Trim()
                 + (dataRow["HdCode"].ToString() == "" ? "" : "-" + dataRow["HdCode"].ToString().Trim()).Trim()
                 + ": " + dataRow["EsShName"].ToString();
                printLine = print.FormatPrintString(temp, 70, 68) + dataRow["EfRollNo"].ToString();
                printResult = print.PrepareAndPrint(3, 1, printLine);
            }
            gLnI = 0;
            validationMessage = "Printing Over";

        endReport:
            print.PrepareAndPrint(9, gLnI == 1 ? 1 : 0, validationMessage);
            PrintSettings printSettings = new PrintSettings();
            printSettings.PageSize = PdfSharp.PageSize.A4;
            printSettings.PageOrientation = PdfSharp.PageOrientation.Landscape;
            return PrintLineReport(print, printSettings);
        }

        private HttpResponseMessage PrepareResultSummary_06(JObject filters)
        {
            LegacyReportFacade reportFacade = new LegacyReportFacade(filters);
            DataSet dataSet = reportFacade.GetResultSummary_06(sessionNo, courseCode, pageBreak);
            if (!dataSet.HasData())
                return new HttpResponseMessage(HttpStatusCode.NoContent);

            return dataSet.Tables[0].ToCSV_InHttpResponse();
        }

        private HttpResponseMessage PrepareRollList_11(JObject filters)
        {
            LegacyReportFacade reportFacade = new LegacyReportFacade(filters);
            DataSet dataSet = reportFacade.GetRollList(sessionNo, courseCode);
            if (!dataSet.HasData())
                return new HttpResponseMessage(HttpStatusCode.NoContent);

            return dataSet.Tables[0].ToCSV_InHttpResponse();
        }

        private HttpResponseMessage PrintRegistrationCountAll(JObject filters)
        {
            int printWidth = 80;
            int printSrNo = 0;

            List<double> printTotals = new List<double>() { 0, 0 };
            List<string> printGrps = new List<string>()
            {
                "",
                ""
            }; //Course, College

            string printFileName = "Registration Count.txt";
            string reportName = "";
            Print print = new Print();
            string printLine = string.Empty;
            int pageNumber = 1;

            printLine = print.FormatPrintString("Registration Count" + " - ", 50) + print.FormatPrintString(_reportPath + printFileName, 50);
            bool printResult = false;
            printResult = print.PrepareAndPrint(0, printWidth, printLine);
            if (!printResult) return new HttpResponseMessage(HttpStatusCode.NoContent);

            LegacyReportFacade reportFacade = new LegacyReportFacade(filters);
            DataSet dataSet = reportFacade.GetRegistrationCount(sessionNo, courseCode, collegeCode, tnCode, dsCode);
            if (!dataSet.HasData())
            {
                gLnI = 1;
                validationMessage = "No Records to print";
                goto endReport;
            }

            // Store Headers
            printLine = instituteName;
            printLine = print.AlignAndPrint(printLine, printWidth, TextAlignment.Center);
            printResult = print.PrepareAndPrint(101, 0, printLine);
            if (!printResult) goto endReport;

            printLine = "CANDIDATES REGISTERED IN THE SESSION - " + sessionName;
            printLine = print.AlignAndPrint(printLine, printWidth, TextAlignment.Center);
            printResult = print.PrepareAndPrint(102, 0, printLine);
            if (!printResult) goto endReport;

            // Fetch first row
            DataRow firstDataRow = dataSet.FirstRow();

            printLine = "FOR - " + firstDataRow["CoName"].ToString();
            printLine = print.AlignAndPrint(printLine, printWidth, TextAlignment.Center);
            printResult = print.PrepareAndPrint(103, 0, printLine);
            if (!printResult) goto endReport;

            printLine = "DISTRICT: " + firstDataRow["DsName"].ToString();
            printLine = print.AlignAndPrint(printLine, printWidth, TextAlignment.Left);
            printResult = print.PrepareAndPrint(104, 0, printLine);
            if (!printResult) goto endReport;

            printResult = print.PrepareAndPrint(105, 1, "-".RepeatForLeftPadding(printWidth));
            if (!printResult) goto endReport;

            printLine = print.FormatPrintString("SrNo", 5)
                + print.FormatPrintString((isSchool || isInstitute ? "Institute" : "College"), 65)
                + print.FormatPrintString(" Count", 10);
            printResult = print.PrepareAndPrint(106, 0, printLine);
            if (!printResult) goto endReport;

            printResult = print.PrepareAndPrint(107, 0, "=".RepeatForLeftPadding(printWidth));
            if (!printResult) goto endReport;

            // Print Header
            printResult = print.PrepareAndPrint(2, 0, "");
            if (!printResult) goto endReport;

            printGrps[0] = "";
            printTotals[0] = 0;
            printTotals[1] = 0;

            int recordCounter = 0;
            Dictionary<string, int> districtTotalMap = new Dictionary<string, int>();

            // Print Details
            foreach (DataRow dataRow in dataSet.Tables[0].Rows)
            {
                recordCounter++;
                if (printGrps[0] != dataRow["TnName"].ToString())
                {
                    printLine = "     Town: " + dataRow["TnCode"].ToString() + " " + dataRow["TnName"].ToString();
                    printResult = print.PrepareAndPrint(3, 1, printLine);
                    printTotals[1] = 0;
                    printSrNo = 0;
                }

                printSrNo++;
                printLine = print.FormatPrintString(printSrNo, 5, 4)
                    + print.FormatPrintString(dataRow["DsCode"].ToString() + " " + dataRow["CgCode"].ToString() + " " + dataRow["CgName"].ToString(), 65, 64)
                    + print.FormatPrintString(dataRow["Total"], 10, 60);
                printResult = print.PrepareAndPrint(3, 0, printLine);
                printTotals[0] += dataRow["Total"].ToString().ToInt32();
                printTotals[1] += dataRow["Total"].ToString().ToInt32();
                if (districtTotalMap.IsEmpty())
                    districtTotalMap.Add(dataRow["DsCode"].ToString(), dataRow["Total"].ToString().ToInt32());
                else
                    districtTotalMap[dataRow["DsCode"].ToString()] = districtTotalMap[dataRow["DsCode"].ToString()] + dataRow["Total"].ToString().ToInt32();

                if (recordCounter >= dataSet.Tables[0].Rows.Count)
                {
                    printTotalCounter = 1;
                }
                else
                {
                    if (recordCounter < dataSet.Tables[0].Rows.Count)
                    {
                        if (dataSet.Tables[0].Rows[recordCounter - 1]["CoCode"].ToString() != printGrps[0])
                            printTotalCounter = 2;
                    }
                }
                if (printTotalCounter != 0)
                {
                    printLine = print.FormatPrintString("", 64)
                        + print.FormatPrintString("Total ", 6)
                        + print.FormatPrintString(printTotals[1], 10, 6);
                    printResult = print.PrepareAndPrint(3, 0, printLine);

                    printLine = print.FormatPrintString("", 55)
                        + print.FormatPrintString("District Total ", 15)
                        + print.FormatPrintString(districtTotalMap[dataRow["DsCode"].ToString()], 10, 6);
                    printResult = print.PrepareAndPrint(3, 0, printLine);

                    if (printTotalCounter == 1)
                    {
                        printLine = print.FormatPrintString("", 54)
                            + print.FormatPrintString("    Grand Total ", 16)
                            + print.FormatPrintString(printTotals[0], 10, 6);
                        printResult = print.PrepareAndPrint(3, 1, printLine);
                    }
                }
            }
            gLnI = 0;
            validationMessage = "Printing Over";

        endReport:
            print.PrepareAndPrint(9, gLnI == 1 ? 1 : 0, validationMessage);

            PrintSettings printSettings = new PrintSettings();
            printSettings.PageSize = PdfSharp.PageSize.A4;
            printSettings.PageOrientation = PdfSharp.PageOrientation.Landscape;
            return PrintLineReport(print, printSettings);
        }

        private HttpResponseMessage PrintConsolidated_MarkList(JObject filters)
        {
            throw new NotImplementedException();
        }

        private HttpResponseMessage PrintRegistrationList(JObject filters)
        {
            bool singleLine = false;
            singleLine = uc.ToString() == "1";

            int printWidth = 132;
            int printSrNo = 0;

            List<double> printTotals = new List<double>() { 0, 0 };
            List<string> printGrps = new List<string>()
            {
                "",
                ""
            }; //Course, College

            string printFileName = "Registration List.txt";
            string reportName = "";
            Print print = new Print();
            string printLine = string.Empty;
            int pageNumber = 1;

            printLine = print.FormatPrintString("Registration List" + " - ", 50) + print.FormatPrintString(_reportPath + printFileName, 50);
            bool printResult = false;
            printResult = print.PrepareAndPrint(0, printWidth, printLine);
            if (!printResult) return new HttpResponseMessage(HttpStatusCode.NoContent);

            LegacyReportFacade reportFacade = new LegacyReportFacade(filters);
            DataSet dataSet = reportFacade.GetRegistrationCard(loginId, instituteId, sessionNo, courseCode, collegeCode, tnCode, dsCode, center, streamId, registartionNo, "N");
            if (!dataSet.HasData())
            {
                gLnI = 1;
                validationMessage = "No Records to print";
                goto endReport;
            }

            // Store Headers
            printLine = instituteName;
            printLine = print.AlignAndPrint(printLine, printWidth, TextAlignment.Center);
            //printLine = print.FormatPrintString(printLine, printWidth - 12) + "Page: " + pageNumber;
            printResult = print.PrepareAndPrint(102, 0, printLine);
            if (!printResult) goto endReport;

            DataRow dataRow = dataSet.FirstRow();
            printGrps[0] = dataRow["RgCoCode"].ToString();
            printGrps[1] = dataRow["CgCode"].ToString();
            printSrNo = 0;

            printLine = "Course: " + dataRow["RgCoCode"].ToString() + " " + dataRow["CoName"].ToString();
            printLine = print.AlignAndPrint(printLine, printWidth, TextAlignment.Center);
            printResult = print.PrepareAndPrint(103, 0, printLine);
            if (!printResult) goto endReport;

            printLine = "District: " + dataRow["DsCode"].ToString() + " " + dataRow["DsName"].ToString();
            printLine = print.AlignAndPrint(printLine, printWidth, TextAlignment.Center);
            printResult = print.PrepareAndPrint(104, 0, printLine);
            if (!printResult) goto endReport;

            printLine = dataRow["CgCode"].ToString() + " " + dataRow["CgName"].ToString();
            printLine = print.AlignAndPrint(printLine, printWidth, TextAlignment.Center);
            printResult = print.PrepareAndPrint(105, 0, printLine);
            if (!printResult) goto endReport;

            printResult = print.PrepareAndPrint(106, 0, "_".RepeatForLeftPadding(printWidth));
            if (!printResult) goto endReport;

            printLine = print.FormatPrintString(" SrNo", 6)
                + print.FormatPrintString("Name", 43)
                + print.FormatPrintString("Father", 31)
                + print.FormatPrintString("Sex", 4)
                + print.FormatPrintString("Cate", 5)
                + print.FormatPrintString((instituteId == 15 ? "" : "Bran"), 5)
                + print.FormatPrintString("Subjects", 38);
            printResult = print.PrepareAndPrint(107, 0, printLine);
            if (!printResult) goto endReport;

            printLine = print.FormatPrintString("", 6)
                + print.FormatPrintString("RegnNo", 21)
                + print.FormatPrintString((instituteId == 18 ? "       UID" : ""), 11)
                + print.FormatPrintString((instituteId == 15 || instituteId == 17 ? "BirthDate" : ""), 11)
                + print.FormatPrintString("Mother", 31)
                + print.FormatPrintString("Med", 4)
                + print.FormatPrintString("Cast", 5)
                + print.FormatPrintString((instituteId == 17 ? "Mar" : ""), 3);
            printResult = print.PrepareAndPrint(108, 0, printLine);
            if (!printResult) goto endReport;

            printResult = print.PrepareAndPrint(109, 0, "=".RepeatForLeftPadding(printWidth));
            if (!printResult) goto endReport;

            // Print Header
            printResult = print.PrepareAndPrint(2, 0, "");
            if (!printResult) goto endReport;

            // Print Details
            foreach (DataRow data in dataSet.Tables[0].Rows)
            {
                if (printGrps[0] != data["RgCoCode"].ToString() || printGrps[1] != data["CgCode"].ToString())
                {
                    printGrps[0] = data["RgCoCode"].ToString();
                    printGrps[1] = data["CgCode"].ToString();
                    printSrNo = 0;

                    printLine = "Course: " + data["RgCoCode"].ToString() + " " + data["CoName"].ToString();
                    printLine = print.AlignAndPrint(printLine, printWidth, TextAlignment.Center);
                    printResult = print.PrepareAndPrint(103, 0, printLine);
                    if (!printResult) goto endReport;

                    printLine = "District: " + data["DsCode"].ToString() + " " + data["DsName"].ToString();
                    printLine = print.AlignAndPrint(printLine, printWidth, TextAlignment.Center);
                    printResult = print.PrepareAndPrint(104, 0, printLine);
                    if (!printResult) goto endReport;

                    printLine = data["CgCode"].ToString() + " " + data["CgName"].ToString();
                    printLine = print.AlignAndPrint(printLine, printWidth, TextAlignment.Center);
                    printResult = print.PrepareAndPrint(105, 0, printLine);
                    if (!printResult) goto endReport;

                    printResult = print.PrepareAndPrint(2, 99, "");
                    if (!printResult) goto endReport;
                }

                printSrNo++;
                printLine = print.FormatPrintString(printSrNo, 6, 5)
                    + print.FormatPrintString(data["RgName"].ToString(), 43, 42)
                    + print.FormatPrintString(data["RgFather"].ToString(), 31, 30)
                    + print.FormatPrintString(data["RgSex"].ToString(), 4)
                    + print.FormatPrintString(data["RgCate"].ToString(), 5)
                    + print.FormatPrintString(data["StrCode"].ToString(), 5, 4)
                    + print.FormatPrintString(data["RgSub"].ToString(), 38);
                printResult = print.PrepareAndPrint(3, 0, printLine);

                printLine = print.FormatPrintString("", 6)
                    + print.FormatPrintString(data["RgNo"], 21, 20)
                    + print.FormatPrintString((instituteId == 18 ? data["RgUID"].ToString() : ""), 11, 10, "Z")
                    + print.FormatPrintString((instituteId == 15 || instituteId == 17 ? data["RgBirthDate"].ToString() : ""), 11)
                    + print.FormatPrintString(data["RgMother"].ToString(), 31, 30)
                    + print.FormatPrintString(data["RgMedium"].ToString(), 4)
                    + print.FormatPrintString(data["RgCaste"].ToString(), 5)
                    + print.FormatPrintString((instituteId == 17 ? data["RgReli"].ToString() : ""), 3);

                if (data["RgSub"].ToString().Length > 38)
                    printLine = print.FormatPrintString(printLine, 92) + data["RgSub"].ToString().SubstringWithCorrection(39, 38);

                printResult = print.PrepareAndPrint(3, 0, printLine);
                printResult = print.PrepareAndPrint(3, 0, "");
            }
            gLnI = 0;
        endReport:
            print.PrepareAndPrint(9, gLnI == 1 ? 1 : 0, validationMessage);

            PrintSettings printSettings = new PrintSettings();
            printSettings.PageSize = PdfSharp.PageSize.Legal;
            printSettings.PageOrientation = PdfSharp.PageOrientation.Landscape;
            return PrintLineReport(print, printSettings);

        }

        private HttpResponseMessage PrintNetRollList(JObject filters)
        {
            //throw new NotImplementedException();
            LegacyReportFacade reportFacade = new LegacyReportFacade(filters);
            string option = "";
            try { option = filters["option"].ToString(); } catch { }
            DataSet dataSet = reportFacade.GetRollList(scheduleId, "Cent" + option, collegeCode, center, dsCode);
            if (!dataSet.HasData())
                return new HttpResponseMessage(HttpStatusCode.NoContent);

            List<string> lines = new List<string>();
            string line = "Exam|Cent|RollNo|RgNo|Bran|Name|Father|Mother|Colg|Cate|Sex";

            lines.Add(line);
            return dataSet.Tables[0].ToCSV_InHttpResponse();
        }

        private HttpResponseMessage PrintNetResult(JObject filters)
        {
            throw new NotImplementedException();
        }

        private HttpResponseMessage PrintNetResult_17(JObject filters)
        {
            return PrintCrystalReport("MarksFoilKKH.rpt", null, filters);
        }

        private HttpResponseMessage PrintNetResult_15(JObject filters)
        {
            throw new NotImplementedException();
        }

        private HttpResponseMessage PrintNetResult_14(JObject filters)
        {
            throw new NotImplementedException();
        }

        private HttpResponseMessage PrintNetResult_11(JObject filters)
        {
            throw new NotImplementedException();
        }

        private HttpResponseMessage PrintCodes_13(JObject filters)
        {
            throw new NotImplementedException();
        }

        private HttpResponseMessage PrintMarksFoil_13C(JObject filters)
        {
            throw new NotImplementedException();
        }

        private HttpResponseMessage PrintMarksFoil_13U(JObject filters)
        {
            throw new NotImplementedException();
        }

        private HttpResponseMessage PrintMarksFoil_NMU(JObject filters)
        {
            throw new NotImplementedException();
        }

        private HttpResponseMessage PrintDivisionOfWork(JObject filters)
        {
            //throw new NotImplementedException();
            int printWidth = 80;
            int printSrNo = 0;
            string totalSubjects = "";
            string totalMedical = "";
            List<List<double>> printTotals = new List<List<double>>();
            printTotals.Add(new List<double>() { 0, 0, 0, 0 });
            printTotals.Add(new List<double>() { 0, 0, 0, 0 });
            printTotals.Add(new List<double>() { 0, 0, 0, 0 });
            List<string> printGrps = new List<string>() { "", "", "" }; //Sub, Cent/Colg

            string printFileName = "DivWork.txt";
            string reportName = "";
            Print print = new Print();
            string printLine = print.FormatPrintString(reportName + " - " + printWidth, 50) + print.FormatPrintString(_reportPath + printFileName, 50);
            bool printResult = false;
            printResult = print.PrepareAndPrint(0, printWidth, printLine);
            if (!printResult) return new HttpResponseMessage(HttpStatusCode.NoContent);

            LegacyReportFacade reportFacade = new LegacyReportFacade(filters);
            DataSet dataSet = new DataSet();
            dataSet = reportFacade.GetControlSheetSummary(scheduleId, uc, scheduleExamId);
            if (!dataSet.HasData())
                return new HttpResponseMessage(HttpStatusCode.NoContent);

            int pageNumber = 1;
            // Store headers
            printLine = instituteName;
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printLine = print.FormatPrintString(printLine, printWidth - 12) + systemDate.ToString(this.dateFormat);
            printResult = print.PrepareAndPrint(101, 0, printLine);
            if (!printResult) goto endReport;
            // 2
            printLine = "DIVISION OF WORK TO EXAMINERS";
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printLine = print.FormatPrintString(printLine, printWidth - 12) + "Page: " + pageNumber;
            printResult = print.PrepareAndPrint(102, 0, printLine);
            if (!printResult) goto endReport;
            // 3
            printLine = examName + " - " + sessionName;
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printResult = print.PrepareAndPrint(103, 0, printLine);
            if (!printResult) goto endReport;

            // 4
            DataRow firstRow = dataSet.FirstRow();
            printGrps[1] = firstRow["FsEsId"].ToString();
            totalSubjects = firstRow["Sub"].ToString() + (firstRow["Head"].ToString().Trim() == "" ? "" : "-" + firstRow["Head"].ToString());
            printLine = totalSubjects + "  " + firstRow["SubName"].ToString();
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printResult = print.PrepareAndPrint(104, 0, printLine);
            if (!printResult) goto endReport;

            // 5
            printResult = print.PrepareAndPrint(105, 0, "-".RepeatForLeftPadding(printWidth));
            if (!printResult) goto endReport;
            // 6
            printLine = print.FormatPrintString("Medium", 8)
                 + print.FormatPrintString("  CsNo", 7, 6)
                 + print.FormatPrintString("Cent", 5, 4)
                 + print.FormatPrintString("     Folios", 20)
                 + print.FormatPrintString("    RollNos", 20);
            printResult = print.PrepareAndPrint(106, 0, printLine);
            if (!printResult) goto endReport;
            // 7
            printLine = print.FormatPrintString("", 8)
                 + print.FormatPrintString("", 7, 6)
                 + print.FormatPrintString("", 5, 4)
                 + print.FormatPrintString("  From     To Cnt", 20)
                 + print.FormatPrintString("  From     To  Count", 21);
            printResult = print.PrepareAndPrint(107, 0, printLine);
            if (!printResult) goto endReport;

            // 8
            printResult = print.PrepareAndPrint(108, 0, "=".RepeatForLeftPadding(printWidth));
            if (!printResult) goto endReport;

            // Print Header
            printResult = print.PrepareAndPrint(2, 0, "");
            if (!printResult) goto endReport;

            // Print Details
            int rowCounter = 0;
            foreach (DataRow dataRow in dataSet.Tables[0].Rows)
            {
                rowCounter++;
                //Check Subject;
                if (printGrps[1] != dataRow["FsEsId"].ToString())
                {
                    printGrps[1] = dataRow["FsEsId"].ToString();
                    totalSubjects = dataRow["Sub"].ToString()
                        + (dataRow["Head"].ToString().Trim() == "" ? "" : "-" + dataRow["Head"].ToString());
                    printLine = totalSubjects + "  " + dataRow["SubName"].ToString();
                    printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
                    printResult = print.PrepareAndPrint(104, 0, printLine);
                    if (!printResult) goto endReport;
                    printGrps[2] = "";
                    printResult = print.PrepareAndPrint(2, 99, "");
                    if (!printResult) goto endReport;
                }
                // Check Medium
                if (printGrps[2] != dataRow["RgMedium"].ToString())
                {
                    printGrps[2] = dataRow["RgMedium"].ToString();
                    totalMedical = dataRow["CdDesc"].ToString();
                    printLine = print.FormatPrintString(totalMedical, 8, 7);
                }
                else
                    printLine = print.FormatPrintString("", 8);

                // Detail line
                printLine = printLine
                     + print.FormatPrintString(dataRow["FsCsNo"].ToString(), 7, 6)
                     + print.FormatPrintString(dataRow["CgCode"].ToString(), 5, 4)
                     + print.FormatPrintString(dataRow["MinFol"].ToString(), 7, 6)
                     + print.FormatPrintString(dataRow["MaxFol"].ToString(), 8, 6)
                     + print.FormatPrintString(dataRow["MaxFol"].ToString().ToInt32() - dataRow["MinFol"].ToString().ToInt32() + 1, 5, 1)
                     + print.FormatPrintString(dataRow["MinRoll"].ToString(), 7, 6)
                     + print.FormatPrintString(dataRow["MaxRoll"].ToString(), 7, 6)
                     + print.FormatPrintString(dataRow["Cnt"].ToString(), 7, 6);
                printResult = print.PrepareAndPrint(3, 1, printLine);

                //take totals;
                printTotals[2][1] = printTotals[2][1] + 1;
                printTotals[2][2] = printTotals[2][2] + dataRow["MaxFol"].ToString().ToInt32() - dataRow["MinFol"].ToString().ToInt32() + 1;
                printTotals[2][2] = printTotals[2][3] + dataRow["Cnt"].ToString().ToInt32();

                printTotalCounter = 0;
                if (rowCounter == dataSet.Tables[0].Rows.Count)
                    printTotalCounter = 3;
                else if (printGrps[1] != dataRow["FsEsId"].ToString())
                    printTotalCounter = 2;
                else if (printGrps[2] != dataRow["RgMedium"].ToString())
                    printTotalCounter = 1;

                if (printTotalCounter != 0)
                {
                    printResult = print.PrepareAndPrint(3, 0, print.FormatPrintString("", 8) + "-".RepeatForLeftPadding(67));
                    printLine = print.FormatPrintString("", 8) + print.FormatPrintString("Total For " + totalSubjects + " " + totalMedical, 25)
                        + print.FormatPrintString("CsNo:  " + printTotals[2][1].ToString(), 14)
                        + print.FormatPrintString("Folio: " + printTotals[2][2].ToString(), 14)
                        + print.FormatPrintString("Cand:  " + printTotals[2][3].ToString(), 14);
                    printResult = print.PrepareAndPrint(3, 0, printLine);
                    printResult = print.PrepareAndPrint(3, 0, "");
                    for (int i = 1; gLnI <= 3; i++)
                    {
                        printTotals[1][i] = printTotals[1][i] + printTotals[2][i];
                        printTotals[2][i] = 0;
                    }

                    if (printTotalCounter > 1)
                    {
                        printLine = print.FormatPrintString("", 8) + print.FormatPrintString("Total For " + totalSubjects, 25)
                            + print.FormatPrintString("CsNo:  " + printTotals[1][1].ToString(), 14)
                            + print.FormatPrintString("Folio: " + printTotals[1][2].ToString(), 14)
                            + print.FormatPrintString("Cand:  " + printTotals[1][3].ToString(), 14);
                        printResult = print.PrepareAndPrint(3, 2, printLine);
                        for (int i = 1; i <= 3; i++)
                        {
                            printTotals[0][i] = printTotals[0][i] + printTotals[1][i];
                            printTotals[1][i] = 0;
                        }
                    }

                    if (printTotalCounter > 2 && scheduleExamId == 0)
                    {
                        printLine = print.FormatPrintString("", 8) + print.FormatPrintString("Total For Exam", 25)
                            + print.FormatPrintString("CsNo:  " + printTotals[0][1], 14)
                            + print.FormatPrintString("Folio: " + printTotals[0][2], 14)
                            + print.FormatPrintString("Cand:  " + printTotals[0][3], 14);
                        printResult = print.PrepareAndPrint(3, 2, printLine);
                    }
                }
            }
            gLnI = 0;
            validationMessage = "Printing Over";
        endReport:
            print.PrepareAndPrint(9, gLnI == 1 ? 1 : 0, validationMessage);

            PrintSettings printSettings = new PrintSettings();
            printSettings.PageSize = PdfSharp.PageSize.A4;
            printSettings.PageOrientation = PdfSharp.PageOrientation.Landscape;
            return PrintLineReport(print, printSettings);
        }

        private HttpResponseMessage PrintGraceList(JObject filters)
        {
            throw new NotImplementedException();
        }

        private HttpResponseMessage PrintResult_College(JObject filters)
        {
            //throw new NotImplementedException();
            int printWidth = 80;
            int printSrNo = 0;
            List<double> printTotals = new List<double>() { 0, 0, 0 };
            List<string> printGrps = new List<string>() { "", "", "" };

            string success = "";
            string reserve = "";
            int pageNumber = 1;
            int total = 0;
            int appeared = 0;
            int passed = 0;
            bool pageSkiped = false;

            string printFileName = "ResultCg.txt";
            string reportName = "";
            Print print = new Print();
            string printLine = print.FormatPrintString(reportName + " - " + printWidth, 50) + print.FormatPrintString(_reportPath + printFileName, 50);
            bool printResult = false;
            printResult = print.PrepareAndPrint(0, printWidth, printLine);
            if (!printResult) return new HttpResponseMessage(HttpStatusCode.NoContent);

            LegacyReportFacade reportFacade = new LegacyReportFacade(filters);
            DataSet dataSet = new DataSet();
            dataSet = reportFacade.GetCollegeResult(scheduleId, collegeCode, streamId);
            if (!dataSet.HasData())
                return new HttpResponseMessage(HttpStatusCode.NoContent);

            if (instituteId == 11)
            {
                success = "The following candidates are declared successful ";
                reserve = "";
            }
            else
            {
                success = "The following candidates are declared successful ";
                reserve = "Results of the following candidates are held in reserve for reasons";
            }

            DataRow firstRow = dataSet.FirstRow();

            total = firstRow["Total"].ToString().ToInt32();
            appeared = firstRow["Appr"].ToString().ToInt32();
            passed = firstRow["Pass"].ToString().ToInt32();

            // Store headers
            // 1
            printLine = instituteName;
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printResult = print.PrepareAndPrint(101, 0, printLine);
            if (!printResult) goto endReport;
            // 2
            printLine = examNo;
            printLine = print.FormatPrintString(printLine, printWidth - 18);
            printResult = print.PrepareAndPrint(102, 0, printLine);
            if (!printResult) goto endReport;
            // 3
            printLine = ("RESULT OF " + examName).SubstringWithCorrection(0, 80, true);
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printResult = print.PrepareAndPrint(103, 0, printLine);
            if (!printResult) goto endReport;
            // 4
            printLine = " Exam Held in " + sessionName;
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printLine = print.FormatPrintString(printLine, printWidth - 12) + "Page: " + pageNumber;
            printResult = print.PrepareAndPrint(104, 0, printLine);
            if (!printResult) goto endReport;
            // 5
            printGrps[0] = firstRow["CgCode"].ToString();
            printLine = firstRow["CgCode"].ToString() + " " + firstRow["CgName"].ToString();
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printResult = print.PrepareAndPrint(105, 0, printLine);
            // 6
            printLine = "-".RepeatForLeftPadding(printWidth);
            printResult = print.PrepareAndPrint(106, 0, printLine);
            // 7
            printGrps[1] = firstRow["StrId"].ToString();
            printLine = firstRow["StrName"].ToString();
            printResult = print.PrepareAndPrint(107, 0, printLine);
            // 8
            printGrps[2] = firstRow["EfPress"].ToString();
            printLine = "  " + (passed > 0 ? success : reserve);
            printResult = print.PrepareAndPrint(108, 1, printLine);
            // 9
            printLine = "    " + firstRow["EfRemarks"].ToString() + (registrationRoll == "G" ? ": RegnNos" : ": Roll Numbers");
            printResult = print.PrepareAndPrint(109, 1, printLine);

            // Print Header
            printResult = print.PrepareAndPrint(2, 0, "");
            if (!printResult) goto endReport;

            printSrNo = 0;
            printTotals[1] = 0;
            printLine = "";

            int recordCount = dataSet.Tables[0].Rows.Count;
            for (int rowCounter = 0; rowCounter < recordCount; rowCounter++)
            {
                DataRow dataRow = dataSet.Tables[0].Rows[rowCounter];
                if (printGrps[0] != dataRow["CgCode"].ToString()
                    || printGrps[1] != dataRow["StrId"].ToString()
                    || printGrps[2] != dataRow["EfPress"].ToString())
                {
                    // 5
                    printLine = dataRow["CgCode"].ToString() + " " + dataRow["CgName"].ToString();
                    printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
                    printResult = print.PrepareAndPrint(105, 1, printLine);
                    // 7
                    printLine = dataRow["StrName"].ToString();
                    printResult = print.PrepareAndPrint(107, 0, printLine);
                    // 8
                    printLine = "  " + (dataRow["EfPress"].ToString().SubstringWithCorrection(0, 1) == "1" ? success : reserve);
                    printResult = print.PrepareAndPrint(108, 1, printLine);
                    // 9
                    printLine = "    " + dataRow["EfRemarks"].ToString() + (registrationRoll == "G" ? ": RegnNos" : ": Roll Numbers");
                    printResult = print.PrepareAndPrint(109, 1, printLine);

                    if (printGrps[0] != dataRow["CgCode"].ToString())
                    {
                        printResult = print.PrepareAndPrint(2, 99, "");
                        if (!printResult) goto endReport;
                    }
                    else
                    {
                        printResult = print.PrepareAndPrint(2, 5, "");
                        if (!printResult) goto endReport;
                        //;
                        if (!pageSkiped)
                        {
                            if (printGrps[1] != dataRow["StrId"].ToString())
                            {
                                printLine = dataRow["StrName"].ToString();
                                printResult = print.PrepareAndPrint(3, 2, printLine);
                            }

                            if (dataRow["EfPress"].ToString().SubstringWithCorrection(0, 1) != printGrps[2].SubstringWithCorrection(0, 1))
                            {
                                printLine = "  " + (dataRow["EfPress"].ToString().SubstringWithCorrection(0, 1) == "1" ? success : reserve);
                                printResult = print.PrepareAndPrint(3, 1, printLine);
                                printResult = print.PrepareAndPrint(3, 0, "");
                            }
                            printLine = "    " + dataRow["EfRemarks"].ToString() + (registrationRoll == "G" ? ": RegnNos" : ": Roll Numbers");
                            printResult = print.PrepareAndPrint(3, 0, printLine);
                        }
                    }
                    //;
                    printGrps[0] = dataRow["CgCode"].ToString();
                    printGrps[1] = dataRow["StrId"].ToString();
                    printGrps[2] = dataRow["EfPress"].ToString();
                    total = dataRow["Total"].ToString().ToInt32();
                    appeared = dataRow["Appr"].ToString().ToInt32();
                    passed = dataRow["Pass"].ToString().ToInt32();
                    printSrNo = 0;
                    printTotals[1] = 0;
                    printLine = "";
                }
                printSrNo = printSrNo + 1;
                printTotals[1] = printTotals[1] + 1;
                if (registrationRoll == "G")
                    printLine = printLine + print.FormatPrintString(dataRow["RgNo"].ToString(), 16, 15);
                else
                    printLine = printLine + print.FormatPrintString(rollNoPrefix + dataRow["EfRollNo"].ToString(), 10, 8);

                printTotalCounter = 0;
                if (rowCounter == recordCount - 1)
                    printTotalCounter = 5;
                else if (rowCounter < recordCount - 1 && printGrps[0] != dataSet.Tables[0].Rows[rowCounter + 1]["CgCode"].ToString())
                    printTotalCounter = 4;
                else if (rowCounter < recordCount - 1 && printGrps[1] != dataSet.Tables[0].Rows[rowCounter + 1]["StrId"].ToString())
                    printTotalCounter = 3;
                else if (rowCounter < recordCount - 1 && printGrps[2] != dataSet.Tables[0].Rows[rowCounter + 1]["EfPress"].ToString())
                    printTotalCounter = 2;
                else
                {
                    if (registrationRoll == "G" && printSrNo == 5)
                        printTotalCounter = 1;
                    if (registrationRoll != "G" && printSrNo == 8)
                        printTotalCounter = 1;
                }

                if (printTotalCounter > 0)
                {
                    printResult = print.PrepareAndPrint(3, 1, printLine);
                    printSrNo = 0;
                    printLine = "";
                }
                if (printTotalCounter > 1)
                {
                    printLine = "    Total: " + print.FormatPrintString(printTotals[1], 6);
                    printResult = print.PrepareAndPrint(3, 0, printLine);
                    printResult = print.PrepareAndPrint(3, 0, "");
                    printTotals[1] = 0;
                }

                if (printTotalCounter > 2)
                {
                    printResult = print.PrepareAndPrint(2, 10, "");
                    if (!printResult) goto endReport;
                    printLine = " ".RepeatForLeftPadding(10) + "Result Statistics  " + "Candidates";
                    printResult = print.PrepareAndPrint(3, 1, printLine);
                    printLine = " ".RepeatForLeftPadding(10) + "-----------------  " + "----------";
                    printResult = print.PrepareAndPrint(3, 0, printLine);
                    printLine = " ".RepeatForLeftPadding(10) + "Total              " + print.FormatPrintString(total, 10);
                    printResult = print.PrepareAndPrint(3, 0, printLine);
                    gLnI = total - appeared;
                    printLine = " ".RepeatForLeftPadding(10) + "Absent             " + print.FormatPrintString(gLnI, 10);
                    printResult = print.PrepareAndPrint(3, 0, printLine);
                    printLine = " ".RepeatForLeftPadding(10) + "Appeared           " + print.FormatPrintString(appeared, 10);
                    printResult = print.PrepareAndPrint(3, 0, printLine);
                    printLine = " ".RepeatForLeftPadding(10) + "Passed             " + print.FormatPrintString(passed, 10);
                    printResult = print.PrepareAndPrint(3, 0, printLine);
                    double passedDouble = passed;
                    double appearedDouble = appeared;
                    double appearedToPassed = (passedDouble / appearedDouble) * 100;
                    printLine = " ".RepeatForLeftPadding(10) + "Pass% to Appeared  " + print.FormatPrintString(appearedToPassed, 10, 10, "2");
                    printResult = print.PrepareAndPrint(3, 0, printLine);
                    printResult = print.PrepareAndPrint(3, 0, "");
                }

                if (printTotalCounter > 3)
                {
                    printResult = print.PrepareAndPrint(2, 5, "");
                    if (!printResult) goto endReport;
                    printLine = print.FormatPrintString(instituteName, 45) + authorityName;
                    printResult = print.PrepareAndPrint(3, 3, printLine);
                    printLine = print.FormatPrintString(resultDate, 45) + authorityDesignation;
                    printResult = print.PrepareAndPrint(3, 0, printLine);
                }
            }
            gLnI = 0;
            validationMessage = "Printing Over";
        endReport:
            print.PrepareAndPrint(9, gLnI == 1 ? 1 : 0, validationMessage);

            PrintSettings printSettings = new PrintSettings();
            printSettings.PageSize = PdfSharp.PageSize.A4;
            printSettings.PageOrientation = PdfSharp.PageOrientation.Landscape;
            return PrintLineReport(print, printSettings);
        }

        private HttpResponseMessage PrintResult_17_College(JObject filters)
        {
            //throw new NotImplementedException();
            int printWidth = 80;
            int printSrNo = 0;
            List<double> printTotals = new List<double>() { 0, 0, 0, 0 };
            List<string> printGrps = new List<string>() { "", "", "", "" };

            string success = "";
            string reserve = "";
            int pageNumber = 1;
            int total = 0;
            int absent = 0;
            int withheld = 0;
            int passed = 0;
            int failed = 0;
            bool pageSkiped = false;

            string printFileName = "ResultST.txt";
            string reportName = "";
            Print print = new Print();
            string printLine = print.FormatPrintString(reportName + " - " + printWidth, 50) + print.FormatPrintString(_reportPath + printFileName, 50);
            bool printResult = false;
            printResult = print.PrepareAndPrint(0, printWidth, printLine);
            if (!printResult) return new HttpResponseMessage(HttpStatusCode.NoContent);

            LegacyReportFacade reportFacade = new LegacyReportFacade(filters);
            DataSet dataSet = new DataSet();
            dataSet = reportFacade.GetCollegeResult_17(scheduleId, collegeCode, streamId);
            if (!dataSet.HasData())
                return new HttpResponseMessage(HttpStatusCode.NoContent);

            DataRow firstRow = dataSet.FirstRow();

            // Store headers
            // 1
            printLine = instituteName;
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printResult = print.PrepareAndPrint(101, 4, printLine);
            if (!printResult) goto endReport;
            // 2
            printLine = "Result Date: " + resultDate;  //strExNo;
            printLine = print.FormatPrintString("", printWidth - printLine.Length) + printLine;
            printResult = print.PrepareAndPrint(102, 0, printLine);
            if (!printResult) goto endReport;
            // 3
            printLine = ("RESULT OF " + examName).SubstringWithCorrection(0, 80, true);
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printResult = print.PrepareAndPrint(103, 0, printLine);
            if (!printResult) goto endReport;
            // 4
            printLine = " Exam Held in " + examHeldIn;
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printLine = print.FormatPrintString(printLine, printWidth - 12) + "Page: " + pageNumber;
            printResult = print.PrepareAndPrint(104, 0, printLine);
            if (!printResult) goto endReport;
            // 5
            printLine = "-".RepeatForLeftPadding(printWidth);
            printResult = print.PrepareAndPrint(105, 0, printLine);
            // 6
            printGrps[1] = firstRow["CgCode"].ToString();
            printLine = firstRow["CgCode"].ToString() + " " + firstRow["CgName"].ToString();
            printResult = print.PrepareAndPrint(106, 0, printLine);
            // 7
            printGrps[2] = firstRow["StrId"].ToString();
            printLine = firstRow["StrName"].ToString();
            printResult = print.PrepareAndPrint(107, 0, printLine);
            // 8
            printGrps[3] = firstRow["EfPress"].ToString();
            printLine = firstRow["RmRemarks"].ToString() + " : RegnNos";
            printResult = print.PrepareAndPrint(108, 1, printLine);

            total = firstRow["Total"].ToString().ToInt32();
            absent = firstRow["Abst"].ToString().ToInt32();
            withheld = firstRow["WH"].ToString().ToInt32();
            passed = firstRow["Pass"].ToString().ToInt32();
            failed = firstRow["Fail"].ToString().ToInt32();
            printSrNo = 0;
            printTotals[1] = 0;
            printLine = "";

            // Print Header
            printResult = print.PrepareAndPrint(2, 0, "");
            if (!printResult) goto endReport;

            // Print Details
            int recordCount = dataSet.Tables[0].Rows.Count;
            for (int rowCounter = 0; rowCounter < recordCount; rowCounter++)
            {
                DataRow dataRow = dataSet.Tables[0].Rows[rowCounter];

                if (printGrps[1] != dataRow["CgCode"].ToString()
                    || printGrps[2] != dataRow["StrId"].ToString())
                {
                    printGrps[1] = dataRow["CgCode"].ToString();
                    printGrps[2] = firstRow["StrId"].ToString();
                    printGrps[3] = firstRow["EfPress"].ToString();
                    total = firstRow["Total"].ToString().ToInt32();
                    absent = firstRow["Abst"].ToString().ToInt32();
                    withheld = firstRow["WH"].ToString().ToInt32();
                    passed = firstRow["Pass"].ToString().ToInt32();
                    failed = firstRow["Fail"].ToString().ToInt32();
                    // 6
                    printLine = dataRow["CgCode"].ToString() + " " + dataRow["CgName"].ToString();
                    printResult = print.PrepareAndPrint(106, 0, printLine);
                    // 7
                    printLine = dataRow["StrName"].ToString();
                    printResult = print.PrepareAndPrint(107, 0, printLine);
                    // 8
                    printLine = dataRow["RmRemarks"].ToString() + " : RegnNos";
                    printResult = print.PrepareAndPrint(108, 1, printLine);

                    printResult = print.PrepareAndPrint(2, 99, "");
                    printSrNo = 0;
                    printTotals[1] = 0;
                    printLine = "";
                }
                if (printGrps[3] != dataRow["EfPress"].ToString())
                {
                    printGrps[3] = firstRow["EfPress"].ToString();
                    // 8
                    printLine = dataRow["RmRemarks"].ToString() + " : RegnNos";
                    printResult = print.PrepareAndPrint(108, 1, printLine);

                    printResult = print.PrepareAndPrint(2, 5, "");
                    if (!pageSkiped)
                    {
                        printLine = dataRow["RmRemarks"].ToString() + ": RegnNos";
                        printResult = print.PrepareAndPrint(3, 1, printLine);
                        printResult = print.PrepareAndPrint(3, 0, "");
                    }
                    printSrNo = 0;
                    printTotals[1] = 0;
                    printLine = "";
                }
                printSrNo = printSrNo + 1;
                printTotals[1] = printTotals[1] + 1;
                printLine = printLine + print.FormatPrintString(dataRow["RgNo"].ToString(), 16, 15);

                printTotalCounter = 0;
                if (rowCounter == recordCount - 1)
                    printTotalCounter = 4;
                else if (rowCounter < recordCount - 1 && (
                    printGrps[1] != dataSet.Tables[0].Rows[rowCounter + 1]["CgCode"].ToString()
                    || printGrps[1] != dataSet.Tables[0].Rows[rowCounter + 1]["StrId"].ToString()
                ))
                    printTotalCounter = 3;
                else if (rowCounter < recordCount - 1 && printGrps[2] != dataSet.Tables[0].Rows[rowCounter + 1]["EfPress"].ToString())
                    printTotalCounter = 2;
                else if (printSrNo == 5)
                    printTotalCounter = 1;

                if (printTotalCounter > 0)
                {
                    printResult = print.PrepareAndPrint(3, 1, printLine);
                    printSrNo = 0;
                    printLine = "";
                }
                if (printTotalCounter > 1)
                {
                    printLine = "    Total: " + print.FormatPrintString(printTotals[1], 6);
                    printResult = print.PrepareAndPrint(3, 0, printLine);
                    printResult = print.PrepareAndPrint(3, 0, "");
                    printTotals[1] = 0;
                }
                if (printTotalCounter > 2)
                {
                    printResult = print.PrepareAndPrint(2, 10, "");
                    printLine = " ".RepeatForLeftPadding(10) + "Result Statistics  " + "Candidates";
                    printResult = print.PrepareAndPrint(3, 1, printLine);
                    printLine = " ".RepeatForLeftPadding(10) + "-----------------  " + "----------";
                    printResult = print.PrepareAndPrint(3, 1, printLine);
                    printLine = " ".RepeatForLeftPadding(10) + "Total              " + print.FormatPrintString(total, 10);
                    printResult = print.PrepareAndPrint(3, 0, printLine);
                    printLine = " ".RepeatForLeftPadding(10) + "Absent             " + print.FormatPrintString(absent, 10);
                    printResult = print.PrepareAndPrint(3, 0, printLine);
                    gLnI = total - absent;
                    printLine = " ".RepeatForLeftPadding(10) + "Appeared           " + print.FormatPrintString(gLnI, 10);
                    printResult = print.PrepareAndPrint(3, 0, printLine);
                    printLine = " ".RepeatForLeftPadding(10) + "Result Withheld    " + print.FormatPrintString(withheld, 10);
                    printResult = print.PrepareAndPrint(3, 0, printLine);
                    printLine = " ".RepeatForLeftPadding(10) + "Clear              " + print.FormatPrintString(passed, 10);
                    printResult = print.PrepareAndPrint(3, 0, printLine);
                    printLine = " ".RepeatForLeftPadding(10) + "Not Clear          " + print.FormatPrintString(failed, 10);
                    printResult = print.PrepareAndPrint(3, 0, printLine);

                    printResult = print.PrepareAndPrint(2, 5, "");
                    printLine = print.FormatPrintString(instituteName, 50) + authorityName;
                    printResult = print.PrepareAndPrint(3, 3, printLine);
                    printLine = print.FormatPrintString("Result Date: " + resultDate, 50) + authorityDesignation;
                    printResult = print.PrepareAndPrint(3, 0, printLine);
                }
            }

            gLnI = 0;
            validationMessage = "Printing Over";
        endReport:
            print.PrepareAndPrint(9, gLnI == 1 ? 1 : 0, validationMessage);

            PrintSettings printSettings = new PrintSettings();
            printSettings.PageSize = PdfSharp.PageSize.A4;
            printSettings.PageOrientation = PdfSharp.PageOrientation.Landscape;
            return PrintLineReport(print, printSettings);
        }

        private HttpResponseMessage PrintResult_15(JObject filters)
        {
            throw new NotImplementedException();
        }

        private HttpResponseMessage PrintResult_14(JObject filters)
        {
            throw new NotImplementedException();
        }

        private HttpResponseMessage PrintResult_11(JObject filters)
        {
            throw new NotImplementedException();
        }

        private HttpResponseMessage PrintPassCertificate_13(JObject filters)
        {
            throw new NotImplementedException();
        }

        private HttpResponseMessage PrintNumericAnalysis_13(JObject filters)
        {
            throw new NotImplementedException();
        }

        private HttpResponseMessage PrintNumericAnalysis(JObject filters)
        {
            throw new NotImplementedException();
        }

        private HttpResponseMessage PrintPaperCenter(JObject filters)
        {
            throw new NotImplementedException();
        }

        private HttpResponseMessage PrintCenterPaperCount(JObject filters)
        {
            //return null;
            int printWidth = 80;
            int printSrNo = 0;
            List<double> printTotals = new List<double>() { 0, 0, 0, 0 };
            List<string> printGrps = new List<string>() { "", "", "", "" }; //Sub, Cent/Colg

            string printFileName = "PC_Count.txt";
            string reportName = "";
            Print print = new Print();
            int pageNumber = 1;
            string printLine = print.FormatPrintString(reportName + " - " + printWidth, 50) + print.FormatPrintString(_reportPath + printFileName, 50);
            bool printResult = false;
            printResult = print.PrepareAndPrint(0, printWidth, printLine);
            if (!printResult) return new HttpResponseMessage(HttpStatusCode.NoContent);

            if (dibrugarhCovidFlag)
            {
                if (filters["Flag"].ToString() == "A")
                    dsCode = "";
                else
                    dsCode = filters["Flag"].ToString();
            }

            LegacyReportFacade reportFacade = new LegacyReportFacade(filters);
            DataSet dataSet = new DataSet();
            if (instituteId == 3 || instituteId == 18)
                dataSet = reportFacade.GetCenterPaperCount_03(scheduleId, esId, center, dsCode, reportCode);
            else
                dataSet = reportFacade.GetCenterPaperCount(loginId, scheduleId, uc, esId, center, dsCode, reportCode);

            if (!dataSet.HasData())
                return new HttpResponseMessage(HttpStatusCode.NoContent);

            DataRow firstRow = dataSet.FirstRow();
            //Store headers;
            //1;
            printLine = instituteName;
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printLine = print.FormatPrintString(printLine, printWidth - 12) + systemDate.ToString(dateFormat);
            printResult = print.PrepareAndPrint(101, 0, printLine);
            if (!printResult) goto endReport;
            //2;
            printLine = reportName + "  " + dsCode;
            if (instituteId == 13)
                printLine = "Subject Wise No of Candidates";
            if (instituteId == 7)
                printLine = "Subject Summary";
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printLine = print.FormatPrintString(printLine, printWidth - 12) + "Page: " + pageNumber;
            printResult = print.PrepareAndPrint(102, 0, printLine);
            if (!printResult) goto endReport;
            // 3
            printLine = examName + ", " + (examHeldIn != "" ? examHeldIn : sessionName);
            if (instituteId == 7)
                printLine = examName + ", " + sessionYear;

            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printResult = print.PrepareAndPrint(103, 0, printLine);
            if (!printResult) goto endReport;

            // 3
            printGrps[0] = firstRow["EsId"].ToString();
            printGrps[1] = firstRow["EsId"].ToString();
            if (instituteId == 3 || instituteId == 18)
                printGrps[1] = firstRow["SubCode"].ToString().Trim() + "-" + firstRow["HdCode"].ToString().Trim();
            printLine = "Subject " + firstRow["StrCode"].ToString() + " " + firstRow["SubCode"].ToString().Trim()
                     + (firstRow["HdCode"].ToString().Trim() == "" ? "" : "-" + firstRow["HdCode"].ToString().Trim())
                     + ": " + firstRow["EsShName"].ToString();
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printResult = print.PrepareAndPrint(104, 0, printLine);
            if (!printResult) goto endReport;

            printLine = "";
            if (firstRow["EtTime"].ToDateTimeWithCoalesce() != "".ToDateTimeWithCoalesce())
                printLine = "Date + Time: " + firstRow["EtTime"].ToDateTimeWithCoalesce().ToString(dateFormat)
                        + " To " + firstRow["EtTimeTo"].ToDateTimeWithCoalesce().ToString(dateFormat);

            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printResult = print.PrepareAndPrint(105, 0, printLine);
            if (!printResult) goto endReport;

            // 4
            printResult = print.PrepareAndPrint(106, 0, "-".RepeatForLeftPadding(printWidth));
            if (!printResult) goto endReport;
            // 5
            printLine = print.FormatPrintString((instituteId == 13 ? "Venue" : "Centre"), 58) + "Appearing Candidates";
            printResult = print.PrepareAndPrint(107, 0, printLine);
            if (!printResult) goto endReport;
            // 6
            printResult = print.PrepareAndPrint(108, 0, "=".RepeatForLeftPadding(printWidth));
            if (!printResult) goto endReport;

            // Print Header
            printResult = print.PrepareAndPrint(2, 0, "");
            if (!printResult) goto endReport;

            // Print Details
            printTotals[1] = 0;
            printTotals[2] = 0;

            int recordCount = dataSet.Tables[0].Rows.Count;
            for (int rowCounter = 0; rowCounter < recordCount; rowCounter++)
            {
                DataRow dataRow = dataSet.Tables[0].Rows[rowCounter];
                //Check Headers for Grp1;
                if (instituteId != 3 && instituteId != 18 && printGrps[1] != dataRow["EsId"].ToString()
                || (instituteId == 3 || instituteId == 18) && printGrps[1] != dataRow["SubCode"].ToString() + "-" + dataRow["HdCode"].ToString().Trim())
                {
                    printGrps[1] = dataRow["EsId"].ToString();
                    if (instituteId == 3 || instituteId == 18)
                        printGrps[1] = dataRow["SubCode"].ToString().Trim() + "-" + dataRow["HdCode"].ToString().Trim();
                    printLine = "Subject " + dataRow["StrCode"].ToString() + " " + dataRow["SubCode"].ToString().Trim()
                             + (dataRow["HdCode"].ToString().Trim() == "" ? "" : "-" + dataRow["HdCode"].ToString().Trim())
                             + ": " + dataRow["EsShName"].ToString();
                    printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
                    printResult = print.PrepareAndPrint(104, 0, printLine);
                    if (!printResult) goto endReport;

                    printLine = "";
                    if (dataRow["EtTime"].ToDateTimeWithCoalesce() != "".ToDateTimeWithCoalesce())
                        printLine = "Date + Time: " + dataRow["EtTime"].ToDateTimeWithCoalesce().ToString(dateFormat)
                                + " To " + dataRow["EtTimeTo"].ToDateTimeWithCoalesce().ToString(dateFormat);

                    printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
                    printResult = print.PrepareAndPrint(105, 0, printLine);
                    if (!printResult) goto endReport;

                    printResult = print.PrepareAndPrint(2, 99, "");
                    if (!printResult) goto endReport;

                    printLine = print.FormatPrintString(dataRow["Cent"].ToString().Trim() + ": " + dataRow["CgName"].ToString(), 70, 68)
                        + dataRow["EfRollNo"].ToString();
                    printResult = print.PrepareAndPrint(3, 1, printLine);
                    printTotals[1] = printTotals[1] + 1;
                }
            }
            gLnI = 0;
            validationMessage = "Printing Over";

        endReport:
            print.PrepareAndPrint(9, gLnI == 1 ? 1 : 0, validationMessage);

            PrintSettings printSettings = new PrintSettings();
            printSettings.PageSize = PdfSharp.PageSize.A4;
            printSettings.PageOrientation = PdfSharp.PageOrientation.Landscape;
            return PrintLineReport(print, printSettings);
        }

        private HttpResponseMessage PrintResult_06(JObject filters)
        {
            int printWidth = 80;
            int printSrNo = 0;

            List<double> printTotals = new List<double>() { 0, 0, 0 };
            List<string> printGrps = new List<string>() { "", "", "" }; //Sub, Cent/Colg

            string printFileName = "Result.txt";
            string reportName = "";
            Print print = new Print();
            string printLine = print.FormatPrintString(reportName + " - " + printWidth, 50) + print.FormatPrintString(_reportPath + printFileName, 50);
            bool printResult = false;
            printResult = print.PrepareAndPrint(0, printWidth, printLine);
            if (!printResult) return new HttpResponseMessage(HttpStatusCode.NoContent);

            LegacyReportFacade reportFacade = new LegacyReportFacade(filters);
            DataSet dataSet = new DataSet();
            dataSet = reportFacade.Result(scheduleId, streamId);
            if (!dataSet.HasData())
                return new HttpResponseMessage(HttpStatusCode.NoContent);

            string msgSuccess = "The following candidates are declared successful ";
            string msgReserve = "Results of the following candidates are held in reserve for reasons";
            string msgWithExm = "The following candidates have secured subject exemption";

            // Store Headers
            printLine = instituteName;
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printResult = print.PrepareAndPrint(101, 4, printLine);
            if (!printResult) goto endReport;

            printLine = examNo;
            printLine = print.FormatPrintString(printLine, printWidth - 18) + sessionName;
            printResult = print.PrepareAndPrint(102, 0, printLine);
            if (!printResult) goto endReport;

            printLine = ("RESULT OF THE " + examName);
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printResult = print.PrepareAndPrint(103, 0, printLine);
            if (!printResult) goto endReport;

            printLine = " Exam held in " + examHeldIn;
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printLine = print.FormatPrintString(printLine, printWidth - 12) + $"Page: {Print._page}";
            printResult = print.PrepareAndPrint(104, 0, printLine);
            if (!printResult) goto endReport;

            printLine = print.FormatPrintString("-".RepeatForLeftPadding(printWidth), printWidth);
            printResult = print.PrepareAndPrint(105, 0, printLine);
            if (!printResult) goto endReport;

            DataRow dataRow = dataSet.FirstRow();

            int total = dataRow["Total"].Coalesce("0").ToInt32();
            int appeared = dataRow["Appr"].Coalesce("0").ToInt32();
            int passed = dataRow["Pass"].Coalesce("0").ToInt32();

            printGrps[0] = dataRow["StrId"].ToString();
            printLine = dataRow["StrName"].ToString();
            printResult = print.PrepareAndPrint(106, 0, printLine);

            printGrps[1] = dataRow["EfPress"].ToString();
            printGrps[2] = printGrps[1].SubstringWithCorrection(0, 1);
            printLine = "  " + (passed > 0 ? msgSuccess : (printGrps[2] == "5" ? msgWithExm : msgReserve));
            printResult = print.PrepareAndPrint(107, 0, printLine);

            printLine = "    " + dataRow["EfRemarks"].ToString() + ": RegnNos";
            printResult = print.PrepareAndPrint(108, 0, printLine);
            printResult = print.PrepareAndPrint(109, 0, "");



            // Print Header
            printResult = print.PrepareAndPrint(2, 0, "");
            if (!printResult) goto endReport;

            printSrNo = 0;
            printTotals = new List<double>() { 0, 0, 0 };
            printLine = "";

            int rowCounter = 0;
            // Print Details
            foreach (DataRow data in dataSet.Tables[0].Rows)
            {
                rowCounter++;
                if (printGrps[0] != data["StrId"].ToString() || printGrps[1] != data["EfPress"].ToString())
                {
                    printGrps[1] = data["EfPress"].ToString();

                    printLine = data["StrName"].ToString();
                    printResult = print.PrepareAndPrint(106, 0, printLine);

                    printLine = "  " + (printGrps[2] == "1" ? msgSuccess : (printGrps[2] == "5" ? msgWithExm : msgReserve));
                    printResult = print.PrepareAndPrint(107, 0, printLine);

                    printLine = "    " + dataRow["EfRemarks"].ToString() + ": RegnNos";
                    printResult = print.PrepareAndPrint(108, 0, printLine);

                    printResult = print.PrepareAndPrint(2, 5, "");
                    if (!printResult) goto endReport;

                    if (!print.pageSkipped)
                    {
                        if (printGrps[0] != data["StrId"].ToString())
                        {
                            printLine = data["StrName"].ToString();
                            printResult = print.PrepareAndPrint(3, 2, printLine);
                        }

                        if (data["EfPress"].ToString().SubstringWithCorrection(0, 1) != printGrps[2])
                        {
                            printLine = "  " + (printGrps[2] == "1" ? msgSuccess : (printGrps[2] == "5" ? msgWithExm : msgReserve));
                            printResult = print.PrepareAndPrint(3, 0, printLine);
                            printResult = print.PrepareAndPrint(3, 0, "");
                        }

                        printLine = "    " + dataRow["EfRemarks"].ToString() + ": RegnNos";
                        printResult = print.PrepareAndPrint(3, 0, printLine);
                    }

                    printGrps[0] = data["StrId"].ToString();
                    printGrps[2] = printGrps[1].SubstringWithCorrection(0, 1);
                    total = dataRow["Total"].Coalesce("0").ToInt32();
                    appeared = dataRow["Appr"].Coalesce("0").ToInt32();
                    passed = dataRow["Pass"].Coalesce("0").ToInt32();

                    printSrNo = 0;
                    printTotals = new List<double>() { 0, 0, 0 };
                    printLine = "";
                }
                printLine = printLine + print.FormatPrintString(data["RgNo"].ToString(), 16, 15);

                printSrNo++;
                printTotals[0]++;

                printTotalCounter = 0;
                if (rowCounter == dataSet.Tables[0].Rows.Count)
                {
                    printTotalCounter = 4;
                }
                else if (printGrps[0] != data["StrId"].ToString())
                {
                    printTotalCounter = 3;
                }
                else if (printGrps[1] != data["EfPress"].ToString())
                {
                    printTotalCounter = 2;
                }
                else
                {
                    if (printSrNo == 5) printTotalCounter = 1;
                }

                if (printTotalCounter > 0)
                {
                    printResult = print.PrepareAndPrint(3, 1, printLine);
                    printSrNo = 0;
                    printLine = "";
                }

                if (printTotalCounter > 1)
                {
                    printLine = "    Total: " + print.FormatPrintString(printTotals[0], 6);
                    printResult = print.PrepareAndPrint(3, 0, printLine);
                    printResult = print.PrepareAndPrint(3, 0, "");
                    printTotals[0] = 0;
                }

                if (printTotalCounter > 2)
                {
                    printResult = print.PrepareAndPrint(2, 8, "");
                    if (!printResult) goto endReport;

                    printLine = " ".RepeatForLeftPadding(10) + "Result Statistics  " + "Candidates";
                    printResult = print.PrepareAndPrint(3, 1, printLine);
                    printLine = " ".RepeatForLeftPadding(10) + "-----------------  " + "----------";
                    printResult = print.PrepareAndPrint(3, 1, printLine);
                    printLine = " ".RepeatForLeftPadding(10) + "Total              " + print.FormatPrintString(total, 10);
                    printResult = print.PrepareAndPrint(3, 0, printLine);
                    gLnI = total - appeared;
                    printLine = " ".RepeatForLeftPadding(10) + "Absent             " + print.FormatPrintString(gLnI, 10);
                    printResult = print.PrepareAndPrint(3, 0, printLine);
                    printLine = " ".RepeatForLeftPadding(10) + "Appeared           " + print.FormatPrintString(appeared, 10);
                    printResult = print.PrepareAndPrint(3, 0, printLine);
                    printLine = " ".RepeatForLeftPadding(10) + "Passed             " + print.FormatPrintString(passed, 10);
                    printResult = print.PrepareAndPrint(3, 0, printLine);
                    double passedDouble = passed;
                    double appearedDouble = appeared;
                    double passPercentage = (passedDouble / appearedDouble) * 100;
                    printLine = " ".RepeatForLeftPadding(10) + "Pass% to Appeared  " + print.FormatPrintString(passPercentage, 10, 10, "2");
                    printResult = print.PrepareAndPrint(3, 0, printLine);
                    printResult = print.PrepareAndPrint(3, 0, "");
                }
            }

            printResult = print.PrepareAndPrint(2, 5, "");
            if (!printResult) goto endReport;
            printLine = print.FormatPrintString(instituteName, 45) + authorityName;
            printResult = print.PrepareAndPrint(3, 3, printLine);
            printLine = print.FormatPrintString(resultDate, 45) + authorityDesignation;
            printResult = print.PrepareAndPrint(3, 0, printLine);
            gLnI = 0;
            validationMessage = "Printing Over";

        endReport:
            print.PrepareAndPrint(9, gLnI == 1 ? 1 : 0, validationMessage);

            PrintSettings printSettings = new PrintSettings();
            printSettings.PageSize = PdfSharp.PageSize.A4;
            printSettings.PageOrientation = PdfSharp.PageOrientation.Landscape;
            return PrintLineReport(print, printSettings);
        }

        private HttpResponseMessage PrintMarkList_16(JObject filters)
        {
            throw new NotImplementedException();
        }

        private HttpResponseMessage PrintMarkList_14(JObject filters)
        {
            throw new NotImplementedException();
        }

        private HttpResponseMessage PrintMarkList_13(JObject filters)
        {
            throw new NotImplementedException();
        }

        private HttpResponseMessage PrintMarkList_13GPA(JObject filters)
        {
            throw new NotImplementedException();
        }

        private HttpResponseMessage PrintMarkList_13GPA_ME(JObject filters)
        {
            throw new NotImplementedException();
        }

        private HttpResponseMessage PrintMarkList_AHS(JObject filters)
        {
            throw new NotImplementedException();
        }

        private HttpResponseMessage PrintLedger_16(JObject filters)
        {
            throw new NotImplementedException();
        }

        private HttpResponseMessage PrintLedger_14(JObject filters)
        {
            throw new NotImplementedException();
        }

        private HttpResponseMessage PrintLedger_13(JObject filters)
        {
            throw new NotImplementedException();
        }

        private HttpResponseMessage PrintLedger_11(JObject filters)
        {
            throw new NotImplementedException();
        }

        private HttpResponseMessage PrintPackingList(JObject filters)
        {
            throw new NotImplementedException();
        }

        private HttpResponseMessage PrintAttendanceSheet(JObject filters)
        {
            throw new NotImplementedException();
        }

        private HttpResponseMessage PrintAttendanceSheetAll(JObject filters)
        {
            LegacyReportFacade reportFacade = new LegacyReportFacade(filters);
            if (!reportImage.IsNullOrWhiteSpace())
            {
                reportFacade.PrepareAdmitCard("Rpt_AdmitCardsI", loginId, scheduleId, collegeCode, rollNo, registartionNo, instituteId, center, streamId, "Y");
                reportFacade.PrepareImages(loginId, reportImage, imagePath);
            }
            return PrintCrystalReport("AttenSheetI.Rpt", null, filters);
        }

        private HttpResponseMessage PrintAttendanceSheet_05(JObject filters)
        {
            throw new NotImplementedException();
        }

        private HttpResponseMessage PrintCenterPaper(JObject filters)
        {
            throw new NotImplementedException();
        }

        private HttpResponseMessage PrintCenterPaper_16(JObject filters)
        {
            throw new NotImplementedException();
        }

        private HttpResponseMessage PrintPreDispatch(JObject filters)
        {
            throw new NotImplementedException();
        }

        private HttpResponseMessage PrintCenterSummary_14(JObject filters)
        {
            throw new NotImplementedException();
        }

        private HttpResponseMessage PrintSummary(JObject filters)
        {
            //throw new NotImplementedException();
            int printWidth = 80;
            int printSrNo = 0;

            List<double> printTotals = new List<double>() { 0, 0, 0, 0, 0, 0, 0 };
            List<string> printGrps = new List<string>() { "", "", "" }; //Sub, Cent/Colg
            string printFileName = "SchSum.txt";
            string reportName = "";
            int pageNumber = 1;

            string centerCollegeFlag = "";
            string tempCollegeCode = collegeCode;
            if (reportCode == 6)
            {
                if (isSchool)
                {
                    printFileName = "SchSum.txt";
                    centerCollegeFlag = "Colg";
                }
                else
                {
                    printFileName = "ColgSum.txt";
                    centerCollegeFlag = "Colg";
                }
            }
            else
            {
                printFileName = "CentSum.txt";
                centerCollegeFlag = "Cent";
                tempCollegeCode = center;
            }

            Print print = new Print();
            string printLine = print.FormatPrintString(reportName + " - " + printWidth, 50) + print.FormatPrintString(_reportPath + printFileName, 50);
            bool printResult = false;
            printResult = print.PrepareAndPrint(0, printWidth, printLine);
            if (!printResult) return new HttpResponseMessage(HttpStatusCode.NoContent);

            LegacyReportFacade reportFacade = new LegacyReportFacade(filters);
            DataSet dataSet = new DataSet();
            dataSet = reportFacade.GetCollegeCenterSummary(loginId, scheduleId, centerCollegeFlag, tempCollegeCode, dsCode, streamId, instituteId);
            if (!dataSet.HasData())
                return new HttpResponseMessage(HttpStatusCode.NoContent);

            // Store headers
            //1;
            printLine = instituteName;
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printResult = print.PrepareAndPrint(101, 0, printLine);
            if (!printResult) goto endReport;
            // 2 - 3
            string temp = (instituteId == 13 ? "UGC Statistics" : reportName);
            temp = temp + " - " + examName + " - " + sessionName + " " + streamName;
            List<string> stringParts = print.BreakString(temp, 65, 2);

            printLine = print.FormatPrintString("", (printWidth - stringParts[0].Length) / 2) + stringParts[0];
            printResult = print.PrepareAndPrint(102, 0, printLine);
            printLine = print.FormatPrintString("", (printWidth - stringParts[1].Length) / 2) + stringParts[1];
            printLine = print.FormatPrintString(printLine, printWidth - 12) + "Page: " + pageNumber;
            printResult = print.PrepareAndPrint(103, 0, printLine);

            // 4
            DataRow firstRow = dataSet.FirstRow();
            printLine = (firstRow["Grp"].ToString() == "xxxxx" ? "Overall" : firstRow["DsCode"].ToString() + " " + firstRow["DsName"].ToString());
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printResult = print.PrepareAndPrint(104, 0, printLine);

            printLine = " Result Date: " + resultDate;
            printLine = print.FormatPrintString("", (printWidth - printLine.Length)) + printLine;
            printResult = print.PrepareAndPrint(105, 0, printLine);

            printGrps[0] = firstRow["Grp"].ToString();
            printSrNo = 0;
            printLine = (firstRow["Grp"].ToString() == "xxxxx" ? "" : firstRow["Grp"].ToString() + " " + firstRow["GrpName"].ToString());
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printResult = print.PrepareAndPrint(106, 0, printLine);

            // 4
            printResult = print.PrepareAndPrint(107, 0, "-".RepeatForLeftPadding(printWidth));
            if (!printResult) goto endReport;
            // 5
            printResult = print.PrepareAndPrint(108, 0, "");
            if (!printResult) goto endReport;
            // Print Header
            printResult = print.PrepareAndPrint(2, 0, "");
            if (!printResult) goto endReport;

            printTotals = new List<double>() { 0, 0, 0, 0, 0, 0, 0 };
            int recordCount = dataSet.Tables[0].Rows.Count;
            for (int rowCounter = 0; rowCounter < recordCount; rowCounter++)
            {
                DataRow dataRow = dataSet.Tables[0].Rows[rowCounter];
                if (printGrps[0] != dataRow["Grp"].ToString())
                {
                    printGrps[0] = dataRow["Grp"].ToString();
                    printGrps[1] = "";

                    printLine = (dataRow["Grp"].ToString() == "xxxxx" ? "Overall" : dataRow["DsCode"].ToString() + " " + dataRow["DsName"].ToString());
                    printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
                    printResult = print.PrepareAndPrint(104, 0, printLine);

                    printLine = " Result Date: " + resultDate;
                    printLine = print.FormatPrintString("", (printWidth - printLine.Length)) + printLine;
                    printResult = print.PrepareAndPrint(105, 0, printLine);

                    printGrps[0] = dataRow["Grp"].ToString();
                    printSrNo = 0;
                    printLine = (dataRow["Grp"].ToString() == "xxxxx" ? "" : dataRow["Grp"].ToString() + " " + dataRow["GrpName"].ToString());
                    printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
                    printResult = print.PrepareAndPrint(106, 0, printLine);

                    printResult = print.PrepareAndPrint(2, 99, "");
                    if (!printResult) goto endReport;
                }
                if (printGrps[1] != dataRow["Seq"].ToString())
                {
                    printGrps[1] = dataRow["Seq"].ToString();
                    printLine = print.FormatPrintString(dataRow["Class"].ToString(), 34, 33)
                    + "      Male    " + "    Female    " + "     Total   ";
                    printResult = print.PrepareAndPrint(3, 1, printLine);
                    printLine = print.FormatPrintString("", 34, 33)
                    + "  Appr " + "  Pass " + "  Appr " + "  Pass " + "  Appr " + "  Pass ";
                    printResult = print.PrepareAndPrint(3, 0, printLine);
                    printLine = print.FormatPrintString("-".RepeatForLeftPadding(dataRow["Class"].ToString().Length), 34, 33)
                    + "------ " + "------ " + "------ " + "------ " + "------ " + "------";
                    printResult = print.PrepareAndPrint(3, 0, printLine);
                    printTotals = new List<double>() { 0, 0, 0, 0, 0, 0, 0 };
                }
                printLine = print.FormatPrintString(dataRow["Grp2"].ToString(), 8, 7)
                    + print.FormatPrintString(dataRow["Grp2Name"].ToString(), 26, 25)
                    + print.FormatPrintString(dataRow["Male"].ToString(), 7, 6, "0", true)
                    + print.FormatPrintString(dataRow["MaleP"].ToString(), 7, 6, "0", true)
                    + print.FormatPrintString(dataRow["Female"].ToString(), 7, 6, "0", true)
                    + print.FormatPrintString(dataRow["FemaleP"].ToString(), 7, 6, "0", true)
                    + print.FormatPrintString((dataRow["Male"].Coalesce("0").ToInt32() + dataRow["Female"].Coalesce("0").ToInt32()).ToString(), 7, 6, "0", true)
                    + print.FormatPrintString((dataRow["MaleP"].Coalesce("0").ToInt32() + dataRow["FemaleP"].Coalesce("0").ToInt32()).ToString(), 7, 6, "0", true);
                printResult = print.PrepareAndPrint(3, 0, printLine);
                printTotals[1] = printTotals[1] + dataRow["Male"].ToString().ToInt32();
                printTotals[2] = printTotals[2] + dataRow["MaleP"].ToString().ToInt32();
                printTotals[3] = printTotals[3] + dataRow["Female"].ToString().ToInt32();
                printTotals[4] = printTotals[4] + dataRow["FemaleP"].ToString().ToInt32();
                printTotals[5] = printTotals[5] + dataRow["Male"].Coalesce("0").ToInt32() + dataRow["Female"].Coalesce("0").ToInt32();
                printTotals[6] = printTotals[6] + dataRow["MaleP"].Coalesce("0").ToInt32() + dataRow["FemaleP"].Coalesce("0").ToInt32();

                if (rowCounter == recordCount - 1)
                    printTotalCounter = 1;
                else if (printGrps[0] != dataRow["Grp"].ToString() || printGrps[1] != dataRow["Seq"].ToString())
                    printTotalCounter = 1;
                else printTotalCounter = 0;

                if (printTotalCounter != 0)
                {
                    printLine = print.FormatPrintString("", 34, 33)
                    + "------ " + "------ " + "------ " + "------ " + "------ " + "------";
                    printResult = print.PrepareAndPrint(3, 0, printLine);
                    printLine = print.FormatPrintString("", 34, 33)
                             + print.FormatPrintString(printTotals[1], 7, 6)
                             + print.FormatPrintString(printTotals[2], 7, 6)
                             + print.FormatPrintString(printTotals[3], 7, 6)
                             + print.FormatPrintString(printTotals[4], 7, 6)
                             + print.FormatPrintString(printTotals[5], 7, 6)
                             + print.FormatPrintString(printTotals[6], 7, 6);
                    printResult = print.PrepareAndPrint(3, 0, printLine);
                }
            }
            printLine = print.FormatPrintString("", 34, 33) + "-".RepeatForLeftPadding(42);
            printResult = print.PrepareAndPrint(3, 1, printLine);
            gLnI = 0;
            validationMessage = "Printing Over";

        endReport:
            print.PrepareAndPrint(9, gLnI == 1 ? 1 : 0, validationMessage);

            PrintSettings printSettings = new PrintSettings();
            printSettings.PageSize = PdfSharp.PageSize.A4;
            printSettings.PageOrientation = PdfSharp.PageOrientation.Landscape;
            return PrintLineReport(print, printSettings);
        }

        private HttpResponseMessage PrintCollageSummary_02(JObject filters)
        {
            throw new NotImplementedException();
        }

        private HttpResponseMessage PrintAdmitCard_16(JObject filters)
        {
            throw new NotImplementedException();
        }

        private HttpResponseMessage PrintAdmitCard_13(JObject filters)
        {
            throw new NotImplementedException();
        }

        private HttpResponseMessage PrintAdmitCard_8()
        {
            throw new NotImplementedException();
        }

        private HttpResponseMessage PrintRollList(JObject filters)
        {
            // ## Need revamp
            int printWidth = 80;
            int printSrNo = 0;
            long examFormId = 0;

            List<double> printTotals = new List<double>() { 0, 0, 0 };
            List<string> printGrps = new List<string>()
            {
                "",
                "",
                "",
                "",
                ""
            };

            string centColl = (reportCode == 4 ? (isSchool ? "Sch" : "Colg") : "Cent");
            string printFileName = centColl + "Roll.txt";
            string reportName = "";
            Print print = new Print();
            int pageNumber = 1;
            string printLine = print.FormatPrintString(reportName + " - " + printWidth, 50) + print.FormatPrintString(_reportPath + printFileName, 50);
            bool printResult = false;
            printResult = print.PrepareAndPrint(0, printWidth, printLine);
            if (!printResult) return new HttpResponseMessage(HttpStatusCode.NoContent);
            LegacyReportFacade reportFacade = new LegacyReportFacade(filters);
            DataSet dataSet = new DataSet();

            //dataSet = reportFacade.GetRollList_6(scheduleId, intCollegeCenter, collegeCode, center);
            string option = "";
            try { option = filters["option"].ToString(); } catch { }
            dataSet = reportFacade.GetRollList(scheduleId, (reportCode == 4 ? "Colg" : "Cent") + option, collegeCode, center, dsCode);
            if (!dataSet.HasData())
                return new HttpResponseMessage(HttpStatusCode.NoContent);

            // Store headers;
            // 1 
            printLine = instituteName;
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printResult = print.PrepareAndPrint(101, 0, printLine);
            if (!printResult) goto endReport;
            // 2
            if (reportName.IsNullOrWhiteSpace())
            {
                reportName = (reportCode == 4 ? "College" : "Center") + " Wise List";
            }
            printLine = reportName;
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printResult = print.PrepareAndPrint(102, 0, printLine);
            if (!printResult) goto endReport;
            // 3
            printLine = examName + ", " + (examHeldIn != "" ? examHeldIn : sessionName);
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printResult = print.PrepareAndPrint(103, 0, printLine);
            if (!printResult) goto endReport;

            // 3-4
            DataRow firstRow = dataSet.FirstRow();
            printGrps[0] = firstRow["Grp"].ToString();
            printGrps[1] = firstRow["StrCode"].ToString();
            printGrps[2] = firstRow["Grp1"].ToString();
            printGrps[3] = firstRow["Grp"].ToString() + "  " + firstRow["GrpName"].ToString();
            printGrps[4] = firstRow["StrCode"].ToString() + "  " + firstRow["StrName"].ToString();
            printSrNo = 0;
            printTotals[1] = 0;
            printTotals[2] = 0;

            // 3
            printLine = (reportCode == 4 ? (isSchool || isInstitute ? "Institute: " : "College: ") : "Centre: ") + printGrps[3];
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printLine = printLine.SubstringWithCorrection(0, 79);
            printLine = print.FormatPrintString(printLine, printWidth - 12) + "Page: " + pageNumber;
            printResult = print.PrepareAndPrint(104, 0, printLine);
            if (!printResult) goto endReport;

            // 4
            printLine = printGrps[4];
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printLine = print.FormatPrintString(printLine, printWidth - 12) + systemDate.ToString(dateFormat);
            printResult = print.PrepareAndPrint(105, 0, printLine);
            if (!printResult) goto endReport;

            printLine = "";
            if (instituteId == 5 || (reportCode != 4 && (instituteId == 7 || instituteId == 8)))
            {
                printLine = (reportCode == 4 ? "Centre: " : "College: ") + printGrps[3];
            }
            printResult = print.PrepareAndPrint(106, 0, printLine);
            if (!printResult) goto endReport;

            printResult = print.PrepareAndPrint(107, 0, "-".RepeatForLeftPadding(printWidth));
            if (!printResult) goto endReport;

            printLine = print.FormatPrintString((instituteId == 5 || instituteId == 7 || instituteId == 8 ? "" : (reportCode == 4 ? "Centre" : (isSchool || isInstitute ? "Inst" : "College"))), 8)
                + print.FormatPrintString("RegnNo", 15)
                + print.FormatPrintString("Cate", 6)
                + (instituteId == 5 ? print.FormatPrintString("Name", 30) + "Candidate//s   Remarks" : print.FormatPrintString("Name", 50));
            printResult = print.PrepareAndPrint(108, 0, printLine);
            if (!printResult) goto endReport;

            printLine = print.FormatPrintString("SrNo", 7)
                    + print.FormatPrintString((instituteId == 17 ? "" : "RollNo"), 11)
                    + print.FormatPrintString((instituteId == 15 ? "" : "Bran"), 6)
                    + print.FormatPrintString(" Sex", 5)
                    + print.FormatPrintString("Father", 25)
                    + print.FormatPrintString("Mother", 25);
            printResult = print.PrepareAndPrint(109, 0, printLine);
            if (!printResult) goto endReport;

            printResult = print.PrepareAndPrint(110, 0, "=".RepeatForLeftPadding(printWidth));
            if (!printResult) goto endReport;

            printLine = print.FormatPrintString("", 20) + "----------------------------------------";
            printResult = print.PrepareAndPrint(601, 8, printLine);
            if (!printResult) goto endReport;

            // Print Header
            printResult = print.PrepareAndPrint(2, 0, "");
            if (!printResult) goto endReport;

            // Print Details
            int rowCounter = 0;
            int recordCount = dataSet.Tables[0].Rows.Count;
            for (rowCounter = 0; rowCounter < recordCount; rowCounter++)
            {
                DataRow dataRow = dataSet.Tables[0].Rows[rowCounter];
                if (printGrps[0] != dataRow["Grp"].ToString() || printGrps[1] != dataRow["StrCode"].ToString())
                {

                    printSrNo = 0;
                }

                if (printGrps[0] != dataRow["Grp"].ToString()
                    || printGrps[1] != dataRow["StrCode"].ToString()
                    || (reportCode != 4 && (instituteId == 7 || instituteId == 8) && printGrps[2] != dataRow["Grp1"].ToString())
                    || (instituteId == 5 && printGrps[2] != dataRow["Grp1"].ToString()))
                {
                    printGrps[0] = dataRow["Grp"].ToString();
                    printGrps[1] = dataRow["StrCode"].ToString();
                    printGrps[2] = dataRow["Grp1"].ToString();

                    printLine = (reportCode == 4 ? (isSchool ? "Institute: " : "College: ") : "Centre: ")
                            + dataRow["Grp"].ToString() + "  " + dataRow["GrpName"].ToString();
                    printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
                    printResult = print.PrepareAndPrint(104, 0, printLine);
                    if (!printResult) goto endReport;

                    printLine = printGrps[4];
                    printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
                    printLine = print.FormatPrintString(printLine, printWidth - 12) + systemDate.ToString(dateFormat);
                    printResult = print.PrepareAndPrint(105, 0, printLine);
                    if (!printResult) goto endReport;

                    printLine = "";
                    if (instituteId == 5 || (reportCode != 4 && (instituteId == 7 || instituteId == 8)))
                    {

                        printLine = (reportCode == 4 ? "Centre: " : "College: ") + printGrps[3];
                    }
                    printResult = print.PrepareAndPrint(106, 0, printLine);
                    if (!printResult) goto endReport;

                    printResult = print.PrepareAndPrint(2, 99, "");
                    if (!printResult) goto endReport;
                }

                printSrNo = printSrNo + 1;
                string temp1 = (registrationRoll == "G" ? "" : "" + dataRow["EfRollNo"].ToString());
                if (instituteId == 8)
                {
                    if (dataRow["EfRollNo"].ToString().Length >= 5)
                        temp1 = " " + dataRow["EfRollNo"].ToString().SubstringWithCorrection(dataRow["EfRollNo"].ToString().Length - 5, 5);
                    else
                        temp1 = " " + dataRow["EfRollNo"].ToString();
                }
                if (instituteId == 16)
                    temp1 = rollNoPrefix + dataRow["EfRollNo"].ToString();

                printLine = print.FormatPrintString((instituteId == 5 || instituteId == 7 || instituteId == 8 ? "" : dataRow["Grp1"].ToString()), 8, 7)
                                + print.FormatPrintString(dataRow["RgNo"].ToString(), 15, 14)
                                + print.FormatPrintString(dataRow["EfCate"].ToString(), 6, 5)
                                + print.FormatPrintString(dataRow["RgName"].ToString(), 50);
                printResult = print.PrepareAndPrint(3, 0, printLine);

                printLine = print.FormatPrintString(printSrNo, 7, 4)
                    + print.FormatPrintString(temp1, 11, 10)
                    + print.FormatPrintString(dataRow["StrCode"].ToString(), 6, 5)
                    + print.FormatPrintString("  " + dataRow["RgSex"].ToString(), 5, 4)
                    + print.FormatPrintString(dataRow["RgFather"].ToString(), 25)
                    + print.FormatPrintString(dataRow["RgMother"].ToString(), 25);
                printResult = print.PrepareAndPrint(3, 0, printLine);

                if (instituteId == 14 && reportCode == 4)
                {
                    examFormId = dataRow["EfId"].ToString().ToInt32();
                    DataSet dataRegDetails = reportFacade.GetExamRegistrationDetails(examFormId);

                    if (dataRegDetails.HasData())
                    {
                        DataRow drRegDetail = dataRegDetails.FirstRow();
                        {
                            printLine = print.FormatPrintString("", 18)
                                                                + print.FormatPrintString("Aadhar: " + drRegDetail["RgAadhar"].ToString(), 25, 24)
                                                                + print.FormatPrintString("Phone: " + drRegDetail["RgMobile"].ToString(), 36, 35);
                            printResult = print.PrepareAndPrint(3, 0, printLine);
                        }
                    }
                }
                // Subjects
                examFormId = dataRow["EfId"].ToString().ToInt32();
                printGrps[3] = "";
                printGrps[4] = "";

            nextSubject:
                printGrps[4] = dataSet.Tables[0].Rows[rowCounter]["SubCode"].ToString() + " ";
                if ((printGrps[3].Length + printGrps[4].Length) > 50)
                {
                    printLine = print.FormatPrintString("", 29) + printGrps[3];
                    if (pageBreak == "S")
                        printResult = print.PrepareAndPrint(3, 0, printLine);

                    printGrps[3] = "";
                }
                printGrps[3] = printGrps[3] + printGrps[4];
                rowCounter++;

                printTotalCounter = 0;
                if (rowCounter == recordCount - 1 || rowCounter >= recordCount)
                    printTotalCounter = 1;
                else if (examFormId == dataSet.Tables[0].Rows[rowCounter]["EfId"].ToString().ToInt32())
                    goto nextSubject;
                else
                {
                    if (printGrps[0] != dataSet.Tables[0].Rows[rowCounter]["Grp"].ToString() || printGrps[1] != dataSet.Tables[0].Rows[rowCounter]["StrCode"].ToString())
                        printTotalCounter = 2;
                }

                if (printGrps[3].Trim() != "")
                {
                    if (pageBreak == "S")
                    {
                        printLine = print.FormatPrintString("", 29) + printGrps[3];
                        printResult = print.PrepareAndPrint(3, 0, printLine);
                    }
                }
                printResult = print.PrepareAndPrint(3, 0, "");

                if (printTotalCounter != 0)
                {
                    printLine = print.FormatPrintString(" ", 29) + "Total: " + printSrNo.ToString();
                    printResult = print.PrepareAndPrint(3, 0, printLine);
                    printTotals[1] = printTotals[1] + printSrNo;
                }

                if (printTotalCounter == 1)
                {
                    //printResult = print.PrepareAndPrint(2, 99, "");
                    //if (!printResult) goto endReport;
                    printLine = print.FormatPrintString(" ", 29) + "Grand Total: " + printTotals[1].ToString();
                    printResult = print.PrepareAndPrint(3, 0, printLine);
                }
            }
            gLnI = 0;
            validationMessage = "Printing Over";

        endReport:
            print.PrepareAndPrint(9, gLnI == 1 ? 1 : 0, validationMessage);
            PrintSettings printSettings = new PrintSettings();
            printSettings.PageSize = PdfSharp.PageSize.A4;
            printSettings.PageOrientation = PdfSharp.PageOrientation.Landscape;
            return PrintLineReport(print, printSettings);
        }

        private HttpResponseMessage PrintRollList_16(JObject filters)
        {
            throw new NotImplementedException();
        }

        private HttpResponseMessage PrintRollList_13(JObject filters)
        {
            throw new NotImplementedException();
        }

        private HttpResponseMessage PrintRollList_11(JObject filters)
        {
            throw new NotImplementedException();
        }

        private HttpResponseMessage PrintCrystalReport(string reportFileName, DataSet dataSet = null, JObject filters = null)
        {
            ReportDocument reportDocument = new ReportDocument();
            var result = new HttpResponseMessage(HttpStatusCode.InternalServerError);

            JObject json = null;
            try
            {
                json = JObject.Parse(extra1);
            }
            catch { }
            if (json != null)
            {
                try { registrationId = json.SelectToken("RgId").ToString().Coalesce().ToInt32(); } catch { }
                try { examId = json.SelectToken("EmId").ToString().Coalesce().ToInt32(); } catch { }
                try { ecId = json.SelectToken("EcId").ToString().Coalesce().ToInt32(); } catch { }
                try { paper = json.SelectToken("Paper").Coalesce(); } catch { }
                try { resultType = json.SelectToken("ResultType").Coalesce(); } catch { }
                try { centerCollege = json.SelectToken("CenterCollege").Coalesce("1").ToInt32(); } catch { }
                try { esId = json.SelectToken("EsId").ToString().Coalesce().ToInt32(); } catch { }
            }

            try
            {
                string tempReportPath = "";
                if (!instituteCode.IsNullOrWhiteSpace())
                    tempReportPath = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath(reportPath), instituteCode, reportFileName);

                if (instituteCode.IsNullOrWhiteSpace() || !File.Exists(tempReportPath))
                    tempReportPath = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath(reportPath), reportFileName);

                //reportDocument.Load(Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath(reportPath), reportFileName));
                reportDocument.Load(tempReportPath);
                #region Change Report Connection string (Only possible if old is set to ODBC and not DSN)
                //(string userId, string password) = ConversionUtils.DecodeBase64Credentials(WebSettings.DBCredentials);
                //// For main report
                //foreach (Table table in reportDocument.Database.Tables)
                //{
                //    TableLogOnInfo logonInfo = table.LogOnInfo;
                //    logonInfo.ConnectionInfo.ServerName = WebSettings.DBServer;
                //    logonInfo.ConnectionInfo.DatabaseName = WebSettings.DBName;
                //    logonInfo.ConnectionInfo.UserID = userId;
                //    logonInfo.ConnectionInfo.Password = password;

                //    table.ApplyLogOnInfo(logonInfo);
                //}

                //// For subreports
                //foreach (ReportDocument subReport in reportDocument.Subreports)
                //{
                //    foreach (Table table in subReport.Database.Tables)
                //    {
                //        TableLogOnInfo logonInfo = table.LogOnInfo;
                //        logonInfo.ConnectionInfo.ServerName = WebSettings.DBServer;
                //        logonInfo.ConnectionInfo.DatabaseName = WebSettings.DBName;
                //        logonInfo.ConnectionInfo.UserID = userId;
                //        logonInfo.ConnectionInfo.Password = password;
                //        table.ApplyLogOnInfo(logonInfo);
                //    }
                //}
                //reportDocument.VerifyDatabase();
                #endregion
                // Get list of report parameters
                ParameterFields parameterFields = reportDocument.ParameterFields;
                if ("MarkLKLE.rpt,MarkLKLE_Formatted.rpt,MarkLKKH_Formatted.rpt".ToCSV().Contains(reportFileName))
                {
                    string recordSelectionFormula = "";
                    switch (resultType.ToUpper())
                    {
                        case "A":
                            recordSelectionFormula = "";
                            break;
                        case "P":
                            recordSelectionFormula = "{@Result} <> \"Fail\"";
                            break;
                        case "F":
                            recordSelectionFormula = "{@Result} <> \"Pass\"";
                            break;
                    }
                    reportDocument.RecordSelectionFormula = $"({recordSelectionFormula})";
                }
                foreach (ParameterField parameterField in parameterFields)
                {
                    switch (parameterField.Name.ToUpper())
                    {
                        case "MAINPARAM": //MainParam
                            reportDocument.SetParameterValue(parameterField.Name, "Main Report Parameter");
                            break;
                        case "@LOGINID": //@LoginId
                            reportDocument.SetParameterValue(parameterField.Name, loginId);
                            break;
                        case "@SHID": //@ShId
                            reportDocument.SetParameterValue(parameterField.Name, scheduleId);
                            break;
                        case "@STRID": //@StrId
                        case "@EFSTRID": //@EfStrId
                            reportDocument.SetParameterValue(parameterField.Name, streamId);
                            break;
                        case "@CGCODE": //@CgCode
                            reportDocument.SetParameterValue(parameterField.Name, collegeCode);
                            break;
                        case "@CENTCODE": //@CentCode
                        case "@CENT": //@Cent
                            reportDocument.SetParameterValue(parameterField.Name, center);
                            break;
                        case "@CENTCG": //@CentCg
                            reportDocument.SetParameterValue(parameterField.Name, centerCollege);
                            break;
                        case "@TNCODE": //@TnCode
                            reportDocument.SetParameterValue(parameterField.Name, tnCode);
                            break;
                        case "@ROLLNO": //@RollNo
                        case "@EFROLLNO": //@EfRollNo
                            reportDocument.SetParameterValue(parameterField.Name, rollNo);
                            break;
                        case "@RGNO": //@RgNo
                            reportDocument.SetParameterValue(parameterField.Name, registartionNo.Coalesce());
                            break;
                        case "@ESID": //@EsId
                            reportDocument.SetParameterValue(parameterField.Name, esId);
                            break;
                        case "@CSNO": //@CsNo
                            reportDocument.SetParameterValue(parameterField.Name, csNo);
                            break;
                        case "@COLLUNI": //@CollUni
                        case "@COLS": //@Cols
                        case "@UC": //@UC
                            reportDocument.SetParameterValue(parameterField.Name, uc);
                            break;
                        case "@FLAG": //@Flag
                        case "@CATE": //@Cate
                            reportDocument.SetParameterValue(parameterField.Name, pageBreak);
                            break;
                        case "@INST": //@Inst
                            reportDocument.SetParameterValue(parameterField.Name, instituteId);
                            break;
                        case "@ECID": //@EcId
                            reportDocument.SetParameterValue(parameterField.Name, ecId);
                            break;
                        case "@REGNROLL": //@RegnRoll
                            reportDocument.SetParameterValue(parameterField.Name, registrationRoll);
                            break;
                        case "@IMG": //@Img
                            reportDocument.SetParameterValue(parameterField.Name, " ");
                            break;
                        case "@SNNO": //@SnNo
                            reportDocument.SetParameterValue(parameterField.Name, srNo);
                            break;
                        case "@SESSION": //@Session
                            reportDocument.SetParameterValue(parameterField.Name, sessionName);
                            break;
                        case "@COCODE": //@CoCode
                            reportDocument.SetParameterValue(parameterField.Name, courseCode);
                            break;
                        case "@DSCODE": //@DsCode
                            reportDocument.SetParameterValue(parameterField.Name, dsCode);
                            break;
                        case "@STRHELDIN": //@strHeldIn
                            reportDocument.SetParameterValue(parameterField.Name, examHeldIn);
                            break;
                        case "@TYPE": //@Type
                            reportDocument.SetParameterValue(parameterField.Name, pageBreak);
                            break;
                        case "@REP": //@Rep
                            reportDocument.SetParameterValue(parameterField.Name, rep);
                            break;
                        case "@PASSFAIL": //@PassFail
                            reportDocument.SetParameterValue(parameterField.Name, passFail);
                            break;
                        case "@EXCP": //@Excp
                            reportDocument.SetParameterValue(parameterField.Name, "");
                            break;
                        case "@COURSES": //@Courses
                            reportDocument.SetParameterValue(parameterField.Name, "," + courseCode + ",");
                            break;
                        case "@SESSIONS": //@Sessions
                            reportDocument.SetParameterValue(parameterField.Name, "," + sessionNo + ",");
                            break;
                        case "@FOOTERSIZE": //@FooterSize
                            reportDocument.SetParameterValue(parameterField.Name, "," + footerSize + ",");
                            break;

                        case "@ECINCODE": //@EcInCode
                            reportDocument.SetParameterValue(parameterField.Name, inCode);
                            break;
                        case "@CHNG": //@Chng
                        case "@ALLLAST": //@AllLast
                            reportDocument.SetParameterValue(parameterField.Name, change);
                            break;
                        case "@EMID":
                            reportDocument.SetParameterValue(parameterField.Name, examId);
                            break;
                        case "@RGID":
                            reportDocument.SetParameterValue(parameterField.Name, registrationId);
                            break;
                        case "@PAPER":
                        case "@ESCODE":
                            reportDocument.SetParameterValue(parameterField.Name, paper);
                            break;
                    }
                }

                if (reportCode == 5 && instituteId == 11)
                {
                    examName = examCourseCode + "-" + examCourseName;
                }
                // Single quote not allowed in formula
                examName = examName.Replace("'", "");
                examShortName = examShortName.Replace("'", "");
                sessionName = sessionName.Replace("'", "");

                // Reval / Change Ledger
                if ((reportCode == 17 || reportCode == 31) && pageBreak != "")
                {
                    examHeldIn += (pageBreak == "E" ? " (EditList)" : (pageBreak == "R" ? " (Revaluation)" : (pageBreak == "C" ? " (Changed)" : "")));
                }

                // Crystal Report formulas
                // Get the list of report objects (including formulas)
                FormulaFieldDefinitions formulaFields = reportDocument.DataDefinition.FormulaFields;

                foreach (FormulaFieldDefinition formulaField in formulaFields)
                {
                    switch (formulaField.Name.ToUpper())
                    {
                        case "CAPTION":
                            formulaField.Text = $"'{instituteName}'";
                            break;
                        case "CAPTION1":
                            formulaField.Text = $"'{institutePlace}'";
                            break;
                        case "CENTCG":
                            formulaField.Text = $"{centerCollege}";
                            break;
                        case "TITLE":
                            formulaField.Text = $"'{reportName}'";
                            break;
                        case "SUBTITLE":
                            formulaField.Text = $"'{reportSubTitle}'";
                            break;
                        case "ENNAME":
                        case "EMNAME":
                        case "EXAMNAME":
                            formulaField.Text = $"'{examName}'";
                            break;
                        case "EMCODE":
                            formulaField.Text = $"'{examCode}'";
                            break;
                        case "EMSHNAME":
                            formulaField.Text = $"'{examShortName}'";
                            break;
                        case "SHPATH":
                            formulaField.Text = $"'{imagePath}'";
                            break;
                        case "SESSION":
                        case "EMSESSION":
                            formulaField.Text = $"'{sessionName}'";
                            break;
                        case "BREAK":
                            formulaField.Text = $"'{pageBreak}'";
                            break;
                        case "HELDIN":
                        case "STRHELDIN":
                            formulaField.Text = $"'{examHeldIn}'";
                            break;
                        case "RESULTDATE":
                        case "EMDATE":
                            formulaField.Text = $"'{resultDate}'";
                            break;
                        case "EMMONTH":
                            formulaField.Text = $"'{examHeldIn + (pageBreak == "E" ? " (EditList)" : (pageBreak == "R" ? " (Revaluation)" : (pageBreak == "C" ? " (Changed)" : "")))}'";
                            break;
                        case "BRANCH":
                            formulaField.Text = $"'{"Branch: " + streamCode + " " + streamName}'";
                            break;
                        case "SNYEAR":
                            formulaField.Text = $"'{sessionYear}'";
                            break;
                        case "SNCODE":
                            formulaField.Text = $"'{sessionCode}'";
                            break;
                        case "SINGLELINE":
                            formulaField.Text = $"'{singleLine}'";
                            break;
                        case "INSTNO":
                        case "INST":
                            formulaField.Text = $"{instituteId.ToString()}";
                            break;
                        case "EXAMDATE":
                            formulaField.Text = $"'{examDate}'";
                            break;
                        case "REP":
                            formulaField.Text = $"'{rep}'";
                            break;
                        case "ROLL":
                            formulaField.Text = $"'{rollNo.ToString()}'";
                            break;
                        case "DETAILS":
                            formulaField.Text = $"'{details}'";
                            break;
                        case "AUTHDSGN":
                            formulaField.Text = $"'{authorityDesignation}'";
                            break;
                        case "AUTHNAME":
                            formulaField.Text = $"'{authorityName}'";
                            break;
                        case "USR_DATE":
                            formulaField.Text = $"'{loginName} / {systemDate.ToString()}'";
                            break;
                        case "COPATTERN":
                            formulaField.Text = $"'{coursePattern}'";
                            break;
                        case "INCODE":
                            formulaField.Text = $"'{inCode}'";
                            break;
                        case "LOGOPATH":
                            logoPath = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/Reports/Logos"), filters["DBName"].Coalesce() + ".jpg");
                            if (!File.Exists(logoPath))
                                logoPath = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/Reports/Logos"), "Logo.jpg");
                            formulaField.Text = $"'{logoPath}'";
                            break;
                        case "RESULTTYPE":
                            formulaField.Text = $"'{resultType}'";
                            break;
                        case "PLACE":
                            formulaField.Text = $"'{institutePlace}'";
                            break;

                        default:
                            if (dataSet.HasData())
                            {
                                try
                                {
                                    int row = 0;
                                    int col = 0;
                                    row = formulaField.Name.SubstringWithCorrection(3).ToInt32();
                                    switch (formulaField.Name.SubstringWithCorrection(0, 3).ToLower())
                                    {
                                        case "sub":
                                            col = 5;
                                            break;
                                        case "opt":
                                            col = 6;
                                            break;
                                        case "max":
                                            col = 7;
                                            break;
                                        case "min":
                                            col = 8;
                                            break;
                                    }
                                    if (row > 0 && col > 0)
                                    {
                                        if (row <= dataSet.Tables[0].Rows.Count)
                                        {
                                            formulaField.Text = $"'{dataSet.Tables[0].Rows[row - 1][col].ToString()}'";
                                        }
                                        else
                                            formulaField.Text = "";
                                    }
                                }
                                catch (Exception er) { }

                            }
                            break;
                    }
                    if (!formulaField.Text.IsNullOrWhiteSpace())
                    {
                        if (formulaField.Text.IndexOf("select {@InCode}") == 0)
                        {
                            string temp = "";
                            switch (inCode)
                            {
                                case 7:
                                    temp = "Marks By " + (registartionNo == "G" ? "RegnNo" : "RollNo");
                                    break;
                                case 8:
                                    temp = "Marks By Code ";
                                    break;
                                case 11:
                                    temp = "Status Report ";
                                    break;
                                case 14:
                                    temp = "Marks Change ";
                                    break;
                                case 15:
                                    temp = "Section Marks ";
                                    break;
                            }
                            temp = temp + (reportSubTitle.IsNullOrWhiteSpace() ? "" : " " + reportSubTitle);
                            formulaField.Text = $"'{temp}'";
                        }

                        //if (formulaField.Text.IndexOf("{@InCode}") >= 0)
                        //{
                        //    formulaField.Text = formulaField.Text.Replace("{@InCode}", $"\"{inCode}\"");
                        //}
                        //if (formulaField.Text.IndexOf("{@RgRoll}") >= 0)
                        //{
                        //    formulaField.Text = formulaField.Text.Replace("{@RgRoll}", $"\"{registartionNo}\"");
                        //}

                        //else if (formulaField.Text.IndexOf("if {@InCode} = \"8\" then cstr({sp_SearchEditMarks;1.BarCode},0,'')") == 0)
                        //{
                        //    if (inCode == 8)
                        //        formulaField.Text = "\"{sp_SearchEditMarks;1.BarCode},0,''\"";
                        //    else if (registartionNo == "G")
                        //        formulaField.Text = "\"{sp_SearchEditMarks;1.RgNo}\"";
                        //    else
                        //        formulaField.Text = "\"{sp_SearchEditMarks;1.RollNo},0,''\"";
                        //}
                        //else if (formulaField.Text.IndexOf("{sp_SearchEditMarks;1.Sub}") >= 0)
                        //{
                        //    formulaField.Text = "''";
                        //}
                        //else if (formulaField.Text.IndexOf("if {@InCode} = \"11\" then {sp_SearchEditMarks;1.Status} ") >= 0)
                        //{
                        //    if (inCode == 11)
                        //        formulaField.Text = "'{sp_SearchEditMarks;1.Status} '";

                        //}
                        formulaField.Text = formulaField.Text.Replace("{?@CentCg}", centerCollege.ToString());
                        formulaField.Text = formulaField.Text.Replace("{?@Inst}", instituteId.ToString());
                        //formulaField.Text = formulaField.Text.Replace("{?@DivPc}", "0");


                        string divPc = MapExtraParams("DivPc", filters).Coalesce("0");

                        formulaField.Text = formulaField.Text.Replace("{?@DivPc}", divPc);
                        //formulaField.Text = formulaField.Text.Replace("{?@ExamName}", instituteId.ToString());


                        //formulaField.Text = formulaField.Text.Replace("{?@InCode}", inCode.ToString());
                        //formulaField.Text = formulaField.Text.Replace("{?@RgRoll}", registartionNo.ToString());
                        switch (reportFileName)
                        {
                            case "Ledger.rpt":
                                switch (formulaField.Name.ToUpper())
                                {
                                    case "HDRCENTCG":
                                        formulaField.Text = centerCollege == 1 ? "'Colg'" : "'Cent'";
                                        break;
                                    case "TOTAL":
                                        formulaField.Text = formulaField.Text.Replace("{@Inst}", instituteId.ToString());
                                        break;
                                }
                                break;
                        }
                    }
                }



                //// Map Data to report
                // For Main Report
                foreach (Table table in reportDocument.Database.Tables)
                {
                    TableLogOnInfo logonInfo = table.LogOnInfo;
                    string query = table.Name;
                    query = query.EndsWith(";1") ? query.Replace(";1", "") : query;
                    DataTable dataTable = ExecuteReportQuery(query, filters).FirstTable();

                    if (!dataTable.HasData())
                        return NoContent();

                    dataTable.TableName = table.Name;
                    table.SetDataSource(dataTable);
                }
                // For subreports
                foreach (ReportDocument subReport in reportDocument.Subreports)
                {
                    foreach (Table table in subReport.Database.Tables)
                    {
                        TableLogOnInfo logonInfo = table.LogOnInfo;
                        string query = table.Name;
                        query = query.EndsWith(";1") ? query.Replace(";1", "") : query;
                        DataTable dataTable = ExecuteReportQuery(query, filters).FirstTable();
                        dataTable.TableName = table.Name;
                        table.SetDataSource(dataTable);
                    }
                }

                // Prepare Report stream in PDF format
                MemoryStream ms = new MemoryStream();
                using (var stream = reportDocument.ExportToStream(ExportFormatType.PortableDocFormat))
                {
                    stream.CopyTo(ms);
                }

                result = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(ms.ToArray())
                };
                result.Content.Headers.ContentDisposition =
                    new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                    {
                        FileName = reportFileName.Replace(".rpt", ".pdf")
                    };
                result.Content.Headers.ContentType =
                    new System.Net.Http.Headers.MediaTypeHeaderValue("application/pdf");
            }
            catch (Exception err)
            {
                log.Error(err.Message, err);
                log.Error(err.StackTrace, err);
            }
            return result;
        }

        private string MapExtraParams(string parameterName, JObject filters)

        {

            string result = "";

            try

            {

                JObject json = null;

                json = JObject.Parse(extra1);

                if (!json.SelectToken(parameterName.Replace("@", "")).Coalesce("").IsNullOrWhiteSpace() ||

                    !filters.SelectToken(parameterName.Replace("@", "")).Coalesce("").IsNullOrWhiteSpace())

                {

                    var jsonParameterValue = json.SelectToken(parameterName.Replace("@", ""));

                    if (!filters.SelectToken(parameterName.Replace("@", "")).Coalesce("").IsNullOrWhiteSpace())

                        jsonParameterValue = filters.SelectToken(parameterName.Replace("@", ""));



                    result = jsonParameterValue.ToString();

                }

            }

            catch { }



            return result;

        }

        private DataSet ExecuteReportQuery(string query, JObject filters, bool passDefaultValues = false)
        {
            DataSet dataSet = new DataSet();

            //Check if SELECT query used in report
            if (query.ToUpper().IndexOf("SELECT") > -1)
            {
                log.Error("Query::" + query);
                throw new NotImplementedException();
            }
            // Else its a Stored Procedure
            else
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                LegacyReportFacade legacyReportFacade = new LegacyReportFacade(filters);
                sqlParameters = legacyReportFacade.GetSqlParameters(query);
                foreach (SqlParameter sqlParameter in sqlParameters)
                {
                    switch (sqlParameter.ParameterName)
                    {
                        case "@Chng":
                            sqlParameter.Value = change;
                            break;
                        case "@Valid":
                            sqlParameter.Value = "";
                            break;
                        case "@Sub":
                            sqlParameter.Value = "";
                            break;
                        case "@Head":
                            sqlParameter.Value = "";
                            break;
                        case "@LoginId":
                            sqlParameter.Value = loginId;
                            break;
                        case "@ShId":
                            sqlParameter.Value = scheduleId;
                            break;
                        case "@StrId":
                        case "@EfStrId":
                            sqlParameter.Value = streamId;
                            break;
                        case "@CgCode":
                            sqlParameter.Value = collegeCode;
                            break;
                        case "@CentCode":
                        case "@Cent":
                            sqlParameter.Value = center;
                            break;
                        case "@CentCg":
                            sqlParameter.Value = centerCollege;
                            break;
                        case "@TnCode":
                            sqlParameter.Value = tnCode;
                            break;
                        case "@RollNo":
                        case "@EfRollNo":
                            sqlParameter.Value = rollNo;
                            break;
                        case "@RgNo":
                            sqlParameter.Value = registartionNo.Coalesce();
                            break;
                        case "@EsId":
                            sqlParameter.Value = esId;
                            break;
                        case "@CsNo":
                            sqlParameter.Value = csNo;
                            break;
                        case "@CollUni":
                        case "@Cols":
                        case "@UC":
                            sqlParameter.Value = uc;
                            break;
                        case "@Flag":
                        case "@Cate":
                            sqlParameter.Value = pageBreak;
                            break;
                        case "@Inst":
                            sqlParameter.Value = instituteId;
                            break;
                        case "@EcId":
                            sqlParameter.Value = ecId;
                            break;
                        case "@RegnRoll":
                            sqlParameter.Value = registrationRoll;
                            break;
                        case "@Img":
                            sqlParameter.Value = " ";
                            break;
                        case "@SnNo":
                            sqlParameter.Value = srNo;
                            //sqlParameter.Value = sessionNo;
                            break;
                        case "@Session":
                            sqlParameter.Value = sessionName;
                            break;
                        case "@CoCode":
                            sqlParameter.Value = courseCode;
                            break;
                        case "@DsCode":
                            sqlParameter.Value = dsCode;
                            break;
                        case "@strHeldIn":
                            sqlParameter.Value = examHeldIn;
                            break;
                        case "@Type":
                            sqlParameter.Value = pageBreak;
                            break;
                        case "@Rep":
                            sqlParameter.Value = rep;
                            break;
                        case "@PassFail":
                            sqlParameter.Value = passFail;
                            break;
                        case "@Excp":
                            sqlParameter.Value = "";
                            break;
                        case "@Courses":
                            sqlParameter.Value = "," + courseCode + ",";
                            break;
                        case "@Sessions":
                            sqlParameter.Value = "," + sessionNo + ",";
                            break;
                        case "@FooterSize":
                            sqlParameter.Value = "," + footerSize + ",";
                            break;
                        case "@EcShId":
                            sqlParameter.Value = scheduleId;
                            break;
                        case "@InCode":
                            sqlParameter.Value = inCode;
                            break;
                        case "@AllLast":
                            sqlParameter.Value = "," + filters["AllLast"].Coalesce("A") + ",";
                            break;
                        case "@RgId":
                            sqlParameter.Value = registrationId;
                            break;
                        case "@EmId":
                            sqlParameter.Value = examId;
                            break;
                        case "@PageSize":
                            sqlParameter.Value = 0;
                            break;
                        case "@PageNumber":
                            sqlParameter.Value = 0;
                            break;
                        case "@ResultType":
                            sqlParameter.Value = resultType;
                            break;
                        case "@ReportCode":
                            sqlParameter.Value = reportCode;
                            break;
                        case "@StuId":
                        case "@StudentId":
                            sqlParameter.Value = studentId;
                            break;
                        case "@SessionId":
                        case "@SessionNo":
                            sqlParameter.Value = sessionNo;
                            break;
                        default:
                            bool isExtraParamSet = false;
                            try
                            {
                                JObject json = null;
                                json = JObject.Parse(extra1);
                                if (!json.SelectToken(sqlParameter.ParameterName.Replace("@", "")).Coalesce("").IsNullOrWhiteSpace() ||
                                    !filters.SelectToken(sqlParameter.ParameterName.Replace("@", "")).Coalesce("").IsNullOrWhiteSpace())
                                {
                                    var sqlParamValue = json.SelectToken(sqlParameter.ParameterName.Replace("@", ""));
                                    if (!filters.SelectToken(sqlParameter.ParameterName.Replace("@", "")).Coalesce("").IsNullOrWhiteSpace())
                                        sqlParamValue = filters.SelectToken(sqlParameter.ParameterName.Replace("@", ""));

                                    switch (sqlParameter.SqlDbType)
                                    {
                                        case SqlDbType.Text:
                                        case SqlDbType.NText:
                                        case SqlDbType.Char:
                                        case SqlDbType.NChar:
                                        case SqlDbType.VarChar:
                                        case SqlDbType.NVarChar:
                                            sqlParameter.Value = sqlParamValue.ToString();
                                            break;
                                        case SqlDbType.BigInt:
                                        case SqlDbType.TinyInt:
                                        case SqlDbType.Int:
                                        case SqlDbType.SmallInt:
                                        case SqlDbType.Bit:
                                        case SqlDbType.Decimal:
                                        case SqlDbType.Float:
                                        case SqlDbType.Money:
                                        case SqlDbType.SmallMoney:
                                            //sqlParameter.Value = json.SelectToken(sqlParameter.ParameterName.Replace("@", "")).Coalesce("0").ToInt32();
                                            sqlParameter.Value = sqlParamValue.ToString().ToInt32();
                                            break;
                                    }

                                    isExtraParamSet = true;
                                }
                            }
                            catch { }
                            if (passDefaultValues && !isExtraParamSet)
                            {
                                switch (sqlParameter.SqlDbType)
                                {
                                    case SqlDbType.Text:
                                    case SqlDbType.NText:
                                    case SqlDbType.Char:
                                    case SqlDbType.NChar:
                                    case SqlDbType.VarChar:
                                    case SqlDbType.NVarChar:
                                        sqlParameter.Value = filters[sqlParameter.ParameterName.Replace("@", "")].Coalesce("");
                                        break;
                                    case SqlDbType.BigInt:
                                    case SqlDbType.TinyInt:
                                    case SqlDbType.Int:
                                    case SqlDbType.SmallInt:
                                    case SqlDbType.Bit:
                                    case SqlDbType.Decimal:
                                    case SqlDbType.Float:
                                    case SqlDbType.Money:
                                    case SqlDbType.SmallMoney:
                                        sqlParameter.Value = filters[sqlParameter.ParameterName.Replace("@", "")].Coalesce("0").ToInt32();
                                        break;
                                }

                            }
                            break;
                    }
                }

                dataSet = legacyReportFacade.ExecuteStoredProcedure(query, sqlParameters);
            }
            return dataSet;
        }

        private HttpResponseMessage PrintLineReport(Print print, PrintSettings printSettings = null)
        {
            string[] textLines = print.ReadReport();

            if (printSettings == null)
                printSettings = new PrintSettings();

            // Generate PDF with custom settings
            HttpResponseMessage response = PdfGenerator.GeneratePdf(
                textLines,
                pageSize: printSettings.PageSize,
                pageOrientation: printSettings.PageOrientation,
                leftMargin: printSettings.LeftMargin,
                rightMargin: printSettings.RightMargin,
                topMargin: printSettings.TopMargin,
                bottomMargin: printSettings.BottomMargin,
                reportHeader: printSettings.ReportHeader,
                reportFooter: printSettings.ReportFooter,
                pageHeader: printSettings.PageHeader,
                pageFooter: printSettings.PageFooter,
                pageNumbering: printSettings.PageNumbering,
                fontSize: printSettings.FontSize);

            return response;
        }

        private HttpResponseMessage PrintLineReport(string textFilePath)
        {
            // Read the content of a text file (replace with your file path)
            if (!File.Exists(textFilePath))
                CreateFolderAndFile(textFilePath);

            string[] textLines = File.ReadAllLines(textFilePath);

            // Generate PDF with custom settings
            HttpResponseMessage response = PdfGenerator.GeneratePdf(
                textLines,
                pageSize: PdfSharp.PageSize.A4,
                pageOrientation: PdfSharp.PageOrientation.Landscape,
                leftMargin: 50,
                rightMargin: 50,
                topMargin: 50,
                bottomMargin: 50,
                reportHeader: "Report Header",
                reportFooter: "Report Footer",
                pageHeader: "Page Header",
                pageFooter: "Page Footer",
                pageNumbering: true);

            return response;
        }

        private void CreateFolderAndFile(string fullFilePath)
        {
            // Get the directory from the full file path
            string directory = Path.GetDirectoryName(fullFilePath);

            // Check if the directory exists, if not, create it
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Check if the file exists, if not, create it and write an empty string
            if (!File.Exists(fullFilePath))
            {
                File.WriteAllText(fullFilePath, string.Empty);
            }
        }
        private HttpResponseMessage PrintRollList_6(JObject filters)
        {
            //throw new NotImplementedException();
            int printWidth = 80;
            int printSrNo = 0;
            int intCollegeCenter = 0;

            List<double> printTotals = new List<double>() { 0, 0, 0 };
            List<string> printGrps = new List<string>()
            {
                "",
                "",
                ""
            }; //District, Centre, GrandTotal;

            intCollegeCenter = reportCode == 4 ? 2 : 1;

            string centColl = (reportCode == 4 ? (isSchool ? "Sch" : "Colg") : "Cent");
            string printFileName = centColl + "Roll.txt";
            string reportName = "";
            if (reportCode == 4)
            {
                reportName = (isSchool ? "School" : "College") + " Roll List";
            }
            else
            {
                reportName = "Center Roll List";
            }
            Print print = new Print();
            int pageNumber = 1;
            string printLine = print.FormatPrintString(reportName + " - " + printWidth, 50) + print.FormatPrintString(_reportPath + printFileName, 50);
            bool printResult = false;
            printResult = print.PrepareAndPrint(0, printWidth, printLine);
            if (!printResult) return new HttpResponseMessage(HttpStatusCode.NoContent);
            LegacyReportFacade reportFacade = new LegacyReportFacade(filters);
            DataSet dataSet = new DataSet();

            dataSet = reportFacade.GetRollList_6(scheduleId, intCollegeCenter, collegeCode, center);
            if (!dataSet.HasData())
                return new HttpResponseMessage(HttpStatusCode.NoContent);

            // Store headers;
            // 1 
            printLine = instituteName;
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printResult = print.PrepareAndPrint(101, 0, printLine);
            if (!printResult) goto endReport;
            // 2
            printLine = reportName;
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printResult = print.PrepareAndPrint(102, 0, printLine);
            if (!printResult) goto endReport;
            // 3
            printLine = examName + ", " + (examHeldIn != "" ? examHeldIn : sessionName);
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printResult = print.PrepareAndPrint(103, 0, printLine);
            if (!printResult) goto endReport;

            // 4
            DataRow firstRow = dataSet.FirstRow();
            printGrps[0] = firstRow["Grp"].ToString();
            printGrps[1] = firstRow["RgNo"].ToString();
            printLine = firstRow["Grp"].ToString() + "  " + firstRow["GrpName"].ToString();
            printTotals[1] = 0;

            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printLine = printLine.SubstringWithCorrection(0, 67);
            printLine = print.FormatPrintString(printLine, printWidth - 12) + "Page: " + pageNumber;
            printResult = print.PrepareAndPrint(104, 0, printLine);
            if (!printResult) goto endReport;

            // 5
            printResult = print.PrepareAndPrint(105, 0, "-".RepeatForLeftPadding(printWidth));
            if (!printResult) goto endReport;
            // 6
            printLine = print.FormatPrintString("RegnNo", 15, 14)
                + print.FormatPrintString((intCollegeCenter == 1 ? "Cent.Code" : (isSchool ? "Sch" : "Colg")), 10, 9)
                + print.FormatPrintString("Name", 45, 44)
                + print.FormatPrintString("Sex", 4, 3)
                + print.FormatPrintString("Caste", 5, 5);
            printResult = print.PrepareAndPrint(106, 0, printLine);
            if (!printResult) goto endReport;

            // 7
            printLine = print.FormatPrintString("", 15) + "Subjects" + (intCollegeCenter == 2 ? "" : " Offered");
            printResult = print.PrepareAndPrint(107, 0, printLine);
            if (!printResult) goto endReport;
            // 8
            printResult = print.PrepareAndPrint(108, 0, "=".RepeatForLeftPadding(printWidth));
            if (!printResult) goto endReport;

            //Print Header;
            printResult = print.PrepareAndPrint(2, 0, "");
            if (!printResult) goto endReport;

            // Print Details
            //int rowCounter = 0;
            string temp = "";
            string temp2 = "";
            int recordCount = dataSet.Tables[0].Rows.Count;
            for (int rowCounter = 0; rowCounter < recordCount; rowCounter++)
            {
                DataRow dataRow = dataSet.Tables[0].Rows[rowCounter];
                //rowCounter++;
                if (printGrps[0] != dataRow["Grp"].ToString())
                {
                    printGrps[0] = dataRow["Grp"].ToString();
                    printLine = (dataRow["Grp"].ToString() + "  " + dataRow["GrpName"].ToString()).SubstringWithCorrection(0, 67);
                    printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
                    printLine = print.FormatPrintString(printLine, printWidth - 12) + "Page: " + pageNumber;
                    printResult = print.PrepareAndPrint(104, 0, printLine);
                    if (!printResult) goto endReport;
                    //;
                    printResult = print.PrepareAndPrint(2, 99, "");
                    if (!printResult) goto endReport;
                    printTotals[1] = 0;
                }

                if (printTotals[1] != 0 && printTotals[1] % 20 == 0)
                {
                    printResult = print.PrepareAndPrint(2, 99, "");
                    if (!printResult) goto endReport;
                }

                printTotals[1] = printTotals[1] + 1;
                printLine = print.FormatPrintString(dataRow["RgNo"].ToString(), 15, 14)
                    + print.FormatPrintString(dataRow["CgCent"].ToString(), 10, 9)
                    + print.FormatPrintString(dataRow["RgName"].ToString(), 45, 44)
                    + print.FormatPrintString(dataRow["RgSex"].ToString(), 4, 3)
                    + print.FormatPrintString(dataRow["RgCaste"].ToString(), 5, 4);
                printResult = print.PrepareAndPrint(3, 1, printLine);
                printGrps[1] = dataRow["RgNo"].ToString();

                // Subjects
                temp = "";
            nextSubject:
                temp2 = "";
                if (intCollegeCenter == 2 || (dataRow["FsApp"].ToString() != "N" && dataRow["FsPrvExmS"].ToString() != "Y"))
                {
                    dataRow = dataSet.Tables[0].Rows[rowCounter];

                    temp2 = dataRow["EsShName"].ToString()
                        + (dataRow["FsApp"].ToString() == "N" ? "-NA" : "")
                        + (dataRow["FsPrvExmS"].ToString() == "Y" ? "-" + dataRow["FsMarks"].ToString() : "");
                    if ((temp + temp2).Length > 66)
                    {
                        printLine = print.FormatPrintString("", 15) + temp;
                        printResult = print.PrepareAndPrint(3, 0, printLine);
                        temp = temp2;
                    }
                    else
                        temp = temp + (temp != "" ? ", " : "") + temp2;
                }

                printTotalCounter = 0;
                if (rowCounter == recordCount - 1)
                    printTotalCounter = 1;
                //else if (dataRow["Grp"].ToString() != printGrps[0])
                //    printTotalCounter = 2;
                //else if (dataRow["RgNo"].ToString() != printGrps[1])
                else if (rowCounter < recordCount - 1 && dataSet.Tables[0].Rows[rowCounter + 1]["Grp"].ToString() != printGrps[0])
                    printTotalCounter = 2;
                else if (rowCounter < recordCount - 1 && dataSet.Tables[0].Rows[rowCounter + 1]["RgNo"].ToString() != printGrps[1])
                    printTotalCounter = 3;
                else
                {
                    rowCounter++;
                    goto nextSubject;
                }

                if (temp.Trim() != "")
                {
                    printLine = print.FormatPrintString("", 15) + temp;
                    printResult = print.PrepareAndPrint(3, 0, printLine);
                }

                if (printTotalCounter < 3)
                {
                    printLine = "  Total Candidates: " + printTotals[1].ToString();
                    printResult = print.PrepareAndPrint(3, 0, printLine);
                }
            }

            gLnI = 0;
            validationMessage = "Printing Over";

        endReport:
            print.PrepareAndPrint(9, gLnI == 1 ? 1 : 0, validationMessage);
            PrintSettings printSettings = new PrintSettings();
            printSettings.PageSize = PdfSharp.PageSize.A4;
            printSettings.PageOrientation = PdfSharp.PageOrientation.Landscape;
            return PrintLineReport(print, printSettings);
        }

        private HttpResponseMessage PrintRollList_5(JObject filters)
        {
            throw new NotImplementedException();
        }

        private HttpResponseMessage PreparePaperCount(JObject filters)
        {
            //throw new NotImplementedException();

            //Sr 4, EsSeq, EsShName 21, SubCode, HdCode 12, Medium, EsSeq, Cnt;
            int printWidth = 80;
            int printSrNo = 0;

            List<double> printTotals = new List<double>() { 0, 0, 0 };
            List<string> printGrps = new List<string>()
            {
                "",
                "",
                ""
            };
            string printFileName = "Papers.txt";
            string reportName = "";
            Print print = new Print();
            int pageNumber = 1;
            string printLine = print.FormatPrintString(reportName + " - " + printWidth, 50) + print.FormatPrintString(_reportPath + printFileName, 50);
            bool printResult = false;
            printResult = print.PrepareAndPrint(0, printWidth, printLine);
            if (!printResult) return new HttpResponseMessage(HttpStatusCode.NoContent);
            LegacyReportFacade reportFacade = new LegacyReportFacade(filters);
            DataSet dataSet = new DataSet();
            string flag = "";
            if (dibrugarhCovidFlag)
            {
                flag = filters["examMode"].ToString().Coalesce();
                if (!"E,P".ToCSV().Contains(flag)) flag = "";
            }

            dataSet = reportFacade.GetPaperCount(scheduleId, flag);
            if (!dataSet.HasData())
                return new HttpResponseMessage(HttpStatusCode.NoContent);

            // Store headers
            // 1
            printLine = instituteName;
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printLine = print.FormatPrintString(printLine, printWidth - 12) + "Page: " + pageNumber;
            printResult = print.PrepareAndPrint(101, 0, printLine);
            if (!printResult) goto endReport;

            // 2
            printLine = "Subject Wise Candidate".ToUpper() + "  " + flag;
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            //printLine = print.FormatPrintString(printLine, printWidth - 12) + "Page: " + pageNumber;
            printResult = print.PrepareAndPrint(102, 0, printLine);

            // 3
            printLine = examName + ", " + (examHeldIn != "" ? examHeldIn : sessionName);
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printLine = printLine.SubstringWithCorrection(0, 79);
            printResult = print.PrepareAndPrint(103, 0, printLine);
            if (!printResult) goto endReport;
            // 3
            printResult = print.PrepareAndPrint(104, 0, "-".RepeatForLeftPadding(printWidth));
            if (!printResult) goto endReport;
            // 4
            printLine = print.FormatPrintString(" Sr", 4)
                    + print.FormatPrintString("Subject", 55)
                    + print.FormatPrintString("Candidates", 11);
            printResult = print.PrepareAndPrint(104, 0, printLine);
            if (!printResult) goto endReport;

            // 5
            printResult = print.PrepareAndPrint(105, 0, "=".RepeatForLeftPadding(printWidth));
            if (!printResult) goto endReport;

            // Print Header
            printResult = print.PrepareAndPrint(2, 0, "");
            if (!printResult) goto endReport;
            printSrNo = 0;
            printTotals[1] = 0;

            // Print Details
            int rowCounter = 0;
            foreach (DataRow dataRow in dataSet.Tables[0].Rows)
            {
                rowCounter++;
                printSrNo = printSrNo + 1;
                printTotals[1] = printTotals[1] + dataRow["Cnt"].ToString().ToInt32();
                printLine = print.FormatPrintString(printSrNo, 4, 3)
                    + print.FormatPrintString(dataRow["StrCode"].ToString() + " "
                    + dataRow["SubCode"].ToString()
                    + (dataRow["HdCode"].ToString() == "" ? "" : "-" + dataRow["HdCode"].ToString()) + " "
                    + dataRow["EsShName"].ToString(), 56, 55)
                    + print.FormatPrintString(dataRow["Cnt"].ToString(), 10, 9)
                    + (!dataRow["EtTime"].ToDateTimeWithCoalesce().IsEmpty() ? dataRow["EtTime"].ToDateTimeWithCoalesce().ToString("dd/mm/yy") : "");

                printResult = print.PrepareAndPrint(3, 1, printLine);

                if (rowCounter == dataSet.Tables[0].Rows.Count)
                {
                    printLine = print.FormatPrintString("", 4)
                        + print.FormatPrintString("Total", 56, 55)
                        + print.FormatPrintString(printTotals[1], 10, 9);
                    printResult = print.PrepareAndPrint(3, 1, printLine);
                }
            }
            gLnI = 0;
            validationMessage = "Printing Over";

        endReport:
            print.PrepareAndPrint(9, gLnI == 1 ? 1 : 0, validationMessage);
            PrintSettings printSettings = new PrintSettings();
            printSettings.PageSize = PdfSharp.PageSize.A4;
            printSettings.PageOrientation = PdfSharp.PageOrientation.Landscape;
            printSettings.LeftMargin = 10;
            printSettings.RightMargin = 10;
            return PrintLineReport(print, printSettings);
        }

        private HttpResponseMessage PrintCollageCenterList(JObject filters)
        {
            //throw new NotImplementedException();
            int printWidth = 80;
            int printSrNo = 0;

            List<double> printTotals = new List<double>() { 0, 0, 0 };
            List<string> printGrps = new List<string>()
            {
                "",
                "",
                ""
            }; //District, Centre, GrandTotal;
            string centColl = (reportCode == 2 ? (isSchool || isInstitute ? "Inst" : "Colg") : "Cent ");
            string printFileName = centColl + "List.txt";
            string reportName = "";
            Print print = new Print();
            int pageNumber = 1;
            string printLine = print.FormatPrintString(reportName + " - " + printWidth, 50) + print.FormatPrintString(_reportPath + printFileName, 50);
            bool printResult = false;
            printResult = print.PrepareAndPrint(0, printWidth, printLine);
            if (!printResult) return new HttpResponseMessage(HttpStatusCode.NoContent);
            LegacyReportFacade reportFacade = new LegacyReportFacade(filters);
            DataSet dataSet = new DataSet();
            string flag = "";
            if (dibrugarhCovidFlag)
            {
                flag = filters["examMode"].ToString().Coalesce();
                if (!"E,P".ToCSV().Contains(flag)) flag = "";
            }
            dataSet = reportFacade.GetCenterCollegeList(scheduleId, (instituteId == 2 ? "Colg" : "Cent") + flag, tnCode);
            if (!dataSet.HasData())
                return new HttpResponseMessage(HttpStatusCode.NoContent);

            // Store headers;
            // 1 
            printLine = instituteName;
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printLine = print.FormatPrintString(printLine, printWidth - 12) + "Page: " + pageNumber;
            printResult = print.PrepareAndPrint(102, 0, printLine);
            if (!printResult) goto endReport;

            // 2
            printLine = (reportCode == 2 ? (isSchool || isInstitute ? "Institute" : "College") : "Centre")
                + " Wise Count of Candidates" + "  " + flag;
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printResult = print.PrepareAndPrint(102, 0, printLine);
            if (!printResult) goto endReport;

            printLine = examName + ", " + (examHeldIn != "" ? examHeldIn : sessionName);
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printResult = print.PrepareAndPrint(103, 0, printLine);
            if (!printResult) goto endReport;

            // 3
            DataRow firstRow = dataSet.FirstRow();
            printGrps[0] = firstRow["TnCode"].ToString();
            printSrNo = 0;
            printTotals[0] = 0;
            printTotals[1] = 0;
            town = "    " + firstRow["TnCode"].ToString() + "-" + firstRow["TnName"].ToString();

            printLine = (pageBreak != "N" || pageBreak == "Y" ? town : ""); // test comment: for Rachana sansad pageBreak = " "
            printResult = print.PrepareAndPrint(104, 1, printLine);
            if (!printResult) goto endReport;

            // 4
            printResult = print.PrepareAndPrint(105, 0, "-".RepeatForLeftPadding(printWidth));
            if (!printResult) goto endReport;

            // 5
            printLine = print.FormatPrintString(" Sr", 4)
                + print.FormatPrintString((reportCode == 2 ? (isSchool || isInstitute ? "Institute" : "College") : "Centre"), 42)
                + (registrationRoll == "G" ? "" : print.FormatPrintString("SeatNos From        To", 23, 22))
                + print.FormatPrintString(" Candidates", 11);
            printResult = print.PrepareAndPrint(106, 0, printLine);
            if (!printResult) goto endReport;

            // 6
            printResult = print.PrepareAndPrint(107, 0, "=".RepeatForLeftPadding(printWidth));
            if (!printResult) goto endReport;

            //Print Header;
            printResult = print.PrepareAndPrint(2, 0, "");
            if (!printResult) goto endReport;
            if (pageBreak == "N" && pageBreak != "Y")
                printResult = print.PrepareAndPrint(3, 1, town);

            // Print Details
            int rowCounter = 0;
            foreach (DataRow dataRow in dataSet.Tables[0].Rows)
            {
                rowCounter++;
                if (printGrps[0] != dataRow["TnCode"].ToString())
                {
                    printGrps[0] = dataRow["TnCode"].ToString();
                    printSrNo = 0;
                    printTotals[1] = 0;
                    town = "    " + dataRow["TnCode"].ToString() + "-" + dataRow["TnName"].ToString();
                    if (pageBreak == "Y")
                    {
                        printResult = print.PrepareAndPrint(104, 1, town);
                        if (!printResult) goto endReport;
                        printResult = print.PrepareAndPrint(2, 99, "");
                        if (!printResult) goto endReport;
                    }
                    else
                        printResult = print.PrepareAndPrint(3, 1, town);
                }

                printSrNo = printSrNo + 1;
                printLine = print.FormatPrintString(printSrNo, 4, 3)
                        + print.FormatPrintString(dataRow["Grp"].ToString() + " " + dataRow["GrpName"].ToString(), 45, 44)
                        + (registrationRoll == "G" ? "" : print.FormatPrintString(dataRow["FrRoll"].ToString(), 10, 9)
                        + print.FormatPrintString(dataRow["ToRoll"].ToString(), 10, 9))
                        + print.FormatPrintString(dataRow["Cnt"].ToString(), 11, 10);
                printResult = print.PrepareAndPrint(3, 0, printLine);
                printTotals[1] = printTotals[1] + dataRow["Cnt"].ToString().ToInt32();

                if (dataRow["GrpName"].ToString().Length > 60)
                {
                    printLine = print.FormatPrintString("", 10) + dataRow["GrpName"].ToString().SubstringWithCorrection(61, 60);
                    printResult = print.PrepareAndPrint(3, 0, printLine);
                }

                printTotalCounter = 0;
                if (rowCounter == dataSet.Tables[0].Rows.Count)
                    printTotalCounter = 1;
                else
                {
                    DataRow nextDataRow = dataSet.Tables[0].Rows[rowCounter];
                    if (printGrps[0] != nextDataRow["TnCode"].ToString())
                        printTotalCounter = 2;
                }

                if (printTotalCounter != 0)
                {
                    printLine = print.FormatPrintString(" ", 4) + "Total for " + town + ": " + printTotals[1].ToString();
                    printResult = print.PrepareAndPrint(3, 1, printLine);
                    printTotals[0] = printTotals[0] + printTotals[1];
                }
                if (printTotalCounter == 1)
                {
                    printLine = print.FormatPrintString(" ", 4) + "Grand Total: " + printTotals[0].ToString();
                    printResult = print.PrepareAndPrint(3, 1, printLine);
                }
            }

            gLnI = 0;
            validationMessage = "Printing Over";

        endReport:
            print.PrepareAndPrint(9, gLnI == 1 ? 1 : 0, validationMessage);
            PrintSettings printSettings = new PrintSettings();
            printSettings.PageSize = PdfSharp.PageSize.A4;
            printSettings.PageOrientation = PdfSharp.PageOrientation.Landscape;
            return PrintLineReport(print, printSettings);
        }

        private HttpResponseMessage PrintCenterSchoolList(JObject filters)
        {
            //throw new NotImplementedException();
            int printWidth = 80;
            int printSrNo = 0;

            List<double> printTotals = new List<double>() { 0, 0, 0 };
            List<string> printGrps = new List<string>()
            {
                "",
                "",
                ""
            }; //District, Centre, GrandTotal;
            string centColl = (reportCode == 2 ? "Colg" : "Cent ");
            string printFileName = centColl + "List.txt";
            string reportName = "";
            Print print = new Print();
            int pageNumber = 1;
            string printLine = print.FormatPrintString(reportName + " - " + printWidth, 50) + print.FormatPrintString(_reportPath + printFileName, 50);
            bool printResult = false;
            printResult = print.PrepareAndPrint(0, printWidth, printLine);
            if (!printResult) return new HttpResponseMessage(HttpStatusCode.NoContent);
            LegacyReportFacade reportFacade = new LegacyReportFacade(filters);
            DataSet dataSet = new DataSet();
            dataSet = reportFacade.GetCenterSchoolList(scheduleId, centColl, center, dsCode);
            if (!dataSet.HasData())
                return new HttpResponseMessage(HttpStatusCode.NoContent);
            //Store headers
            //1
            printLine = instituteName;
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printResult = print.PrepareAndPrint(101, 0, printLine);
            if (!printResult) goto endReport;
            //2
            printLine = reportName;
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printLine = print.FormatPrintString(printLine, printWidth - 12) + "Page: " + pageNumber;
            printResult = print.PrepareAndPrint(102, 0, printLine);
            if (!printResult) goto endReport;

            //3;
            printLine = examName + ", " + sessionYear;
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printResult = print.PrepareAndPrint(103, 0, printLine);

            //4
            DataRow firstRow = dataSet.FirstRow();
            printGrps[0] = firstRow["DsCode"].ToString();
            printGrps[1] = firstRow["Cent"].ToString();

            printLine = "District: " + firstRow["DsCode"].ToString() + " " + firstRow["DsName"].ToString();
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printResult = print.PrepareAndPrint(104, 0, printLine);
            if (!printResult) goto endReport;

            center = "Centre: " + firstRow["Cent"] + " " + firstRow["CentName"];

            //4
            printResult = print.PrepareAndPrint(105, 0, "-".RepeatForLeftPadding(printWidth));
            if (!printResult) goto endReport;

            //5
            printLine = print.FormatPrintString(" Sr", 4)
                + print.FormatPrintString((reportCode == 2 ? "Institute" : "Centre"), 70, 69)
                + print.FormatPrintString(" Count", 6, 6);
            printResult = print.PrepareAndPrint(106, 0, printLine);
            if (!printResult) goto endReport;

            //6;
            printResult = print.PrepareAndPrint(107, 0, "=".RepeatForLeftPadding(printWidth));
            if (!printResult) goto endReport;

            //Print Header
            printResult = print.PrepareAndPrint(2, 0, "");
            if (!printResult) goto endReport;
            printSrNo = 0;
            printTotals[0] = 0;
            printTotals[1] = 0;
            printTotals[2] = 0;
            if (printGrps[1] != "")
                printResult = print.PrepareAndPrint(3, 0, center);

            //Print Details
            int rowCounter = 0;
            foreach (DataRow dataRow in dataSet.Tables[0].Rows)
            {
                rowCounter++;
                if (printGrps[0] != dataRow["DsCode"].ToString())
                {
                    printGrps[0] = dataRow["DsCode"].ToString();
                    //
                    printLine = "District: " + dataRow["DsCode"].ToString() + " " + dataRow["DsName"].ToString();
                    printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
                    printResult = print.PrepareAndPrint(104, 0, printLine);
                    if (!printResult) goto endReport;
                    //
                    printResult = print.PrepareAndPrint(2, 99, "");
                    if (!printResult) goto endReport;
                    //;
                    printTotals[0] = 0;
                    printSrNo = 0;
                    printTotals[1] = 0;
                }

                if (printGrps[1] != dataRow["Cent"].ToString())
                {
                    printGrps[1] = dataRow["Cent"].ToString();
                    printLine = "Centre: " + dataRow["Cent"].ToString() + " " + dataRow["CentName"].ToString();
                    printResult = print.PrepareAndPrint(3, 1, printLine);
                    printSrNo = 0;
                    printTotals[1] = 0;
                }

                printSrNo = printSrNo + 1;
                printLine = print.FormatPrintString(printSrNo, 4, 3)
                    + print.FormatPrintString(dataRow["Grp"].ToString(), 6, 5)
                    + print.FormatPrintString(dataRow["GrpName"].ToString(), 61, 60)
                    + print.FormatPrintString(dataRow["Cnt"].ToString(), 6, 6);
                printResult = print.PrepareAndPrint(3, 0, printLine);
                printTotals[0] = printTotals[0] + dataRow["Cnt"].ToString().ToInt32();

                printTotalCounter = 0;
                if (rowCounter == dataSet.Tables[0].Rows.Count)
                    printTotalCounter = 1;
                else if (printGrps[0] != dataRow["DsCode"].ToString())
                    printTotalCounter = 2;
                else if (printGrps[1] != dataRow["Cent"].ToString())
                    printTotalCounter = 3;

                if (printTotalCounter != 0)
                {
                    printLine = print.FormatPrintString(" ", 10) + print.FormatPrintString("Total Count ", 61) + print.FormatPrintString(printTotals[0], 6, 6);
                    printResult = print.PrepareAndPrint(3, 0, printLine);
                    printTotals[0] = printTotals[0] + printTotals[1];
                    printTotals[2] = printTotals[2] + printTotals[1];
                }

                if ((printTotalCounter == 1 || printTotalCounter == 2) && printGrps[1] != "")
                {
                    printLine = print.FormatPrintString(" ", 10) + print.FormatPrintString("District Total", 61) + print.FormatPrintString(printTotals[0], 6, 6);
                    printResult = print.PrepareAndPrint(3, 1, printLine);
                }

                if (printTotalCounter == 1 && dsCode.Trim() == "" && center.Trim() == "")
                {
                    printLine = print.FormatPrintString(" ", 10) + print.FormatPrintString("Grand Total", 61) + print.FormatPrintString(printTotals[2], 6, 6);
                    printResult = print.PrepareAndPrint(3, 1, printLine);
                }
            }
            gLnI = 0;
            validationMessage = "Printing Over";

        endReport:
            print.PrepareAndPrint(9, gLnI == 1 ? 1 : 0, validationMessage);
            PrintSettings printSettings = new PrintSettings();
            printSettings.PageSize = PdfSharp.PageSize.A4;
            printSettings.PageOrientation = PdfSharp.PageOrientation.Landscape;
            return PrintLineReport(print, printSettings);
        }

        private HttpResponseMessage NoInfo(JObject filters)
        {
            //throw new Exception("Testing Pening");


            int cols;
            string detailLine = "";
            uc = uc.ToUpper();
            if (uc == "C")
            {
                return NoInfo_College();
            }
            int printWidth = 80;
            int printSrNo = 0;

            List<double> printTotals = new List<double>() { 0, 0, 0 };
            List<string> printGrps = new List<string>() { "", "", "" }; //Sub, Cent/Colg


            string printFileName = "NoInfo.txt";
            string reportName = "No Info";
            Print print = new Print();
            string printLine = print.FormatPrintString(reportName + " - " + printWidth, 50) + print.FormatPrintString(_reportPath + printFileName, 50);
            bool printResult = false;
            printResult = print.PrepareAndPrint(0, printWidth, printLine);
            if (!printResult) return new HttpResponseMessage(HttpStatusCode.NoContent);

            LegacyReportFacade reportFacade = new LegacyReportFacade(filters);
            DataSet dataSet = new DataSet();
            dataSet = reportFacade.NoMarks(scheduleId, streamId, center, dsCode, uc);
            if (!dataSet.HasData())
                return new HttpResponseMessage(HttpStatusCode.NoContent);

            // 1 - Store Headers
            printLine = instituteName;
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printLine = print.FormatPrintString(printLine, printWidth - 12) + DateTime.Now.ToString(dateFormat);
            printResult = print.PrepareAndPrint(101, 0, printLine);
            if (!printResult) goto endReport;

            // 2 - 
            printLine = reportName + " (" + (uc == "U" ? "University" : "College") + " Marks)";
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printLine = print.FormatPrintString(printLine, printWidth - 12) + "Page: " + Print._page;
            printResult = print.PrepareAndPrint(102, 0, printLine);
            if (!printResult) goto endReport;

            // 3 - 
            printLine = (examName + " - " + (examHeldIn.IsNullOrWhiteSpace() ? sessionName : examHeldIn)); // .SubstringWithCorrection(0, 79) removed the mentioned code and commented for "impact analysis".
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printLine = print.FormatPrintString(printLine, printWidth - 12) + "Page: " + Print._page;
            printResult = print.PrepareAndPrint(103, 0, printLine);
            if (!printResult) goto endReport;

            // 5 -
            if (instituteId == 11)
            {
                printGrps[1] = "";
                printGrps[2] = "";
            }
            else
            {
                DataRow dataRow = dataSet.FirstRow();
                printGrps[1] = dataRow["Sub"].ToString();
                printGrps[2] = dataRow["EfCent"].ToString();
                printLine = "Sub: " + dataRow["StrCode"].Coalesce() + " "
                    + dataRow["Sub"].Coalesce() + " "
                    + dataRow["SubName"].Coalesce() + " "
                    + (dataRow["EsMarkGrd"].Coalesce().ToUpper() == "G" ? "(Grade)" : "");
            }
            //printLine = printLine.SubstringWithCorrection(0, 79);
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            printResult = print.PrepareAndPrint(104, 1, printLine);
            if (!printResult) goto endReport;

            // 4 - 
            printResult = print.PrepareAndPrint(105, 0, "_".RepeatForLeftPadding(printWidth));
            if (!printResult) goto endReport;

            // 5 - 
            if (registrationRoll.ToUpper() == "G")
            {
                cols = 4;
                printLine = (print.FormatPrintString("RegnNo", 13, 12) + print.FormatPrintString("Mark", 5, 4)).RepeatForLeftPadding(4);
            }
            else
            {
                cols = 5;
                string tempString = (instituteId == 13 ? "   SeatNo" : "   RollNo");
                printLine = (print.FormatPrintString(tempString, 11, 10) + print.FormatPrintString("Mark", 5, 4)).RepeatForLeftPadding(4);
            }
            printResult = print.PrepareAndPrint(106, 0, printLine);
            if (!printResult) goto endReport;

            // 6 - 
            printResult = print.PrepareAndPrint(107, 0, "=".RepeatForLeftPadding(printWidth));
            if (!printResult) goto endReport;

            // Print Header
            printResult = print.PrepareAndPrint(2, 0, "");
            if (!printResult) goto endReport;
            printTotals[0] = 0;
            printTotals[1] = 0;
            printSrNo = 0;
            detailLine = "";

            int recordCount = dataSet.Tables[0].Rows.Count;
            for (int rowCounter = 0; rowCounter < recordCount; rowCounter++)
            {
                DataRow dr = dataSet.Tables[0].Rows[rowCounter];

                if (printGrps[1] != dr["Sub"].Coalesce())
                {
                    printGrps[1] = dr["Sub"].Coalesce();
                    printGrps[2] = dr["EfCent"].Coalesce();
                    printLine = "Sub: " + dr["StrCode"].Coalesce() + " "
                        + dr["Sub"].Coalesce() + " "
                        + dr["SubName"].Coalesce() + " "
                        + (dr["EsMarkGrd"].Coalesce().ToUpper() == "G" ? "(Grade)" : "");
                    printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
                    if (instituteId == 11)
                        printResult = print.PrepareAndPrint(3, 1, printLine);
                    else
                    {
                        printResult = print.PrepareAndPrint(104, 1, printLine);
                        printResult = print.PrepareAndPrint(2, 99, "");
                    }
                    srNo = 0;
                    printTotals[1] = 0;
                    detailLine = "";
                }
                printSrNo++;
                if (registrationRoll.ToUpper() == "G")
                    detailLine += print.FormatPrintString(dr["RgNo"].Coalesce(), 13, 12) + print.FormatPrintString("", 5);
                else
                    detailLine += print.FormatPrintString(rollNoPrefix + dr["EfRollNo"].Coalesce(), 11, 10) + print.FormatPrintString("", 5);

                bool isEOF = rowCounter == recordCount - 1;

                if (rowCounter == recordCount - 1)
                    printTotalCounter = 1;
                else if (!isEOF && printGrps[1] != dataSet.Tables[0].Rows[rowCounter + 1]["Sub"].Coalesce())
                    printTotalCounter = 2;
                else if (!isEOF && printGrps[2] != dr["EfCent"].Coalesce())
                    printTotalCounter = 3;
                else if (printSrNo % cols == 0)
                    printTotalCounter = 4;


                if (printTotalCounter != 0)
                {
                    printResult = print.PrepareAndPrint(3, 1, detailLine);
                    if (printTotalCounter < 4)
                    {
                        string tempString = (instituteId == 13 ? "Venue  " : "Centre ") + printGrps[2] + ": ";
                        if (uc.ToUpper() == "C")
                            tempString = "College " + printGrps[2] + ": ";
                        printLine = print.FormatPrintString("", 2) + print.FormatPrintString(tempString, 20) + print.FormatPrintString(printSrNo, 5);
                        printResult = print.PrepareAndPrint(3, 1, printLine);
                        printTotals[1] = printTotals[1] + printSrNo;
                        printSrNo = 0;
                    }
                    if (printTotalCounter == 3)
                        printGrps[1] = dr["EfCent"].Coalesce();
                    if (printTotalCounter < 3)
                    {
                        printLine = print.FormatPrintString("", 2) + print.FormatPrintString("Subject Total: ", 20)
                            + print.FormatPrintString(printTotals[1], 5);
                        printResult = print.PrepareAndPrint(3, 1, printLine);
                        printTotals[0] = printTotals[0] + printTotals[1];
                    }
                    if (printTotalCounter < 2)
                    {
                        printLine = print.FormatPrintString("", 2) + print.FormatPrintString("Grand Total: ", 20) + print.FormatPrintString(printTotals[0], 5);
                        printResult = print.PrepareAndPrint(3, 1, printLine);
                    }
                    detailLine = "";
                }
            }
            printTotalCounter = 1;
            gLnI = 0;
            validationMessage = "Printing Over";
        endReport:
            print.PrepareAndPrint(9, gLnI == 1 ? 1 : 0, validationMessage);

            PrintSettings printSettings = new PrintSettings();
            printSettings.PageSize = PdfSharp.PageSize.A4;
            printSettings.PageOrientation = PdfSharp.PageOrientation.Landscape;
            return PrintLineReport(print, printSettings);
        }
        private HttpResponseMessage NoInfo_College()
        {
            throw new Exception("Testing Pening");
        }

        public HttpResponseMessage NoContent()
        {
            Print print = new Print();
            int printWidth = 80;
            string printLine = "No Data Found";
            printLine = print.FormatPrintString("", (printWidth - printLine.Length) / 2) + printLine;
            print.PrepareAndPrint(0, 0, "");
            print.PrepareAndPrint(3, 0, printLine);

            PrintSettings printSettings = new PrintSettings();
            printSettings.PageSize = PdfSharp.PageSize.A4;
            printSettings.PageOrientation = PdfSharp.PageOrientation.Landscape;
            return PrintLineReport(print, printSettings);
        }

        private HttpResponseMessage RenderDocxReport(JObject filters)
        {
            string reportFileName = "";
            string exportFileName = "Report.pdf";
            string spName = "";

            switch (reportCode)
            {
                //case 301:
                //    spName = "usp_TranscriptReport";
                //    reportFileName = "TRANSCRIPT.docx";
                //    break;
                case 302:
                    spName = "usp_Report_TentativeSchedules";
                    reportFileName = "Tentative_Timetable_Combined.docx";
                    filters.Add("IsReportRequest", "1");
                    break;

                    //Change For mehul reports 
                    //case 303:
                    //    spName = "usp_GetStudentCertificateDetails";
                    //    reportFileName = "Provisional_Passing_Certificate_Template.docx";
                    //    break;
                    //case 304:
                    //    spName = "usp_GetStudentCertificateDetails";
                    //    reportFileName = "Rank_Certificate_Template.docx";
                    //    break;
                    //case 305:
                    //    spName = "usp_GetStudentCertificateDetails";
                    //    reportFileName = "OriginalPassingCertificate.docx";
                    //    break;
                    //case 306:
                    //    spName = "usp_GetStudentCertificateDetails";
                    //    reportFileName = "MigrationCertificate.docx";
                    //    break;
            }

            try
            {
                if (spName.IsNullOrWhiteSpace())
                {
                    DataTable spData = ExecuteReportQuery("usp_GetReportSP", filters).FirstTable();
                    if (spData.HasData())
                    {
                        spName = spData.FirstRow()["SPName"].ToString();
                        reportFileName = spData.FirstRow()["TemplateName"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                log.Error(ex.StackTrace, ex);
            }

            JObject payload = GetDocXPayload(spName, filters);

            JObject formBody = new JObject();
            formBody.Add("filters", filters);
            formBody.Add("reportFileName", reportFileName);
            formBody.Add("exportFilename", exportFileName);
            formBody.Add("payload", payload);

            var content = new StringContent(formBody.ToString(), System.Text.Encoding.UTF8, "application/json");

            HttpClient _httpClient = new HttpClient();
            var apiUrl = WebSettings.ReadKey("DocXAPIEndpoint");
            HttpResponseMessage response = _httpClient.PostAsync(apiUrl, content).GetAwaiter().GetResult();

            //return response;

            // Create a new response to exclude headers
            var cleanedResponse = new HttpResponseMessage
            {
                StatusCode = response.StatusCode,
                Content = response.Content // Retain the content
            };

            // Delete Temp files
            try
            {
                if (payload["PDFFiles"] != null)
                {
                    JArray tempFiles = (JArray)payload["PDFFiles"];
                    foreach (string tempFile in tempFiles)
                    {
                        if (File.Exists(tempFile))
                        {
                            try
                            {
                                File.Delete(tempFile);
                            }
                            catch (Exception err1) { }
                        }
                    }
                }
            }
            catch (Exception err) { }

            return cleanedResponse;
        }

        private JObject GetDocXPayload(string spName, JObject filters)
        {
            JObject payload = new JObject();
            DataTable dataTable = new DataTable();
            switch (reportCode)
            {
                case 301:
                    dataTable = ExecuteReportQuery(spName, filters).FirstTable();
                    payload = dataTable.FirstRow().ToJObject();

                    JArray pdfFilesForMerging = new JArray();
                    string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TempPDFFiles");
                    if (!Directory.Exists(folderPath))
                        Directory.CreateDirectory(folderPath);

                    string filePrefix = $"TC_{studentId.ToString().PadLeft(10, '0')}_";
                    filters.Add("CertType", "TS");
                    pdfFilesForMerging = ExecuteReportQuery("sp_GetTclcDocs", filters, true).FirstTable().SaveBase64ToFiles("FileUpload", SaveFileType.PDF, folderPath, filePrefix);
                    payload.Add("PDFFiles", pdfFilesForMerging);

                    break;
                case 302:
                default:
                    payload = ExecuteReportQuery(spName, filters).ToJObject(true);
                    break;
            }

            return payload;
        }

        private HttpResponseMessage RenderCSVReports(JObject filters)
        {
            JObject json = null;
            try
            {
                json = JObject.Parse(extra1);
            }
            catch { }
            if (json != null)
            {
                foreach (var property in json)
                {
                    filters.Add(property.Key, property.Value);
                }
            }

            try
            {
                DataTable spData = ExecuteReportQuery("usp_GetReportSP", filters).FirstTable();
                if (spData.HasData())
                {
                    string spName = spData.FirstRow()["SPName"].ToString();
                    DataTable dataTable = ExecuteReportQuery(spName, filters, true).FirstTable();
                    if (dataTable.HasData())
                        return dataTable.ToCSV_InHttpResponse(true, ",", false, true);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                log.Error(ex.StackTrace, ex);
            }
            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }
    }
}