using DurableTask.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtTest
{
    public class IntervalOrchestration : TaskOrchestration<double, Interval>
    {
        public override async Task<double> RunTask(OrchestrationContext context, Interval input)
        {
            // if the interval's size is bigger than the step
            // start a suborchestration of the same type to process each subinterval of length step
            // for each subinterval compute the average of the numbers in it

            int dist = input.End - input.Start + 1;
            double sum = 0;
            var chunks = new List<Task<double>>();
            if (dist > input.Step)
            {   
                for (int i = input.Start + input.Step; i <= input.End + input.Step; i += input.Step) 
                {
                    var subInterval = new Interval(i - input.Step, Math.Min(input.End, i - 1), input.Step);
                    chunks.Add(context.CreateSubOrchestrationInstance<double>(typeof(IntervalOrchestration), subInterval));
                    await context.CreateTimer(context.CurrentUtcDateTime.Add(TimeSpan.FromSeconds(3)), "timer");
                }
            }
            else
            {
                Task<double> activity = context.ScheduleTask<double>(typeof(AverageOnIntervalTask), input);
                chunks.Add(activity);
            }

            // sum all the averages
            double[] allChunks = await Task.WhenAll(chunks.ToArray());
            foreach (double result in allChunks)
            {
                sum += result;
            }
            return sum;
        }    
    }
}
