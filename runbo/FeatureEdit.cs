using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;

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
    class FeatureEdit
    {
        private double ConvertPixelToMapUnits(IActiveView activeView, double pixelUnits)
        {
            double realWorldDiaplayExtent;
            int pixelExtent;
            double sizeOfOnePixel;
            double mapUnits;

            //获取设备中视图显示宽度，即像素个数
            pixelExtent = activeView.ScreenDisplay.DisplayTransformation.get_DeviceFrame().right - activeView.ScreenDisplay.DisplayTransformation.get_DeviceFrame().left;
            //获取地图坐标系中地图显示范围
            realWorldDiaplayExtent = activeView.ScreenDisplay.DisplayTransformation.VisibleBounds.Width;
            //每个像素大小代表的实际距离
            sizeOfOnePixel = realWorldDiaplayExtent / pixelExtent;
            //地理距离
            mapUnits = pixelUnits * sizeOfOnePixel;

            return mapUnits;
        }
        //删除要素
        public void DeleteFeature(AxMapControl axMapControl1, IPoint pPoint, IActiveView pActiveView)
        {
            IFeatureLayer pFeatureLayer;
            IFeatureClass pFeatureClass;
            //获取图层和要素类，为空时返回
            pFeatureLayer = axMapControl1.Map.get_Layer(0) as IFeatureLayer;
            pFeatureClass = pFeatureLayer.FeatureClass;

            double length;
            //2个像素大小的屏幕距离转换为地图距离
            length = ConvertPixelToMapUnits(pActiveView, 20);

            ITopologicalOperator pTopoOperator;
            IGeometry pGeoBuffer;
            ISpatialFilter pSpatialFilter;
            //根据缓冲半径生成空间过滤器
            pTopoOperator = pPoint as ITopologicalOperator;
            pGeoBuffer = pTopoOperator.Buffer(length);
            pSpatialFilter = new SpatialFilterClass();
            pSpatialFilter.Geometry = pGeoBuffer;

            //根据图层类型选择缓冲方式
            switch (pFeatureClass.ShapeType)
            {
                case esriGeometryType.esriGeometryPoint:
                    pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelContains;
                    break;
                case esriGeometryType.esriGeometryPolyline:
                    pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelCrosses;
                    break;
                case esriGeometryType.esriGeometryPolygon:
                    pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                    break;
            }

            //定义空间过滤器的空间字段
            pSpatialFilter.GeometryField = pFeatureClass.ShapeFieldName;
            IQueryFilter pQueryFilter;
            IFeatureCursor pFeatureCursor;
            IFeature pFeature;
            //利用要素过滤器查询要素
            pQueryFilter = pSpatialFilter as IQueryFilter;
            pFeatureCursor = pFeatureLayer.Search(pQueryFilter, true);
            pFeature = pFeatureCursor.NextFeature();

            //开启编辑状态
            IWorkspaceFactory pWorkspaceFactory;
            pWorkspaceFactory = new AccessWorkspaceFactoryClass();
            IFeatureWorkspace pFeatureWorkspace;
            pFeatureWorkspace = pWorkspaceFactory.OpenFromFile("ShpData/Nanjing.mdb", 0) as IFeatureWorkspace;
            IWorkspaceEdit pWorkspaceEdit;
            pWorkspaceEdit = pFeatureWorkspace as IWorkspaceEdit;
            pWorkspaceEdit.StartEditing(false);
            pWorkspaceEdit.StartEditOperation();

            if (pFeature != null)
            {
                //删除所选要素
                pFeature.Delete();
            }
            else
            {
                MessageBox.Show("没有选中要素！","提示");
                return;
            }

            DialogResult iResponse;
            iResponse = MessageBox.Show("是否确定删除？", "删除商场数据", MessageBoxButtons.YesNo);
            if (iResponse == DialogResult.No)
            {
                pWorkspaceEdit.UndoEditOperation();
            }
            pWorkspaceEdit.StopEditOperation();
            pWorkspaceEdit.StopEditing(true);
            //刷新图层
            axMapControl1.Refresh();
        }

        //添加要素
        public void AddFeature(AxMapControl axMapControl1, IPoint pPoint)
        {
            IFeatureLayer pFeatureLayer;
            IFeatureClass pFeatureClass;
            //获取图层和要素类，为空时返回
            pFeatureLayer = axMapControl1.Map.get_Layer(0) as IFeatureLayer;
            pFeatureClass = pFeatureLayer.FeatureClass;

            IFeature pFeature=null;
            //用户输入新建商城的名字和标识码，在该类中完成属性编辑
            SetValue set_v = new SetValue(pFeatureClass, pPoint,1,pFeature);
            set_v.Show();

        }

        //编辑要素属性
        public void EditFeature(AxMapControl axMapControl1, IPoint pPoint,IActiveView pActiveView)
        {
            IFeatureLayer pFeatureLayer;
            IFeatureClass pFeatureClass;
            //获取图层和要素类，为空时返回
            pFeatureLayer = axMapControl1.Map.get_Layer(0) as IFeatureLayer;
            pFeatureClass = pFeatureLayer.FeatureClass;

            double length;
            //2个像素大小的屏幕距离转换为地图距离
            length = ConvertPixelToMapUnits(pActiveView, 20);

            ITopologicalOperator pTopoOperator;
            IGeometry pGeoBuffer;
            ISpatialFilter pSpatialFilter;
            //根据缓冲半径生成空间过滤器
            pTopoOperator = pPoint as ITopologicalOperator;
            pGeoBuffer = pTopoOperator.Buffer(length);
            pSpatialFilter = new SpatialFilterClass();
            pSpatialFilter.Geometry = pGeoBuffer;

            //根据图层类型选择缓冲方式
            switch (pFeatureClass.ShapeType)
            {
                case esriGeometryType.esriGeometryPoint:
                    pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelContains;
                    break;
                case esriGeometryType.esriGeometryPolyline:
                    pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelCrosses;
                    break;
                case esriGeometryType.esriGeometryPolygon:
                    pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                    break;
            }

            //定义空间过滤器的空间字段
            pSpatialFilter.GeometryField = pFeatureClass.ShapeFieldName;
            IQueryFilter pQueryFilter;
            IFeatureCursor pFeatureCursor;
            IFeature pFeature;
            //利用要素过滤器查询要素
            pQueryFilter = pSpatialFilter as IQueryFilter;
            pFeatureCursor = pFeatureLayer.Search(pQueryFilter, true);
            pFeature = pFeatureCursor.NextFeature();

            if (pFeature != null)
            {
                //用户输入新的属性值，在该类中进行操作
                SetValue set_v = new SetValue(pFeatureClass, pPoint, 2, pFeature);
                set_v.Show();
            }
            else
            {
                MessageBox.Show("没有选中要素！", "提示");
                return;
            }


        }

    }
}
