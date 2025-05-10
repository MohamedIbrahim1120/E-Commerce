using Shared.OrderModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions
{
    public interface IOrderService 
    {
        // Get Order ById Async
        Task<OrderResultDto> GetOrderByIdAsync(Guid id);

        // Get Order
        Task<IEnumerable<OrderResultDto>> GetOrderByUserEmailAsync(string userEmail);

        Task<OrderResultDto>CreateOrderAsync(OrderRequestDto orderRequest, string userEmail);

        // Get All Delivery Method

       Task<IEnumerable<DeliveryMethodDto>> GetAllDeliveryMethods();


    }
}
