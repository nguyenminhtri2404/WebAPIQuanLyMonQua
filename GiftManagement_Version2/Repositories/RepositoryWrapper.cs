using GiftManagement_Version2.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace GiftManagement_Version2.Repositories
{
    public interface IRepositoryWrapper
    {
        IRepositoryBase<User> Users { get; }
        IRepositoryBase<Gift> Gifts { get; }
        IRepositoryBase<Cart> Carts { get; }
        IRepositoryBase<Order> Orders { get; }
        IRepositoryBase<Role> Roles { get; }
        IRepositoryBase<RolePermission> RolePermissions { get; }
        IRepositoryBase<Permission> Permissions { get; }
        IRepositoryBase<Promotion> Promotions { get; }
        IRepositoryBase<RefreshToken> RefreshTokens { get; }
        IRepositoryBase<Ranking> Rankings { get; }

        void Save();
        IDbContextTransaction Transaction();
    }

    public class RepositoryWrapper : IRepositoryWrapper
    {
        private readonly MyDBContext dbContext;
        private IRepositoryBase<User> user;
        private IRepositoryBase<Gift> gift;
        private IRepositoryBase<Cart> cart;
        private IRepositoryBase<Order> order;
        private IRepositoryBase<Role> role;
        private IRepositoryBase<RolePermission> rolePermission;
        private IRepositoryBase<Permission> permission;
        private IRepositoryBase<Promotion> promotion;
        private IRepositoryBase<RefreshToken> refreshToken;
        private IRepositoryBase<Ranking> ranking;

        public RepositoryWrapper(MyDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IRepositoryBase<User> Users
        {
            get
            {
                if (user == null)
                {
                    user = new RepositoryBase<User>(dbContext);
                }
                return user;
            }
        }

        public IRepositoryBase<Gift> Gifts
        {
            get
            {
                if (gift == null)
                {
                    gift = new RepositoryBase<Gift>(dbContext);
                }
                return gift;
            }
        }

        public IRepositoryBase<Cart> Carts
        {
            get
            {
                if (cart == null)
                {
                    cart = new RepositoryBase<Cart>(dbContext);
                }
                return cart;
            }
        }

        public IRepositoryBase<Order> Orders
        {
            get
            {
                if (order == null)
                {
                    order = new RepositoryBase<Order>(dbContext);
                }
                return order;
            }
        }

        public IRepositoryBase<Role> Roles
        {
            get
            {
                if (role == null)
                {
                    role = new RepositoryBase<Role>(dbContext);
                }
                return role;
            }
        }

        public IRepositoryBase<RolePermission> RolePermissions
        {
            get
            {
                if (rolePermission == null)
                {
                    rolePermission = new RepositoryBase<RolePermission>(dbContext);
                }
                return rolePermission;
            }
        }

        public IRepositoryBase<Permission> Permissions
        {
            get
            {
                if (permission == null)
                {
                    permission = new RepositoryBase<Permission>(dbContext);
                }
                return permission;
            }
        }

        public IRepositoryBase<Promotion> Promotions
        {
            get
            {
                if (promotion == null)
                {
                    promotion = new RepositoryBase<Promotion>(dbContext);
                }
                return promotion;
            }
        }

        public IRepositoryBase<RefreshToken> RefreshTokens
        {
            get
            {
                if (refreshToken == null)
                {
                    refreshToken = new RepositoryBase<RefreshToken>(dbContext);
                }
                return refreshToken;
            }
        }

        public IRepositoryBase<Ranking> Rankings
        {
            get
            {
                if (ranking == null)
                {
                    ranking = new RepositoryBase<Ranking>(dbContext);
                }
                return ranking;
            }
        }

        public void Save() => dbContext.SaveChanges();

        public IDbContextTransaction Transaction()
        {
            return dbContext.Database.BeginTransaction();
        }
    }
}
