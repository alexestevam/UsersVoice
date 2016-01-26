namespace UsersVoice.Services.API.CQRS.Queries
{
    public enum IdeaStatusQuery
    {
        Pending,
        Cancelled,
        Approved,
        Implemented,
        All // keep this the last one
    }

    public enum IdeaSortBy
    {
        None,
        Title,
        CreationDate,
        Points
    }

    public enum UserSortBy
    {
        None,
        Name,
        Points,
        RegistrationDate
    }

    public enum SortDirection
    {
        ASC,
        DESC
    }
}