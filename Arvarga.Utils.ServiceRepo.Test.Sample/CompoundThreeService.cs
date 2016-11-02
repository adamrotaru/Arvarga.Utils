using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arvarga.Utils.ServiceRepo.Test.Sample
{
    /// <summary>
    /// A service that uses other services.
    /// </summary>
    public class CompoundThreeService : ICompoundThreeService
    {
        private ISimpleOneService _simpleOne;
        private ISimpleTwoService _simpleTwo;

        public void Init(IServiceRepository serviceRepository)
        {
            // get references to the other services and store them
            _simpleOne = serviceRepository.Get<ISimpleOneService>();
            _simpleTwo = serviceRepository.Get<ISimpleTwoService>();
        }

        public string ThreeMethodOne(string param)
        {
            string one = _simpleOne.OneMethodOne(param);
            string two = _simpleTwo.TwoMethodOne(param);
            return $"ThreeMethodOne: {one} {two}";
        }
    }
}
