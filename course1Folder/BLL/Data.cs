using course1Folder.BLL.DTO;
using course1Folder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace course1Folder.BLL
{
    public class Data
    {
        static Data() { }

        public static long CreateOrUpdateUser(UserDTO user)
        {
            using (var context = new DAL.instDBEntities())
            {
                var userDB = context.Users.FirstOrDefault(x => x.Id == user.Id) ?? context.Users.Add(new DAL.Users());

                if (context.Users.Any(x => x.Nickname == user.Nickname && x.Id != userDB.Id))
                    throw new Exception($"Никнейм {userDB.Nickname} занят");


                userDB.BirthDate = user.BirthDate;
                userDB.Description = user.Description;
                userDB.Login = user.Login;
                userDB.Nickname = user.Nickname;
                userDB.PasswordHash = user.PasswordHash;
                userDB.RegDate = user.RegDate;
                userDB.Salt = user.Salt;
                userDB.SharedProfile = user.SharedProfile;

                context.SaveChanges();

                return userDB.Id;
            }
        }


        //получение дто из бд
        public static UserDTO GetUserDB(long? id = null, string Login = null)
        {
            if (!id.HasValue && string.IsNullOrEmpty(Login))
                return null;
            try
            {
                using (var ctx = new DAL.instDBEntities())
                {
                    var user = ctx.Users.Where(x => x.Id == id || x.Login == Login).Select(x => new UserDTO()
                    {
                        Id = x.Id,
                        Login = x.Login,
                        PasswordHash = x.PasswordHash,
                        Salt = x.Salt,
                        Nickname = x.Nickname,
                        RegDate = x.RegDate,
                        BirthDate = x.BirthDate,
                        Description=x.Description,
                        SharedProfile = x.SharedProfile,
                    }).FirstOrDefault();
                    if (user != null)

                        return user;

                    throw new Exception($"Not found User with ID:{id}");
                }
            }
            catch (Exception ex)
            {
                //throw;
                return null;
            }

        }

        public static bool CreateUpdateSubscription(long userId, long subTo)
        {
            //true - added sub
            //false - unsubed
            using (var ctx = new DAL.instDBEntities())
            {
                bool result;
                var subDB = ctx.Subscribes.FirstOrDefault(x => x.UserId == userId && x.FollowingId== subTo);

                if (subDB == null)
                {
                    subDB = ctx.Subscribes.Add(new DAL.Subscribes{
                        UserId = userId,
                        Date = DateTime.Now,
                        FollowingId = subTo,
                        isActive = true
                    });
                    result = true;

                }
                else
                {
                    //если подписка существует в базе, то меняем её статус на противоположный
                    subDB.isActive = !subDB.isActive;
                    result = subDB.isActive;
                }

                ctx.SaveChanges();

                return result;
            }
        }

        public static bool ValidateUser(string login, string password)
        {
            var user = BLL.Data.GetUserDB(Login: login);
            if (user != null)
            {
                var salt = Convert.FromBase64String(user.Salt);
                var passhash = BLL.Hash.GenerateSaltedHash(password, salt);

                var oldHash = Convert.FromBase64String(user.PasswordHash);

                return BLL.Hash.CompareArrays(passhash, oldHash);
            }
            return false;
        }

        public static void UpdatePhotoPost(DAL.Photos photo, long postId)
        {
            using (var ctx = new DAL.instDBEntities())
            {
                photo.PostId = postId;
                ctx.SaveChanges();

            }
        }

        public static long? CreateUpdatePost(PostDTO post)
        {
            using (var ctx = new DAL.instDBEntities())
            {
                var imgIds = post.Photos.Select(x => x.Id).ToArray();
                var images = ctx.Photos.Where(x => imgIds.Contains(x.Id)).ToList();

                var dbPost = new DAL.Posts
                {
                    UserId=post.UserId,
                    Description = post.Description,
                    LocationName = post.LocationName,
                };

                dbPost.Photos.Clear();
                images.ForEach((x) => dbPost.Photos.Add(x));
                dbPost.LoadDate = DateTime.Now;
                dbPost.PublicateDate = DateTime.Now;


                ctx.Posts.Add(dbPost);
                ctx.SaveChanges();

                return dbPost.Id;
            }
        }

        public static long AddPhoto(long UserId, ImageContent ImageData)
        {
            using (var ctx = new DAL.instDBEntities())
            {
                var img = ctx.Photos.Add(new DAL.Photos
                {
                    UserId = UserId,
                    LoadDate = DateTime.Now,
                    ImageContent = ImageData.Content,
                    MimeType = ImageData.Mime,
                });
                
                ctx.SaveChanges();

                return img.Id;
            }
        }
        

        public static void DelImageDB(long ImageId)
        {
            using (var ctx = new DAL.instDBEntities())
            {
                var img = ctx.Photos.FirstOrDefault(x => x.Id == ImageId);
                if (img == null)
                    throw new Exception("Нет такой фотки");

                ctx.Photos.Remove(img);

                ctx.SaveChanges();
            }

        }
        public static ImageContent GetPhotoDB(long PhotoId)
        {
            var res = new ImageContent();
            using (var ctx = new DAL.instDBEntities())
            {
                var userDB = ctx.Photos.FirstOrDefault(x => x.Id == PhotoId);
                if (userDB == null)
                    throw new Exception("Image not found");

                res.Content = userDB.ImageContent;
                res.Mime = userDB.MimeType;
            }

            return res;

        }
        public static void SetAvatar(long UserId, ImageContent avatar)
        {
            using (var ctx = new DAL.instDBEntities())
            {
                var user = ctx.Users.FirstOrDefault(x => x.Id == UserId);
                if (user == null)
                    throw new Exception("User not found");

                user.AvatarContent = avatar.Content;
                user.AvatarMime = avatar.Mime;

                ctx.SaveChanges();
            }
        }

        public static ImageContent GetAvatar(long UserId)
        {
            var res = new ImageContent();
            using (var ctx = new DAL.instDBEntities())
            {
                var user = ctx.Users.FirstOrDefault(x => x.Id == UserId);
                if (user == null)
                    throw new Exception("User not Found");

                res.Content = user.AvatarContent;
                res.Mime = user.AvatarMime;
            }

            return res;
        }

        public static List<long> GetListImagesIds(long UserId)
        {
            var res = new List<long>();
            using (var ctx = new DAL.instDBEntities())
            {
                res.AddRange(ctx.Photos.Where(x => x.UserId == UserId && !x.PostId.HasValue).Select(x => x.Id));

            }

            return res;
        }

        public static void CollectData(List<PostDTO> res, long? userId = null, long? currUser = null)
        {
            using (var ctx = new DAL.instDBEntities())
            {
                foreach (var post in res)
                {
                    bool liked = new bool();
                    if (currUser != null)
                    {
                        liked = ctx.Likes.Where(x => x.PostId == post.Id && x.UserId == currUser.Value).Any();
                    }else
                    {
                        liked = ctx.Likes.Where(x => x.PostId == post.Id && x.UserId == userId.Value).Any();
                    }
                    

                    var photosList = ctx.Photos.Where(x => x.PostId == post.Id).Select(x => new PhotoDTO
                    {
                        Id = x.Id,
                        UserId = x.UserId,
                        PostId = x.PostId,
                        LoadDate = x.LoadDate
                    }).ToList();

                    var likesList = ctx.Likes.Where(x => x.PostId == post.Id).Select(x => new LikesDTO
                    {
                        Id = x.Id,
                        PostId = x.PostId,
                        Date = x.Date,
                        UserId = x.UserId
                    }).ToList();

                    var commentsList = ctx.Comments.Where(x => x.PostId == post.Id).Select(x => new CommentDTO
                    {
                        Id = x.Id,
                        UserId = x.UserId,
                        PostId = x.PostId,
                        Date = x.Date,
                        DateString = x.Date.ToString(),
                        ContentText = x.ContentText,
                        UserNick = x.Users.Nickname

                    }).ToList();

                    post.CurrentUserLiked = liked;
                    post.Likes = likesList;
                    post.Photos = photosList;
                    post.Comments = commentsList;

                }
            }
                
        }

        public static List<PostDTO> GetPostsModeDB(int page = 0, long? userId = null,sortMode mode = sortMode.newest)
        {
            var take = 3;
            var skip = take * page;
            var res = (List<PostDTO>)null;
            using (var ctx = new DAL.instDBEntities())
            {
                
                var subs = ctx.Subscribes.Where(x => x.UserId == userId).Select(x => new SubDTO
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    FollowingId = x.FollowingId,
                    isActive = x.isActive
                }).ToList().FindAll(x => x.isActive == true);

                List<DAL.Posts> postsList = new List<DAL.Posts>();

                if (subs.Count == 0)
                {
                    return null;
                }

                foreach (var sub in subs)
                {
                    var currPosts = ctx.Posts.Where(x => x.UserId == sub.FollowingId).ToList();
                    foreach (var post in currPosts)
                        postsList.Add(post);
                }

                if (mode == sortMode.newest)
                {
                    res = postsList.OrderByDescending(x => x.PublicateDate).Skip(skip).Take(take).Select(x => new PostDTO
                    {
                        Id = x.Id,
                        LoadDate = x.LoadDate,
                        UserId = x.UserId,
                        Description = x.Description,
                        LocationName = x.LocationName,
                        PublicateDate = x.PublicateDate,

                    }).ToList();
                }
                if (mode == sortMode.liked)
                {
                    res = postsList.OrderByDescending(x => x.Likes.Count).Skip(skip).Take(take).Select(x => new PostDTO
                    {
                        Id = x.Id,
                        LoadDate = x.LoadDate,
                        UserId = x.UserId,
                        Description = x.Description,
                        LocationName = x.LocationName,
                        PublicateDate = x.PublicateDate

                    }).ToList();
                }


                CollectData(res, userId.Value);

            }

            return res;
        }


        public static List<PostDTO> GetPostsOfUserDB(long userId,long? currUser=null, sortMode mode = sortMode.newest, int page = 0)
        {
            var take = 3;
            var skip = take * page;
            var res = (List<PostDTO>)null;
            using (var ctx = new DAL.instDBEntities())
            {
                if (mode == sortMode.newest)
                {
                    res = ctx.Posts.OrderByDescending(x => x.PublicateDate).Where(x => x.UserId == userId).Skip(skip).Take(take).Select(x => new PostDTO
                    {
                        Id = x.Id,
                        LoadDate = x.LoadDate,
                        UserId = x.UserId,
                        Description = x.Description,
                        LocationName = x.LocationName,
                        PublicateDate = x.PublicateDate,

                    }).ToList();
                }
                if (mode == sortMode.liked)
                {
                    res = ctx.Posts.OrderByDescending(x => x.Likes.Count).Where(x => x.UserId == userId).Skip(skip).Take(take).Select(x => new PostDTO
                    {
                        Id = x.Id,
                        LoadDate = x.LoadDate,
                        UserId = x.UserId,
                        Description = x.Description,
                        LocationName = x.LocationName,
                        PublicateDate = x.PublicateDate

                    }).ToList();
                }

                CollectData(res, userId,currUser);
            }
            return res;
        }



        public static PostDTO GetPostById(long id)
        {
            var res = (PostDTO)null;
            using (var ctx = new DAL.instDBEntities())
            {
                var postDB = ctx.Posts.Where(x => x.Id == id).Select(x => new PostDTO
                {
                    Id = x.Id,
                    LoadDate = x.LoadDate,
                    UserId = x.UserId,
                    Description = x.Description,
                    LocationName = x.LocationName,
                    PublicateDate = x.PublicateDate
                }).ToList();

                CollectData(postDB, postDB[0].UserId, postDB[0].UserId);

                res = postDB.FirstOrDefault();
            }

            return res;
        }


        public static long LikePostById(long postId,long? userId)
        {
            using (var context = new DAL.instDBEntities())
            {
                var likeDB = context.Likes.FirstOrDefault(x => x.PostId == postId && x.UserId == userId);
                //если такого лайка нет в базе
                if (likeDB == null)
                {
                    likeDB = context.Likes.Add(new DAL.Likes());

                    likeDB.Date = DateTime.Now;
                    likeDB.UserId = userId.Value;
                    likeDB.PostId = postId;

                    var resss = context.SaveChanges();
 
                    return likeDB.Id;
                }else
                {
                    context.Likes.Remove(likeDB);
                    context.SaveChanges();
                    return 0;
                }
                
            }
        }

        public static UserWrapper FindUserByNicknameDB(string nick,long currUser)
        {
            try
            {
                using (var ctx = new DAL.instDBEntities())
                {
                    var userDB = ctx.Users.Where(x => x.Nickname.ToUpper() == nick.ToUpper()).Select(x => new UserWrapper()
                    {
                        Id = x.Id,
                        AvatarContent = x.AvatarContent,
                        AvatarMime = x.AvatarMime,
                        Nickname = x.Nickname,
                        BirthDate = x.BirthDate,
                        Description = x.Description,
                        SharedProfile = x.SharedProfile
                    }).FirstOrDefault();

                    var subscribers = ctx.Subscribes.Where(x => x.FollowingId == userDB.Id && x.isActive == true).Count();
                    var currSubed = ctx.Subscribes.Where(x => x.UserId == currUser && x.FollowingId == userDB.Id && x.isActive == true).Any();
                    userDB.SubscribersCount = subscribers;
                    userDB.CurrUserSubed = currSubed;
    
                        return userDB;

                }
            }
            catch (Exception ex)
            {
                //throw;
                return null;
            }

        }

        public static long CreateCommentDB(CommentDTO comment)
        {
            using (var ctx = new DAL.instDBEntities())
            {
                var dbCom = new DAL.Comments {
                    Id=comment.Id,
                    UserId = comment.UserId,
                    PostId = comment.PostId,
                    Date = comment.Date,
                    ContentText = comment.ContentText
                };

                ctx.Comments.Add(dbCom);
                ctx.SaveChanges();

                return dbCom.Id;
            }
        }

        public static CommentDTO GetCommentDB(long id)
        {
            using (var ctx = new DAL.instDBEntities())
            {
                var ct = ctx.Comments.Where(x => x.Id == id).Select(x => new CommentDTO {
                    Id = x.Id,
                    UserId = x.UserId,
                    PostId = x.PostId,
                    Date = x.Date,
                    ContentText = x.ContentText,
                    DateString = x.Date.ToString()
                }).FirstOrDefault();

                var userNick = ctx.Users.Where(x => x.Id == ct.UserId).FirstOrDefault().Nickname;
                ct.UserNick = userNick;

                return ct;
            }
        }


        public static List<SubModel> GetAllSubscribtionsForUser(long currUser)
        {
            using (var ctx = new DAL.instDBEntities())
            {
                var subList = ctx.Subscribes.Where(x => x.UserId == currUser && x.isActive==true).Select(x => new SubModel
                {
                    Id = x.Id,
                    FollowingUserId = x.FollowingId,
                    FollowingUserNickname = x.Users1.Nickname
                }).ToList();

                
                return subList;
            }
        }

        public static SubModel GetSubIcon(long subTo)
        {
            using (var ctx = new DAL.instDBEntities())
            {
                var sub = ctx.Subscribes.Where(x => x.FollowingId == subTo && x.isActive == true).Select(x => new SubModel
                {
                    Id = x.Id,
                    FollowingUserId = x.FollowingId,
                    FollowingUserNickname = x.Users1.Nickname
                }).FirstOrDefault();


                return sub;
            }
        }

        public static void DeletePostDB(long id)
        {
            using (var ctx = new DAL.instDBEntities())
            {
                var post = ctx.Posts.FirstOrDefault(x => x.Id == id);
                if (post == null)
                    return;
                var likes = ctx.Likes.Where(x => x.PostId == post.Id).ToList();
                var comments = ctx.Comments.Where(x => x.PostId == post.Id).ToList();
                var photos = ctx.Photos.Where(x => x.PostId == post.Id).ToList();

                foreach(var like in likes)
                    ctx.Likes.Remove(like);

                foreach (var comment in comments)
                    ctx.Comments.Remove(comment);

                foreach (var photo in photos)
                    ctx.Photos.Remove(photo);

                ctx.Posts.Remove(post);

                ctx.SaveChanges();
            }
        }
    }
}