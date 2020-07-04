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
    public partial class schAnaForm : DevExpress.XtraEditors.XtraForm
    {
        private AxMapControl axMap;
        public schAnaForm(AxMapControl axm)
        {
            axMap = axm;
            InitializeComponent();
        }

        private void schAnaForm_Load(object sender, EventArgs e)
        {
            //加入学校和地区选项;
            //加入地区名字进入COMBOBOX
            ILayer pLayer;
            //图层名称
            string strLayerName;
            for (int i = 4; i < this.axMap.LayerCount; i++)
            {
                pLayer = this.axMap.get_Layer(i);
                strLayerName = pLayer.Name;
                //图层名称加入ComboBox
                this.cboField.Items.Add(strLayerName);
            }

            //加入学校类型进入COMBOBOX
            for (int i = 0; i < 4; i++)
            {
                pLayer = this.axMap.get_Layer(i);
                strLayerName = pLayer.Name;
                //图层名称加入ComboBox
                this.cboSch.Items.Add(strLayerName);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            IFeature pFeature;
            IFeatureClass pFeatureClass;
            IFeatureCursor pFeatureCursor = null;
            pFeatureClass = (GetLayerByName(this.cboField.Text) as IFeatureLayer).FeatureClass;
            pFeatureCursor = pFeatureClass.Search(null, true);
            pFeature = pFeatureCursor.NextFeature();

            IFeature sFeature;
            IFeatureClass sFeatureClass;
            IFeatureCursor sFeatureCursor = null;
            sFeatureClass = (GetLayerByName(this.cboSch.Text) as IFeatureLayer).FeatureClass;
            sFeatureCursor = sFeatureClass.Search(null, true);
            sFeature = sFeatureCursor.NextFeature();

            IGeometry pTemPt;

            //空间包含查询
            IRelationalOperator pRelOpt = pFeature.Shape as IRelationalOperator;

            while (sFeature != null)
            {
                pTemPt = sFeature.ShapeCopy;
                if (pRelOpt.Contains(pTemPt))
                {
                    axMap.Map.SelectFeature((GetLayerByName(this.cboSch.Text) as IFeatureLayer), sFeature);
                    axMap.ActiveView.Refresh();
                }
                sFeature = sFeatureCursor.NextFeature();
            }

            IFeatureLayer pFeatureLayer = GetLayerByName(this.cboSch.Text);
            //缓冲距离
            double bufferDistance;
            //输入的缓冲距离转换为double
            double.TryParse("5000", out bufferDistance);

            string outputpath = "schdata\\中学_buffer.shp";
            //判断输出路径是否合法
            if (!System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(outputpath)) ||
              ".shp" != System.IO.Path.GetExtension(outputpath))
            {
                MessageBox.Show("输出路径错误!");
                return;
            }




            //获取一个geoprocessor的实例
            Geoprocessor gp = new Geoprocessor();
            //OverwriteOutput为真时，输出图层会覆盖当前文件夹下的同名图层
            gp.OverwriteOutput = true;
            string distance = "5000";
            //创建一个Buffer工具的实例
            //ESRI.ArcGIS.AnalysisTools.Buffer buffer = new ESRI.ArcGIS.AnalysisTools.Buffer(pFeatureLayer, strOutputPath, bufferDistance.ToString());
            string para = distance + " Meters";
            ESRI.ArcGIS.AnalysisTools.Buffer buffer = new ESRI.ArcGIS.AnalysisTools.Buffer();
            //IFeatureLayer pFeatureLayer = (IFeatureLayer)pEnumFeatureSetup;
            //IFeatureLayer pFeatureLayer = mMapControl.Map.FeatureSelection as IFeatureLayer;
            buffer.in_features = /*pEnumFeatureSetup*/pFeatureLayer;
            buffer.out_feature_class = outputpath;
            buffer.buffer_distance_or_field = para;
            buffer.dissolve_option = "ALL";


            //执行缓冲区分析
            IGeoProcessorResult results = null;
            //results = (IGeoProcessorResult)gp.Execute(buffer, null);

            results = (IGeoProcessorResult)gp.Execute(buffer, null);



            //判断缓冲区是否成功生成
            if (results.Status != esriJobStatus.esriJobSucceeded)
                MessageBox.Show("图层" + "缓冲区生成失败！");
            else
            {
                this.DialogResult = DialogResult.OK;
                //MessageBox.Show("缓冲区生成成功！");
                axMap.AddShapeFile("schdata", "中学_buffer.shp");

            }


            //进行叠置分析裁剪图层
            Clip(cboField.Text);

            
            
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


        private void Clip(string cityName) 
        {
            IWorkspaceFactory pWorkspaceFactory;
            //使用Geoprocessor进行Clip
            Geoprocessor gp = new Geoprocessor();
            ESRI.ArcGIS.AnalysisTools.Clip myclip = new ESRI.ArcGIS.AnalysisTools.Clip();
            IFeatureLayer pFlyr1 = new FeatureLayer();
            IFeatureLayer pFlyr2 = new FeatureLayer();
            IFeatureClass pFclass1;
            IFeatureClass pFclass2;
            //OverwriteOutput为真时，输出图层会覆盖当前文件夹下的同名图层
            gp.OverwriteOutput = true;
            string stroutputpath = "schdata\\分析结果.shp";
            //FileInfo fFile = new FileInfo(stroutputpath);

            pFlyr1 = GetLayerByName(cityName);
            pFlyr2 = GetLayerByName("中学_buffer");

            pFclass1 = pFlyr1.FeatureClass;
            pFclass2 = pFlyr2.FeatureClass;

            myclip.clip_features = pFclass2;
            myclip.in_features = pFclass1;

            myclip.out_feature_class = stroutputpath;

            gp.Execute(myclip, null);

            //if (MessageBox.Show("添加到图层？") == System.Windows.Forms.DialogResult.Yes)
            //{
            //    axMapControl1.AddShapeFile("d:\\Temp","分析结果.shp");
            //    axMapControl1.ActiveView.Refresh();
            //}

            axMap.AddShapeFile("schdata", "分析结果.shp");
            axMap.ActiveView.Refresh();
        }

    }
}