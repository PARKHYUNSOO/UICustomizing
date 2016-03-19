using Aveva.Marine.Drafting;
using Aveva.Marine.Geometry;
using Aveva.Marine.UI;
using Aveva.Marine.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KcsCaptureRegion2D = Aveva.Marine.Drafting.MarCaptureRegionPlanar;
using KcsContour2D = Aveva.Marine.Geometry.MarContourPlanar;
using KcsCursorType = Aveva.Marine.Drafting.MarCursorType;
using KcsElementHandle = Aveva.Marine.Drafting.MarElementHandle;
using KcsHighlightSet = Aveva.Marine.Drafting.MarHighlightSet;
using KcsPoint2D = Aveva.Marine.Geometry.MarPointPlanar;
using KcsPoint3D = Aveva.Marine.Geometry.MarPoint;
using KcsRectangle2D = Aveva.Marine.Geometry.MarRectanglePlanar;
using KcsStat_point2D_req = Aveva.Marine.UI.MarStatPointPlanarReq;
using KcsText = Aveva.Marine.Drafting.MarText;
using KcsVector2D = Aveva.Marine.Geometry.MarVectorPlanar;
using KcsVector3D = Aveva.Marine.Geometry.MarVector;
using KcsModel = Aveva.Marine.Drafting.MarModel;
using Aveva.Pdms.Database;


namespace PHS.Utilities.Library
{
    class PHS_lib
    {
        MarDrafting kcs_draft = new MarDrafting();
        MarUi kcs_ui = new MarUi();
        MarUtil kcs_util = new MarUtil();

        public List<KcsElementHandle> handler = new List<KcsElementHandle>();
        public List<KcsModel> models = new List<KcsModel>();
        public List<DbElement> dbelements = new List<DbElement>();
       
        KcsPoint2D p1 = new KcsPoint2D();
        KcsPoint2D p2 = new MarPointPlanar();        


        KcsCaptureRegion2D region = new KcsCaptureRegion2D();
        public PHS_lib()
        {



        }

