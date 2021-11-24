using System.Threading.Tasks;
using Application.Wrappers.Response;

namespace Application.Services.Interfaces
{
  public interface ITokenService<I> where I : class
  {
    public Task<CustomServiceResultWrapper<string>> LoginAndReturnIdToken(I input);
    public CustomServiceResultWrapper<string> ExchangeIdTokenForAccessToken(string IdToken);
    public Task<CustomServiceResultWrapper<string>> ExchangeIdTokenForAccessToken();
  }

}