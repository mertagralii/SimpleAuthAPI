using AutoMapper;
using Microsoft.AspNetCore.Identity;
using SimpleAuthAPI.Model.Dtos.Comment;
using SimpleAuthAPI.Model.Dtos.Tweet;
using SimpleAuthAPI.Model.Dtos.User;
using SimpleAuthAPI.Model.Entities;

namespace SimpleAuthAPI;

// Oluşturduğumuz Dtoları mapleyeceğimiz Kısım
public class MappingProfile : Profile // Mapper'de mapleme işlemleri yaparken MappingProfile adında bir class açmalı ve Profile 'Den kalıtım yapmak gerekiyor.
{
    public MappingProfile()
    {
        CreateMap<Tweet, TweetDto>().ReverseMap();
        CreateMap<Comment, CommentDto>().ReverseMap();
        CreateMap<ApplicationUser,IdentityUser>().ReverseMap();
        CreateMap<AddTweetDto,Tweet>().ReverseMap();
        CreateMap<ApplicationUser,ApplicationUserDto>().ReverseMap();
        CreateMap<TweetDto[], Tweet>().ReverseMap();
        CreateMap<AddCommentDto, Comment>().ReverseMap();
    }
    
}