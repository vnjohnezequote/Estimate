using System;
using System.Globalization;
using System.Windows.Data;

namespace DrawingModule.CommandLine
{
    public class ReplaceConverter : IValueConverter
	{
		
		public object SourceValue
		{
			get
			{
				return this.m_sourceValue;
			}
			set
			{
				this.m_sourceValue = value;
				this.m_bIsSrcInitialized = true;
			}
		}

		
		public object TargetValue
		{
			get
			{
				return this.m_targetValue;
			}
			set
			{
				this.m_targetValue = value;
				this.m_bIsTgtInitialized = true;
			}
		}

		
		public IValueConverter Converter { get; set; }

		
		public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (this.m_bIsSrcInitialized)
			{
				object sourceValue = this.m_sourceValue;
				if (object.Equals(value, sourceValue))
				{
					if (this.m_bIsTgtInitialized)
					{
						return this.m_targetValue;
					}
					IValueConverter valueConverter = this.Converter;
					if (valueConverter != null)
					{
						object sourceValue2 = this.m_sourceValue;
						return valueConverter.Convert(sourceValue2, targetType, parameter, culture);
					}
				}
			}
			return value;
		}

		
		public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (this.m_bIsTgtInitialized)
			{
				object targetValue = this.m_targetValue;
				if (object.Equals(value, targetValue))
				{
					if (this.m_bIsSrcInitialized)
					{
						return this.m_sourceValue;
					}
					IValueConverter valueConverter = this.Converter;
					if (valueConverter != null)
					{
						object targetValue2 = this.m_targetValue;
						return valueConverter.ConvertBack(targetValue2, targetType, parameter, culture);
					}
				}
			}
			return value;
		}

		
		private IValueConverter converter;

		
		private object m_sourceValue = null;

		
		private object m_targetValue = null;

		
		private bool m_bIsSrcInitialized = false;

		
		private bool m_bIsTgtInitialized = false;
	}
}
