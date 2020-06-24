using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;
using DrawingModule.Enums;
using DrawingModule.ViewModels;

namespace DrawingModule.CommandLine
{
    public class CommandLineSettings
    {
        public class ContentItem
	{
		private string mStringKey;

		private bool mEnabled;

		private static Dictionary<string, string> NameMap;

		public string Name => NameMap[mStringKey];

		public string StringKey => mStringKey;

		public bool Enabled => mEnabled;

		public HintItemType Type
		{
			get
			{
				if (mStringKey.Length == 1)
				{
					switch (mStringKey[0])
					{
					case 'B':
						return HintItemType.Block;
					case 'L':
						return HintItemType.Layer;
					case 'H':
						return HintItemType.Hatch;
					case 'V':
						return HintItemType.VisualStyle;
					case 'T':
						return HintItemType.TextStyle;
					case 'D':
						return HintItemType.DimStyle;
					}
				}
				if (NameMap.ContainsKey(mStringKey))
				{
					return HintItemType.Extended;
				}
				return HintItemType.Block;
			}
		}

		public bool IsSupported { get; set; }
		/*{
			get
			{
				if (mStringKey.Length == 1)
				{
					switch (mStringKey[0])
					{
					case 'B':
						return SubSetting.IsCommandEnabled("BLOCK");
					case 'L':
						return SubSetting.IsCommandEnabled("LAYER");
					case 'H':
						return SubSetting.IsCommandEnabled("HATCH");
					case 'V':
						return SubSetting.IsCommandEnabled("VISUALSTYLES");
					case 'T':
						return SubSetting.IsCommandEnabled("STYLE");
					case 'D':
						return SubSetting.IsCommandEnabled("DIMSTYLE");
					}
				}
				if (NameMap.ContainsKey(mStringKey))
				{
					return true;
				}
				return false;
			}
		}*/

		public static ContentItem CreateWithName(string name, bool enabled)
		{
			NameMap.ContainsValue(name);
			ContentItem contentItem = new ContentItem();
			foreach (KeyValuePair<string, string> item in NameMap)
			{
				if (item.Value == name)
				{
					contentItem.mStringKey = item.Key;
				}
			}
			contentItem.mEnabled = enabled;
			return contentItem;
		}

		public static ContentItem CreateWithKey(string key, bool enabled)
		{
			NameMap.ContainsKey(key);
			ContentItem contentItem = new ContentItem();
			contentItem.mStringKey = key;
			contentItem.mEnabled = enabled;
			return contentItem;
		}

		protected ContentItem()
		{
			mStringKey = string.Empty;
			mEnabled = false;
		}

		static ContentItem()
		{
            /*
			NameMap = new Dictionary<string, string>();
			NameMap.Add("B", ResxResource.GetString("CML_Content_Block"));
			NameMap.Add("L", ResxResource.GetString("CML_Content_Layer"));
			NameMap.Add("H", ResxResource.GetString("CML_Content_Hatch"));
			NameMap.Add("V", ResxResource.GetString("CML_Content_VisualStyle"));
			NameMap.Add("T", ResxResource.GetString("CML_Content_TextStyle"));
			NameMap.Add("D", ResxResource.GetString("CML_Content_DimStyle"));*/
		}

		public static void AddExtendedItem(string key, string name)
		{
			Regex regex = new Regex("([^a-zA-Z]+)");
			if (!regex.IsMatch(key))
			{
				NameMap.Add(key, name);
			}
		}

		public static void RemoveExtendedItem(string key)
		{
			NameMap.Remove(key);
		}
	}

	private const string INPUTSEARCHOPTIONFLAGS = "INPUTSEARCHOPTIONFLAGS";

	private const string REMEMBERMISTYPE = "REMEMBERMISTYPE";

	private const string MISTYPETIMES = "MISTYPETIMES";

	private const string CMDSYSVARSORTMODE = "CMDSYSVARSORTMODE";

	private const string CMDSYSVARSEPARATE = "CMDSYSVARSEPARATE";

	private const string INPUTSEARCHDELAY = "INPUTSEARCHDELAY";

	private const string TOOLTIPS = "TOOLTIPS";

	private const string CONTENTTYPEORDER = "CONTENTTYPEORDER";

	private List<ContentItem> mContentOrderList;

	public static CommandLineSettings Instance
	{
		get;
		private set;
	}

