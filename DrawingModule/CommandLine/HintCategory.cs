using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Data;
using DrawingModule.Enums;
using DrawingModule.ViewModels;

namespace DrawingModule.CommandLine
{
    public class HintCategory : DependencyObject
    {
        static HintCategory()
        {
            HintCategory.IsExpandedProperty = DependencyProperty.Register("IsExpanded", typeof(bool), typeof(HintCategory));
            UIPropertyMetadata typeMetadata = new UIPropertyMetadata(null, new PropertyChangedCallback(HintCategory.SelectedHintChangedCallback));
            HintCategory.SelectedHintProperty = DependencyProperty.Register("SelectedHint", typeof(HintItem), typeof(HintCategory), typeMetadata);
        }
        private static void SelectedHintChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            HintCategory hintCategory = d as HintCategory;
            hintCategory.RaiseSelectedHintChangedEvent(e);
        }
        private static void BindingData(DependencyObject target, DependencyProperty dp, object source, string path, BindingMode mode, IValueConverter converter, object converterParameter)
        {
            BindingOperations.SetBinding(target, dp, new Binding(path)
            {
                Source = source,
                Mode = mode,
                Converter = converter,
                ConverterParameter = converterParameter
            });
        }
        public event PropertyChangedCallback SelectedHintChanged;
        public HintCategoryType Type { get; set; }
        public ObservableCollection<HintItem> Hints { get; private set; }
        internal HintCategory(HintViewerViewModel hintViewer)
        {
            this.Hints = new ObservableCollection<HintItem>();
            HintCategory.BindingData(this, HintCategory.IsExpandedProperty, hintViewer, "CurrentCategory", BindingMode.TwoWay, HintCategory.isExpandedConverter, this);
        }
        public void CleanUp()
        {
            this.SelectedHint = null;
            this.Hints.Clear();
            BindingOperations.ClearAllBindings(this);
        }
        public bool IsExpanded
        {
            get
            {
                return (bool)base.GetValue(HintCategory.IsExpandedProperty);
            }
            set
            {
                base.SetValue(HintCategory.IsExpandedProperty, value);
            }
        }
        public HintItem SelectedHint
        {
            get
            {
                return (HintItem)base.GetValue(HintCategory.SelectedHintProperty);
            }
            set
            {
                base.SetValue(HintCategory.SelectedHintProperty, value);
            }
        }
        private void RaiseSelectedHintChangedEvent(DependencyPropertyChangedEventArgs e)
        {
            if (this.SelectedHintChanged != null)
            {
                this.SelectedHintChanged(this, e);
            }
        }
        public void SelectFirstHint()
        {
            if (this.Hints.Count > 0 && this.SelectedHint != this.Hints[0])
            {
                this.SelectedHint = this.Hints[0];
            }
        }
        public void SelectNextHint()
        {
            int num = this.Hints.IndexOf(this.SelectedHint);
            if (num < 0 || num >= this.Hints.Count - 1)
            {
                this.SelectFirstHint();
                return;
            }
            this.SelectedHint = this.Hints[num + 1];
        }
        public void SelectPreviousHint()
        {
            int num = this.Hints.IndexOf(this.SelectedHint);
            if (num <= 0)
            {
                this.SelectedHint = this.Hints[this.Hints.Count - 1];
                return;
            }
            this.SelectedHint = this.Hints[num - 1];
        }
        public void SelectHintByOffset(int iOffset)
        {
            int num = this.Hints.IndexOf(this.SelectedHint);
            num = Math.Min(this.Hints.Count - 1, Math.Max(0, num + iOffset));
            this.SelectedHint = this.Hints[num];
        }
        public static readonly DependencyProperty IsExpandedProperty;

        
        public static readonly DependencyProperty SelectedHintProperty;
        
        private static HintCategoryIsExpandedConverter isExpandedConverter = new HintCategoryIsExpandedConverter();
    }
}
