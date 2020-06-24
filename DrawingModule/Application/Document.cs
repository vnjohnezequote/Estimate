namespace DrawingModule.Application
{
    public class Document
    {
        #region Field
        private Editor m_editor;
        #endregion
        #region Properties
        public Editor Editor => m_editor ?? (m_editor = new Editor());
        #endregion
        public Document()
        {
        }

    }
}