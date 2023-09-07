﻿using System;
using System.IO;
using SprutTechnology.MachinigToolsImportTypes;
using SprutTechnology.STMachiningToolsImportHelper;
using SprutTechnology.STTypes;
using THDT = SprutTechnology.MachinigToolsImportTypes.TMTI_TurnToolHolderDimensionTypes;
using TIDT = SprutTechnology.MachinigToolsImportTypes.TMTI_TurnToolInsertDimensionTypes;

namespace STConsoleApp
{
    class Program
    {
        private static string myToolStorageFilePath = @"TestStorage.db";

        private static IMTI_MachiningToolsImportLibrary LoadImporter() 
        {
            //Specify an existing path to the Bin64 folder according to your environment SprutCam
            string assemblyPath = @"c:\Program Files\SprutCAM Tech\SprutCAM X 17\Bin64\" + MTIMachiningToolsImportHelper.DllName;
            return File.Exists(assemblyPath) ? MTIMachiningToolsImportHelper.CreateImporter(assemblyPath) : null;
        }

        private static void CreateNewMillTool() {
            var importer = LoadImporter();
            if (importer==null) return;

            var storage = importer.CreateNewToolsStorage(myToolStorageFilePath);

            var tool = importer.CreateCylindricalMill();
            tool.OverallLength = 100;
            tool.CuttingDiameter = 10;
            tool.WorkingLength = 30;
            tool.ShankDiameter = 12;
            tool.ShoulderLength = 60;
            tool.ShankTaperAngle = 40;

            tool.SetName("My Cylindrical Mill");
            tool.SetIdentifier("m001");
            tool.SetTeethsCount(3);
            tool.SetMagazineNumber(1);
            tool.SetToolNumber(1);
            tool.SetUnits(TMTI_LinearUnits.luMillimeter);
            tool.SetOverhang(155);
            tool.SetDurability(70);
            tool.SetMaxPlungeAngle(90);

            var toolings = tool.GetToolingPoints();
            toolings.ToolContactPointType = TMTI_AxialToolContactPointType.cpCustomPoint;
            toolings.ToolContactPointsCount = 1;

            toolings.ToolingPointCount = 1;
            toolings.ToolingPointType[0] = TMTI_AxialToolToolingPointType.tpCustomPoint;
            toolings.ToolingPointShift[0] = 1.5;
            toolings.ToolingPointLengthCorrectorNumber[0] = 1;
            toolings.ToolingPointRadiusCorrectorNumber[0] = 1;

            var cond = tool.GetCuttingConditions();
            cond.CuttingSpeedMode = TMTI_CuttingSpeedMode.csmRPM;
            cond.RotationsPerMinute = 100;
            cond.SpindleGearRange = 1;
            cond.RotationDirection = TMTI_RotationDirection.rdCW;
            cond.FeedUnits = TMTI_FeedUnits.perMinute;
            cond.FeedValue = 100;

            int tubesCount = cond.Coolant.TubeCount; //20
            cond.Coolant.TubeIsOn[0] = true;
            cond.Coolant.TubeIsOn[1] = false;
            cond.Coolant.TubeIsOn[2] = true;

            var adapter = tool.GetStepsAdapter();       
            adapter.HolderName = "my custom adapter";
            adapter.HolderStepCount=3;
            adapter.HolderStepDiameter[0] = 20;
            adapter.HolderStepHeight[0] = 20;
            adapter.HolderStepDiameter[1] = 30;
            adapter.HolderStepHeight[1] = 10;
            adapter.HolderStepDiameter[2] = 30;
            adapter.HolderStepHeight[2] = 60;

            storage.AddToolItem(tool);
            Console.WriteLine("Mill tool Ok!");
        }

