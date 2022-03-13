namespace Core.Entities.ChildrenItems
{
    public class ChildrenItemCategory
    {
        public int ChildrenItemId { get; set; }
        public ChildrenItem ChildrenItem { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}