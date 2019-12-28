using System;
using System.Data;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using Word = Microsoft.Office.Interop.Word;

namespace Одиноко_проживающие
{
    class Print
    {
        //public void ExcelAllSoc(BindingSource _bindingSource)
        //{
        //    Excel.Application excelApp = new Excel.Application();
        //    Excel.Workbook workBook;
        //    Excel.Worksheet workSheet;
        //    excelApp.DisplayAlerts = false;

        //    workBook = excelApp.Workbooks.Add(System.Reflection.Missing.Value);
        //    workSheet = (Excel.Worksheet)workBook.Worksheets[1];
           
        //    DataTable dt = (DataTable)_bindingSource.DataSource;

        //    workSheet.Cells[3, 1] = "п.п.";
        //    workSheet.Cells[3, 2] = "ФИО специалист";
        //    workSheet.Cells[3, 3] = "ФИО соц. работник";
        //    workSheet.Cells[3, 4] = "ФИО пенсионера";
        //    workSheet.Cells[3, 5] = "Дата рождения";
        //    workSheet.Cells[3, 6] = "Нас. пункт";
        //    workSheet.Cells[3, 7] = "Адрес";
        //    workSheet.Cells[3, 8] = "Состояние";
        //    workSheet.Cells[3, 9] = "Дата состояния";

        //    string start = null;
        //    int start_i = 0;
        //    int end_i = 0;

        //    string start_soc = null;
        //    int start_soc_i = 0;
        //    int end_soc_i = 0;

        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        workSheet.Cells[i + 4, 1] = i + 1;
        //        for (int j = 0; j < dt.Columns.Count; j++)
        //        {
        //            workSheet.Cells[i + 4, j + 2] = dt.Rows[i][j];
        //        }

        //        if (i == 0)
        //        {
        //            start = dt.Rows[i][0].ToString();
        //            start_soc = dt.Rows[i][1].ToString();
        //        }
        //        else
        //        {
        //            //специалист
        //            if (start != dt.Rows[i][0].ToString())
        //            {
        //                start = dt.Rows[i][0].ToString();

        //                end_i = i - 1;

        //                if (start_i != end_i)
        //                    workSheet.Range[workSheet.Cells[start_i + 4, 2], workSheet.Cells[end_i + 4, 2]].Merge();

        //                start_i = i;

        //                if (i + 1 == dt.Rows.Count)
        //                {
        //                    workSheet.Range[workSheet.Cells[start_i + 4, 2], workSheet.Cells[end_i + 4, 2]].Merge();
        //                }
        //            }
        //            else
        //            {
        //                if (i + 1 == dt.Rows.Count)
        //                {
        //                    workSheet.Range[workSheet.Cells[start_i + 4, 2], workSheet.Cells[i + 4, 2]].Merge();
        //                }
        //            }

        //            //соц. работник
        //            if (start_soc != dt.Rows[i][1].ToString())
        //            {
        //                start_soc = dt.Rows[i][1].ToString();

        //                end_soc_i = i - 1;

        //                if (start_soc_i != end_soc_i)
        //                    workSheet.Range[workSheet.Cells[start_soc_i + 4, 3], workSheet.Cells[end_soc_i + 4, 3]].Merge();

        //                start_soc_i = i;

        //                if (i + 1 == dt.Rows.Count)
        //                {
        //                    workSheet.Range[workSheet.Cells[start_soc_i + 4, 3], workSheet.Cells[end_soc_i + 4, 3]].Merge();
        //                }
        //            }
        //            else
        //            {
        //                if (i + 1 == dt.Rows.Count)
        //                {
        //                    workSheet.Range[workSheet.Cells[start_soc_i + 4, 3], workSheet.Cells[i + 4, 3]].Merge();
        //                }
        //            }
                    
        //        }
        //    }

        //    workSheet.Columns.AutoFit();
        //    workSheet.Columns.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;

        //    excelApp.Visible = true;
        //    excelApp.UserControl = true;
        //    //excelApp.Quit();
        //}

        //public void ExcelSoc(string spezialist, BindingSource _bindingSource)
        //{
        //    Excel.Application excelApp = new Excel.Application();
        //    Excel.Workbook workBook;
        //    Excel.Worksheet workSheet;
        //    excelApp.DisplayAlerts = false;

        //    workBook = excelApp.Workbooks.Add(System.Reflection.Missing.Value);
        //    workSheet = (Excel.Worksheet)workBook.Worksheets[1];

        //    workSheet.Cells[1, 1] = "Специалист:";
        //    workSheet.Cells[1, 2] = spezialist;
        //    DataTable dt = (DataTable)_bindingSource.DataSource;

