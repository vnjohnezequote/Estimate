﻿
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Resources;
using System.Runtime.Remoting.Messaging;
using System.Windows.Input;
using ApplicationInterfaceCore;
using DrawingModule.Enums;
using DrawingModule.Interface;

namespace DrawingModule.CommandClass
{
    delegate void CommandHandler();
    public class CommandClass
    {

        private List<CommandThunk> mCommandThunks = new List<CommandThunk>();
        public List<CommandThunk> CommandThunks => mCommandThunks;
        private ICommand Command;
        private object _commandObject;
        private readonly bool _isProcessingTool;
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
            var canvas = Application.Application.DocumentManager.MdiActiveDocument.Editor.CanvasDrawing;
            
            var objectType = mi.DeclaringType;
            //var magicConstructor = objectType.GetConstructor(Type.EmptyTypes);
            //var magicClassObject = magicConstructor.Invoke(new object[] { });
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
                var canvas = Application.Application.DocumentManager.MdiActiveDocument.Editor.CanvasDrawing;
                canvas.SetDrawing((IDrawInteractive)commandObject);
                (Delegate.CreateDelegate(typeof(CommandHandler), commandObject, methodInfo) as CommandHandler)?.BeginInvoke(new AsyncCallback(DoSomeThingCompleteAsyncComplete), null);
            }
            
        }
        private void DoSomeThingCompleteAsyncComplete(IAsyncResult doResult)
        {
            AsyncResult result = (AsyncResult)doResult;
            CommandHandler caller = (CommandHandler)result.AsyncDelegate;
            // Call EndInvoke to retrieve the results.
            caller.EndInvoke(doResult);
            var canvas = Application.Application.DocumentManager.MdiActiveDocument.Editor.CanvasDrawing;
            canvas.ReleaseDrawing();
            // if(this._commandObject is IDisposable disposableObject)
            //     disposableObject.Dispose();
            // this._commandObject = null;
            // var editor = Application.Application.DocumentManager.MdiActiveDocument.Editor;
            //     editor.ResetProcessTool();
            
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

            if (!CheckIfCommandExisting(commandThunk))
            {
                this.mCommandThunks.Add(commandThunk);
            }

        }

        private bool CheckIfCommandExisting(CommandThunk commandThunk)
        {
            foreach (var mCommandThunk in this.mCommandThunks)
            {
                if (mCommandThunk.GlobalName == commandThunk.GlobalName)
                {
                    return true;
                }
            }

            return false;
        }
        
    }
}
