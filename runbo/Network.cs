using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.NetworkAnalysis;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Analyst3D;
using ESRI.ArcGIS.Animation;
using ESRI.ArcGIS.GlobeCore;
using System.Drawing.Drawing2D;



namespace runbo
{
    public partial class Network : Form
    {
        
        //几何网络
        private IGeometricNetwork mGeometricNetwork;
        //给定点的集合
        private IPointCollection mPointCollectionNet;
        //获取给定点最近的Network元素
        private IPointToEID mPointToEID;

        //返回结果变量
        private IEnumNetEID mEnumNetEID_Junctions;
        private IEnumNetEID mEnumNetEID_Edges;
        private double mdblPathCost;

        public IPolyline pPolyLineResult = new PolylineClass();
        public Network()
        {
            InitializeComponent();
            axMapControl1.LoadMxFile("ShpData\\nanjing.mxd");
        }
        

        private void Network_Load(object sender, EventArgs e)
        {
            //获取几何网络文件路径
            //注意修改此路径为当前存储路径
            string strPath = Application.StartupPath + @"\nanjingroad.mdb";
            //打开工作空间
            IWorkspaceFactory pWorkspaceFactory = new AccessWorkspaceFactory();
            IFeatureWorkspace pFeatureWorkspace = pWorkspaceFactory.OpenFromFile(strPath, 0) as IFeatureWorkspace;
            //获取要素数据集
            //注意名称的设置要与上面创建保持一致
            IFeatureDataset pFeatureDataset = pFeatureWorkspace.OpenFeatureDataset("road_merge");

            //获取network集合
            INetworkCollection pNetWorkCollection = pFeatureDataset as INetworkCollection;
            //获取network的数量,为零时返回
            int intNetworkCount = pNetWorkCollection.GeometricNetworkCount;
            //if (intNetworkCount < 1)
            //    return;
            //FeatureDataset可能包含多个network，我们获取指定的network
            //注意network的名称的设置要与上面创建保持一致
            mGeometricNetwork = pNetWorkCollection.get_GeometricNetworkByName("road_merge_Net");

            //将Network中的每个要素类作为一个图层加入地图控件
            IFeatureClassContainer pFeatClsContainer = mGeometricNetwork as IFeatureClassContainer;
            //获取要素类数量，为零时返回
            int intFeatClsCount = pFeatClsContainer.ClassCount;
            if (intFeatClsCount < 1)
                return;
            IFeatureClass pFeatureClass;
            IFeatureLayer pFeatureLayer;
            for (int i = 0; i < intFeatClsCount; i++)
            {
                //获取要素类
                pFeatureClass = pFeatClsContainer.get_Class(i);
                pFeatureLayer = new FeatureLayerClass();
                pFeatureLayer.FeatureClass = pFeatureClass;
                pFeatureLayer.Name = pFeatureClass.AliasName;
                //加入地图控件
                this.axMapControl1.AddLayer((ILayer)pFeatureLayer, 0);
            }

            //计算snap tolerance为图层最大宽度的1/100
            //获取图层数量
            int intLayerCount = this.axMapControl1.LayerCount;
            IGeoDataset pGeoDataset;
            IEnvelope pMaxEnvelope = new EnvelopeClass();
            for (int i = 0; i < intLayerCount; i++)
            {
                //获取图层
                pFeatureLayer = this.axMapControl1.get_Layer(i) as IFeatureLayer;
                pGeoDataset = pFeatureLayer as IGeoDataset;
                //通过Union获得较大图层范围
                pMaxEnvelope.Union(pGeoDataset.Extent);
            }
            double dblWidth = pMaxEnvelope.Width;
            double dblHeight = pMaxEnvelope.Height;
            double dblSnapTol;
            if (dblHeight < dblWidth)
                dblSnapTol = dblWidth * 0.05;
            else
                dblSnapTol = dblHeight * 0.05;

            //设置源地图，几何网络以及捕捉容差
            mPointToEID = new PointToEIDClass();
            mPointToEID.SourceMap = this.axMapControl1.Map;
            mPointToEID.GeometricNetwork = mGeometricNetwork;
            mPointToEID.SnapTolerance = dblSnapTol;

            
        }
        private void SolvePath(string weightName)
        {
            //创建ITraceFlowSolverGEN
            ITraceFlowSolverGEN pTraceFlowSolverGEN = new TraceFlowSolverClass();
            INetSolver pNetSolver = pTraceFlowSolverGEN as INetSolver;
            //初始化用于路径计算的Network
            INetwork pNetWork = mGeometricNetwork.Network;
            pNetSolver.SourceNetwork = pNetWork;

            //获取分析经过的点的个数
            int intCount = mPointCollectionNet.PointCount;
            if (intCount < 1)
                return;


            INetFlag pNetFlag;
            //用于存储路径计算得到的边
            IEdgeFlag[] pEdgeFlags = new IEdgeFlag[intCount];


            IPoint pEdgePoint = new PointClass();
            int intEdgeEID;
            IPoint pFoundEdgePoint;
            double dblEdgePercent;

            //用于获取几何网络元素的UserID, UserClassID,UserSubID
            INetElements pNetElements = pNetWork as INetElements;
            int intEdgeUserClassID;
            int intEdgeUserID;
            int intEdgeUserSubID;
            for (int i = 0; i < intCount; i++)
            {
                pNetFlag = new EdgeFlagClass();
                //获取用户点击点
                pEdgePoint = mPointCollectionNet.get_Point(i);
                //获取距离用户点击点最近的边
                mPointToEID.GetNearestEdge(pEdgePoint, out intEdgeEID, out pFoundEdgePoint, out dblEdgePercent);
                if (intEdgeEID <= 0)
                    continue;
                //根据得到的边查询对应的几何网络中的元素UserID, UserClassID,UserSubID
                pNetElements.QueryIDs(intEdgeEID, esriElementType.esriETEdge,
                    out intEdgeUserClassID, out intEdgeUserID, out intEdgeUserSubID);
                if (intEdgeUserClassID <= 0 || intEdgeUserID <= 0)
                    continue;

                pNetFlag.UserClassID = intEdgeUserClassID;
                pNetFlag.UserID = intEdgeUserID;
                pNetFlag.UserSubID = intEdgeUserSubID;
                pEdgeFlags[i] = pNetFlag as IEdgeFlag;
            }
            //设置路径求解的边
            pTraceFlowSolverGEN.PutEdgeOrigins(ref pEdgeFlags);

            //路径计算权重
            INetSchema pNetSchema = pNetWork as INetSchema;
            INetWeight pNetWeight = pNetSchema.get_WeightByName(weightName);
            if (pNetWeight == null)
                return;

            //设置权重，这里双向的权重设为一致
            INetSolverWeights pNetSolverWeights = pTraceFlowSolverGEN as INetSolverWeights;
            pNetSolverWeights.ToFromEdgeWeight = pNetWeight;
            pNetSolverWeights.FromToEdgeWeight = pNetWeight;

            object[] arrResults = new object[intCount - 1];
            //执行路径计算
            pTraceFlowSolverGEN.FindPath(esriFlowMethod.esriFMConnected, esriShortestPathObjFn.esriSPObjFnMinSum,
                out mEnumNetEID_Junctions, out mEnumNetEID_Edges, intCount - 1, ref arrResults);

            //获取路径计算总代价（cost）
            mdblPathCost = 0;
            for (int i = 0; i < intCount - 1; i++)
                mdblPathCost += (double)arrResults[i];
        }
        private IPolyline PathToPolyLine()
        {
            IPolyline pPolyLine = new PolylineClass();
            IGeometryCollection pNewGeometryCollection = pPolyLine as IGeometryCollection;
            if (mEnumNetEID_Edges == null)
                return null;

            IEIDHelper pEIDHelper = new EIDHelperClass();
            //获取几何网络
            pEIDHelper.GeometricNetwork = mGeometricNetwork;
            //获取地图空间参考
            ISpatialReference pSpatialReference = this.axMapControl1.Map.SpatialReference;
            pEIDHelper.OutputSpatialReference = pSpatialReference;
            pEIDHelper.ReturnGeometries = true;
            //根据边的ID获取边的信息
            IEnumEIDInfo pEnumEIDInfo = pEIDHelper.CreateEnumEIDInfo(mEnumNetEID_Edges);
            int intCount = pEnumEIDInfo.Count;
            pEnumEIDInfo.Reset();

            IEIDInfo pEIDInfo;
            IGeometry pGeometry;
            for (int i = 0; i < intCount; i++)
            {
                pEIDInfo = pEnumEIDInfo.Next();
                //获取边的几何要素
                pGeometry = pEIDInfo.Geometry;
                pNewGeometryCollection.AddGeometryCollection((IGeometryCollection)pGeometry);
            }
            return pPolyLine;
        }

