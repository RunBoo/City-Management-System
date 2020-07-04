using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace runbo
{
    public partial class QueryResult : DevExpress.XtraEditors.XtraForm
    {
        public QueryResult(DataTable pDataTable,int count)
        {
            InitializeComponent();
            dataGridView1.DataSource = pDataTable;
            textBox1.Text = "\r\n" + "共查询到" + count + "个要素，如下所示：";
        }

        private void QueryResult_Load(object sender, EventArgs e)
        {

        }
    }
}