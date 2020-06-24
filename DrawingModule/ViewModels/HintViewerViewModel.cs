using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using DrawingModule.CommandLine;
using DrawingModule.Enums;

namespace DrawingModule.ViewModels
{
    public class HintViewerViewModel : DependencyObject
    {
        static HintViewerViewModel()
        {
            UIPropertyMetadata typeMetadata = new UIPropertyMetadata(null, new PropertyChangedCallback(HintViewerViewModel.CurrentCategoryChangedCallback));
            HintViewerViewModel.CurrentCategoryProperty = DependencyProperty.Register("CurrentCategory", typeof(HintCategory), typeof(HintViewerViewModel), typeMetadata);
        }
        
        private static void CurrentCategoryChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            HintViewerViewModel hintViewerViewModel = d as HintViewerViewModel;
            hintViewerViewModel.RaiseCurrentCategoryChangedEvent(e);
        }
        
        public static List<HintCategoryType> SortedCategoryTypes
        {
            get
            {
                if (HintViewerViewModel.mSortedCategoryTypes == null)
                {
                    HintViewerViewModel.mSortedCategoryTypes = new List<HintCategoryType>();
                    HintViewerViewModel.mSortedCategoryTypes.Add(HintCategoryType.Command);
                    HintViewerViewModel.mSortedCategoryTypes.Add(HintCategoryType.SysVar);
                    HintViewerViewModel.mSortedCategoryTypes.Add(HintCategoryType.DwgContent);
                }
                return HintViewerViewModel.mSortedCategoryTypes;
            }
        }
        
        public static HintViewerViewModel Instance
        {
            get
            {
                if (HintViewerViewModel.mHintViewerViewModel == null)
                {
                    HintViewerViewModel.mHintViewerViewModel = new HintViewerViewModel();
                }
                return HintViewerViewModel.mHintViewerViewModel;
            }
        }
        
        public event PropertyChangedCallback CurrentCategoryChanged;
        public event PropertyChangedCallback CurrentHintChanged;
        public ObservableCollection<HintCategory> HintCategories { get; private set; }
        
        public HintViewerViewModel()
        {
            this.HintCategories = new ObservableCollection<HintCategory>();
        }
        
        public void CleanUp()
        {
            this.CurrentCategory = null;
            foreach (HintCategory hintCategory in this.HintCategories)
            {
                hintCategory.CleanUp();
                hintCategory.SelectedHintChanged -= this.HintCategorySelectedHintChanged;
            }
            this.HintCategories.Clear();
        }
        public HintCategory CurrentCategory
        {
            get => (HintCategory)base.GetValue(HintViewerViewModel.CurrentCategoryProperty);
            set => SetValue(HintViewerViewModel.CurrentCategoryProperty, value);
        }
        public HintItem CurrentHint
        {
            get
            {
                if (this.CurrentCategory != null)
                {
                    return this.CurrentCategory.SelectedHint;
                }
                return null;
            }
            set
            {
                if (value == null)
                {
                    if (this.CurrentCategory != null)
                    {
                        this.CurrentCategory.SelectedHint = null;
                    }
                    return;
                }
                HintCategory category = this.GetCategory(value.Type);
                if (category == null)
                {
                    throw new Exception("Invalid Hint Category type");
                }
                if (!category.Hints.Contains(value))
                {
                    throw new Exception("The Hint Item is not in the list");
                }
                if (this.CurrentCategory != category)
                {
                    this.CurrentCategory = category;
                }
                if (this.CurrentCategory.SelectedHint != value)
                {
                    this.CurrentCategory.SelectedHint = value;
                }
            }
        }
        
        public int HintsCount
        {
            get
            {
                int num = 0;
                foreach (HintCategory hintCategory in this.HintCategories)
                {
                    num += hintCategory.Hints.Count;
                }
                return num;
            }
        }
        
