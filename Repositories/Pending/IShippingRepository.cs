using AdoNetWindow.Model;
using Libs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IShippingRepository 
    {
        ShippingModel GetShipping(string id);
        StringBuilder InsertShipping(ShippingModel model);
        StringBuilder DeleteShipping(int id);
        StringBuilder UpdateShipping(ShippingModel model);
    }
}
