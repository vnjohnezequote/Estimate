using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using DrawingModule.ViewModels;

namespace DrawingModule.CommandLine
{
    [ContentProperty("Filters")]
	public class MultiReplaceConverter : IValueConverter
	{
		public MultiReplaceConverter()
		{
			this.m_Filters = new List<ReplaceConverter>();
		}


		public List<ReplaceConverter> Filters
		{
			get
			{
				return this.m_Filters;
			}
		}
		
		public IValueConverter DefaultConverter { get; set; }
		
		public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			List<ReplaceConverter>.Enumerator enumerator = this.m_Filters.GetEnumerator();
			if (enumerator.MoveNext())
			{
				ReplaceConverter replaceConverter;
				do
				{
					replaceConverter = enumerator.Current;
					if (object.Equals(value, replaceConverter.SourceValue))
					{
						goto IL_36;
					}
				}
				while (enumerator.MoveNext());
				goto IL_42;
				IL_36:
				return replaceConverter.Convert(value, targetType, parameter, culture);
			}
			IL_42:
			if (this.DefaultConverter != null)
			{
				return this.DefaultConverter.Convert(value, targetType, parameter, culture);
			}
			return value;
		}
		
		public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			List<ReplaceConverter>.Enumerator enumerator = this.m_Filters.GetEnumerator();
			if (enumerator.MoveNext())
			{
				ReplaceConverter replaceConverter;
				do
				{
					replaceConverter = enumerator.Current;
					if (object.Equals(value, replaceConverter.TargetValue))
					{
						goto IL_36;
					}
				}
				while (enumerator.MoveNext());
				goto IL_42;
				IL_36:
				return replaceConverter.ConvertBack(value, targetType, parameter, culture);
			}
			IL_42:
			if (this.DefaultConverter != null)
			{
				return this.DefaultConverter.ConvertBack(value, targetType, parameter, culture);
			}
			return value;
		}
		
		private IValueConverter defaultConverter;
		
		private List<ReplaceConverter> m_Filters;
	}
}
