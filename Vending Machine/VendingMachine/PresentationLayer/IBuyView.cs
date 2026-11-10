namespace iQuest.VendingMachine.PresentationLayer
{
    internal interface IBuyView
    {
        int RequestProduct();

        void DispenseProduct(string productName);
    }
}