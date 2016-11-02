using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arvarga.Utils.ServiceRepo.Test.Sample
{
    /// <summary>
    /// A service that uses another services right in Init().
    /// </summary>
    public class CompoundFourService : ICompoundFourService
    {
        private ISimpleOneService _simpleOne;
        private ISimpleTwoService _simpleTwo;
        private string _nameOne;

        public void Init(IServiceRepository serviceRepository)
        {
            _simpleOne = serviceRepository.Get<ISimpleOneService>(true);
            _simpleTwo = serviceRepository.Get<ISimpleTwoService>();

            // use other services right here in Init()
            _nameOne = _simpleOne.OneMethodOne("ONE");
        }

        public string FourMethodOne(string param)
        {
            string two = _simpleTwo.TwoMethodOne(param);
            return $"FourMethodOne: {_nameOne} {two}";
        }
    }
}
