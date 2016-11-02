using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arvarga.Utils.ServiceRepo.Test.Sample
{
    public interface ISimpleOneService : IService
    {
        string OneMethodOne(string param);
    }
}
