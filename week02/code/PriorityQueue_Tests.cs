using Microsoft.VisualStudio.TestTools.UnitTesting;

// TODO Problem 2 - Write and run test cases and fix the code to match requirements.

[TestClass]
public class PriorityQueueTests
{
    [TestMethod]
    // Scenario:  a single item was added "A"(5), then dequeue() is applied and checks the string representation.
    // Expected Result: Dequeue() should return "A", and the queue should be empty afterwards.
    // Defect(s) Found: Dequeue() never removed the item from the list, so it was fixed by 
    // adding:_queue.RemoveAt(highPriorityIndex) before returning the value. 
    public void TestPriorityQueue_DequeueRemovesItem()
    {
        var priorityQueue = new PriorityQueue();
        priorityQueue.Enqueue("A", 5);

        var result = priorityQueue.Dequeue();

        Assert.AreEqual("A", result);
        Assert.AreEqual("[]", priorityQueue.ToString());
    }


    [TestMethod]
    // Scenario: Add "A" (priority 1), "B" (priority 3), and "C" (5) 
    // Expected Result: Dequeue() return "C" since is the highest.
    // Defect(s) Found: ruturned "B" rather "C, because of index < _queue.Count - 1, which skipped the very
    // last item in the queue,so it was fixed by changing it to index < _queue.Count.
    public void TestPriorityQueue_ReturnHighest()
    {
        var priorityQueue = new PriorityQueue();
        priorityQueue.Enqueue("A", 1);
        priorityQueue.Enqueue("B", 3);
        priorityQueue.Enqueue("C", 5);

        var result = priorityQueue.Dequeue();

        Assert.AreEqual("C", result);
    }

    [TestMethod]
    // Scenario: Add "A" (1), "B" (5), "C" (5), "D" (2). B and C have the same priority,
    //but B was added first.
    // Expected Result: Dequeue() returns "B" since it was added first than "C" even if have the same value.
    // Defect(s) Found: returned "C" instead of "B" because it was comparing with >= instead of >,
    // so ties kept overwriting.
    public void TestPriorityQueue_Tie()
    {
        var priorityQueue = new PriorityQueue();
        priorityQueue.Enqueue("A", 1);
        priorityQueue.Enqueue("B", 5);
        priorityQueue.Enqueue("C", 5);
        priorityQueue.Enqueue("D", 2);

        var result = priorityQueue.Dequeue();

        Assert.AreEqual("B", result);
    }

    [TestMethod]
    // Scenario: Dequeue() on an empty PriorityQueue.
    // Expected Result: InvalidOperationException thrown with message "The queue is empty."
    // Defect(s) Found: None
    public void TestPriorityQueue_EmptyThrowsException()
    {
        var priorityQueue = new PriorityQueue();

        try
        {
            priorityQueue.Dequeue();
            Assert.Fail("Exception should have been thrown.");
        }
        catch (InvalidOperationException e)
        {
            Assert.AreEqual("The queue is empty.", e.Message);
        }
    }
}