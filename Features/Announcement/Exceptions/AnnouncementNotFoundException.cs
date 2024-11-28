namespace Coffee_Ecommerce.API.Features.Announcement.Exceptions
{
    public sealed class AnnouncementNotFoundException : ApplicationException
    {
        public AnnouncementNotFoundException(string? message) : base(message)
        { }
    }
}
