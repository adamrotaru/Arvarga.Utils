using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Arvarga.Utils.TestUtils
{
    /// <summary>
    /// Helper class for tests with asynchron result, with periodic check.
    /// </summary>
    public static class AsyncTestResultChecker
    {
        /// <summary>
        /// Delegate for checking test result once, invoked several times as needed.
        /// </summary>
        /// <returns>True if result is OK, in this case no more checking will be done.</returns>
        public delegate bool CheckResult();

        /// <summary>
        /// Check periodically for asnc test result, with a maximum timeout, with increasing wait between checks.
        /// </summary>
        /// <example>
        ///     bool res = AsyncTestResultChecker.Check(30 * 1000, 50, 125, () => {
        ///         bool found = CheckResult();
        ///         if (found == null) return false;
        ///         return true;
        ///     }, "Newly created artifact found");
        /// 
        ///     This will check first after 50ms, if not yet there, check again in another 62.5ms, and so on, until at most 30 seconds have elapsed.
        /// </example>
        /// <param name="maxTimeMs">Maximum timeout (ms), e.g. 3000</param>
        /// <param name="firstWaitMs">Wait time until the first check, e.g. 50</param>
        /// <param name="waitIncreasePct">Percentage of increase between each time, e.g. 120.  Valid range: 100-200</param>
        /// <param name="checkDelegate">The delegate to check, return true if test is fulfilled</param>
        /// <param name="checkDescription">String description of the check, for logging (e.g. "Newly created log is found in the saved logs")</param>
        /// <returns></returns>
        public static bool Check(int maxTimeMs, int firstWaitMs, int waitIncreasePct, CheckResult checkDelegate, string checkDescription)
        {
            DateTime startTime = DateTime.Now;
            DateTime stopTime = startTime.AddMilliseconds(maxTimeMs);
            int waitTimeMs = firstWaitMs;
            waitIncreasePct = Math.Max(Math.Min(waitIncreasePct, 200), 100);

            Console.WriteLine($"Checking '{checkDescription}', with max timeout of {maxTimeMs} ms, wait strategy of start={firstWaitMs} ms and increase={waitIncreasePct} %");

            // check in a loop
            while (true)
            {
                //Console.WriteLine("(waiting for {0} ms before check)", waitTimeMs);
                Thread.Sleep(waitTimeMs);

                //Console.WriteLine("(performing check...)");
                bool res = checkDelegate();
                int curElapsedMs = (int)DateTime.Now.Subtract(startTime).TotalMilliseconds;
                if (res)
                {
                    Console.WriteLine($"check fulfilled, stop, elapsed {curElapsedMs} ms");
                    // OK
                    return true;
                }

                if (DateTime.Now > stopTime)
                {
                    Console.WriteLine($"failed due to time-out, elapsed {curElapsedMs} ms, max {maxTimeMs}");
                    return false;
                }

                int prevWaitTimeMs = waitTimeMs;
                waitTimeMs = (int)((double)waitTimeMs * (double)waitIncreasePct * 0.01);
                // trim
                if (curElapsedMs + waitTimeMs > maxTimeMs)
                {
                    waitTimeMs = Math.Max(maxTimeMs - curElapsedMs + 1, 1);
                }
                Console.WriteLine($"check still failed, time elapsed {curElapsedMs} ms, wait time prev {prevWaitTimeMs}, next {waitTimeMs} ms");
            }
        }
    }
}
