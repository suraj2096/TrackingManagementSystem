namespace DataTrackingSystem.Service.IService
{
    public interface ITokenHandler
    {
        public string GenerateToken(string userId);
        public string? GetUserIdFromToken(string userToken);

    }
}
