using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ghostDataAccess;

namespace ghost.Controllers
{
    public class ArtistsController : ApiController
    {
        public IEnumerable<Artist> Get()
        {
            using (ghostDBEntities entities = new ghostDBEntities())
            {
                return entities.Artists.ToList();
            }
        }
        public HttpResponseMessage Get(int id)
        {
            using (ghostDBEntities entities = new ghostDBEntities())
            {
                var entity = entities.Artists.FirstOrDefault(e => e.idArtist.Equals(id));
                if (entity != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Artist with id '" + id.ToString() + "' not found.");
                }
            }
        }
        public HttpResponseMessage Get(string name)
        {
            using (ghostDBEntities entities = new ghostDBEntities())
            {
                var entity = entities.Artists.FirstOrDefault(e => e.Name.ToLower().Contains(name.ToLower()));
                if (entity != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Artists with name like '" + name + "' not found.");
                }
            }
        }
        public HttpResponseMessage Post([FromBody]Artist artist)
        {
            try
            {
                using (ghostDBEntities entities = new ghostDBEntities())
                {
                    entities.Artists.Add(artist);
                    entities.SaveChanges();

                    var message = Request.CreateResponse(HttpStatusCode.Created, artist);
                    message.Headers.Location = new Uri(Request.RequestUri + artist.idArtist.ToString());
                    return message;
                }
            }
            catch(Exception ex)
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
                    var entity = entities.Artists.FirstOrDefault(e => e.idArtist.Equals(id));
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Artist with id '" + id.ToString() + "' not found.");
                    }
                    else
                    {
                        entities.Artists.Remove(entity);
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
        public HttpResponseMessage Put(int id, [FromBody]Artist artist)
        {
            try
            {
                using (ghostDBEntities entities = new ghostDBEntities())
                {
                    var entity = entities.Artists.FirstOrDefault(e => e.idArtist.Equals(id));

                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Artist with id '" + id.ToString() + "' not found.");
                    }
                    else
                    {
                        entity.Name = artist.Name;
                        entity.UrlName = artist.UrlName;
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
