using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ghostDataAccess;

namespace ghost.Controllers
{
    public class TracksController : ApiController
    {
        public IEnumerable<Track> Get()
        {
            using (ghostDBEntities entities = new ghostDBEntities())
            {
                return entities.Tracks.ToList();
            }
        }
        public HttpResponseMessage Get(int id)
        {
            using (ghostDBEntities entities = new ghostDBEntities())
            {
                var entity = entities.Tracks.FirstOrDefault(e => e.idTrack.Equals(id));
                if (entity != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Track with id '" + id.ToString() + "' not found.");
                }
            }
        }
        public HttpResponseMessage Get(string title)
        {
            using (ghostDBEntities entities = new ghostDBEntities())
            {
                var entity = entities.Tracks.FirstOrDefault(e => e.Title.ToLower().Contains(title.ToLower()));
                if (entity != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Tracks with title like '" + title + "' not found.");
                }
            }
        }
        public HttpResponseMessage Post([FromBody]Track track)
        {
            try
            {
                using (ghostDBEntities entities = new ghostDBEntities())
                {
                    entities.Tracks.Add(track);
                    entities.SaveChanges();

                    var message = Request.CreateResponse(HttpStatusCode.Created, track);
                    message.Headers.Location = new Uri(Request.RequestUri + track.idTrack.ToString());
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
                    var entity = entities.Tracks.FirstOrDefault(e => e.idTrack.Equals(id));
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Track with id '" + id.ToString() + "' not found.");
                    }
                    else
                    {
                        entities.Tracks.Remove(entity);
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
        public HttpResponseMessage Put(int id, [FromBody]Track track)
        {
            try
            {
                using (ghostDBEntities entities = new ghostDBEntities())
                {
                    var entity = entities.Tracks.FirstOrDefault(e => e.idTrack.Equals(id));

                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Track with id '" + id.ToString() + "' not found.");
                    }
                    else
                    {
                        entity.idAlbum = track.idAlbum;
                        entity.Title = track.Title;
                        entity.TrackNumber = track.TrackNumber;
                        entity.FileName = track.FileName;
                        entity.FileUrlName = track.FileUrlName;

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
