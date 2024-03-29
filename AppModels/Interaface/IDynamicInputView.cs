﻿using System.ComponentModel;
using System.Windows.Input;
using AppModels.Enums;

namespace AppModels.Interaface
{
    public interface IDynamicInputView:INotifyPropertyChanged
    {
        #region Dependency Property

        #endregion


        #region Properties
        bool IsValid { get; }
        

        FocusType PreviusDynamicInputFocus { get ; }

       
        #endregion

        #region Command Line Properties

        #endregion
        #region Constructor

       
        #endregion

        #region Public Method

        bool PreProcessKeyboardInput(KeyEventArgs e);
        void FocusDynamicInputTextBox(FocusType focusType);
        void FocusLength();
        void FocusCommandTextbox();
        void FocusTextContent();
        void FocusTextHeight();
        void FocusTextArrowSize();
        void FocusAngle();
        void FocusWidth();
        void FocusTextAngle();
        void FocusLeaderSegment();
        void FocusHeight();
        void FocusScaleFactor();

        #endregion



    }
}
