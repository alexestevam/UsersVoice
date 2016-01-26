namespace UsersVoice.Services.Infrastructure.Common
{
    public interface IPagingInfo
    {
        int Page { get; }

        int PageSize { get; }
    }
}