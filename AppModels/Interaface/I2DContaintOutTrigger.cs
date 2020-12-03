using System;
using System.Collections.Generic;
using AppModels.CustomEntity;
using AppModels.PocoDataModel.Framings.FloorAndRafter;

namespace AppModels.Interaface
{
    public interface I2DContaintOutTrigger
    {
        Guid? OutTriggerAId { get; set; }
        Guid? OutTriggerBId { get; set; }
        OutTrigger2D OutTriggerA { get; set; }
        OutTrigger2D OutTriggerB { get; set; }
        bool IsOutTriggerA { get; set; }
        bool IsOutTriggerB { get; set; }
        bool OutTriggerAFlipped { get; set; }
        bool OutTriggerBFlipped { get; set; }
        void SetFlippedOutriggerA(bool outTriggerAFlipped);
        void SetFlippedOutriggerB(bool outTriggerBFlipped);
    }
}