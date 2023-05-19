namespace UnitGPT.Services.Status
{
    internal interface IStatusStrategy
    {
        Task SetUpAsync();
        Task TearDown(string message = "");
        Task UpdateStep(string message);
    }
}
