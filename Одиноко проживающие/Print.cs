using System.Data;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Одиноко_проживающие
{
    class Print
    {
        public void ExcelAllSoc(BindingSource _bindingSource)
        {
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workBook;
            Excel.Worksheet workSheet;
            excelApp.DisplayAlerts = false;

            workBook = excelApp.Workbooks.Add(System.Reflection.Missing.Value);
            workSheet = (Excel.Worksheet)workBook.Worksheets[1];
           
            DataTable dt = (DataTable)_bindingSource.DataSource;

            workSheet.Cells[3, 1] = "п.п.";
            workSheet.Cells[3, 2] = "ФИО специалист";
            workSheet.Cells[3, 3] = "ФИО соц. работник";
            workSheet.Cells[3, 4] = "ФИО пенсионера";
            workSheet.Cells[3, 5] = "Дата рождения";
            workSheet.Cells[3, 6] = "Нас. пункт";
            workSheet.Cells[3, 7] = "Адрес";
            workSheet.Cells[3, 8] = "Состояние";
            workSheet.Cells[3, 9] = "Дата состояния";

            string start = null;
            int start_i = 0;
            int end_i = 0;

            string start_soc = null;
            int start_soc_i = 0;
            int end_soc_i = 0;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                workSheet.Cells[i + 4, 1] = i + 1;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    workSheet.Cells[i + 4, j + 2] = dt.Rows[i][j];
                }

                if (i == 0)
                {
                    start = dt.Rows[i][0].ToString();
                    start_soc = dt.Rows[i][1].ToString();
                }
                else
                {
                    //специалист
                    if (start != dt.Rows[i][0].ToString())
                    {
                        start = dt.Rows[i][0].ToString();

                        end_i = i - 1;

                        if (start_i != end_i)
                            workSheet.Range[workSheet.Cells[start_i + 4, 2], workSheet.Cells[end_i + 4, 2]].Merge();

                        start_i = i;

                        if (i + 1 == dt.Rows.Count)
                        {
                            workSheet.Range[workSheet.Cells[start_i + 4, 2], workSheet.Cells[end_i + 4, 2]].Merge();
                        }
                    }
                    else
                    {
                        if (i + 1 == dt.Rows.Count)
                        {
                            workSheet.Range[workSheet.Cells[start_i + 4, 2], workSheet.Cells[i + 4, 2]].Merge();
                        }
                    }

                    //соц. работник
                    if (start_soc != dt.Rows[i][1].ToString())
                    {
                        start_soc = dt.Rows[i][1].ToString();

                        end_soc_i = i - 1;

                        if (start_soc_i != end_soc_i)
                            workSheet.Range[workSheet.Cells[start_soc_i + 4, 3], workSheet.Cells[end_soc_i + 4, 3]].Merge();

                        start_soc_i = i;

                        if (i + 1 == dt.Rows.Count)
                        {
                            workSheet.Range[workSheet.Cells[start_soc_i + 4, 3], workSheet.Cells[end_soc_i + 4, 3]].Merge();
                        }
                    }
                    else
                    {
                        if (i + 1 == dt.Rows.Count)
                        {
                            workSheet.Range[workSheet.Cells[start_soc_i + 4, 3], workSheet.Cells[i + 4, 3]].Merge();
                        }
                    }
                    
                }
            }

            workSheet.Columns.AutoFit();
            workSheet.Columns.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;

            excelApp.Visible = true;
            excelApp.UserControl = true;
            //excelApp.Quit();
        }

        public void ExcelSoc(string spezialist, BindingSource _bindingSource)
        {
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workBook;
            Excel.Worksheet workSheet;
            excelApp.DisplayAlerts = false;

            workBook = excelApp.Workbooks.Add(System.Reflection.Missing.Value);
            workSheet = (Excel.Worksheet)workBook.Worksheets[1];

            workSheet.Cells[1, 1] = "Специалист:";
            workSheet.Cells[1, 2] = spezialist;
            DataTable dt = (DataTable)_bindingSource.DataSource;

            workSheet.Cells[3, 1] = "п.п.";
            workSheet.Cells[3, 2] = "ФИО соц. работник";
            workSheet.Cells[3, 3] = "ФИО пенсионера";
            workSheet.Cells[3, 4] = "Дата рождения";
            workSheet.Cells[3, 5] = "Нас. пункт";
            workSheet.Cells[3, 6] = "Адрес";
            workSheet.Cells[3, 7] = "Состояние";
            workSheet.Cells[3, 8] = "Дата состояния";

            string start = null;
            int start_i = 0;
            int end_i = 0;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                workSheet.Cells[i + 4, 1] = i + 1;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    workSheet.Cells[i + 4, j + 2] = dt.Rows[i][j];
                }

                if (i == 0)
                {
                    start = dt.Rows[i][0].ToString();
                }
                else
                {
                    if (start != dt.Rows[i][0].ToString())
                    {
                        start = dt.Rows[i][0].ToString();

                        end_i = i - 1;

                        if (start_i != end_i)
                            workSheet.Range[workSheet.Cells[start_i + 4, 2], workSheet.Cells[end_i + 4, 2]].Merge();

                        start_i = i;

                        if (i + 1 == dt.Rows.Count)
                        {
                            workSheet.Range[workSheet.Cells[start_i + 4, 2], workSheet.Cells[end_i + 4, 2]].Merge();
                        }
                    }
                    else
                    {
                        if (i + 1 == dt.Rows.Count)
                        {
                            workSheet.Range[workSheet.Cells[start_i + 4, 2], workSheet.Cells[i + 4, 2]].Merge();
                        }
                    }
                }
            }

            workSheet.Columns.AutoFit();
            workSheet.Columns.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;

            excelApp.Visible = true;
            excelApp.UserControl = true;
            //excelApp.Quit();
        }
    }
}
