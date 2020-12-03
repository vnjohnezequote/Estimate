using System;
using System.Drawing;
using AppModels.CustomEntity.CustomEntitySurrogate;
using AppModels.Enums;
using AppModels.Interaface;
using AppModels.ResponsiveData.Framings;
using AppModels.ResponsiveData.Openings;
using AppModels.ViewModelEntity;
using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using devDept.Serialization;
using Attribute = devDept.Eyeshot.Entities.Attribute;

namespace AppModels.CustomEntity
{
    [Serializable]
    public class BeamEntity : BlockReference, IFraming2D,IEntityVmCreateAble
    {

        private IFraming _framingReference;
        private bool _showBeamNameOnly;
        private bool _continuesBeam;
        private Color _beamMarkedColor;
        private Color _beamLineColor;
        private string _beamLineType;
        private string _clientName;
        private bool _supportWallOver;
        private string _customAtrributeString;
        private string _beamName;

        public Line BeamLine { get; set; }
        public Attribute BeamNameAttribute { get; set; }
        public Attribute BeamQtyAttribute { get; set; }
        public Attribute LintelAttribute { get; set; }
        public Attribute ContinuesAttribute { get; set; }
        public Attribute TreatmentAttribute { get; set; }
        public Attribute SupportWallAttribute { get; set; }
        public Attribute CustomAttribute { get; set; }
        //public Guid BeamReferenceId { get; set; }
        //public MultilineText Name { get; set; }
        public Point3D BaseAttributePoint { get; set; }
        public Leader BeamLeader { get; set; }
        public Block BeamBlock
        {
            get;
            set;
        }
        public string ClientName
        {
            get => _clientName;
            set
            {
                _clientName = value;
                ChangeColorBeam();
            }
        }
        public string BeamLocation
        {
            get
            {
                if (FramingReference != null)
                {
                    if (FramingReference is Beam beamRef)
                    {
                        return beamRef.Location;
                    }

                }

                return string.Empty;
            }
            set
            {
                if (FramingReference != null)
                {
                    if (FramingReference is Beam beamRef)
                    {
                        beamRef.Location = value;
                    }

                }
            }
        }
        public string BeamNameString
        {
            get
            {
                if (!string.IsNullOrEmpty(_beamName))
                {
                    return _beamName;
                }
                if (FramingReference == null)
                {
                    return BeamNameAttribute.Value;
                }
                else
                {
                    return FramingReference.Name;
                }

            }
            set
            {

                if (FramingReference != null)
                {
                    if (FramingReference.Name != value)
                    {
                        FramingReference.Name = value;
                        _beamName = value;
                    }

                }

                if (ShowBeamNameOnly)
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        this.Attributes["Name"].Value = FramingReference.Name;
                    }
                    else
                    {
                        this.Attributes["Name"].Value = value;
                    }

                }
            }

        }
        public IFraming FramingReference
        {
            get => _framingReference;
            set
            {
                _framingReference = value;
                if (_framingReference != null)
                {
                    BeamNameString = _framingReference.Name;
                    _framingReference.PropertyChanged += FramingReferencePropertyChanged;
                }

            }
        }

        public Guid FramingReferenceId { get; set; }

        public string LevelName
        {
            get;
            set;
        }
        private BeamMarkedLocation _beamMarkedLocation;
        public BeamMarkedLocation BeamMarkedLocation
        {
            get => _beamMarkedLocation;
            set
            {
                _beamMarkedLocation = value;
                RecalculatorBeamMarkedLocation();
            }
        }
        public bool ShowBeamNameOnly
        {
            get => _showBeamNameOnly;
            set
            {
                _showBeamNameOnly = value;
                if (ShowBeamNameOnly == true)
                {
                    this.PreCalculatorAttributePoint();
                    if (this.Attributes.ContainsKey("Name"))
                    {
                        this.Attributes["Name"].Value = BeamNameString;
                    }

                    //Name.TextString = BeamNameString;
                }
                else
                {
                    if (FramingReference == null) return;
                    if (FramingReference.FramingInfo != null)
                    {
                        this.PreCalculatorAttributePoint();
                        if (this.Attributes.ContainsKey("Name"))
                        {
                            //Need to check to fix
                            this.Attributes["Name"].Value = FramingReference.FramingInfo.SizeGrade + " @ " + FramingReference.Quantity + "/" + FramingReference.QuoteLength;
                        }
                    }
                }
            }
        }

        public bool ContinuesBeam
        {
            get => _continuesBeam;
            set
            {
                _continuesBeam = value;
                PreCalculatorAttributePoint();
            }
        }

        public bool SupportWallOver
        {
            get => _supportWallOver;
            set
            {
                _supportWallOver = value;
                PreCalculatorAttributePoint();
            }
        }

        public string CustomAtrributeString
        {
            get => _customAtrributeString;
            set
            {
                _customAtrributeString = value;
                PreCalculatorAttributePoint();
            }
        }

        //public FramingTypes FramingType
        //{
        //    get
        //    {
        //        if (FramingReference != null)
        //        {
        //            return FramingReference.FramingType;
        //        }

        //        return FramingTypes.TrussBeam;
        //    }
        //    set
        //    {
        //        if (FramingReference != null)
        //        {
        //            FramingReference.FramingType = value;
        //            PreCalculatorAttributePoint();
        //        }
        //    }
        //}
        public Guid Id { get; set; }
        //public Guid LevelId { get; set; }
        //public Guid FramingSheetId { get; set; }
        //public Guid FramingReferenceId { get; }
        public double FullLength { get; set; }
       

        public BeamEntity(BeamEntity another) : base(another)
        {

        }

        public BeamEntity(BlockReference another) : base(another)
        {

        }
        public BeamEntity(Point3D insPoint, string blockName, Point3D beamStartPoint, Point3D beamEndPoint, string clientName, string levelName = "", double rotationAngleInRadians = 0, BeamMarkedLocation? beamMarkedLocation = null) : base(insPoint, blockName, rotationAngleInRadians)
        {
            Id = Guid.NewGuid();
            ClientName = clientName;
            var beamMarkAlignmentType = Text.alignmentType.MiddleCenter;
            if (string.IsNullOrEmpty(levelName))
            {
                LevelName = levelName;
            }

            if (beamMarkedLocation == null)
            {
                if (Math.Abs(beamStartPoint.Y - beamEndPoint.Y) < 0.01)
                {
                    beamMarkAlignmentType = Text.alignmentType.MiddleCenter;
                    if (beamStartPoint.X < beamEndPoint.X)
                    {
                        _beamMarkedLocation = BeamMarkedLocation.top;

                    }
                    else
                    {
                        _beamMarkedLocation = BeamMarkedLocation.bottom;
                    }
                }
                else if (Math.Abs(beamStartPoint.X - beamEndPoint.X) < 0.001)
                {

                    if (beamStartPoint.Y > beamEndPoint.Y)
                    {
                        _beamMarkedLocation = BeamMarkedLocation.right;
                    }
                    else
                    {
                        _beamMarkedLocation = BeamMarkedLocation.left;
                    }
                }
            }
            else
            {
                _beamMarkedLocation = (BeamMarkedLocation)beamMarkedLocation;
            }


            BeamBlock = new devDept.Eyeshot.Block(BlockName);
            BeamBlock.Units = linearUnitsType.Millimeters;
            BeamLine = new Line(beamStartPoint, beamEndPoint);
            BeamLine.ColorMethod = colorMethodType.byEntity;
            BeamLine.Color = _beamLineColor;
            BeamLine.LineTypeMethod = colorMethodType.byEntity;
            BeamLine.LineWeight = 5;
            BeamLine.LineTypeName = _beamLineType;
            BeamLine.LineTypeScale = 50;
            BeamLine.LineWeightMethod = colorMethodType.byEntity;
            BeamBlock.Entities.Add(BeamLine);
            beamMarkAlignmentType = CalculatorMarkedBeamPoint(out var startPoint, out var endPoint, out var beamNamePoint);
            BaseAttributePoint = beamNamePoint;
            BeamLeader = new Leader(Plane.XY, startPoint, endPoint);
            BeamLeader.ArrowheadSize = 400;
            BeamLeader.ColorMethod = colorMethodType.byEntity;
            BeamLeader.Color = _beamMarkedColor;
            BeamLeader.LineTypeMethod = colorMethodType.byEntity;
            BeamLeader.LineWeight = 3;
            BeamLeader.LineWeightMethod = colorMethodType.byEntity;
            BeamBlock.Entities.Add(BeamLeader);

            //Line offsetLine2 = offsetLine.Offset(350*_beamMarkPositionFactor, Vector3D.AxisZ) as Line;
            //Name = new MultilineText(Plane.XY, beamNamePoint,blockName,5500,500,100,beamMarkAlignmentType);
            BeamNameAttribute = new Attribute(beamNamePoint, "Name", blockName, 500);
            BeamNameAttribute.Alignment = beamMarkAlignmentType;
            BeamNameAttribute.ColorMethod = colorMethodType.byEntity;
            BeamNameAttribute.Color = _beamMarkedColor;

            ContinuesAttribute = new Attribute(beamNamePoint, "Continues", "Continues", 500);
            ContinuesAttribute.Alignment = beamMarkAlignmentType;
            ContinuesAttribute.ColorMethod = colorMethodType.byEntity;
            ContinuesAttribute.Color = _beamMarkedColor;

            SupportWallAttribute = new Attribute(beamNamePoint, "Support", "Support Wall2D Over", 500);
            SupportWallAttribute.Alignment = beamMarkAlignmentType;
            SupportWallAttribute.ColorMethod = colorMethodType.byEntity;
            SupportWallAttribute.Color = _beamMarkedColor;

            TreatmentAttribute = new Attribute(beamNamePoint, "Treatment", "H3 Treated", 500);
            TreatmentAttribute.Alignment = beamMarkAlignmentType;
            TreatmentAttribute.ColorMethod = colorMethodType.byEntity;
            TreatmentAttribute.Color = _beamMarkedColor;

            CustomAttribute = new Attribute(beamNamePoint, "Custom", "CustomNote", 500);
            CustomAttribute.Alignment = beamMarkAlignmentType;
            CustomAttribute.ColorMethod = colorMethodType.byEntity;
            CustomAttribute.Color = _beamMarkedColor;

            LintelAttribute = new Attribute(beamNamePoint, "Lintel", "Lintel Beam", 500);
            LintelAttribute.Alignment = beamMarkAlignmentType;
            LintelAttribute.ColorMethod = colorMethodType.byEntity;
            LintelAttribute.Color = _beamMarkedColor;

            //BeamNameAttribute.TextString = blockName;
            //BeamNameAttribute.Value = blockName;
            BeamBlock.Entities.Add(BeamNameAttribute);
            BeamBlock.Entities.Add(ContinuesAttribute);
            BeamBlock.Entities.Add(SupportWallAttribute);
            BeamBlock.Entities.Add(TreatmentAttribute);
            BeamBlock.Entities.Add(CustomAttribute);
            BeamBlock.Entities.Add(LintelAttribute);

            //BeamBlock.Entities.Add(Name);
            //BeamNameAttribute.Alignment = Text.alignmentType.MiddleCenter;
        }
        private Text.alignmentType CalculatorMarkedBeamPoint(out Point3D startPoint, out Point3D endPoint, out Point3D beamNamePoint)
        {
            startPoint = null;
            endPoint = null;
            beamNamePoint = null;
            switch (BeamMarkedLocation)
            {
                case BeamMarkedLocation.top:
                    startPoint = new Point3D(BeamLine.MidPoint.X, BeamLine.MidPoint.Y + 50);
                    endPoint = new Point3D(BeamLine.MidPoint.X, BeamLine.MidPoint.Y + 1940);
                    beamNamePoint = new Point3D(BeamLine.MidPoint.X, BeamLine.MidPoint.Y + 2290);
                    BaseAttributePoint = beamNamePoint;
                    return Text.alignmentType.MiddleCenter;
                case BeamMarkedLocation.bottom:
                    startPoint = new Point3D(BeamLine.MidPoint.X, BeamLine.MidPoint.Y - 50);
                    endPoint = new Point3D(BeamLine.MidPoint.X, BeamLine.MidPoint.Y - 1940);
                    beamNamePoint = new Point3D(BeamLine.MidPoint.X, BeamLine.MidPoint.Y - 2290);
                    BaseAttributePoint = beamNamePoint;
                    return Text.alignmentType.MiddleCenter;
                case BeamMarkedLocation.right:
                    startPoint = new Point3D(BeamLine.MidPoint.X + 50, BeamLine.MidPoint.Y);
                    endPoint = new Point3D(BeamLine.MidPoint.X + 1940, BeamLine.MidPoint.Y);
                    beamNamePoint = new Point3D(BeamLine.MidPoint.X + 2290, BeamLine.MidPoint.Y);
                    BaseAttributePoint = beamNamePoint;
                    return Text.alignmentType.MiddleLeft;
                case BeamMarkedLocation.left:
                    startPoint = new Point3D(BeamLine.MidPoint.X - 50, BeamLine.MidPoint.Y);
                    endPoint = new Point3D(BeamLine.MidPoint.X - 1940, BeamLine.MidPoint.Y);
                    beamNamePoint = new Point3D(BeamLine.MidPoint.X - 2290, BeamLine.MidPoint.Y);
                    BaseAttributePoint = beamNamePoint;
                    return Text.alignmentType.MiddleRight;
                default:
                    throw new ArgumentOutOfRangeException();
            }

        }
        private void PreCalculatorAttributePoint()
        {
            var continuesMovement = 0;
            var supportWallMovement = 0;
            var treatmentMovement = 0;
            var customMovement = 0;
            var lintelMovement = 0;

            var wallMoveDisctance = 0;
            var continuesMoveDisctance = 0;
            var supportMoveDisctance = 0;
            var treatMentMoveDistance = 0;
            var lintelMoveDistance = 0;
            if (ContinuesBeam)
            {
                continuesMovement = 800;
                if (this.Attributes.ContainsKey("Continues"))
                {
                    this.Attributes["Continues"].Value = "Continues";
                }

            }
            else
            {
                if (this.Attributes.ContainsKey("Continues"))
                {
                    this.Attributes["Continues"].Value = string.Empty;
                }

            }

            if (SupportWallOver)
            {
                supportWallMovement = 800;
                if (this.Attributes.ContainsKey("Support"))
                {
                    this.Attributes["Support"].Value = "Support Wall2D Over";
                }

            }
            else
            {
                if (this.Attributes.ContainsKey("Support"))
                {
                    this.Attributes["Support"].Value = string.Empty;
                }


            }

            if (FramingReference != null && FramingReference.FramingInfo != null &&
                FramingReference.FramingInfo.Treatment.Contains("H3"))
            {
                treatmentMovement = 800;
                if (this.Attributes.ContainsKey("Treatment"))
                {
                    this.Attributes["Treatment"].Value = "H3 Treated";
                }

            }
            else
            {
                if (this.Attributes.ContainsKey("Treatment"))
                {
                    this.Attributes["Treatment"].Value = string.Empty;
                }

            }
            if (!string.IsNullOrEmpty(CustomAtrributeString))
            {
                customMovement = 800;
                if (this.Attributes.ContainsKey("Custom"))
                {
                    this.Attributes["Custom"].Value = CustomAtrributeString;
                }

            }
            else
            {
                if (this.Attributes.ContainsKey("Custom"))
                {
                    this.Attributes["Custom"].Value = string.Empty;
                }

            }

            if (FramingReference != null && FramingReference.FramingType == FramingTypes.LintelBeam && !ShowBeamNameOnly)
            {
                lintelMovement = 800;
                if (this.Attributes.ContainsKey("Lintel"))
                {
                    this.Attributes["Lintel"].Value = "Lintel Beam";
                }
            }
            else
            {
                if (this.Attributes.ContainsKey("Lintel"))
                {
                    this.Attributes["Lintel"].Value = string.Empty;
                }
            }

            if (BeamMarkedLocation == BeamMarkedLocation.top)
            {
                wallMoveDisctance = supportWallMovement + treatmentMovement + continuesMovement + customMovement + lintelMovement;
                lintelMoveDistance = supportWallMovement + treatmentMovement + continuesMovement + customMovement;
                supportMoveDisctance = treatmentMovement + continuesMovement + customMovement;
                treatMentMoveDistance = continuesMovement + customMovement;
                continuesMoveDisctance = customMovement;
                if (this.Attributes.ContainsKey("Custom"))
                {
                    this.Attributes["Custom"].InsertionPoint = BaseAttributePoint;
                }

                if (this.Attributes.ContainsKey("Continues"))
                {
                    this.Attributes["Continues"].InsertionPoint = new Point3D(BaseAttributePoint.X, BaseAttributePoint.Y + continuesMoveDisctance);
                }

                if (this.Attributes.ContainsKey("Treatment"))
                {
                    this.Attributes["Treatment"].InsertionPoint = new Point3D(BaseAttributePoint.X, BaseAttributePoint.Y + treatMentMoveDistance);
                }

                if (this.Attributes.ContainsKey("Support"))
                {
                    this.Attributes["Support"].InsertionPoint = new Point3D(BaseAttributePoint.X, BaseAttributePoint.Y + supportMoveDisctance);
                }

                if (this.Attributes.ContainsKey("Lintel"))
                {
                    this.Attributes["Lintel"].InsertionPoint = new Point3D(BaseAttributePoint.X, BaseAttributePoint.Y + lintelMoveDistance);
                }

                if (this.Attributes.ContainsKey("Name"))
                {
                    this.Attributes["Name"].InsertionPoint = new Point3D(BaseAttributePoint.X, BaseAttributePoint.Y + wallMoveDisctance);
                }
            }
            else
            {

                wallMoveDisctance = -lintelMovement;
                lintelMoveDistance = -(supportWallMovement + lintelMovement);
                supportMoveDisctance = -(treatmentMovement + supportWallMovement + lintelMovement);
                treatMentMoveDistance = -(continuesMovement + treatmentMovement + supportWallMovement + lintelMovement);
                continuesMoveDisctance = -(customMovement + continuesMovement + treatmentMovement + supportWallMovement + lintelMovement); ;
                if (this.Attributes.ContainsKey("Custom"))
                {
                    this.Attributes["Custom"].InsertionPoint = new Point3D(BaseAttributePoint.X, BaseAttributePoint.Y + continuesMoveDisctance);
                }

                if (this.Attributes.ContainsKey("Continues"))
                {
                    this.Attributes["Continues"].InsertionPoint = new Point3D(BaseAttributePoint.X, BaseAttributePoint.Y + treatMentMoveDistance);
                }

                if (this.Attributes.ContainsKey("Treatment"))
                {
                    this.Attributes["Treatment"].InsertionPoint = new Point3D(BaseAttributePoint.X, BaseAttributePoint.Y + supportMoveDisctance);
                }

                if (this.Attributes.ContainsKey("Support"))
                {
                    this.Attributes["Support"].InsertionPoint = new Point3D(BaseAttributePoint.X, BaseAttributePoint.Y + lintelMoveDistance);
                }

                if (this.Attributes.ContainsKey("Lintel"))
                {
                    this.Attributes["Lintel"].InsertionPoint = new Point3D(BaseAttributePoint.X, BaseAttributePoint.Y + wallMoveDisctance);
                }

                if (this.Attributes.ContainsKey("Name"))
                {
                    this.Attributes["Name"].InsertionPoint = BaseAttributePoint;
                }
            }
        }
        private void FramingReferencePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(FramingReference.FramingInfo) || e.PropertyName == nameof(FramingReference.Quantity) || e.PropertyName == nameof(FramingReference.QuoteLength))
            {
                if (FramingReference.FramingInfo != null && !ShowBeamNameOnly)
                {
                    if (this.Attributes.ContainsKey("Name"))
                    {
                        this.Attributes["Name"].Value = FramingReference.FramingInfo.SizeGrade + " @ " + FramingReference.Quantity + "/" + FramingReference.QuoteLength;
                    }

                    this.PreCalculatorAttributePoint();
                }
            }

            if ((e.PropertyName == nameof(FramingReference.Name) || e.PropertyName == nameof(FramingReference.FramingType)) && (ShowBeamNameOnly|| FramingReference.FramingInfo==null))
            {
                if (this.Attributes.ContainsKey("Name"))
                {
                    this.Attributes["Name"].Value = BeamNameString;
                }

            }

            if (e.PropertyName == nameof(FramingReference.FramingType))
            {
                this.PreCalculatorAttributePoint();
            }

           
        }
        private void RecalculatorBeamMarkedLocation()
        {
            var textAlignment = CalculatorMarkedBeamPoint(out var startPoint, out var endPoint, out var beamNamePoint);
            BeamBlock.Entities.Remove(BeamLeader);
            BeamLeader = new Leader(Plane.XY, startPoint, endPoint);
            BeamLeader.ArrowheadSize = 400;
            BeamLeader.ColorMethod = colorMethodType.byEntity;
            BeamLeader.Color = _beamMarkedColor;
            BeamLeader.LineTypeMethod = colorMethodType.byEntity;
            BeamLeader.LineWeight = 3;
            BeamLeader.LineWeightMethod = colorMethodType.byEntity;
            BeamLeader.LayerName = "Beam";
            BeamBlock.Entities.Add(BeamLeader);
            //this.Name.InsertionPoint = beamNamePoint;
            //this.Name.Alignment = textAlignment;
            PreCalculatorAttributePoint();
            //this.Attributes["Name"].InsertionPoint = beamNamePoint;
            foreach (var attribute in Attributes)
            {
                attribute.Value.Alignment = textAlignment;
            }
        }
        public IEntityVm CreateEntityVm(IEntitiesManager entitiesManager)
        {
            return new BeamEntityVm(this, entitiesManager);
        }
        public void ChangeColorBeam()
        {
            if (ClientName == "StickFrame" || ClientName == "ITMTumu" || string.IsNullOrEmpty(ClientName))
            {
                _beamLineColor = Color.Aqua;
                _beamLineType = "Dash Space";
                _beamMarkedColor = Color.Magenta;

            }
            else if (ClientName == "Prenail")
            {
                _beamLineColor = Color.Aqua;
                _beamLineType = "Dash Space";
                _beamMarkedColor = Color.Blue;
            }
            else
            {
                _beamLineColor = Color.Blue;
                _beamLineType = "";
                _beamMarkedColor = Color.Blue;

            }

            if (BeamLine != null)
            {
                BeamLine.Color = _beamLineColor;
                BeamLine.LineTypeName = _beamLineType;
            }

            if (BeamLeader != null)
            {
                BeamLeader.Color = _beamMarkedColor;
            }

            foreach (var attribute in Attributes)
            {
                attribute.Value.Color = _beamMarkedColor;
            }
        }
        public override EntitySurrogate ConvertToSurrogate()
        {
            return new BeamEntitySurrogate(this);
        }

        public override object Clone()
        {
            return new BeamEntity(this);
            //return base.Clone();
        }
       //public void SetFramingType(FramingTypes framingType)
       // {
       //     FramingType = framingType;
       // }
    }
}
