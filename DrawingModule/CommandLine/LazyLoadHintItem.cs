using System.Windows.Media;

namespace DrawingModule.CommandLine
{
    public abstract class LazyLoadHintItem : AcHintItem
    {
        public override ImageSource Image
        {
            get
            {
                if (!base.ShowImage)
                {
                    return null;
                }
                if (base.Image != null)
                {
                    return base.Image;
                }
                base.Image = this.CreateItemIcon();
                return base.Image;
            }
        }
        public override object ToolTip
        {
            get
            {
                if (!base.ShowToolTip)
                {
                    return null;
                }
                if (base.ToolTip != null)
                {
                    return base.ToolTip;
                }
                base.ToolTip = this.CreateTooltip();
                return base.ToolTip;
            }
        }
        protected abstract ImageSource CreateItemIcon();
        protected abstract object CreateTooltip();
    }
}