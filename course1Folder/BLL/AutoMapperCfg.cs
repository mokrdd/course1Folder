using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using course1Folder.BLL.DTO;
using course1Folder.DAL;

namespace course1Folder.BLL.DTOBLL
{
    public static class AutoMapperCfg
    {
        public static void RegisterMappings()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<DAL.Users, DTO.UserDTO>();
                cfg.CreateMap<DAL.Posts, DTO.PostDTO>();
                cfg.CreateMap<DAL.Photos, DTO.PhotoDTO>();
                cfg.CreateMap<DAL.Comments, DTO.CommentDTO>();
                cfg.CreateMap<DAL.Likes, DTO.LikesDTO>();
                cfg.CreateMap<DTO.UserDTO, DAL.Users>();
                cfg.CreateMap<DTO.PostDTO, DAL.Posts>();
                cfg.CreateMap<DTO.PhotoDTO, DAL.Photos>();
                cfg.CreateMap<DTO.CommentDTO, DAL.Comments>();
                cfg.CreateMap<DTO.LikesDTO, DAL.Likes>();

            });
        }
    }
}