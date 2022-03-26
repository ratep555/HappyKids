using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Dtos.Identity;
using Core.Dtos.StatisticsDtos;
using Core.Entities.Identity;
using Core.Interfaces;
using Core.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly HappyKidsContext _context;
        public AdminRepository(HappyKidsContext context)
        {
            _context = context;
        }

        public async Task<List<UserToReturnDto>> GetAllUsers(QueryParameters queryParameters)
        {
            var userRoles = await _context.UserRoles.Include(x => x.User).Include(x => x.Role)
                .Where(x => x.RoleId == queryParameters.RoleId).ToListAsync();

            var userRolesUserIds = userRoles.Select(x => x.UserId);

            IQueryable<UserToReturnDto> users = (from u in _context.Users
                                               select new UserToReturnDto 
                                               {
                                                   Username = u.UserName,
                                                   Email = u.Email,
                                                   UserId = u.Id,
                                                   LockoutEnd = u.LockoutEnd,
                                                   Roles = u.UserRoles.Select(r => r.Role.Name).ToList()
                                                }).AsQueryable().OrderBy(x => x.Username);
            
            if (queryParameters.HasQuery())
            {
                users = users.Where(x => x.Username.Contains(queryParameters.Query));
            }

            if (queryParameters.RoleId.HasValue)
            {
                users = users.Where(x => userRolesUserIds.Contains(x.UserId));
            }

            users = users.Skip(queryParameters.PageCount * (queryParameters.Page - 1))
                .Take(queryParameters.PageCount);
            
            return await users.ToListAsync();       
        }

        public async Task<int> GetCountForUsers()
        {
            return await _context.Users.CountAsync();
        }

        public async Task<ApplicationUser> FindUserById(int id)
        {
            return await _context.Users.Where(a => a.Id == id).FirstOrDefaultAsync();
        }

        public async Task LockUser(int id)
        {
            var userFromDb = await FindUserById(id);

            userFromDb.LockoutEnd = DateTime.Now.AddYears(1000);

            await _context.SaveChangesAsync();
        }

        public async Task UnlockUser(int id)
        {
            var userFromDb = await FindUserById(id);

            userFromDb.LockoutEnd = null;

            await _context.SaveChangesAsync();
        }

        public async Task<List<ApplicationRole>> GetRolesAssociatedWithUsers()
        {
            var userRoles = await _context.UserRoles.ToListAsync();

            IEnumerable<int> ids = userRoles.Select(x => x.RoleId);

            var roles = await _context.Roles.Where(x => ids.Contains(x.Id))
                .OrderBy(x => x.Name).ToListAsync();

            return roles;
        }

        public async Task<StatisticsDto> ShowCountForEntities()
        {
            var clientRoles = await _context.Roles.Where(x => x.Name == "Client").ToListAsync();

            IEnumerable<int> ids = clientRoles.Select(x => x.Id);

            var clientUserRoles = await _context.UserRoles.Where(x => ids.Contains(x.RoleId)).ToListAsync();

            IEnumerable<int> ids1 = clientUserRoles.Select(x => x.UserId);

            var statistics = new StatisticsDto();

            statistics.KidActivitiesCount = await _context.KidActivities.CountAsync();
            statistics.BirthdayPackagesCount = await _context.BirthdayPackages.CountAsync();
            statistics.ChildrenItemsCount = await _context.ChildrenItems.CountAsync();
            statistics.ClientsCount = await _context.Users.Where(x => ids1.Contains(x.Id)). CountAsync();
            statistics.BirthdayOrdersCount = await _context.ClientBirthdayOrders.CountAsync();
            statistics.ChildrenItemsOrdersCount = await _context.ClientOrders.CountAsync();

            return statistics;
        }

        public async Task<IEnumerable<BuyersPaymentOptionsChart>> GetNumberOfBuyersForEachPaymentOption()
        {
            List<BuyersPaymentOptionsChart> list = new List<BuyersPaymentOptionsChart>();

            var generalCardSlip = await _context.PaymentOptions.Where(x => x.Name == "General Card Slip").ToListAsync();

            IEnumerable<int> ids = generalCardSlip.Select(x => x.Id);

            var cod = await _context.PaymentOptions.Where(x => x.Name == "Cash on Delivery (COD)").ToListAsync();

            IEnumerable<int> ids1 = cod.Select(x => x.Id);

            list.Add(new BuyersPaymentOptionsChart { PaymentOption = "Stripe", 
                NumberOfBuyers = await _context.ClientOrders.Where(x => x.PaymentIntentId != null).CountAsync()  });

            list.Add(new BuyersPaymentOptionsChart { PaymentOption = "General Card Slip", 
                NumberOfBuyers = await _context.ClientOrders.Where(x => ids.Contains(x.PaymentOptionId)).CountAsync()  });

            list.Add(new BuyersPaymentOptionsChart { PaymentOption = "Cash on Delivery", 
                NumberOfBuyers = await _context.ClientOrders.Where(x => ids1.Contains(x.PaymentOptionId)).CountAsync()  });
            
            return list;
        }

        public async Task<IEnumerable<ClientOrderStatusesChart>> GetAllOrderStatusesForChildrenItems()
        {
            List<ClientOrderStatusesChart> list = new List<ClientOrderStatusesChart>();

            var delivered = await _context.OrderStatuses.Where(x => x.Name == "Delivered").ToListAsync();

            IEnumerable<int> ids = delivered.Select(x => x.Id);

            var shipped = await _context.OrderStatuses.Where(x => x.Name == "Shipped").ToListAsync();

            IEnumerable<int> ids1 = shipped.Select(x => x.Id);

            var notyetshipped = await _context.OrderStatuses.Where(x => x.Name == "Not yet Shipped").ToListAsync();

            IEnumerable<int> ids2 = notyetshipped.Select(x => x.Id);

            var failed = await _context.OrderStatuses.Where(x => x.Name == "Failed Payment").ToListAsync();

            IEnumerable<int> ids3 = failed.Select(x => x.Id);

            var received = await _context.OrderStatuses.Where(x => x.Name == "Received Payment").ToListAsync();

            IEnumerable<int> ids4 = received.Select(x => x.Id);

            var pending = await _context.OrderStatuses.Where(x => x.Name == "Pending Payment").ToListAsync();

            IEnumerable<int> ids5 = pending.Select(x => x.Id);

            var rejected = await _context.OrderStatuses.Where(x => x.Name == "Order Rejected").ToListAsync();

            IEnumerable<int> ids6 = rejected.Select(x => x.Id);

            var accepted = await _context.OrderStatuses.Where(x => x.Name == "Order Accepted").ToListAsync();

            IEnumerable<int> ids7 = accepted.Select(x => x.Id);

            list.Add(new ClientOrderStatusesChart { OrderStatus = "Delivered", 
                Count = await _context.ClientOrders.Where(x => ids.Contains((int)x.OrderStatusId)).CountAsync()  });

            list.Add(new ClientOrderStatusesChart { OrderStatus = "Shipped", 
                Count = await _context.ClientOrders.Where(x => ids1.Contains((int)x.OrderStatusId)).CountAsync()  });

            list.Add(new ClientOrderStatusesChart { OrderStatus = "Not yet Shipped", 
                Count = await _context.ClientOrders.Where(x => ids2.Contains((int)x.OrderStatusId)).CountAsync()  });
            
            list.Add(new ClientOrderStatusesChart { OrderStatus = "Failed Payment", 
                Count = await _context.ClientOrders.Where(x => ids3.Contains((int)x.OrderStatusId)).CountAsync()  });
            
            list.Add(new ClientOrderStatusesChart { OrderStatus = "Received Payment", 
                Count = await _context.ClientOrders.Where(x => ids4.Contains((int)x.OrderStatusId)).CountAsync()  });
            
            list.Add(new ClientOrderStatusesChart { OrderStatus = "Pending Payment", 
                Count = await _context.ClientOrders.Where(x => ids5.Contains((int)x.OrderStatusId)).CountAsync()  });
            
            list.Add(new ClientOrderStatusesChart { OrderStatus = "Order Rejected", 
                Count = await _context.ClientOrders.Where(x => ids6.Contains((int)x.OrderStatusId)).CountAsync()  });
            
            list.Add(new ClientOrderStatusesChart { OrderStatus = "Order Accepted", 
                Count = await _context.ClientOrders.Where(x => ids7.Contains((int)x.OrderStatusId)).CountAsync()  });
            
            return list;
        }

        public async Task<IEnumerable<BirthdayOrdersStatusesChart>> GetAllOrderStatusesForBirthdayOrders()
        {
            List<BirthdayOrdersStatusesChart> list = new List<BirthdayOrdersStatusesChart>();

            var delivered = await _context.OrderStatuses.Where(x => x.Name == "Delivered").ToListAsync();

            IEnumerable<int> ids = delivered.Select(x => x.Id);

            var shipped = await _context.OrderStatuses.Where(x => x.Name == "Shipped").ToListAsync();

            IEnumerable<int> ids1 = shipped.Select(x => x.Id);

            var notyetshipped = await _context.OrderStatuses.Where(x => x.Name == "Not yet Shipped").ToListAsync();

            IEnumerable<int> ids2 = notyetshipped.Select(x => x.Id);

            var failed = await _context.OrderStatuses.Where(x => x.Name == "Failed Payment").ToListAsync();

            IEnumerable<int> ids3 = failed.Select(x => x.Id);

            var received = await _context.OrderStatuses.Where(x => x.Name == "Received Payment").ToListAsync();

            IEnumerable<int> ids4 = received.Select(x => x.Id);

            var pending = await _context.OrderStatuses.Where(x => x.Name == "Pending Payment").ToListAsync();

            IEnumerable<int> ids5 = pending.Select(x => x.Id);

            var rejected = await _context.OrderStatuses.Where(x => x.Name == "Order Rejected").ToListAsync();

            IEnumerable<int> ids6 = rejected.Select(x => x.Id);

            var accepted = await _context.OrderStatuses.Where(x => x.Name == "Order Accepted").ToListAsync();

            IEnumerable<int> ids7 = accepted.Select(x => x.Id);

            list.Add(new BirthdayOrdersStatusesChart { OrderStatus = "Delivered", 
                Count = await _context.ClientBirthdayOrders.Where(x => ids.Contains((int)x.OrderStatusId)).CountAsync()  });

            list.Add(new BirthdayOrdersStatusesChart { OrderStatus = "Shipped", 
                Count = await _context.ClientBirthdayOrders.Where(x => ids1.Contains((int)x.OrderStatusId)).CountAsync()  });

            list.Add(new BirthdayOrdersStatusesChart { OrderStatus = "Not yet Shipped", 
                Count = await _context.ClientBirthdayOrders.Where(x => ids2.Contains((int)x.OrderStatusId)).CountAsync()  });
            
            list.Add(new BirthdayOrdersStatusesChart { OrderStatus = "Failed Payment", 
                Count = await _context.ClientBirthdayOrders.Where(x => ids3.Contains((int)x.OrderStatusId)).CountAsync()  });
            
            list.Add(new BirthdayOrdersStatusesChart { OrderStatus = "Received Payment", 
                Count = await _context.ClientBirthdayOrders.Where(x => ids4.Contains((int)x.OrderStatusId)).CountAsync()  });
            
            list.Add(new BirthdayOrdersStatusesChart { OrderStatus = "Pending Payment", 
                Count = await _context.ClientBirthdayOrders.Where(x => ids5.Contains((int)x.OrderStatusId)).CountAsync()  });
            
            list.Add(new BirthdayOrdersStatusesChart { OrderStatus = "Order Rejected", 
                Count = await _context.ClientBirthdayOrders.Where(x => ids6.Contains((int)x.OrderStatusId)).CountAsync()  });
            
            list.Add(new BirthdayOrdersStatusesChart { OrderStatus = "Order Accepted", 
                Count = await _context.ClientBirthdayOrders.Where(x => ids7.Contains((int)x.OrderStatusId)).CountAsync()  });
            
            return list;
        }
    }
}










