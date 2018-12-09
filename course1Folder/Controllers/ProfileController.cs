﻿using course1Folder.CustomAuthorization;
using course1Folder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace course1Folder.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private long? _currentUserId { get { return ((CustomAuthorization.CustomPrincipal)User)?.UserId; } }

        private ProfileModel InitModel()
        {
            var userDB = BLL.Data.GetUserDB(((CustomPrincipal)User).UserId);
            var subs = BLL.Data.GetAllSubscribtionsForUser(userDB.Id);
            var posts = BLL.Data.GetPostsOfUserDB(userDB.Id,null);

            var model = new ProfileModel
            {
                User = new UserModel(userDB),
                SubsPanel = new SubsPanelModel { Subs = subs },
                UserPosts = new FeedModel { NextExist=true,Posts=posts}
            };

            return model;
        }

        // GET: Profile
        public ActionResult Index()
        {
            return View(InitModel());
        }

        [HttpGet]
        public ActionResult EditProfile()
        { 
            return View(InitModel().User);
        }

        [HttpPost]
        public ActionResult EditProfile(UserModel model, HttpPostedFileBase Avatar)
        {
            var user = BLL.Data.GetUserDB(model.Id);

            if (Avatar != null)
            {
                BLL.Data.SetAvatar(model.Id, new BLL.DTO.ImageContent(Avatar));
            }

            user.Login = model.Login;
            user.Nickname = model.Nickname;
            user.SharedProfile = model.SharedProfile;
            user.Description = model.Description;

            BLL.Data.CreateOrUpdateUser(user);

            return View(model);
        }

        public ActionResult Avatar(long Id)
        {
            var avatar = BLL.Data.GetAvatar(Id);

            if (avatar.Content == null)

                return HttpNotFound();
            return File(avatar.Content, avatar.Mime);
        }


       

        public PartialViewResult FindUser(string nick)
        {
            var currUser = ((CustomAuthorization.CustomPrincipal)User).UserId;
            var model = BLL.Data.FindUserByNicknameDB(nick, currUser);
            return PartialView("_SubscribeView", model);
        }

        public JsonResult AddSubcribtion(long userId,long subTo)
        {
            var result = new JsonResultLikeSub { Success = true };
            try
            {
                var resDB = BLL.Data.CreateUpdateSubscription(userId, subTo);
                result.Added = resDB;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Result = ex.Message;
            }
            return Json(result,JsonRequestBehavior.AllowGet);
        }

        public ActionResult UserProfile(long userId)
        {
            var user = BLL.Data.GetUserDB(userId);
            var posts = BLL.Data.GetPostsOfUserDB(userId, _currentUserId);
            var model = new ProfileModel
            {
                User = new UserModel(user),
                UserPosts = new FeedModel { NextExist = true, Posts = posts }
            };
            return View(model);
        }
    }
}