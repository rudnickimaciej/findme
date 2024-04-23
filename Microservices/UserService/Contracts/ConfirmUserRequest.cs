namespace UserService.Contracts
{
    public class ConfirmUserRequest
    {
        public string UserId { get; set; }
        public string Token { get; set; }
    }
}
