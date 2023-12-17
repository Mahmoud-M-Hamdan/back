using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Api.Extensions;

namespace Api.Model
{
    public class UserModel
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhotoUrl { get; set; } ="https://media.istockphoto.com/id/1223671392/vector/default-profile-picture-avatar-photo-placeholder-vector-illustration.jpg?s=170667a&w=0&k=20&c=m-F9Doa2ecNYEEjeplkFCmZBlc5tm1pl1F7cBCh9ZzM=";
        public string UserName { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public byte[] HashPassword { get; set; }
        public byte[] SaltPassword { get; set; }
        public DateOnly Birthday { get; set; }

        public List<Photo> Photos { get; set; }
        public List<FavouritePhoto> Favourites { get; set; }
        public int GetAge()
        {
            return Birthday.GetAgeFromBirth();
        }
    }


}