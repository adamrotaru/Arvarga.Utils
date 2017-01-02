/*
 * See the LICENSE file for copyright and license conditions.
 * (c) 2016 Adam Rotaru
 */

using System;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Xunit;
using Arvarga.Utils.ServiceRepo.Test.Sample;

namespace Arvarga.Utils.ServiceRepo.Test.Unit
{
    //[TestClass]
    public class TestServiceCreation
    {
        //[TestMethod]
        [Fact]
        public void TestServiceCreation_Simple()
        {
            // create the service repo with the services
            IServiceRepositoryFull repo = new ServiceRepository();
            repo.InitWithServices(new List<IService>
            {
                new SimpleOneService(),
                new SimpleTwoService(),
                new CompoundThreeService(),
            });
            Console.WriteLine($"Services initialized: {repo.ToString()}");

            // make sure services can be retrieved
            Assert.True(repo.Get<ISimpleOneService>() != null, "Service could not be retrieved, SimpleOneService");
            Assert.True(repo.Get<ISimpleTwoService>() != null, "Service could not be retrieved, SimpleTwoService");
            Assert.True(repo.Get<ICompoundThreeService>() != null, "Service could not be retrieved, CompoundThreeService");

            // make sure services work
            string threeResult = repo.Get<ICompoundThreeService>().ThreeMethodOne("Test");
            Assert.Equal<string>("ThreeMethodOne: OneMethodOne: Test TwoMethodOne: Test", threeResult);  //, "Wrong result from CompoundThreeService");
        }

        //[TestMethod]
        [Fact]
        public void TestServiceCreation_AddOneByOne()
        {
            // create the service repo with the services
            ServiceRepository repo = new ServiceRepository();
            repo.Add(new SimpleOneService());
            repo.Add(new SimpleTwoService());
            repo.Add(new CompoundThreeService());
            repo.InitServices();

            // make sure services can be retrieved
            Assert.True(repo.Get<ISimpleOneService>() != null, "Service could not be retrieved, SimpleOneService");
            Assert.True(repo.Get<ISimpleTwoService>() != null, "Service could not be retrieved, SimpleTwoService");
            Assert.True(repo.Get<ICompoundThreeService>() != null, "Service could not be retrieved, CompoundThreeService");

            // make sure services work
            string threeResult = repo.Get<ICompoundThreeService>().ThreeMethodOne("Test");
            Assert.Equal<string>("ThreeMethodOne: OneMethodOne: Test TwoMethodOne: Test", threeResult);  // "Wrong result from CompoundThreeService");
        }

        //[TestMethod]
        [Fact]
        public void TestServiceCreation_Aggregate()
        {
            // create the service repo with the services
            ServiceRepository repo = new ServiceRepository();
            repo.InitWithServices(new List<IService>
            {
                new ServicesOneToThreeAggregate(),
            });

            // make sure services can be retrieved
            Assert.True(repo.Get<ISimpleOneService>() != null, "Service could not be retrieved, SimpleOneService");
            Assert.True(repo.Get<ISimpleTwoService>() != null, "Service could not be retrieved, SimpleTwoService");
            Assert.True(repo.Get<ICompoundThreeService>() != null, "Service could not be retrieved, CompoundThreeService");

            // make sure services work
            string threeResult = repo.Get<ICompoundThreeService>().ThreeMethodOne("Test");
            Assert.Equal<string>("ThreeMethodOne: OneMethodOne: Test TwoMethodOne: Test", threeResult);  // "Wrong result from CompoundThreeService");
        }

        //[TestMethod]
        [Fact]
        public void TestServiceCreation_WithDependencies()
        {
            // create the service repo with the services
            ServiceRepository repo = new ServiceRepository();
            repo.InitWithServices(new List<IService>
            {
                new SimpleOneService(),
                new SimpleTwoService(),
                new CompoundFourService(),
            });

            // make sure services can be retrieved
            Assert.True(repo.Get<ISimpleOneService>() != null, "Service could not be retrieved, SimpleOneService");
            Assert.True(repo.Get<ISimpleTwoService>() != null, "Service could not be retrieved, SimpleTwoService");
            Assert.True(repo.Get<ICompoundFourService>() != null, "Service could not be retrieved, CompoundFourService");

            // make sure services work
            string threeResult = repo.Get<ICompoundFourService>().FourMethodOne("Test");
            Assert.Equal<string>("FourMethodOne: OneMethodOne: ONE TwoMethodOne: Test", threeResult);  // "Wrong result from CompoundThreeService");
        }

        //[TestMethod]
        //[ExpectedException(typeof(System.ApplicationException))]
        [Xunit.Fact]
        public void TestServiceCreation_MissingService()
        {
            // create the service repo with the services
            ServiceRepository repo = new ServiceRepository();
            repo.InitWithServices(new List<IService>
            {
                new SimpleOneService(),
            });

            // make sure services can be retrieved
            Assert.True(repo.Get<ISimpleOneService>() != null, "Service could not be retrieved, SimpleOneService");
            Assert.Throws<System.Exception>(() => { repo.Get<ISimpleTwoService>(); });  // "Service could not be retrieved, SimpleTwoService"
        }

        //[TestMethod]
        [Fact]
        //[ExpectedException(typeof(System.ApplicationException))]
        public void TestServiceCreation_WithDependenciesReverseOrder()
        {
            // create the service repo with the services
            ServiceRepository repo = new ServiceRepository();
            Assert.Throws<System.Exception>(() => 
            {
                repo.InitWithServices(new List<IService>
                {
                    new CompoundFourService(),
                    new SimpleOneService(),
                    new SimpleTwoService(),
                });
            });
        }

        //[TestMethod]
        [Fact]
        //[ExpectedException(typeof(System.ApplicationException))]
        public void TestServiceCreation_ExceptionFromInit()
        {
            // create the service repo with the services
            ServiceRepository repo = new ServiceRepository();
            Assert.Throws<System.Exception>(() => 
            {
                try
                {
                    repo.InitWithServices(new List<IService>
                    {
                        new ThrowsInInitService(),
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"EXCEPTION: {ex.Message}");
                    Console.WriteLine($"{ex}");
                    throw;
                }
            });
        }
    }
}
