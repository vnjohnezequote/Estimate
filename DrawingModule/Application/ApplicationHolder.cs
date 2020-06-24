using System;
using System.Collections.Generic;
using System.Reflection;
using DrawingModule.CommandClass;
using DrawingModule.Interface;

namespace DrawingModule.Application
{
    public class ApplicationHolder
    {
        private static CommandClass.CommandClass mCommandClass;
        public static CommandClass.CommandClass CommandClass=>mCommandClass;
        

        public static void CreateCommandList(Assembly assembly)
        {

            if (mCommandClass== null)
            {
                mCommandClass = new CommandClass.CommandClass();
            }
            /*
                       foreach (Type t in Assembly.GetExecutingAssembly().GetTypes())
                       {
                           var testMethods = from m in t.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public| BindingFlags.NonPublic)
                               where m.GetCustomAttributes(false).Any(a => a is CommandMethodAttribute)
                               select m;



                       }*/

            //foreach (Type t in Assembly.GetExecutingAssembly().GetTypes())
            
            object[] customAttributes = assembly.GetCustomAttributes(typeof(CommandClassAttribute), false);
            if (customAttributes.Length > 0)
            {
                List<Type> types = new List<Type>();

                foreach (var attribute in customAttributes)
                {
                    if (attribute is CommandClassAttribute commandClassType)
                    {
                        types.Add(commandClassType.Type);
                    }
                }

                if (types.Count>0)
                {
                    foreach (Type t in types) 
                    {
                        MethodInfo[] methods = t.GetMethods(BindingFlags.Instance | BindingFlags.Static |
                                                            BindingFlags.Public | BindingFlags.NonPublic);

                        foreach (MethodInfo method in methods)
                        {
                            object[] customAttributes2 = method.GetCustomAttributes(typeof(ICommandLineCallable), false);
                            if (customAttributes2.Length == 0)
                            {
                                //donothing
                            }
                            else
                            {

                                foreach (var atribute in customAttributes2)
                                {
                                    if (atribute is ICommandLineCallable atributeTemp)
                                        //CommandClassList.Add(new CommandClass((ICommandLineCallable) atribute, method));
                                    {
                                        mCommandClass.AddCommand((ICommandLineCallable) atribute, method);
                                    }
                                        
                                }

                            }

                        }


                    }
                }
                
            }


        }
    }
}
