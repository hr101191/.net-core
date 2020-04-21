using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProtoActorWebApiDemo.DataAccess
{
    public interface ITestService
    {
        void Test();
    }
    public class TestService : ITestService
    {
        public TestService()
        {
            Console.WriteLine("Created test service instance");
        }

        public void Test()
        {
            Console.WriteLine("This is a test");
        }
    }
}
