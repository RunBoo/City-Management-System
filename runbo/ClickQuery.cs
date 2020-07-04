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


namespace runbo
{
    class ClickQuery
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

        //点查询
        public void Query(AxMapControl axMapControl1,IPoint pPoint,IActiveView pActiveView)
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
                //选择指定要素
                axMapControl1.Map.ClearSelection();
                axMapControl1.Map.SelectFeature((ILayer)pFeatureLayer, pFeature);
                axMapControl1.Refresh();

                string[] attribute=new string[4];
                ILayerFields pLayerFields;
                pLayerFields = pFeatureLayer as ILayerFields;
                for (int i = 0; i < pLayerFields.FieldCount; i++)
                {
                    attribute[i] = pFeature.get_Value(i).ToString();
                }

                BusinessAttribute bus_a = new BusinessAttribute(attribute);
                bus_a.Show();
            }

        }

        //其他查询
        public void LoadQueryResult(AxMapControl axMapControl1, IGeometry geometry)
        {
            IFeatureLayer pFeatureLayer;
            IFeatureClass pFeatureClass;
            //获取图层和要素类，为空时返回
            pFeatureLayer = axMapControl1.Map.get_Layer(0) as IFeatureLayer;
            pFeatureClass = pFeatureLayer.FeatureClass;

            //根据图层属性字段初始化DataTable
            IFields pFields = pFeatureClass.Fields;
            DataTable pDataTable = new DataTable();
            for (int i = 0; i < pFields.FieldCount; i++)
            {
                string strFldName;
                strFldName = pFields.get_Field(i).AliasName;
                pDataTable.Columns.Add(strFldName);
            }

            //空间过滤器
            ISpatialFilter pSpatialFilter = new SpatialFilterClass();
            pSpatialFilter.Geometry = geometry;

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

            int count = 0;
            while (pFeature != null)
            {
                string strFldValue = null;
                DataRow dr = pDataTable.NewRow();
                //遍历图层属性表字段值，并加入pDataTable
                for (int i = 0; i < pFields.FieldCount; i++)
                {
                    string strFldName = pFields.get_Field(i).Name;
                    if (strFldName == "Shape")
                    {
                        strFldValue = Convert.ToString(pFeature.Shape.GeometryType);
                    }
                    else
                        strFldValue = Convert.ToString(pFeature.get_Value(i));
                    dr[i] = strFldValue;
                }
                pDataTable.Rows.Add(dr);
                //高亮选择要素
                axMapControl1.Map.SelectFeature((ILayer)pFeatureLayer, pFeature);
                axMapControl1.ActiveView.Refresh();
                pFeature = pFeatureCursor.NextFeature();
                count++;
            }
            QueryResult qr = new QueryResult(pDataTable,count);
            qr.Show();
        }






    }
}
