## **Proposal for automating max scale updates** ##

### **Steps needed to calculate max scale for queues:** ###

1. Determine the usage for the queue for the past 60 days by querying the Kusto database and getting the workitems that were queued, process and completed during this time frame. 
2. Determine the average waittime and 95th percentile waittime for the queue based on #1 
3. Determine max machines / cores used to process the workitems from #1. 

The above steps are done by using the [Scaler Simulator](#Scaler-Simulator) tool.

### **Scaler Simulator:** ###

[Scaler Simulator](https://dev.azure.com/dnceng/internal/_git/dotnet-helix-machines?path=%2Ftools%2FScalerSimulator&version=GBScalerSimulator&_a=contents) is a tool that can currently do the following: 

- Query Kusto to get workitems for a specific queue for a specified time frame.
- Calculate the average and 95th percentile wait time for the queue based on data from Kusto. 
- Determine the max machines that were needed to process the workitems for that specific queue. 
- Given a max set of machines for a queue, the simulator can simulate the processing of workitems (based on the Kusto query above) and calculate the average and 95th percentile waititme. 
- Given a SLA for wait time, the simulator can also reverse-engineer and determine how many machines would be needed for the queue to process the workitems(based on the Kusto query above) 

### **Automation Proposal Stages:** ###

**Stage 1:**

* Extending Scaler Simulator to do the following based on the assumption that the baseline of core/machine distribution is already arrived at in the machine config yamls: 

    * Inputs: 
        * Total machines / cores that can be used 
        * 95th percentile SLA for wait time for Build Queues 
        * 95th percentile SLA for wait time for Test Queues 

    * Process/simulate each queue in the list of queues that are active (this list currently is hardcoded in a csv but can be extended to pick non-deadlettered queues from info.json directly) to determine what the max machines that are needed to process the load in the queue to be able to hit the SLA given. 

    * Rebalance the max machines allocated based on the past usage by maintaining a running surplus list during the course of a simulator run.A successful run spits out the updated max scale list for the queues processed.

        * i. If the simulation determines that the max machines needed for a specific queue is less than the max scale already set, then update the max scale and add the remaining to the surplus.
        * ii. Maintain a list of queues that need more machines than the current max scale value. If the simulation determines that the max machines needed are more than the max scale already set, then update the list with the queues and how much more is needed.  
        * iii. Once the simulation finishes processing all queues and a final surplus machines count is available, allocate / distribute the surplus among the list of queues identified in #ii. TBD – Actual implementation on the mechanics behind the distribution process. 
        * iv. While doing #iii, if the simulator exhausts all items in the surplus list and there are still queues that require more machines than the max scale that is already set, then fail the simulator letting the user know “This operation will exceed the max total cores allowed, please readjust the total cores or the wait time SLA” 
        * v. During the process, if any queue has no past usage then mark the queue to be deadlettered unless identified as a new queue. TBD -  process to identify a new queue.

**Stage 2:**

* The extended simulator opens a PR with the determined changes to the max scale to dotnet-helix-machines master branch which then will be reviewed and merged by a human. The change will then follow the normal rollout process to make it to Production.

**Stage 3:**

* The extended Simulator can be run manually by providing the inputs on as-need-basis. If we determine that this is needed to be run on a regular cadence, then the simulator can be hooked up to a separate pipeline and schedule a run based on the need with the variables of the pipeline as inputs.

**Stage 4:**

* Instead of using a simulator to determine the max scale, rebalance the max scale on the fly based on the usage by the custom auto-scaler and is able to take in the input of what the total machines allowed are to consider while rebalancing. TBD - Feasibility of implementing this into auto-scaler and ability to test this thoroughly.

<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CProject-Docs%5CCore%20Reduction%5CAutomated_MaxScale_Updates.md)](https://helix.dot.net/f/p/5?p=Documentation%5CProject-Docs%5CCore%20Reduction%5CAutomated_MaxScale_Updates.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CProject-Docs%5CCore%20Reduction%5CAutomated_MaxScale_Updates.md)</sub>
<!-- End Generated Content-->
