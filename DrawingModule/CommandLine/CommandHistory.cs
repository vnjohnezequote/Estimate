// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Beam.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   The beam.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Prism.Mvvm;

namespace DrawingModule.CommandLine
{
    /// <summary>
    /// Defines the <see cref="CommandHistory" />
    /// </summary>
    public class CommandHistory : BindableBase
    {
        /// <summary>
        /// Gets or sets the CommandLine
        /// </summary>
        public string Command { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandHistory"/> class.
        /// </summary>
        /// <param name="commandLine">The commandLine<see cref="string"/></param>
        public CommandHistory(string commandLine)
        {
            this.Command = commandLine;
        }
    }
}
