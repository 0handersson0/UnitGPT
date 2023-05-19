using Microsoft.VisualStudio.Shell.Interop;

namespace UnitGPT.Services.Status.Strategies
{
    internal class ThreadedWaitDialogStrategy : IStatusStrategy
    {
        private IVsThreadedWaitDialog4 _twd;
        private int _totalSteps;
        private int _currentStep = 0;

        public ThreadedWaitDialogStrategy(int totalSteps)
        {
            _totalSteps = totalSteps;
        }

        public async Task SetUpAsync()
        {
#pragma warning disable CVST001 // Cast interop services to their specific type
            var fac = await VS.Services.GetThreadedWaitDialogAsync() as IVsThreadedWaitDialogFactory;
#pragma warning restore CVST001 // Cast interop services to their specific type
            _twd = fac.CreateInstance();
        }

        public async Task TearDown(string message = "")
        {
            _currentStep = 0;
            _twd.UpdateProgress("In progress", message, message, _totalSteps, _totalSteps, true, out _);
            (_twd as IDisposable).Dispose();
        }

        public async Task UpdateStep(string message)
        {
            if (_currentStep == 0)
            {
                _twd.StartWaitDialog("UnitGPT", "Starting process", "", null, "", 1, true, true);
            }

            _currentStep++;

            if (_currentStep == _totalSteps)
            {
                await TearDown(message);
                return;
            }

            _twd.UpdateProgress("In progress", message, message, _currentStep, _totalSteps, true, out _);
        }
    }
}