        //    workSheet.Cells[3, 1] = "п.п.";
        //    workSheet.Cells[3, 2] = "ФИО соц. работник";
        //    workSheet.Cells[3, 3] = "ФИО пенсионера";
        //    workSheet.Cells[3, 4] = "Дата рождения";
        //    workSheet.Cells[3, 5] = "Нас. пункт";
        //    workSheet.Cells[3, 6] = "Адрес";
        //    workSheet.Cells[3, 7] = "Состояние";
        //    workSheet.Cells[3, 8] = "Дата состояния";

        //    string start = null;
        //    int start_i = 0;
        //    int end_i = 0;

        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        workSheet.Cells[i + 4, 1] = i + 1;
        //        for (int j = 0; j < dt.Columns.Count; j++)
        //        {
        //            workSheet.Cells[i + 4, j + 2] = dt.Rows[i][j];
        //        }

        //        if (i == 0)
        //        {
        //            start = dt.Rows[i][0].ToString();
        //        }
        //        else
        //        {
        //            if (start != dt.Rows[i][0].ToString())
        //            {
        //                start = dt.Rows[i][0].ToString();

        //                end_i = i - 1;

        //                if (start_i != end_i)
        //                    workSheet.Range[workSheet.Cells[start_i + 4, 2], workSheet.Cells[end_i + 4, 2]].Merge();

        //                start_i = i;

        //                if (i + 1 == dt.Rows.Count)
        //                {
        //                    workSheet.Range[workSheet.Cells[start_i + 4, 2], workSheet.Cells[end_i + 4, 2]].Merge();
        //                }
        //            }
        //            else
        //            {
        //                if (i + 1 == dt.Rows.Count)
        //                {
        //                    workSheet.Range[workSheet.Cells[start_i + 4, 2], workSheet.Cells[i + 4, 2]].Merge();
        //                }
        //            }
        //        }
        //    }

        //    workSheet.Columns.AutoFit();
        //    workSheet.Columns.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;

        //    excelApp.Visible = true;
        //    excelApp.UserControl = true;
        //    //excelApp.Quit();
        //}

        Word._Application application;
        Word._Document document;
        object _missingObj = System.Reflection.Missing.Value;
        object trueObj = true;
        object falseObj = false;