	private InputSearchOptions InputSearchOptionFlags { get; set; }
	//{
	//	get
	//	{
	//		//return (InputSearchOptions)Util.GetSysVarSafely("INPUTSEARCHOPTIONFLAGS", (short)0);
	//	}
	//	set
	//	{
	//		//Util.SetSysVarSafely("INPUTSEARCHOPTIONFLAGS", (short)value);
	//	}
	//}

	private string ContentTypeOrder { get; set; }
	//{
	//	get
	//	{
	//		return Util.GetSysVarSafely("CONTENTTYPEORDER", "B1L1H1T1D1V1");
	//	}
	//	set
	//	{
	//		Util.SetSysVarSafely("CONTENTTYPEORDER", value);
	//	}
	//}

	public List<ContentItem> ContentOrderList
	{
		get
		{
			if (mContentOrderList == null)
			{
				mContentOrderList = new List<ContentItem>();
				string contentTypeOrder = ContentTypeOrder;
				Regex regex = new Regex("([a-zA-Z]+[01])");
				MatchCollection matchCollection = regex.Matches(contentTypeOrder);
				foreach (Match item in matchCollection)
				{
					string key = item.Value.Substring(0, item.Value.Length - 1);
					bool enabled = (string.Compare(item.Value.Substring(item.Value.Length - 1), "1") == 0) ? true : false;
					ContentItem contentItem = ContentItem.CreateWithKey(key, enabled);
					if (contentItem.IsSupported)
					{
						mContentOrderList.Add(contentItem);
					}
				}
			}
			return mContentOrderList;
		}
		set
		{
			string text = string.Empty;
			if (value != null)
			{
				foreach (ContentItem item in value)
				{
					text = text.Insert(text.Length, item.StringKey);
					text = text.Insert(text.Length, item.Enabled ? "1" : "0");
				}
			}
			ContentTypeOrder = text;
			mContentOrderList = null;
		}
	}

	public bool IsAcadLT { get; set; }
	//{
	//	get
	//	{
	//		string program = HostApplicationServices.Current.Program;
	//		return string.Compare(program, "ACADLT", ignoreCase: true) == 0;
	//	}
	//}

    public bool AutoComplete = true;
	//{
	//	get
	//	{
	//		return InputSearchOptionFlags.HasFlag(InputSearchOptions.AutoComplete);
	//	}
	//	set
	//	{
	//		InputSearchOptions inputSearchOptionFlags = InputSearchOptionFlags;
	//		InputSearchOptionFlags = (value ? (inputSearchOptionFlags | InputSearchOptions.AutoComplete) : (inputSearchOptionFlags & ~InputSearchOptions.AutoComplete));
	//	}
	//}

    public bool AutoCorrect = true;
	//{
	//	get
	//	{
	//		return InputSearchOptionFlags.HasFlag(InputSearchOptions.AutoCorrect);
	//	}
	//	set
	//	{
	//		InputSearchOptions inputSearchOptionFlags = InputSearchOptionFlags;
	//		InputSearchOptionFlags = (value ? (inputSearchOptionFlags | InputSearchOptions.AutoCorrect) : (inputSearchOptionFlags & ~InputSearchOptions.AutoCorrect));
	//	}
	//}

	//public bool SupportAutoCorrect => AutoCorrectorService.IsAutoCorrectSupported();

    public bool SearchContent = true;
	//{
	//	get
	//	{
	//		return InputSearchOptionFlags.HasFlag(InputSearchOptions.ContentSearch);
	//	}
	//	set
	//	{
	//		InputSearchOptions inputSearchOptionFlags = InputSearchOptionFlags;
	//		InputSearchOptionFlags = (value ? (inputSearchOptionFlags | InputSearchOptions.ContentSearch) : (inputSearchOptionFlags & ~InputSearchOptions.ContentSearch));
	//	}
	//}

	public bool DisplaySysvar
	{
		get
		{
			return InputSearchOptionFlags.HasFlag(InputSearchOptions.SysvarSearch);
		}
		set
		{
			InputSearchOptions inputSearchOptionFlags = InputSearchOptionFlags;
			InputSearchOptionFlags = (value ? (inputSearchOptionFlags | InputSearchOptions.SysvarSearch) : (inputSearchOptionFlags & ~InputSearchOptions.SysvarSearch));
		}
	}

