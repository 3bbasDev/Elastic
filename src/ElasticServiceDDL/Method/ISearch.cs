using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticServiceDDL.Method
{
    public interface ISearch<T> where T : class
    {
        public Task<List<T>> SearchAll(string query);
        public Task Add(T Model);
        public Task<List<T>> Ser(
            string query, 
            int page, 
            int pageSize, 
            T Model, 
            Func<QueryStringQueryDescriptor<T>, IQueryStringQuery> selector = null,
            Func<MatchQueryDescriptor<T>, IMatchQuery> math= null,
            List<Func<QueryContainerDescriptor<T>, QueryContainer>> queries = null);

        public Task<List<T>> Check(
            string query,
            T Model,
            Func<QueryStringQueryDescriptor<T>, IQueryStringQuery> selector = null,
            Func<MatchQueryDescriptor<T>, IMatchQuery> math = null,
            List<Func<QueryContainerDescriptor<T>, QueryContainer>> queries = null);

        public void Clear();

        public Task<List<T>> SerTest(
            string query,
            int page,
            int pageSize,
            T Model,
            Func<QueryStringQueryDescriptor<T>, IQueryStringQuery> selector = null,
            Func<MatchQueryDescriptor<T>, IMatchQuery> math = null,
            List<Func<QueryContainerDescriptor<T>, QueryContainer>> queries = null);

        public Task<List<T>> SerTestTT(
            List<Func<QueryContainerDescriptor<T>, QueryContainer>> queries = null);
    }


}
