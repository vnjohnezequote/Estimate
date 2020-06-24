using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace DrawingModule.CommandLine
{
    public static class MoreFunction
    {
        public static bool GetShowContextMenuOnClick(DependencyObject obj)
        {
            return (bool)obj.GetValue(MoreFunction.ShowContextMenuOnClickProperty);
        }

        // Token: 0x06000BCF RID: 3023 RVA: 0x000287F8 File Offset: 0x000277F8
        public static void SetShowContextMenuOnClick(DependencyObject obj, bool value)
        {
            obj.SetValue(MoreFunction.ShowContextMenuOnClickProperty, value);
        }

        // Token: 0x06000BD0 RID: 3024 RVA: 0x0002880C File Offset: 0x0002780C
        private static void OnShowContextMenuOnClickChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ButtonBase buttonBase = sender as Button;
            if (buttonBase == null)
            {
                return;
            }
            if ((bool)e.NewValue)
            {
                buttonBase.Click += MoreFunction.Button_Click;
                return;
            }
            buttonBase.Click -= MoreFunction.Button_Click;
        }

        // Token: 0x06000BD1 RID: 3025 RVA: 0x00028858 File Offset: 0x00027858
        private static void Button_Click(object sender, RoutedEventArgs e)
        {
            ButtonBase buttonBase = sender as Button;
            if (buttonBase == null)
            {
                return;
            }
            ContextMenu contextMenu = buttonBase.ContextMenu;
            if (contextMenu == null)
            {
                return;
            }
            contextMenu.PlacementTarget = buttonBase;
            contextMenu.Placement = PlacementMode.Bottom;
            contextMenu.IsOpen = true;
        }

        // Token: 0x0400040C RID: 1036
        public static readonly DependencyProperty ShowContextMenuOnClickProperty = DependencyProperty.RegisterAttached("ShowContextMenuOnClick", typeof(bool), typeof(MoreFunction), new PropertyMetadata(false, new PropertyChangedCallback(MoreFunction.OnShowContextMenuOnClickChanged)));
    }
}
