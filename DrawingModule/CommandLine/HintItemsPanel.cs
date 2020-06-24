using System;
using System.Windows;
using System.Windows.Controls;

namespace DrawingModule.CommandLine
{
    public class HintItemsPanel : VirtualizingStackPanel
    {
       
		protected override Size MeasureOverride(Size constraint)
		{
			double itemHeight = this.GetItemHeight();
			if (itemHeight != double.NaN)
			{
				constraint.Height = itemHeight * (double)this.MaxItemCount;
			}
			return base.MeasureOverride(constraint);
		}

		
		private double GetItemHeight()
		{
			ItemsControl itemsOwner = ItemsControl.GetItemsOwner(this);
			if (itemsOwner == null || itemsOwner.ItemTemplate == null)
			{
				return double.NaN;
			}
			FrameworkElement frameworkElement = itemsOwner.ItemTemplate.LoadContent() as FrameworkElement;
			TextBlock textBlock = frameworkElement.FindName("mHintText") as TextBlock;
			if (textBlock == null)
			{
				throw new Exception("Can't found mHintText member");
			}
			textBlock.FontFamily = itemsOwner.FontFamily;
			textBlock.FontSize = itemsOwner.FontSize;
			textBlock.FontStyle = itemsOwner.FontStyle;
			textBlock.FontWeight = itemsOwner.FontWeight;
			textBlock.Text = "TEST TEXT";
			frameworkElement.Measure(new Size(double.MaxValue, double.MaxValue));
			FrameworkElement frameworkElement2 = null;
			double num = 0.0;
			if (itemsOwner.ItemContainerStyle != null && itemsOwner.ItemContainerStyle.Setters != null)
			{
				foreach (SetterBase setterBase in itemsOwner.ItemContainerStyle.Setters)
				{
					Setter setter = setterBase as Setter;
					if (setter != null && setter.Property == System.Windows.Controls.Control.TemplateProperty)
					{
						frameworkElement2 = ((setter.Value as ControlTemplate).LoadContent() as FrameworkElement);
						frameworkElement2.Measure(new Size(double.MaxValue, double.MaxValue));
					}
					else if (setter != null && setter.Property == System.Windows.Controls.Control.BorderThicknessProperty)
					{
						Thickness thickness = (Thickness)setter.Value;
						num = thickness.Top + thickness.Bottom;
					}
				}
			}
			if (frameworkElement2 == null)
			{
				return frameworkElement.DesiredSize.Height;
			}
			return frameworkElement.DesiredSize.Height + frameworkElement2.DesiredSize.Height + num;
		}
		
		public int MaxItemCount { get; set; } 
    }
}