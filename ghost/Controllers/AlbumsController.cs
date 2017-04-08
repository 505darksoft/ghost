using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ghostDataAccess;

namespace ghost.Controllers
{
    public class AlbumsController : ApiController
    {
        public IEnumerable<Album> Get()
        {
            using (ghostDBEntities entities = new ghostDBEntities())
            {
                return entities.Albums.ToList();
            }
        }
        public HttpResponseMessage Get(int id)
        {
            using (ghostDBEntities entities = new ghostDBEntities())
            {
                var entity = entities.Albums.FirstOrDefault(e => e.idAlbum.Equals(id));
                if (entity != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Album with id '" + id.ToString() + "' not found.");
                }
            }
        }
        public HttpResponseMessage Get(string title)
        {
            using (ghostDBEntities entities = new ghostDBEntities())
            {
                var entity = entities.Albums.FirstOrDefault(e => e.Title.ToLower().Contains(title.ToLower()));
                if (entity != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Albums with title like '" + title + "' not found.");
                }
            }
        }
        public HttpResponseMessage Post([FromBody]Album album)
        {
            try
            {
                using (ghostDBEntities entities = new ghostDBEntities())
                {
                    entities.Albums.Add(album);
                    entities.SaveChanges();

                    var message = Request.CreateResponse(HttpStatusCode.Created, album);
                    message.Headers.Location = new Uri(Request.RequestUri + album.idAlbum.ToString());
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
                    var entity = entities.Albums.FirstOrDefault(e => e.idAlbum.Equals(id));
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Album with id '" + id.ToString() + "' not found.");
                    }
                    else
                    {
                        entities.Albums.Remove(entity);
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
        public HttpResponseMessage Put(int id, [FromBody]Album album)
        {
            try
            {
                using (ghostDBEntities entities = new ghostDBEntities())
                {
                    var entity = entities.Albums.FirstOrDefault(e => e.idAlbum.Equals(id));

                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Album with id '" + id.ToString() + "' not found.");
                    }
                    else
                    {
                        entity.idArtist = album.idArtist;
                        entity.Title = album.Title;
                        entity.Tracks = album.Tracks;
                        entity.Year = album.Year;

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
