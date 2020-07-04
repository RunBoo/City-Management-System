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
    public partial class Weather : DevExpress.XtraEditors.XtraForm
    {

        //天气查询返回结果
        private string[] s = new string[23];
        public Weather()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Weather_Load(object sender, EventArgs e)
        {
            string city = "南京";
            try
            {
                WeatherForecast.WeatherWebService w = new WeatherForecast.WeatherWebService();
                //把webservice当做一个类来操作  
                s = w.getWeatherbyCityName(city);    //以文本框内容为变量实现方法getWeatherbyCityName

                if (s[8] == "")
                {
                    MessageBox.Show("网络异常!", "提示");
                }
                else
                {
                    textBox1.Text = "南京市" + "   "+s[4];
                    pictureBox1.Image = Image.FromFile(@"Image\" + "a_"+s[8] + "");
                    textBox2.Text = "\r\n" + "\r\n" + s[6] + "\r\n" + "\r\n" + s[5];
                    textBox3.Text = "风力：" + s[7]+ "\r\n" +s[11];
                    pictureBox2.Image = Image.FromFile(@"Image\" + "b_" + s[16] + "");
                    textBox4.Text = "\r\n" + "\r\n" + s[13] + "\r\n" + "\r\n" + s[12] + "\r\n" + "\r\n" + s[14];
                    pictureBox3.Image = Image.FromFile(@"Image\" + "b_" + s[21] + "");
                    textBox5.Text = "\r\n" + "\r\n" + s[18] + "\r\n" + "\r\n" + s[17] + "\r\n" + "\r\n" + s[19];
                    
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show("网络异常，是否继续？", "提示");
            }
        }
    }
}