using System.Threading.Tasks;
using UnitGPT.Services.OpenAI.Models;

namespace UnitGPT.Services.OpenAI.Interface
{
    internal interface IRequestService
    {
        public Task<ResponseModel> MakeRequest(string promptData);
        public ResponseModel ParseResponse(Response response);

    }
}
