using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ghostDataAccess;

namespace ghost.Controllers
{
    public class TextReplaceWebController : ApiController
    {
        public IEnumerable<TextReplaceWeb> Get()
        {
            using (ghostDBEntities entities = new ghostDBEntities())
            {
                return entities.TextReplaceWebs.ToList();
            }
        }
        public HttpResponseMessage Get(int id)
        {
            using (ghostDBEntities entities = new ghostDBEntities())
            {
                var entity = entities.TextReplaceWebs.FirstOrDefault(e => e.idTextReplaceWeb.Equals(id));
                if (entity != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "TextReplaceWeb with id '" + id.ToString() + "' not found.");
                }
            }
        }
        public HttpResponseMessage Post([FromBody]TextReplaceWeb textreplaceweb)
        {
            try
            {
                using (ghostDBEntities entities = new ghostDBEntities())
                {
                    entities.TextReplaceWebs.Add(textreplaceweb);
                    entities.SaveChanges();

                    var message = Request.CreateResponse(HttpStatusCode.Created, textreplaceweb);
                    message.Headers.Location = new Uri(Request.RequestUri + textreplaceweb.idTextReplaceWeb.ToString());
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
                    var entity = entities.TextReplaceWebs.FirstOrDefault(e => e.idTextReplaceWeb.Equals(id));
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "TextReplaceWeb with id '" + id.ToString() + "' not found.");
                    }
                    else
                    {
                        entities.TextReplaceWebs.Remove(entity);
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
        public HttpResponseMessage Put(int id, [FromBody]TextReplaceWeb textreplaceweb)
        {
            try
            {
                using (ghostDBEntities entities = new ghostDBEntities())
                {
                    var entity = entities.TextReplaceWebs.FirstOrDefault(e => e.idTextReplaceWeb.Equals(id));

                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "TextReplaceWeb with id '" + id.ToString() + "' not found.");
                    }
                    else
                    {
                        entity.OldText = textreplaceweb.OldText;
                        entity.NewText = textreplaceweb.NewText;
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
