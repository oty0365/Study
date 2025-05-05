using System;
using System.Collections.Generic;

public class PriorityQueue<T>
{
    private List<(T item, int priority)> heap = new();

    public int Count => heap.Count;

    public void Enqueue(T item, int priority)
    {
        heap.Add((item, priority));
        HeapifyUp(heap.Count - 1);
    }

    public T Dequeue()
    {
        if (heap.Count == 0) throw new InvalidOperationException("Queue is empty");

        var root = heap[0].item;
        heap[0] = heap[^1];
        heap.RemoveAt(heap.Count - 1);
        HeapifyDown(0);
        return root;
    }

    public void Clear()
    {
        foreach(var item in heap) heap.Remove(item);
    }

    private void HeapifyUp(int i)
    {
        while (i > 0)
        {
            int parent = (i - 1) / 2;
            if (heap[i].priority >= heap[parent].priority) break;
            (heap[i], heap[parent]) = (heap[parent], heap[i]);
            i = parent;
        }
    }

    private void HeapifyDown(int i)
    {
        while (true)
        {
            int left = 2 * i + 1, right = 2 * i + 2, smallest = i;
            if (left < heap.Count && heap[left].priority < heap[smallest].priority) smallest = left;
            if (right < heap.Count && heap[right].priority < heap[smallest].priority) smallest = right;
            if (smallest == i) break;
            (heap[i], heap[smallest]) = (heap[smallest], heap[i]);
            i = smallest;
        }
    }
}
