using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace QuestionnaireApp.Services
{
    public abstract class BackgroundService : IHostedService
    {
        private Task _executingTask;
        private readonly CancellationTokenSource _stoppingCts = new CancellationTokenSource();

        public virtual Task StartAsync(CancellationToken cancellationToken)
        {
            //to store the task we're executing
            _executingTask = ExecuteAsync(_stoppingCts.Token);
            //if the task is completed then return it
            if (_executingTask.IsCompleted)
            {
                return _executingTask;
            }
            //otherwise it's running
            return Task.CompletedTask;
        }
        public virtual async Task StopAsync(CancellationToken cancellationToken)
        {
            //stop called without start
            if (_executingTask == null)
            {
                return;
            }
            try
            {
                //signal cancellation to the executing method
                _stoppingCts.Cancel();
            }
            finally
            {
                //wait untill the task comletes or the stop token triggers
                await Task.WhenAny(_executingTask, Task.Delay(Timeout.Infinite, cancellationToken));
            }
        }
        protected virtual async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            do
            {
                await Process();
                await Task.Delay(60000, stoppingToken);
            }
            while (!stoppingToken.IsCancellationRequested);
        }
        protected abstract Task Process();
    }
}
