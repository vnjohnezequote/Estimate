// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimberBase.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the TimberBase type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

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

        /// <summary>
        /// The treatment.
        /// </summary>
        private string _treatment;

        /// <summary>
        /// The quantities.
        /// </summary>
        private int _quantities;

        /// <summary>
        /// The unit price.
        /// </summary>
        private string _unitPrice;
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="TimberBase"/> class.
        /// </summary>
        public TimberBase()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimberBase"/> class.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="thickness">
        /// The thickness.
        /// </param>
        /// <param name="depth">
        /// The depth.
        /// </param>
        /// <param name="noItem">
        /// The no item.
        /// </param>
        /// <param name="timberGrade">
        /// The TimberGrade.
        /// </param>
        /// <param name="treatment">
        /// The treatment.
        /// </param>
        public TimberBase(int id, int thickness, int depth, int noItem, string timberGrade, string treatment = "H2S")
        {
            this.Id = id;
            this.Thickness = thickness;
            this.Depth = depth;
            this.NoItem = noItem;
            this.TimberGrade = timberGrade;
            this.Treatment = treatment;
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
            this.Treatment = info.Treatment;
        }
        #endregion
        /// <summary>
        /// Gets or sets the timber id.
        /// </summary>
        public int Id { get; set; }

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
        /// Gets or sets the treatment.
        /// </summary>
        public string Treatment
        {
            get => this._treatment;
            set
            {
                this.SetProperty(ref this._treatment, value);
                this.CallBackPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the quantities.
        /// </summary>
        public int Quantities
        {
            get => this._quantities;
            set => this.SetProperty(ref this._quantities, value);
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
                    return this.Size + " " + this._timberGrade;
                }
            }

        }

        /// <summary>
        /// Gets or sets the size treatment. 90x35 H2S Treated
        /// </summary>
        public string SizeTreatment => this.Size + " " + this._treatment;

        /// <summary>
        /// Gets or sets the full name. 90x35 MGP12 H2S Treated
        /// </summary>
        public string FullName => this.SizeGrade + " " + this._treatment;

        /// <summary>
        /// Gets or sets the supplier.
        /// </summary>
        public string Supplier { get; set; }

        /// <summary>
        /// Gets or sets the customer.
        /// </summary>
        public string Customer { get; set; }

        /// <summary>
        /// Gets or sets the unit price.
        /// </summary>
        public string UnitPrice { get => this._unitPrice; set => this.SetProperty(ref this._unitPrice, value); }

        /// <summary>
        /// The call back property changed.
        /// </summary>
        private void CallBackPropertyChanged()
        {
            this.RaisePropertyChanged(nameof(this.Size));
            this.RaisePropertyChanged(nameof(this.SizeGrade));
            this.RaisePropertyChanged(nameof(this.SizeTreatment));
            this.RaisePropertyChanged(nameof(this.FullName));
        }

    }
}
