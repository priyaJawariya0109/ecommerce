using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Data;
using System.Text.RegularExpressions;
using System.Globalization;
/*using iTextSharp.text;
using iTextSharp.text.pdf;*/
using System.Web;
using System.Drawing.Printing;
using System.Drawing;
using System.Xml.Linq;

namespace ecommerce.Models
{
    public class Utility
    {
        public static int GetWeekNumber()
        {
            CultureInfo ciCurr = CultureInfo.CurrentCulture;
            int weekNum = ciCurr.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            return weekNum;
        }

        public static void WriteMsg(Exception ex)
        {
            int WeekNum = GetWeekNumber();
            int Year = DateTime.Now.Year;
            string AppendStr = WeekNum.ToString() + "_" + Year.ToString() + "_";

            string appath = System.AppDomain.CurrentDomain.BaseDirectory + "Error\\";

            if (!Directory.Exists(appath))
            {
                Directory.CreateDirectory(appath);
            }

            string filePath = appath + AppendStr + "Error.txt";
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine("Message :" + ex.Message + "<br/>" + Environment.NewLine + "StackTrace :" + ex.StackTrace +
                   "" + Environment.NewLine + "Date :" + DateTime.Now.ToString());
                writer.WriteLine(Environment.NewLine + "-----------------------------------------------------------------------------" + Environment.NewLine);
            }
        }

        public static void WriteMsg(string msg)
        {
            int WeekNum = GetWeekNumber();
            int Year = DateTime.Now.Year;
            string AppendStr = WeekNum.ToString() + "_" + Year.ToString() + "_";

            string appath = System.AppDomain.CurrentDomain.BaseDirectory + "Error\\";
            if (!Directory.Exists(appath))
            {
                Directory.CreateDirectory(appath);
            }

            string filePath = appath + AppendStr + "Error.txt";
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine("Message :" + msg + "<br/>" + Environment.NewLine +
                   "" + Environment.NewLine + "Date :" + DateTime.Now.ToString());
                writer.WriteLine(Environment.NewLine + "-----------------------------------------------------------------------------" + Environment.NewLine);
            }
        }

        public static void WriteMsg(string msg, string FileName)
        {
            int WeekNum = GetWeekNumber();
            int Year = DateTime.Now.Year;
            string AppendStr = WeekNum.ToString() + "_" + Year.ToString() + "_";

            string appath = System.AppDomain.CurrentDomain.BaseDirectory + "Error\\";
            if (!Directory.Exists(appath))
            {
                Directory.CreateDirectory(appath);
            }
            string filePath = appath + AppendStr + FileName + ".txt";
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine("Message :" + msg + "<br/>" + Environment.NewLine +
                   "" + Environment.NewLine + "Date :" + DateTime.Now.ToString());
                writer.WriteLine(Environment.NewLine + "-----------------------------------------------------------------------------" + Environment.NewLine);
            }
        }