        private static void CreateNewTurnTool() {
            var importer = LoadImporter();
            if (importer==null) return;

            var storage = importer.OpenExistingToolsStorage(myToolStorageFilePath);

            var tool = importer.CreateTurnToolWithExternalHolder();
            tool.SetName("My Turn tool");
            tool.SetIdentifier("t001");
            tool.SetMagazineNumber(1);
            tool.SetToolNumber(1);
            tool.SetUnits(TMTI_LinearUnits.luMillimeter);
            tool.SetDurability(70);

            tool.GetHandType().Hand = TMTI_Hand.hRight;

            bool isCompatable = tool.SetHolderAndInsertType(
                TMTI_ExternalToolHolderTypes.htA_90deg, 
                TMTI_ExternalToolHolderInsertTypes.itC_80degRhombic);
            if (isCompatable) {
                var tdim = tool.GetDimensions();
                tdim.Holder[THDT.hdL1] = 130;
                tdim.Holder[THDT.hdB] = 20;
                tdim.Holder[THDT.hdL3] = 30;
                tdim.Holder[THDT.hdF1] = 30;

                tdim.Insert[TIDT.idL] = 17;
                tdim.Insert[TIDT.idTi] = 2;
                tdim.Insert[TIDT.idRe] = 0.1;
                //if you have changed this property, the holder type is automatically set to undefined
                //tdim.Insert[TIDT.idKr] = 90; 
            }
            
            var tdir = tool.GetDirections();
            tdir.FixingDirection = TMTI_FixingDirection.fdDirect;
            tdir.CuttingDirection[TMTI_CuttingDirections.cdS] = true;
            tdir.CuttingDirection[TMTI_CuttingDirections.cdE] = true;
            tdir.CuttingDirection[TMTI_CuttingDirections.cdSE] = true;

            var overh = tool.GetOverhang();
            overh.isAutoCalc = false;
            overh.Axial = -10;
            overh.Radial = 121;
            overh.Matching = 1.5;

            var tpoints = tool.GetToolingPoints();
            tpoints.Corrector[0] = 1;
            tpoints.Corrector[1] = 99;
            tpoints.TPointX[0] = -2.22;
            tpoints.TPointX[1] = 1.11;
            tpoints.TPointY[0] = -1.55;
            tpoints.TPointY[1] = -0.29;

            var cond = tool.GetCuttingConditions();
            cond.RotationDirection = TMTI_RotationDirection.rdCCW;
            cond.CuttingSpeedMode = TMTI_CuttingSpeedMode.csmCSS;
            cond.Coolant.TubeIsOn[0] = false;
            cond.Coolant.TubeIsOn[1] = true;
            cond.Coolant.TubeIsOn[2] = true;
            cond.FeedValue = 2;

            storage.AddToolItem(tool);
            Console.WriteLine("Turn tool Ok!");
        }

        private static void CreateNewCustomAxialShapedTool() {
            var importer = LoadImporter();
            if (importer==null) return;

            var storage = importer.OpenExistingToolsStorage(myToolStorageFilePath);

            var tool = importer.CreateCustomAxialShapeTool();
            tool.ToolGroup = TMTI_AxialToolGroup.tgSpherical;
            tool.OverallLength = 100;
            
            tool.SetName("My Custom Spherical Mill Tool");  
            tool.SetIdentifier("s001");
            tool.SetTeethsCount(2);
            tool.SetMagazineNumber(1);
            tool.SetToolNumber(1);
            tool.SetUnits(TMTI_LinearUnits.luMillimeter);
            tool.SetDurability(70);
            tool.SetMaxPlungeAngle(90);

            var rcv = tool.BeginGeneratrix();
            TST2DPoint p = new TST2DPoint() {X = 1, Y = 1};
            TST2DPoint pc = new TST2DPoint() {X = 0.0359, Y = 7.3301};
            rcv.SetSpanType(TMTI_CurveSpanType.csfWorking);
            rcv.StartCurve(p);
            p.X = 6; p.Y = 5;
            rcv.ArcTo(pc, p, 6.4031);
            p.X = 6; p.Y = 25;
            rcv.CutTo(p);
            rcv.SetSpanType(TMTI_CurveSpanType.csfNonWorking);
            p.X = 6; p.Y = 30;
            rcv.CutTo(p);
            p.X = 7.5; p.Y = 40;
            rcv.CutTo(p);

            var toolings = tool.GetToolingPoints();
            toolings.ToolContactPointType = TMTI_AxialToolContactPointType.cpCustomPoint;
            toolings.ToolContactPointsCount = 1;

            toolings.ToolingPointCount = 1;
            toolings.ToolingPointType[0] = TMTI_AxialToolToolingPointType.tpCustomPoint;
            toolings.ToolingPointShift[0] = 1.5;
            toolings.ToolingPointLengthCorrectorNumber[0] = 1;
            toolings.ToolingPointRadiusCorrectorNumber[0] = 1;

            var cond = tool.GetCuttingConditions();
            cond.CuttingSpeedMode = TMTI_CuttingSpeedMode.csmRPM;
            cond.RotationsPerMinute = 100;
            cond.SpindleGearRange = 1;
            cond.RotationDirection = TMTI_RotationDirection.rdCW;
            cond.FeedUnits = TMTI_FeedUnits.perMinute;
            cond.FeedValue = 100;

            int tubesCount = cond.Coolant.TubeCount; //20
            cond.Coolant.TubeIsOn[0] = true;
            cond.Coolant.TubeIsOn[1] = false;
            cond.Coolant.TubeIsOn[2] = true;

            storage.AddToolItem(tool);
            Console.WriteLine("Custom tool Ok!");
        }

        static void Main(string[] args)
        {
            if (args.Length>0)
                myToolStorageFilePath = args[0];
            CreateNewMillTool();
            CreateNewTurnTool();
            CreateNewCustomAxialShapedTool();
            MTIMachiningToolsImportHelper.FinalizeImporter();
            Console.WriteLine("All is Ok!");
        }
    }
}
