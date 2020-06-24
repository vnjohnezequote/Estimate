// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimberBase.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the TimberBase type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AppModels
{
    using Prism.Mvvm;

    /// <summary>
    /// The timber base.
    /// </summary>
    public class TimberBase : BindableBase
    {
        #region Private Member

        /// <summary>
        /// The thickness.
        /// </summary>
        private int thickness;

        /// <summary>
        /// The depth.
        /// </summary>
        private int depth;

        /// <summary>
        /// The no item.
        /// </summary>
        private int noItem = 1;

        /// <summary>
        /// The grade.
        /// </summary>
        private string grade;

        /// <summary>
        /// The treatment.
        /// </summary>
        private string treatment;

        /// <summary>
        /// The quantities.
        /// </summary>
        private int quantities;

        /// <summary>
        /// The unit price.
        /// </summary>
        private string unitPrice;
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
        /// <param name="grade">
        /// The grade.
        /// </param>
        /// <param name="treatment">
        /// The treatment.
        /// </param>
        public TimberBase(int id, int thickness, int depth, int noItem, string grade, string treatment = "H2S")
        {
            this.Id = id;
            this.Thickness = thickness;
            this.Depth = depth;
            this.NoItem = noItem;
            this.Grade = grade;
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
            this.Grade = info.Grade;
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
            get => this.thickness;
            set
            {
                this.SetProperty(ref this.thickness, value);
                this.CallBackPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the depth.
        /// </summary>
        public int Depth
        {
            get => this.depth;
            set
            {
                this.SetProperty(ref this.depth, value);
                this.CallBackPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the number of item.
        /// </summary>
        public int NoItem
        {
            get => this.noItem;
            set
            {
                this.SetProperty(ref this.noItem, value);
                this.CallBackPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the treatment.
        /// </summary>
        public string Treatment
        {
            get => this.treatment;
            set
            {
                this.SetProperty(ref this.treatment, value);
                this.CallBackPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the quantities.
        /// </summary>
        public int Quantities
        {
            get => this.quantities;
            set => this.SetProperty(ref this.quantities, value);
        }

        /// <summary>
        /// Gets or sets the type. MGP12 or MPG10
        /// </summary>
        public string Grade
        {
            get => this.grade;
            set
            {
                this.SetProperty(ref this.grade, value);
                this.CallBackPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the size.
        /// Size = NoItem/thickness x depth 2/90x35
        /// 90x35
        /// </summary>
        public string Size => this.noItem == 1 ? this.thickness + "x" + this.depth : this.noItem + "/" + this.thickness + "x" + this.depth;

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
                    return this.Size + " " + this.grade;
                }
            }

        }

        /// <summary>
        /// Gets or sets the size treatment. 90x35 H2S Treated
        /// </summary>
        public string SizeTreatment => this.Size + " " + this.treatment;

        /// <summary>
        /// Gets or sets the full name. 90x35 MGP12 H2S Treated
        /// </summary>
        public string FullName => this.SizeGrade + " " + this.treatment;

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
        public string UnitPrice { get => this.unitPrice; set => this.SetProperty(ref this.unitPrice, value); }

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
