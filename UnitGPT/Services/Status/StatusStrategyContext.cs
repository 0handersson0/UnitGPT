namespace UnitGPT.Services.Status
{
    internal class StatusStrategyContext
    {
        private IStatusStrategy _Context { get; set; }

        public StatusStrategyContext()
        { }

        internal async Task SetStrategy(IStatusStrategy strategy)
        {
            _Context = strategy;
        }

        internal async Task UpdateStep(string msg)
        {
            await _Context.UpdateStep(msg);
        }
    }
}