        //화면상에 점을찍을때 나타나는 마우스커서모양 지정
        public KcsStat_point2D_req PointState(string cursortype, string defmode)
        {
            
            KcsPoint2D startpoint = new KcsPoint2D(0.0d, 0.0d);
            KcsPoint2D point = new KcsPoint2D();
            KcsHighlightSet highlighter=new KcsHighlightSet();
            KcsStat_point2D_req status = new KcsStat_point2D_req();
            status.DefMode = defmode;
            KcsHighlightSet highlight = new KcsHighlightSet();
            KcsCursorType Curtype = new KcsCursorType();

            if (cursortype == "CrossHair")
                Curtype.SetCrossHair();
            else if (cursortype == "RubberBand")
                Curtype.SetRubberBand(startpoint);
            else if (cursortype == "RubberRectangle")
                Curtype.SetRubberRectangle(startpoint);
            else if (cursortype == "RubberCircle")
                Curtype.SetRubberCircle(startpoint);
            else if (cursortype == "DragCursor")
                Curtype.SetDragCursor(highlighter, startpoint);
                
            status.Cursor = Curtype;
            return status;

        }
        //Pick한 점이 포함하는 객체를 Capture를 통해서 Handle값을 얻어내는 기능.
        public KcsElementHandle[] DetermineCapture(int sel)
        {
            KcsElementHandle[] temp = new KcsElementHandle[] { };
            if (sel == 0)
                temp = kcs_draft.ViewCapture(region);
            else if (sel == 1)
                temp = kcs_draft.SubviewCapture(region);
            else if (sel == 2)
                temp = kcs_draft.ComponentCapture(region);
            else if (sel == 3)
                temp = kcs_draft.ModelCapture(region);
            else if (sel == 4)
                temp = kcs_draft.DimCapture(region);
            else if (sel == 5)
                temp = kcs_draft.NoteCapture(region);
            else if (sel == 6)
                temp = kcs_draft.PosnoCapture(region);
            else if (sel == 7)
                temp = kcs_draft.HatchCapture(region);
            else if (sel == 8)
                temp = kcs_draft.GeometryCapture(region);
            else if (sel == 9)
                temp = kcs_draft.ContourCapture(region);
            else if (sel == 10)
                temp = kcs_draft.TextCapture(region);
            else if (sel == 11)
                temp = kcs_draft.SymbolCapture(region);
            else
                temp = kcs_draft.PointCapture(region);
            return temp;
        }
        // Pick한 점의 선택할 Object의 유형을 결정하는 기능 
        public KcsElementHandle DetermineIdentify(int sel)
        {
            KcsElementHandle temp = new KcsElementHandle();
            if (sel == 0)

                temp = kcs_draft.SubviewIdentify(p1);
            else if (sel == 1)
                temp = kcs_draft.ComponentIdentify(p1);
            else if (sel == 2)
                temp = kcs_draft.DimIdentify(p1);
            else if (sel == 3)
                temp = kcs_draft.NoteIdentify(p1);
            else if (sel == 4)
                temp = kcs_draft.PosnoIdentify(p1);
            else if (sel == 5)
                temp = kcs_draft.HatchIdentify(p1);
            else if (sel == 6)
                temp = kcs_draft.GeometryIdentify(p1);
            else if (sel == 7)
                temp = kcs_draft.ContourIdentify(p1);
            else if (sel == 8)
                temp = kcs_draft.TextIdentify(p1);
            else if (sel == 9)
                temp = kcs_draft.SymbolIdentify(p1);
            else if (sel == 10)
                temp = kcs_draft.PointIdentify(p1);
            return temp;

        }
        /// <summary>
        ///  하나의 뷰를 선택하여 그 안에 있는 모든 모델핸들값을 가져오는 구문
        /// </summary>
        /// <param name="sel">0은 Subview</param>
        /// <param name="highlight">선택되서 캡쳐된 부분 하이라이팅 할 여부</param>
        public void Select_By_View(int sel,bool highlighting=true)
        {
            KcsElementHandle viewhandle=new KcsElementHandle();
            KcsCaptureRegion2D region=new KcsCaptureRegion2D();
            kcs_draft.HighlightOff(0);
            KcsElementHandle [] capturehandles=new KcsElementHandle[]{};
            int resp=0;
            try{
                resp=kcs_ui.PointPlanarReq("원하는 뷰에 속하는 아이템을 찍으면 뷰가 선택됩니다.", p1);            
                if(resp==kcs_util.Reject())
                    return;            
                else{
                    viewhandle= kcs_draft.ViewIdentify(p1);
                    KcsRectangle2D temp_area= kcs_draft.ViewRestrictionAreaGet(viewhandle);
                
                
                    region.SetInside();
                    region.SetRectangle(temp_area);
                    region.SetNoCut();
                    capturehandles=this.DetermineCapture(sel);
                

                }
            }catch(Exception ee)
            {
                kcs_ui.MessageConfirm("캡쳐된 모델이 없습니다.");
                
            }
            finally{
                if(resp==kcs_util.Reject())
                {
                    if(highlighting==true)
                    {
                        kcs_draft.ElementHighlight(capturehandles);
                        kcs_ui.MessageNoConfirm("캡쳐된 Element 수량:"+capturehandles.Count().ToString());
                    }
                    handler=capturehandles.ToList();
                    kcs_draft.HighlightOff(0);
                    kcs_draft.ElementHighlight(capturehandles);
                }
                else{
                    kcs_draft.ElementHighlight(capturehandles);

                }
            }

            
        }
        
        public void SelectbyBox(int sel=3,int cut=1)
        {
            try
            {
                handler = new List<KcsElementHandle>();
                int resp=kcs_ui.PointPlanarReq("첫번째 코너를 찍으세요", p1);
                if (resp == kcs_util.Reject() || resp == 253 || resp == 254)
                    return;
                KcsCursorType cursor = new KcsCursorType();
                KcsStat_point2D_req stat = new KcsStat_point2D_req();
                stat.Cursor = cursor;
                cursor.SetRubberRectangle(p1);

                int resp2 = kcs_ui.PointPlanarReq("두번째 코너를 찍으세요", p2, stat);
                if(resp2== kcs_util.Reject() || resp2==253 || resp==254)                
                    return;
                kcs_draft.HighlightOff(0);
                KcsRectangle2D rect = new KcsRectangle2D(p1, p2);
                KcsCaptureRegion2D region = new KcsCaptureRegion2D();
                region.SetInside();
                region.SetRectangle(rect);
                region.Cut = cut;
                KcsElementHandle[] capturehandles=this.DetermineCapture(sel);
                kcs_draft.ElementHighlight(capturehandles);
                kcs_ui.MessageNoConfirm("캡쳐된 Element 갯수는: " + capturehandles.Count().ToString());
                handler = capturehandles.ToList();
                kcs_draft.HighlightOff(0);



            }
            catch(Exception ee)
            {
                kcs_ui.MessageConfirm("캡쳐된 Element가 없습니다.");
            }
        }

