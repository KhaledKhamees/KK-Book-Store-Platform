using MVCProject2.Models;
using MVCProject2.Models.Models;
using MVCProject2.Repository.IRepository;

namespace MVCProject2.Reprository.IReprository
{
    public interface IOrderHeaderRepository : IRepository<OrderHeader>
    {
        void Update(OrderHeader orderHeader); 
        void UpdateStatus(int id , string orderStatus , string? PaymentStatus  = null);
        void UpdateStripePaymentId(int id, string sessionId, string paymentIntentId );

    }
}
