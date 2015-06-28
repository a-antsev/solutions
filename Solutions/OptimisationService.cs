using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;

namespace Solutions
{
    public class OptimisationService
    {

        public Dictionary<string, int> FirstColumn(string filename)
        {
            Application xlsApp = new Application();

            //Displays Excel so you can see what is happening
            //xlsApp.Visible = true;

            Workbook wb = xlsApp.Workbooks.Open(filename,
                0, true, 5, "", "", true, XlPlatform.xlWindows, "\t", false, false, 0, true);
            Sheets sheets = wb.Worksheets;
            Worksheet ws = (Worksheet)sheets.Item[1];

            Range firstColumn = ws.UsedRange.Columns[1];
            Range secondColumn = ws.UsedRange.Columns[2];
            var keys = (Array)firstColumn.Cells.Value;
            var values = (Array) secondColumn.Cells.Value;
            var keysArray = keys.OfType<object>().Select(o => o.ToString()).ToArray();
            var valuesArray = values.OfType<object>().Select(o => (int)o).ToArray();
            var result = new Dictionary<string, int>();
            for (int i = 0; i < keysArray.Count(); i++)
            {
                result.Add(keysArray[i],valuesArray[i]);
            }
            return result;
        }
    }
}