        public List<KcsPoint2D> selectByIndicate(int sel, string defmode = "ModeCursor")
        {
            List<KcsPoint2D> points = new List<KcsPoint2D>();
            kcs_draft.HighlightOff(0);
            
            //--TB와 다르게 MarElementHandle OBJ는 핸들값을 handlePML()메소드를 통해서 handle을 비교해야 다른것을 알수 있다
            //--그래서 !handleNums라는 Handle값을 가지는 Array를 통해서 비교로직을 추가로 구현하였다.
            //--그러나 Highlight를 위해서는 그냥 marelementhandle의 Array를 던져주면 동작한다.
            KcsStat_point2D_req status= PointState("CrossHair", defmode);

            while (true)
            {
                try
                {
                    p1 = new KcsPoint2D();
                    int resp = kcs_ui.PointPlanarReq("Indicate Geomery", p1, status);
                    KcsModel model = new KcsModel();
                    
                    if (resp == kcs_util.OperationComplete())
                        break;
                    if (resp == kcs_util.Ok())
                    {
                        KcsElementHandle temp_handle = DetermineIdentify(sel);
                        

                        if (handler.Where(h => h.handle == temp_handle.handle).Count()!=0)
                        {
                            handler.RemoveAll(h => h.handle == temp_handle.handle);
                            kcs_draft.HighlightOff(0);
                            kcs_draft.ElementHighlight(handler.ToArray());
                        }
                        else
                        {
                            points.Add(p1);
                            handler.Add(temp_handle);
                            kcs_draft.ElementHighlight(temp_handle);
                        }

                    }
                    else if (resp == kcs_util.Reject())
                    {
                        handler = new List<KcsElementHandle>();
                        points = new List<KcsPoint2D>();
                        break;
                    }
                }
                catch (Exception)
                {
                    kcs_ui.MessageNoConfirm("오류");
                }
            }
            if(handler.Count==0)
                kcs_ui.MessageConfirm("선택된 모델이 없습니다. 종료합니다.");
            kcs_draft.HighlightOff(0);
            return points;

                    
                  

            //3D포인트 테스트 한 부분
            //p1 = new KcsPoint2D();
            //kcs_ui.PointPlanarReq("Indicate Geomery", p1, status);

            //KcsPoint3D p3 = new KcsPoint3D();
            ////                kcs_ui.PointReq()
            ////int resp = kcs_ui.PointPlanarReq("Indicate Geometry", p1);
            //int resp = kcs_ui.PointReq("Indicate Geometry", p3);
            //KcsElementHandle vh = kcs_draft.ViewIdentify("PLAN");
            //p1 = kcs_draft.PointTransform(vh, p3);
            //if (resp == kcs_util.Ok())
            //{
            //    KcsElementHandle handle = this.DetermineIdentify(sel);

            //    handler.Add(handle);
            //}


        }
        public void Configure_Model_Information()
        {
            try
            {
                foreach (KcsElementHandle handle in handler)
                {
                    try
                    {
                        KcsModel model=kcs_draft.ModelPropertiesGet(handle); //this can be only get  model handle. to get part handle, you have to use below method.
                        string refno=kcs_draft.ElementDbrefGet(handle);
                        //KcsElementHandle viewhandle= kcs_draft.ViewIdentify(p1);
                        //string  viewdbref=kcs_draft.ElementPADDDbrefGet(viewhandle);

                        KcsModel m=new KcsModel();
                        m.Name = "=21184/1156";
                        kcs_draft.ModelDraw(m);
                        
                        if(!models.Contains(model))
                        {
                            models.Add(model);

                            //DbElement dbelement = DbElement.GetElement(model.Name);
                            DbElement dbelement = DbElement.GetElement(refno);

                            dbelements.Add(dbelement);
                            kcs_ui.MessageNoConfirm(dbelement.GetAsString(DbAttributeInstance.FLNN));
                            kcs_ui.MessageNoConfirm(dbelement.Members().ToArray()[model.PartId].GetAsString(DbAttributeInstance.FLNN));
                        }
                    }catch(Exception ee)
                    {
                        kcs_ui.MessageNoConfirm("핸들을 모델로 변환중 에라 : " + handle.handle.ToString());
                    }
                    
                }
            }catch(Exception ee)
            {

            }
        }
        public void modeldraw()
        {
            //KcsModel m = new KcsModel();
            //m.Name = "=21184/1156";
            ////m.Type = "PDMS model";
            //KcsElementHandle viewhandle= kcs_draft.ViewIdentify(p1);
            //kcs_draft.ModelDraw(m,viewhandle);
        }


    }

}
