using System.Collections.Generic;
using AutoMapper;
using Core.Dtos;
using Core.Dtos.BasketsDtos;
using Core.Dtos.ChildrenItemsDtos;
using Core.Dtos.DiscountsDto;
using Core.Dtos.Identity;
using Core.Dtos.OrdersDtos;
using Core.Entities;
using Core.Entities.Discounts;
using Core.Entities.ChildrenItems;
using Core.Entities.ClientBaskets;
using Core.Entities.Identity;
using Core.Entities.Orders;
using NetTopologySuite.Geometries;
using Core.Dtos.WarehousesDtos;
using Core.Dtos.DiscountsDtos;
using Core.Entities.BirthdayOrders;
using Core.Dtos.BirthdayOrdersDtos;
using Core.Entities.Blogs;
using Core.Dtos.BlogsDtos;

namespace API.Helpers
{
    /// <summary>
    /// Mapping properties with the help of Automapper and manually
    /// <param name="geometryFactory">NetTopologySuite is required since we are storing coordinates of our branches.</param>
    /// </summary>
    public class MappingHelper : Profile
    {
        public MappingHelper(GeometryFactory geometryFactory)
        {
            CreateMap<RegisterDto, ApplicationUser>()
                .ForMember(d => d.UserName, o => o.MapFrom(s => s.DisplayName));

            CreateMap<Address, ShippingAddressDto>().ReverseMap();

            CreateMap<ShippingAddressDto, ShippingAddress>().ReverseMap();

            CreateMap<ApplicationRole, RoleDto>().ReverseMap();
            
            CreateMap<BasketChildrenItemDto, BasketChildrenItem>();

            CreateMap<BirthdayPackage, BirthdayPackageDto>()
                .ForMember(d => d.KidActivities, o => o.MapFrom(MapForKidActivities))
                .ForMember(d => d.Discounts, o => o.MapFrom(MapForDiscounts));
            
            CreateMap<BirthdayPackageCreateEditDto, BirthdayPackage>()
                .ForMember(x => x.Picture, options => options.Ignore())
                .ForMember(x => x.BirthdayPackageDiscounts, options => options.MapFrom(MapBirthdayPackageDisounts))
                .ForMember(x => x.BirthdayPackageKidActivities, options => options.MapFrom(MapBirthdayPackageKidActivities));
           
            CreateMap<Branch, BranchDto>()
                .ForMember(d => d.Country, o => o.MapFrom(s => s.Country.Name))
                .ForMember(d => d.Latitude, o => o.MapFrom(s => s.Location.Y))
                .ForMember(d => d.Longitude, o => o.MapFrom(s => s.Location.X)); 
            
            CreateMap<BranchCreateEditDto, Branch>()
               .ForMember(x => x.Location, x => x.MapFrom(dto =>
                geometryFactory.CreatePoint(new Coordinate(dto.Longitude, dto.Latitude))));

            CreateMap<ClientBasketDto, ClientBasket>();
            
            CreateMap<Category, CategoryDto>().ReverseMap();

            CreateMap<CategoryCreateEditDto, Category>();

            CreateMap<ChildrenItem, ChildrenItemDto>()
                .ForMember(d => d.Categories, o => o.MapFrom(MapForCategories))
                .ForMember(d => d.Discounts, o => o.MapFrom(MapForDiscounts))
                .ForMember(d => d.Manufacturers, o => o.MapFrom(MapForManufacturers))
                .ForMember(d => d.Tags, o => o.MapFrom(MapForTags));
            
            CreateMap<ChildrenItem, ChildrenItemPureDto>();
            
            CreateMap<ChildrenItemCreateEditDto, ChildrenItem>()
                .ForMember(x => x.Picture, options => options.Ignore())
                .ForMember(x => x.ChildrenItemCategories, options => options.MapFrom(MapChildrenItemCategories))
                .ForMember(x => x.ChildrenItemDiscounts, options => options.MapFrom(MapItemDiscounts))
                .ForMember(x => x.ChildrenItemManufacturers, options => options.MapFrom(MapChildrenItemManufacturers))
                .ForMember(x => x.ChildrenItemTags, options => options.MapFrom(MapChildrenItemTags));
            
            CreateMap<ChildrenItemWarehouse, ChildrenItemWarehouseDto>()
                .ForMember(d => d.ChildrenItem, o => o.MapFrom(s => s.ChildrenItem.Name))
                .ForMember(d => d.Warehouse, o => o.MapFrom(s => s.Warehouse.Name))
                .ForMember(d => d.City, o => o.MapFrom(s => s.Warehouse.City));
            
            CreateMap<ChildrenItemWarehouseCreateEditDto, ChildrenItemWarehouse>();

            CreateMap<ClientOrder, ClientOrderToReturnDto>()
                .ForMember(d => d.ShippingOption, o => o.MapFrom(s => s.ShippingOption.Name))
                .ForMember(d => d.PaymentOption, o => o.MapFrom(s => s.PaymentOption.Name))
                .ForMember(d => d.ShippingPrice, o => o.MapFrom(s => s.ShippingOption.Price))
                .ForMember(d => d.OrderStatus, o => o.MapFrom(s => s.OrderStatus.Name));

            CreateMap<OrderChildrenItem, OrderChildrenItemDto>()
                .ForMember(d => d.ChildrenItemId, o => o.MapFrom
                    (s =>  s.BasketChildrenItemOrdered.BasketChildrenItemOrderedId))
                .ForMember(d => d.ChildrenItemName, o => o.MapFrom
                    (s =>  s.BasketChildrenItemOrdered.BasketChildrenItemOrderedName));
            
            CreateMap<ClientBirthdayOrder, ClientBirthdayOrderDto>()
                .ForMember(d => d.Branch, o => o.MapFrom(s => s.Branch.City))
                .ForMember(d => d.BirthdayPackage, o => o.MapFrom(s => s.BirthdayPackage.PackageName))
                .ForMember(d => d.OrderStatus, o => o.MapFrom(s => s.OrderStatus.Name));

            CreateMap<Blog, BlogDto>()
                .ForMember(d => d.Username, o => o.MapFrom(s => s.ApplicationUser.UserName));

            CreateMap<BlogCreateEditDto, Blog>()
                .ForMember(x => x.Picture, options => options.Ignore());
            
            CreateMap<BlogComment, BlogCommentDto>()
                .ForMember(d => d.Username, o => o.MapFrom(s => s.ApplicationUser.UserName));

            CreateMap<BlogCommentCreateEditDto, BlogComment>();
            
            CreateMap<ClientBirthdayOrderCreateDto, ClientBirthdayOrder>();
            
            CreateMap<ClientBirthdayOrderEditDto, ClientBirthdayOrder>();

            CreateMap<Country, CountryDto>().ReverseMap();

            CreateMap<Discount, DiscountDto>()
                .ForMember(d => d.ChildrenItems, o => o.MapFrom(MapForChildrenItems))
                .ForMember(d => d.Categories, o => o.MapFrom(MapForCategories1))
                .ForMember(d => d.Manufacturers, o => o.MapFrom(MapForManufacturers));
            
            CreateMap<DiscountCreateEditDto, Discount>()
                .ForMember(x => x.ChildrenItemDiscounts, options => options.MapFrom(MapDiscountChildrenItems))
                .ForMember(x => x.CategoryDiscounts, options => options.MapFrom(MapDiscountCategories))
                .ForMember(x => x.ManufacturerDiscounts, options => options.MapFrom(MapDiscountManufacturers));
            
            CreateMap<KidActivity, KidActivityDto>().ReverseMap();

            CreateMap<KidActivityCreateEditDto, KidActivity>()
                .ForMember(x => x.Picture, options => options.Ignore());

            CreateMap<Manufacturer, ManufacturerDto>().ReverseMap();

            CreateMap<ManufacturerCreateEditDto, Manufacturer>();

            CreateMap<Message, MessageDto>().ReverseMap();

            CreateMap<MessageCreateEditDto, Message>();

            CreateMap<OrderStatus, OrderStatusDto>().ReverseMap();

            CreateMap<OrderStatusCreateEditDto, OrderStatus>();
        
            CreateMap<PaymentOption, PaymentOptionDto>().ReverseMap();

            CreateMap<PaymentOptionCreateEditDto, PaymentOption>();

            CreateMap<ShippingOption, ShippingOptionDto>().ReverseMap();

            CreateMap<ShippingOptionCreateEitDto, ShippingOption>();

            CreateMap<Tag, TagDto>().ReverseMap();

            CreateMap<TagCreateEditDto, Tag>();

            CreateMap<Warehouse, WarehouseDto>()
                .ForMember(d => d.Country, o => o.MapFrom(s => s.Country.Name)); 

            CreateMap<WarehouseCreateEditDto, Warehouse>();
        }

