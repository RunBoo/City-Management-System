using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.GlobeCore;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Output;
using ESRI.ArcGIS.Geodatabase;


namespace runbo
{
    public partial class attributeTable : DevExpress.XtraEditors.XtraForm
    {

        private ILayer pLayer;
        private IFeatureLayer pFeatureLayer;
        private IFeatureClass pFeatureClass;
        private ILayerFields pLayerFields;



        public attributeTable(ILayer mLayer)
        {
            InitializeComponent();
            pLayer = mLayer;
        }

        private void attributeTable_Load(object sender, EventArgs e)
        {

            try
            {
                pFeatureLayer = pLayer as IFeatureLayer;
                pFeatureClass = pFeatureLayer.FeatureClass;
                pLayerFields = pFeatureLayer as ILayerFields;
                DataSet ds = new DataSet("dsTest");
                DataTable dt = new DataTable(pFeatureLayer.Name);

                DataColumn dc = null;
                for (int i = 0; i < pLayerFields.FieldCount; i++)
                {
                    dc = new DataColumn(pLayerFields.get_Field(i).Name);

                    dt.Columns.Add(dc);
                    dc = null;
                }
                IFeatureCursor pFeatureCursor = pFeatureClass.Search(null, false);
                IFeature pFeature = pFeatureCursor.NextFeature();
                while (pFeature != null)
                {
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < pLayerFields.FieldCount; i++)
                    {
                        if (pLayerFields.FindField(pFeatureClass.ShapeFieldName) == i)
                        {
                            dr[i] = pFeatureClass.ShapeType.ToString();
                        }
                        else
                        {
                            dr[i] = pFeature.get_Value(i);
                        }
                    }

                    dt.Rows.Add(dr);
                    pFeature = pFeatureCursor.NextFeature();

                }

                dataGridView1.DataSource = dt;

            }
            catch (System.Exception ex)
            {
                MessageBox.Show("读取属性表失败" + ex.Message);
                this.Dispose();
            }

        }
    
    }
}