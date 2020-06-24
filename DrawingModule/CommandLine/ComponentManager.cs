using DrawingModule.ViewModels;

namespace DrawingModule.CommandLine
{
    public static class ComponentManager
    {
        private static ComponentSettings mSettings;

        public static bool AreKeyTipsVisible => false;

        public static ComponentSettings Settings => mSettings;
    }
}
