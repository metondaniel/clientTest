using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Service.Interfaces
{
    public interface IMapperService
    {
        TDest Map<TDest>(object source);
        TDest Map<TSource, TDest>(TSource source);
    }
}
