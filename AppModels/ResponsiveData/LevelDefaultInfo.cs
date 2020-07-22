// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LevelDefaultInfo.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the LevelWallDefaulInfo type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


using AppModels.Enums;
using Prism.Mvvm;

namespace AppModels.ResponsiveData
{
    /// <summary>
    /// The level wall default info.
    /// </summary>
    public class LevelDefaultInfo : BindableBase
    {
        #region Private Field
        private JobWallDefaultInfo _jobDefaultInfo;
        private int _wallHeight;
        private string _externalDoorHeight;
        private string _internalDoorHeight;

        #endregion
        #region public Property

        public int WallHeight
        {
            get => this._wallHeight;
            set => this.SetProperty(ref this._wallHeight, value);
        }
        public string ExternalDoorHeight
        {
            get => string.IsNullOrEmpty(_externalDoorHeight)
                ? JobDefaultInfo.ExternalDoorHeight.ToString()
                : _externalDoorHeight;
            set => SetProperty(ref _externalDoorHeight, value);
        }
        public string InternalDoorHeight
        {
            get => string.IsNullOrEmpty(_internalDoorHeight)
                ? JobDefaultInfo.InternalDoorHeight.ToString()
                : _internalDoorHeight;
            set => SetProperty(ref _internalDoorHeight, value);
        }
        public double StepDown => JobDefaultInfo.StepDown;

        public double CeilingPitch => JobDefaultInfo.CeilingPitch;

        public JobWallDefaultInfo JobDefaultInfo
        {
            get => this._jobDefaultInfo;
            set => this.SetProperty(ref this._jobDefaultInfo, value);
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="LevelDefaultInfo"/> class.
        /// </summary>
        /// <param name="defaultDefaultInfo">
        /// The default info.
        /// </param>
        public LevelDefaultInfo(JobWallDefaultInfo defaultDefaultInfo)
        {
            this.JobDefaultInfo = defaultDefaultInfo;
            this.InitializeLevelDefault();
        }
        

        #endregion
        

        #region private Method

        private void InitializeLevelDefault()
        {
            JobDefaultInfo.PropertyChanged += JobDefaultInfo_PropertyChanged;
        }

        private void JobDefaultInfo_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "ExternalDoorHeight":
                    RaisePropertyChanged(nameof(ExternalDoorHeight));
                    RaisePropertyChanged(nameof(InternalDoorHeight));
                    break;
                case "InternalDoorHeight":
                    RaisePropertyChanged(nameof(InternalDoorHeight));
                    break;
                case "StepDown":
                    RaisePropertyChanged(nameof(StepDown));
                    break;
                case "RoofPitch":
                case "CeilingPitch":
                    RaisePropertyChanged(nameof(CeilingPitch));
                    break;
            }
        }




        #endregion

    }
}
