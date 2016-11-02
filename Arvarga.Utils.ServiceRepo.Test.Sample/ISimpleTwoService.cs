using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arvarga.Utils.ServiceRepo.Test.Sample
{
    public interface ISimpleTwoService : IService
    {
        string TwoMethodOne(string param);
    }
}
