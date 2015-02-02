﻿/* 
 * Copyright (c) 2014, Furore (info@furore.com) and contributors
 * See the file CONTRIBUTORS for details.
 * 
 * This file is licensed under the BSD 3-Clause license
 * available at https://raw.github.com/furore-fhir/spark/master/LICENSE
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Hl7.Fhir.Model;
using Spark.Service;
using System.Web.Http.Description;
using System.Net.Http;
using Spark.Config;
using Spark.Support;
using System.Diagnostics;
using Hl7.Fhir.Rest;
using System.Web.Http;
using Spark.Core;
using Spark.Http;
using System.Web.Http.Cors;
using System.Net.Http.Headers;
using System.IO;

namespace Spark.Controllers
{
    [RoutePrefix("fhir")]
    [EnableCors("*","*","*","*")]
    public class FhirController : ApiController
    {
        FhirService service; 
        public FhirController()
        {
            service = DependencyCoupler.Inject<FhirService>();
        }

        // ============= Instance Level Interactions
       
        [HttpGet, Route("{type}/{id}")] 
        public HttpResponseMessage Read(string type, string id)
        {
            Key key = new Key(type, id);
            Entry entry = service.Read(key);
            return Request.ResourceResponse(entry);
        }
        
        [HttpGet, Route("{type}/{id}/_history/{vid}")]
        public HttpResponseMessage VRead(string type, string id, string vid)
        {
            Key key = new Key(type, id, vid);
            Entry entry = service.VRead(key);
            return Request.ResourceResponse(entry);
        }

        [HttpPut, Route("{type}/{id}")]
        public HttpResponseMessage Upsert(string type, string id, Resource resource)
        {
            Key key = new Key(type, id);

            // todo: DSTU2
            //entry.Tags = Request.GetFhirTags(); // todo: move to model binder?

            if (service.Exists(key))
            {
                Entry entry = service.Update(resource, key);
                return Request.StatusResponse(entry, HttpStatusCode.OK);
            }
            else
            {
                Entry entry = service.Create(resource, key);
                return Request.StatusResponse(entry, HttpStatusCode.Created);
            }
        }

        [HttpPost, Route("{type}")]
        public HttpResponseMessage Create(string type, Resource resource)
        {
            //entry.Tags = Request.GetFhirTags(); // todo: move to model binder?
            Key key = service.NextKey(type);
            Entry entry = service.Create(resource, key);
            return Request.StatusResponse(entry, HttpStatusCode.Created);
        }
        
        [Route("{type}/{id}")]
        public HttpResponseMessage Delete(string type, string id)
        {
            Key key = new Key(type, id);
            service.Delete(key);
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }

        [HttpGet, Route("{type}/{id}/_history")]
        public Bundle History(string type, string id)
        {
            Key key = new Key(type, id);
            DateTimeOffset? since = Request.GetDateParameter(FhirParameter.SINCE);
            string sortby = Request.GetParameter(FhirParameter.SORT);
            return service.History(key, since, sortby);
        }


        // ============= Validate
        [HttpPost, Route("{type}/_validate/{id}")]
        public HttpResponseMessage Validate(string type, string id, Resource entry)
        {
            //entry.Tags = Request.GetFhirTags();
            Key key = new Key(type, id);
            Entry outcome = service.Validate(entry, key);

            if (outcome == null)
                return Request.CreateResponse(HttpStatusCode.OK);
            else
                return Request.ResourceResponse(outcome, (HttpStatusCode)422);
        }

        [HttpPost, Route("{type}/_validate")]
        public HttpResponseMessage Validate(string type, Resource resource)
        {
            // todo: DSTU2
            //entry.Tags = Request.GetFhirTags();
            Entry outcome = service.Validate(resource);

            if (outcome == null)
                return Request.CreateResponse(HttpStatusCode.Created);
            else
                return Request.ResourceResponse(outcome, (HttpStatusCode)422);
        }
        
        // ============= Type Level Interactions

        /*
        // According to the spec, this interaction should not exist, so I commented it.
        [HttpPost, Route("{type}/{id}")]
        public HttpResponseMessage Create(string type, string id, ResourceEntry entry)
        {
            entry.Tags = Request.GetFhirTags(); // todo: move to model binder?

            ResourceEntry newentry = service.Create(type, entry, id);
            return Request.StatusResponse(newentry, HttpStatusCode.Created);
        }
        */

        [HttpGet, Route("{type}")]
        public Bundle Search(string type)
        {
            var parameters = Request.TupledParameters();
            int pagesize = Request.GetIntParameter(FhirParameter.COUNT) ?? Const.DEFAULT_PAGE_SIZE;
            bool summary = Request.GetBooleanParameter(FhirParameter.SUMMARY) ?? false;
            string sortby = Request.GetParameter(FhirParameter.SORT);
            // On implementing _summary: this has to be done at two different abstraction layers:
            // a) The serialization (which is the formatter in WebApi2 needs to call the serializer with a _summary param
            // b) The service needs to generate self/paging links which retain the _summary parameter
            // This is all still todo ;-)
            return service.Search(type, parameters, pagesize, sortby);
        }

        [HttpGet, Route("{type}/_search")]
        public Bundle SearchWithOperator(string type)
        {
            return Search(type);
        }

        [HttpGet, Route("{type}/_history")]
        public Bundle History(string type)
        {
            DateTimeOffset? since = Request.GetDateParameter(FhirParameter.SINCE);
            string sortby = Request.GetParameter(FhirParameter.SORT);
            return service.History(type, since, sortby);
        }


        // ============= Whole System Interactions

        [HttpGet, Route("metadata")]
        public Resource Metadata()
        {
            return service.Conformance();
        }

        [HttpOptions, Route("")]
        public Entry Options()
        {
            return new Entry(service.Conformance());
        }

        [HttpPost, Route("")]
        public Bundle Transaction(Bundle bundle)
        {
            return service.Transaction(bundle);
        }

        [HttpPost, Route("Mailbox")]
        public Bundle Mailbox(Bundle document)
        {
            Binary b = Request.GetBody();
            return service.Mailbox(document, b);
        }
        
        [HttpGet, Route("_history")]
        public Bundle History()
        {
            DateTimeOffset? since = Request.GetDateParameter(FhirParameter.SINCE);
            string sortby = Request.GetParameter(FhirParameter.SORT);
            return service.History(since, sortby);
        }

        [HttpGet, Route("_snapshot")]
        public Bundle Snapshot()
        {
            string snapshot = Request.GetParameter(FhirParameter.SNAPSHOT_ID);
            int start = Request.GetIntParameter(FhirParameter.SNAPSHOT_INDEX) ?? 0; 
            int count = Request.GetIntParameter(FhirParameter.COUNT) ?? Const.DEFAULT_PAGE_SIZE;
            return service.GetSnapshot(snapshot, start, count);
        }


        // ============= Tag Interactions

        /*
        [HttpGet, Route("_tags")]
        public TagList AllTags()
        {
            return service.TagsFromServer();
        }

        [HttpGet, Route("{type}/_tags")]
        public TagList ResourceTags(string type)
        {
            return service.TagsFromResource(type);
        }

        [HttpGet, Route("{type}/{id}/_tags")]
        public TagList InstanceTags(string type, string id)
        {
            return service.TagsFromInstance(type, id);
        }

        [HttpGet, Route("{type}/{id}/_history/{vid}/_tags")]
        public HttpResponseMessage HistoryTags(string type, string id, string vid)
        {
            TagList tags = service.TagsFromHistory(type, id, vid);
            return Request.CreateResponse(HttpStatusCode.OK, tags);
        }

        [HttpPost, Route("{type}/{id}/_tags")]
        public HttpResponseMessage AffixTag(string type, string id, TagList taglist)
        {
            service.AffixTags(type, id, taglist != null ? taglist.Category : null);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpPost, Route("{type}/{id}/_history/{vid}/_tags")]
        public HttpResponseMessage AffixTag(string type, string id, string vid, TagList taglist)
        {
            service.AffixTags(type, id, vid, taglist != null ? taglist.Category : null);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpPost, Route("{type}/{id}/_tags/_delete")]
        public HttpResponseMessage DeleteTags(string type, string id, TagList taglist)
        {
            service.RemoveTags(type, id, taglist != null ? taglist.Category : null);
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }

        [HttpPost, Route("{type}/{id}/_history/{vid}/_tags/_delete")]
        public HttpResponseMessage DeleteTags(string type, string id, string vid, TagList taglist)
        {
            service.RemoveTags(type, id, vid, taglist != null ? taglist.Category : null);
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }
        */

       
    }

  
}
