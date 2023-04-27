namespace KTLT_QuanLyCuaHang.Constants
{
    public class ExpirationStatus
    {
        public static readonly string EXPIRED = "expired";
        public static readonly string NEAR_EXPIRATION_7 = "near_expiration_7";
        public static readonly string NEAR_EXPIRATION_14 = "near_expiration_14";
        public static readonly string GOOD = "good";
        public static readonly string UNEXPIRED = "unexpired";
        public static readonly string ALL_EXPIRATION_STATUS = "all_expiration_status";
    }

    public enum ExpirationStatusEnum
    {
        EXPIRED,
        UNEXPIRED,
        ALL_EXPIRATION_STATUS,
    }
}
