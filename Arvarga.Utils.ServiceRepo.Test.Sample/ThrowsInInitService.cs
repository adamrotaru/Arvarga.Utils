using System;

namespace Arvarga.Utils.ServiceRepo.Test.Sample
{
    public class ThrowsInInitService : IThrowsInInitService
    {
        public void Init(IServiceRepository serviceRepository)
        {
            throw new FromInitException();
        }
    }
}
