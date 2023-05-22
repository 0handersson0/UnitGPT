using Microsoft.VisualStudio.TaskStatusCenter;

namespace UnitGPT.Services.Status.Strategies
{
    internal class TaskStatusCenterStrategy : IStatusStrategy
    {
        private ITaskHandler _taskHandler;
        private TaskProgressData _progressData;
        private readonly int _totalSteps;
        private int _currentStep;
        private bool _runTask = true;
        private string _progressText;
        public TaskStatusCenterStrategy(int totalSteps)
        {
            _totalSteps = totalSteps;
        }

        public async Task SetUpAsync()
        {
            var tsc = await VS.Services.GetTaskStatusCenterAsync();

            var options = default(TaskHandlerOptions);
            options.Title = "Creating unit tests...";
            options.ActionsAfterCompletion = CompletionActions.None;

            _progressData = default;
            _progressData.CanBeCanceled = false;

            _taskHandler = tsc.PreRegister(options, _progressData);
            var task = LongRunningTaskAsync(_progressData, _taskHandler);
            _taskHandler.RegisterTask(task);
        }

        public async Task TearDown(string message = "")
        {
            _currentStep = _totalSteps;
        }

        public async Task UpdateStep(string message)
        {
            _currentStep++;
            _progressText = message;
        }

        private async Task LongRunningTaskAsync(TaskProgressData data, ITaskHandler handler)
        {
            for (; _currentStep <= _totalSteps;)
            {
                await Task.Delay(100);
                if (_currentStep == _totalSteps) break;
                data.PercentComplete = (int)(_currentStep / _totalSteps * 100);
                data.ProgressText = $"{_progressText}. Step {_currentStep} of {_totalSteps} completed";
                handler.Progress.Report(data);
            }
        }
    }


}

