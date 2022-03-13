namespace Core.Entities.ChildrenItems
{
    public class ChildrenItemTag
    {
        public int ChildrenItemId { get; set; }
        public ChildrenItem ChildrenItem { get; set; }

        public int TagId { get; set; }
        public Tag Tag { get; set; }
    }
}