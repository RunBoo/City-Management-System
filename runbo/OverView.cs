using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.GlobeCore;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Output;


class OverView
{
    public void axMapControl1_OnExtentUpdated(AxMapControl axMapControl1, AxMapControl axMapControl2, object sender, IMapControlEvents2_OnExtentUpdatedEvent e)
    {
        //创建鹰眼中线框
        IEnvelope pEnv = (IEnvelope)e.newEnvelope;
        IRectangleElement pRectangleEle = new RectangleElementClass();
        IElement pEle = pRectangleEle as IElement;
        pEle.Geometry = pEnv;

        //设置线框的边线对象，包括颜色和线宽
        IRgbColor pColor = new RgbColorClass();
        pColor.Red = 255;
        pColor.Green = 0;
        pColor.Blue = 0;
        pColor.Transparency = 255;
        // 产生一个线符号对象 
        ILineSymbol pOutline = new SimpleLineSymbolClass();
        pOutline.Width = 2;
        pOutline.Color = pColor;

        // 设置颜色属性 
        pColor.Red = 255;
        pColor.Green = 0;
        pColor.Blue = 0;
        pColor.Transparency = 0;

        // 设置线框填充符号的属性 
        IFillSymbol pFillSymbol = new SimpleFillSymbolClass();
        pFillSymbol.Color = pColor;
        pFillSymbol.Outline = pOutline;
        IFillShapeElement pFillShapeEle = pEle as IFillShapeElement;
        pFillShapeEle.Symbol = pFillSymbol;

        // 得到鹰眼视图中的图形元素容器
        IGraphicsContainer pGra = axMapControl2.Map as IGraphicsContainer;
        IActiveView pAv = pGra as IActiveView;
        // 在绘制前，清除 axMapControl2 中的任何图形元素 
        pGra.DeleteAllElements();
        // 鹰眼视图中添加线框
        pGra.AddElement((IElement)pFillShapeEle, 0);
        // 刷新鹰眼
        pAv.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);

        

    }

    public void axMapControl2_OnMouseDown(AxMapControl axMapControl1, AxMapControl axMapControl2, object sender, IMapControlEvents2_OnMouseDownEvent e)
    {
        if (axMapControl2.Map.LayerCount != 0)
        {
            // 按下鼠标左键移动矩形框 
            if (e.button == 1)
            {
                IPoint pPoint = new PointClass();
                pPoint.PutCoords(e.mapX, e.mapY);
                IEnvelope pEnvelope = axMapControl1.Extent;
                pEnvelope.CenterAt(pPoint);
                axMapControl1.Extent = pEnvelope;
                axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);
            }
            // 按下鼠标右键绘制矩形框 
            else if (e.button == 2)
            {
                IEnvelope pEnvelop = axMapControl2.TrackRectangle();
                axMapControl1.Extent = pEnvelop;
                axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);
            }
        }
    }

    public void axMapControl2_OnMouseMove(AxMapControl axMapControl1, AxMapControl axMapControl2, object sender, IMapControlEvents2_OnMouseMoveEvent e)
    {
        // 如果不是左键按下就直接返回 
        if (e.button != 1) return;
        IPoint pPoint = new PointClass();
        pPoint.PutCoords(e.mapX, e.mapY);
        axMapControl1.CenterAt(pPoint);
        axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);

    }

    public ILayer GetOverviewLayer(AxMapControl axMapControl1, AxMapControl axMapControl2, IMap map)
    {
        
        //获取主视图的第一个图层
        ILayer pLayer = map.get_Layer(0);
        //遍历其他图层，并比较视图范围的宽度，返回宽度最大的图层
        ILayer pTempLayer = null;
        for (int i = 1; i < map.LayerCount; i++)
        {
            pTempLayer = map.get_Layer(i);
            if (pLayer.AreaOfInterest.Width < pTempLayer.AreaOfInterest.Width)
                pLayer = pTempLayer;
        }
        return pLayer;
        
    }


    public void axMapControl1_OnMapReplaced(AxMapControl axMapControl1, AxMapControl axMapControl2, object sender, IMapControlEvents2_OnMapReplacedEvent e,IEnvelope origin_ipenv)
    {
        //获取鹰眼图层
        axMapControl2.AddLayer(GetOverviewLayer(axMapControl1, axMapControl2, axMapControl1.Map));
        // 设置 MapControl 显示范围至数据的全局范围
        axMapControl2.Extent = origin_ipenv;
        // 刷新鹰眼控件地图
        axMapControl2.Refresh();
    }

    public void axMapControl1_OnFullExtentUpdated(AxMapControl axMapControl1, AxMapControl axMapControl2, object sender, IMapControlEvents2_OnFullExtentUpdatedEvent e, IEnvelope origin_ipenv)
    {
        //获取鹰眼图层
        axMapControl2.AddLayer(GetOverviewLayer(axMapControl1, axMapControl2,axMapControl1.Map));
        // 设置 MapControl 显示范围至数据的全局范围
        axMapControl2.Extent =origin_ipenv;
        // 刷新鹰眼控件地图
        axMapControl2.Refresh();

    }
        
}