        // Methods called when mapping manually
        private List<CategoryDto> MapForCategories(ChildrenItem childrenItem, ChildrenItemDto childrenItemDto)
        {
            var result = new List<CategoryDto>();

            if (childrenItem.ChildrenItemCategories != null)
            {
                foreach (var category in childrenItem.ChildrenItemCategories)
                {
                    result.Add(new CategoryDto() { Id = category.CategoryId, 
                    Name = category.Category.Name });
                }
            }
            return result;
        }

        private List<DiscountDto> MapForDiscounts(ChildrenItem childrenItem, ChildrenItemDto childrenItemDto)
        {
            var result = new List<DiscountDto>();

            if (childrenItem.ChildrenItemDiscounts != null)
            {
                foreach (var discount in childrenItem.ChildrenItemDiscounts)
                {
                    result.Add(new DiscountDto() { Id = discount.DiscountId, 
                    Name = discount.Discount.Name, 
                    DiscountPercentage = discount.Discount.DiscountPercentage,
                    StartDate = discount.Discount.StartDate,
                    EndDate = discount.Discount.EndDate });
                }
            }
            return result;
        }

        private List<ManufacturerDto> MapForManufacturers(ChildrenItem childrenItem, ChildrenItemDto childrenItemDto)
        {
            var result = new List<ManufacturerDto>();

            if (childrenItem.ChildrenItemManufacturers != null)
            {
                foreach (var manufacturer in childrenItem.ChildrenItemManufacturers)
                {
                    result.Add(new ManufacturerDto() { Id = manufacturer.ManufacturerId, 
                    Name = manufacturer.Manufacturer.Name });
                }
            }
            return result;
        }

