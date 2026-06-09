using AutoMapper;
using DotNote.Model;
using DotNote.DTOs;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDetails>();
        CreateMap<User, FirebaseAuthDTO>();
    }
}