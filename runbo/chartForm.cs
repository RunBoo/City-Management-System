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

using ESRI.ArcGIS.Geoprocessing;
using ESRI.ArcGIS.Geoprocessor;
using ESRI.ArcGIS.AnalysisTools;

using ESRI.ArcGIS.NetworkAnalysis;
using ESRI.ArcGIS.DataSourcesGDB;

namespace runbo
{
    public partial class chartForm : DevExpress.XtraEditors.XtraForm
    {
        private AxMapControl axMap;
        public chartForm(AxMapControl axm)
        {
            axMap = axm;
            InitializeComponent();
        }

        private void chartForm_Load(object sender, EventArgs e)
        {
            IFeatureCursor pFeatureCursor;
            IFeatureClass pFeatureClass;
            IFeature pFeature;
            int sch0 = 0, sch1 = 0, sch2 = 0, sch3 = 0, sch6 = 0, sch8 = 0, sch9 = 0;
            try
            {
                pFeatureClass = (GetLayerByName("学校_point") as IFeatureLayer).FeatureClass;
                pFeatureCursor = pFeatureClass.Search(null, true);
                pFeature = pFeatureCursor.NextFeature();
                int index = pFeatureClass.FindField("KIND");
                string kind;

                ////使图层处于编辑状态
                //IDataset dataset = (IDataset)pFeatureClass;
                //IWorkspace workspace = dataset.Workspace;
                //IWorkspaceEdit workspaceedit = (IWorkspaceEdit)workspace;
                //workspaceedit.StartEditing(true);
                //workspaceedit.StartEditOperation();


                while (pFeature != null)
                {
                    kind = pFeature.get_Value(index).ToString();
                    switch (kind)
                    {
                        case "A700":
                            sch0++;
                            //pFeature.set_Value(pFeature.Fields.FindField("POP"),30000);
                            break;
                        case "A701":
                            sch1++;
                            //pFeature.set_Value(pFeature.Fields.FindField("POP"), 300);
                            break;
                        case "A702":
                            sch2++;
                            //pFeature.set_Value(pFeature.Fields.FindField("POP"), 500);
                            break;
                        case "A703":
                            sch3++;
                            //pFeature.set_Value(pFeature.Fields.FindField("POP"), 2000);
                            break;
                        case "A706":
                            sch6++;
                            //pFeature.set_Value(pFeature.Fields.FindField("POP"), 100);
                            break;
                        case "A708":
                            sch8++;
                            //pFeature.set_Value(pFeature.Fields.FindField("POP"), 10000);
                            break;
                        case "A709":
                            sch9++;
                            //pFeature.set_Value(pFeature.Fields.FindField("POP"), 5000);
                            break;
                    }
                    pFeature = pFeatureCursor.NextFeature();
                }
                //pFeature.Store();
                ////关闭要素的编辑状态
                //workspaceedit.StopEditing(true);
                //workspaceedit.StopEditOperation();
            }


            catch (System.Exception ex)
            {

            }

            List<string> xData = new List<string>() { "大学", "幼教", "小学", "中学", "职高", "成人教育", "驾校" };
            List<int> yData = new List<int>() { sch0, sch1, sch2, sch3, sch6, sch8, sch9 };
            chart1.Series[0]["PieLabelStyle"] = "OutSide";
            chart1.Series[0]["PieLineColor"] = "Black";
            chart1.Series[0].Points.DataBindXY(xData, yData);

            chart2.Series[0].Points.DataBindXY(xData, yData);
        }

        private IFeatureLayer GetLayerByName(string selectedLayerName)
        {
            IFeatureLayer pFeatureLayer = null;
            IFeatureLayer reFeatureLayer = null;

            try
            {
                for (int i = 0; i < axMap.LayerCount; i++)
                {
                    pFeatureLayer = (IFeatureLayer)axMap.get_Layer(i);
                    if (pFeatureLayer.Name == selectedLayerName)
                    {
                        reFeatureLayer = pFeatureLayer;
                    }
                }
                return reFeatureLayer;
            }
            catch (System.Exception ex)
            {

            }

            return pFeatureLayer;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (chart1.Visible == true)
            {
                chart1.Visible = false;
                chart2.Visible = true;
            }
            else {
                chart1.Visible = true;
                chart2.Visible = false;
            }
        }
    }
}