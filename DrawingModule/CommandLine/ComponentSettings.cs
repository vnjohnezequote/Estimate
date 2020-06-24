using DrawingModule.ViewModels;
using Prism.Mvvm;

namespace DrawingModule.CommandLine
{
    public sealed class ComponentSettings : BindableBase
    {
        private KeyTipManager mKeyTipManager = new KeyTipManager();
        public KeyTipManager KeyTipManager => mKeyTipManager;
    }
}
