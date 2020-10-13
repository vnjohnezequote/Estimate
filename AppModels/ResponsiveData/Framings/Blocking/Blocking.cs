namespace AppModels.ResponsiveData.Framings.Blocking
{
    public class Blocking: FramingBase
    {
        #region Field


        #endregion

        #region Properties
        public override double QuoteLength => ((double)FramingSpan / 1000);
        
        #endregion

    }
}