        private void axMapControl1_OnMouseDown(object sender, IMapControlEvents2_OnMouseDownEvent e)
        {
            ////记录鼠标点击的点
            //IPoint pNewPoint = new PointClass();
            //pNewPoint.PutCoords(e.mapX, e.mapY);

            //if (mPointCollectionNet == null)
            //    mPointCollectionNet = new MultipointClass();
            ////添加点，before和after标记添加点的索引，这里不定义
            //object before = Type.Missing;
            //object after = Type.Missing;
            //mPointCollectionNet.AddPoint(pNewPoint, ref before, ref after);
        }

        private void axMapControl1_OnKeyDown(object sender, IMapControlEvents2_OnKeyDownEvent e)
        {
           
        }

        public ITextElement createTextElement(double x_longitude, double y_latitude, string text_content)
        {

            IBalloonCallout bc = CreateBalloonCallout(x_longitude, y_latitude);
            IRgbColor color_rgb = new RgbColorClass();
            ITextSymbol text_symbol = new TextSymbolClass();
            IFormattedTextSymbol ftext_symbol;
            IPoint point = new PointClass();
            ITextElement text_elt = new TextElementClass();
            double width, height;
            IElement e;

            color_rgb.Green = 255;
            text_symbol.Color = color_rgb;

            ftext_symbol = text_symbol as IFormattedTextSymbol;
            ftext_symbol.Background = bc as ITextBackground;

            //fts.Size = 8;
            text_symbol.Size = 8;

            width = this.axMapControl1.Extent.Width / 13;
            height = this.axMapControl1.Extent.Height / 20;
            point.PutCoords(x_longitude + width, y_latitude + height);

            //IMarkerElement me = new MarkerElementClass();
            text_elt.Symbol = text_symbol;
            text_elt.Text = text_content;

            e = text_elt as IElement;
            e.Geometry = point;


            return text_elt;

        }

