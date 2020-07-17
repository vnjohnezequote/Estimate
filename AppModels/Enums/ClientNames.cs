// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ClientNames.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the ClientNames type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using ProtoBuf;

namespace AppModels.Enums
{
    [ProtoContract]
    public enum ClientNames
    {
        [ProtoEnum]
        Warnervale,
        [ProtoEnum]
        Prenail,
        [ProtoEnum]
        Rivo,
        [ProtoEnum]
        StickFrame,
        [ProtoEnum]
        ITMTumu
    }
}