(window.webpackJsonp=window.webpackJsonp||[]).push([[10],{hhoJ:function(e,t,n){"use strict";n.r(t),n.d(t,"_frontmatter",(function(){return s})),n.d(t,"default",(function(){return p}));var r=n("wx14"),a=n("zLVn"),o=(n("q1tI"),n("7ljp")),i=n("hhGP"),s=(n("qKvR"),{});void 0!==s&&s&&s===Object(s)&&Object.isExtensible(s)&&!s.hasOwnProperty("__filemeta")&&Object.defineProperty(s,"__filemeta",{configurable:!0,value:{name:"_frontmatter",filename:"src/pages/Get_started/Architecture.md"}});var c={_frontmatter:s},l=i.a;function p(e){var t=e.components,n=Object(a.a)(e,["components"]);return Object(o.b)(l,Object(r.a)({},c,n,{components:t,mdxType:"MDXLayout"}),Object(o.b)("h1",{id:"spark-fhir-server"},"Spark FHIR server"),Object(o.b)("p",null,"Spark is built in three layers:"),Object(o.b)("ol",null,Object(o.b)("li",{parentName:"ol"},"Spark Server (Spark.Web for Asp.net core 2.1, or Spark.csproj for ASP.net 4.6): An ASP.Net MVC application hosting both a (minimal) visual interface, the FHIR (REST) endpoint and a Maintenance operation."),Object(o.b)("li",{parentName:"ol"},"Spark Engine (Spark.Engine.csproj): The implementation of everything FHIR: the REST interface, indexing of the search parameters and interpreting search requests, construction of FHIR responses etc."),Object(o.b)("li",{parentName:"ol"},"Spark Mongo (Spark.Mongo.csproj): Storage and retrieval of both resources and the index based on MongoDB.")),Object(o.b)("h1",{id:"spark-is-built-on-the-net-fhir-api"},"Spark is built on the .NET FHIR API"),Object(o.b)("p",null,"Spark uses the .NET FHIR API to parse and serialize resources, and as a source of metadata about the FHIR specification: what Resource types are available, what is the definition of the SearchParameters and so on. The parsing and serialization in this API is heavily optimized. Using Spark you get the benefits of that. It also means that Spark is bound to a specific version of the API, which is currently the DSTU2 version."),Object(o.b)("h1",{id:"spark-engine"},"Spark Engine"),Object(o.b)("p",null,"Spark Engine provides:"),Object(o.b)("ol",null,Object(o.b)("li",{parentName:"ol"},"Interfaces for the various functions that must be implemented by the storage layer:",Object(o.b)("ul",{parentName:"li"},Object(o.b)("li",{parentName:"ul"},"IFhirStore: Add and retrieve resources."),Object(o.b)("li",{parentName:"ul"},"IFhirIndex: Process resources to index entries, search resources using the index."),Object(o.b)("li",{parentName:"ul"},"IIndexStore: Save and delete index entries."),Object(o.b)("li",{parentName:"ul"},"ISnapShotStore: Save and retrieve snapshots of search results for paging."),Object(o.b)("li",{parentName:"ul"},"IHistoryStore: Get previous versions of a resource, a resource type or the whole system."),Object(o.b)("li",{parentName:"ul"},"IGenerator: Generate ID's for new resources and new versions of resources."))),Object(o.b)("li",{parentName:"ol"},"Services for handling generic FHIR functionality",Object(o.b)("ul",{parentName:"li"},Object(o.b)("li",{parentName:"ul"},"SearchService: Combine IFhirIndex and ISnapShotStore to paging results."),Object(o.b)("li",{parentName:"ul"},"ElementIndexer: Translate FHIR DataTypes to parts of an Index entry."),Object(o.b)("li",{parentName:"ul"},"ConformanceService: Provide the conformance statement of Spark."))),Object(o.b)("li",{parentName:"ol"},"ASP.NET Filters for handling cross cutting FHIR concerns:",Object(o.b)("ul",{parentName:"li"},Object(o.b)("li",{parentName:"ul"},"Translating Exceptions to FHIR Response Messages (an http error or OperationOutcome)"),Object(o.b)("li",{parentName:"ul"},"(De)compression of request and response data."))),Object(o.b)("li",{parentName:"ol"},"ASP.NET Formatters for several wire formats:",Object(o.b)("ul",{parentName:"li"},Object(o.b)("li",{parentName:"ul"},"JSON"),Object(o.b)("li",{parentName:"ul"},"XML"),Object(o.b)("li",{parentName:"ul"},"Binary"),Object(o.b)("li",{parentName:"ul"},"HTML"))),Object(o.b)("li",{parentName:"ol"},"All kinds of model and helper classes to assist in the functions above.")),Object(o.b)("p",null,"This all accumulates in (I)FhirService, that presents all the FHIR functions."),Object(o.b)("h1",{id:"spark-mongo"},"Spark Mongo"),Object(o.b)("p",null,"The MongoDB implementation of Spark stores the resources, the index, the snapshots and the generated ID's in MongoDB, with one collection for each. Previous versions of resources are also in the Resources collection. The Index collection contains only the current version of every resource."),Object(o.b)("p",null,"MongoSearcher implements the actual searching mechanism on the MongoDB index, using the generic ResourceVisitor and Criterium classes in Spark Engine."),Object(o.b)("p",null,"Be aware that MongoDB is heavily used (especially on searches), so it should be on an endpoint with very little latency."))}void 0!==p&&p&&p===Object(p)&&Object.isExtensible(p)&&!p.hasOwnProperty("__filemeta")&&Object.defineProperty(p,"__filemeta",{configurable:!0,value:{name:"MDXContent",filename:"src/pages/Get_started/Architecture.md"}}),p.isMDXComponent=!0}}]);
//# sourceMappingURL=component---src-pages-get-started-architecture-md-dcae46dac37bca09cbd6.js.map