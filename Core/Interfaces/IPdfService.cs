namespace Core.Interfaces
{
    public interface IPdfService
    {
        void GeneratePdfForGeneralCardSlip(int orderNo, decimal amount, string firstName, string lastName);
    }
}