namespace TicketHive.Domain.Exceptions.Base;

public static class ExceptionCode
{
    public const string DUPLICATE_EMAIL = "USER_EMAIL_DUPLICATE";
    public const string UN_AUTHORIZED = "UN_AUTHORIZED";
    public const string TOKEN_ALREADY_REVOKED = "TOKEN_ALREADY_REVOKED";

    public const string TOKEN_ALREADY_USED = "TOKEN_ALREADY_USED";

    public const string EMAIL_NOT_VERIFIED = "EMAIL_NOT_VERIFIED";
    public const string INVALID_DATE = "INVALID_DATE";

    public const string INVALID_OPERATION = "INVALID_OPERATION";
}


