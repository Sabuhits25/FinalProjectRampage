namespace Rampage.BLL.Services.Interfaces
{
    public interface IClaimService
    {
        string GetUserId();
        string GetClaim(string key);
    }
}
