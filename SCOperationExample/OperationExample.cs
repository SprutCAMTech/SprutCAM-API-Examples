namespace OperationExample;

using System;
using SprutCAMTech.STTypes;
using SprutCAMExternalOperations;
using SprutCAMTech.STOperationTypes;
using SprutCAMTech.STMCDFormerTypes;
using SprutCAMTech.STCuttingToolTypes;
using System.Runtime.InteropServices;

/// <summary> SprutCAM example of a custom operation class </summary>
[SCOperationAttribute(
    ClassID = "DACB16F5-127B-45EC-9207-65E1292D712B",
    InterfaceID = "C6558CC8-082F-4C68-868B-4F7722F355FC" //IST_Operation
)]
public class SimpleToolPathGenerator : IST_Operation, IST_OperationSolver
{
    IST_OpContainer? opContainer;
    IST_UpdateHandler? updateHandler;
    IST_CLDReceiver? clf;

    // ------------ IST_Operation --------------------------

    public void Create(IST_OpContainer Container) {
        opContainer = Container;
    }

    public void ClearReferences() {
        if (opContainer != null) {
            Marshal.FinalReleaseComObject(opContainer);
            opContainer = null;
        }
    }

    public void InitModelFormers() {

    }

    public void SaveToStream(SprutCAMTech.STXMLPropTypes.IStream Stream) {

    }

    public void LoadFromStream(SprutCAMTech.STXMLPropTypes.IStream Stream) {

    }

    public void SaveToXML(SprutCAMTech.STXMLPropTypes.IST_XMLPropPointer XMLProp) {

    }

    public void LoadFromXML(SprutCAMTech.STXMLPropTypes.IST_XMLPropPointer XMLProp) {

    }

    public void SetDefaultParameters(IST_OpContainer CopyFrom) {

    }

    public bool IsToolTypeSupported(TSTMillToolType tt) {
        return true;
    }

    public bool IsCorrectParameters() {
        return true;
    }

    public void ResetAll() {

    }

    public void ResetFillOnly() {

    }

    public void ResetTransitionOnly() {

    }

    public void ResetTechInfOnly() {

    }

    public void SaveDebugFiles(string FileNameWithoutExt) {

    }

    private Guid GetClassID() {
        var id = Guid.Empty;
        var atr = this.GetType().GetCustomAttributes(typeof(SCOperationAttribute), true)
            .FirstOrDefault() as SCOperationAttribute;
        if (atr != null)
            id = new Guid(atr.ClassID);
        return id;
    }

    public Guid ID => GetClassID();

    public IST_OpContainer Container => opContainer;

    public IST_OperationSolver Solver => this;

    public IST_OpParametersUI ParametersUI => null;

    // ------------ IST_OperationSolver --------------------------

    public bool IsCorrectParameters(IST_Operation Operation) {
        return true;
    }

    public void InitializeRun(IST_CLDReceiver CLDFormer, IST_UpdateHandler UpdateHandler) {
        clf = CLDFormer;
        updateHandler = UpdateHandler;
    }

    public void Prepare() {

    }

    private delegate void TMakeOneLayer(double currentZ);

    public void MakeWorkPath() {
        if (opContainer == null || clf == null)
            return;
        var xp = opContainer.XMLProp;
        bool isFirstMove = true;
        int pattern = xp.Int["ToolpathParams.Pattern"];
        int layersCount = xp.Int["ToolpathParams.ZLayers.Count"];
        double ZStart = xp.Flt["ToolpathParams.ZLayers.ZStart"];
        double ZStep = xp.Flt["ToolpathParams.ZLayers.ZStep"];
        TST3DPoint lastPoint = default(TST3DPoint);
        TMakeOneLayer? MakeOneLayer = null;
        if (pattern==0) // Rectangle
        {
            TST2DPoint startPoint;
            startPoint.X = xp.Flt["ToolpathParams.RectParams.StartPoint.X"];
            startPoint.Y = xp.Flt["ToolpathParams.RectParams.StartPoint.Y"];
            double width = xp.Flt["ToolpathParams.RectParams.Width"];
            double height = xp.Flt["ToolpathParams.RectParams.Height"];
            MakeOneLayer =
                (curZ) => {
                    if (isFirstMove) {
                        isFirstMove = false;
                        clf.OutStandardFeed((int)TSTFeedTypeFlag.ffRapid);
                    } else
                        clf.OutStandardFeed((int)TSTFeedTypeFlag.ffPlunge);
                    TST3DPoint p = new TST3DPoint { X = startPoint.X, Y = startPoint.Y, Z = curZ };
                    clf.CutTo(p);
                    clf.OutStandardFeed((int)TSTFeedTypeFlag.ffWorking);
                    p.X = p.X + width;
                    clf.CutTo(p);
                    p.Y = p.Y + height;
                    clf.CutTo(p);
                    p.X = p.X - width;
                    clf.CutTo(p);
                    p.Y = p.Y - height;
                    clf.CutTo(p);
                    lastPoint = p;
                };
        } else  if (pattern==1) { // Circle 
            TST2DPoint centerPoint;
            centerPoint.X = xp.Flt["ToolpathParams.CircParams.CenterPoint.X"];
            centerPoint.Y = xp.Flt["ToolpathParams.CircParams.CenterPoint.Y"];
            double radius = 0.5*xp.Flt["ToolpathParams.CircParams.Diameter"];
            MakeOneLayer =
                (curZ) => {
                    if (isFirstMove) {
                        isFirstMove = false;
                        clf.OutStandardFeed((int)TSTFeedTypeFlag.ffRapid);
                    } else
                        clf.OutStandardFeed((int)TSTFeedTypeFlag.ffPlunge);
                    TST3DPoint pc = new TST3DPoint { X = centerPoint.X, Y = centerPoint.Y, Z = curZ };
                    TST3DPoint p1 = new TST3DPoint { X = pc.X-radius, Y = pc.Y, Z = curZ };
                    TST3DPoint p2 = new TST3DPoint { X = pc.X+radius, Y = pc.Y, Z = curZ };
                    clf.CutTo(p1);
                    clf.OutStandardFeed((int)TSTFeedTypeFlag.ffWorking);
                    clf.ArcTo2d(p2, pc, TST_CLDPlaneType.plXY, radius, false);
                    clf.ArcTo2d(p1, pc, TST_CLDPlaneType.plXY, radius, false);
                    lastPoint = p1;
                };
        }
        if (MakeOneLayer != null) {
            for (int i = 0; i < layersCount; i++)
                MakeOneLayer(ZStart + i * ZStep);

            clf.OutStandardFeed((int)TSTFeedTypeFlag.ffRapid);
            lastPoint.Z = ZStart + ZStep;
            clf.CutTo(lastPoint);
        }
    }

    public void MakeFill() {

    }

    public void MakeTransition() {

    }

    public void MakeTechInf() {

    }

    public void FinalizeRun() {

    }

    public void InitLngRes(int LngID) {

    }
}