  // --------------------------------------------------------------------------------------------------------------------
// <copyright file="DocumentCollection.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the DrawingModuleControler type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using DrawingModule.Application;
using DrawingModule.ViewModels;

namespace DrawingModule.CommandLine
{
    public delegate void DocumentCollectionEventHandler(object sender, DocumentCollectionEventArgs e);
    public delegate void DocumentLockModeChangedEventHandler(object sender, DocumentLockModeChangedEventArgs e);
    public delegate void DocumentActivationChangedEventHandler(object sender, DocumentActivationChangedEventArgs e);
    public delegate void DocumentLockModeWillChangeEventHandler(object sender, DocumentLockModeWillChangeEventArgs e);
    public delegate void DocumentLockModeChangeVetoedEventHandler(object sender, DocumentLockModeChangeVetoedEventArgs e);
    public delegate void DocumentDestroyedEventHandler(object sender, DocumentDestroyedEventArgs e);


    public sealed class DocumentCollection : MarshalByRefObject, ICollection
    {
        public sealed class ExecutionResult : INotifyCompletion
        {
            private Action m_continuation;

            private bool m_completed = false;

            public bool IsCompleted
            {
                [return: MarshalAs(UnmanagedType.U1)]
                get
                {
                    return m_completed;
                }
            }

            internal static void Callback(object result)
            {
                (result as ExecutionResult).Callback();
            }

            internal void Callback()
            {
                try
                {
                    m_completed = true;
                    if (m_continuation != null)
                    {
                        m_continuation();
                    }
                }
                catch (System.Exception e)
                {
                    //UnhandledExceptionFilter.CerOrShowExceptionDialog(e);
                }
            }

            internal static void Continuation(System.Threading.Tasks.Task task, object state)
            {
                System.Threading.SynchronizationContext.Current.Post(Callback, state);
            }

            internal ExecutionResult()
            {
            }

            public ExecutionResult GetAwaiter()
            {
                return this;
            }

            public void OnCompleted(Action continuation)
            {
                m_continuation = continuation;
            }

            public void GetResult()
            {
            }
        }

        private DocumentCollectionEventHandler m_pDocumentCreateStartedEvent;

        private DocumentCollectionEventHandler m_pDocumentToBeDestroyedEvent;

        private DocumentLockModeChangedEventHandler m_pDocumentLockModeChangedEvent;

        private DocumentCollectionEventHandler m_pDocumentCreationCanceledEvent;

        private DocumentCollectionEventHandler m_pDocumentToBeDeactivatedEvent;

        private DocumentActivationChangedEventHandler m_pDocumentActivationChangedEvent;

        private DocumentCollectionEventHandler m_pDocumentActivatedEvent;

        private DocumentCollectionEventHandler m_pDocumentToBeActivatedEvent;

        private DocumentCollectionEventHandler m_pDocumentBecameCurrentEvent;

        private DocumentLockModeWillChangeEventHandler m_pDocumentLockModeWillChangeEvent;

        private DocumentCollectionEventHandler m_pDocumentCreatedEvent;

        private DocumentLockModeChangeVetoedEventHandler m_pDocumentLockModeChangeVetoedEvent;

        private DocumentDestroyedEventHandler m_pDocumentDestroyedEvent;

        /*private unsafe CDocCollReactorImpl* m_docReactor;

        public unsafe DocumentSaveFormat DefaultFormatForSave
        {
            get
            {
                //IL_0014: Expected I, but got I8
                AcApDocManager* intPtr = <Module>.acDocManagerPtr();
                return (DocumentSaveFormat);
            }
            set
            {
                //IL_0017: Expected I, but got I8
                AcApDocManager* ptr = <Module>.acDocManagerPtr();
                Autodesk.AutoCAD.Runtime.Interop.Check((int));
            }
        }*/

        public bool IsApplicationContext { get; }

        public bool DocumentActivationEnabled { get; set; }

        public Document CurrentDocument { get; set; }
        private Document mdiActiveDocument;

        public Document MdiActiveDocument { 
            get{
                if (this.mdiActiveDocument == null)
                {
                    return mdiActiveDocument = new Document();
                }

                return mdiActiveDocument;
            }

        }

        public unsafe int Count { get; set; }

