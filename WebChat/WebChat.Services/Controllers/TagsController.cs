using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using WebChat.DataLayer.Contracts;
using WebChat.DataLayer.Data;
using WebChat.Models;
using WebChat.Services.Models.BindingModels;
using WebChat.Services.UserSessionUtilities;
using Convert = WebChat.Services.Models.Utilities.Convert;

namespace WebChat.Services.Controllers
{
    public class TagsController : BaseApiController
    {
        public TagsController()
            : this(new WebChatData())
        {
        }

        public TagsController(IWebChatData data) : base(data)
        {
        }

        
        [SessionAuthorize]
        [HttpGet]
        [Route("api/tags")]
        public IEnumerable<Tag> GetAll()
        {
            var tags = this.Data.Tags
                .GetAll()
                .OrderByDescending(c => c.Rooms.Count)
                .ThenBy(c => c.Name);

            return tags;
        }

        [SessionAuthorize]
        [HttpDelete]
        public IHttpActionResult CreateTag(CreateTagBindingModel model)
        {
            var userId = this.User.Identity.GetUserId();

            if (userId == null)
            {
                return this.Unauthorized();
            }

            var tag = new Tag()
            {
                Name = model.Name
            };

            return this.Ok(tag);
        }

        [HttpGet]
        [SessionAuthorize]
        [Route("api/tags/{tagId}")]
        public IHttpActionResult GetTagById(int tagId)
        {
            var userId = this.User.Identity.GetUserId();
            var user = this.Data.Users.GetById(userId);

            if (user == null)
            {
                return this.Unauthorized();
            }

            var tag = this.Data.Tags.GetAll().FirstOrDefault(t => t.Id == tagId);
            if (tag == null)
            {
                return this.BadRequest(string.Format("Tag with id #{0} doesn't exist", tagId));
            }

            return this.Ok(tag);
        }

        [SessionAuthorize]
        [HttpPost]
        [ActionName("createroom")]
        public IHttpActionResult CreateRoom(CreateRoomBindingModel model)
        {
            var userId = this.User.Identity.GetUserId();

            if (userId == null)
            {
                return this.Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var room = new Room
            {
                Name = model.Name
            };

            this.Data.Rooms.Add(room);
            this.Data.SaveChanges();

            return Ok(string.Format("Room with id: {0}, successfully created", room.Id));
        }

        [SessionAuthorize]
        [HttpDelete]
        [ActionName("deletetag")]
        public IHttpActionResult DeleteTag(int tagId)
        {
            var userId = this.User.Identity.GetUserId();

            if (userId == null)
            {
                return this.Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tag = Data.Tags.GetById(tagId);

            if (tag == null)
            {
                return Ok(string.Format("No tag with id: {0}", tagId));
            }

            this.Data.Rooms.Delete(tag);
            this.Data.Rooms.SaveChanges();

            return Ok(string.Format("Tag with id: {0}, successfully deleted", tagId));
        }


    }
}
