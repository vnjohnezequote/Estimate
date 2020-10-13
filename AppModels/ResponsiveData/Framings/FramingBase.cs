﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;

namespace AppModels.ResponsiveData.Framings
{
    public abstract class FramingBase: BindableBase
    {
        #region Field
        private double _pitch;
        private bool _isExisting;
        private FramingBase _framingInfo;
        private int _framingSpan;
        private int _quantities;
        #endregion

        #region Properties
        public bool IsExisting { get => _isExisting; set => SetProperty(ref _isExisting, value); }
        public abstract double QuoteLength { get; }
        public int FramingSpan { get=>_framingSpan; set=>SetProperty(ref _framingSpan,value); }
        public double Pitch { get=>_pitch; set=>SetProperty(ref _pitch,value); }
        public FramingBase FramingInfo { get=>_framingInfo; set=>SetProperty(ref _framingInfo,value); }
        #endregion

        #region Constructor

        protected FramingBase()
        {
            PropertyChanged += FramingBasePropertyChanged;
        }
        #endregion

        #region Private Method
        private void FramingBasePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(FramingSpan):
                case nameof(Pitch):
                    NotifyPropertiesChanged();
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region Protected Method
        protected virtual void NotifyPropertiesChanged()
        {
            RaisePropertyChanged(nameof(QuoteLength));
        }
        #endregion
        #region Public Method
        #endregion

    }
}