        bool ICollection.IsSynchronized
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                return false;
            }
        }

        object ICollection.SyncRoot => null;

        public event DocumentLockModeChangedEventHandler DocumentLockModeChanged
        {
            add
            {
                if (m_pDocumentLockModeChangedEvent == null)
                {
                    reactorAddRef(1);
                }
                m_pDocumentLockModeChangedEvent = (DocumentLockModeChangedEventHandler)Delegate.Combine(m_pDocumentLockModeChangedEvent, value);
            }
            remove
            {
                if ((m_pDocumentLockModeChangedEvent = (DocumentLockModeChangedEventHandler)Delegate.Remove(m_pDocumentLockModeChangedEvent, value)) == null)
                {
                    reactorRelease(1);
                }
            }
        }

        public event DocumentLockModeChangeVetoedEventHandler DocumentLockModeChangeVetoed
        {
            add
            {
                if (m_pDocumentLockModeChangeVetoedEvent == null)
                {
                    reactorAddRef(4);
                }
                m_pDocumentLockModeChangeVetoedEvent = (DocumentLockModeChangeVetoedEventHandler)Delegate.Combine(m_pDocumentLockModeChangeVetoedEvent, value);
            }
            remove
            {
                if ((m_pDocumentLockModeChangeVetoedEvent = (DocumentLockModeChangeVetoedEventHandler)Delegate.Remove(m_pDocumentLockModeChangeVetoedEvent, value)) == null)
                {
                    reactorRelease(4);
                }
            }
        }

        public event DocumentLockModeWillChangeEventHandler DocumentLockModeWillChange
        {
            add
            {
                if (m_pDocumentLockModeWillChangeEvent == null)
                {
                    reactorAddRef(2);
                }
                m_pDocumentLockModeWillChangeEvent = (DocumentLockModeWillChangeEventHandler)Delegate.Combine(m_pDocumentLockModeWillChangeEvent, value);
            }
            remove
            {
                if ((m_pDocumentLockModeWillChangeEvent = (DocumentLockModeWillChangeEventHandler)Delegate.Remove(m_pDocumentLockModeWillChangeEvent, value)) == null)
                {
                    reactorRelease(2);
                }
            }
        }

        public event DocumentCollectionEventHandler DocumentActivated
        {
            add
            {
                if (m_pDocumentActivatedEvent == null)
                {
                    reactorAddRef(4096);
                }
                m_pDocumentActivatedEvent = (DocumentCollectionEventHandler)Delegate.Combine(m_pDocumentActivatedEvent, value);
            }
            remove
            {
                if ((m_pDocumentActivatedEvent = (DocumentCollectionEventHandler)Delegate.Remove(m_pDocumentActivatedEvent, value)) == null)
                {
                    reactorRelease(4096);
                }
            }
        }

        public event DocumentActivationChangedEventHandler DocumentActivationChanged
        {
            add
            {
                if (m_pDocumentActivationChangedEvent == null)
                {
                    reactorAddRef(2048);
                }
                m_pDocumentActivationChangedEvent = (DocumentActivationChangedEventHandler)Delegate.Combine(m_pDocumentActivationChangedEvent, value);
            }
            remove
            {
                if ((m_pDocumentActivationChangedEvent = (DocumentActivationChangedEventHandler)Delegate.Remove(m_pDocumentActivationChangedEvent, value)) == null)
                {
                    reactorRelease(2048);
                }
            }
        }

        public event DocumentCollectionEventHandler DocumentToBeDeactivated
        {
            add
            {
                if (m_pDocumentToBeDeactivatedEvent == null)
                {
                    reactorAddRef(1024);
                }
                m_pDocumentToBeDeactivatedEvent = (DocumentCollectionEventHandler)Delegate.Combine(m_pDocumentToBeDeactivatedEvent, value);
            }
            remove
            {
                if ((m_pDocumentToBeDeactivatedEvent = (DocumentCollectionEventHandler)Delegate.Remove(m_pDocumentToBeDeactivatedEvent, value)) == null)
                {
                    reactorRelease(1024);
                }
            }
        }

        public event DocumentCollectionEventHandler DocumentToBeActivated
        {
            add
            {
                if (m_pDocumentToBeActivatedEvent == null)
                {
                    reactorAddRef(512);
                }
                m_pDocumentToBeActivatedEvent = (DocumentCollectionEventHandler)Delegate.Combine(m_pDocumentToBeActivatedEvent, value);
            }
            remove
            {
                if ((m_pDocumentToBeActivatedEvent = (DocumentCollectionEventHandler)Delegate.Remove(m_pDocumentToBeActivatedEvent, value)) == null)
                {
                    reactorRelease(512);
                }
            }
        }

        public event DocumentCollectionEventHandler DocumentBecameCurrent
        {
            add
            {
                if (m_pDocumentBecameCurrentEvent == null)
                {
                    reactorAddRef(256);
                }
                m_pDocumentBecameCurrentEvent = (DocumentCollectionEventHandler)Delegate.Combine(m_pDocumentBecameCurrentEvent, value);
            }
            remove
            {
                if ((m_pDocumentBecameCurrentEvent = (DocumentCollectionEventHandler)Delegate.Remove(m_pDocumentBecameCurrentEvent, value)) == null)
                {
                    reactorRelease(256);
                }
            }
        }

        public event DocumentCollectionEventHandler DocumentCreationCanceled
        {
            add
            {
                if (m_pDocumentCreationCanceledEvent == null)
                {
                    reactorAddRef(128);
                }
                m_pDocumentCreationCanceledEvent = (DocumentCollectionEventHandler)Delegate.Combine(m_pDocumentCreationCanceledEvent, value);
            }
            remove
            {
                if ((m_pDocumentCreationCanceledEvent = (DocumentCollectionEventHandler)Delegate.Remove(m_pDocumentCreationCanceledEvent, value)) == null)
                {
                    reactorRelease(128);
                }
            }
        }

        public event DocumentDestroyedEventHandler DocumentDestroyed
        {
            add
            {
                if (m_pDocumentDestroyedEvent == null)
                {
                    reactorAddRef(8);
                }
                m_pDocumentDestroyedEvent = (DocumentDestroyedEventHandler)Delegate.Combine(m_pDocumentDestroyedEvent, value);
            }
            remove
            {
                if ((m_pDocumentDestroyedEvent = (DocumentDestroyedEventHandler)Delegate.Remove(m_pDocumentDestroyedEvent, value)) == null)
                {
                    reactorRelease(8);
                }
            }
        }

        public event DocumentCollectionEventHandler DocumentToBeDestroyed
        {
            add
            {
                if (m_pDocumentToBeDestroyedEvent == null)
                {
                    reactorAddRef(64);
                }
                m_pDocumentToBeDestroyedEvent = (DocumentCollectionEventHandler)Delegate.Combine(m_pDocumentToBeDestroyedEvent, value);
            }
            remove
            {
                if ((m_pDocumentToBeDestroyedEvent = (DocumentCollectionEventHandler)Delegate.Remove(m_pDocumentToBeDestroyedEvent, value)) == null)
                {
                    reactorRelease(64);
                }
            }
        }

        public event DocumentCollectionEventHandler DocumentCreated
        {
            add
            {
                if (m_pDocumentCreatedEvent == null)
                {
                    reactorAddRef(32);
                }
                m_pDocumentCreatedEvent = (DocumentCollectionEventHandler)Delegate.Combine(m_pDocumentCreatedEvent, value);
            }
            remove
            {
                if ((m_pDocumentCreatedEvent = (DocumentCollectionEventHandler)Delegate.Remove(m_pDocumentCreatedEvent, value)) == null)
                {
                    reactorRelease(32);
                }
            }
        }

        public event DocumentCollectionEventHandler DocumentCreateStarted
        {
            add
            {
                if (m_pDocumentCreateStartedEvent == null)
                {
                    reactorAddRef(16);
                }
                m_pDocumentCreateStartedEvent = (DocumentCollectionEventHandler)Delegate.Combine(m_pDocumentCreateStartedEvent, value);
            }
            remove
            {
                if ((m_pDocumentCreateStartedEvent = (DocumentCollectionEventHandler)Delegate.Remove(m_pDocumentCreateStartedEvent, value)) == null)
                {
                    reactorRelease(16);
                }
            }
        }

        public void CopyTo(Document[] array, int index)
        {
            ((ICollection)this).CopyTo((Array)array, index);
        }

        private void CopyTo(Array array, int index)
        {
            int num = Count + index;
            if (array.Length < num)
            {
                throw new ArgumentOutOfRangeException("index");
            }
            IEnumerator enumerator = GetEnumerator();
            if (enumerator.MoveNext())
            {
                do
                {
                    array.SetValue(enumerator.Current, index);
                }
                while (enumerator.MoveNext());
            }
        }

        void ICollection.CopyTo(Array array, int index)
        {
            //ILSpy generated this explicit interface implementation from .override directive in CopyTo
            this.CopyTo(array, index);
        }

        internal DocumentCollection()
        {
            DocumentToBeDestroyed += OnDocumentToBeDestroyed;
        }

        internal unsafe void cleanUp()
        {
            //IL_0016: Expected I, but got I8
            //IL_001f: Expected I, but got I8
            /*CDocCollReactorImpl* docReactor = m_docReactor;
            if (docReactor != null)
            {
                CDocCollReactorImpl* ptr = docReactor;
                ;
                m_docReactor = null;
            }*/
            m_pDocumentCreateStartedEvent = null;
            m_pDocumentToBeDestroyedEvent = null;
            m_pDocumentLockModeChangedEvent = null;
            m_pDocumentCreationCanceledEvent = null;
            m_pDocumentToBeDeactivatedEvent = null;
            m_pDocumentActivationChangedEvent = null;
            m_pDocumentActivatedEvent = null;
            m_pDocumentToBeActivatedEvent = null;
            m_pDocumentBecameCurrentEvent = null;
            m_pDocumentLockModeWillChangeEvent = null;
            m_pDocumentCreatedEvent = null;
            m_pDocumentLockModeChangeVetoedEvent = null;
            m_pDocumentDestroyedEvent = null;
        }

        /*internal Document LookupDocument(Document doc)
        {
            
        }*/

        internal void AddDocument(Document doc, Document mgDoc)
        {
            //		<Module>.CDocCollReactorImpl.AddDocument(m_docReactor, doc, mgDoc);
        }

        public IEnumerator GetEnumerator()
        {
            return new DocumentIterator();
        }

        public void AppContextNewDocument(string templateFileName)
        {
            /*//IL_0012: Expected I, but got I8
            //IL_0023: Expected I, but got I8
            StringToWchar stringToWchar;
            StringToWchar* ptr = <Module>.StringToWchar.{ctor}(&stringToWchar, templateFileName);
            try
            {
                AcApDocManager* ptr2 = <Module>.acDocManagerPtr();
                char* ptr3 = (char*)(*(long*)ptr);
                Autodesk.AutoCAD.Runtime.Interop.Check((int));
            }
            catch
            {
                //try-fault
                <Module>.___CxxCallUnwindDtor(__ldftn(<Module>.StringToWchar.{dtor}), &stringToWchar);
                throw;
            }
            <Module>.StringToWchar.{dtor}(&stringToWchar);*/
        }

        public unsafe void AppContextOpenDocument(string fileName)
        {
            /*//IL_0012: Expected I, but got I8
            //IL_0023: Expected I, but got I8
            StringToWchar stringToWchar;
            StringToWchar* ptr = <Module>.StringToWchar.{ctor}(&stringToWchar, fileName);
            try
            {
                AcApDocManager* ptr2 = <Module>.acDocManagerPtr();
                char* ptr3 = (char*)(*(long*)ptr);
                Autodesk.AutoCAD.Runtime.Interop.Check((int));
            }
            catch
            {
                //try-fault
                <Module>.___CxxCallUnwindDtor(__ldftn(<Module>.StringToWchar.{dtor}), &stringToWchar);
                throw;
            }
            <Module>.StringToWchar.{dtor}(&stringToWchar);*/
        }

        /*public unsafe void AppContextRecoverDocument(string fileName)
        {
            //IL_0012: Expected I, but got I8
            //IL_0023: Expected I, but got I8
            StringToWchar stringToWchar;
            StringToWchar* ptr = <Module>.StringToWchar.{ctor}(&stringToWchar, fileName);
            try
            {
                AcApDocManager* ptr2 = <Module>.acDocManagerPtr();
                char* ptr3 = (char*)(*(long*)ptr);
                Autodesk.AutoCAD.Runtime.Interop.Check((int));
            }
            catch
            {
                //try-fault
                <Module>.___CxxCallUnwindDtor(__ldftn(<Module>.StringToWchar.{dtor}), &stringToWchar);
                throw;
            }
            <Module>.StringToWchar.{dtor}(&stringToWchar);
        }*/

        /*public void ExecuteInApplicationContext(ExecuteInApplicationContextCallback callback, object data)
        {
            //IL_0034: Expected I, but got I8
            IntPtr intPtr = (IntPtr)GCHandle.Alloc(new MyCallbackData(callback, data));
            AcApDocManager* ptr = <Module>.acDocManagerPtr();

        }*/

        /*public unsafe ExecutionResult ExecuteInCommandContextAsync(Func<object, System.Threading.Tasks.Task> callback, object data)
        {
            ExecutionResult executionResult = new ExecutionResult();
            IntPtr intPtr = (IntPtr)GCHandle.Alloc(new MyCallbackData(callback, data, executionResult));
            <Module>.AcApDocManager.beginExecuteInCommandContext(<Module>.acDocManagerPtr(), (IntPtr)<Module>.__unep@?myCallback@@$$FYAXPEAX@Z, intPtr.ToPointer());
            return executionResult;
        }*/

        /*public Document GetDocument(Database db)
        {
            //IL_0021: Expected I, but got I8
            IntPtr unmanagedObject = db.UnmanagedObject;
            AcApDocManager* ptr = <Module>.acDocManagerPtr();
            return Document.Create((AcApDocument*));
        }*/

        /*public Tuple<bool, Document> GetPendingDocumentForSwitch()
        {
            Document* doc;
            bool item = <Module>.acedPendingFiberlessDocSwitch(&doc);
            return Tuple.Create(item, Document.Create(doc));
        }*/

        internal void FireDocumentLockModeChanged(DocumentLockModeChangedEventArgs e)
        {
            if (m_pDocumentLockModeChangedEvent != null)
            {
                m_pDocumentLockModeChangedEvent(this, e);
            }
        }

        internal void FireDocumentLockModeWillChange(DocumentLockModeWillChangeEventArgs e)
        {
            if (m_pDocumentLockModeWillChangeEvent != null)
            {
                m_pDocumentLockModeWillChangeEvent(this, e);
            }
        }

        internal void FireDocumentLockModeChangeVetoed(DocumentLockModeChangeVetoedEventArgs e)
        {
            if (m_pDocumentLockModeChangeVetoedEvent != null)
            {
                m_pDocumentLockModeChangeVetoedEvent(this, e);
            }
        }

        internal void FireDocumentDestroyed(DocumentDestroyedEventArgs e)
        {
            if (m_pDocumentDestroyedEvent != null)
            {
                m_pDocumentDestroyedEvent(this, e);
            }
        }

        internal void FireDocumentCreateStarted(DocumentCollectionEventArgs e)
        {
            if (m_pDocumentCreateStartedEvent != null)
            {
                m_pDocumentCreateStartedEvent(this, e);
            }
        }

        internal void FireDocumentCreated(DocumentCollectionEventArgs e)
        {
            if (m_pDocumentCreatedEvent != null)
            {
                m_pDocumentCreatedEvent(this, e);
            }
        }

        internal void FireDocumentToBeDestroyed(DocumentCollectionEventArgs e)
        {
            if (m_pDocumentToBeDestroyedEvent != null)
            {
                m_pDocumentToBeDestroyedEvent(this, e);
            }
        }

        internal void FireDocumentCreationCanceled(DocumentCollectionEventArgs e)
        {
            if (m_pDocumentCreationCanceledEvent != null)
            {
                m_pDocumentCreationCanceledEvent(this, e);
            }
        }

        internal void FireDocumentBecameCurrent(DocumentCollectionEventArgs e)
        {
            if (m_pDocumentBecameCurrentEvent != null)
            {
                m_pDocumentBecameCurrentEvent(this, e);
            }
        }

        internal void FireDocumentToBeActivated(DocumentCollectionEventArgs e)
        {
            if (m_pDocumentToBeActivatedEvent != null)
            {
                m_pDocumentToBeActivatedEvent(this, e);
            }
        }

        internal void FireDocumentToBeDeactivated(DocumentCollectionEventArgs e)
        {
            if (m_pDocumentToBeDeactivatedEvent != null)
            {
                m_pDocumentToBeDeactivatedEvent(this, e);
            }
        }

        internal void FireDocumentActivationChanged(DocumentActivationChangedEventArgs e)
        {
            if (m_pDocumentActivationChangedEvent != null)
            {
                m_pDocumentActivationChangedEvent(this, e);
            }
        }

        internal void FireDocumentActivated(DocumentCollectionEventArgs e)
        {
            if (m_pDocumentActivatedEvent != null)
            {
                m_pDocumentActivatedEvent(this, e);
            }
        }

        private void reactorAddRef(int flag)
        {

        }

        private void reactorRelease(int flag)
        {

        }

        private void OnDocumentToBeDestroyed(object sender, DocumentCollectionEventArgs e)
        {
        }


    }
}
