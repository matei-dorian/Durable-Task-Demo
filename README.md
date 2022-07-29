# Demo description
We have a range of numbers that we want to split into subintervals of a certain length, average the numbers in the subintervals and return the sum of all averages.
In the initial orchestration, a suborchestration is created for each subinterval. Each orchestration receives a range, and if the length is ok it will start a task to compute the average. A single suborchestration may be created each 3 seconds (because we used a durable timer). 

# Workflow of the framework 
<img src="framework schema.png">

# Link to the documentation
https://github.com/Azure/durabletask/wiki
