using MinimalAPIDemo.Models;

namespace MinimalAPIDemo.Data
{
    public static class CouponStore
    {
        public static List<Coupon> couponList = new List<Coupon>()
        {
            new Coupon{Id=1, Name="1000C", Percent=10, IsActive=true},
            new Coupon{Id=2, Name="2000C", Percent=20, IsActive=false}
        };
    }
}

