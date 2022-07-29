using DurableTask.Core;

namespace DtTest
{
    public class AverageOnIntervalTask : TaskActivity<Interval, double>
    {
        protected override double Execute(TaskContext context, Interval input)
        {
            // compute the average of the numbers in the interval

            int sum = 0;
            int ct = 1;

            for (int i = input.Start; i <= input.End; i++)
            {
                ct++;
                sum += i;
                Console.WriteLine($"Task saw number {i}");
            }
            Console.WriteLine("\n");
            return sum / ct;
        }
    }
}