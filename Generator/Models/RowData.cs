using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneratorApp.Models;

public class RowData
{
    public List<string> Columns { get; set; }

    public RowData(List<string> columns)
    {
        Columns = columns;
    }
}
