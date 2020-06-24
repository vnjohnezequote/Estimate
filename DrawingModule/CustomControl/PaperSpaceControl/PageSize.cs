using DrawingModule.Enums;

namespace DrawingModule.CustomControl.PaperSpaceControl
{
    public class PageSize
    {
        private double _pageWidth;
        private double _pageHeight;
        private double _scaleFactor;
        private PageLayout _pageLayout;
        public PageSize(double pageWidth, double pageHeight, double scaleFactor, PageLayout pageLayout)
        {
            _pageWidth = pageWidth;
            _pageHeight = pageHeight;
            _scaleFactor = scaleFactor;
            _pageLayout = pageLayout;
        }

        public double PageWidth => _pageWidth;
        public double PageHeight => _pageHeight;
        public double ScaleFactor => _scaleFactor;
        public PageLayout PageLayout => _pageLayout;
    }
}
