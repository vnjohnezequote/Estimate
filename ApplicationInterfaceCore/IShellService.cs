// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IShellService.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the IShellService type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;

namespace ApplicationInterfaceCore
{
    /// <summary>
    /// The ShellService interface.
    /// </summary>
    public interface IShellService
    {
        /// <summary>
        /// The show shell.
        /// </summary>
        void ShowShell<T>() where T:Window;
    }
}