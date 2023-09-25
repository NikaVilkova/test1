using System.Collections.Concurrent;
using System.Diagnostics;

var syncList = new List<int>();

var asyncList = new ConcurrentBag<int>();

var rnd = new Random();

FillAsyncList(asyncList, rnd);

FillSyncList(syncList, () => { for (int i = 0; i < 1000; syncList.Add(rnd.Next()), Thread.Sleep(10), i++) ; });

static void FillSyncList(List<int> syncList, Action action)
{
    var sw = new Stopwatch();
    sw.Start();
    action();
    sw.Stop();
    TimeSpan ts = sw.Elapsed;
    string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}:{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
    Console.WriteLine($"Я заполнил синхронно! Время: {elapsedTime}");
    Console.WriteLine(syncList.Count);
}


static void FillAsyncList(ConcurrentBag<int> asyncList, Random rnd)
{
    var tasks = new List<Task>();
    var sw = new Stopwatch();
    sw.Start();
    for (int i = 0; i < 10; i++)
        tasks.Add(Task.Factory.StartNew(() => { for (int i = 0; i < 100; asyncList.Add(rnd.Next()), Thread.Sleep(10), i++) ; }));
    Task.WaitAll(tasks.ToArray());
    sw.Stop();
    TimeSpan ts = sw.Elapsed;
    string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}:{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
    Console.WriteLine($"Я заполнил асинхронно! Время: {elapsedTime}");
    Console.WriteLine(syncList.Count);
}



