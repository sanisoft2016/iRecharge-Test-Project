namespace iRechargeTestProject.Domain.Enum
{
    public enum CURRENT_STATUS : byte
    {
        CURRENT = 1,
        PREVIOUS,
    }

    public enum USER_TYPE : byte
    {
        SUPER_ADMIN = 1,
        ADMIN,
        CUSTOMER
    }

    public enum ACCESS_STATUS : byte
    {
        ENABLED = 1,
        DISABLE,
    }
    public enum GENDER : byte
    {
        MALE = 1,
        FEMALE,
    }
}