        private List<TagDto> MapForTags(ChildrenItem childrenItem, ChildrenItemDto childrenItemDto)
        {
            var result = new List<TagDto>();

            if (childrenItem.ChildrenItemTags != null)
            {
                foreach (var tag in childrenItem.ChildrenItemTags)
                {
                    result.Add(new TagDto() { Id = tag.TagId, Name = tag.Tag.Name });
                }
            }
            return result;
        }

        private List<ChildrenItemCategory> MapChildrenItemCategories
            (ChildrenItemCreateEditDto itemDto, ChildrenItem item)
        {
            var result = new List<ChildrenItemCategory>();

            if (itemDto.CategoriesIds == null) { return result; }

            foreach (var id in itemDto.CategoriesIds)
            {
                result.Add(new ChildrenItemCategory() { CategoryId = id });
            }
            return result;
        }

        private List<ChildrenItemDiscount> MapItemDiscounts
            (ChildrenItemCreateEditDto itemDto, ChildrenItem item)
        {
            var result = new List<ChildrenItemDiscount>();

            if (itemDto.DiscountsIds == null) { return result; }

            foreach (var id in itemDto.DiscountsIds)
            {
                result.Add(new ChildrenItemDiscount() { DiscountId = id });
            }
            return result;
        }

        private List<ChildrenItemManufacturer> MapChildrenItemManufacturers
            (ChildrenItemCreateEditDto itemDto, ChildrenItem item)
        {
            var result = new List<ChildrenItemManufacturer>();

            if (itemDto.ManufacturersIds == null) { return result; }

            foreach (var id in itemDto.ManufacturersIds)
            {
                result.Add(new ChildrenItemManufacturer() { ManufacturerId = id });
            }
            return result;
        }

        private List<ChildrenItemTag> MapChildrenItemTags
            (ChildrenItemCreateEditDto itemDto, ChildrenItem item)
        {
            var result = new List<ChildrenItemTag>();

            if (itemDto.TagsIds == null) { return result; }

            foreach (var id in itemDto.TagsIds)
            {
                result.Add(new ChildrenItemTag() { TagId = id });
            }
            return result;
        }

        private List<ChildrenItemDto> MapForChildrenItems(Discount discount, DiscountDto discountDto)
        {
            var result = new List<ChildrenItemDto>();

            if (discount.ChildrenItemDiscounts != null)
            {
                foreach (var item in discount.ChildrenItemDiscounts)
                {
                    result.Add(new ChildrenItemDto() { Id = item.ChildrenItemId, 
                    Name = item.ChildrenItem.Name, Price = item.ChildrenItem.Price });
                }
            }
            return result;
        }

