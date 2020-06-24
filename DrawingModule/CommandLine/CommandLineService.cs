using System.Windows.Controls;
using DrawingModule.ViewModels;

namespace DrawingModule.CommandLine
{
    public static class CommandLineService
    {
        internal class CommandLineControlData
        {
            public DockPosition Dock
            {
                get;
                set;
            }

            public System.Windows.Point Location
            {
                get;
                set;
            }

            public System.Windows.Size Size
            {
                get;
                set;
            }

            public System.Windows.Size HistorySize
            {
                get;
                set;
            }

            public System.Windows.Size DockedSize
            {
                get;
                set;
            }

            public double InactiveOpacity
            {
                get;
                set;
            }

            public double RolloverOpacity
            {
                get;
                set;
            }

            public double WidthProportion
            {
                get;
                set;
            }
        }
        private static Border mCLIBorder;

        private static PaletteSet mCLIPaletteSet;

        private static CommandLineControlData mProfileData;

        //private static CommandLineView mCommandLineControl;
        //public static CommandLineView CommandControl {get=>CommandLineView.CommandControl;}

        internal static CommandLineControlData ProfileData
        {
            get
            {
                if (mProfileData == null)
                {
                    LoadProfile();
                }
                return mProfileData;
            }
        }

        private static void LoadProfile(bool forceReload = false)
        {
            /*
            if (!forceReload && mProfileData != null)
            {
                return;
            }
            mProfileData = new CommandLineControlData();
            ResetProfile(mProfileData);
            if (mProfileData.InactiveOpacity <= 0.0)
            {
                mProfileData.InactiveOpacity = 0.7;
                mProfileData.RolloverOpacity = 1.0;
            }
            string sysVarSafely = Util.GetSysVarSafely<string>("WSCURRENT", null);
            if (sysVarSafely != null)
            {
                string text = ProfileManager.LoadData("CommandLineControl", sysVarSafely, bCurrentProfile: true);
                if (!string.IsNullOrEmpty(text))
                {
                    try
                    {
                        XElement xElement = XElement.Parse(text);
                        if (Enum.TryParse(xElement.Attribute("Dock").Value, out DockPosition result))
                        {
                            mProfileData.Dock = result;
                        }
                        mProfileData.Location = System.Windows.Point.Parse(xElement.Attribute("Location").Value);
                        mProfileData.Size = System.Windows.Size.Parse(xElement.Attribute("Size").Value);
                        mProfileData.HistorySize = System.Windows.Size.Parse(xElement.Attribute("HistorySize").Value);
                        mProfileData.DockedSize = System.Windows.Size.Parse(xElement.Attribute("DockedSize").Value);
                        mProfileData.InactiveOpacity = double.Parse(xElement.Attribute("InactiveOpacity").Value);
                        mProfileData.RolloverOpacity = double.Parse(xElement.Attribute("RolloverOpacity").Value);
                        mProfileData.WidthProportion = double.Parse(xElement.Attribute("WidthProportion").Value);
                        if (mProfileData.Dock.HasFlag(DockPosition.AnchoredTop) || mProfileData.Dock.HasFlag(DockPosition.AnchoredBottom))
                        {
                            System.Windows.Size size = mProfileData.Size;
                            Rect canvasRect = Util.GetCanvasRect();
                            if (!canvasRect.IsEmpty)
                            {
                                size.Width = canvasRect.Width * mProfileData.WidthProportion;
                                mProfileData.Size = size;
                            }
                        }
                    }
                    catch
                    {
                    }
                }
            }*/
        }

        private static void ResetProfile(CommandLineControlData profileData, bool resetOnlySize = false)
        {
            /*
            profileData.Dock = (Util.IsAeroEffectOpened() ? DockPosition.AnchoredBottom : DockPosition.DockedInESW);
            Rect rect = Util.GetCanvasRect();
            if (rect.IsEmpty)
            {
                System.Windows.Point location = rect.Location;
                Rectangle workingArea = SystemInformation.WorkingArea;
                rect = new Rect(location, new System.Windows.Size(workingArea.Width, workingArea.Height));
            }
            profileData.Size = new System.Windows.Size(rect.Width * 0.6, 0.0);
            profileData.HistorySize = new System.Windows.Size(rect.Width * 0.6, 300.0);
            profileData.DockedSize = new System.Windows.Size(rect.Width / 2.0, 60.0);
            */
        }

        public static void SetFocusToCmdLine()
        {
            /*if (CommandLineService.CommandControl != null)
            {
                CommandLineService.CommandControl.PART_Input.Focus();
            }*/
        }

    }
}
