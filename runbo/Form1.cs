using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;

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
    public partial class Form1 : DevExpress.XtraBars.Ribbon.RibbonForm
    {

        #region  基础变量
        public string defaultSkinName;//皮肤
        int flag = 0;
        IEnvelope ipenv,origin_ipenv;
        OverView InstantiationOfOvewView = new OverView();  //鹰眼
        //TOCControl中Map菜单  
        private IToolbarMenu m_pMenuMap;
        //点击查询
        private int mMouseFlag=0;
        //其他查询方式
        private int mQueryMode=0;
        int juanlian = 0;

        //给定查询的起点和终点
        public IPoint pNewPointBegin = new PointClass();
        public IPoint pNewPointEnd = new PointClass();
        public IPoint pNewPointVia1 = new PointClass();
        public IPoint pNewPointVia2 = new PointClass();
        public IPoint pNewPointVia3 = new PointClass();
        
        //最佳路径
        public IPolyline _pPolyLine = new PolylineClass();

        #endregion

        public Form1()
        {
            InitializeComponent();

            //导入数据
            axMapControl1.LoadMxFile("ShpData/nanjing.mxd");
            axMapControl1.MousePointer = esriControlsMousePointer.esriPointerDefault;
            origin_ipenv = axMapControl1.Extent;

            barEditItem2.EditValue = "易家福超市东江分店";
            barEditItem3.EditValue = "上海华联超市丁家桥店";
            barEditItem4.EditValue = "一言一语简餐厅";
            barEditItem5.EditValue = "苏果便民店东屏东岗店";
            barEditItem6.EditValue = "苏果便民店溧水塘头村店";
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        #region 基础地理信息

        #region    基本功能操作
        //加载原始数据
        private void barButtonItem17_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            axMapControl1.LoadMxFile("ShpData/nanjing.mxd");
            axMapControl1.MousePointer = esriControlsMousePointer.esriPointerDefault;
            origin_ipenv = axMapControl1.Extent;
        }

        //拖曳
        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            axMapControl1.MousePointer = esriControlsMousePointer.esriPointerPan;
            flag+=1;
        }

        //放大
        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ipenv = axMapControl1.Extent;
            ipenv.Expand(0.5, 0.5, true);
            axMapControl1.Extent = ipenv;
        }

        //缩小
        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ipenv = axMapControl1.Extent;
            ipenv.Expand(2, 2, true);
            axMapControl1.Extent = ipenv;
        }

        //还原
        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            axMapControl1.Extent = origin_ipenv;
        }

        //清空地图
        private void barButtonItem13_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            axMapControl1.Map.ClearLayers();
            axTOCControl1.SetBuddyControl(null);
            axMapControl1.Refresh();
            axTOCControl1.SetBuddyControl(axMapControl1);
        }

        private void axMapControl1_OnMouseDown(object sender, IMapControlEvents2_OnMouseDownEvent e)
        {
            if (flag % 2 == 1)
            {
                axMapControl1.Pan();
            }
            else
            {
                axMapControl1.MousePointer = esriControlsMousePointer.esriPointerDefault;
            }

            IGeometry pGeometry=null;
            IActiveView pActiveView;
            IPoint pPoint;
            //获取视图范围
            pActiveView = axMapControl1.ActiveView;
            //获取鼠标点击屏幕坐标
            pPoint = pActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(e.x, e.y);
            //点击查询
            ClickQuery c_query = new ClickQuery();
            //数据编辑
            FeatureEdit f_edit = new FeatureEdit();
            if (mMouseFlag == 1)
            {
                //传入数据
                c_query.Query(axMapControl1, pPoint, pActiveView);
                mMouseFlag = 10;
                
            }
            else if (mMouseFlag == 2)
            {
                //删除数据
                f_edit.DeleteFeature(axMapControl1, pPoint, pActiveView);
                mMouseFlag = 10;

            }
            else if (mMouseFlag == 3)
            {
                //添加数据
                f_edit.AddFeature(axMapControl1, pPoint);
                mMouseFlag = 10;
            }
            else if (mMouseFlag == 4)
            {
                //修改数据
                f_edit.EditFeature(axMapControl1, pPoint, pActiveView);
                mMouseFlag = 10;
            }
            
            if (this.mQueryMode == 2)//矩形查询
            {
                pGeometry = this.axMapControl1.TrackRectangle();
                c_query.LoadQueryResult(axMapControl1, pGeometry);
                this.mQueryMode = 4;
            }
            else if (this.mQueryMode == 1)//线查询
            {
                pGeometry = this.axMapControl1.TrackLine();
                c_query.LoadQueryResult(axMapControl1, pGeometry);
                this.mQueryMode = 4;
            }
            else if (this.mQueryMode == 3)//圆查询
            {
                pGeometry = this.axMapControl1.TrackCircle();
                c_query.LoadQueryResult(axMapControl1, pGeometry);
                this.mQueryMode = 4;
            }
        }

        //TOCControl打开属性表
        private void axTOCControl1_OnMouseDown(object sender, ITOCControlEvents_OnMouseDownEvent e)
        {
            IBasicMap pMap = null;
            ILayer pLayer = null;
            object legendgp = null;
            object index = null;
            esriTOCControlItem pItem = esriTOCControlItem.esriTOCControlItemNone;
            try
            {
                axTOCControl1.HitTest(e.x, e.y, ref pItem, ref pMap, ref pLayer, ref legendgp, ref index);
            }
            catch (Exception)
            {
                throw;
            }
            switch (e.button)
            {
                case 2:
                    if (pItem == esriTOCControlItem.esriTOCControlItemMap)
                    {
                        axTOCControl1.SelectItem(axMapControl1, null);
                    }
                    else
                    {
                        axTOCControl1.SelectItem(pLayer, null);
                    }

                    //将该图层设置为地图控件的自定义属性;
                    axMapControl1.CustomProperty = pLayer;
                    //弹出右键菜单
                    if (pItem == esriTOCControlItem.esriTOCControlItemMap)
                    {

                        m_pMenuMap.PopupMenu(e.x, e.y, axTOCControl1.hWnd);

                    }
                    if (pItem == esriTOCControlItem.esriTOCControlItemLayer)
                    {
                        m_pMenuMap = new ToolbarMenuClass();
                        m_pMenuMap.AddItem(new OpenAttribute(pLayer), -1, 0, true, esriCommandStyles.esriCommandStyleTextOnly);
                        m_pMenuMap.SetHook(axMapControl1);
                        m_pMenuMap.PopupMenu(e.x, e.y, axTOCControl1.hWnd);
                    }
                    break;
            }
        }

        //显示在线地图
        private void barButtonItem32_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.axMapControl1.Visible = false;
            this.webBrowser1.Visible = true;
            Uri url = new Uri(Application.StartupPath + @"/travel.html");
            this.webBrowser1.Url = url;

        }
        //清楚在线地图
        private void barButtonItem33_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.webBrowser1.Visible = false;
            this.axMapControl1.Visible = true;
        }
        #endregion

        #region    鹰眼

        private void axMapControl1_OnExtentUpdated(object sender, IMapControlEvents2_OnExtentUpdatedEvent e)
        {
            InstantiationOfOvewView.axMapControl1_OnExtentUpdated(axMapControl1, axMapControl2, sender, e);
        }

        private void axMapControl2_OnMouseMove(object sender, IMapControlEvents2_OnMouseMoveEvent e)
        {
            InstantiationOfOvewView.axMapControl2_OnMouseMove(axMapControl1, axMapControl2, sender, e);
        }
        private ILayer GetOverviewLayer(IMap map)
        {
            return InstantiationOfOvewView.GetOverviewLayer(axMapControl1, axMapControl2, map);
        }

        private void axMapControl1_OnMapReplaced(object sender, IMapControlEvents2_OnMapReplacedEvent e)
        {
            InstantiationOfOvewView.axMapControl1_OnMapReplaced(axMapControl1, axMapControl2, sender, e, origin_ipenv);
        }

        private void axMapControl1_OnFullExtentUpdated(object sender, IMapControlEvents2_OnFullExtentUpdatedEvent e)
        {
            InstantiationOfOvewView.axMapControl1_OnFullExtentUpdated(axMapControl1, axMapControl2, sender, e, origin_ipenv);
        }

        private void axMapControl2_OnMouseDown(object sender, IMapControlEvents2_OnMouseDownEvent e)
        {
            InstantiationOfOvewView.axMapControl2_OnMouseDown(axMapControl1, axMapControl2, sender, e);
        }
        #endregion

        #region   地理要素加载

        //行政区划
        private void barButtonItem9_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            axMapControl1.LoadMxFile("ShpData/xingzheng.mxd");
            axMapControl1.MousePointer = esriControlsMousePointer.esriPointerDefault;
            origin_ipenv = axMapControl1.Extent;
        }

        //道路图
        private void barButtonItem10_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            axMapControl1.LoadMxFile("ShpData/road.mxd");
            axMapControl1.MousePointer = esriControlsMousePointer.esriPointerDefault;
            origin_ipenv = axMapControl1.Extent;
        }

        //水系图
        private void barButtonItem11_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            axMapControl1.LoadMxFile("ShpData/water.mxd");
            axMapControl1.MousePointer = esriControlsMousePointer.esriPointerDefault;
            origin_ipenv = axMapControl1.Extent;
        }

        //绿地信息
        private void barButtonItem12_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            axMapControl1.LoadMxFile("ShpData/grass.mxd");
            axMapControl1.MousePointer = esriControlsMousePointer.esriPointerDefault;
            origin_ipenv = axMapControl1.Extent;
        }
        #endregion

        #region  决策辅助

        //日历
        private void barButtonItem16_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Calendar calendar = new Calendar();
            calendar.Show();
        }

        //天气
        private void barButtonItem14_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Weather weather = new Weather();
            weather.Show();
        }
        #endregion

        #endregion

        #region  商业区管理

        //加载商业数据
        private void barButtonItem20_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            axMapControl1.LoadMxFile("ShpData/Business.mxd");
            axMapControl1.MousePointer = esriControlsMousePointer.esriPointerDefault;
            origin_ipenv = axMapControl1.Extent;
        }

        //加载底图
        private void barButtonItem18_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            axMapControl1.LoadMxFile("ShpData/nanjing.mxd");
            axMapControl1.AddShapeFile("ShpData/","超市商城_point.shp");
            axMapControl1.MousePointer = esriControlsMousePointer.esriPointerDefault;
            origin_ipenv = axMapControl1.Extent;
        }

        //删除底图
        private void barButtonItem19_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            axMapControl1.LoadMxFile("ShpData/Business.mxd");
            axMapControl1.MousePointer = esriControlsMousePointer.esriPointerDefault;
            origin_ipenv = axMapControl1.Extent;
        }

        //点击查询
        private void barButtonItem21_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            mMouseFlag = 1;
            axMapControl1.MousePointer = esriControlsMousePointer.esriPointerPencil;
            axMapControl1.Map.ClearSelection();
            axMapControl1.Refresh();
        }

        //清楚选中要素
        private void barButtonItem29_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            axMapControl1.Map.ClearSelection();
            axMapControl1.Refresh();
        }

        //线划查询
        private void barButtonItem22_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.mQueryMode = 1;
            axMapControl1.Map.ClearSelection();
            axMapControl1.Refresh();
        }
        //矩形查询
        private void barButtonItem23_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.mQueryMode = 2;
            axMapControl1.MousePointer = esriControlsMousePointer.esriPointerCrosshair;
            axMapControl1.Map.ClearSelection();
            axMapControl1.Refresh();
        }
        //圆形查询
        private void barButtonItem24_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.mQueryMode = 3;
            axMapControl1.MousePointer = esriControlsMousePointer.esriPointerCrosshair;
            axMapControl1.Map.ClearSelection();
            axMapControl1.Refresh();
        }

        //增加
        private void barButtonItem26_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            mMouseFlag = 3;
            MessageBox.Show("请点击选择要添加商场的地理位置：","提示");
            axMapControl1.MousePointer = esriControlsMousePointer.esriPointerCrosshair;
            axMapControl1.Map.ClearSelection();
            axMapControl1.Refresh();
        }
        //删除
        private void barButtonItem27_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            mMouseFlag = 2;
            MessageBox.Show("请点击选择要删除的商场：", "提示");
            axMapControl1.MousePointer = esriControlsMousePointer.esriPointerPencil;
            axMapControl1.Map.ClearSelection();
            axMapControl1.Refresh();
        }
        //修改
        private void barButtonItem28_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            mMouseFlag = 4;
            MessageBox.Show("请点击选择要修改的商场：", "提示");
            axMapControl1.MousePointer = esriControlsMousePointer.esriPointerPencil;
            axMapControl1.Map.ClearSelection();
            axMapControl1.Refresh();
        }

        #endregion
      
        #region  环保建设

        //绿地信息
        private void barButtonItem34_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            axMapControl1.LoadMxFile("ShpData/grass.mxd");
            axMapControl1.MousePointer = esriControlsMousePointer.esriPointerDefault;
            origin_ipenv = axMapControl1.Extent;
        }

        //添加数据
        private void barButtonItem35_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ICommand cmd = new ControlsAddDataCommand();
            cmd.OnCreate(this.axMapControl1.Object);
            cmd.OnClick();

        }

        private void barButtonItem36_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            BufferForm bufferForm = new BufferForm(this.axMapControl1.Object);
            if (bufferForm.ShowDialog() == DialogResult.OK)
            {
                //获取输出文件路径
                string strBufferPath = bufferForm.strOutputPath;
                //缓冲区图层载入到MapControl
                int index = strBufferPath.LastIndexOf("\\");
                this.axMapControl1.AddShapeFile(strBufferPath.Substring(0, index), strBufferPath.Substring(index));
            }

        }

        private void barButtonItem37_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OverlayForm overlayForm = new OverlayForm(this.axMapControl1.Object);
            if (overlayForm.ShowDialog() == DialogResult.OK)
            {
                string strOverlayPath = overlayForm.strOutputPath;
                int index = strOverlayPath.LastIndexOf("\\");
                this.axMapControl1.AddShapeFile(strOverlayPath.Substring(0, index), strOverlayPath.Substring(index));
            }

        }

        //卷帘效果
        ILayerEffectProperties pEffectLayer = new CommandsEnvironmentClass();
        private void barButtonItem38_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            juanlian += 1;
            if (juanlian % 2 == 1)
            {
                ILayer pSwipeLayer = axMapControl1.get_Layer(0);//获得卷帘图层
                pEffectLayer.SwipeLayer = pSwipeLayer;//设置卷帘图层
                ICommand pCommand = new ControlsMapSwipeToolClass();//调用卷帘工具
                pCommand.OnCreate(this.axMapControl1.Object);//绑定工具
                this.axMapControl1.CurrentTool = pCommand as ITool;

            }
            else
            {
                ICommand pCommand = new ControlsMapPanToolClass();//调用卷帘工具

                this.axMapControl1.CurrentTool = pCommand as ITool;
            }
            
        }

        #endregion

        #region   生活区建设

        private void barButtonItem40_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            axMapControl1.AddShapeFile("ShpData", "超市商城_point.shp");
        }

        private void barButtonItem41_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            axMapControl1.AddShapeFile("ShpData", "餐饮_point.shp");
        }

        private void barButtonItem42_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            axMapControl1.AddShapeFile("ShpData", "宾馆酒店_point.shp");
        }

        private void barButtonItem43_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            axMapControl1.AddShapeFile("ShpData", "药店_point.shp");
        }

        private void barButtonItem44_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            axMapControl1.AddShapeFile("ShpData", "银行_point.shp");
        }
        public IQueryFilter pQueryFilterBegin, pQueryFilterEnd, pQueryFilterVia1, pQueryFilterVia2, pQueryFilterVia3;
        private void barButtonItem45_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //定义图层，要素游标，查询过滤器，要素
            IFeatureLayer pFeatureLayer;
            IFeatureCursor pFeatureCursorBegin, pFeatureCursorEnd, pFeatureCursorVia1, pFeatureCursorVia2, pFeatureCursorVia3;
            
            IFeature pFeatureBegin, pFeatureEnd, pFeatureVia1, pFeatureVia2, pFeatureVia3;


            pFeatureLayer = this.axMapControl1.Map.get_Layer(0) as IFeatureLayer;
            int intLayerCount = this.axMapControl1.LayerCount;
            for (int i = 0; i < intLayerCount; i++)
            {
                //获取图层
                pFeatureLayer = this.axMapControl1.get_Layer(i) as IFeatureLayer;
                pQueryFilterBegin = new QueryFilterClass();
                pQueryFilterBegin.WhereClause = "NAME='" + barEditItem2.EditValue + "'";
                if (i == intLayerCount - 1 && pQueryFilterBegin.WhereClause == null)
                {
                    MessageBox.Show("输入地名有误！");
                    return;
                }
                else if (i <= intLayerCount - 1 && pQueryFilterBegin.WhereClause != null)
                {
                    pFeatureCursorBegin = pFeatureLayer.Search(pQueryFilterBegin, true);
                    pFeatureBegin = pFeatureCursorBegin.NextFeature();
                    if (pFeatureBegin != null)
                    {
                        pNewPointBegin = pFeatureBegin.Shape as IPoint;
                        
                        break;
                    }
                }
            }

            for (int i = 0; i < intLayerCount; i++)
            {
                //获取图层
                pFeatureLayer = this.axMapControl1.get_Layer(i) as IFeatureLayer;
                pQueryFilterVia1 = new QueryFilterClass();
                pQueryFilterVia1.WhereClause = "NAME='" + barEditItem4.EditValue + "'";
                if (i == intLayerCount - 1 && pQueryFilterVia1.WhereClause == null)
                {
                    MessageBox.Show("输入地名有误！");
                    return;
                }
                else if (i <= intLayerCount - 1 && pQueryFilterVia1.WhereClause != null)
                {
                    pFeatureCursorVia1 = pFeatureLayer.Search(pQueryFilterVia1, true);
                    pFeatureVia1 = pFeatureCursorVia1.NextFeature();
                    if (pFeatureVia1 != null)
                    {
                        pNewPointVia1 = pFeatureVia1.Shape as IPoint;
                        pNewPointVia1 = pFeatureVia1.Shape as IPoint;
                        
                        break;
                    }
                }
            }


            for (int i = 0; i < intLayerCount; i++)
            {
                //获取图层
                pFeatureLayer = this.axMapControl1.get_Layer(i) as IFeatureLayer;
                pQueryFilterVia2 = new QueryFilterClass();
                pQueryFilterVia2.WhereClause = "NAME='" + barEditItem5.EditValue + "'";
                if (i == intLayerCount - 1 && pQueryFilterVia2.WhereClause == null)
                {
                    MessageBox.Show("输入地名有误！");
                    return;
                }
                else if (i <= intLayerCount - 1 && pQueryFilterVia2.WhereClause != null)
                {
                    pFeatureCursorVia2 = pFeatureLayer.Search(pQueryFilterVia2, true);
                    pFeatureVia2 = pFeatureCursorVia2.NextFeature();
                    if (pFeatureVia2 != null)
                    {
                        pNewPointVia2 = pFeatureVia2.Shape as IPoint;
                        pNewPointVia2 = pFeatureVia2.Shape as IPoint;
                        
                        break;
                    }
                }
            }


            for (int i = 0; i < intLayerCount; i++)
            {
                //获取图层
                pFeatureLayer = this.axMapControl1.get_Layer(i) as IFeatureLayer;
                pQueryFilterVia3 = new QueryFilterClass();
                pQueryFilterVia3.WhereClause = "NAME='" + barEditItem6.EditValue + "'";
                if (i == intLayerCount - 1 && pQueryFilterVia3.WhereClause == null)
                {
                    MessageBox.Show("输入地名有误！");
                    return;
                }
                else if (i <= intLayerCount - 1 && pQueryFilterVia3.WhereClause != null)
                {
                    pFeatureCursorVia3 = pFeatureLayer.Search(pQueryFilterVia3, true);
                    pFeatureVia3 = pFeatureCursorVia3.NextFeature();
                    if (pFeatureVia3 != null)
                    {
                        pNewPointVia3 = pFeatureVia3.Shape as IPoint;
                        pNewPointVia3 = pFeatureVia3.Shape as IPoint;
                        
                        break;
                    }
                }
            }


            for (int i = 0; i < intLayerCount; i++)
            {
                //获取图层
                pFeatureLayer = this.axMapControl1.get_Layer(i) as IFeatureLayer;
                pQueryFilterEnd = new QueryFilterClass();
                pQueryFilterEnd.WhereClause = "NAME='" + barEditItem3.EditValue + "'";
                if (i == intLayerCount - 1 && pQueryFilterEnd.WhereClause == null)
                {
                    MessageBox.Show("输入地名有误！");
                    return;
                }
                else if (i <= intLayerCount - 1 && pQueryFilterEnd.WhereClause != null)
                {
                    pFeatureCursorEnd = pFeatureLayer.Search(pQueryFilterEnd, true);
                    pFeatureEnd = pFeatureCursorEnd.NextFeature();
                    if (pFeatureEnd != null)
                    {
                        pNewPointEnd = pFeatureEnd.Shape as IPoint;
                        pNewPointEnd = pFeatureEnd.Shape as IPoint;
                        
                        break;
                    }
                }
            }
            Network nw = new Network();
            nw.ShowDialog(this);
            
        }

        private void barButtonItem39_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            axMapControl1.LoadMxFile("ShpData/grass_final.mxd");
            axMapControl1.MousePointer = esriControlsMousePointer.esriPointerDefault;
            origin_ipenv = axMapControl1.Extent;
        }

        #endregion

        #region     科教区建设
        private void fileLoad_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            axMapControl1.Map.ClearLayers();
            axTOCControl1.SetBuddyControl(null);
            axMapControl1.Refresh();
            axTOCControl1.SetBuddyControl(axMapControl1);
            string IsFilePathNameNull = null;//文件路径名,用于判断路径是否为空  
            string FilePath = null;//文件路径  
            string FileName = null;//文件名称  

            try
            {
                IsFilePathNameNull = "schdata/schAna.mxd";
                FilePath = System.IO.Path.GetDirectoryName(IsFilePathNameNull);//路径  
                FileName = System.IO.Path.GetFileName(IsFilePathNameNull);//文件名  

                axMapControl1.MousePointer = esriControlsMousePointer.esriPointerHourglass;

                axMapControl1.LoadMxFile(IsFilePathNameNull);

                axMapControl1.MousePointer = esriControlsMousePointer.esriPointerDefault;
            }
            catch (Exception ex)
            {
               
            }
            
        }

        private void charView_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            chartForm cF = new chartForm(axMapControl1);
            cF.Show();
        }

        private void schAnalysis_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            schAnaForm sca = new schAnaForm(axMapControl1);
            sca.Show();
        }

        #endregion




































    }
}
