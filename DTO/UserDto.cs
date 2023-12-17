namespace Api.DTO
{
    public class UserDto
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string PhotoUrl { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

        public List<PhotoDto> Photos { get; set; }


    }
}