        private List<CategoryDto> MapForCategories1(Discount discount, DiscountDto discountDto)
        {
            var result = new List<CategoryDto>();

            if (discount.CategoryDiscounts != null)
            {
                foreach (var category in discount.CategoryDiscounts)
                {
                    result.Add(new CategoryDto() { Id = category.CategoryId, 
                    Name = category.Category.Name });
                }
            }
            return result;
        }

        private List<ManufacturerDto> MapForManufacturers(Discount discount, DiscountDto discountDto)
        {
            var result = new List<ManufacturerDto>();

            if (discount.ManufacturerDiscounts != null)
            {
                foreach (var manufacturer in discount.ManufacturerDiscounts)
                {
                    result.Add(new ManufacturerDto() { Id = manufacturer.ManufacturerId, 
                    Name = manufacturer.Manufacturer.Name });
                }
            }
            return result;
        }

        private List<ChildrenItemDiscount> MapDiscountChildrenItems(DiscountCreateEditDto discountDto, Discount discount)
        {
            var result = new List<ChildrenItemDiscount>();

            if (discountDto.ChildrenItemsIds == null) { return result; }

            foreach (var id in discountDto.ChildrenItemsIds)
            {
                result.Add(new ChildrenItemDiscount() { ChildrenItemId = id });
            }
            return result;
        }

        private List<CategoryDiscount> MapDiscountCategories(DiscountCreateEditDto discountDto, Discount discount)
        {
            var result = new List<CategoryDiscount>();

            if (discountDto.CategoriesIds == null) { return result; }

            foreach (var id in discountDto.CategoriesIds)
            {
                result.Add(new CategoryDiscount() { CategoryId = id });
            }
            return result;
        }

        private List<ManufacturerDiscount> MapDiscountManufacturers(DiscountCreateEditDto discountDto, Discount discount)
        {
            var result = new List<ManufacturerDiscount>();

            if (discountDto.ManufacturersIds == null) { return result; }

            foreach (var id in discountDto.ManufacturersIds)
            {
                result.Add(new ManufacturerDiscount() { ManufacturerId = id });
            }
            return result;
        }

        private List<KidActivityDto> MapForKidActivities(
            BirthdayPackage birthdayPackage, BirthdayPackageDto birthdayPackageDto)
        {
            var result = new List<KidActivityDto>();

            if (birthdayPackage.BirthdayPackageKidActivities != null)
            {
                foreach (var activity in birthdayPackage.BirthdayPackageKidActivities)
                {
                    result.Add(new KidActivityDto() { Id = activity.KidActivityId, 
                    Name = activity.KidActivity.Name });
                }
            }
            return result;
        }

        private List<DiscountDto> MapForDiscounts(
            BirthdayPackage birthdayPackage, BirthdayPackageDto birthdayPackageDto)
        {
            var result = new List<DiscountDto>();

            if (birthdayPackage.BirthdayPackageDiscounts != null)
            {
                foreach (var discount in birthdayPackage.BirthdayPackageDiscounts)
                {
                    result.Add(new DiscountDto() { Id = discount.DiscountId, 
                    Name = discount.Discount.Name });
                }
            }
            return result;
        }

        private List<BirthdayPackageDiscount> MapBirthdayPackageDisounts(
                BirthdayPackageCreateEditDto birthdayDto, BirthdayPackage birthdayPackage)
        {
            var result = new List<BirthdayPackageDiscount>();

            if (birthdayDto.DiscountsIds == null) { return result; }

            foreach (var id in birthdayDto.DiscountsIds)
            {
                result.Add(new BirthdayPackageDiscount() { DiscountId = id });
            }
            return result;
        }

        private List<BirthdayPackageKidActivity> MapBirthdayPackageKidActivities(
                BirthdayPackageCreateEditDto birthdayDto, BirthdayPackage birthdayPackage)
        {
            var result = new List<BirthdayPackageKidActivity>();

            if (birthdayDto.KidActivitiesIds == null) { return result; }

            foreach (var id in birthdayDto.KidActivitiesIds)
            {
                result.Add(new BirthdayPackageKidActivity() { KidActivityId = id });
            }
            return result;
        }
    }
}







