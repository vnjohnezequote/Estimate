using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;

namespace DrawingModule.Rawinput
{
    public class Input : BindableBase
    {
        private double _inputLength;
        public double InputLength { get=>this._inputLength; set=>SetProperty(ref this._inputLength,value); }
    }
}
