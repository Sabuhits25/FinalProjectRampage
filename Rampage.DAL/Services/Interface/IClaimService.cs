namespace Rampage.DAL.Services.Interface
{
    public interface IClaimService
    {
        string GetUserId();
        string GetClaim(string key);
    }
}
