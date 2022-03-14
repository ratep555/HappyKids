namespace Core.Interfaces
{
    public interface IOrderStatusRepository
    {
        int GetPendingPaymentOrderStatusId();
        int GetFailedPaymentOrderStatusId();
        int GetReceivedPaymentOrderStatusId();
    }
}