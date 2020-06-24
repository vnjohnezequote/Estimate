using System;
using System.Collections.Generic;
using System.Reflection;
using System.Resources;
using System.Runtime.Remoting.Messaging;
using System.Windows.Input;
using AppModels.ApplicationData;

namespace DrawingModule.ViewModels
{
    delegate void CommandHandler();
    public class CommandClass
    {

        private List<CommandThunk> mCommandThunks = new List<CommandThunk>();
        public List<CommandThunk> CommandThunks => mCommandThunks;
        private ICommand Command;
        private object _commandObject;
        private bool _isProcessingTool;
        public CommandClass()
        {
            this._isProcessingTool = false;
        }
        public CommandClass(ICommandLineCallable attribute, MethodInfo methodInfo)
        {
            
            // Action function = (Action)Delegate.CreateDelegate(typeof(Action), null, methodInfo);
            // this.Command = new DelegateCommand(function);
        }
/*
        public void InvokeCommand()
        {
            this.Command.Execute(null);
        }
*/
        
        public virtual void Invoke(MethodInfo mi, bool bLispFunction)
        {
            var objectType = mi.DeclaringType;
            var magicConstructor = objectType.GetConstructor(Type.EmptyTypes);
            var magicClassObject = magicConstructor.Invoke(new object[] { });
            var objectInstance = Activator.CreateInstance(objectType, null);
            this._commandObject = objectInstance;
            this.InvokeWorkerWithExceptionFilter(mi, objectInstance, bLispFunction);
            

        }

        private void InvokeWorker(MethodInfo methodInfo, object commandObject, bool bLispFunction)
        {
            if (bLispFunction)
            {
                // Do nothing
            }
            else
            {
                //var toolMethod = Delegate.CreateDelegate(typeof(CommandHandler), commandObject, methodInfo) as CommandHandler;

                (Delegate.CreateDelegate(typeof(CommandHandler), commandObject, methodInfo) as CommandHandler)?.BeginInvoke(new AsyncCallback(DoSomeThingCompleteAsyncComplete), null);
            }
            
        }
        private void DoSomeThingCompleteAsyncComplete(IAsyncResult doResult)
        {
            AsyncResult result = (AsyncResult)doResult;
            CommandHandler caller = (CommandHandler)result.AsyncDelegate;
            // Call EndInvoke to retrieve the results.
            caller.EndInvoke(doResult);
            if(this._commandObject is IDisposable disposableObject)
                disposableObject.Dispose();
            //_commandObject = null;
        }

        protected void InvokeWorkerWithExceptionFilter(MethodInfo methodInfo, object commandObject, bool bLispFunction)
        {
            this.InvokeWorker(methodInfo,commandObject,bLispFunction);
        }

        public void AddCommand(ICommandLineCallable ca, MethodInfo mi)
        {
            string groupName = ca.GroupName;
            Type reflectedType = mi.ReflectedType;
            if (groupName==null)
            {
                if (reflectedType != null) groupName = reflectedType.Assembly.FullName;
            }

            string localName = ca.LocalizedNameId;
            if (localName==null)
            {
                localName = ca.GlobalName;
            }
            else
            {
                string @string = new ResourceManager(reflectedType).GetString(localName);
                if (@string != null)
                {
                    localName = @string;
                }
            }

            int temFlag = (int)ca.Flags;
            int temFlag2 = (int) CommandFlags.Transparent;
            int temFlag3 = (int) CommandFlags.Modal;
            
            bool flag = (temFlag >> 11 & temFlag2) != temFlag3;
            CommandThunk commandThunk = new CommandThunk(this, ca.GlobalName, localName, mi, false);
            //this.mCommandGroups.Add(groupName);
            this.mCommandThunks.Add(commandThunk);
        }
        
    }
}
