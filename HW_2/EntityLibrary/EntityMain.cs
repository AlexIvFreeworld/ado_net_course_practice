using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLibrary
{
    public static class EntityMain
    {
        public static List<Products> GetAllProductsFromEntity()
        {
            using (var db = new Shop_2Entities())
            {
                return db.Products.ToList();
            }
        }
        public static List<ProductsMoving> GetAllProductsMovingFromEntity()
        {
            using (var db = new Shop_2Entities())
            {
                return db.ProductsMoving.ToList();
            }
        }
        public static List<Users> GetAllUsersFromEntity()
        {
            using (var db = new Shop_2Entities())
            {
                return db.Users.ToList();
            }
        }
        public static List<UserRoles> GetAllUserRolesFromEntity()
        {
            using (var db = new Shop_2Entities())
            {
                return db.UserRoles.ToList();
            }
        }

        public static void InsertProductsToEntity(Products products)
        {
            using (var db = new Shop_2Entities())
            {
                db.Products.Add(products);
                db.SaveChanges();
            }
        }
        public static void InsertProductsMovingToEntity(ProductsMoving prMove)
        {
            using (var db = new Shop_2Entities())
            {
                db.ProductsMoving.Add(prMove);
                db.SaveChanges();
            }
        }
        public static void InsertUsersToEntity(Users users)
        {
            using (var db = new Shop_2Entities())
            {
                db.Users.Add(users);
                db.SaveChanges();
            }
        }
        public static void InsertUserRolesToEntity(UserRoles userRoles)
        {
            using (var db = new Shop_2Entities())
            {
                db.UserRoles.Add(userRoles);
                db.SaveChanges();
            }
        }
        public static void UpdateProductsToEntity(Products products)
        {
            using (var db = new Shop_2Entities())
            {
                var newproducts = db.Products.First(it => it.id == products.id);
                newproducts.Name = products.Name;
                newproducts.PriceBuy = products.PriceBuy;
                newproducts.PriceSell = products.PriceSell;
                newproducts.UoM = products.UoM;
                db.SaveChanges();
            }
        }
        public static void UpdateProductsMovingToEntity(ProductsMoving prMove)
        {
            using (var db = new Shop_2Entities())
            {
                var newproducts = db.ProductsMoving.First(it => it.id == prMove.id);
                newproducts.idProduct = prMove.idProduct;
                newproducts.CountShop = prMove.CountShop;
                newproducts.CountStore = prMove.CountStore;
                newproducts.Sold = prMove.Sold;
                db.SaveChanges();
            }
        }
        public static void UpdateUsersToEntity(Users users)
        {
            using (var db = new Shop_2Entities())
            {
                var newusers = db.Users.First(it => it.id == users.id);
                newusers.Login = users.Login;
                newusers.Passwword = users.Passwword;
                newusers.idRole = users.idRole;
                db.SaveChanges();
            }
        }
        public static void UpdateUserRolesToEntity(UserRoles userRoles)
        {
            using (var db = new Shop_2Entities())
            {
                db.UserRoles.First(it => it.id == userRoles.id).Role = userRoles.Role;
                db.SaveChanges();
            }
        }
        public static void DeleteProductsToEntity(int id)
        {
            using (var db = new Shop_2Entities())
            {
                db.Products.Remove(db.Products.First(it => it.id == id));
                db.SaveChanges();
            }
        }
        public static void DeleteProductsMovingToEntity(int id)
        {
            using (var db = new Shop_2Entities())
            {
                db.ProductsMoving.Remove(db.ProductsMoving.First(it => it.id == id));
                db.SaveChanges();
            }
        }
        public static void DeleteUsersToEntity(int id)
        {
            using (var db = new Shop_2Entities())
            {
                db.Users.Remove(db.Users.First(it => it.id == id));
                db.SaveChanges();
            }
        }
        public static void DeleteUserRolesToEntity(int id)
        {
            using (var db = new Shop_2Entities())
            {
                db.UserRoles.Remove(db.UserRoles.First(it => it.id == id));
                db.SaveChanges();
            }
        }

    }
}
