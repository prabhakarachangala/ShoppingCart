namespace ShoppingCart.Services
{
    public class UserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void SetUserRole(string role)
        {
            // Using IHttpContextAccessor to access HttpContext and Session
            _httpContextAccessor.HttpContext.Session.SetString("UserRole", role);
        }

        public string GetUserRole()
        {
            return _httpContextAccessor.HttpContext.Session.GetString("UserRole");
        }
    }
}
