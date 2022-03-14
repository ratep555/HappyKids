namespace Core.Entities.ClientBaskets
{
    public class BasketChildrenItemOrdered
    {
        public BasketChildrenItemOrdered()
        {
            
        }
        public BasketChildrenItemOrdered(int basketChildrenItemOdreredId, string basketChildrenItemOrderedName)
        {
            BasketChildrenItemOrderedId = basketChildrenItemOdreredId;
            BasketChildrenItemOrderedName = basketChildrenItemOrderedName;
        }
        public int BasketChildrenItemOrderedId { get; set; }
        public string BasketChildrenItemOrderedName { get; set; }
    }
}