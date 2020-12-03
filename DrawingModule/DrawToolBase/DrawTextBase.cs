using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ApplicationInterfaceCore;
using ApplicationInterfaceCore.Enums;
using AppModels.Enums;
using devDept.Geometry;

namespace DrawingModule.DrawToolBase
{
    public class DrawTextBase: ToolBase,ITextTool
    {
        public override Point3D BasePoint { get; protected set; }
        private string _textInput;
        private double _textHeight=500;
        private double _textAngle=0;
        public virtual string TextInput {
            get => string.IsNullOrEmpty(_textInput) ? "Please enter string to insert" : _textInput;
            set => SetProperty(ref _textInput, value);

        }

        public virtual double TextHeight
        {
            get
            {
                if (Math.Abs(_textHeight) < 0.1)
                {
                    return 500;
                }

                if (_textHeight<0)
                {
                    return -1 * _textHeight;
                }

                return _textHeight;
            }
            set => SetProperty(ref _textHeight, value);
        }

        public double TextAngle { get=>_textAngle ; set=>SetProperty(ref _textAngle,value); }

        public DrawTextBase()
        {
            IsUsingTextStringTextBox = true;
            IsUsingTextStringAngleTextBox = true;
            IsUsingTextStringHeightTextBox = true;
            DefaultDynamicInputTextBoxToFocus = FocusType.TextContent;
            IsSnapEnable = true;
            IsUsingOrthorMode = true;
        }

        public override void NotifyMouseMove(object sender, MouseEventArgs e)
        {
            DynamicInput?.FocusDynamicInputTextBox(FocusType.Previous);
        }

    }
}
