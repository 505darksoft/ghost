using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ghostDataAccess;

namespace ghost.Controllers
{
    public class TextReplaceController : ApiController
    {
        public IEnumerable<TextReplace> Get()
        {
            using (ghostDBEntities entities = new ghostDBEntities())
            {
                return entities.TextReplaces.ToList();
            }
        }
        public HttpResponseMessage Get(int id)
        {
            using (ghostDBEntities entities = new ghostDBEntities())
            {
                var entity = entities.TextReplaces.FirstOrDefault(e => e.idTextReplace.Equals(id));
                if (entity != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "TextReplace with id '" + id.ToString() + "' not found.");
                }
            }
        }
        public HttpResponseMessage Post([FromBody]TextReplace textreplace)
        {
            try
            {
                using (ghostDBEntities entities = new ghostDBEntities())
                {
                    entities.TextReplaces.Add(textreplace);
                    entities.SaveChanges();

                    var message = Request.CreateResponse(HttpStatusCode.Created, textreplace);
                    message.Headers.Location = new Uri(Request.RequestUri + textreplace.idTextReplace.ToString());
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
                    var entity = entities.TextReplaces.FirstOrDefault(e => e.idTextReplace.Equals(id));
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "TextReplace with id '" + id.ToString() + "' not found.");
                    }
                    else
                    {
                        entities.TextReplaces.Remove(entity);
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
        public HttpResponseMessage Put(int id, [FromBody]TextReplace textreplace)
        {
            try
            {
                using (ghostDBEntities entities = new ghostDBEntities())
                {
                    var entity = entities.TextReplaces.FirstOrDefault(e => e.idTextReplace.Equals(id));

                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Artist with id '" + id.ToString() + "' not found.");
                    }
                    else
                    {
                        entity.OldText = textreplace.OldText;
                        entity.NewText = textreplace.NewText;
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
