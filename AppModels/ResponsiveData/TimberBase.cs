// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimberBase.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the TimberBase type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using devDept.Serialization;
using Prism.Mvvm;

namespace AppModels.ResponsiveData
{
    /// <summary>
    /// The timber base.
    /// </summary>
    public class TimberBase : BindableBase
    {
        #region Private Member

        /// <summary>
        /// The thickness.
        /// </summary>
        private int _thickness;

        /// <summary>
        /// The depth.
        /// </summary>
        private int _depth;

        /// <summary>
        /// The no item.
        /// </summary>
        private int _noItem = 1;

        /// <summary>
        /// The TimberGrade.
        /// </summary>
        private string _timberGrade;

        private string _treatment;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="TimberBase"/> class.
        /// </summary>
        public TimberBase()
        {
        }

        public TimberBase(int id, int thickness, int depth, int noItem, string timberGrade)
        {
            this.Id = id;
            this.Thickness = thickness;
            this.Depth = depth;
            this.NoItem = noItem;
            this.TimberGrade = timberGrade;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimberBase"/> class.
        /// </summary>
        /// <param name="info">
        /// The info.
        /// </param>
        public TimberBase(TimberBase info)
        {
            this.Id = info.Id;
            this.Thickness = info.Thickness;
            this.Depth = info.Depth;
            this.NoItem = info.NoItem;
            this.TimberGrade = info.TimberGrade;
        }
        #endregion
        /// <summary>
        /// Gets or sets the timber id.
        /// </summary>
        public int Id { get; set; }

        
        /// /// <summary>
        /// Gets or sets the number of item.
        /// </summary>
        public int NoItem
        {
            get => this._noItem;
            set
            {
                this.SetProperty(ref this._noItem, value);
                this.CallBackPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the thickness.
        /// </summary>
        public int Thickness
        {
            get => this._thickness;
            set
            {
                this.SetProperty(ref this._thickness, value);
                this.CallBackPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the depth.
        /// </summary>
        public int Depth
        {
            get => this._depth;
            set
            {
                this.SetProperty(ref this._depth, value);
                this.CallBackPropertyChanged();
            }
        }

        

       /// <summary>
        /// Gets or sets the type. MGP12 or MPG10
        /// </summary>
        public string TimberGrade
        {
            get => this._timberGrade;
            set
            {
                this.SetProperty(ref this._timberGrade, value);
                this.CallBackPropertyChanged();
            }
        }

       public string Treatment {
           get=>_treatment;
           set
           {
               SetProperty(ref _treatment,value);
               CallBackPropertyChanged();
           }

       }


        /// <summary>
        /// Gets or sets the size.
        /// Size = NoItem/thickness x depth 2/90x35
        /// 90x35
        /// </summary>
        public string Size => this._noItem == 1 ? this._thickness + "x" + this._depth : this._noItem + "/" + this._thickness + "x" + this._depth;

        /// <summary>
        /// Gets or sets the name. 2/90x35 MGP10
        /// </summary>
        public string SizeGrade
        {
            get
            {
                if (this.Thickness == 0 || this.Depth == 0)
                {
                    return "Nil";
                }
                else
                {
                    return this.Size + " " + this.TimberGrade;
                }
            }

        }
        public string SizeTreatment
        {
            get
            {
                if (Thickness == 0 || Depth == 0)
                {
                    return "Nil";
                }
                else
                {
                    return this.Size + " " + this.Treatment;
                }
            }
        }
        public string SizeGradeTreatment
        {
            get
            {
                if (Thickness == 0 || Depth == 0)
                {
                    return "Nil";
                }
                else
                {
                    return this.SizeGrade + " " + this.Treatment;
                }
            }
        }
        public decimal UnitPrice { get; set; }
        public decimal UnitCostPrice { get; set; }
        public decimal UnitCostSell { get; set; }
        public double MaximumLength { get; set; }
        public string Code { get; set; }


        /// <summary>
        /// The call back property changed.
        /// </summary>
        private void CallBackPropertyChanged()
        {
            this.RaisePropertyChanged(nameof(this.Size));
            this.RaisePropertyChanged(nameof(this.SizeGrade));
            this.RaisePropertyChanged(nameof(SizeGradeTreatment));
            this.RaisePropertyChanged(nameof(SizeTreatment));
        }

    }
}
