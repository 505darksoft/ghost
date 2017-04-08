using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ghostDataAccess;

namespace ghost.Controllers
{
    public class UsersController : ApiController
    {
        public IEnumerable<User> Get()
        {
            using (ghostDBEntities entities = new ghostDBEntities())
            {
                return entities.Users.ToList();
            }
        }
        public HttpResponseMessage Get(int id)
        {
            using (ghostDBEntities entities = new ghostDBEntities())
            {
                var entity = entities.Users.FirstOrDefault(e => e.idUser.Equals(id));
                if (entity != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "User with id '" + id.ToString() + "' not found.");
                }
            }
        }
        public HttpResponseMessage Get(string username)
        {
            using (ghostDBEntities entities = new ghostDBEntities())
            {
                var entity = entities.Users.FirstOrDefault(e => e.username.ToLower().Contains(username.ToLower()));
                if (entity != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "User with name like '" + username + "' not found.");
                }
            }
        }
        public HttpResponseMessage Post([FromBody]User user)
        {
            try
            {
                using (ghostDBEntities entities = new ghostDBEntities())
                {
                    entities.Users.Add(user);
                    entities.SaveChanges();

                    var message = Request.CreateResponse(HttpStatusCode.Created, user);
                    message.Headers.Location = new Uri(Request.RequestUri + user.idUser.ToString());
                    return message;
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                using (ghostDBEntities entities = new ghostDBEntities())
                {
                    var entity = entities.Users.FirstOrDefault(e => e.idUser.Equals(id));
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "User with id '" + id.ToString() + "' not found.");
                    }
                    else
                    {
                        entities.Users.Remove(entity);
                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
        public HttpResponseMessage Put(int id, [FromBody]User user)
        {
            try
            {
                using (ghostDBEntities entities = new ghostDBEntities())
                {
                    var entity = entities.Users.FirstOrDefault(e => e.idUser.Equals(id));

                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "User with id '" + id.ToString() + "' not found.");
                    }
                    else
                    {
                        entity.password = user.password;
                        entity.username = user.username;
                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
