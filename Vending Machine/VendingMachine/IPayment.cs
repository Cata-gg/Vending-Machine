namespace iQuest.VendingMachine
{
    internal interface IPayment
    {
        void Pay(int id, float price);
    }
}