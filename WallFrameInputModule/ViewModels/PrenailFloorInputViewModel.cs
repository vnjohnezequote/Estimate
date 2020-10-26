// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PrenailFloorInputViewModel.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the PrenailFloorInputViewModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


using System;
using System.ComponentModel;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows.Controls;
using ApplicationInterfaceCore;
using ApplicationService;
using AppModels.CustomEntity;
using AppModels.Enums;
using AppModels.ExportData;
using AppModels.Factories;
using AppModels.Interaface;
using AppModels.PocoDataModel;
using AppModels.ResponsiveData;
using AppModels.ResponsiveData.EngineerMember;
using AppModels.ResponsiveData.Framings;
using AppModels.ResponsiveData.Openings;
using AppModels.ResponsiveData.WallMemberData;
using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using Syncfusion.Data.Extensions;
using Syncfusion.Office;
using Syncfusion.XlsIO;
using Syncfusion.XlsIO.Implementation.Security;
using WallFrameInputModule.Views;
using Environment = System.Environment;

namespace WallFrameInputModule.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.CodeAnalysis;
    //using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;

    using ApplicationCore.BaseModule;

    using AppModels;
    using CsvHelper;

    using MaterialDesignExtensions.Controls;

    using Prism.Commands;
    using Prism.Events;
    using Prism.Regions;

    using Syncfusion.UI.Xaml.Grid;
    using Syncfusion.UI.Xaml.Grid.Helpers;

    using Unity;
    using Excel= Microsoft.Office.Interop.Excel;



    /// <summary>
    /// The Prenail floor input view model.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
    public class PrenailFloorInputViewModel : BaseFloorViewModelAware
    {
        #region private Field
        private IEntitiesManager _entitiesManager;
        /// <summary>
        /// The title.
        /// </summary>
        private string _title;

        //private string _levelName;
        /// <summary>
        /// The start item id.
        /// </summary>
        private int _startItemId;

        private Opening _selectedDoor;

        /// <summary>
        /// The studs.
        /// </summary>
        private Dictionary<string, List<TimberBase>> _studs;

        /// <summary>
        /// The bottomPlates.
        /// </summary>
        private Dictionary<string, List<TimberBase>> _ribbonPlates;

        /// <summary>
        /// The topPlates.
        /// </summary>
        private Dictionary<string, List<TimberBase>> _topPlates;

        /// <summary>
        /// The topPlates.
        /// </summary>
        private Dictionary<string, List<TimberBase>> _bottomPlates;

        /// <summary>
        /// The csv file path.
        /// </summary>
        private string _csvFilePath;

        private PrenailWallLayer _selectedWall;
        #endregion

        #region Properties
        public List<int> SupportSpans { get; set; } = new List<int>(){3000,6000,9000,12000,15000};
        public List<int> ExtDoorListHeights { get; set; } = new List<int> { 600, 900, 1200, 1500, 1800, 2100, 2400, 2700, 3000 };
        public Visibility PrenailVisibility
        {
            get
            {
                if (this.SelectedClient!=null && SelectedClient.Name=="Prenail")
                {
                    return Visibility.Visible;
                }

                return Visibility.Collapsed;
            }
        }

        public Visibility FloorRafterFramingVisibility
        {
            get
            {
                if (JobModel.Info.QuoteFloorFrame)
                    return Visibility.Visible;
                else return Visibility.Collapsed;
            }
        }

        public Opening SelectedDoor
        {
            get=>_selectedDoor;
            set=>SetProperty(ref _selectedDoor,value);
        }

        public Visibility WarnervaleVisibility
        {
            get
            {
                if (this.SelectedClient!=null && SelectedClient.Name=="Warnervale")
                {
                    return Visibility.Visible;
                }

                return Visibility.Collapsed;
            }
        }
        public ICommand AddNewDoorCommand { get; private set; }
        public ICommand DeleteBracingCommand { get; private set; }
        public ICommand DeleteOpeningRowCommand { get; private set; }
        public ICommand AddNewFloorSheetCommand { get; private set; }
        public ICommand RemoveFloorSheetCommand { get; private set; }
        public ICommand SelectedFloorFramingChanged { get; private set; }

        #endregion
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="PrenailFloorInputViewModel"/> class.
        /// </summary>
        public PrenailFloorInputViewModel()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrenailFloorInputViewModel"/> class.
        /// </summary>
        /// <param name="unityContainer">
        /// The unity container.
        /// </param>
        /// <param name="regionManager">
        /// The region manager.
        /// </param>
        /// <param name="eventAggregator">
        /// The event aggregator.
        /// </param>
        public PrenailFloorInputViewModel(IUnityContainer unityContainer, IRegionManager regionManager, IEventAggregator eventAggregator, ILayerManager layerManager, IEntitiesManager entitiesManager, IJob jobModel)
            : base(unityContainer, regionManager, eventAggregator, layerManager, jobModel)
        {
            EngineerList = new ObservableCollection<EngineerMemberInfo>();
            this.EntitiesManager = entitiesManager;
            EntitiesManager.EntitiesCollectionChanged += EntitiesManager_EntitiesCollectionChanged;
            this.PropertyChanged += PrenailFloorInputViewModelPropertyChanged;
            this.WallThicknessList = new List<int> { 90, 70 };
            WallSpacingList = new List<int>() { 300, 350, 400, 450, 600 };
            //this.CreateLayers();
            //this.WallInputLoadedCommand = new DelegateCommand<FrameworkElement>(this.InputControlLoaded);
            this.NewWallRowInputCommand = new DelegateCommand(this.OnAddNewWallRow);
            this.DeleteWallRowCommand = new DelegateCommand<SfDataGrid>(this.OnDeleteWallRow);
            this.WallInputSortCommand = new DelegateCommand(this.OnWallInputSort);
            this.LoadCSVFileCommand = new DelegateCommand(this.OnLoadCSVFile);
            this.AddBeamCommand = new DelegateCommand(this.OnAddNewBeam);
            this.AddBracingCommand = new DelegateCommand(OnAddBracing);
            this.ReFreshWallCommand = new DelegateCommand(this.CalculatorWallLength);
            DeleteBracingCommand = new DelegateCommand<SfDataGrid>(OnDeleteBracing);
            AddNewDoorCommand = new DelegateCommand(OnAddNewDoorCommand);
            DeleteOpeningRowCommand = new DelegateCommand<SfDataGrid>(OnDeleteOpening);
            ExportDataToExcelCommand = new DelegateCommand(OnExportData);
            AddNewFloorSheetCommand = new DelegateCommand(AddNewFloorSheet);
            RemoveFloorSheetCommand = new DelegateCommand(RemoveFloorSheet);
            SelectedFloorFramingChanged = new DelegateCommand<FramingSheet>(SetSelectedFloorFraming);
            JobModel.EngineerMemberList.CollectionChanged += EngineerMemberList_CollectionChanged;
            JobModel.Info.PropertyChanged += JobInfoPropertyChanged;
        }

        private void SetSelectedFloorFraming(FramingSheet floorSheet)
        {
            if (floorSheet!=null)
            {
                this.JobModel.ActiveFloorSheet = floorSheet;
            }
            else
            {
                this.JobModel.ActiveFloorSheet = null;
            }
        }
        private void RemoveFloorSheet( )
        {
            if (this.JobModel.ActiveFloorSheet != null)
            {
                if (this.Level.FloorSheets.Contains(this.JobModel.ActiveFloorSheet))
                {
                    this.Level.FloorSheets.Remove(this.JobModel.ActiveFloorSheet);
                }
            }
        }
        private void AddNewFloorSheet()
        {
            var floorSheet = new FramingSheet();
            floorSheet.FloorName = "Floor - "+ this.Level.LevelName;
            var id = Level.FloorSheets.Count+1;
            if (id>1)
            {
                floorSheet.ShowSheetId = true;
                Level.FloorSheets[0].ShowSheetId = true;
            }
            floorSheet.Id = id;
            Level.FloorSheets.Add(floorSheet);

        }
        private void JobInfoPropertyChanged(object sender,PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(JobModel.Info.QuoteFloorFrame))
            {
                RaisePropertyChanged(nameof(FloorRafterFramingVisibility));
            }
        }
        private void OnDeleteBracing(SfDataGrid bracingGrid)
        {
            var recordId = bracingGrid.SelectedIndex;

            if (recordId < 0)
            {
                return;
            }

            this.Level.TimberWallBracings.RemoveAt(recordId);

        }

        private void OnDeleteOpening(SfDataGrid openingGrid)
        {
            var recordId = openingGrid.SelectedIndex;
            if (recordId < 0)
            {
                return;
            }

            if (Level.Lintels.Contains(Level.Openings[recordId].Lintel))
            {
                Level.Lintels.Remove(Level.Openings[recordId].Lintel);
            }
            Level.Openings.RemoveAt(recordId);
        }
        private void OnAddNewDoorCommand()
        {
            var _startDoorId = Level.Openings.Count + 1;
            var door = new Opening(this.Level.LevelInfo) { Id = _startDoorId };
            Level.AddOpening(door);
        }
        protected override void OnRaisePropertiesChanged()
        {
            //base.OnRaisePropertiesChanged();
            RaisePropertyChanged(nameof(Title));
            RaisePropertyChanged(nameof(PrenailVisibility));
            RaisePropertyChanged(nameof(WarnervaleVisibility));
        }

        private void EngineerMemberList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {

            EngineerList.Clear();

            InitEngineerList();

            foreach (var beam in Level.RoofBeams)
            {
                beam.NotifyPropertyChanged();
            }
            RaisePropertyChanged(nameof(EngineerReferenceVisibility));
        }

        private void OnExportData()
        {
            if (SelectedClient != null)
            {
                if (SelectedClient.Name == "Prenail")
                {
                    ExportPrenailData();
                }

                if (SelectedClient.Name == "Warnervale")
                {
                    ExportWarnervaleData();
                }
            }
            
        }

        private void ExportWarnervaleData()
        {
            if (!CheckJobInfo(out var message))
            {
                MessageBox.Show(message);

                return;
            }
            WarnervaleExportDataList = new List<WarnervaleExportData>();
            var excel = new Excel.Application();
            var workbook = excel.Workbooks.Open(System.AppDomain.CurrentDomain.BaseDirectory + "WarnervaleTemplate.xlsm");
            var sheets = workbook.Worksheets;
            var infoSheet = sheets["Job Particulars Input"] as Excel.Worksheet;
            var isNeedSecondWorkBook = false;
            switch (JobModel.Levels.Count)
            {
                case 1:
                    //InputJobWarnervaleInfo(infoSheet);
                    if (!GeneralFileForWarnervale(workbook, JobModel.Levels[0], true))
                    {
                        MessageBox.Show("Export failure");
                        excel.Quit();
                        System.Runtime.InteropServices.Marshal.FinalReleaseComObject(excel);
                        return;
                    }
                    
                    break;
                case 2:
                    foreach (var jobModelLevel in JobModel.Levels)
                    {
                        var levelName = jobModelLevel.LevelName.Replace(" Floor", "");
                        if (!GeneralFileForWarnervale(workbook, jobModelLevel, true))
                        {
                            MessageBox.Show("Export failure");
                            excel.Quit();
                            System.Runtime.InteropServices.Marshal.FinalReleaseComObject(excel);
                            return;
                        }
                        //InputJobWarnervaleInfo(infoSheet,levelName,jobModelLevel);
                        
                    }
                    //InputJobWarnervaleInfo(infoSheet);
                    break;
                case 3:
                    foreach (var jobModelLevel in JobModel.Levels)
                    {
                        //InputJobWarnervaleInfo(infoSheet, jobModelLevel.LevelName, jobModelLevel,true);
                        if (!GeneralFileForWarnervale(workbook, JobModel.Levels[0], true))
                        {
                            MessageBox.Show("Export failure");
                            excel.Quit();
                            System.Runtime.InteropServices.Marshal.FinalReleaseComObject(excel);
                            return;
                        }
                        
                    }
                    //InputJobWarnervaleInfo(infoSheet);
                    break;
                case 4:
                    foreach (var jobModelLevel in JobModel.Levels)
                    {
                        //InputJobWarnervaleInfo(infoSheet, jobModelLevel.LevelName, jobModelLevel,true);
                        
                        if (!GeneralFileForWarnervale(workbook, JobModel.Levels[0], true))
                        {
                            MessageBox.Show("Export failure");
                            excel.Quit();
                            System.Runtime.InteropServices.Marshal.FinalReleaseComObject(excel);
                            return;
                        }
                    }
                    //InputJobWarnervaleInfo(infoSheet);
                    break;
                default:break;
                    
            }
            excel.Quit();
            System.Runtime.InteropServices.Marshal.FinalReleaseComObject(excel);
            foreach (var warnervaleExportData in WarnervaleExportDataList)
            {
                warnervaleExportData.ExportToExcel(JobModel);
            }
            MessageBox.Show("Export Complete");
        }

        private bool GeneralFileForWarnervale(Excel.Workbook workbook,LevelWall level,bool includeLevelName)
        {
            var fileSave = JobModel.Info.JobNumber;
            if (JobModel.Info.JobLocation.EndsWith("\\"))
            {
                fileSave = JobModel.Info.JobLocation + fileSave;
            }
            else
            {
                fileSave = JobModel.Info.JobLocation + "\\" + fileSave;
            }

            if (!string.IsNullOrEmpty(JobModel.Info.UnitNumber))
            {
                fileSave = fileSave + "-Unit " + JobModel.Info.UnitNumber;
            }

            if (includeLevelName)
            {
                fileSave = fileSave + " " + level.LevelName;
                fileSave = fileSave.Replace(" Floor", "");
            }
            var warnervaleExportData = new WarnervaleExportData(level,fileSave);
            try
            {
                warnervaleExportData.SaveFile(workbook);
                WarnervaleExportDataList.Add(warnervaleExportData);
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("Please Close your file before export data");
                return false;
            }
            
            
            
        }

        private void ExportPrenailData()
        {
            if (!CheckJobInfo(out var message))
            {
                MessageBox.Show(message);

                return;
            }
            var excel = new Excel.Application();
            
            var workbook = excel.Workbooks.Open(System.AppDomain.CurrentDomain.BaseDirectory + "PrenailTemplate.xlsm");
            var sheets = workbook.Worksheets;
            var isNeedSecondWorkBook = false;
            var listLevelIndexAddTwoSheet = new List<int>();
            switch (JobModel.Levels.Count)
            {
                case 1:
                    excel.GetType().InvokeMember("Run",
                        System.Reflection.BindingFlags.Default | System.Reflection.BindingFlags.InvokeMethod, null,
                        excel, new Object[] { "PrenailTemplate.xlsm!ShowSingleStorey" });
                    excel.ScreenUpdating = false;
                    Excel.Worksheet worksheet = sheets["JOB SETUP"];
                    OnFillJobInfoToExcel(worksheet);
                    LoadSingleFrameDelivery(worksheet);
                    excel.ScreenUpdating = true;
                    isNeedSecondWorkBook = OnFillWallToExcel(workbook, listLevelIndexAddTwoSheet);
                    //excel.Run("ShowSingleStorey");
                    break;
                case 2:
                    excel.GetType().InvokeMember("Run",
                       System.Reflection.BindingFlags.Default | System.Reflection.BindingFlags.InvokeMethod, null,
                       excel, new Object[] { "PrenailTemplate.xlsm!ShowDoubleStorey" });
                    excel.ScreenUpdating = false;
                    Excel.Worksheet worksheet2 = sheets["JOB SETUP"];
                    OnFillJobInfoToExcel(worksheet2);
                    LoadDoubleFrameDelivery(worksheet2);
                    excel.ScreenUpdating = true;
                    isNeedSecondWorkBook = OnFillWallToExcel(workbook, listLevelIndexAddTwoSheet);
                    //excel.Run("ShowDoubleStorey");
                    break;
                case 3:
                    excel.GetType().InvokeMember("Run",
                      System.Reflection.BindingFlags.Default | System.Reflection.BindingFlags.InvokeMethod, null,
                      excel, new Object[] { "PrenailTemplate.xlsm!threestorey" });
                    excel.ScreenUpdating = false;
                    Excel.Worksheet worksheet3 = sheets["JOB SETUP"];
                    OnFillJobInfoToExcel(worksheet3);
                    LoadThreeFrameDelivery(worksheet3);
                    excel.ScreenUpdating = true;
                    isNeedSecondWorkBook = OnFillWallToExcel(workbook, listLevelIndexAddTwoSheet);
                    //excel.Run(" threestorey");
                    break;
                default:
                    break;
            }
            string fileSave = "Prenail Estimate - " + JobModel.Info.JobNumber;
            if (JobModel.Info.JobLocation.EndsWith("\\"))
            {
                fileSave = JobModel.Info.JobLocation + fileSave;
            }
            else
            {
                fileSave = JobModel.Info.JobLocation + "\\" + fileSave;
            }
            if (!isNeedSecondWorkBook)
            {
                try
                {
                    workbook.SaveAs(fileSave);
                }
                catch (Exception e)
                {
                    MessageBox.Show("Please close your File Before Export Data");
                    return;
                }
                
            }

            if (isNeedSecondWorkBook)
            {
                var fileSave1 = fileSave + "-Sheet 1";
                workbook.SaveAs(fileSave1);
                Excel.Worksheet jobSetup = workbook.Worksheets["JOB SETUP"];
                jobSetup.Range["C15"].ClearContents();
                jobSetup.Range["C17"].ClearContents();
                jobSetup.Range["C19"].ClearContents();
                switch (JobModel.Levels.Count)
                {
                    case 1:
                        var singleFloor = workbook.Worksheets["Single Storey ESTIMATE"];
                        FillSingleLevelToExcel(singleFloor, false);
                        break;
                    case 2:
                        var doubleFloor = workbook.Worksheets["2 Storey ESTIMATE"];
                        FillDoubleLevelToExcel(doubleFloor, listLevelIndexAddTwoSheet, false);
                        break;
                    case 3:
                        var lowerFloor = workbook.Worksheets["Basement Level ESTIMATE"];
                        var midleFloor = workbook.Worksheets["Middle & Upper Level ESTIMATE"];
                        FillThreeLevelWallToExcel(lowerFloor, midleFloor, listLevelIndexAddTwoSheet, false);
                        break;
                    default:
                        break;
                }
                var fileSave2 = fileSave + "-Sheet 2";
                try
                {
                    workbook.SaveAs(fileSave2);
                }
                catch (Exception e)
                {
                    MessageBox.Show("Please close your file before export Data");
                    return;
                }
                
            }
            excel.Quit();
            System.Runtime.InteropServices.Marshal.FinalReleaseComObject(excel);
            MessageBox.Show("Export Complete");
        }
        private void OnFillJobInfoToExcel(Excel.Worksheet worksheet)
        {
            worksheet.Range["C12"].Formula = "=TODAY()";
            worksheet.Range["C13"].Value = JobModel.Info.JobNumber;
            worksheet.Range["F13"].Value = JobModel.Info.BuilderName;
            worksheet.Range["F14"].Value = JobModel.Info.JobAddress;
            worksheet.Range["F15"].Value = JobModel.Info.SubAddress;
            worksheet.Range["C23"].Value = JobModel.Info.Treatment;
            worksheet.Range["C25"].Value = JobModel.Info.WindRate;
            StringBuilder plannIssue = new StringBuilder("ISSUE ");
            plannIssue.Append(JobModel.Info.PlanIsueDate.ToString("dd/MM/yyyy"));
            plannIssue.Append(" + Arch");
            if (JobModel.Info.IsEngineer)
            {
                plannIssue.Append(" + Engineer");
            }

            if (JobModel.Info.IsBracingPlan)
            {
                plannIssue.Append(" + BC-Plan");
            }

            if (JobModel.Info.IsEPlan)
            {
                plannIssue.Append(" + E-Plan");
            }

            worksheet.Range["C33"].Value = plannIssue.ToString();



        }
        private bool OnFillWallToExcel(Excel.Workbook workbook,List<int> levelIndexsaddTwoSheet)
        {
            switch (JobModel.Levels.Count)
                {
                    case 1:
                    {
                        Excel.Worksheet singleFloor = workbook.Worksheets["Single Storey ESTIMATE"];
                        singleFloor.Range["I61"].Value = BuildNote();
                        if (JobModel.Levels[0].WallLayers.Count>13)
                        {
                            FillSingleLevelToExcel(singleFloor);
                            return true;
                        }
                        FillSingleLevelToExcel(singleFloor);
                        return false;

                    }
                    case 2:
                    {
                        var returnValue = false;
                        Excel.Worksheet doubleFloor = workbook.Worksheets["2 Storey ESTIMATE"];
                        doubleFloor.Range["I148"].Value = BuildNote();
                        if (JobModel.Levels[0].WallLayers.Count>13)
                        {
                            levelIndexsaddTwoSheet.Add(0);
                            returnValue = true;
                        }

                        if (JobModel.Levels[1].WallLayers.Count>13)
                        {
                            levelIndexsaddTwoSheet.Add(1);
                            returnValue = true;
                        }
                        FillDoubleLevelToExcel(doubleFloor,levelIndexsaddTwoSheet);
                        return returnValue;
                    }

                    case 3:
                    {
                        var returnValue = false;
                        Excel.Worksheet lowerFloor = workbook.Worksheets["Basement Level ESTIMATE"];
                        Excel.Worksheet midleFloor = workbook.Worksheets["Middle & Upper Level ESTIMATE"];
                        midleFloor.Range["I148"].Value = BuildNote();
                        if (JobModel.Levels[0].WallLayers.Count > 13)
                        {
                            levelIndexsaddTwoSheet.Add(0);
                            returnValue = true;
                        }

                        if (JobModel.Levels[1].WallLayers.Count > 13)
                        {
                            levelIndexsaddTwoSheet.Add(1);
                            returnValue = true;
                        }
                        if (JobModel.Levels[2].WallLayers.Count > 13)
                        {
                            levelIndexsaddTwoSheet.Add(2);
                            returnValue = true;
                        }

                        FillThreeLevelWallToExcel(lowerFloor, midleFloor,levelIndexsaddTwoSheet);
                        return returnValue;
                    }
                }
            return false;
        }
        private void FillThreeLevelWallToExcel(Excel.Worksheet lowerSheet, Excel.Worksheet midleSheet, List<int> levelNeedTobeAddList, bool isOnlyOneSheetToFill = true)
        {
            if (isOnlyOneSheetToFill)
            {
                FillSingleLevelToExcel(lowerSheet);
                FillUpperLevel(midleSheet,JobModel.Levels[2],isOnlyOneSheetToFill);
                FillLowerLevel(midleSheet, JobModel.Levels[1],isOnlyOneSheetToFill);
            }
            else
            {
                lowerSheet.Range["B12", "H24"].ClearContents();
                lowerSheet.Range["J12", "J24"].ClearContents();
                lowerSheet.Range["C25"].ClearContents();
                lowerSheet.Range["J30", "J36"].ClearContents();
                lowerSheet.Range["B40", "G49"].ClearContents();

                midleSheet.Range["B12", "H24"].ClearContents();
                midleSheet.Range["J12", "J24"].ClearContents();
                midleSheet.Range["C25"].ClearContents();
                midleSheet.Range["J30", "J36"].ClearContents();
                midleSheet.Range["B40", "G49"].ClearContents();

                midleSheet.Range["B90", "H102"].ClearContents();
                midleSheet.Range["J90", "J102"].ClearContents();
                midleSheet.Range["C103"].ClearContents();
                midleSheet.Range["J108", "J114"].ClearContents();
                midleSheet.Range["B118", "G127"].ClearContents();

                if (levelNeedTobeAddList.Contains(0))
                {
                    FillSingleLevelToExcel(lowerSheet,false);
                }

                if (levelNeedTobeAddList.Contains(1))
                {
                    FillLowerLevel(midleSheet, JobModel.Levels[1],false);
                }

                if (levelNeedTobeAddList.Contains(2))
                {
                    FillUpperLevel(midleSheet, JobModel.Levels[2], false);
                }
                
            }
        }
        private void FillDoubleLevelToExcel(Excel.Worksheet doubleSheet,List<int> levelNeedTobeAddList, bool isOnlyOneSheetToFill = true)
        {
            CleardoubleConttent(doubleSheet);
            if (isOnlyOneSheetToFill)
            {
                FillUpperLevel(doubleSheet, JobModel.Levels[1], isOnlyOneSheetToFill);
                FillLowerLevel(doubleSheet, JobModel.Levels[0], isOnlyOneSheetToFill);
            }
            else
            {
                //Clear Content before fill new Sheet
                

                foreach (var level in levelNeedTobeAddList)
                {
                    if (level == 0)
                    {
                        FillLowerLevel(doubleSheet, JobModel.Levels[0], isOnlyOneSheetToFill);
                    }
                    else
                    {
                        FillUpperLevel(doubleSheet, JobModel.Levels[1], isOnlyOneSheetToFill);
                    }
                }
            }
        }
        private void CleardoubleConttent(Excel.Worksheet doubleSheet)
        {
            doubleSheet.Range["B12", "H24"].ClearContents();
            doubleSheet.Range["J12", "J24"].ClearContents();
            doubleSheet.Range["C25"].ClearContents();
            doubleSheet.Range["J30", "J36"].ClearContents();
            doubleSheet.Range["B40", "G49"].ClearContents();

            doubleSheet.Range["B90", "H102"].ClearContents();
            doubleSheet.Range["J90", "J102"].ClearContents();
            doubleSheet.Range["C103"].ClearContents();
            doubleSheet.Range["J108", "J114"].ClearContents();
            doubleSheet.Range["B118", "G127"].ClearContents();
        }
        private void FillUpperLevel(Excel.Worksheet doubleSheet,LevelWall level,bool isOnlyOneSheetToFill)
        {
            var startIndexToFill = 0;
            if (isOnlyOneSheetToFill == true)
            {
                if (level.LintelLm != 0)
                {
                    doubleSheet.Range["C103"].Value = level.LintelLm;
                }

                if (level.GeneralBracings[0].Quantity != 0)
                    doubleSheet.Range["J108"].Value = level.GeneralBracings[0].Quantity;
                if (level.TimberWallBracings.Count > 0)
                {
                    FillTimberBracing(doubleSheet, level,78);
                }
                if (level.RoofBeams.Count > 0)
                {
                    FillRoofBeam(doubleSheet, level,78);
                }

            }
            else
            {
                startIndexToFill = 13;
            }
            FillWallToExcel(doubleSheet, level, startIndexToFill,90);
        }
        private void FillLowerLevel(Excel.Worksheet doubleSheet,LevelWall level, bool isOnlyOneSheetToFill)
        {
            var startIndexToFill = 0;
            if (isOnlyOneSheetToFill == true)
            {
                if (level.LintelLm != 0)
                {
                    doubleSheet.Range["C25"].Value = level.LintelLm;
                }

                if (level.GeneralBracings[0].Quantity != 0)
                    doubleSheet.Range["J30"].Value = level.GeneralBracings[0].Quantity;
                if (level.TimberWallBracings.Count > 0)
                {
                    FillTimberBracing(doubleSheet, level);
                }
                if (level.RoofBeams.Count > 0)
                {
                    FillRoofBeam(doubleSheet, level);
                }

            }
            else
            {
                startIndexToFill = 13;
            }
            FillWallToExcel(doubleSheet, level, startIndexToFill);
        }
        private void FillSingleLevelToExcel(Excel.Worksheet singleSheet,bool isOnlyOneSheetToFill = true)
        {
            var level = JobModel.Levels[0];
            var startIndexToFill = 0;
            ClearOneContentBeforeFill(singleSheet);
            if ( isOnlyOneSheetToFill== true)
            {
                if (level.LintelLm != 0)
                {
                    singleSheet.Range["C25"].Value = level.LintelLm;
                }

                if (level.GeneralBracings[0].Quantity != 0)
                    singleSheet.Range["J30"].Value = level.GeneralBracings[0].Quantity;
                if (level.TimberWallBracings.Count > 0)
                {
                    FillTimberBracing(singleSheet, level);
                }

                if (level.RoofBeams.Count>0)
                {
                    FillRoofBeam(singleSheet, level);
                }
            }
            else
            {
                startIndexToFill = 13;
            }
            FillWallToExcel(singleSheet, level, startIndexToFill);

        }

        private string BuildNote()
        {
            StringBuilder notes = new StringBuilder("Design wind speed used ");
            notes = notes.Append(JobModel.Info.WindRate);
            notes.Append(Environment.NewLine);
            notes.Append(JobModel.Info.FrameDesignInfor.Header);
            notes.Append(" ");
            notes.Append(JobModel.Info.FrameDesignInfor.Content);
            notes.Append(Environment.NewLine);
            if (JobModel.Info.BeamDesignInfor != null)
            {
                notes.Append(JobModel.Info.BeamDesignInfor.Header);
                notes.Append(" ");
                notes.Append(JobModel.Info.BeamDesignInfor.Content);
                notes.Append(Environment.NewLine);
            }

            notes.Append(JobModel.Info.BracingDesignInfor.Header);
            notes.Append(" ");
            notes.Append(JobModel.Info.BracingDesignInfor.Content);
            notes.Append(Environment.NewLine);
            notes.Append("Please check and Confirm");
            return notes.ToString();

        }
        private void ClearOneContentBeforeFill(Excel.Worksheet singleSheet)
        {
            singleSheet.Range["B12", "H24"].ClearContents();
            singleSheet.Range["J12", "J24"].ClearContents();
            singleSheet.Range["C25"].ClearContents();
            singleSheet.Range["J30", "J36"].ClearContents();
            singleSheet.Range["B40", "G49"].ClearContents();
        }
        private void FillRoofBeam(Excel.Worksheet inputSheet,LevelWall level,int movementIndexRow = 0)
        {
            var rowIndex = 40 + movementIndexRow;
            foreach (var roofBeam in level.RoofBeams)
            {
                if (roofBeam.MaterialType == MaterialTypes.Steel)
                {
                    continue;
                }
                var location = "B" + rowIndex;
                var grade = "C" + rowIndex;
                var sizeTreatement = "D" + rowIndex;
                var qty = "F" + rowIndex;
                var length = "G" + rowIndex;

                inputSheet.Range[location].Value = roofBeam.Location;
                inputSheet.Range[grade].Value = roofBeam.TimberInfo.TimberGrade;
                inputSheet.Range[sizeTreatement].Value = roofBeam.TimberInfo.SizeTreatment;
                inputSheet.Range[qty].Value = roofBeam.Quantity;
                inputSheet.Range[length].Value = roofBeam.QuoteLength;
                rowIndex++;
            }
            

            
        }
        private void FillTimberBracing(Excel.Worksheet inputSheet,LevelWall level,int movementIndexRow = 0)
        {
            
            foreach (var timberWallBracing in level.TimberWallBracings)
            {
                var fillQtyIndex = "0";
                bool isAvailableFillIndex = false;
                switch (timberWallBracing.BracingInfo.Height)
                {
                    case 2400:
                        if (timberWallBracing.BracingInfo.Width == 900)
                        {
                            var index = 31 + movementIndexRow;
                            fillQtyIndex = "J"+index;
                            isAvailableFillIndex = true;
                        }

                        if (timberWallBracing.BracingInfo.Width == 1200)
                        {
                            var index2 = 32 + movementIndexRow;
                            fillQtyIndex = "J"+index2;
                            isAvailableFillIndex = true;
                        }
                        break;
                    case 2700:
                        if (timberWallBracing.BracingInfo.Width == 900)
                        {
                            var index3 = 33 + movementIndexRow;
                            fillQtyIndex = "J"+index3;
                            isAvailableFillIndex = true;
                        }

                        if (timberWallBracing.BracingInfo.Width == 1200)
                        {
                            var index4 = 34 + movementIndexRow;
                            fillQtyIndex = "J"+index4;
                            isAvailableFillIndex = true;
                        }
                        break;
                    case 3000:
                        if (timberWallBracing.BracingInfo.Width == 900)
                        {
                            var index5 = 35 + movementIndexRow;
                            fillQtyIndex = "J"+index5;
                            isAvailableFillIndex = true;
                        }

                        if (timberWallBracing.BracingInfo.Width == 1200)
                        {
                            var index6 = 36 + movementIndexRow;
                            fillQtyIndex = "J"+index6;
                            isAvailableFillIndex = true;
                        }
                        break;
                }

                if (isAvailableFillIndex)
                {
                    inputSheet.Range[fillQtyIndex].Value = timberWallBracing.Quantity;
                }
            }
        }
        private void FillWallToExcel(Excel.Worksheet inputSheet,LevelWall level,int startIndexToFill,int startIndexRow = 12)
        {
            var endIndex = 0;
            if (startIndexToFill ==0)
            {
                if (level.WallLayers.Count>13)
                {
                    endIndex = 13;
                }
                else
                {
                    endIndex = level.WallLayers.Count;
                }
            }
            else
            {
                endIndex = level.WallLayers.Count;
            }
            for (var j = startIndexToFill; j < endIndex; j++)
            {
                FillWallRow(startIndexRow,level.WallLayers[j],inputSheet);
                startIndexRow++;
            }
        }
        private bool CheckJobInfo(out string message)
        {
            message = "";
            
            if (string.IsNullOrEmpty(JobModel.Info.JobNumber))
            {
                message = "You Missed JobNumber, Please Check";
                return false;
            }

            if (string.IsNullOrEmpty(JobModel.Info.BuilderName))
            {
                message = "You Missed Builder, Please Check";
                return false;
            }
            if (string.IsNullOrEmpty(JobModel.Info.JobAddress))
            {
                message = "You Missed Job Address, Please Check";
                return false;
            }
            if (string.IsNullOrEmpty(JobModel.Info.Treatment))
            {
                message = "You Missed Wall2D Treatment, Please Check";
                return false;
            }

            if (SelectedClient!=null && SelectedClient.Name=="Prenail" && JobModel.Info.FrameDesignInfor == null)
            {
                message = "You Missed Frame Design Info";
                return false;
            }

            if (SelectedClient != null && SelectedClient.Name == "Prenail" && JobModel.Info.BracingDesignInfor == null)
            {
                message = "You Missed Bracing Design Info";
                return false;
            }
            if (JobModel.Levels.Count == 0)
            {
                message = "Are you sure this job has no any level?";
                return false;
            }
            foreach (var jobModelLevel in JobModel.Levels)
            {
                if (jobModelLevel.WallLayers.Count==0)
                {
                    message = "Are you sure "+jobModelLevel.LevelName+" has no any Wall2D?";
                    return false;
                }

                foreach (var wall in jobModelLevel.WallLayers)
                {
                    if (wall.WallType == null)
                    {
                        message = "You Missed Wall2D Type";
                        return false;
                    }

                    if (wall.WallColorLayer==null)
                    {
                        message = "You Missed Wall2D Layer";
                        return false;
                    }

                    if (wall.WallLength==0)
                    {
                        message = "You Missed Wall2D Length";
                        return false;
                    }
                }

                if (SelectedClient!=null && SelectedClient.Name == "Prenail")
                {
                    if (Level.TimberWallBracings.Count != 0)
                    {
                        foreach (var wallbracing in Level.TimberWallBracings)
                        {
                            if (wallbracing.Quantity == 0)
                            {
                                message = "You Missed Wall2D Bracings Quantity";
                                return false;
                            }

                            if (wallbracing.BracingInfo == null)
                            {
                                message = "You Missed Wall2D Bracings Type";
                                return false;
                            }
                        }
                    }

                    if (Level.RoofBeams.Count != 0)
                    {
                        int countBeam = 0;
                        foreach (var roofBeam in Level.RoofBeams)
                        {
                            if (roofBeam.MaterialType != MaterialTypes.Steel)
                            {
                                countBeam++;
                            }
                        }
                        if (countBeam > 10)
                        {
                            message = "Your's Beams out of beam list in excel, please combine some beam";
                            return false;
                        }
                        foreach (var roofBeam in Level.RoofBeams)
                        {
                            if (roofBeam.Quantity == 0)
                            {
                                message = "You Missed Roof Beam Quantity";
                                return false;
                            }

                            if (roofBeam.IsBeamToLongWithStockList)
                            {
                                message = "Your's beams has a beam out of stock, please check again";
                                return false;
                            }

                            if (roofBeam.TimberInfo == null)
                            {
                                message = "You Missed choose beam in your's Beams List, please check againt";
                                return false;
                            }

                            if (string.IsNullOrEmpty(roofBeam.Location))
                            {
                                message = "Yours Beam Missed Beam Location";
                                return false;
                            }
                        }
                    }


                }
            }
            

            return true;
        }
        private void FillWallRow(int i, WallBase wall, Excel.Worksheet worksheet)
        {
            if (wall.WallType == null)
            {
                return;
            }
            var wallType = "B" + i;
            var wallHeight = "C" + i;
            var studSize = "D" + i;
            var studSpacing = "E" + i;
            var topPlate = "F" + i;
            var ribbonPlate = "G" + i;
            var bottomPlate = "H" + i;
            var wallLength = "J" + i;

            worksheet.Range[wallType].Value = wall.WallType.AliasName;
            worksheet.Range[wallHeight].Value = wall.FinalWallHeight;
            worksheet.Range[studSize].Value = wall.Stud.SizeGrade;
            worksheet.Range[studSpacing].Value = wall.WallSpacing + "mm";
            worksheet.Range[topPlate].Value = wall.TopPlate.SizeGrade;
            worksheet.Range[ribbonPlate].Value = wall.RibbonPlate.SizeGrade;
            worksheet.Range[bottomPlate].Value = wall.BottomPlate.SizeGrade;
            worksheet.Range[wallLength].Value = wall.WallLength;
        }
        private void LoadSingleFrameDelivery(Excel.Worksheet worksheet)
        {
            worksheet.Activate();
            worksheet.Unprotect("Secureme");

            worksheet.Range["C21"].Value="SINGLE STOREY Frame Delivery";

            ResetFormatDeliveryRange(worksheet);

            worksheet.Range["B15"].Value = "SINGLE STOREY DELIVERY:";

            Excel.Range range = worksheet.Range["C15"];

            FormatDeliveryRange(range);

            if (JobModel.Levels[0].CostDelivery != 0)
            {
                worksheet.Range["C15"].Value = JobModel.Levels[0].CostDelivery;
            }

            worksheet.Range["A1"].Activate();
            worksheet.Protect("Secureme");

        }
        private void LoadDoubleFrameDelivery(Excel.Worksheet worksheet)
        {
            worksheet.Activate();
            worksheet.Unprotect("Secureme");

            worksheet.Range["C21"].Value = "2 STOREY, 'Separate Frame Deliveries'";

            ResetFormatDeliveryRange(worksheet);

            worksheet.Range["B15"].Value = "UPPER STOREY DELIVERY:";
            Excel.Range range = worksheet.Range["C15"];
            FormatDeliveryRange(range);
            worksheet.Range["B17"].Value = "LOWER STOREY DELIVERY:";
            range = worksheet.Range["C17"];
            FormatDeliveryRange(range);
            if (JobModel.Levels[0].CostDelivery != 0)
            {
                worksheet.Range["C17"].Value = JobModel.Levels[0].CostDelivery;
            }
            if (JobModel.Levels[1].CostDelivery != 0)
            {
                worksheet.Range["C15"].Value = JobModel.Levels[1].CostDelivery;
            }

        }
        private void LoadThreeFrameDelivery(Excel.Worksheet worksheet)
        {
            worksheet.Activate();
            worksheet.Unprotect("Secureme");
            worksheet.Range["C21"].Value = "Three STOREY, 'Separate Frame Deliveries'";
            ResetFormatDeliveryRange(worksheet);
            worksheet.Range["B15"].Value = "UPPER STOREY DELIVERY:";
            Excel.Range range = worksheet.Range["C15"];
            FormatDeliveryRange(range);
            worksheet.Range["B17"].Value = "MIDDLE LEVEL DELIVERY:";
            range = worksheet.Range["C17"];
            FormatDeliveryRange(range);
            worksheet.Range["B19"].Value = "BASEMENT LEVEL DELIVERY:";
            range = worksheet.Range["C19"];
            FormatDeliveryRange(range);

            if (JobModel.Levels[0].CostDelivery != 0)
            {
                worksheet.Range["C19"].Value = JobModel.Levels[0].CostDelivery;
            }
            if (JobModel.Levels[1].CostDelivery != 0)
            {
                worksheet.Range["C17"].Value = JobModel.Levels[1].CostDelivery;
            }
            if (JobModel.Levels[2].CostDelivery != 0)
            {
                worksheet.Range["C15"].Value = JobModel.Levels[2].CostDelivery;
            }


        }
        private void ResetFormatDeliveryRange(Excel.Worksheet worksheet)
        {
            worksheet.Range["D19"].ClearContents();
            worksheet.Range["D15", "B19"].ClearContents();
            Excel.Range range = worksheet.Range["C15", "C19"];
            range.ClearContents();
            range.ClearFormats();
            range.Locked = true;
            range.Interior.Pattern = Excel.Constants.xlSolid;
            range.Interior.PatternColorIndex = Excel.Constants.xlAutomatic;
            range.Interior.ThemeColor = Excel.XlThemeColor.xlThemeColorDark2;
            range.Interior.TintAndShade = 0;
            range.Interior.PatternTintAndShade = 0;
        }
        private void FormatDeliveryRange(Excel.Range range)
        {
            range.NumberFormat = "$#,##0";
            var border = range.Borders[Excel.XlBordersIndex.xlEdgeLeft];
            SetBoderRangeDelivery(border);
            border = range.Borders[Excel.XlBordersIndex.xlEdgeRight];
            SetBoderRangeDelivery(border);
            border = range.Borders[Excel.XlBordersIndex.xlEdgeTop];
            SetBoderRangeDelivery(border);
            border = range.Borders[Excel.XlBordersIndex.xlEdgeBottom];
            SetBoderRangeDelivery(border);
            range.Interior.Pattern = Excel.Constants.xlSolid;
            range.Interior.PatternColorIndex = Excel.Constants.xlAutomatic;
            range.Interior.ThemeColor = Excel.XlThemeColor.xlThemeColorAccent5;
            range.Interior.TintAndShade = 0.799981688894314;
            range.Interior.PatternTintAndShade = 0;
            range.Locked = false;
            range.FormulaHidden = false;
            range.HorizontalAlignment = Excel.Constants.xlCenter;
            range.VerticalAlignment = Excel.Constants.xlCenter;
        }
        private void SetBoderRangeDelivery(Excel.Border border)
        {
            border.LineStyle = Excel.XlLineStyle.xlContinuous;
            border.Color = -65536;
            border.TintAndShade = 0;
            border.Weight = Excel.XlBorderWeight.xlThin;
        }

        public void InitEngineerList()
        {
            if (Level == null) return;
            foreach (var engineerMemberInfo in JobModel.EngineerMemberList)
            {
                if (EngineerList.Contains(engineerMemberInfo))
                {
                    continue;
                }
                if ((engineerMemberInfo.LevelType == this.Level.LevelName) || engineerMemberInfo.LevelType == "Global")
                {
                    EngineerList.Add(engineerMemberInfo);
                }
            }
        }


        #endregion

        #region public
        //public string WallLevelName
        //{
        //    get => _levelName;
        //    set => SetProperty(ref _levelName, value);
        //}
        public ObservableCollection<EngineerMemberInfo> EngineerList { get; }
        public bool EngineerReferenceVisibility
        {
            get => EngineerList.Count == 0 ? true : false;
        }
        public IEntitiesManager EntitiesManager
        {
            get => _entitiesManager;
            set => SetProperty(ref _entitiesManager, value);
        }
        public PrenailWallLayer SelectedWall
        {
            get => _selectedWall;
            set
            {
                SetProperty(ref _selectedWall, value);
                if (this.EventAggregator != null)
                {
                    this.EventAggregator.GetEvent<WallEventService>().Publish(SelectedWall);
                }
            }
        }
        /// <summary>
        /// Gets or sets the beam grade list.
        /// </summary>
        public List<string> BeamGradeList
        {
            get
            {
                if (this.SelectedClient != null && this.SelectedClient.Beams != null)
                    return new List<string>(this.SelectedClient.Beams.Keys);
                else return null;
            }
        }

        /// <summary>
        /// Gets or sets the wall input data grid.
        /// </summary>
        public SfDataGrid WallInputDataGrid { get; set; }

        /// <summary>
        /// Gets or sets the wall thickness.
        /// </summary>
        public List<int> WallThicknessList { get; set; }
        public List<int> WallSpacingList { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title
        {
            get
            {
                if (this.SelectedClient!=null)
                {
                    return this.SelectedClient.Name + " - " + this.Level.LevelName;
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// Gets or sets the layers.
        /// </summary>
        //public List<Layer> Layers { get; set; }

        /// <summary>
        /// Gets the new wall row input command.
        /// </summary>
        public ICommand NewWallRowInputCommand { get; private set; }
        
        /// <summary>
        /// Gets the delete wall row command.
        /// </summary>
        public ICommand DeleteWallRowCommand { get; private set; }

        /// <summary>
        /// Gets the wall info cell value change command.
        /// </summary>
        public ICommand WallInputCellValueValidateChangeCommand { get; private set; }

        /// <summary>
        /// Gets the wall input sort.
        /// </summary>
        public ICommand WallInputSortCommand { get; private set; }

        /// <summary>
        /// Gets the wall input loaded command.
        /// </summary>
        public ICommand WallInputLoadedCommand { get; private set; }

        /// <summary>
        /// Gets the load csv file command.
        /// </summary>
        public ICommand LoadCSVFileCommand { get; private set; }

        /// <summary>
        /// Gets the add beam command.
        /// </summary>
        public ICommand AddBeamCommand { get; private set; }

        public ICommand ReFreshWallCommand { get; private set; }

        public ICommand AddBracingCommand { get; private set; }
        public ICommand ExportDataToExcelCommand { get; private set; }
        private List<WarnervaleExportData> WarnervaleExportDataList { get; set; }

        #endregion

        #region Public Method
       
        /// <summary>
        /// The on navigated to.
        /// </summary>
        /// <param name="navigationContext">
        /// The navigation context.
        /// </param>
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
            //if (this.SelectedClient!=null)
            //{
            //    //this.Title = this.SelectedClient.Name + " - " + this.Level.LevelName;
            //}
            
            //this.WallLevelName = this.Level.WallLevelName;
            // this.Studs = this.SelectedClient.Studs;
            // this.RibbonPlates = this.SelectedClient.RibbonPlates;
            // this.TopPlates = this.SelectedClient.TopPlates;
            // this.BottomPlates = this.SelectedClient.BottomPlates;
            // this.BeamGradeList = new List<string>(this.SelectedClient.Beams.Keys);
            if (this.Level.TimberWallBracings == null)
            {
                this.Level.TimberWallBracings = new ObservableCollection<Bracing>();    
            }
            
            if (this.Level.GeneralBracings == null)
            {
                this.Level.GeneralBracings = new ObservableCollection<GenericBracing>();    
            }

            if (this.Level.GeneralBracings.Count == 0 && this.JobModel.Info.Client.Name == "Prenail")
            {
                var genericBracing = new GenericBracing { BracingInfo = SelectedClient.GenericBracingBases.FirstOrDefault() };    
                this.Level.GeneralBracings.Add(genericBracing);
            }
            //LoadBeamInput();
            //.WallLayers[1].WallColorLayer.Color

        }

        public void LoadBeamInput()
        {
            var parameters =
                   new NavigationParameters
                   {
                        { "Level", this.Level },
                        { "SelectedClient", this.SelectedClient }
                   };
            if (Level != null)
            {
                this.RegionManager.RequestNavigate("BeamInputRegion", nameof(StickFrameBeamAndLintelInputView), parameters);
            }


        }

        #endregion

        #region Private method

        private void EntitiesManager_EntitiesCollectionChanged(object sender, System.EventArgs e)
        {
            CalculatorWallLength();
        }

        public void CalculatorWallLength()
        {
            this.Level.TotalWallLength = 0;
            if (EntitiesManager.Entities != null && this.Level != null && this.Level.WallLayers != null && this.Level.WallLayers.Count != 0)
            {
                var wallayers = this.Level.WallLayers;
                var entities = EntitiesManager.Entities;
                foreach (var levelWallLayer in wallayers)
                {
                    var tempLength = 0.0;
                    if (levelWallLayer.WallColorLayer == null)
                    {
                        continue;
                    }
                    foreach (var entity in entities)
                    {
                        if (entity is IWall2D iWall2D && iWall2D.WallLevelName == this.Level.LevelName)
                        {
                            if (entity.LayerName == levelWallLayer.WallColorLayer.Name)
                            {
                                tempLength += iWall2D.Length();
                            }
                        }

                    }

                    levelWallLayer.TempLength = tempLength / 1000;
                    Level.TotalWallLength += levelWallLayer.WallLength;
                }

            }
            this.RecalculatorWallTypeId();
        }
        private void PrenailFloorInputViewModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(this.SelectedClient):
                    this.RaisePropertyChanged(nameof(this.BeamGradeList));
                    break;
            }

        }

        /// <summary>
        /// The on add new beam.
        /// </summary>
        private void OnAddNewBeam()
        {
            if (this.Level.RoofBeams == null)
            {
                this.Level.RoofBeams = new ObservableCollection<Beam>();
            }
            var beamId = Level.RoofBeams.Count + 1;
            var newBeam = new Beam(BeamType.TrussBeam, Level.LevelInfo) { Id = beamId };
            this.Level.RoofBeams.Add(newBeam);
        }

        private void OnAddBracing()
        {
            var id = Level.TimberWallBracings.Count;
            var timberBracing = new Bracing();
            this.Level.TimberWallBracings.Add(timberBracing);
        }
        /// <summary>
        /// The on load csv file.
        /// </summary>
        private async void OnLoadCSVFile()
        {
            OpenFileDialogArguments dialogArgs = new OpenFileDialogArguments()
            {
                Width = 600,
                Height = 1600,
                Filters = "All files|*.*|CSv files|*.csv|Text files|*.txt"
            };
            OpenFileDialogResult result = await OpenFileDialog.ShowDialogAsync("openFileDialogHost", dialogArgs);

            if (result.File == null)
            {
                return;
            }
            this._csvFilePath = result.File;

            //this.LoadCsvLength();
            //this.LoadDataLength();
        }

        /// <summary>
        /// The load data length.
        /// </summary>
        //private void LoadCsvLength()
        //{
        //    using (var reader = new StreamReader(this._csvFilePath))
        //    using (var csvFile = new CsvReader(reader, CultureInfo.InvariantCulture))
        //    {
        //        var records = csvFile.GetRecords<WallTempLength>().ToList();
        //        //this.Level.TempLengths = new ObservableCollection<WallTempLength>(records);
        //    }
        //}

        /// <summary>
        /// The load data length.
        /// </summary>
        //private void LoadDataLength()
        //{
        //    //if (this.Level.WallLayers.Count <= 0 || this.Level.TempLengths.Count <= 0)
        //    //{
        //    //    return;
        //    //}
        //    //foreach (var wallLayer in this.Level.WallLayers)
        //    //{
        //    //    WallTempLength first = null;
        //    //    foreach (var x in this.Level.TempLengths)
        //    //    {
        //    //        if (x.Id != wallLayer.WallColorLayer.Name)
        //    //        {
        //    //            continue;
        //    //        }

        //    //        first = x;
        //    //        break;
        //    //    }

        //    //    if (first == null)
        //    //    {
        //    //        continue;
        //    //    }

        //    //    var length =
        //    //        first.Length;

        //    //    //wallLayer.TempLength = length / 1000;
        //    //}
        //}

        /// <summary>
        /// The on add new wall row.
        /// </summary>
        private void OnAddNewWallRow()
        {
            this._startItemId = this.Level.WallLayers.Count + 1;
            var data = WallLayerFactory.CreateWallLayer(JobModel.Info.Client.Name, _startItemId, this.Level.LevelInfo,
                this.SelectedClient.WallTypes[0],this.Level.LevelName);
            if (SelectedClient.Name == "Warnervale")
            {
               SetWallTypeIdForWall(data);
            }
            data.PropertyChanged += WallLayerPropertyChanged;
            this.Level.WallLayers.Add(data);
            //SelectedWall = data;
        }

        private void WallLayerPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (SelectedClient.Name=="Warnervale")
            {
                if (e.PropertyName==nameof(SelectedWall.WallType))
                {
                    //SetWallTypeIdForWall(SelectedWall);
                    //RecalculatorWallTypeId();
                    this.OnWallInputSort();
                    this.RecalculatorWallTypeId();
                }
            }
        }

        private void SetWallTypeIdForWall(WallBase wall)
        {
            var wallTypeId = 1;

            foreach (var levelWallLayer in Level.WallLayers)
            {
                if (levelWallLayer.WallType == wall.WallType && levelWallLayer!=wall)
                {
                    if (levelWallLayer.TypeId >= wallTypeId)
                    {
                        wallTypeId = levelWallLayer.TypeId + 1;
                    }
                }
            }
            wall.TypeId = wallTypeId;
        }

        private void RecalculatorWallTypeId()
        {
            var wallCountDic = new Dictionary<string, int>();

            foreach (var levelWallLayer in Level.WallLayers)
            {
                if (wallCountDic.ContainsKey(levelWallLayer.WallType.AliasName))
                {
                    wallCountDic[levelWallLayer.WallType.AliasName] += 1;
                    levelWallLayer.TypeId = wallCountDic[levelWallLayer.WallType.AliasName];
                }
                else
                {
                   
                    wallCountDic.Add(levelWallLayer.WallType.AliasName, 1);
                    levelWallLayer.TypeId = 1;
                }
            }

        }

        /// <summary>
        /// The on delete wall row.
        /// </summary>
        /// <param name="wallGrid">
        /// The wall grid.
        /// </param>
        private void OnDeleteWallRow(SfDataGrid wallGrid)
        {
            var recordId = wallGrid.SelectedIndex;

            if (recordId < 0)
            {
                return;
            }

            this.Level.WallLayers[recordId].PropertyChanged -= WallLayerPropertyChanged;
            this.Level.WallLayers.RemoveAt(recordId);

            if (this.Level.WallLayers.Count == 0)
            {
                return;
            }

            var startCount = 0;

            if (recordId == 0)
            {
                startCount = 1;
            }
            else
            {
                startCount = this.Level.WallLayers[recordId - 1].Id + 1;
            }


            var itemIndex = recordId;

            for (var i = recordId; i <= this.Level.WallLayers.Count - 1; i++)
            {
                this.Level.WallLayers[itemIndex].Id = startCount;
                startCount++;
                itemIndex++;
            }
        }

        #endregion

        #region Create method sample

        /// <summary>
        /// The on wall input sort.
        /// </summary>
        private void OnWallInputSort()
        {
            if (this.Level.WallLayers.Count > 0)
            {
                var tempData = new List<WallBase>(this.Level.WallLayers);
                tempData.Sort();
                this.Level.WallLayers = new ObservableCollection<WallBase>(tempData);

                // re Order ID
                var startId = 1;
                foreach (var wallLayer in this.Level.WallLayers)
                {
                    wallLayer.Id = startId;
                    startId++;
                }
            }
        }

        #endregion


    }
}
