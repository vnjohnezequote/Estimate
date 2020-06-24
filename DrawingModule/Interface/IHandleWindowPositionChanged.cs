using DrawingModule.CommandLine;

namespace DrawingModule.Interface
{
    public interface IHandleWindowPositionChanged
    {
        void OnWindowPositionChanged(object sender, WindowPositionChangedEventArgs e);
    }
}