using System;

namespace Arvarga.Utils.ServiceRepo.Test.Sample
{
    public interface IThrowsInInitService : IService
    {
    }

    public class FromInitException : Exception
    {
    }
}
