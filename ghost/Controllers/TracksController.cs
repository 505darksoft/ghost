using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ghostDataAccess;
using ghostTools;

namespace ghost.Controllers
{
    public class TracksController : ApiController
    {
        public IEnumerable<Track> Get()
        {
            using (ghostDBEntities entities = new ghostDBEntities())
            {
                IEnumerable<TextReplace> textreplace = entities.TextReplaces.ToList();
                IEnumerable<Track> tracklist = entities.Tracks.ToList();
                IEnumerable<Album> albumlist = entities.Albums.ToList();
                IEnumerable<Artist> artistlist = entities.Artists.ToList();
                IEnumerable<Format> formatlist = entities.Formats.ToList();
                string f = formatlist.FirstOrDefault(e => e.Name.Equals("GitHubFile")).Text;
                foreach (Track track in tracklist)
                {
                    Album Album = albumlist.FirstOrDefault(e => e.idAlbum.Equals(track.idAlbum));
                    Artist Artist = artistlist.FirstOrDefault(e => e.idArtist.Equals(Album.idArtist));
                    track.Url = string.Format(f, Tools.ConvertToGitHubFolder(Artist.Name), Tools.ConvertToGitHubFolder(Album.Title), Tools.ConvertToGitHubFile(track.FileName, textreplace));
                }
                return tracklist;
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
                IEnumerable<TextReplace> textreplace = entities.TextReplaces.ToList();
                IEnumerable<Track> tracklist = entities.Tracks.Where(e => e.Title.ToLower().Contains(title.ToLower()));
                IEnumerable<Album> albumlist = entities.Albums.ToList();
                IEnumerable<Artist> artistlist = entities.Artists.ToList();
                IEnumerable<Format> formatlist = entities.Formats.ToList();
                string f = formatlist.FirstOrDefault(e => e.Name.Equals("GitHubFile")).Text;
                foreach (Track track in tracklist)
                {
                    Album Album = albumlist.FirstOrDefault(e => e.idAlbum.Equals(track.idAlbum));
                    Artist Artist = artistlist.FirstOrDefault(e => e.idArtist.Equals(Album.idArtist));
                    track.Url = string.Format(f, Tools.ConvertToGitHubFolder(Artist.Name), Tools.ConvertToGitHubFolder(Album.Title), Tools.ConvertToGitHubFile(track.FileName, textreplace));
                }
                List<Track> l = tracklist.ToList<Track>();
                //var entity = entities.Tracks.Where(e => e.Title.ToLower().Contains(title.ToLower()));
                if (tracklist != null || tracklist.Count() == 0)
                {
                    return Request.CreateResponse<List<Track>>(HttpStatusCode.OK, l);
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
