using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Arvarga.Utils.ServiceRepo.Test.Sample
{    
    public class Program
    {
        public static void Main(string[] args)
        {
            ServiceRepository serviceRepository = new ServiceRepository();
            serviceRepository.AddServices(
                new List<IService>{
                    new SimpleOneService(),
                    new SimpleTwoService(),
                    new CompoundThreeService(),
                }
            );
            Console.WriteLine($"Services added, {serviceRepository.ToString()}");

            serviceRepository.AddServiceInstanceFromDll("Arvarga.Utils.ServiceRepo.Test.Sample", "Arvarga.Utils.ServiceRepo.Test.Sample.CompoundFourService");
            Console.WriteLine($"Runtime-created service added, {serviceRepository.ToString()}");
            
            serviceRepository.InitServices();
            Console.WriteLine($"Services initialized, {serviceRepository.ToString()}");
        }
    }
}