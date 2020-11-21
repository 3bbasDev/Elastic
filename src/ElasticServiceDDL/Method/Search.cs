using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticServiceDDL.Method
{
    internal class Search<T> : ISearch<T> where T : class
    {
        private readonly ElasticClient client;
        private string IndexName = "";
        [Obsolete]
        public Search(ElasticClientProvider elastic)
        {
            client = elastic.Client;
            IndexName = elastic.Index;
            //var withMaxRecursionDescriptor = new CreateIndexDescriptor(elastic.Index)
            //         .Mappings(ms => ms
            //             .Map<T>(m => m.AutoMap(3))
            //         );
        }

        public async Task Add(T Model)
        {
            var Res =await client.IndexDocumentAsync(Model);
            //Console.WriteLine(System.Text.ASCIIEncoding.ASCII.GetString(Res.ApiCall.ResponseBodyInBytes));
        }

        public async Task<List<T>> Check(string query, T Model, Func<QueryStringQueryDescriptor<T>, IQueryStringQuery> selector = null, Func<MatchQueryDescriptor<T>, IMatchQuery> math = null, List<Func<QueryContainerDescriptor<T>, QueryContainer>> queries = null)
        {
            var response1 =
                selector != null ?
                await this.client.SearchAsync<T>(searchDescriptor => searchDescriptor
                    .Query(queryContainerDescriptor => queryContainerDescriptor
                        .Bool(queryDescriptor => queryDescriptor
                            .Must(queryStringQuery => queryStringQuery
                                .QueryString(selector))))
                                        )
                :
                math != null ?
                await this.client.SearchAsync<T>(searchDescriptor => searchDescriptor
                    .Query(queryContainerDescriptor => queryContainerDescriptor
                        .Bool(queryDescriptor => queryDescriptor
                            .Must(queryStringQuery => queryStringQuery
                            .Match(math)
                                )
                            )
                        ))
                :
                queries != null ?
                await this.client.SearchAsync<T>(searchDescriptor => searchDescriptor
                    .Query(queryContainerDescriptor => queryContainerDescriptor
                        .Bool(queryDescriptor => queryDescriptor
                            .Should(queries
                                )
                            )
                        ))
                :
                await this.client.SearchAsync<T>(searchDescriptor => searchDescriptor
                    .Query(queryContainerDescriptor => queryContainerDescriptor
                        .Bool(queryDescriptor => queryDescriptor
                            .Must(queryStringQuery => queryStringQuery
                            .MultiMatch(m => m
                            .Fields(f => f
                            )
                            .Query(query))))))

                ;

            return response1?.Documents.ToList();
        }

        public async Task<List<T>> SearchAll(string query)
        {
            var searchResult1 = client.Search<T>(s => s
                    .Query(q => q
                        .MultiMatch(m => m
                            .Fields(f => f
                            )
                            .Query(query)
                        )
                    ));
            return await Task.FromResult(searchResult1?.Documents.ToList<T>());
        }
        public async Task<List<T>> Ser(
            string query, 
            int page, 
            int pageSize,
            T Model, 
            Func<QueryStringQueryDescriptor<T>, IQueryStringQuery> selector = null,
            Func<MatchQueryDescriptor<T>, IMatchQuery> math=null,
            List<Func<QueryContainerDescriptor<T>, QueryContainer>> queries =null)
        {
            var response = await this.client.SearchAsync<T>(searchDescriptor => searchDescriptor
                    .Query(queryContainerDescriptor => queryContainerDescriptor
                        .Bool(queryDescriptor => queryDescriptor
                            .Must(queryStringQuery => queryStringQuery
                                .QueryString(queryString => queryString
                                    //.Query("customerName:Abbas")
                                    //.Query("phone1:009647711087733")
                                    //.Query("phone2:213546987985")
                                    //.Query("email:abbas@gmail.com")
                                    .Query(query)
                                    .Fuzziness(Fuzziness.Ratio(75))

                                    ))))
                                        .From((page - 1) * pageSize)
                                        .Size(pageSize));
            var response1 =
                selector!=null?
                await this.client.SearchAsync<T>(searchDescriptor => searchDescriptor
                    .Query(queryContainerDescriptor => queryContainerDescriptor
                        .Bool(queryDescriptor => queryDescriptor
                            .Must(queryStringQuery => queryStringQuery
                                .QueryString(selector))))
                                        .From((page - 1) * pageSize)
                                        .Size(pageSize))
                :
                math!=null?
                await this.client.SearchAsync<T>(searchDescriptor => searchDescriptor
                    .Query(queryContainerDescriptor => queryContainerDescriptor
                        .Bool(queryDescriptor => queryDescriptor
                            .Must(queryStringQuery => queryStringQuery
                            .Match(math)
                                )
                            )
                        ))
                :
                queries!=null?
                await this.client.SearchAsync<T>(searchDescriptor => searchDescriptor
                    .Query(queryContainerDescriptor => queryContainerDescriptor
                        .Bool(queryDescriptor => queryDescriptor
                            .Should(queries
                                )
                            )
                        ))
                :
                await this.client.SearchAsync<T>(searchDescriptor => searchDescriptor
                    .Query(queryContainerDescriptor => queryContainerDescriptor
                        .Bool(queryDescriptor => queryDescriptor
                            .Must(queryStringQuery => queryStringQuery
                            .MultiMatch(m => m
                            .Fields(f => f
                            )
                            .Query(query))))))

                ;
            
            return response1?.Documents.ToList();
        }
    
    
        public void Clear()
        {
            var Clear =  client.DeleteByQueryAsync<T>(del => del
                .Query(q => q.QueryString(qs => qs.Query("*")))).Result;
            //Console.WriteLine("Clear1");
            ////client.Indices.RefreshAsync();
            ////Console.WriteLine("Clear");
        }


        public async Task<List<T>> SerTest(
            string query,
            int page,
            int pageSize,
            T Model,
            Func<QueryStringQueryDescriptor<T>, IQueryStringQuery> selector = null,
            Func<MatchQueryDescriptor<T>, IMatchQuery> math = null,
            List<Func<QueryContainerDescriptor<T>, QueryContainer>> queries = null)
        {
            var response1 =
                selector != null ?
                await this.client.SearchAsync<T>(searchDescriptor => searchDescriptor
                    .Query(queryContainerDescriptor => queryContainerDescriptor
                        .Bool(queryDescriptor => queryDescriptor
                            .Must(queryStringQuery => queryStringQuery
                                .QueryString(selector))))
                                        .From((page - 1) * pageSize)
                                        .Size(pageSize))
                :
                math != null ?
                await this.client.SearchAsync<T>(searchDescriptor => searchDescriptor
                    .Query(queryContainerDescriptor => queryContainerDescriptor
                        .Bool(queryDescriptor => queryDescriptor
                            .Must(queryStringQuery => queryStringQuery
                            .Match(math)
                                )
                            )
                        ))
                :
                queries != null ?
                await this.client.SearchAsync<T>(searchDescriptor => searchDescriptor
                    .Query(queryContainerDescriptor => queryContainerDescriptor
                        .Bool(queryDescriptor => queryDescriptor
                            .Should(queries
                                )
                            )
                        ))
                :
                await this.client.SearchAsync<T>(searchDescriptor => searchDescriptor
                    .Query(queryContainerDescriptor => queryContainerDescriptor
                        .Bool(queryDescriptor => queryDescriptor
                            .Must(queryStringQuery => queryStringQuery
                            .MultiMatch(m => m
                            .Fields(f => f
                            )
                            .Query(query))))))

                ;

            return response1?.Documents.ToList();
        }

        public async Task<List<T>> SerTestTT(List<Func<QueryContainerDescriptor<T>, QueryContainer>> queries = null)
        {

            var response1 = await
                 this.client.SearchAsync<T>(searchDescriptor => searchDescriptor
                    .Query(queryContainerDescriptor => queryContainerDescriptor
                        .Bool(queryDescriptor => queryDescriptor
                            .Should(queries
                                )
                            )
                        ));
            return response1?.Documents.ToList();
        }
    }

}
