namespace Api.DTO
{
    public class PhotoDto
    {
        public int Id { get; set; }
        public string ImageName { get; set; }

        public string Url { get; set; }

        public bool IsMain { get; set; } = false;

    }
}