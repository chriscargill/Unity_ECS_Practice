using UnityEngine;
using Unity.Collections;
using Unity.Jobs;

public class Para_jobs : MonoBehaviour
{
    // Job adding two floating point values together
    public struct OurJobs : IJobParallelFor
    {
        [ReadOnly]
        public NativeArray<float> a;
        [ReadOnly]
        public NativeArray<float> b;
        public NativeArray<float> result;

        public void Execute(int i)
        {
            result[i] = a[i] + b[i];
        }
    }
    public void Update()
    {
        // Create a native array of a single float to store the result. This example waits for the job to complete for illustration purposes
        NativeArray<float> a = new NativeArray<float>(3, Allocator.TempJob);
        NativeArray<float> b = new NativeArray<float>(3, Allocator.TempJob);
        NativeArray<float> result = new NativeArray<float>(3, Allocator.TempJob);

        // Set up the job data
        a[0] = 1337;
        b[0] = 2021;
        a[1] = 10;
        b[1] = 20;
        a[2] = 2;
        b[2] = 4;

        OurJobs jobData = new OurJobs();
        jobData.a = a;
        jobData.b = b;
        jobData.result = result;

        // Schedule the job
        JobHandle handle = jobData.Schedule(3, 32);

        // Wait for the job to complete
        handle.Complete();

        // All copies of the NativeArray point to the same memory, you can access the result in "your" copy of the NativeArray
        float aPlusB = result[0];
        float two = result[1];
        float three = result[2];

        // Free the memory allocated by the result array
        a.Dispose();
        b.Dispose();
        result.Dispose();

        Debug.Log(aPlusB);
        Debug.Log(two);
        Debug.Log(three);
    }
}
