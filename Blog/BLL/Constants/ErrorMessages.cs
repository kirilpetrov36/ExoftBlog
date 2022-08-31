namespace Blog.BLL.Constants
{
    public static class ErrorMessages
    {
        public const string UserDoesntExist = "User does not exist";
        public const string NoUserWithThePassword = "Password/User combination is wrong";
        public const string NoUserWithTheRefreshToken = "There's no such user in db with the refresh-token";
        public const string IncorrectRefreshToken = "Refresh token is incorrect";
        public const string RefreshTokenExpired = "Refresh Token expired";
        public const string InvalidData = "Invalid data";
    }
}
