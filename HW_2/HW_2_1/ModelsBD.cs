using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson_6_Cashier
{
    public class ModelsBD
    {
      //  public List<ProductShop> arrProductShop { get; set; }
      //  public List<ProductMoving> arrProductMoving { get; set; }
     //   public List<UserRoleShop> arrUserRoleShop { get; set; }
      //  public List<UserShop> arrUserShop { get; set; }

    }
    [Serializable]
    public class ProductShop
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public float PriceBuy { get; set; }

        public float PriceSell { get; set; }

        public UoM UoM { get; set; }
    }
    public enum UoM
    {
        [Description("Штуки")]
        Piece,
        [Description("Кг")]
        Kg,
        [Description("Литр")]
        Liter,
        [Description("Банки")]
        Cans
    }
    [Serializable]
    public class ProductMoving
    {
        public int ID { get; set; }
        public int IDproduct { get; set; }
        public double CountStore { get; set; }
        public double CountShop { get; set; }
        public double Sold { get; set; }

    }
    [Serializable]
    public class UserRoleShop
    {
        public int ID { get; set; }
        public UserRole Role { get; set; }
    }

    public enum UserRole
    {
        Cashier,
        Manager
    }

    [Serializable]
    public class UserShop
    {
        public int ID { get; set; }
        public string Login { get; set; }
        public string Passwword { get; set; }
        public int IDrole { get; set; }
    }
    public enum TypeBD
    {
        XML,
        SQL,
        Entity
    }
}
