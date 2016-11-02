using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arvarga.Utils.ServiceRepo.Test.Sample
{
    public class ServicesOneToThreeAggregate : IAggregateService
    {
        public IList<IService> GetChildServices()
        {
            return new List<IService>
            {
                new SimpleOneService(),
                new SimpleTwoService(),
                new CompoundThreeService(),
            };
        }

        public void Init(IServiceRepository serviceRepository)
        {
        }
    }
}
