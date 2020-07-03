using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ApplicationInterfaceCore.Enums;

namespace ApplicationInterfaceCore
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
        void FocusTextLength();
        void FocusCommandTextbox();
        void FocusTextContentInput();
        void FocusTextHeightInput();
        void FocusTextArrowSize();
        void FocusTextAngle();
        void FocusTextWidth();
        void FocusTextStringAngle();
        void FocusLeaderSegment();
        void FocusTextHeight();
        void FocusScaleFactor();

        #endregion



    }
}
