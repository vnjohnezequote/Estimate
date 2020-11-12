using System;
using ApplicationInterfaceCore;
using AppModels.EventArg;
using AppModels.Interaface;
using DrawingModule.CommandLine;
using DrawingModule.DrawToolBase;
using DrawingModule.Interface;

namespace DrawingModule.Application
{
    public delegate void DrawInteractiveDelegate(ICadDrawAble drawAbleObject, DrawInteractiveArgs drawInteractiveArgs);

    public delegate void DrawingToolChanged (ICadDrawAble drawAbleObject,ToolChangedArgs toolArgs);

    public class Application
    {
        #region fields

        private static DocumentCollection m_docman;

        private static DataBindings _mUiBindings;

        #endregion

        #region public properties
        //public static CanvasDrawing DrawingModel { get; set; }

        public static DataBindings UiBindings
        {
            get
            {
                if (Application._mUiBindings == null)
                {
                    return Application._mUiBindings = new DataBindings();
                }

                return Application._mUiBindings;
            }
        }
        public static DocumentCollection DocumentManager
        {
            get
            {
                if (Application.m_docman == null)
                {
                    Application.m_docman = new DocumentCollection();
                }
                return Application.m_docman;
            }
        }
        private static event EventHandler idle;
        public static event EventHandler Idle
        {
            add => idle += value;
            remove => idle -= value;
        }
        
        #endregion
        
    }
}