        public IBalloonCallout CreateBalloonCallout(double x, double y)
        {
            IRgbColor color_rgb = new RgbColorClass();
            ISimpleFillSymbol simpleFillSbl = new SimpleFillSymbolClass();
            IPoint point = new PointClass(); ;
            IBalloonCallout balloonCallout = new BalloonCalloutClass(); // 气球类型的插图编号

            color_rgb.Red = 255;
            color_rgb.Green = 255;
            color_rgb.Blue = 200;

            simpleFillSbl.Color = color_rgb;
            simpleFillSbl.Style = esriSimpleFillStyle.esriSFSSolid;
            point.PutCoords(x, y);

            balloonCallout.Style = esriBalloonCalloutStyle.esriBCSRoundedRectangle;
            balloonCallout.Symbol = simpleFillSbl;
            balloonCallout.LeaderTolerance = 10;
            balloonCallout.AnchorPoint = point;

            return balloonCallout;
        }


        private void button2_Click(object sender, EventArgs e)
        {

            Form1 path1 = (Form1)this.Owner;
            

            //把查询到的点加入到点集中
            if (mPointCollectionNet == null)
                mPointCollectionNet = new MultipointClass();
            object before = Type.Missing;
            object after = Type.Missing;
            
            mPointCollectionNet.AddPoint(path1.pNewPointBegin, ref before, ref after); 
            mPointCollectionNet.AddPoint(path1.pNewPointVia1, ref before, ref after);
            mPointCollectionNet.AddPoint(path1.pNewPointVia2, ref before, ref after);
            mPointCollectionNet.AddPoint(path1.pNewPointVia3, ref before, ref after);
            mPointCollectionNet.AddPoint(path1.pNewPointEnd, ref before, ref after);
            SolvePath("lenth");
            pPolyLineResult = PathToPolyLine();

            //mPointCollectionNet.AddPoint(path1.pNewPointVia1, ref before, ref after);
            //mPointCollectionNet.AddPoint(path1.pNewPointVia2, ref before, ref after);
            //mPointCollectionNet.AddPoint(path1.pNewPointVia3, ref before, ref after);

            //路径计算
            //注意权重名称与设置保持一致
            //SolvePath("lenth");
            //路径转换为几何要素
            //pPolyLineResult1 = PathToPolyLine();
            //pPolyLineResult2 = PathToPolyLine();
            //pPolyLineResult3 = PathToPolyLine();
            //pPolyLineResult4 = PathToPolyLine();

            path1._pPolyLine = this.pPolyLineResult;
          
            try
            {

                //获取屏幕显示
                IActiveView pActiveView = this.axMapControl1.ActiveView;
                IScreenDisplay pScreenDisplay = pActiveView.ScreenDisplay;
                //设置显示符号
                ILineSymbol pLineSymbol = new CartographicLineSymbolClass();
                IRgbColor pColor = new RgbColorClass();
                pColor.Red = 255;
                pColor.Green = 0;
                pColor.Blue = 0;
                //设置线宽
                pLineSymbol.Width = 4;
                //设置颜色
                pLineSymbol.Color = pColor as IColor;
                //绘制线型符号
                pScreenDisplay.StartDrawing(0, 0);
                pScreenDisplay.SetSymbol((ISymbol)pLineSymbol);
                pScreenDisplay.DrawPolyline(pPolyLineResult);


                
                //注记
                ITextElement te1 = createTextElement(path1.pNewPointBegin.X, path1.pNewPointBegin.Y,path1.pQueryFilterBegin.WhereClause );
                ITextElement te2 = createTextElement(path1.pNewPointVia1.X, path1.pNewPointVia1.Y, path1.pQueryFilterVia1.WhereClause);
                ITextElement te3 = createTextElement(path1.pNewPointVia2.X, path1.pNewPointVia2.Y, path1.pQueryFilterVia2.WhereClause);
                ITextElement te4 = createTextElement(path1.pNewPointVia3.X, path1.pNewPointVia3.Y, path1.pQueryFilterVia3.WhereClause);
                ITextElement te5 = createTextElement(path1.pNewPointEnd.X, path1.pNewPointEnd.Y, path1.pQueryFilterEnd.WhereClause);
                this.axMapControl1.ActiveView.GraphicsContainer.AddElement(te1 as IElement, 1);
                this.axMapControl1.ActiveView.GraphicsContainer.AddElement(te2 as IElement, 1);
                this.axMapControl1.ActiveView.GraphicsContainer.AddElement(te3 as IElement, 1);
                this.axMapControl1.ActiveView.GraphicsContainer.AddElement(te4 as IElement, 1);
                this.axMapControl1.ActiveView.GraphicsContainer.AddElement(te5 as IElement, 1);
                this.axMapControl1.Refresh(esriViewDrawPhase.esriViewGraphics, null, null);

                pScreenDisplay.FinishDrawing();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("路径分析出现错误:" + "\r\n" + ex.Message);
            }
            //点集设为空
            mPointCollectionNet = null;

        }

    }
}
