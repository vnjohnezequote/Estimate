﻿using System;
using System.Windows.Input;
using devDept.Geometry;
using DrawingModule.Enums;
using DrawingModule.EventArgs;
using DrawingModule.Interface;
using DrawingModule.Views;
using Prism.Mvvm;

namespace DrawingModule.DrawToolBase
{
    public abstract class ToolBase :BindableBase, IDrawInteractive
    {
        private string _toolMessage;
        private DynamicInputView _dynamicInput;
        public event EventHandler ToolMessageChanged;
        
        public virtual string ToolName { get; }
        public virtual string ToolMessage
        {
            get=>_toolMessage;
            set
            {
                SetProperty(ref _toolMessage, value);
                ToolMessageChanged?.Invoke(this,System.EventArgs.Empty);
            } 
        }
        public virtual bool IsSnapEnable { get; protected set; }
        public abstract Point3D BasePoint { get; protected set; }
        public bool IsUsingOrthorMode { get; protected set; }
        public bool IsUsingLengthTextBox { get; protected set; }
        public bool IsUsingWidthTextBox { get; protected set; }
        public bool IsUsingHeightTextBox { get; protected set; }
        public bool IsUsingAngleTextBox { get; protected set; }
        public bool IsUsingTextStringTextBox { get; protected set; }
        public bool IsUsingTextStringHeightTextBox { get; protected set; }
        public bool IsUsingTextStringAngleTextBox { get; protected set; }
        public bool IsUsingLeaderSegmentTextBox { get; protected set; }
        public bool IsUsingArrowHeadSizeTextBox { get; protected set; }
        public FocusType DefaultDynamicInputTextBoxToFocus { get; protected set; }
        public DynamicInputView DynamicInput => _dynamicInput;

        protected ToolBase()
        {
            InitForToolBase();
        }

        private void InitForToolBase()
        {
            IsSnapEnable = true;
            IsUsingOrthorMode = true;
            IsUsingLengthTextBox = false;
            IsUsingWidthTextBox = false;
            IsUsingHeightTextBox = false;
            IsUsingAngleTextBox = false;
            IsUsingTextStringTextBox = false;
            IsUsingTextStringHeightTextBox = false;
            IsUsingTextStringAngleTextBox = false;
            IsUsingLeaderSegmentTextBox = false;
            IsUsingArrowHeadSizeTextBox = false;
            DefaultDynamicInputTextBoxToFocus = FocusType.Length;
        }
        public virtual void NotifyMouseMove(object sender, MouseEventArgs e)
        {
            
        }

        public virtual void NotifyMouseDown(object sender, MouseButtonEventArgs e)
        {
            
        }

        public virtual void NotifyMouseUp(object sender, MouseButtonEventArgs e)
        {
            
        }

        public virtual void OnJigging(object sender, DrawInteractiveArgs e)
        {
            
        }

        public virtual void NotifyPreviewKeyDown(object sender, KeyEventArgs e)
        {
            
        }

        public void SetDynamicInput(DynamicInputView dynamicInput)
        {
            this._dynamicInput = dynamicInput;
        }
    }
}