/*        public static byte[] exportpdf(EmailData ed, string Remark)
        {
            //HttpServerUtility Server = new HttpServerUtility();
            ed.HeaderNote = ed.HeaderNote.Replace("\\n", "\n");
            ed.HeaderNote = ed.HeaderNote.Replace("\\r", "\r");
            ed.HeaderNote = ed.HeaderNote.Replace("\\t", "\t");
            DataTable dtData = ed.DTLines;
            // creating document object  
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            iTextSharp.text.Rectangle rec = new iTextSharp.text.Rectangle(PageSize.A4);
            rec.BackgroundColor = new BaseColor(System.Drawing.Color.Olive);
            Document doc = new Document(rec, 0, 0, 0, 0);
            doc.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
            PdfWriter writer = PdfWriter.GetInstance(doc, ms);
            doc.Open();

            //Creating paragraph for header  
            BaseFont bfntHead = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            iTextSharp.text.Font fntHead = new iTextSharp.text.Font(bfntHead, 9, 0, iTextSharp.text.BaseColor.BLACK);
            Paragraph prgHeading = new Paragraph();
            prgHeading.Alignment = Element.ALIGN_LEFT;
            prgHeading.IndentationLeft = 18f;
            prgHeading.SpacingAfter = 10f;
            prgHeading.SpacingBefore = 15f;
            prgHeading.Add(new Chunk(ed.HeaderNote, fntHead));
            doc.Add(prgHeading);

            //Adding  PdfPTable  
            PdfPTable table = new PdfPTable(dtData.Columns.Count - 1);
            table.WidthPercentage = 96f;
            table.DefaultCell.Border = Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER;

            int idxCalcCityTotal = -1;
            for (int i = 0; i < dtData.Columns.Count; i++)
            {
                if (dtData.Columns[i].ColumnName == "Qty Recevied")
                {
                    idxCalcCityTotal = i;
                }

                if (dtData.Columns[i].ColumnName != "CalcCityTotal")
                {
                    string cellText = HttpUtility.HtmlDecode(dtData.Columns[i].ColumnName);
                    PdfPCell cell = new PdfPCell();
                    //cell.Phrase = new Phrase(cellText, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 10, 1, new iTextSharp.text.BaseColor(System.Drawing.ColorTranslator.FromHtml("#000000"))));
                    //cell.Phrase = new Phrase(cellText, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 8, 1, new iTextSharp.text.BaseColor(System.Drawing.ColorTranslator.FromHtml("#bb893c"))));
                    cell.Phrase = new Phrase(cellText, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 8, 1, iTextSharp.text.BaseColor.BLACK));
                    cell.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fff"));

                    //cell.Phrase = new Phrase(cellText, new Font(Font.FontFamily.TIMES_ROMAN, 10, 1, new BaseColor(grdStudent.HeaderStyle.ForeColor)));  
                    //cell.BackgroundColor = new BaseColor(grdStudent.HeaderStyle.BackColor);  
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.PaddingBottom = 5;
                    cell.Border = Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER;
                    table.AddCell(cell);
                }
            }

            Font fontCell = new Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 8, Font.NORMAL);

            decimal TotalQtyRecevied = 0, QtyRecevied = 0;
            decimal CityWiseQtyRecevied = 0;
            //writing table Data
            for (int i = 0; i < dtData.Rows.Count; i++)
            {
                int CalcCityTotal = 0;
                if (i > 0)
                {
                    int.TryParse(dtData.Rows[i]["CalcCityTotal"].ToString(), out CalcCityTotal);
                }

                if (CalcCityTotal == 1)
                {
                    for (int j = 0; j < dtData.Columns.Count - 1; j++)
                    {
                        if (idxCalcCityTotal == j)
                        {
                            //System.Net.WebUtility.HtmlDecode();
                            string cellText = HttpUtility.HtmlDecode(CityWiseQtyRecevied.ToString());
                            PdfPCell cell = new PdfPCell();
                            cell.Phrase = new Phrase(cellText, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 8, 0, iTextSharp.text.BaseColor.BLACK));
                            cell.Border = Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER;
                            table.AddCell(cell);
                        }
                        else if (idxCalcCityTotal - 1 == j)
                        {
                            string cellText = HttpUtility.HtmlDecode("Total ");
                            PdfPCell cell = new PdfPCell();
                            cell.Phrase = new Phrase(cellText, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 8, 0, iTextSharp.text.BaseColor.BLACK));
                            cell.Border = Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER;
                            table.AddCell(cell);
                        }
                        else
                        {
                            string cellText = HttpUtility.HtmlDecode("");
                            PdfPCell cell = new PdfPCell();
                            cell.Phrase = new Phrase(cellText, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 8, 0, iTextSharp.text.BaseColor.BLACK));
                            cell.Border = Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER;
                            table.AddCell(cell);
                        }
                    }

                    CityWiseQtyRecevied = 0;
                }

                for (int j = 0; j < dtData.Columns.Count - 1; j++)
                {
                    //table.AddCell(dtData.Rows[i][j].ToString());
                    string cellText = HttpUtility.HtmlDecode(dtData.Rows[i][j].ToString());
                    PdfPCell cell = new PdfPCell();
                    cell.Phrase = new Phrase(cellText, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 8, 0, iTextSharp.text.BaseColor.BLACK));
                    cell.Border = Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER;
                    table.AddCell(cell);
                    //table.AddCell(new PdfPCell(new Phrase(dtData.Rows[i][j].ToString(), fontCell)));
                }

                decimal.TryParse(dtData.Rows[i]["Qty Recevied"].ToString(), out QtyRecevied);
                TotalQtyRecevied = TotalQtyRecevied + QtyRecevied;
                CityWiseQtyRecevied = CityWiseQtyRecevied + QtyRecevied;
            }

            for (int j = 0; j < dtData.Columns.Count - 1; j++)
            {
                if (idxCalcCityTotal == j)
                {
                    string cellText = HttpUtility.HtmlDecode(CityWiseQtyRecevied.ToString());
                    PdfPCell cell = new PdfPCell();
                    cell.Phrase = new Phrase(cellText, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 8, 0, iTextSharp.text.BaseColor.BLACK));
                    cell.Border = Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER;
                    table.AddCell(cell);
                }
                else if (idxCalcCityTotal - 1 == j)
                {
                    string cellText = HttpUtility.HtmlDecode("Total ");
                    PdfPCell cell = new PdfPCell();
                    cell.Phrase = new Phrase(cellText, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 8, 0, iTextSharp.text.BaseColor.BLACK));
                    cell.Border = Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER;
                    table.AddCell(cell);
                }
                else
                {
                    string cellText = HttpUtility.HtmlDecode("");
                    PdfPCell cell = new PdfPCell();
                    cell.Phrase = new Phrase(cellText, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 8, 0, iTextSharp.text.BaseColor.BLACK));
                    cell.Border = Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER;
                    table.AddCell(cell);
                }
            }

            doc.Add(table);

            string Foot = "Total Qty Recevied : " + TotalQtyRecevied.ToString();
            Paragraph prgFoot = new Paragraph();
            prgFoot.Alignment = Element.ALIGN_RIGHT;
            prgFoot.IndentationRight = 18f;
            prgFoot.Add(new Chunk(Foot, fntHead));
            doc.Add(prgFoot);

            doc.Add(new Chunk("\n"));

            if (Remark.Trim() != "")
            {
                Remark = "Remarks :\n" + Remark;
                Paragraph prgRemark = new Paragraph();
                prgRemark.IndentationLeft = 18f;
                prgRemark.Add(new Chunk(Remark, fntHead));
                doc.Add(prgRemark);
            }

            doc.Close();

            byte[] result = ms.ToArray();
            return result;
        }
*/
        public static string NumberToWord(string Amount, string MainCurrency = "", string SubCurrency = "")
        {
            string passstr1 = Amount;
            try
            {
                string[] a = new string[15];
                string[] A1 = new string[15];
                string[] b = new string[15];
                string[] c = new string[15];
                string toword;
                string FIN_STR;
                string STR2;
                string str3;
                int COUNTER;
                int k1;
                int tmp;
                int endcounter;
                int ijk;
                int pos;
                long amt;
                double str1;

                a[1] = "One";
                a[2] = "Two";
                a[3] = "Three";
                a[4] = "Four";
                a[5] = "Five";
                a[6] = "Six";
                a[7] = "Seven";
                a[8] = "Eight";
                a[9] = "Nine";
                a[10] = "Ten";
                A1[1] = "Eleven";
                A1[2] = "Twelve";
                A1[3] = "Thirteen";
                A1[4] = "Fourteen";
                A1[5] = "Fifteen";
                A1[6] = "Sixteen";
                A1[7] = "Seventeen";
                A1[8] = "Eighteen";
                A1[9] = "Nineteen";
                A1[10] = "Twenty";
                b[1] = "Ten";
                b[2] = "Twenty";
                b[3] = "Thirty";
                b[4] = "Fourty";
                b[5] = "Fifty";
                b[6] = "Sixty";
                b[7] = "Seventy";
                b[8] = "Eighty";
                b[9] = "Ninty";
                c[1] = "Hundred";
                c[2] = "Thousand";
                c[3] = "Lac";
                c[4] = "Crore";
                FIN_STR = MainCurrency + " ";

                str3 = passstr1.ToString();
                pos = str3.CompareTo(".");
                if (pos != 0)
                {
                    if (decimal.Parse(str3) - Math.Round(decimal.Parse(str3)) != 0)
                    {
                        endcounter = 2;
                    }
                    else
                    {
                        endcounter = 1;
                        str3 = (Math.Round(decimal.Parse(str3))).ToString();
                        pos = str3.Length;
                    }
                }
                else
                {
                    endcounter = 1;
                    pos = str3.Length + 1;
                }
                for (ijk = 1; ijk <= endcounter; ijk++)
                {
                    amt = 0;
                    string[] TempStr;
                    TempStr = str3.Split('.');
                    if (ijk == 1)
                    {

                        STR2 = TempStr[0];
                    }
                    else
                    {
                        STR2 = TempStr[1];
                    }
                    COUNTER = STR2.Length - 1;
                    amt = long.Parse(STR2);
                    if (amt < 20 && amt > 10.00001)
                    {
                        k1 = int.Parse(amt.ToString()) - 10;
                        FIN_STR = FIN_STR + A1[k1];
                        COUNTER = 0;
                        STR2 = "";
                    }
                    while (COUNTER > 0)
                    {
                        str1 = Math.Pow(10, COUNTER);
                        tmp = int.Parse(STR2 == "" ? "0" : STR2) / int.Parse(str1.ToString());
                        if (tmp != 0)
                        {
                            switch (COUNTER)
                            {
                                case 1:
                                    if (decimal.Parse(STR2) < 20 && int.Parse(STR2) > 10.00001)
                                    {
                                        tmp = int.Parse(STR2) - 10;
                                        FIN_STR = FIN_STR + A1[tmp];
                                        STR2 = "";
                                        COUNTER = 0;
                                        break;
                                    }
                                    else
                                    {
                                        FIN_STR = FIN_STR + b[tmp] + " ";
                                    }
                                    break;

                                case 2:
                                    FIN_STR = FIN_STR + a[tmp] + " " + c[1] + " ";
                                    break;
                                case 3:
                                    FIN_STR = FIN_STR + a[tmp] + " " + c[2] + " ";
                                    break;
                                case 4:
                                    if (int.Parse(STR2.Substring(0, 1)) == 1 && int.Parse(STR2.Substring(1, 1)) != 0)
                                    {
                                        FIN_STR = FIN_STR + A1[int.Parse(STR2.Substring(1, 1))] + " " + c[2] + " ";
                                        COUNTER = COUNTER - 1;
                                        STR2 = STR2.Substring(1);
                                    }
                                    else
                                    {
                                        FIN_STR = FIN_STR + b[tmp] + " ";
                                        if (int.Parse(STR2.Substring(1, 1)) == 0)
                                        {
                                            FIN_STR = FIN_STR + c[2] + " ";
                                        }
                                    }
                                    break;

                                case 5:
                                    FIN_STR = FIN_STR + a[tmp] + " " + c[3] + " ";
                                    break;

                                case 6:
                                    if (int.Parse(STR2.Substring(0, 1)) == 1 && int.Parse(STR2.Substring(1, 1)) != 0)
                                    {
                                        FIN_STR = FIN_STR + A1[int.Parse(STR2.Substring(1, 1))] + " " + c[3] + " ";
                                        COUNTER = COUNTER - 1;
                                        STR2 = STR2.Substring(1);
                                    }
                                    else
                                    {
                                        FIN_STR = FIN_STR + b[tmp] + " ";
                                        if (decimal.Parse(STR2.Substring(1, 1)) == 0)
                                        {
                                            FIN_STR = FIN_STR + c[3] + " ";
                                        }
                                    }
                                    break;

                                case 7:
                                    FIN_STR = FIN_STR + a[tmp] + " " + c[4] + " ";
                                    break;
                                case 8:
                                    FIN_STR = FIN_STR + b[tmp] + " ";
                                    if (int.Parse(STR2.Substring(1, 1)) == 0)
                                    {
                                        FIN_STR = FIN_STR + " " + c[4] + " ";
                                    }
                                    break;
                            }
                        }
                        if (STR2 != "")
                        {
                            STR2 = STR2.PadLeft(STR2.Length - 1);
                            STR2 = STR2.Substring(1);
                        }
                        COUNTER = COUNTER - 1;
                    }
                    if (int.Parse((STR2 == "" ? "0" : STR2)) > 0)
                    {
                        FIN_STR = FIN_STR + a[int.Parse(STR2)];
                    }
                    if (endcounter == 2)
                    {
                        if (ijk == 1)
                        {
                            FIN_STR = FIN_STR + " And ";
                        }
                        else
                        {
                            FIN_STR = FIN_STR.TrimEnd() + " " + SubCurrency;
                        }
                    }
                }
                toword = FIN_STR + " Only";
                return toword;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public static string EnryptString(string strEncrypted)
        {
            byte[] b = System.Text.ASCIIEncoding.ASCII.GetBytes(strEncrypted);
            string encrypted = Convert.ToBase64String(b);
            return encrypted;
        }

        public static string DecryptString(string encrString)
        {
            byte[] b;
            string decrypted;
            try
            {
                b = Convert.FromBase64String(encrString);
                decrypted = System.Text.ASCIIEncoding.ASCII.GetString(b);
            }
            catch (Exception fe)
            {
                decrypted = "";
            }
            return decrypted;
        }

        public static string ConvertByteArrayToString(byte[] data)
        {
            char[] characters = data.Select(b => (char)b).ToArray();
            return new string(characters);
        }

        public IEnumerable<byte> GetBytesFromByteString(string s)
        {
            for (int index = 0; index < s.Length; index += 2)
            {
                yield return Convert.ToByte(s.Substring(index, 2), 16);
            }
        }

        public static string GetCurrentYear()
        {
            int CurrentYear = DateTime.Today.Year;
            int PreviousYear = DateTime.Today.Year - 1;
            int NextYear = DateTime.Today.Year + 1;
            string PreYear = PreviousYear.ToString();
            string NexYear = NextYear.ToString();
            string CurYear = CurrentYear.ToString();
            string CurrentFinYear = null;
            if (DateTime.Today.Month > 3)
            {
                CurrentFinYear = CurYear + "-" + NexYear;
            }
            else
            {
                CurrentFinYear = PreYear + "-" + CurYear;

            }
            return CurrentFinYear;
        }


    }

    public static class GenericUtility
    {
        public static T ConvertTo<T>(this object value)
        {
            Type t = typeof(T);

            // Get the type that was made nullable.
            Type valueType = Nullable.GetUnderlyingType(typeof(T));

            if (valueType != null)
            {
                // Nullable type.

                if (value == null)
                {
                    // you may want to do something different here.
                    return default(T);
                }
                else
                {
                    // Convert to the value type.
                    object result = Convert.ChangeType(value, valueType);

                    // Cast the value type to the nullable type.
                    return (T)result;
                }
            }
            else
            {
                // Not nullable.
                try
                {
                    return (T)Convert.ChangeType(value, typeof(T));
                }
                catch
                {
                    return default(T);
                }
            }
        }


        #region honey
        public static DataSet ToDataSet<T>(this IList<T> list)
        {
            Type elementType = typeof(T);
            DataSet ds = new DataSet();
            DataTable t = new DataTable();
            ds.Tables.Add(t);

            //add a column to table for each public property on T
            foreach (var propInfo in elementType.GetProperties())
            {
                Type ColType = Nullable.GetUnderlyingType(propInfo.PropertyType) ?? propInfo.PropertyType;

                t.Columns.Add(propInfo.Name, ColType);
            }

            //go through each property on T and add each value to the table
            foreach (T item in list)
            {
                DataRow row = t.NewRow();

                foreach (var propInfo in elementType.GetProperties())
                {
                    row[propInfo.Name] = propInfo.GetValue(item, null) ?? DBNull.Value;
                }

                t.Rows.Add(row);
            }

            return ds;
        }
        #endregion

    }
}