        public void Inv(StructuresAlones alones)
        {
            #region открываем шаблон
            application = new Word.Application();
            object templatePathObj = Path.Combine(Application.StartupPath, @"карта обследования ребенка-инвалида.dotx");

            try
            {
                document = application.Documents.Add(ref templatePathObj, ref _missingObj, ref _missingObj, ref _missingObj);
            }catch(Exception ex)
            {
                document.Close(ref falseObj, ref _missingObj, ref _missingObj);
                application.Quit(ref _missingObj, ref _missingObj, ref _missingObj);
                document = null;
                application = null;
                throw ex;
            }
            application.Visible = true;
            #endregion
            #region Замена
            Word.Range wordRange;
            object replaceTypeObj = Word.WdReplace.wdReplaceAll;

            for(int i = 1; i <= document.Sections.Count; i++)
            {
                wordRange = document.Sections[i].Range;

                Word.Find wordFindObj = wordRange.Find;

                //основное
                object[] wordFindParameters = new object[15] { (object)"@@Adres", _missingObj, _missingObj, _missingObj, _missingObj, _missingObj, _missingObj, _missingObj, _missingObj, (object)alones.alone.Country + " " + alones.alone.TypeUl + alones.alone.Street + " д." + alones.alone.House + " корп." + alones.alone.Apartament + " кв." + alones.alone.Housing, replaceTypeObj, _missingObj, _missingObj, _missingObj, _missingObj };
                wordFindObj.GetType().InvokeMember("Execute", BindingFlags.InvokeMethod, null, wordFindObj, wordFindParameters);

                wordFindParameters = new object[15] { (object)"@@Telephon", _missingObj, _missingObj, _missingObj, _missingObj, _missingObj, _missingObj, _missingObj, _missingObj, (object)alones.alone.Phone, replaceTypeObj, _missingObj, _missingObj, _missingObj, _missingObj };
                wordFindObj.GetType().InvokeMember("Execute", BindingFlags.InvokeMethod, null, wordFindObj, wordFindParameters);

                wordFindParameters = new object[15] { (object)"@@FIO", _missingObj, _missingObj, _missingObj, _missingObj, _missingObj, _missingObj, _missingObj, _missingObj, (object)alones.alone.Family + " " + alones.alone.Name + " " + alones.alone.Surname, replaceTypeObj, _missingObj, _missingObj, _missingObj, _missingObj };
                wordFindObj.GetType().InvokeMember("Execute", BindingFlags.InvokeMethod, null, wordFindObj, wordFindParameters);

                wordFindParameters = new object[15] { (object)"@@DateRo", _missingObj, _missingObj, _missingObj, _missingObj, _missingObj, _missingObj, _missingObj, _missingObj, (object)alones.alone.DateRo, replaceTypeObj, _missingObj, _missingObj, _missingObj, _missingObj };
                wordFindObj.GetType().InvokeMember("Execute", BindingFlags.InvokeMethod, null, wordFindObj, wordFindParameters);

                //инвалидность
                wordFindParameters = new object[15] { (object)"@@Stepen", _missingObj, _missingObj, _missingObj, _missingObj, _missingObj, _missingObj, _missingObj, _missingObj, (object)alones.invalidnost.stepen, replaceTypeObj, _missingObj, _missingObj, _missingObj, _missingObj };
                wordFindObj.GetType().InvokeMember("Execute", BindingFlags.InvokeMethod, null, wordFindObj, wordFindParameters);

                wordFindParameters = new object[15] { (object)"@@Diagnoz", _missingObj, _missingObj, _missingObj, _missingObj, _missingObj, _missingObj, _missingObj, _missingObj, (object)alones.invalidnost.diagnoz, replaceTypeObj, _missingObj, _missingObj, _missingObj, _missingObj };
                wordFindObj.GetType().InvokeMember("Execute", BindingFlags.InvokeMethod, null, wordFindObj, wordFindParameters);

                wordFindParameters = new object[15] { (object)"@@DateInv", _missingObj, _missingObj, _missingObj, _missingObj, _missingObj, _missingObj, _missingObj, _missingObj, (object)alones.invalidnost.date_start, replaceTypeObj, _missingObj, _missingObj, _missingObj, _missingObj };
                wordFindObj.GetType().InvokeMember("Execute", BindingFlags.InvokeMethod, null, wordFindObj, wordFindParameters);

                wordFindParameters = new object[15] { (object)"@@DatePereos", _missingObj, _missingObj, _missingObj, _missingObj, _missingObj, _missingObj, _missingObj, _missingObj, (object)alones.invalidnost.date_pere, replaceTypeObj, _missingObj, _missingObj, _missingObj, _missingObj };
                wordFindObj.GetType().InvokeMember("Execute", BindingFlags.InvokeMethod, null, wordFindObj, wordFindParameters);

                //родители
                wordFindParameters = new object[15] { (object)"@@Mather", _missingObj, _missingObj, _missingObj, _missingObj, _missingObj, _missingObj, _missingObj, _missingObj, (object)alones.family.fioMather, replaceTypeObj, _missingObj, _missingObj, _missingObj, _missingObj };
                wordFindObj.GetType().InvokeMember("Execute", BindingFlags.InvokeMethod, null, wordFindObj, wordFindParameters);

                wordFindParameters = new object[15] { (object)"@@Father", _missingObj, _missingObj, _missingObj, _missingObj, _missingObj, _missingObj, _missingObj, _missingObj, (object)alones.family.fioFather, replaceTypeObj, _missingObj, _missingObj, _missingObj, _missingObj };
                wordFindObj.GetType().InvokeMember("Execute", BindingFlags.InvokeMethod, null, wordFindObj, wordFindParameters);
            }

            #endregion
            #region сохранение

            #endregion
        }



        private Excel.Application _application = null;
        private Excel.Workbook _workBook = null;
        private Excel.Worksheet _workSheet = null;

        public void ExcelOneSoc(BindingSource _bindingSource)
        {
            _application = new Excel.Application();
            _workBook = _application.Workbooks.Add(_missingObj);
            _workSheet = (Excel.Worksheet)_workBook.Worksheets.get_Item(1);
            ExcelDocument(_bindingSource);
        }

        protected void ExcelDocument(BindingSource _bindingSource)
        {
            object pathToTemplateObj = Path.Combine(Application.StartupPath, @"1 СОЦ.xltx");

            _application = new Excel.Application();
            _workBook = _application.Workbooks.Add(pathToTemplateObj);
            _workSheet = (Excel.Worksheet)_workBook.Worksheets.get_Item(1);
            DataTable dt = (DataTable)_bindingSource.DataSource;
            
            try
            {
                int row = 0;
                foreach(var cell in dt.Rows[0].ItemArray)
                {
                    _workSheet.Cells[row + 4, 4] = cell;
                    row++;
                }
            }catch(Exception)
            {
                Close();
            }
            Visible = true;
        }

        protected bool Visible
        {
            get
            {
                return _application.Visible;
            }
            set
            {
                _application.Visible = value;
            }
        }

        protected void Close()
        {
            _workBook.Close(false, _missingObj, _missingObj);

            _application.Quit();

            System.Runtime.InteropServices.Marshal.ReleaseComObject(_application);

            _application = null;
            _workBook = null;
            _workSheet = null;

            System.GC.Collect();
        }

    }
}
