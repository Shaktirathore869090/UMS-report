//using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Web;
using static UMSReportsAPI.Extenders.DateExtender;

namespace UMSReportsAPI.Extenders
{
    public static class DataExtender
    {
        public static bool HasRecords(this DataTable value)
        {
            if (value == null)
                return false;
            if (value.Rows.Count <= 0)
                return false;
            else
                return true;
        }

        public static int RecordCount(this DataTable value)
        {
            if (!value.HasRecords())
                return 0;
            else
            {
                return value.Rows.Count;
            }
        }

        public static bool HasTable(this DataSet value, string tableName)
        {
            if (value.Tables.Contains(tableName))
                return true;
            else
                return false;
        }

        public static bool HasData(this DataSet value)
        {
            if (value != null && value.Tables.Count > 0 && value.Table(0).HasRecords())
                return true;
            else
                return false;
        }

        public static bool HasData(this DataTable value)
        {
            if (value != null && value.HasRecords())
                return true;
            else
                return false;
        }

        public static DataTable Table(this DataSet value, string tableName)
        {
            if (value.HasTable(tableName))
                return value.Tables[tableName];
            else
                return null;
        }

        public static DataTable Table(this DataSet value, int index)
        {
            if (value.Tables.Count > index)
                return value.Tables[index];
            else
                return null;
        }

        public static void MapTableNames(this DataSet value)
        {
            foreach (DataTable oTable in value.Tables)
            {
                try
                {
                    if (oTable.Columns.Contains("TableName"))
                        oTable.TableName = oTable.Rows[0]["TableName"].ToString();
                }
                catch { }
            }
        }

        public static string ToJSON(this DataTable oTable)
        {
            //System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            //List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            //Dictionary<string, object> row;
            //foreach (DataRow dr in oTable.Rows)
            //{
            //    row = new Dictionary<string, object>();
            //    foreach (DataColumn col in oTable.Columns)
            //    {
            //        row.Add(col.ColumnName, dr[col]);
            //    }
            //    rows.Add(row);
            //}
            //return serializer.Serialize(rows);
            try
            {
                return JsonConvert.SerializeObject(oTable, Formatting.Indented);
            }
            catch { }
            return "";
        }

        public static long ToLong(this object value)
        {
            long _value = 0;
            try
            {
                _value = Convert.ToInt64(value);
            }
            catch { }
            return _value;
        }

        public static decimal ToDecimal(this object value)
        {
            decimal _value = 0;
            try
            {
                _value = Convert.ToDecimal(value);
            }
            catch { }
            return _value;
        }

