using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arvarga.Utils.ServiceRepo.Test.Sample
{
    public class SimpleOneService : ISimpleOneService
    {
        public void Init(IServiceRepository serviceRepository)
        {
        }

        public string OneMethodOne(string param)
        {
            return $"OneMethodOne: {param}";
        }
    }
}
