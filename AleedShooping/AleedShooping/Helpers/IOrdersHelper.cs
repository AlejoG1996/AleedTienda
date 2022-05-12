using AleedShooping.Common;
using AleedShooping.Models;

namespace AleedShooping.Helpers
{
    public interface IOrdersHelper
    {
        Task<Response> ProcessOrderAsync(ShowCartViewModel model);

        Task<Response> CancelOrderAsync(int id);
    }
}
