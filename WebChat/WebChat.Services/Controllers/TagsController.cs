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

        //GET api/Tags
        public IEnumerable<Tag> GetAllTags()
        {
            var tags = this.Data.Tags
                .GetAll()
                .OrderByDescending(c => c.Rooms.Count)
                .ThenBy(c => c.Name);

            return tags;
        }

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

        //GET api/Tags/tagId
        public IHttpActionResult GetTagById(int id)
        {
            var tag = this.Data.Tags.GetAll().FirstOrDefault(t => t.Id == id);

            if (tag == null)
            {
                return this.BadRequest(string.Format("Tag with id #{0} doesn't exist", id));
            }

            return this.Ok(tag);
        }
    }
}
