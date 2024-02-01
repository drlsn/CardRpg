using System;
using System.Threading.Tasks;

namespace Core.Threads
{
    //public class CallbackTask<TResult>
    //{
    //    private TaskCompletionSource<TResult> _tcs;
    //    private bool _inProgress;

    //    public Task<TResult> Run(Func<TResult, Action> action)
    //    {
    //        if (_inProgress)
    //            return _tcs.Task;

    //        var result = action(onDone: () =>
    //        {
    //            _inProgress = false;
    //            _tcs.SetResult(result);
    //        });

    //    }
    //}
}
