using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoSharp
{
    /// <summary>
    /// Interface of a point. Both 2D and 3D point-like objects should implement
    /// </summary>
    public interface IPoint
    {
        float X { get; set; }
        float Y { get; set; }
        bool IsEmpty { get; }
    }
}