        public static List<T> ToList<T>(this DataTable dt) where T : new()
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = new T();
                item.MapDataRow(row);
                data.Add(item);
            }
            return data;
        }

        private static T MapDataRow<T>(this T item, DataRow row) where T : new()
        {
            foreach (DataColumn column in row.Table.Columns)
            {
                try
                {
                    // find the property for the column
                    PropertyInfo p = item.GetType().GetProperty(column.ColumnName);

                    // if exists, set the value
                    if (p != null && row[column] != DBNull.Value)
                    {
                        p.SetValue(item, row[column], null);
                    }
                }
                catch (Exception error)
                {
                    Console.WriteLine(error.Message);
                }
            }
            return item;
        }

        public static void ToCSV(this DataTable dtDataTable, string strFilePath, bool headers = true, string deliminator = ",", bool removeCommaInValue = false, bool addTextQualifier = false, DateFormat dateFormat = DateFormat.NoChange, string jsonFields = "")
        {
            deliminator = deliminator.Coalesce(",");
            StreamWriter sw = new StreamWriter(strFilePath, false);
            //headers    
            if (headers)
            {
                for (int i = 0; i < dtDataTable.Columns.Count; i++)
                {
                    sw.Write(dtDataTable.Columns[i]);
                    if (i < dtDataTable.Columns.Count - 1)
                    {
                        sw.Write(deliminator);
                    }
                }
                sw.Write(sw.NewLine);
            }
            foreach (DataRow dr in dtDataTable.Rows)
            {
                for (int i = 0; i < dtDataTable.Columns.Count; i++)
                {
                    if (!Convert.IsDBNull(dr[i]))
                    {
                        string value = dr[i].ToString();

                        if (dtDataTable.Columns[i].DataType == typeof(DateTime))
                        {
                            switch (dateFormat)
                            {
                                case DateFormat.Iso8601:
                                    value = dr[i].ToDateTimeWithCoalesce().ToISO8601();
                                    break;
                                case DateFormat.NoChange:
                                default:
                                    break;
                            }
                        }
                        //Convert to base64
                        if (dtDataTable.Columns[i].ColumnName.ToLower().IndexOf("base64") >= 0)
                        {
                            value = value.Base64Encode();
                        }

                        //Check Json fields
                        List<string> jsonFieldsList = jsonFields.Split(',').ToList();
                        if (jsonFieldsList.Contains(dtDataTable.Columns[i].ColumnName))
                        {
                            value = value.Replace("'", "");
                            //value = value.Replace("\\", "");
                            //value = "\"" + value.Replace("\"", "'") + "\"";
                            value = "\"" + value.Replace("\"", "\"\"") + "\"";
                            //value = "\"" + value + "\"";
                        }
                        else
                        {
                            if (addTextQualifier && value.Contains("\""))
                            {
                                //value = value.Replace("\"", "\"\"");
                                value = value.Replace("'", "");
                                value = value.Replace("\"", "'");
                            }
                            if (value.Contains("NULL"))
                            {
                                value = value.Replace("NULL", "");
                            }
                            if (value.Contains(',') && !removeCommaInValue)
                            {
                                value = String.Format("\"{0}\"", value);

                            }
                            else if (removeCommaInValue)
                            {
                                value = value.Replace(",", "");
                            }
                        }

                        sw.Write(value);

                    }
                    if (i < dtDataTable.Columns.Count - 1)
                    {
                        sw.Write(deliminator);
                    }
                }
                sw.Write(sw.NewLine);
            }
            sw.Close();
        }
        public static HttpResponseMessage ToCSV_InHttpResponse(this DataTable dtDataTable, bool headers = true, string deliminator = ",", bool removeCommaInValue = false, bool addTextQualifier = false, DateFormat dateFormat = DateFormat.NoChange, string jsonFields = "")
        {
            deliminator = deliminator.Coalesce(",");
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            MemoryStream stream = new MemoryStream();
            StreamWriter sw = new StreamWriter(stream, Encoding.UTF8);
            //headers    
            if (headers)
            {
                for (int i = 0; i < dtDataTable.Columns.Count; i++)
                {
                    sw.Write(dtDataTable.Columns[i]);
                    if (i < dtDataTable.Columns.Count - 1)
                    {
                        sw.Write(deliminator);
                    }
                }
                sw.Write(sw.NewLine);
            }
            foreach (DataRow dr in dtDataTable.Rows)
            {
                for (int i = 0; i < dtDataTable.Columns.Count; i++)
                {
                    if (!Convert.IsDBNull(dr[i]))
                    {
                        string value = dr[i].ToString();

                        if (dtDataTable.Columns[i].DataType == typeof(DateTime))
                        {
                            switch (dateFormat)
                            {
                                case DateFormat.Iso8601:
                                    value = dr[i].ToDateTimeWithCoalesce().ToISO8601();
                                    break;
                                case DateFormat.NoChange:
                                default:
                                    break;
                            }
                        }
                        //Convert to base64
                        if (dtDataTable.Columns[i].ColumnName.ToLower().IndexOf("base64") >= 0)
                        {
                            value = value.Base64Encode();
                        }

                        //Check Json fields
                        List<string> jsonFieldsList = jsonFields.Split(',').ToList();
                        if (jsonFieldsList.Contains(dtDataTable.Columns[i].ColumnName))
                        {
                            value = value.Replace("'", "");
                            //value = value.Replace("\\", "");
                            //value = "\"" + value.Replace("\"", "'") + "\"";
                            value = "\"" + value.Replace("\"", "\"\"") + "\"";
                            //value = "\"" + value + "\"";
                        }
                        else
                        {
                            if (addTextQualifier && value.Contains("\""))
                            {
                                //value = value.Replace("\"", "\"\"");
                                value = value.Replace("'", "");
                                value = value.Replace("\"", "'");
                            }
                            if (value.Contains("NULL"))
                            {
                                value = value.Replace("NULL", "");
                            }
                            if (value.Contains(',') && !removeCommaInValue)
                            {
                                value = String.Format("\"{0}\"", value);

                            }
                            else if (removeCommaInValue)
                            {
                                value = value.Replace(",", "");
                            }
                        }

                        sw.Write(value);

                    }
                    if (i < dtDataTable.Columns.Count - 1)
                    {
                        sw.Write(deliminator);
                    }
                }
                sw.Write(sw.NewLine);
            }

            sw.Flush();
            stream.Position = 0;

            response.Content = new StreamContent(stream);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/csv");
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "data.csv"
            };
            return response;
        }

        public static DataRow FirstRow(this DataSet ds)
        {
            return ds.Tables[0].FirstRow();
        }

        public static DataTable FirstTable(this DataSet ds)
        {
            DataTable dt = new DataTable();
            if (ds.HasData())
                return ds.Tables[0];

            return dt;
        }

        public static DataRow FirstRow(this DataTable dt)
        {
            return dt.Rows[0];
        }

        public static JObject ToJObject(this DataSet ds, bool eliminateArrayForSingleRecord = false)
        {
            //JArray tablesArray = new JArray();
            JObject jObject = new JObject();
            int counter = 0;
            foreach (DataTable dt in ds.Tables)
            {
                try
                {
                    if (dt.HasData() && dt.Rows[0]["DocxArray"].ToBoolean())
                        eliminateArrayForSingleRecord = false;
                }
                catch { }
                string tableName = dt.TableName.Coalesce("Table_" + counter);
                jObject.Add(tableName, dt.ToJObject(eliminateArrayForSingleRecord));
                counter++;
            }
            return jObject;
        }
        //public static JObject ToJObject(this DataTable dt, bool eliminateArrayForSingleRecord = false)
        //{
        //    JArray rowsArray = new JArray();
        //    foreach (DataRow row in dt.Rows)
        //    {
        //        rowsArray.Add(JObject.FromObject(row.Table.Columns.Cast<DataColumn>().ToDictionary(col => col.ColumnName, col => row[col])));
        //    }
        //    if (eliminateArrayForSingleRecord && rowsArray.Count == 1)
        //        return (JObject)rowsArray[0];

        //    return new JObject { ["Rows"] = rowsArray };
        //}

        public static JObject ToJObject(this DataTable dt, bool eliminateArrayForSingleRecord = false)
        {
            var groupColumns = dt.Columns
                .Cast<DataColumn>()
                .Where(c => c.ColumnName.StartsWith("GRP_"))
                .Select(c => c.ColumnName)
                .ToList();

            var rows = dt.AsEnumerable();

            if (!groupColumns.Any())
            {
                // No grouping, return flat
                var flatRows = new JArray();
                foreach (var row in rows)
                    flatRows.Add(row.ToJObject());
                if (eliminateArrayForSingleRecord && flatRows.Count == 1)
                    return (JObject)flatRows[0];
                else
                    return new JObject { ["Rows"] = flatRows };
            }

            var grouped = rows
                .GroupBy(row => string.Join("||", groupColumns.Select(col => row[col]?.ToString())))
                .Select(g =>
                {
                    var sample = g.First();
                    var groupObject = new JObject();

                    // Add group keys
                    foreach (var col in groupColumns)
                        groupObject[col] = JToken.FromObject(sample[col]);

                    // Add grouped rows
                    var rowArray = new JArray();
                    foreach (var row in g)
                        rowArray.Add(row.ToJObject());
                    groupObject["Rows"] = rowArray;

                    return groupObject;
                });

            var groupsArray = new JArray(grouped);

            if (eliminateArrayForSingleRecord && groupsArray.Count == 1)
                return (JObject)groupsArray[0];

            return new JObject { ["Groups"] = groupsArray };
        }

        public static JObject ToJObject(this DataRow row)
        {
            return JObject.FromObject(row.Table.Columns.Cast<DataColumn>().ToDictionary(col => col.ColumnName, col => row[col]));
        }

        public static List<byte[]> ToByteArrayList(this DataTable dataTable, string columnName)
        {
            var byteArrayList = new List<byte[]>();

            foreach (DataRow row in dataTable.Rows)
            {
                string base64Data = row[columnName].ToString();

                // Remove the prefix if it exists
                if (base64Data.StartsWith("data:application/pdf;base64,"))
                {
                    base64Data = base64Data.Replace("data:application/pdf;base64,", string.Empty);
                }

                // Convert the Base64 string to a byte array
                try
                {
                    byte[] byteArray = Convert.FromBase64String(base64Data);
                    byteArrayList.Add(byteArray);
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid Base64 string: " + base64Data);
                }
            }

            return byteArrayList;
        }

        public static JArray ToByteJArray(this DataTable dataTable, string columnName)
        {
            JArray byteJArray = new JArray();

            foreach (DataRow row in dataTable.Rows)
            {
                string base64Data = row[columnName].ToString();

                // Remove the prefix if it exists
                if (base64Data.StartsWith("data:application/pdf;base64,"))
                {
                    base64Data = base64Data.Replace("data:application/pdf;base64,", string.Empty);
                }

                // Convert the Base64 string to a byte array
                try
                {
                    byte[] byteArray = Convert.FromBase64String(base64Data);
                    byteJArray.Add(byteArray);
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid Base64 string: " + base64Data);
                }
            }

            return byteJArray;
        }

        public static JArray SaveBase64ToFiles(this DataTable dataTable, string columnName, SaveFileType saveFileType, string folderPath, string filePrefix)
        {
            JArray fileArray = new JArray();

            foreach (DataRow row in dataTable.Rows)
            {
                string fileName = Path.Combine(folderPath, $"{filePrefix}_{Guid.NewGuid().ToString()}.{saveFileType.ToString()}");
                try
                {
                    string rawData = row[columnName].ToString();
                    string base64Identifier = ";base64,";
                    int base64Index = rawData.IndexOf(base64Identifier);
                    string fileType = rawData.Substring(0, base64Index).Replace("data:", "");

                    if (fileType.ToUpper().IndexOf(saveFileType.ToString().ToUpper()) > 0)
                    {
                        byte[] fileBytes = Convert.FromBase64String(rawData.Substring(base64Index + base64Identifier.Length));
                        File.WriteAllBytes(fileName, fileBytes);
                        fileArray.Add(fileName);
                    }
                }
                catch (Exception error) { }
            }
            return fileArray;
        }
    }

    public enum SaveFileType
    {
        PDF,
        PNG,
        JPEG,
        TXT
    }
}