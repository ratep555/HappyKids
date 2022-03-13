namespace Core.Entities.ChildrenItems
{
    public class ChildrenItemManufacturer
    {
        public int ChildrenItemId { get; set; }
        public ChildrenItem ChildrenItem { get; set; }

        public int ManufacturerId { get; set; }
        public Manufacturer Manufacturer { get; set; }
    }
}