        public void AddHint(HintItem hint)
        {
            if (hint == null)
            {
                throw new Exception("Invalid hint item");
            }
            HintCategory hintCategory = this.GetCategory(hint.Type);
            if (hintCategory != null)
            {
                hintCategory.Hints.Add(hint);
                return;
            }
            hintCategory = new HintCategory(this);
            hintCategory.Type = hint.Type;
            hintCategory.SelectedHintChanged += this.HintCategorySelectedHintChanged;
            int num = HintViewerViewModel.SortedCategoryTypes.IndexOf(hintCategory.Type);
            if (num < 0 || num >= HintViewerViewModel.SortedCategoryTypes.Count)
            {
                throw new Exception("unexpected hint type");
            }
            int num2 = 0;
            foreach (HintCategory hintCategory2 in this.HintCategories)
            {
                if (num < HintViewerViewModel.SortedCategoryTypes.IndexOf(hintCategory2.Type))
                {
                    break;
                }
                num2++;
            }
            this.HintCategories.Insert(num2, hintCategory);
            hintCategory.Hints.Add(hint);
            hintCategory.SelectFirstHint();
            
        }
        public HintCategory GetCategory(HintCategoryType type)
        {
            foreach (HintCategory hintCategory in this.HintCategories)
            {
                if (hintCategory.Type == type)
                {
                    return hintCategory;
                }
            }
            return null;
        }
        public void SelectCategory(HintCategoryType type)
        {
            HintCategory category = this.GetCategory(type);
            if (category != null)
            {
                this.CurrentCategory = category;
            }
        }
        public void SelectFirstCategory()
        {
            if (this.HintCategories.Count > 0 && this.CurrentCategory != this.HintCategories[0])
            {
                this.CurrentCategory = this.HintCategories[0];
            }
        }
        public void SelectNextCategory()
        {
            int num = this.HintCategories.IndexOf(this.CurrentCategory);
            if (num < 0 || num >= this.HintCategories.Count - 1)
            {
                this.SelectFirstCategory();
                return;
            }
            this.CurrentCategory = this.HintCategories[num + 1];
        }
        public void SelectPreviousCategory()
        {
            int num = this.HintCategories.IndexOf(this.CurrentCategory);
            if (num < 0)
            {
                this.SelectFirstCategory();
                return;
            }
            if (num == 0)
            {
                this.CurrentCategory = this.HintCategories[this.HintCategories.Count - 1];
                return;
            }
            this.CurrentCategory = this.HintCategories[num - 1];
        }
        public void SelectFirstHintInCurrentCategory()
        {
            if (this.CurrentCategory == null)
            {
                return;
            }
            this.CurrentCategory.SelectFirstHint();
        }
        public void SelectNextHintInCurrentCategory()
        {
            if (this.CurrentCategory == null)
            {
                return;
            }
            this.CurrentCategory.SelectNextHint();
        }
        public void SelectPreviousHintInCurrentCategory()
        {
            if (this.CurrentCategory == null)
            {
                return;
            }
            this.CurrentCategory.SelectPreviousHint();
        }
        public void SelectHintInCurrentCategoryByOffset(int iOffset)
        {
            if (this.CurrentCategory == null)
            {
                return;
            }
            this.CurrentCategory.SelectHintByOffset(iOffset);
        }
        private void RaiseCurrentCategoryChangedEvent(DependencyPropertyChangedEventArgs e)
        {
            HintCategory hintCategory = e.OldValue as HintCategory;
            if (hintCategory != null && hintCategory.SelectedHint == null)
            {
                hintCategory.SelectFirstHint();
            }
            if (this.CurrentCategoryChanged != null)
            {
                this.CurrentCategoryChanged(this, e);
            }
            if (this.CurrentHintChanged != null)
            {
                HintItem oldValue = null;
                if (hintCategory != null)
                {
                    oldValue = hintCategory.SelectedHint;
                }
                HintItem newValue = null;
                HintCategory hintCategory2 = e.NewValue as HintCategory;
                if (hintCategory2 != null)
                {
                    newValue = hintCategory2.SelectedHint;
                }
                DependencyProperty selectedHintProperty = HintCategory.SelectedHintProperty;
                DependencyPropertyChangedEventArgs e2 = new DependencyPropertyChangedEventArgs(selectedHintProperty, oldValue, newValue);
                this.CurrentHintChanged(this, e2);
            }
        }
        private void HintCategorySelectedHintChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            if (this.CurrentHintChanged != null && o == this.CurrentCategory)
            {
                this.CurrentHintChanged(this, e);
            }
        }
        
        public static int MaxItemsCount = 7;

        // Token: 0x04000467 RID: 1127
        public static readonly DependencyProperty CurrentCategoryProperty;

        // Token: 0x04000468 RID: 1128
        private static List<HintCategoryType> mSortedCategoryTypes = null;

        // Token: 0x04000469 RID: 1129
        private static HintViewerViewModel mHintViewerViewModel = null;
        
       

        //public string Text { get=>this.text; set=>this.SetProperty(ref this.text,value); }
        

    }
}
