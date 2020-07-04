using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;

using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.DataSourcesGDB;

namespace runbo
{
    public partial class SetValue : DevExpress.XtraEditors.XtraForm
    {
        public string name=null;
        public string ID=null;
        IFeatureClass fea_class;
        IFeature fea;
        IPoint point;
        int type = 0;//确定是新建要素还是修改要素


        public SetValue(IFeatureClass pFeatureClass, IPoint pPoint, int i, IFeature pFeature)
        {
            InitializeComponent();
            fea_class = pFeatureClass;
            point = pPoint;
            fea = pFeature;
            type = i;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            name = textBox1.Text;
            ID = textBox2.Text;

            //开启编辑状态
            IWorkspaceFactory pWorkspaceFactory;
            pWorkspaceFactory = new AccessWorkspaceFactoryClass();
            IFeatureWorkspace pFeatureWorkspace;
            pFeatureWorkspace = pWorkspaceFactory.OpenFromFile("ShpData/Nanjing.mdb", 0) as IFeatureWorkspace;
            IWorkspaceEdit pWorkspaceEdit;
            pWorkspaceEdit = pFeatureWorkspace as IWorkspaceEdit;
            pWorkspaceEdit.StartEditing(false);
            pWorkspaceEdit.StartEditOperation();

            IFeatureClassWrite fr = fea_class as IFeatureClassWrite;

            if (type == 1)  //添加要素
            {
                //添加要素
                IFeature pFeature;
                pFeature = fea_class.CreateFeature();
                pFeature.Shape = point;

                pFeature.set_Value(2, name);
                pFeature.set_Value(3, ID);
                fr.WriteFeature(pFeature);

                System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeature);
                pFeature = null;
            }
            else if (type == 2)  //修改要素
            {
                fea.set_Value(2, name);
                fea.set_Value(3, ID);
                fr.WriteFeature(fea);
            }

            pWorkspaceEdit.StopEditOperation();
            pWorkspaceEdit.StopEditing(true);
            this.Hide();
            MessageBox.Show("操作成功！", "提示");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}