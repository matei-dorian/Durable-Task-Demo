using DurableTask.Core;
using DurableTask.SqlServer;

namespace DtTest
{
    public class Program
    {
        static public void Main(String[] args)
        {
            var input = new Interval(1, 100, 3);
            var ConnectionString = @"sql server connection string";

            // connect to the database
            var settings = new SqlOrchestrationServiceSettings(ConnectionString);
            var provider = new SqlOrchestrationService(settings);
            provider.CreateIfNotExistsAsync().Wait();

            // create the schema of the orchestration
            var taskHubWorker = new TaskHubWorker(provider);
            taskHubWorker.AddTaskOrchestrations(typeof(IntervalOrchestration))
                .AddTaskActivities(new AverageOnIntervalTask());
            taskHubWorker.StartAsync().Wait();

            // create an instance of the orchestration
            string instanceId = Guid.NewGuid().ToString();
            var taskHubClient = new TaskHubClient(provider);
            var instanceTask = taskHubClient.CreateOrchestrationInstanceAsync(
                typeof(IntervalOrchestration),
                instanceId,
                input).Result;

            Console.WriteLine("Waiting up to 30 seconds for completion.");
            try
            {
                // Run the orchestration for 30 seconds or until it ends
                OrchestrationState taskResult = taskHubClient.WaitForOrchestrationAsync(instanceTask, TimeSpan.FromSeconds(30), CancellationToken.None).Result;
                Console.WriteLine($"Task done: {taskResult?.OrchestrationStatus}");
                Console.WriteLine(taskResult.Output);
            }
            catch (Exception)
            {
                Console.WriteLine("Timeout");
            }

            taskHubWorker.StopAsync(true).Wait();
        }
    }
}