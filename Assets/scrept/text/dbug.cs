using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class TaskWithCancelTest : MonoBehaviour
{
    private CancellationTokenSource cancellationTokenSource;

    void Start()
    {
        StartLogging();
    }

    public void StartLogging()
    {
        StopLogging(); // 既に実行中なら停止（多重起動を防ぐ）
        cancellationTokenSource = new CancellationTokenSource();
        Task.Run(() => LogEverySecond(cancellationTokenSource.Token));
    }

    async Task LogEverySecond(CancellationToken token)
    {
        try
        {
            while (!token.IsCancellationRequested)
            {
                Debug.Log("Task 実行中：" + Time.time);
                await Task.Delay(1000, token); // キャンセル可能な遅延
            }
        }
        catch (TaskCanceledException)
        {
            Debug.Log("Task がキャンセルされました");
        }
        finally
        {
            Debug.Log("Task ループが完全に終了しました");
        }
    }

    public void StopLogging()
    {
        if (cancellationTokenSource != null)
        {
            cancellationTokenSource.Cancel(); // タスクをキャンセル
            cancellationTokenSource.Dispose();
            cancellationTokenSource = null;
            Debug.Log("Task 停止！");
        }
    }
}
