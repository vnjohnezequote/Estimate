// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommandLineEditorManager.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the <see cref="CommandEditorManager" />
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using DrawingModule.Application;
using Prism.Mvvm;

namespace DrawingModule.CommandLine
{
    /// <summary>
    /// Defines the <see cref="CommandEditorManager" />
    /// </summary>
    public sealed class CommandEditorManager : BindableBase
    {
        #region Field

        private CommandEditor mActive;

        private Dictionary<Document, CommandEditor> mCommandEditors;

        #endregion

        #region Constructor

        public CommandEditorManager()
        {
            mCommandEditors = new Dictionary<Document, CommandEditor>();

            this.mActive = this.GetCommandEditor(Application.Application.DocumentManager.MdiActiveDocument);
            Application.Application.DocumentManager.DocumentCreateStarted += this.OnDocumentCreateStarted;
        }
        

        #endregion

        #region properties

        public CommandEditor ActiveEditor => this.mActive;

        private  CommandEditor GetCommandEditor(Document doc)
        {
            if (doc == null)
            {
                return null;
            }

            if (this.mCommandEditors.ContainsKey(doc))
            {
                return this.mCommandEditors[doc];
            }
            CommandEditor commandEditor = new CommandEditor();
            commandEditor.AddDocumentEvents(doc);
            this.mCommandEditors.Add(doc,commandEditor);
            return commandEditor;
        }

        private void OnDocumentCreateStarted(object sender, DocumentCollectionEventArgs e)
        {
            this.mActive = this.GetCommandEditor(e.Document);
            this.RaisePropertyChanged("ActiveEditor");
        }

        #endregion
    }
}
