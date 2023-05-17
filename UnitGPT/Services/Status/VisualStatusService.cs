using Microsoft.VisualStudio.Shell.Interop;

namespace UnitGPT.Services.Status
{
    internal class VisualStatusService
    {
        private IVsThreadedWaitDialog4 _twd;
        private int _totalSteps;
        private int _currentStep = 0;

        internal async Task SetUp(int totalSteps)
        {
            var fac = await VS.Services.GetThreadedWaitDialogAsync() as IVsThreadedWaitDialogFactory;
            _twd = fac.CreateInstance();
            _totalSteps = totalSteps;
        }

        internal async Task TearDown(string message = "")
        {
            _currentStep = 0;
            _twd.UpdateProgress("In progress", message, message, _totalSteps, _totalSteps, true, out _);
            await VS.StatusBar.ShowProgressAsync(message, _totalSteps, _totalSteps);
            (_twd as IDisposable).Dispose();
            
        }

        internal async Task UpdateStep(string message)
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
           
            await VS.StatusBar.ShowProgressAsync(message, _currentStep, _totalSteps);
            _twd.UpdateProgress("In progress", message, message, _currentStep, _totalSteps, true, out _);
            
            
        }
    }
}