    public bool DisplaySuggestionList = true;
	/*{
		get
		{
			InputSearchOptions inputSearchOptionFlags = InputSearchOptionFlags;
			if (!inputSearchOptionFlags.HasFlag(InputSearchOptions.AutoComplete) && !inputSearchOptionFlags.HasFlag(InputSearchOptions.AutoCorrect))
			{
				return inputSearchOptionFlags.HasFlag(InputSearchOptions.ContentSearch);
			}
			return true;
		}
	}*/

	public bool SupportAutoAppend
	{
		get
		{
			InputSearchOptions inputSearchOptionFlags = InputSearchOptionFlags;
			if (!inputSearchOptionFlags.HasFlag(InputSearchOptions.AutoComplete))
			{
				return inputSearchOptionFlags.HasFlag(InputSearchOptions.AutoCorrect);
			}
			return true;
		}
	}

	public bool MidString
	{
		get
		{
			return InputSearchOptionFlags.HasFlag(InputSearchOptions.MidString);
		}
		set
		{
			InputSearchOptions inputSearchOptionFlags = InputSearchOptionFlags;
			InputSearchOptionFlags = (value ? (inputSearchOptionFlags | InputSearchOptions.MidString) : (inputSearchOptionFlags & ~InputSearchOptions.MidString));
		}
	}

	public bool RememberMisType { get; set; }
	/*{
		get
		{
			return Util.GetSysVarSafely("REMEMBERMISTYPE", (short)1) != 0;
		}
		set
		{
			Util.SetSysVarSafely("REMEMBERMISTYPE", (short)(value ? 1 : 0));
		}
	}*/

	public short MisTypeTimes { get; set; }
	//{
	//	get
	//	{
	//		return Util.GetSysVarSafely("MISTYPETIMES", (short)3);
	//	}
	//	set
	//	{
	//		Util.SetSysVarSafely("MISTYPETIMES", value);
	//	}*/
	//}

	public bool SortByUsage { get; set; }
	//{
	//	get
	//	{
	//		return Util.GetSysVarSafely("CMDSYSVARSORTMODE", (short)1) != 0;
	//	}
	//	set
	//	{
	//		Util.SetSysVarSafely("CMDSYSVARSORTMODE", (short)(value ? 1 : 0));
	//	}
	//}

	public bool SeparateSysvar { get; set; }
	//{
	//	get
	//	{
	//		return Util.GetSysVarSafely("CMDSYSVARSEPARATE", (short)1) != 0;
	//	}
	//	set
	//	{
	//		Util.SetSysVarSafely("CMDSYSVARSEPARATE", (short)(value ? 1 : 0));
	//	}
	//}

	public short DelayTime {get; set; }
	//{
	//	get
	//	{
	//		return Util.GetSysVarSafely("INPUTSEARCHDELAY", (short)300);
	//	}
	//	set
	//	{
	//		Util.SetSysVarSafely("INPUTSEARCHDELAY", value);
	//	}
	//}

	public bool ShowTooltip { get; set; }
	//{
	//	get
	//	{
	//		return Util.GetSysVarSafely("TOOLTIPS", (short)1) != 0;
	//	}
	//	set
	//	{
	//		Util.SetSysVarSafely("TOOLTIPS", (short)(value ? 1 : 0));
	//	}
	//}

	public event PropertyChangedEventHandler SettingsChnaged;

	static CommandLineSettings()
	{
		Instance = new CommandLineSettings();
	}

	public CommandLineSettings()
	{
		SystemVariable systemVariable = Application.Application.UiBindings.SystemVariables["INPUTSEARCHOPTIONFLAGS"];
		if (systemVariable != null)
		{
			systemVariable.PropertyChanged += SysVarChanged;
		}
		systemVariable = Application.Application.UiBindings.SystemVariables["CONTENTTYPEORDER"];
		if (systemVariable != null)
		{
			systemVariable.PropertyChanged += SysVarChanged;
		}
	}

	private void SysVarChanged(object sender, PropertyChangedEventArgs e)
	{
		SystemVariable systemVariable = sender as SystemVariable;
		if (systemVariable != null)
		{
			if (systemVariable.Name.ToUpper() == "CONTENTTYPEORDER")
			{
				mContentOrderList = null;
			}
			else if (systemVariable.Name.ToUpper() == "INPUTSEARCHOPTIONFLAGS" && this.SettingsChnaged != null)
			{
				this.SettingsChnaged(this, new PropertyChangedEventArgs("INPUTSEARCHOPTIONFLAGS"));
			}
		}
	}
    }
}