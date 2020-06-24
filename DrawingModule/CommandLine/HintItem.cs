using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using DrawingModule.Enums;

namespace DrawingModule.CommandLine
{
    public class HintItem: DependencyObject
    {

        #region Public Property

        public string Name
        {
            get => (string) base.GetValue(HintItem.NameProperty);
            set => base.SetValue(HintItem.NameProperty,value);
        }
        public string Value
        {
            get => (string)base.GetValue(HintItem.ValueProperty);
            set => base.SetValue(HintItem.ValueProperty, value);
        }
        
        public ICommand Command { get; set; }
        public ICommand SearchHelp { get; set; }
        public HintCategoryType Type { get; set; }
        public bool ShowImage { get; set; }
        public virtual ImageSource Image { get; set; }
        public bool ShowToolTip { get; set; }
        public virtual object ToolTip { get; set; }

        public object ToolTipHolder => !this.ShowToolTip ? null : this;

        public bool IsCommandOrSysVarHint => this.Type == HintCategoryType.Command || this.Type == HintCategoryType.SysVar;
        public bool SupportOnlineSearch { get; set; }

        #endregion

        #region Dependency Property
        public static readonly DependencyProperty NameProperty = DependencyProperty.Register("Name",typeof(string),typeof(HintItem));
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value",typeof(string),typeof(HintItem));

        #endregion

        #region Private Method

        

        #endregion

        #region Public Method

        public virtual string GetCommandExecutableString()
        {
            return this.Value;
        }
        

        #endregion
        
    }
}