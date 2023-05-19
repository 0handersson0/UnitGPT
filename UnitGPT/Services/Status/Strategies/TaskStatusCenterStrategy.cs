using Microsoft.VisualStudio.TaskStatusCenter;

namespace UnitGPT.Services.Status.Strategies
{
    internal class TaskStatusCenterStrategy : IStatusStrategy
    {
        private ITaskHandler _taskHandler;
        private TaskProgressData _progressData;
        private int _totalSteps;
        private int _currentStep;

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
            var task = LongRunningTaskAsync();
            _taskHandler.RegisterTask(task);
        }

        public async Task TearDown(string message = "")
        { }

        public async Task UpdateStep(string message)
        {
            _currentStep++;
            _progressData.PercentComplete = _currentStep / _totalSteps * 100;
            _progressData.ProgressText = message;
            _taskHandler.Progress.Report(_progressData);
        }

        private async Task LongRunningTaskAsync()
        {
            _progressData.PercentComplete = _currentStep / _totalSteps * 100;
            _progressData.ProgressText = "";
            _taskHandler.Progress.Report(_progressData);
        }
    }


}

