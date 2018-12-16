using course1Folder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace course1Folder.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private long? _currentUserId { get { return ((CustomAuthorization.CustomPrincipal)User)?.UserId; } }
        // GET: Post
        public ActionResult Index()
        {
            var model = new Models.FeedModel { };
            model.Posts = BLL.Data.GetPostsModeDB(0, _currentUserId);
            model.NextExist = BLL.Data.GetPostsModeDB(1, _currentUserId).Any();

            return View(model);
        }

        [HttpGet]
        public PartialViewResult Create()
        {
            ViewBag.Images = BLL.Data.GetListImagesIds(((CustomAuthorization.CustomPrincipal)User).UserId);

            return PartialView();
        }

        [HttpPost]
        public PartialViewResult Create(BLL.DTO.PostDTO model)
        {
           
            model.UserId = ((CustomAuthorization.CustomPrincipal)User).UserId;
            var postId = BLL.Data.CreateUpdatePost(model);
            var resModel = BLL.Data.GetPostById(postId.Value);
            return PartialView("_PostView", resModel);
        }

        [HttpPost]
        public JsonResult UploadImages()
        {
            var result = new JsonResultResponce { Success = true };
            try
            {
                if (Request.Files != null && Request.Files.Count != 0 && Request.Files[0].ContentLength > 0)
                {
                    var file = Request.Files[0];

                    var userId = ((CustomAuthorization.CustomPrincipal)User).UserId;

                    result.Result = BLL.Data.AddPhoto(userId, new BLL.DTO.ImageContent(file));
                }
                else
                    throw new Exception("Не найдено файлов");

            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Result = ex.Message;
            }
            return Json(result);

        }

        public ActionResult GetImage(long Id)
        {
            var avatar = BLL.Data.GetPhotoDB(Id);

            if (avatar.Content == null)
                return HttpNotFound();

            return File(avatar.Content, avatar.Mime);
        }

        [HttpPost]
        public JsonResult DelImage(long id)
        {
            var result = new JsonResultResponce { Success = true };
            try
            {
                BLL.Data.DelImageDB(id);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Result = ex.Message;
            }
            return Json(result);

        }

        public PartialViewResult GetPosts(int page=0)
        {
            var model = new Models.FeedModel { };
            model.Posts = BLL.Data.GetPostsModeDB(page, _currentUserId);
            model.NextExist = BLL.Data.GetPostsModeDB(page + 1, _currentUserId).Any();

            return PartialView("_MorePostsView", model);
        }

        public PartialViewResult GetPostsSortedByLikes(int page=0)
        {
            var model = new Models.FeedModel { };
            model.Posts = BLL.Data.GetPostsModeDB(page, _currentUserId, sortMode.liked);
            model.NextExist = BLL.Data.GetPostsModeDB(page+1, _currentUserId, sortMode.liked).Any();

            return PartialView("_MorePostsView", model);
        }
        

        public PartialViewResult GetPost(long id)
        {
            var model = BLL.Data.GetPostById(id);
            return PartialView("_PostView", model);
        }

        public PartialViewResult GetPostFull(long id)
        {
            var model = BLL.Data.GetPostById(id);
            return PartialView("_FullPost", model);
        }

        [HttpPost]
        public JsonResult LikePost(long id)
        {
            var result = new JsonResultLikeSub{ Success = true };
            try
            {
                var resDB = BLL.Data.LikePostById(id, _currentUserId);
                if (resDB != 0)
                    result.Added = true;
                else
                    result.Added = false;
                result.Result = resDB;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Result = ex.Message;
            }
            return Json(result);
        }

        /*public PartialViewResult FindUserByNickname(string nick)
        {
            var model = BLL.Data.FindUserByNicknameDB(nick);
            return PartialView("_PostView", model);
        }*/

        public JsonResult GetComments(long id)
        {
            var model = BLL.Data.GetPostById(id);
            return Json(new { model.Comments }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult AddComment(long postId, string commentText)
        {
            var result = new JsonResultResponce { Success = true };
            try
            {
                var userId = _currentUserId.Value;

                var comId = BLL.Data.CreateCommentDB(new BLL.DTO.CommentDTO { UserId = userId, ContentText = commentText, PostId = postId, Date = DateTime.Now });


                result.Result = BLL.Data.GetCommentDB(comId);

            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Result = ex.Message;
            }
            return Json(result);
        }
    }
}