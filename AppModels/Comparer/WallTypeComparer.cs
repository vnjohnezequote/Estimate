// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WallTypeComparer.cs" company="John Nguyen">
//   JohnNguyen
// </copyright>
// <summary>
//   The wall type comparer.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using AppModels.ResponsiveData;

namespace AppModels.Comparer
{
    using System.Collections.Generic;

    /// <summary>
    /// The wall type comparer.
    /// </summary>
    public class WallTypeComparer : IComparer<object>
    {
        /// <summary>
        /// The compare.
        /// </summary>
        /// <param name="x">
        /// The x.
        /// </param>
        /// <param name="y">
        /// The y.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
       public int Compare(object x, object y)
        {
            var objectA = (WallLayer)x;
            var objectB = (WallLayer)y;
            if (objectA!= null && objectB!=null)
            {
                return objectA.TimberWallTypePoco.CompareTo(objectB.TimberWallTypePoco);    
            }

            return 0;

        }
    }
}
