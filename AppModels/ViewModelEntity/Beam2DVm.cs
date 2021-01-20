using AppModels.CustomEntity;
using AppModels.Interaface;
using devDept.Eyeshot.Entities;

namespace AppModels.ViewModelEntity
{
    public class Beam2DVm : EntityContainhangerAndOutTriggerVm
    {
        #region Field



        #endregion
        #region Properties
        public bool BeamUnder
        {
            get
            {
                if (Entity is Beam2D beam)
                {
                    return beam.IsBeamUnder;
                }

                return false;
            }
            set
            {
                if (Entity is Beam2D beam)
                {
                    beam.IsBeamUnder = value;
                    RaisePropertyChanged(nameof(BeamUnder));
                    RaisePropertyChanged(nameof(Color));
                }
            }
        }
        
        #endregion


        public Beam2DVm(Entity entity, IEntitiesManager entityManager) : base(entity,entityManager)
        {
            
        }
    }
}