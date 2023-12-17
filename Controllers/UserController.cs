using System.Security.Cryptography;
using System.Text;
using Api.Context;
using Api.DTO;
using Api.Extensions;
using Api.Model;
using Api.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{

    [ApiController]
    [Route("/api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly UserContext _context;
        private readonly ITokenServices _tokenServices;
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;
        private readonly ILogger<UserController> _logger;


        public UserController(UserContext context, ITokenServices tokenServices, IUserRepository userRepo, IMapper mapper, IPhotoService photoService)
        {
            _photoService = photoService;
            _mapper = mapper;
            _userRepo = userRepo;
            _context = context;
            _tokenServices = tokenServices;

        }


        [AllowAnonymous]
        [HttpPost("signin")]

        public async Task<ActionResult<ReturnDto>> Signin(LoginDto dto)
        {


            var user = await _userRepo.GetUserByUserNameAsync(dto.UserName);

            if (user == null) return NotFound("User Is Not Exist");

            var hmac = new HMACSHA512(user.SaltPassword);


            var calculatedPAss = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password));
            for (int i = 0; i < dto.Password.Length; i++)
            {
                if (user.HashPassword[i] != calculatedPAss[i]) return Unauthorized("Password Is Invalid");
            }
            var urlphoto = "";
            for (int i = 0; i < user.Photos.Count; i++)
            {
                if (user.Photos[i].IsMain == true) urlphoto = user.Photos[i].Url;
            }
            return new ReturnDto
            {
                Username = user.UserName,
                PhotoUrl = urlphoto,

                Token = _tokenServices.CreateToken(user)
            };
        }





        [AllowAnonymous]
        [HttpPost("signup")]

        public async Task<ActionResult<ReturnDto>> Signup(SignUpDto dto)
        {

            var hmac = new HMACSHA512();



            var user = await _userRepo.GetUserByUserNameAsync(dto.UserName);

            if (user != null) return BadRequest("User already exists");

            var Newuser = _mapper.Map<UserModel>(dto);
            var age = user.Birthday.GetAgeFromBirth();
            Newuser.HashPassword = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password));
            Newuser.SaltPassword = hmac.Key;
            Newuser.Age = age;

            _userRepo.AddUser(Newuser);
            await _userRepo.SaveChangesAsync();
            var token = _tokenServices.CreateToken(Newuser);

            var urlphoto = "";
            for (int i = 0; i < Newuser.Photos.Count; i++)
            {
                if (Newuser.Photos[i].IsMain == true) urlphoto = Newuser.Photos[i].Url;
            }



            return new ReturnDto
            {
                Username = Newuser.UserName,
                PhotoUrl = urlphoto,
                Token = token
            };
        }




        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserModel>>> GetUsers()
        {
            var users = await _userRepo.GetAllUsersAsync();
            return Ok(_mapper.Map<IEnumerable<UserDto>>(users));



        }



        [HttpGet("account")]
        public async Task<ActionResult<UserDto>> GetMyAcc()
        {
        var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userRepo.GetUserByUserNameAsync(username);
            return Ok(_mapper.Map<UserDto>(await _userRepo.GetUserByUserNameAsync(username)));

        }
        [HttpGet("{username}")]
        public async Task<ActionResult<UserModel>> GetUser(string username)
        {

            return Ok(_mapper.Map<ReturnDto>(await _userRepo.GetUserByUserNameAsync(username)));

        }


        [HttpPost("addPhoto")]

        public async Task<ActionResult<Photo>> AddPhotoAsy(IFormFile file)
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userRepo.GetUserByUserNameAsync(username);
            if (user == null) return NotFound();
            var result = await _photoService.AddPhotoAsync(file);
            if (result.Error != null) return BadRequest();



            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };
            if (user.Photos.Count == 0) photo.IsMain = true;
            user.Photos.Add(photo);
            if (await _userRepo.SaveChangesAsync()) return CreatedAtAction(nameof(GetUser), new
            {
                username = user.UserName
            }, _mapper.Map<PhotoDto>(photo));





            return BadRequest("there is  A Problem");



        }


        [HttpGet("get_photo")]

        public async Task<IEnumerable<PhotoDto>> GetPhoto()
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userRepo.GetUserByUserNameAsync(username);

            return _mapper.Map<PhotoDto[]>(user.Photos.ToList());
            //  await _context.Users.SelectMany(user => user.Photos).Select(photo => _mapper.Map<PhotoDto>(photo)).ToListAsync();
        }
//         [HttpPost("add_like/{id}")]

//         public async Task<IEnumerable<PhotoDto>> AddLike(int id)
//         {
//             var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
//             var user = await _userRepo.GetUserByUserNameAsync(username);

//             user.Favourites.Add(new FavouritePhoto{ImageId= id});
//             await _context.SaveChangesAsync();
//             var favPhotos = user.Favourites; 
// var photosForFavorite = _context.Users.
//     .Where(p => user.Any(f => f.Id == favoriteId && f.PhotoId == p.Id))
//     .ToList();

    }
}