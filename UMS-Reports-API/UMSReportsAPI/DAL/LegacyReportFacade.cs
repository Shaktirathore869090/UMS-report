using Antlr.Runtime;
using CrystalDecisions.ReportAppServer.CommonControls;
using CrystalDecisions.ReportAppServer.Prompting;
using log4net.Repository.Hierarchy;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using UMSReportsAPI.Extenders;

using log4net;
using static System.Net.Mime.MediaTypeNames;
using Newtonsoft.Json.Linq;
using PdfSharp;
using CrystalDecisions.ReportSource;


namespace UMSReportsAPI.DAL
{
    public class LegacyReportFacade
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(LegacyReportFacade));

        private string _connectionString;
        public LegacyReportFacade(JObject filters)
        {
            _connectionString = WebSettings.ConnectionString;
            try
            {
                if (filters != null)
                {

                    string dbName = filters["DBName"].Coalesce();
                    if (!dbName.IsNullOrWhiteSpace())
                        _connectionString = _connectionString.Replace(WebSettings.DBName, dbName);
                }
            }
            catch { }
        }

        public void GetReportParameters(int loginId, ref int reportCode, ref string reportType, ref string reportName,
            ref string reportStoredProcedure, ref string reportImage, ref string instituteName, ref int instituteId,
            ref string registrationRoll, ref int scheduleId, ref string collegeCode, ref int rollNo, ref string center,
            ref string tnCode, ref int esId, ref int csNo, ref string pageBreak, ref int streamId, ref string streamCode,
            ref string registartionNo, ref string uc, ref string ledger, ref string examCode, ref string examName,
            ref string examShortName, ref string examMarkList, ref string examEvaluation, ref string imagePath, ref int medium,
            ref string resultDate, ref string examHeldIn, ref string examNo, ref string authorityName, ref string authorityDesignation,
            ref string dsCode, ref string courseCode, ref int sessionNo, ref string extra1, ref string examCourseCode, ref string examCourseName
            , ref string institutePlace, ref string instituteCode)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@LoginId", SqlDbType.Int) { Value = loginId });

                parameters.Add(new SqlParameter("@RptCode", SqlDbType.SmallInt) { Direction = ParameterDirection.Output });
                parameters.Add(new SqlParameter("@RptType", SqlDbType.Char, 1) { Direction = ParameterDirection.Output });
                parameters.Add(new SqlParameter("@RptName", SqlDbType.VarChar, 50) { Direction = ParameterDirection.Output });
                parameters.Add(new SqlParameter("@RptProc", SqlDbType.VarChar, 30) { Direction = ParameterDirection.Output });
                parameters.Add(new SqlParameter("@RptImg", SqlDbType.VarChar, 5) { Direction = ParameterDirection.Output });
                parameters.Add(new SqlParameter("@InstName", SqlDbType.VarChar, 100) { Direction = ParameterDirection.Output });
                parameters.Add(new SqlParameter("@InstNo", SqlDbType.SmallInt) { Direction = ParameterDirection.Output });
                parameters.Add(new SqlParameter("@RegnRoll", SqlDbType.Char, 1) { Direction = ParameterDirection.Output });
                parameters.Add(new SqlParameter("@ShId", SqlDbType.Int) { Direction = ParameterDirection.Output });
                parameters.Add(new SqlParameter("@CgCode", SqlDbType.VarChar, 10) { Direction = ParameterDirection.Output });
                parameters.Add(new SqlParameter("@RollNo", SqlDbType.Int) { Direction = ParameterDirection.Output });
                parameters.Add(new SqlParameter("@Cent", SqlDbType.VarChar, 10) { Direction = ParameterDirection.Output });
                parameters.Add(new SqlParameter("@TnCode", SqlDbType.VarChar, 10) { Direction = ParameterDirection.Output });
                parameters.Add(new SqlParameter("@EsId", SqlDbType.Int) { Direction = ParameterDirection.Output });
                parameters.Add(new SqlParameter("@CsNo", SqlDbType.Int) { Direction = ParameterDirection.Output });
                parameters.Add(new SqlParameter("@PgBreak", SqlDbType.Char, 1) { Direction = ParameterDirection.Output });
                parameters.Add(new SqlParameter("@StrId", SqlDbType.Int) { Direction = ParameterDirection.Output });
                parameters.Add(new SqlParameter("@StrCode", SqlDbType.VarChar, 5) { Direction = ParameterDirection.Output });
                parameters.Add(new SqlParameter("@RgNo", SqlDbType.VarChar, 20) { Direction = ParameterDirection.Output });
                parameters.Add(new SqlParameter("@UC", SqlDbType.Char, 1) { Direction = ParameterDirection.Output });
                parameters.Add(new SqlParameter("@Ledger", SqlDbType.Char, 1) { Direction = ParameterDirection.Output });
                parameters.Add(new SqlParameter("@EmCode", SqlDbType.VarChar, 6) { Direction = ParameterDirection.Output });
                parameters.Add(new SqlParameter("@EmName", SqlDbType.VarChar, 80) { Direction = ParameterDirection.Output });
                parameters.Add(new SqlParameter("@EmShName", SqlDbType.VarChar, 20) { Direction = ParameterDirection.Output });
                parameters.Add(new SqlParameter("@EmMarkList", SqlDbType.VarChar, 5) { Direction = ParameterDirection.Output });
                parameters.Add(new SqlParameter("@EmEval", SqlDbType.VarChar, 5) { Direction = ParameterDirection.Output });
                parameters.Add(new SqlParameter("@ShPath", SqlDbType.VarChar, 100) { Direction = ParameterDirection.Output });
                parameters.Add(new SqlParameter("@Mediums", SqlDbType.SmallInt) { Direction = ParameterDirection.Output });
                parameters.Add(new SqlParameter("@ResultDate", SqlDbType.VarChar, 200) { Direction = ParameterDirection.Output });
                parameters.Add(new SqlParameter("@HeldIn", SqlDbType.VarChar, 200) { Direction = ParameterDirection.Output });
                parameters.Add(new SqlParameter("@ExNo", SqlDbType.VarChar, 200) { Direction = ParameterDirection.Output });
                parameters.Add(new SqlParameter("@AuthName", SqlDbType.VarChar, 200) { Direction = ParameterDirection.Output });
                parameters.Add(new SqlParameter("@AuthDsgn", SqlDbType.VarChar, 200) { Direction = ParameterDirection.Output });
                parameters.Add(new SqlParameter("@DsCode", SqlDbType.VarChar, 5) { Direction = ParameterDirection.Output });
                parameters.Add(new SqlParameter("@CoCode", SqlDbType.VarChar, 10) { Direction = ParameterDirection.Output });
                parameters.Add(new SqlParameter("@SnNo", SqlDbType.SmallInt) { Direction = ParameterDirection.Output });
                parameters.Add(new SqlParameter("@Extra1", SqlDbType.VarChar, int.MaxValue) { Direction = ParameterDirection.Output });
                parameters.Add(new SqlParameter("@EmCoCode", SqlDbType.VarChar, 10) { Direction = ParameterDirection.Output });
                parameters.Add(new SqlParameter("@EmCoName", SqlDbType.VarChar, 80) { Direction = ParameterDirection.Output });
                parameters.Add(new SqlParameter("@InstPlace", SqlDbType.NVarChar, 100) { Direction = ParameterDirection.Output });
                parameters.Add(new SqlParameter("@InstituteCode", SqlDbType.VarChar, 50) { Direction = ParameterDirection.Output });


                ExecuteStoredProcedure("sp_getReportParam", parameters);

                foreach (SqlParameter parameter in parameters)
                {
                    try
                    {
                        switch (parameter.ParameterName)
                        {
                            case "@RptCode":
                                reportCode = parameter.Value.ToString().ToInt32();
                                break;
                            case "@RptType":
                                reportType = parameter.Value.ToString();
                                break;
                            case "@RptName":
                                reportName = parameter.Value.ToString();
                                break;
                            case "@RptProc":
                                reportStoredProcedure = parameter.Value.ToString();
                                break;
                            case "@RptImg":
                                reportImage = parameter.Value.ToString();
                                break;
                            case "@InstName":
                                instituteName = parameter.Value.ToString();
                                break;
                            case "@InstNo":
                                instituteId = parameter.Value.ToString().ToInt32();
                                break;
                            case "@RegnRoll":
                                registrationRoll = parameter.Value.ToString();
                                break;
                            case "@ShId":
                                scheduleId = parameter.Value.ToString().ToInt32();
                                break;
                            case "@CgCode":
                                collegeCode = parameter.Value.ToString();
                                break;
                            case "@RollNo":
                                rollNo = parameter.Value.ToString().ToInt32();
                                break;
                            case "@Cent":
                                center = parameter.Value.ToString();
                                break;
                            case "@TnCode":
                                tnCode = parameter.Value.ToString();
                                break;
                            case "@EsId":
                                esId = parameter.Value.ToString().ToInt32();
                                break;
                            case "@CsNo":
                                csNo = parameter.Value.ToString().ToInt32();
                                break;
                            case "@PgBreak":
                                pageBreak = parameter.Value.ToString();
                                break;
                            case "@StrId":
                                streamId = parameter.Value.ToString().ToInt32();
                                break;
                            case "@StrCode":
                                streamCode = parameter.Value.ToString();
                                break;
                            case "@RgNo":
                                registartionNo = parameter.Value.ToString();
                                break;
                            case "@UC":
                                uc = parameter.Value.ToString();
                                break;
                            case "@Ledger":
                                ledger = parameter.Value.ToString();
                                break;
                            case "@EmCode":
                                examCode = parameter.Value.ToString();
                                break;
                            case "@EmName":
                                examName = parameter.Value.ToString();
                                break;
                            case "@EmShName":
                                examShortName = parameter.Value.ToString();
                                break;
                            case "@EmMarkList":
                                examMarkList = parameter.Value.ToString();
                                break;
                            case "@EmEval":
                                examEvaluation = parameter.Value.ToString();
                                break;
                            case "@ShPath":
                                imagePath = parameter.Value.ToString();
                                break;
                            case "@Mediums":
                                medium = parameter.Value.ToString().ToInt32();
                                break;
                            case "@ResultDate":
                                resultDate = parameter.Value.ToString();
                                break;
                            case "@HeldIn":
                                examHeldIn = parameter.Value.ToString();
                                break;
                            case "@ExNo":
                                examNo = parameter.Value.ToString();
                                break;
                            case "@AuthName":
                                authorityName = parameter.Value.ToString();
                                break;
                            case "@AuthDsgn":
                                authorityDesignation = parameter.Value.ToString();
                                break;
                            case "@DsCode":
                                dsCode = parameter.Value.ToString();
                                break;
                            case "@CoCode":
                                courseCode = parameter.Value.ToString();
                                break;
                            case "@SnNo":
                                sessionNo = parameter.Value.ToString().ToInt32();
                                break;
                            case "@Extra1":
                                extra1 = parameter.Value.ToString();
                                break;
                            case "@EmCoCode":
                                examCourseCode = parameter.Value.ToString();
                                break;
                            case "@EmCoName":
                                examCourseName = parameter.Value.ToString();
                                break;
                            case "@InstPlace":
                                institutePlace = parameter.Value.ToString();
                                break;
                            case "@InstituteCode":
                                instituteCode = parameter.Value.ToString();
                                break;
                        }
                    }
                    catch (Exception error)
                    {
                        log.Error(error.Message, error);
                        log.Error(error.StackTrace, error);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                log.Error(ex.StackTrace, ex);
            }
        }

        public void InitializeSystemParameters(int loginId, int sessionNo, int streamId, ref DateTime systemDate
                , ref string connectionCode, ref string loginName, ref string loginAccess, ref int loginLevel, ref string sessionYear
                , ref string sessionCode, ref string sessionName, ref string sessionStatus, ref string examFormShow, ref string name, ref string towns)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@LoginId", SqlDbType.Int) { Value = loginId });
                parameters.Add(new SqlParameter("@SnNo", SqlDbType.SmallInt) { Value = sessionNo });
                parameters.Add(new SqlParameter("@StrId", SqlDbType.Int) { Value = streamId });

                parameters.Add(new SqlParameter("@SysDate", SqlDbType.DateTime) { Direction = ParameterDirection.Output });
                parameters.Add(new SqlParameter("@CnCode", SqlDbType.VarChar, 25) { Direction = ParameterDirection.Output });
                parameters.Add(new SqlParameter("@LoginName", SqlDbType.VarChar, 10) { Direction = ParameterDirection.Output });
                parameters.Add(new SqlParameter("@LoginAccess", SqlDbType.VarChar, 8000) { Direction = ParameterDirection.Output });
                parameters.Add(new SqlParameter("@LoginLevel", SqlDbType.SmallInt) { Direction = ParameterDirection.Output });
                parameters.Add(new SqlParameter("@SnYear", SqlDbType.Char, 4) { Direction = ParameterDirection.Output });
                parameters.Add(new SqlParameter("@SnCode", SqlDbType.VarChar, 15) { Direction = ParameterDirection.Output });
                parameters.Add(new SqlParameter("@SnName", SqlDbType.VarChar, 20) { Direction = ParameterDirection.Output });
                parameters.Add(new SqlParameter("@SnStatus", SqlDbType.Char, 1) { Direction = ParameterDirection.Output });
                parameters.Add(new SqlParameter("@EfShow", SqlDbType.VarChar, 10) { Direction = ParameterDirection.Output });
                parameters.Add(new SqlParameter("@StrName", SqlDbType.VarChar, 60) { Direction = ParameterDirection.Output });
                parameters.Add(new SqlParameter("@StrTowns", SqlDbType.VarChar, 20) { Direction = ParameterDirection.Output });

                ExecuteStoredProcedure("sy_Initialise", parameters);

                foreach (SqlParameter parameter in parameters)
                {
                    try
                    {
                        switch (parameter.ParameterName)
                        {
                            case "@SysDate":
                                systemDate = parameter.Value.ToString().ToDateTimeWithCoalesce();
                                break;
                            case "@CnCode":
                                connectionCode = parameter.Value.ToString();
                                break;
                            case "@LoginName":
                                loginName = parameter.Value.ToString();
                                break;
                            case "@LoginAccess":
                                loginAccess = parameter.Value.ToString();
                                break;
                            case "@LoginLevel":
                                loginLevel = parameter.Value.ToString().ToInt32();
                                break;
                            case "@SnYear":
                                sessionYear = parameter.Value.ToString();
                                break;
                            case "@SnCode":
                                sessionCode = parameter.Value.ToString();
                                break;
                            case "@SnName":
                                sessionName = parameter.Value.ToString();
                                break;
                            case "@SnStatus":
                                sessionStatus = parameter.Value.ToString();
                                break;
                            case "@EfShow":
                                examFormShow = parameter.Value.ToString();
                                break;
                            case "@StrName":
                                name = parameter.Value.ToString();
                                break;
                            case "@StrTowns":
                                towns = parameter.Value.ToString();
                                break;
                        }
                    }
                    catch (Exception error)
                    {
                        log.Error(error.Message, error);
                        log.Error(error.StackTrace, error);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                log.Error(ex.StackTrace, ex);
            }
        }

        public void InitializeMiscParameters(string courseCode, int scheduleId, int instituteId, int streamId, ref string coursePattern, ref int scheduleExamId, ref string resultDate, ref string heldId, ref string rollNoPrefix, ref string examDate, ref int examSrNo)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@CoCode", SqlDbType.VarChar, 10) { Value = courseCode });
                parameters.Add(new SqlParameter("@ShId", SqlDbType.Int) { Value = scheduleId });
                parameters.Add(new SqlParameter("@InstId", SqlDbType.Int) { Value = instituteId });
                parameters.Add(new SqlParameter("@BranchId", SqlDbType.Int) { Value = streamId });

                DataSet ds = ExecuteStoredProcedure("usp_GetMiscReportParameters", parameters);
                if (ds.HasData())
                {
                    DataRow dataRow = ds.FirstRow();
                    coursePattern = dataRow["CoursePattern"].ToString();
                    scheduleExamId = dataRow["ShEmId"].Coalesce("0").ToInt32();
                    resultDate = dataRow["ResultDate"].ToString().Length > 0 ? dataRow["ResultDate"].ToString() : resultDate;
                    heldId = dataRow["HeldIn"].ToString().Length > 0 ? dataRow["HeldIn"].ToString() : heldId;
                    rollNoPrefix = dataRow["RollNoPrefix"].ToString();
                    examDate = dataRow["ExamDate"].ToString();
                    examSrNo = dataRow["ExamSrNo"].Coalesce("0").ToInt32();

                    resultDate = resultDate.ToDateTimeWithCoalesce().ToShortDateStringWithCoalesce();
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                log.Error(ex.StackTrace, ex);
            }
        }
        public DataSet NoMarks(int scheduleId, int branch, string center, string dsCode, string UC)
        {
            DataSet ds = new DataSet();
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@ShId", SqlDbType.Int) { Value = scheduleId });
                parameters.Add(new SqlParameter("@StrId", SqlDbType.Int) { Value = branch });
                parameters.Add(new SqlParameter("@Cent", SqlDbType.VarChar, 10) { Value = center });
                parameters.Add(new SqlParameter("@DsCode", SqlDbType.VarChar, 5) { Value = dsCode });
                parameters.Add(new SqlParameter("@StrUC", SqlDbType.Char, 1) { Value = UC });
                ds = ExecuteStoredProcedure("rpt_NoMarks", parameters);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                log.Error(ex.StackTrace, ex);
            }
            return ds;
        }
        public DataSet ExecuteStoredProcedure(string storedProcedureName, List<SqlParameter> parameters = null)
        {
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = storedProcedureName;
                    command.CommandTimeout = 0;
                    if (parameters != null && parameters.Count > 0)
                    {
                        command.Parameters.Clear();
                        command.Parameters.AddRange(parameters.ToArray());
                    }
                    try
                    {
                        connection.Open();
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            adapter.Fill(ds);
                        }
                        // Retrieve output parameter values if needed
                        foreach (var parameter in parameters)
                        {
                            if (parameter.Direction == ParameterDirection.Output ||
                                parameter.Direction == ParameterDirection.InputOutput)
                            {
                                parameter.Value = command.Parameters[parameter.ParameterName].Value;
                            }
                        }
                        if (ds != null && ds.Tables.Count > 0)
                        {
                            foreach (DataTable table in ds.Tables)
                            {
                                try
                                {
                                    if (table.FirstRow()["TableName"].Coalesce() != "")
                                    {
                                        table.TableName = table.FirstRow()["TableName"].ToString();
                                    }
                                }
                                catch (Exception ex) { }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex.Message, ex);
                        log.Error(ex.StackTrace, ex);
                    }
                    finally
                    {
                        if (connection.State != ConnectionState.Closed)
                        {
                            connection.Close();
                        }
                    }
                }
            }
            return ds;
        }

        public void PrepareAdmitCard(string spName, int loginId, int scheduleId, string collegeCode, int rollNo, string registartionNo, int instituteId, string center, int branchId, string image)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@LoginId", SqlDbType.Int) { Value = loginId });
                parameters.Add(new SqlParameter("@ShId", SqlDbType.Int) { Value = scheduleId });
                parameters.Add(new SqlParameter("@CgCode", SqlDbType.VarChar, 10) { Value = collegeCode });
                parameters.Add(new SqlParameter("@RollNo", SqlDbType.Int) { Value = rollNo });
                if (spName.ToUpper() == "rpt_AdmitCardsI".ToUpper())
                {
                    parameters.Add(new SqlParameter("@RgNo", SqlDbType.VarChar, 20) { Value = registartionNo });
                    parameters.Add(new SqlParameter("@Inst", SqlDbType.SmallInt) { Value = instituteId });

                }
                else if (spName.ToUpper() == "rpt_AdmitCardsI_11".ToUpper())
                {
                    parameters.Add(new SqlParameter("@StrId", SqlDbType.Int) { Value = branchId });
                }
                parameters.Add(new SqlParameter("@CentCode", SqlDbType.VarChar, 10) { Value = center });
                parameters.Add(new SqlParameter("@Img", SqlDbType.Char, 1) { Value = image });

                ExecuteStoredProcedure(spName, parameters);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                log.Error(ex.StackTrace, ex);
            }
        }

        public string PrepareImages(int loginId, string imageType, string imagePath)
        {
            try
            {
                bool includePhoto = imageType.Contains("P");
                bool includeSignature = imageType.Contains("S");
                bool includeAddress = imageType.Contains("A");

                DataSet imageRecordSet = new DataSet();
                try
                {
                    List<SqlParameter> parameters = new List<SqlParameter>();
                    parameters.Add(new SqlParameter("@LoginId", SqlDbType.Int) { Value = loginId });
                    imageRecordSet = ExecuteStoredProcedure("usp_GetImagesForReport", parameters);
                }
                catch (Exception ex) { }


                long totalRecords = 0;
                long notFoundCount = 0;

                if (imageRecordSet.HasData())
                {
                    foreach (DataRow imageRow in imageRecordSet.Tables[0].Rows)
                    {
                        try
                        {
                            string imageName = imagePath + imageRow["RgImg"].ToString();
                            byte[] imageBytes = null;
                            if (includePhoto)
                            {
                                totalRecords++;
                                string photoFileName = imageName + "_P.JPG";
                                if (File.Exists(photoFileName))
                                {
                                    imageBytes = File.ReadAllBytes(photoFileName);
                                }
                                else
                                {
                                    photoFileName = imageName + "_P.JPEG";
                                    if (File.Exists(photoFileName))
                                    {
                                        imageBytes = File.ReadAllBytes(photoFileName);
                                    }
                                    else
                                    {
                                        notFoundCount++;
                                    }
                                }
                            }

                            if (includeSignature)
                            {
                                totalRecords++;
                                string signatureFileName = imageName + "_S.JPG";
                                if (File.Exists(signatureFileName))
                                {
                                    imageBytes = File.ReadAllBytes(signatureFileName);
                                }
                                else
                                {
                                    signatureFileName = imageName + "_S.JPEG";
                                    if (File.Exists(signatureFileName))
                                    {
                                        imageBytes = File.ReadAllBytes(signatureFileName);
                                    }
                                    else
                                    {
                                        notFoundCount++;
                                    }
                                }
                            }

                            if (includeAddress)
                            {
                                totalRecords++;
                                string addressFileName = imageName + "_A.JPG";
                                if (File.Exists(addressFileName))
                                {
                                    imageBytes = File.ReadAllBytes(addressFileName);
                                }
                                else
                                {
                                    addressFileName = imageName + "_A.JPEG";
                                    if (File.Exists(addressFileName))
                                    {
                                        imageBytes = File.ReadAllBytes(addressFileName);
                                    }
                                    else
                                    {
                                        notFoundCount++;
                                    }
                                }
                            }

                            try
                            {
                                if (imageBytes != null)
                                {
                                    List<SqlParameter> parameters = new List<SqlParameter>();
                                    parameters.Add(new SqlParameter("@LoginId", SqlDbType.Int) { Value = loginId });
                                    parameters.Add(new SqlParameter("@ImgRgId", SqlDbType.Int) { Value = imageRow["ImgRgId"].ToString().ToInt32() });
                                    parameters.Add(new SqlParameter("@RgImg", SqlDbType.VarChar, 20) { Value = imageRow["RgImg"].ToString() });
                                    parameters.Add(new SqlParameter("@Image", SqlDbType.Image) { Value = imageBytes });
                                    parameters.Add(new SqlParameter("@ImageType", SqlDbType.VarChar, 10) { Value = (includePhoto ? "P" : (includeSignature ? "S" : (includeAddress ? "A" : ""))) });
                                    ExecuteStoredProcedure("usp_UpdateReportImage", parameters);
                                }
                            }
                            catch (Exception ex) { }
                        }
                        catch (Exception ex) { }
                    }

                }
                return notFoundCount + " Images Not Found out of expected " + totalRecords + Environment.NewLine + "Proceed with Printing?";
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                log.Error(ex.StackTrace, ex);
                return null;
            }
            throw new NotImplementedException();
        }

        public void ClearImages(int loginId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@LoginId", SqlDbType.Int) { Value = loginId });
                ExecuteStoredProcedure("usp_ClearReportImages", parameters);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                log.Error(ex.StackTrace, ex);
            }
        }

        public void PrepareExamForms(int loginId, int esId, string collegeCode, string image)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@LoginId", SqlDbType.Int) { Value = loginId });
            parameters.Add(new SqlParameter("@EcId", SqlDbType.Int) { Value = esId });
            parameters.Add(new SqlParameter("@CgCode", SqlDbType.VarChar, 10) { Value = collegeCode });
            parameters.Add(new SqlParameter("@Img", SqlDbType.Char, 1) { Value = image });
            ExecuteStoredProcedure("rpt_ExamFormI", parameters);
        }

        public List<SqlParameter> GetSqlParameters(string storedProcedureName)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(storedProcedureName, connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Derive the parameters
                        connection.Open();
                        SqlCommandBuilder.DeriveParameters(cmd);
                        connection.Close();


                        foreach (SqlParameter param in cmd.Parameters)
                        {
                            SqlParameter sqlParameter = new SqlParameter();
                            sqlParameter.ParameterName = param.ParameterName;
                            sqlParameter.Value = param.Value;
                            sqlParameter.DbType = param.DbType;
                            sqlParameter.Direction = param.Direction;
                            sqlParameter.Size = param.Size;
                            sqlParameter.IsNullable = param.IsNullable;
                            sqlParameter.Precision = param.Precision;
                            sqlParameter.Scale = param.Scale;
                            sqlParameters.Add(sqlParameter);
                        }
                    }
                }
            }
            catch (Exception error)
            {
                log.Error(error.Message, error);
                log.Error(error.StackTrace, error);
            }
            return sqlParameters;
        }

        public void PrepareMarkListImages(int loginId, int scheduleId, int streamId, string collegeCode, int rollNo, string registartionNo, string pageBreak, int instituteId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@LoginId", SqlDbType.Int) { Value = loginId });
                parameters.Add(new SqlParameter("@ShId", SqlDbType.Int) { Value = scheduleId });
                parameters.Add(new SqlParameter("@StrId", SqlDbType.Int) { Value = streamId });
                parameters.Add(new SqlParameter("@CgCode", SqlDbType.VarChar, 10) { Value = collegeCode });
                parameters.Add(new SqlParameter("@EfRollNo", SqlDbType.Int) { Value = rollNo });
                parameters.Add(new SqlParameter("@RgNo", SqlDbType.VarChar, 20) { Value = registartionNo });
                parameters.Add(new SqlParameter("@Flag", SqlDbType.Char, 1) { Value = pageBreak });
                parameters.Add(new SqlParameter("@Inst", SqlDbType.Int) { Value = instituteId });

                ExecuteStoredProcedure("Rpt_CryMarkL_Img", parameters);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                log.Error(ex.StackTrace, ex);
            }
        }

        public void PrepareMarkListWithImages(int loginId, int scheduleId, int streamId, string collegeCode, int rollNo, string registartionNo, object flag, object imageNames, int instituteId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@LoginId", SqlDbType.Int) { Value = loginId });
                parameters.Add(new SqlParameter("@ShId", SqlDbType.Int) { Value = scheduleId });
                parameters.Add(new SqlParameter("@StrId", SqlDbType.Int) { Value = streamId });
                parameters.Add(new SqlParameter("@CgCode", SqlDbType.VarChar, 10) { Value = collegeCode });
                parameters.Add(new SqlParameter("@EfRollNo", SqlDbType.Int) { Value = rollNo });
                parameters.Add(new SqlParameter("@RgNo", SqlDbType.VarChar, 20) { Value = registartionNo });
                parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar, 10) { Value = flag });
                parameters.Add(new SqlParameter("@ImgRun", SqlDbType.Char, 1) { Value = imageNames });
                parameters.Add(new SqlParameter("@Inst", SqlDbType.Int) { Value = instituteId });

                ExecuteStoredProcedure("Rpt_MarkListI", parameters);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                log.Error(ex.StackTrace, ex);
            }
        }

        public void PrepareImages_RptLedgerCo(int loginId, int scheduleId, int streamId, string collegeCode, string center, int gLnI, int instituteId, int passFail, string pageBreak, object reportType, int rollNo, object imageYN, string dsCode)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@LoginId", SqlDbType.Int) { Value = loginId });
                parameters.Add(new SqlParameter("@ShId", SqlDbType.Int) { Value = scheduleId });
                parameters.Add(new SqlParameter("@StrId", SqlDbType.Int) { Value = streamId });
                parameters.Add(new SqlParameter("@CgCode", SqlDbType.VarChar, 10) { Value = collegeCode });
                parameters.Add(new SqlParameter("@Cent", SqlDbType.VarChar, 10) { Value = center });
                parameters.Add(new SqlParameter("@CentCg", SqlDbType.SmallInt) { Value = gLnI });
                parameters.Add(new SqlParameter("@Inst", SqlDbType.SmallInt) { Value = instituteId });
                parameters.Add(new SqlParameter("@PassFail", SqlDbType.SmallInt) { Value = passFail });
                parameters.Add(new SqlParameter("@Type", SqlDbType.Char, 1) { Value = pageBreak });
                parameters.Add(new SqlParameter("@Rep", SqlDbType.VarChar, 6) { Value = reportType });
                parameters.Add(new SqlParameter("@RollNo", SqlDbType.Int) { Value = rollNo });
                parameters.Add(new SqlParameter("@Img", SqlDbType.VarChar, 20) { Value = imageYN });
                parameters.Add(new SqlParameter("@DsCode", SqlDbType.VarChar, 10) { Value = dsCode });

                ExecuteStoredProcedure("Rpt_LedgerCo", parameters);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                log.Error(ex.StackTrace, ex);
            }
        }

        internal string GetMarksheetReportFileName(string examCourseCode)
        {
            string reportFileName = "";
            DataSet dataSet = new DataSet();
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@examCourseCode", SqlDbType.VarChar, 20) { Value = examCourseCode });
                dataSet = ExecuteStoredProcedure("usp_GetMarksheetReportFileName", parameters);
                if (dataSet.HasData())
                {
                    DataRow dataRow = dataSet.FirstRow();
                    reportFileName = dataRow["ReportFileName"].Coalesce();
                }
            }
            catch (Exception ex) { }
            return reportFileName;
        }

        internal DataSet Result(int scheduleId, int streamId)
        {
            DataSet ds = new DataSet();
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@ShId", SqlDbType.Int) { Value = scheduleId });
                parameters.Add(new SqlParameter("@EfStrId", SqlDbType.Int) { Value = streamId });
                ds = ExecuteStoredProcedure("rpt_Result", parameters);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                log.Error(ex.StackTrace, ex);
            }
            return ds;
        }

        internal void Prepare_MarkList_KKH_C(int loginId, int scheduleId, int streamId, string collegeCode, string registartionNo, int instituteId, string flag, string createImages)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@LoginId", SqlDbType.Int) { Value = loginId });
                parameters.Add(new SqlParameter("@ShId", SqlDbType.Int) { Value = scheduleId });
                parameters.Add(new SqlParameter("@StrId", SqlDbType.Int) { Value = streamId });
                parameters.Add(new SqlParameter("@CgCode", SqlDbType.VarChar, 10) { Value = collegeCode });
                parameters.Add(new SqlParameter("@RgNo", SqlDbType.VarChar, 20) { Value = registartionNo });
                parameters.Add(new SqlParameter("@Inst", SqlDbType.SmallInt) { Value = instituteId });
                parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar, 1) { Value = flag });
                parameters.Add(new SqlParameter("@Img", SqlDbType.Char, 1) { Value = createImages });

                ExecuteStoredProcedure("Rpt_MarkLKKH_C", parameters);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                log.Error(ex.StackTrace, ex);
            }
        }

        internal DataSet GetRegistrationCard(int loginId, int instituteId, int sessionNo, string courseCode, string collegeCode, string tnCode, string dsCode, string centerCode, int streamId, string registartionNo, string createImages)
        {
            DataSet ds = new DataSet();
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@LoginId", SqlDbType.Int) { Value = loginId });
                parameters.Add(new SqlParameter("@Inst", SqlDbType.Int) { Value = instituteId });
                parameters.Add(new SqlParameter("@SnNo", SqlDbType.Int) { Value = sessionNo });
                parameters.Add(new SqlParameter("@CoCode", SqlDbType.VarChar, 10) { Value = courseCode });
                parameters.Add(new SqlParameter("@CgCode", SqlDbType.VarChar, 10) { Value = collegeCode });
                parameters.Add(new SqlParameter("@TnCode", SqlDbType.VarChar, 5) { Value = tnCode });
                parameters.Add(new SqlParameter("@DsCode", SqlDbType.VarChar, 5) { Value = dsCode });
                parameters.Add(new SqlParameter("@CentCode", SqlDbType.VarChar, 10) { Value = centerCode });
                parameters.Add(new SqlParameter("@StrId", SqlDbType.Int) { Value = streamId });
                parameters.Add(new SqlParameter("@RgNo", SqlDbType.VarChar, 20) { Value = registartionNo });
                parameters.Add(new SqlParameter("@Img", SqlDbType.Char, 1) { Value = createImages });

                ds = ExecuteStoredProcedure("rpt_RegnCardI2", parameters);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                log.Error(ex.StackTrace, ex);
            }
            return ds;
        }

        internal DataSet GetRegistrationCount(int sessionNo, string courseCode, string collegeCode, string tnCode, string dsCode)
        {
            DataSet ds = new DataSet();
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@SnNo", SqlDbType.Int) { Value = sessionNo });
                parameters.Add(new SqlParameter("@CoCode", SqlDbType.VarChar, 10) { Value = courseCode });
                parameters.Add(new SqlParameter("@CgCode", SqlDbType.VarChar, 10) { Value = collegeCode });
                parameters.Add(new SqlParameter("@TnCode", SqlDbType.VarChar, 5) { Value = tnCode });
                parameters.Add(new SqlParameter("@DsCode", SqlDbType.VarChar, 5) { Value = dsCode });

                ds = ExecuteStoredProcedure("rpt_RegnCount", parameters);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                log.Error(ex.StackTrace, ex);
            }
            return ds;
        }

        internal DataSet GetRollList(int sessionNo, string courseCode)
        {
            DataSet ds = new DataSet();
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@SnNo", SqlDbType.Int) { Value = sessionNo });
                parameters.Add(new SqlParameter("@CoCode", SqlDbType.VarChar, 10) { Value = courseCode });

                ds = ExecuteStoredProcedure("rpt_CoRollList_11", parameters);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                log.Error(ex.StackTrace, ex);
            }
            return ds;
        }

        internal DataSet GetResultSummary_06(int sessionNo, string courseCode, string flag)
        {
            DataSet ds = new DataSet();
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@SnNo", SqlDbType.Int) { Value = sessionNo });
                parameters.Add(new SqlParameter("@CoCode", SqlDbType.VarChar, 10) { Value = courseCode });
                parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar, 1) { Value = flag });

                ds = ExecuteStoredProcedure("rpt_ConvList_06_Summ", parameters);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                log.Error(ex.StackTrace, ex);
            }
            return ds;
        }

        internal DataSet GetCenterSchoolList(int scheduleId, string centColl, string center, string dsCode)
        {
            DataSet ds = new DataSet();
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@ShId", SqlDbType.Int) { Value = scheduleId });
                parameters.Add(new SqlParameter("@CgCent", SqlDbType.VarChar, 4) { Value = centColl });
                parameters.Add(new SqlParameter("@Cent", SqlDbType.VarChar, 10) { Value = center });
                parameters.Add(new SqlParameter("@DsCode", SqlDbType.VarChar, 5) { Value = dsCode });

                ds = ExecuteStoredProcedure("rpt_CentSchoolList", parameters);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                log.Error(ex.StackTrace, ex);
            }
            return ds;
        }

        internal DataSet GetCenterCollegeList(int scheduleId, string flag, string tnCode)
        {
            DataSet ds = new DataSet();
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@ShId", SqlDbType.Int) { Value = scheduleId });
                parameters.Add(new SqlParameter("@CgCent", SqlDbType.VarChar, 10) { Value = flag });
                parameters.Add(new SqlParameter("@TnCode", SqlDbType.VarChar, 5) { Value = tnCode });

                ds = ExecuteStoredProcedure("rpt_CentColgList", parameters);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                log.Error(ex.StackTrace, ex);
            }
            return ds;
        }

        internal DataSet GetPaperCount(int scheduleId, string flag)
        {
            DataSet ds = new DataSet();
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@ShId", SqlDbType.Int) { Value = scheduleId });
                parameters.Add(new SqlParameter("@EfSpFlag", SqlDbType.VarChar, 5) { Value = flag });

                ds = ExecuteStoredProcedure("rpt_PaperCount", parameters);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                log.Error(ex.StackTrace, ex);
            }
            return ds;
        }

        internal DataSet GetCollegePaperSummary(int scheduleId, string uc, string collegeCode, string dsCode, int reportCode, int instituteId)
        {
            DataSet ds = new DataSet();
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@ShId", SqlDbType.Int) { Value = scheduleId });
                parameters.Add(new SqlParameter("@UC", SqlDbType.Char, 1) { Value = uc });
                parameters.Add(new SqlParameter("@CgCode", SqlDbType.VarChar, 10) { Value = collegeCode });
                parameters.Add(new SqlParameter("@DsCode", SqlDbType.VarChar, 5) { Value = dsCode });
                parameters.Add(new SqlParameter("@InCode", SqlDbType.SmallInt) { Value = reportCode });
                parameters.Add(new SqlParameter("@Inst", SqlDbType.SmallInt) { Value = instituteId });

                ds = ExecuteStoredProcedure("rpt_ColgPaperSumm", parameters);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                log.Error(ex.StackTrace, ex);
            }
            return ds;
        }

        internal DataSet GetRollList_6(int scheduleId, int intCollegeCenter, string collegeCode, string center)
        {
            DataSet ds = new DataSet();
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@ShId", SqlDbType.Int) { Value = scheduleId });
                parameters.Add(new SqlParameter("@CgCent", SqlDbType.Int) { Value = intCollegeCenter });
                parameters.Add(new SqlParameter("@CgCode", SqlDbType.VarChar, 10) { Value = collegeCode });
                parameters.Add(new SqlParameter("@Cent", SqlDbType.VarChar, 10) { Value = center });

                ds = ExecuteStoredProcedure("rpt_RollList6", parameters);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                log.Error(ex.StackTrace, ex);
            }
            return ds;
        }

        internal DataSet GetControlSheetSummary(int scheduleId, string uc, int scheduleExamId)
        {
            DataSet ds = new DataSet();
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@ShId", SqlDbType.Int) { Value = scheduleId });
                parameters.Add(new SqlParameter("@CollUni", SqlDbType.Char, 1) { Value = uc });
                parameters.Add(new SqlParameter("@EsId", SqlDbType.VarChar, 10) { Value = scheduleExamId });

                ds = ExecuteStoredProcedure("rpt_ControlSheetSum", parameters);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                log.Error(ex.StackTrace, ex);
            }
            return ds;
        }

        internal DataSet GetCollegeResult(int scheduleId, string collegeCode, int streamId)
        {
            DataSet ds = new DataSet();
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@ShId", SqlDbType.Int) { Value = scheduleId });
                parameters.Add(new SqlParameter("@CgCode", SqlDbType.VarChar, 10) { Value = collegeCode });
                parameters.Add(new SqlParameter("@EfStrId", SqlDbType.Int) { Value = streamId });

                ds = ExecuteStoredProcedure("rpt_ResultCg", parameters);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                log.Error(ex.StackTrace, ex);
            }
            return ds;
        }

        internal DataSet GetCollegeCenterSummary(int loginId, int scheduleId, string centerCollegeFlag, string collegeCode, string dsCode, int streamId, int instituteId)
        {
            DataSet ds = new DataSet();
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@LoginId", SqlDbType.Int) { Value = loginId });
                parameters.Add(new SqlParameter("@ShId", SqlDbType.Int) { Value = scheduleId });
                parameters.Add(new SqlParameter("@CgCent", SqlDbType.VarChar, 4) { Value = centerCollegeFlag });
                parameters.Add(new SqlParameter("@CgCode", SqlDbType.VarChar, 10) { Value = collegeCode });
                parameters.Add(new SqlParameter("@DsCode", SqlDbType.VarChar, 5) { Value = dsCode });
                parameters.Add(new SqlParameter("@StrId", SqlDbType.Int) { Value = streamId });

                ds = ExecuteStoredProcedure((instituteId == 14 || instituteId == 15 ? "rpt_Summary14" : "rpt_Summary"), parameters);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                log.Error(ex.StackTrace, ex);
            }
            return ds;
        }

        internal DataSet GetCenterPaperCount(int loginId, int scheduleId, string uc, int esId, string center, string dsCode, int reportCode)
        {
            DataSet ds = new DataSet();
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@LoginId", SqlDbType.Int) { Value = loginId });
                parameters.Add(new SqlParameter("@ShId", SqlDbType.Int) { Value = scheduleId });
                parameters.Add(new SqlParameter("@UC", SqlDbType.Char, 1) { Value = uc });
                parameters.Add(new SqlParameter("@EsId", SqlDbType.Int) { Value = esId });
                parameters.Add(new SqlParameter("@Cent", SqlDbType.VarChar, 10) { Value = center });
                parameters.Add(new SqlParameter("@DsCode", SqlDbType.VarChar, 5) { Value = dsCode });
                parameters.Add(new SqlParameter("@InCode", SqlDbType.SmallInt) { Value = reportCode });

                ds = ExecuteStoredProcedure("rpt_CentPaperCount", parameters);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                log.Error(ex.StackTrace, ex);
            }
            return ds;
        }

        internal DataSet GetCenterPaperCount_03(int scheduleId, int esId, string center, string dsCode, int reportCode)
        {
            DataSet ds = new DataSet();
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@ShId", SqlDbType.Int) { Value = scheduleId });
                parameters.Add(new SqlParameter("@EsId", SqlDbType.Int) { Value = esId });
                parameters.Add(new SqlParameter("@Cent", SqlDbType.VarChar, 10) { Value = center });
                parameters.Add(new SqlParameter("@DsCode", SqlDbType.VarChar, 5) { Value = dsCode });
                parameters.Add(new SqlParameter("@InCode", SqlDbType.SmallInt) { Value = reportCode });

                ds = ExecuteStoredProcedure("rpt_CentPaperCount_03", parameters);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                log.Error(ex.StackTrace, ex);
            }
            return ds;
        }

        internal DataSet GetLowerCasesCleared(string courseCode, int sessionNo)
        {
            DataSet ds = new DataSet();
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@CoCode", SqlDbType.VarChar, 10) { Value = courseCode });
                parameters.Add(new SqlParameter("@SnNo", SqlDbType.Int) { Value = sessionNo });

                ds = ExecuteStoredProcedure("rpt_LowerCleared", parameters);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                log.Error(ex.StackTrace, ex);
            }
            return ds;
        }

        internal DataSet GetSectionMarks(int loginId, int scheduleId, int centerCollege, string collegeCode, string center, int esId)
        {
            DataSet ds = new DataSet();
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@LoginId", SqlDbType.Int) { Value = loginId });
                parameters.Add(new SqlParameter("@ShId", SqlDbType.Int) { Value = scheduleId });
                parameters.Add(new SqlParameter("@CentCg", SqlDbType.SmallInt) { Value = centerCollege });
                parameters.Add(new SqlParameter("@CgCode", SqlDbType.VarChar, 10) { Value = collegeCode });
                parameters.Add(new SqlParameter("@Cent", SqlDbType.VarChar, 10) { Value = center });
                parameters.Add(new SqlParameter("@EsId", SqlDbType.Int) { Value = esId });

                ds = ExecuteStoredProcedure("rpt_SectionMarks", parameters);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                log.Error(ex.StackTrace, ex);
            }
            return ds;
        }

        internal DataSet GetConvocationList(int sessionNo, string courseCode)
        {
            DataSet ds = new DataSet();
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@SnNo", SqlDbType.SmallInt) { Value = sessionNo });
                parameters.Add(new SqlParameter("@Courses", SqlDbType.VarChar, 500) { Value = "," + courseCode + "," });

                ds = ExecuteStoredProcedure("rpt_ConvList_Dib", parameters);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                log.Error(ex.StackTrace, ex);
            }
            return ds;
        }

        internal void CrystalReportCheck(int loginId, int scheduleId, int streamId, string collegeCode, int centerCollege, string dsCode, int instituteId, string flag)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@LoginId", SqlDbType.Int) { Value = loginId });
                parameters.Add(new SqlParameter("@ShId", SqlDbType.Int) { Value = scheduleId });
                parameters.Add(new SqlParameter("@StrId", SqlDbType.Int) { Value = streamId });
                parameters.Add(new SqlParameter("@CgCode", SqlDbType.VarChar, 10) { Value = collegeCode });
                parameters.Add(new SqlParameter("@CentCg", SqlDbType.Int) { Value = centerCollege });
                parameters.Add(new SqlParameter("@DsCode", SqlDbType.VarChar, 5) { Value = dsCode });
                parameters.Add(new SqlParameter("@Inst", SqlDbType.SmallInt) { Value = instituteId });
                parameters.Add(new SqlParameter("@Flag", SqlDbType.Char, 1) { Value = flag });

                ExecuteStoredProcedure("Rpt_CryLedgerChk", parameters);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                log.Error(ex.StackTrace, ex);
            }
        }

        internal DataSet GetCrystalReportHeaders(int loginId)
        {
            DataSet ds = new DataSet();
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@LoginId", SqlDbType.Int) { Value = loginId });

                ds = ExecuteStoredProcedure("rpt_GetCrystalReportHeaders", parameters);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                log.Error(ex.StackTrace, ex);
            }
            return ds;
        }

        internal DataSet GetExamSubjectSetup(int scheduleExamId, int streamId)
        {
            DataSet ds = new DataSet();
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@EsEmId", SqlDbType.Int) { Value = scheduleExamId });
                parameters.Add(new SqlParameter("@EsStrId", SqlDbType.Int) { Value = streamId });

                ds = ExecuteStoredProcedure("sp_SearchExamSub", parameters);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                log.Error(ex.StackTrace, ex);
            }
            return ds;
        }

        internal DataSet GetExamSubjectOptions(int scheduleExamId, int streamId)
        {
            DataSet ds = new DataSet();
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@OpEmId", SqlDbType.Int) { Value = scheduleExamId });
                parameters.Add(new SqlParameter("@OpStrId", SqlDbType.Int) { Value = streamId });

                ds = ExecuteStoredProcedure("sp_SearchEsOption", parameters);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                log.Error(ex.StackTrace, ex);
            }
            return ds;
        }

        internal DataSet GetRollList(int scheduleId, string option, string collegeCode, string center, string dsCode)
        {
            DataSet ds = new DataSet();
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@ShId", SqlDbType.Int) { Value = scheduleId });
                parameters.Add(new SqlParameter("@CgCent", SqlDbType.VarChar, 10) { Value = option });
                parameters.Add(new SqlParameter("@CgCode", SqlDbType.VarChar, 10) { Value = collegeCode });
                parameters.Add(new SqlParameter("@Cent", SqlDbType.VarChar, 10) { Value = center });
                parameters.Add(new SqlParameter("@DsCode", SqlDbType.VarChar, 10) { Value = dsCode });

                ds = ExecuteStoredProcedure("rpt_RollList", parameters);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                log.Error(ex.StackTrace, ex);
            }
            return ds;
        }

        internal DataSet GetExamSchedules(int sessionNo, int loginId)
        {
            DataSet ds = new DataSet();
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@ShSnNo", SqlDbType.SmallInt) { Value = sessionNo });
                if (loginId > 0)
                    parameters.Add(new SqlParameter("@LoginId", SqlDbType.Int) { Value = loginId });

                if (loginId == 0)
                    ds = ExecuteStoredProcedure("rpt_Schedule", parameters);
                else
                {
                    parameters.Add(new SqlParameter("@PageSize", SqlDbType.Int) { Value = -1 });
                    parameters.Add(new SqlParameter("@PageNumber", SqlDbType.Int) { Value = 1 });

                    ds = ExecuteStoredProcedure("sp_SearchSchedule", parameters);
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                log.Error(ex.StackTrace, ex);
            }
            return ds;
        }

        internal DataSet GetCourseSchedules(int sessionNo, int loginId)
        {
            DataSet ds = new DataSet();
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@CShSnNo", SqlDbType.SmallInt) { Value = sessionNo });
                if (loginId > 0)
                    parameters.Add(new SqlParameter("@LoginId", SqlDbType.Int) { Value = loginId });

                if (loginId == 0)
                    ds = ExecuteStoredProcedure("rpt_CoSchedule", parameters);
                else
                {
                    parameters.Add(new SqlParameter("@PageSize", SqlDbType.Int) { Value = -1 });
                    parameters.Add(new SqlParameter("@PageNumber", SqlDbType.Int) { Value = 1 });

                    ds = ExecuteStoredProcedure("sp_SearchCoSchedule", parameters);
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                log.Error(ex.StackTrace, ex);
            }
            return ds;
        }

        internal DataSet GetCourseEditSchedules(int scheduleId, int inCode, string allLast)
        {
            DataSet ds = new DataSet();
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@EcShId", SqlDbType.Int) { Value = scheduleId });
                parameters.Add(new SqlParameter("@EcInCode", SqlDbType.SmallInt) { Value = inCode });
                parameters.Add(new SqlParameter("@AllLast", SqlDbType.Char, 1) { Value = allLast });

                ds = ExecuteStoredProcedure("rpt_CoEditControl", parameters);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                log.Error(ex.StackTrace, ex);
            }
            return ds;
        }

        internal DataSet GetFacultyList(string facultyCode = "", string filterType = "")
        {
            DataSet ds = new DataSet();
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@FcCode", SqlDbType.VarChar, 20) { Value = facultyCode });
                parameters.Add(new SqlParameter("@FilterType", SqlDbType.VarChar, 10) { Value = filterType });

                parameters.Add(new SqlParameter("@PageSize", SqlDbType.Int) { Value = 1000 });
                parameters.Add(new SqlParameter("@PageNumber", SqlDbType.Int) { Value = 1 });

                ds = ExecuteStoredProcedure("sp_SearchFaculty", parameters);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                log.Error(ex.StackTrace, ex);
            }
            return ds;
        }

        internal DataSet GetCollegeList(string collegeCode, string collegeName, string districtCode, string townCode, string collegeCenter, string onlyCenter, string collegeType, string hslcAhm, string searchString, int pageSize = -1, int pageNumber = 1)
        {
            DataSet ds = new DataSet();
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@CgCode", SqlDbType.VarChar, 10) { Value = collegeCode });
                parameters.Add(new SqlParameter("@CgName", SqlDbType.VarChar, 80) { Value = collegeName });
                parameters.Add(new SqlParameter("@DsCode", SqlDbType.VarChar, 5) { Value = districtCode });
                parameters.Add(new SqlParameter("@TnCode", SqlDbType.VarChar, 5) { Value = townCode });
                parameters.Add(new SqlParameter("@CgCent", SqlDbType.VarChar, 1) { Value = (collegeCenter.IsNullOrWhiteSpace() ? "" : (collegeCenter.ToBoolean() ? "Y" : "N")) });
                parameters.Add(new SqlParameter("@CgOnlyCent", SqlDbType.VarChar, 1) { Value = (onlyCenter.IsNullOrWhiteSpace() ? "" : (onlyCenter.ToBoolean() ? "Y" : "N")) });
                parameters.Add(new SqlParameter("@CgType", SqlDbType.VarChar, 5) { Value = collegeType });
                parameters.Add(new SqlParameter("@HslcAhm", SqlDbType.VarChar, 1) { Value = hslcAhm });
                parameters.Add(new SqlParameter("@SearchString", SqlDbType.VarChar, 5) { Value = searchString });

                parameters.Add(new SqlParameter("@PageSize", SqlDbType.Int) { Value = pageSize });
                parameters.Add(new SqlParameter("@PageNumber", SqlDbType.Int) { Value = pageNumber });

                ds = ExecuteStoredProcedure("sp_SearchCollege", parameters);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                log.Error(ex.StackTrace, ex);
            }
            return ds;
        }

        internal DataSet GetCourseCollegeList(string collegeCode, int collegeEmId, string courseCode, string courseEm, string byCollege)
        {
            DataSet ds = new DataSet();
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@CoCgCgCode", SqlDbType.VarChar, 10) { Value = collegeCode });
                parameters.Add(new SqlParameter("@CoCgEmId", SqlDbType.Int) { Value = collegeEmId });
                parameters.Add(new SqlParameter("@CoCgCoCode", SqlDbType.VarChar, 10) { Value = courseCode });
                parameters.Add(new SqlParameter("@CoEm", SqlDbType.VarChar, 5) { Value = courseEm });
                parameters.Add(new SqlParameter("@ByCg", SqlDbType.VarChar, 1) { Value = byCollege });

                ds = ExecuteStoredProcedure("rpt_CoColg", parameters);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                log.Error(ex.StackTrace, ex);
            }
            return ds;
        }

        internal DataSet GetCourseList(int loginId)
        {
            DataSet ds = new DataSet();
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@LoginId", SqlDbType.Int) { Value = loginId });

                ds = ExecuteStoredProcedure("rpt_Courses", parameters);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                log.Error(ex.StackTrace, ex);
            }
            return ds;
        }

        internal DataSet GetExamStream(string courseCode, int emId)
        {
            DataSet ds = new DataSet();
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@CoCode", SqlDbType.VarChar, 10) { Value = courseCode });
                parameters.Add(new SqlParameter("@EmId", SqlDbType.Int) { Value = emId });
                ds = ExecuteStoredProcedure("sp_SearchExamStr", parameters);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                log.Error(ex.StackTrace, ex);
            }
            return ds;
        }

        internal DataSet GetExamList(string courseCode, int emId)
        {
            String cgCode = "";
            DataSet ds = new DataSet();
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@EmCoCode", SqlDbType.VarChar, 10) { Value = courseCode });
                parameters.Add(new SqlParameter("@CgCode", SqlDbType.VarChar, 5) { Value = cgCode });
                parameters.Add(new SqlParameter("@EmId", SqlDbType.Int) { Value = emId });
                parameters.Add(new SqlParameter("@PageSize", SqlDbType.Int) { Value = 1000 });
                parameters.Add(new SqlParameter("@PageNumber", SqlDbType.Int) { Value = 1 });

                ds = ExecuteStoredProcedure("sp_SearchExam", parameters);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                log.Error(ex.StackTrace, ex);
            }
            return ds;
        }

        internal DataSet GetExamDivision(int examId, int examStreamId, string marksType)
        {
            DataSet ds = new DataSet();
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@EdEmId", SqlDbType.Int) { Value = examId });
                parameters.Add(new SqlParameter("@EdStrId", SqlDbType.Int) { Value = examStreamId });
                parameters.Add(new SqlParameter("@EdMarksType", SqlDbType.VarChar, 5) { Value = marksType });
                ds = ExecuteStoredProcedure("sp_SearchExamDiv", parameters);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                log.Error(ex.StackTrace, ex);
            }
            return ds;
        }

        internal DataSet GetExamAggregate(int examId)
        {
            DataSet ds = new DataSet();
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@EaEmId", SqlDbType.Int) { Value = examId });
                ds = ExecuteStoredProcedure("sp_SearchExamAggr", parameters);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                log.Error(ex.StackTrace, ex);
            }
            return ds;
        }

        internal DataSet GetDistrictList()
        {
            DataSet ds = new DataSet();
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@PageSize", SqlDbType.Int) { Value = -1 });
                parameters.Add(new SqlParameter("@PageNumber", SqlDbType.Int) { Value = 1 });

                ds = ExecuteStoredProcedure("sp_SearchDistrict", parameters);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                log.Error(ex.StackTrace, ex);
            }
            return ds;
        }

        internal DataSet GetTownList()
        {
            DataSet ds = new DataSet();
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@PageSize", SqlDbType.Int) { Value = -1 });
                parameters.Add(new SqlParameter("@PageNumber", SqlDbType.Int) { Value = 1 });

                ds = ExecuteStoredProcedure("sp_SearchTown", parameters);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                log.Error(ex.StackTrace, ex);
            }
            return ds;
        }

        internal DataSet GetDivisionList()
        {
            DataSet ds = new DataSet();
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@PageSize", SqlDbType.Int) { Value = -1 });
                parameters.Add(new SqlParameter("@PageNumber", SqlDbType.Int) { Value = 1 });
                ds = ExecuteStoredProcedure("sp_SearchDivision", parameters);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                log.Error(ex.StackTrace, ex);
            }
            return ds;
        }

        internal DataSet GetStateList()
        {
            DataSet ds = new DataSet();
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@PageSize", SqlDbType.Int) { Value = -1 });
                parameters.Add(new SqlParameter("@PageNumber", SqlDbType.Int) { Value = 1 });
                ds = ExecuteStoredProcedure("sp_SearchCountryState", parameters);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                log.Error(ex.StackTrace, ex);
            }
            return ds;
        }

        internal DataSet GetCountryList()
        {
            DataSet ds = new DataSet();
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@PageSize", SqlDbType.Int) { Value = -1 });
                parameters.Add(new SqlParameter("@PageNumber", SqlDbType.Int) { Value = 1 });

                ds = ExecuteStoredProcedure("sp_SearchCountry", parameters);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                log.Error(ex.StackTrace, ex);
            }
            return ds;
        }

        internal DataSet GetTehsilList()
        {
            DataSet ds = new DataSet();
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@PageSize", SqlDbType.Int) { Value = -1 });
                parameters.Add(new SqlParameter("@PageNumber", SqlDbType.Int) { Value = 1 });
                ds = ExecuteStoredProcedure("usp_SearchTehsil", parameters);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                log.Error(ex.StackTrace, ex);
            }
            return ds;
        }

        internal DataSet GetExamRegistrationDetails(long examFormId)
        {
            DataSet ds = new DataSet();
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@ExamFormId", SqlDbType.Int) { Value = examFormId });
                ds = ExecuteStoredProcedure("usp_GetExamRegistrationDetails", parameters);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                log.Error(ex.StackTrace, ex);
            }
            return ds;
        }

        internal DataSet Result_17(int scheduleId, int streamId)
        {
            DataSet ds = new DataSet();
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@ShId", SqlDbType.Int) { Value = scheduleId });
                parameters.Add(new SqlParameter("@EfStrId", SqlDbType.Int) { Value = streamId });
                ds = ExecuteStoredProcedure("rpt_Result_17", parameters);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                log.Error(ex.StackTrace, ex);
            }
            return ds;
        }

        internal DataSet GetCollegeResult_17(int scheduleId, string collegeCode, int streamId)
        {
            DataSet ds = new DataSet();
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@ShId", SqlDbType.Int) { Value = scheduleId });
                parameters.Add(new SqlParameter("@CgCode", SqlDbType.VarChar, 10) { Value = collegeCode });
                parameters.Add(new SqlParameter("@EfStrId", SqlDbType.Int) { Value = streamId });

                ds = ExecuteStoredProcedure("rpt_Result_17_Cg", parameters);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                log.Error(ex.StackTrace, ex);
            }
            return ds;
        }

        internal DataSet Get_SP_SearchEditMarks(int editEcId, string editChange, string editValid, string subjectCode, string subjectHead)
        {
            DataSet ds = new DataSet();
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@EcId", SqlDbType.Int) { Value = editEcId });
                parameters.Add(new SqlParameter("@Chng", SqlDbType.VarChar, 1) { Value = editChange });
                parameters.Add(new SqlParameter("@Valid", SqlDbType.VarChar, 1) { Value = editValid });
                parameters.Add(new SqlParameter("@Sub", SqlDbType.VarChar, 10) { Value = subjectCode });
                parameters.Add(new SqlParameter("@Head", SqlDbType.VarChar, 10) { Value = subjectHead });

                ds = ExecuteStoredProcedure("sp_SearchEditMarks", parameters);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                log.Error(ex.StackTrace, ex);
            }
            return ds;
        }

        internal void GetEditListTitle(int ecId, int inCode, ref string reportTitle, ref string reportSubTitle)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@EcId", SqlDbType.Int) { Value = ecId });
            parameters.Add(new SqlParameter("@InCode", SqlDbType.SmallInt) { Value = inCode });

            parameters.Add(new SqlParameter("@Title", SqlDbType.VarChar, 80) { Direction = ParameterDirection.Output });
            parameters.Add(new SqlParameter("@SubTitle", SqlDbType.VarChar, 80) { Direction = ParameterDirection.Output });

            ExecuteStoredProcedure("sp_GetEditListTitle", parameters);

            foreach (SqlParameter parameter in parameters)
            {
                try
                {
                    switch (parameter.ParameterName)
                    {
                        case "@Title":
                            reportTitle = parameter.Value.Coalesce();
                            break;
                        case "@SubTitle":
                            reportSubTitle = parameter.Value.Coalesce();
                            break;
                    }

                }
                catch (Exception err) { }
            }
        }

        internal void GetEditMarksD(int ecId, ref string emMarksD)
        {

            DataSet ds = new DataSet();
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@EcId", SqlDbType.Int) { Value = ecId });

                ds = ExecuteStoredProcedure("sp_GetEmMarksD", parameters);
                if (ds.HasData())
                {
                    emMarksD = ds.FirstRow()[0].ToString();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                log.Error(ex.StackTrace, ex);
            }

        }

        internal DataSet Get_SP_SearchCentreAllo(int ecId, string editChange, string editValid)
        {
            DataSet ds = new DataSet();
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@EcId", SqlDbType.Int) { Value = ecId });
                parameters.Add(new SqlParameter("@Chng", SqlDbType.VarChar, 1) { Value = editChange });
                parameters.Add(new SqlParameter("@Valid", SqlDbType.VarChar, 1) { Value = editValid });

                ds = ExecuteStoredProcedure("sp_SearchCentreAllo", parameters);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                log.Error(ex.StackTrace, ex);
            }
            return ds;
        }

        internal DataSet Get_SP_SearchEditExamForm(int ecId, string collegeCode, string editValid, string editChange, string editApplication, int examFormId)
        {
            DataSet ds = new DataSet();
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@EcId", SqlDbType.Int) { Value = ecId });
                parameters.Add(new SqlParameter("@CgCode", SqlDbType.VarChar, 10) { Value = collegeCode });
                parameters.Add(new SqlParameter("@Chng", SqlDbType.VarChar, 1) { Value = editChange });
                parameters.Add(new SqlParameter("@Valid", SqlDbType.VarChar, 1) { Value = editValid });
                parameters.Add(new SqlParameter("@App", SqlDbType.VarChar, 1) { Value = editApplication });
                parameters.Add(new SqlParameter("@EfId", SqlDbType.Int) { Value = examFormId });

                ds = ExecuteStoredProcedure("sp_SearchEditExamForm", parameters);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                log.Error(ex.StackTrace, ex);
            }
            return ds;
        }

        internal DataSet Get_SP_GetEditFormSub_Rpt(int ecId, int registrationId)
        {
            DataSet ds = new DataSet();
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@EcId", SqlDbType.Int) { Value = ecId });
                parameters.Add(new SqlParameter("@RgId", SqlDbType.Int) { Value = registrationId });

                ds = ExecuteStoredProcedure("sp_GetEditFormSub_rpt", parameters);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                log.Error(ex.StackTrace, ex);
            }
            return ds;
        }

        internal DataSet Get_SP_SearchEditChResult(int ecId, string editChange, string editValid)
        {
            DataSet ds = new DataSet();
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@EcId", SqlDbType.Int) { Value = ecId });
                parameters.Add(new SqlParameter("@Chng", SqlDbType.VarChar, 1) { Value = editChange });
                parameters.Add(new SqlParameter("@Valid", SqlDbType.VarChar, 1) { Value = editValid });

                ds = ExecuteStoredProcedure("sp_SearchEditChResult", parameters);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                log.Error(ex.StackTrace, ex);
            }
            return ds;
        }

        internal DataSet Get_SP_GetEditControl(int ecId)
        {
            DataSet ds = new DataSet();
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@EcId", SqlDbType.Int) { Value = ecId });

                ds = ExecuteStoredProcedure("sp_GetEditControl", parameters);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                log.Error(ex.StackTrace, ex);
            }
            return ds;
        }

        internal DataSet Get_Rpt_EditSubChng(int ecId, string collegeCode, string editChange, string editValid)
        {
            DataSet ds = new DataSet();
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@EcId", SqlDbType.Int) { Value = ecId });
                parameters.Add(new SqlParameter("@CgCode", SqlDbType.VarChar, 10) { Value = collegeCode });
                parameters.Add(new SqlParameter("@Chng", SqlDbType.VarChar, 1) { Value = editChange });
                parameters.Add(new SqlParameter("@Valid", SqlDbType.VarChar, 1) { Value = editValid });

                ds = ExecuteStoredProcedure("rpt_EditSubChng", parameters);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                log.Error(ex.StackTrace, ex);
            }
            return ds;
        }

        internal DataSet Get_Rpt_InvdRegnForm(int ecId, string collegeCode)
        {
            DataSet ds = new DataSet();
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@EcId", SqlDbType.Int) { Value = ecId });
                parameters.Add(new SqlParameter("@CgCode", SqlDbType.VarChar, 10) { Value = collegeCode });

                ds = ExecuteStoredProcedure("rpt_InvdRegnForm", parameters);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                log.Error(ex.StackTrace, ex);
            }
            return ds;
        }

        internal DataSet Get_SP_SearchEditRegnForm(int ecId, string collegeCode, string editValid, string editChange)
        {
            DataSet ds = new DataSet();
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@EcId", SqlDbType.Int) { Value = ecId });
                parameters.Add(new SqlParameter("@CgCode", SqlDbType.VarChar, 10) { Value = collegeCode });
                parameters.Add(new SqlParameter("@Chng", SqlDbType.VarChar, 1) { Value = editChange });
                parameters.Add(new SqlParameter("@Valid", SqlDbType.VarChar, 1) { Value = editValid });

                ds = ExecuteStoredProcedure("sp_SearchEditRegnForm", parameters);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                log.Error(ex.StackTrace, ex);
            }
            return ds;
        }

        internal DataSet Get_SP_SearchEditStatus(int ecId, string collegeCode, string editValid, string editChange)
        {
            DataSet ds = new DataSet();
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@EcId", SqlDbType.Int) { Value = ecId });
                parameters.Add(new SqlParameter("@CgCode", SqlDbType.VarChar, 10) { Value = collegeCode });
                parameters.Add(new SqlParameter("@Chng", SqlDbType.VarChar, 1) { Value = editChange });
                parameters.Add(new SqlParameter("@Valid", SqlDbType.VarChar, 1) { Value = editValid });

                ds = ExecuteStoredProcedure("sp_SearchEditStatus", parameters);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                log.Error(ex.StackTrace, ex);
            }
            return ds;
        }
    }
}