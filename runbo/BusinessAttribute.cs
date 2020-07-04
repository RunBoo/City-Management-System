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
    public partial class BusinessAttribute : DevExpress.XtraEditors.XtraForm
    {
        public BusinessAttribute(string[] attribute)
        {
            InitializeComponent();


            textBox1.Text=attribute[0];
            textBox2.Text=attribute[2];
            textBox3.Text = attribute[3];

        }

        private void BusinessAttribute_Load(object sender, EventArgs e)
        {

        }
    }
}