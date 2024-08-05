namespace Play.Identity.Service.Dtos
{
    public  record UserDto(Guid Id, string Username,string Email,decimal Gil,DateTimeOffset CreatedDate);
    public record UpdateUserDto(string Email,decimal Gil);
}
