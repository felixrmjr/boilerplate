using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

namespace Business.Background.Benchmark
{
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class HeapSortBenchmark
    {
        static int[] array = { 717, 502, 902, 673, 390, 550, 376, 698, 954, 991, 856, 699, 344, 942, 446, 83, 728, 422, 911, 943, 994, 564, 436, 706, 747, 165, 306, 76, 572, 952, 54, 792, 18, 240, 244, 525, 450, 717, 335, 362, 965, 457, 872, 343, 851, 730, 104, 373, 276, 661, 650, 692, 937, 320, 191, 317, 236, 140, 417, 899, 973, 243, 60, 929, 271, 955, 568, 175, 392, 73, 528, 654, 417, 73, 743, 422, 790, 393, 821, 375, 948, 360, 176, 91, 729, 34, 484, 853, 95, 715, 452, 991, 948, 124, 351, 273, 213, 70, 544, 169 };
        [Benchmark]
        public void heap()
        {
            sort(array, array.Length);
        }

        void sort(int[] arr, int n)
        {
            if (n <= 1)
                return;

            for (int i = n / 2 - 1; i >= 0; i--)
                heapify(arr, n, i);

            for (int i = n - 1; i > 0; i--)
            {
                int _ = arr[0];
                arr[0] = arr[i];
                arr[i] = _;

                heapify(arr, i, 0);
            }
        }

        void heapify(int[] arr, int n, int i)
        {
            int largest = i;
            int l = 2 * i + 1;
            int r = 2 * i + 2;

            if (l < n && arr[l] > arr[largest])
                largest = l;

            if (r < n && arr[r] > arr[largest])
                largest = r;

            if (largest != i)
            {
                int _ = arr[i];
                arr[i] = arr[largest];
                arr[largest] = _;

                heapify(arr, n, largest);
            }
        }

        [Benchmark]
        public void heap2()
        {
            sort2(array, array.Length);
        }

        int[] sort2(int[] arr, int n)
        {
            if (n <= 1)
                return arr;

            for (int i = n / 2 - 1; i >= 0; i--)
                heapify2(arr, n, i);

            for (int i = n - 1; i >= 0; i--)
            {
                int _ = arr[0];
                arr[0] = arr[i];
                arr[i] = _;

                heapify2(arr, i, 0);
            }

            return arr;
        }

        void heapify2(int[] arr, int n, int i)
        {
            int largest = i;
            int l = 2 * i + 1;
            int r = 2 * i + 2;

            if (l < n && arr[l] > arr[largest])
                largest = l;

            if (l < n && arr[l] > arr[largest])
                largest = r;

            if (largest != i)
            {
                int _ = arr[i];
                arr[i] = arr[largest];
                arr[largest] = _;

                heapify2(arr, n, largest);
            }
        }
    }
}
