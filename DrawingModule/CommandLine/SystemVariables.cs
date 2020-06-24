using System.Collections.Generic;
using DrawingModule.Interface;
using Prism.Mvvm;

namespace DrawingModule.CommandLine
{
    class SystemVariables : BindableBase, ILookup<SystemVariable>
    {
        private readonly Dictionary<string, SystemVariable> mSysVars = new Dictionary<string, SystemVariable>();


	public virtual SystemVariable this[string value]
    {
        get => null;

    }

	

	internal SystemVariables()
	{
	}

    }
}
