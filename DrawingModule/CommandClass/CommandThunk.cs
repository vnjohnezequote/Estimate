using System;
using System.Reflection;
using System.Windows.Input;
using Prism.Commands;

namespace DrawingModule.CommandClass
{
    public class CommandThunk
    {
        #region Private Field
        
        private static int s_funcNo;
        private int m_funcNo;
        private MethodInfo m_method;
        private DrawingModule.CommandClass.CommandClass m_pCommandClass;
        private string m_globalName;
        private string m_localName;
        public Delegate Invoker { get; set; }
        //private HashSet<Document> m_docSet;
        public delegate void CommandInvokeHandler();
        private delegate int LispInvokeHandler();
        

        #endregion

        #region Constructor

        public CommandThunk(DrawingModule.CommandClass.CommandClass commandClass, string globalName, string localName, MethodInfo methodInfo, bool lisp)
        {
            int funcNo;
            if (lisp)
            {
                funcNo = s_funcNo;
                s_funcNo++;
            }
            else
            {
                funcNo = -1;
            }
            this.m_funcNo = funcNo;
            this.m_method = methodInfo;
            this.m_pCommandClass = commandClass;
            this.m_globalName = globalName;
            this.m_localName = localName;
            
            Action function = (Action)Delegate.CreateDelegate(typeof(Action), null, methodInfo);
            this.Command = new DelegateCommand(function);
            if (lisp)
            {
                //this.Invoker = new LispInvokeHandler(this.InvokeLisp);
                //Application.DocumentManager.DocumentActivated += this.OnDocumentActivated;
                //Application.DocumentManager.DocumentToBeDestroyed += this.OnDocumentToBeDestroyed;
                //this.m_docSet = new HashSet<Document>();
            }
            else
            {
                this.Invoker = new CommandInvokeHandler(this.Invoke);
            }
        }
        

        #endregion

        #region Property

        public string GlobalName => this.m_globalName;
        public string LocalName => this.m_localName;
        public int FuncNo => this.m_funcNo;
        public ICommand Command { get; set; }

        #endregion

        #region Method

        private void Invoke()
        {
            this.m_pCommandClass.Invoke(this.m_method,false);
        }

        
        

        #endregion
    }
}