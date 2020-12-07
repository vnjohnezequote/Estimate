using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppModels.Enums;
using AppModels.Interaface;
using AppModels.ResponsiveData;
using AppModels.ResponsiveData.Framings;
using Prism.Mvvm;

namespace AppModels.ExportData.FrameData
{
    public class FrameJob: BindableBase
    {
        #region private

        private string _createDate;
        private string _refNo;
        private string _clientName;
        private string _jobName;
        private string _address;
        private string _windRate;
        private string _roofType;
        private string _treatement;
        
        #endregion
        public string CreateDate { get; set; }
        public string RefNo { get; set; }
        public string ClientName { get; set; }
        public string JobName { get; set; }
        public string Address { get; set; }
        public string WindRate { get; set; }
        public string RoofType { get; set; }
        public string Treatement { get; set; }
        public FramingList FrameList { get; set; } = new FramingList();

        #region Constructor

        public FrameJob(IJob job,FramingSheet framingSheet,string levelName = "")
        {
            CreateDate = DateTime.Now.ToShortDateString();
            ClientName = job.Info.Client.Name;
            JobName = job.Info.JobNumber;
            switch (framingSheet.FramingSheetType)
            {
                case FramingSheetTypes.FloorFraming when string.IsNullOrEmpty(levelName):
                    JobName += " - Floor";
                    break;
                case FramingSheetTypes.FloorFraming:
                    JobName += " - " + levelName + " Floor";
                    break;
                case FramingSheetTypes.CeilingJoistFraming when string.IsNullOrEmpty(levelName):
                    JobName += " - Ceiling";
                    break;
                case FramingSheetTypes.CeilingJoistFraming:
                    JobName += " - " + levelName + " Ceiling";
                    break;
                case FramingSheetTypes.RafterFraming when string.IsNullOrEmpty(levelName):
                    JobName += " - Rafter";
                    break;
                case FramingSheetTypes.RafterFraming:
                    JobName += " - " + levelName + " Rafter";
                    break;
                case FramingSheetTypes.PurlinFraming when string.IsNullOrEmpty(levelName):
                    JobName += " - Purlin";
                    break;
                case FramingSheetTypes.PurlinFraming:
                    JobName += " - " + levelName + " Purlin";
                    break;
            }

            Address = job.Info.FullAddress;
            WindRate = job.Info.WindRate;
            RoofType = job.Info.RoofMaterial;
            Treatement = job.Info.Treatment;
            foreach (var framing in framingSheet.Joists)
            {
                var framingData = new FrameData(framing, framing.Name);
                FrameList.Add(framingData);
            }

            foreach (var framing in framingSheet.OutTriggers)
            {
                var framingData = new FrameData(framing,framing.Name);
                FrameList.Add(framingData);
            }

            foreach (var framing in framingSheet.Beams )       
            {
                var framingData = new FrameData(framing, framing.Name);
                FrameList.Add(framingData);
            }

            foreach (var framing in framingSheet.Blockings)
            {
                var framingData = new FrameData(framing, framing.Name);
                FrameList.Add(framingData);
            }

            foreach (var framing in framingSheet.Hangers)
            {
                var framingData = new FrameData(framing, framing.Name);
                FrameList.Add(framingData);
            }
        }

        #endregion

        public void ExportToExcel()
        {

        }
    }
}
