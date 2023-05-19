namespace UnitGPT.Services.Status.Strategies
{
    internal class StatusBarStrategy : IStatusStrategy
    {
        private int _totalSteps;
        private int _currentStep = 0;

        public StatusBarStrategy(int totalSteps)
        {
            _totalSteps = totalSteps;
        }

        public async Task SetUpAsync()
        {
        }

        public async Task TearDown(string message = "")
        {
            _currentStep = 0;
            await VS.StatusBar.ShowProgressAsync(message, _totalSteps, _totalSteps);
        }

        public async Task UpdateStep(string message)
        {
            _currentStep++;

            if (_currentStep == _totalSteps)
            {
                await TearDown(message);
                return;
            }

            await VS.StatusBar.ShowProgressAsync(message, _currentStep, _totalSteps);
        }
    }
}
