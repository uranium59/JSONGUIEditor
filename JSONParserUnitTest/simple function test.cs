using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace JSONParserUnitTest
{
    [TestFixture]
    class simple_function_test
    {
        [SetUp]
        public void Setup()
        {

        }
        
        [Test]
        public void CheckMinThreadPool()
        {

            int minWorker, minIOC;
            // Get the current settings.
            ThreadPool.GetMinThreads(out minWorker, out minIOC);
            Console.WriteLine(minWorker + " " + minIOC);
            // Change the minimum number of worker threads to four, but
            // keep the old setting for minimum asynchronous I/O 
            // completion threads.
            if (ThreadPool.SetMinThreads(4, minIOC))
            {
                // The minimum number of threads was set successfully.
            }
            else
            {
                // The minimum number of threads was not changed.
            }

            ThreadPool.GetMaxThreads(out minWorker, out minIOC);
            Console.WriteLine(minWorker + " " + minIOC);
        }
    }
}
