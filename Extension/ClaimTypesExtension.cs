using System.Security.Claims;

namespace JwtAspNet.Extension
{
    public static class ClaimTypesExtension
    {
        public static int Id(this ClaimsPrincipal user)
        {
            try
            {
                var id = user.Claims.FirstOrDefault(x => x.Type == "id")?.Value ?? "0";
                return int.Parse(id);
            }
            catch
            {

                return 0;
            }
        }

    }
}