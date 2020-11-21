using ElasticServiceDDL.Method;
using ElasticServiceDDL.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElasticApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class DynamicController : ControllerBase
    {
        private readonly ISearch<dynamic> _search;
        private readonly IHelperFunc _helper;
        public DynamicController(ISearch<dynamic> search, IHelperFunc helper)
        {
            _search = search;
            _helper = helper;
        }

        [HttpPost]
        public async Task<IActionResult> Get([FromForm] IFormFile File)
        {
            try
            {
                _search.Clear();

                List<string[]> Names = new List<string[]>();
                using (var reader = new System.IO.StreamReader(File.OpenReadStream()))
                {
                    while (!reader.EndOfStream)
                    {

                        Names.Add(reader.ReadLine().Split(','));
                    }

                }

                #region Get Keys and score
                Dictionary<string, double> keys = new Dictionary<string, double>();

                for (int i = 0; i < Names.FirstOrDefault().Count(); i++)
                {
                    keys.Add(Names.FirstOrDefault().ElementAtOrDefault(i), Convert.ToDouble(Names.Skip(1).FirstOrDefault().ElementAtOrDefault(i)));
                }
                #endregion


                #region Add To Elastic
                int divarr = Names.Count() % 2 == 0 ? Names.Skip(2).Count() / 2 : (Names.Skip(2).Count() / 2) + 1;
                Task s = Task.Run(() =>
                {
                    for (int i = 2; i < divarr ; i++)
                    {
                         _search.Add(_helper.ConvertToDynamic(Names.FirstOrDefault(), Names.ElementAtOrDefault(i)));
                    }

                });
                Task s1 = Task.Run(() =>
                {
                    for (int j = divarr; j < Names.Count(); j++)
                    {
                        _search.Add(_helper.ConvertToDynamic(Names.FirstOrDefault(), Names.ElementAtOrDefault(j)));
                    }
                });

                Task.WaitAll(new[] { s, s1 });
                #endregion


                #region Search

                List<Func<QueryContainerDescriptor<dynamic>, QueryContainer>> Filter = new List<Func<QueryContainerDescriptor<dynamic>, QueryContainer>>();
                ElasticServiceDDL.Model.Result UserSimilar = new ElasticServiceDDL.Model.Result() { Header = Names.FirstOrDefault() };
                List<ModelInfo> modelInfos = new List<ModelInfo>();

                int j = 0;
                for (int i = 2; i < Names.Count(); i++)
                {
                    for(int countFillter = 0; countFillter < Names.FirstOrDefault().Count(); countFillter++)
                        Filter.Add(new Func<QueryContainerDescriptor<dynamic>, QueryContainer>(
                           sh => sh
                               .Match(c => c
                               .Field(keys.ElementAtOrDefault(j).Key)
                               .Query(Names.ElementAtOrDefault(i).ElementAtOrDefault(j++).ToString())
                               .MinimumShouldMatch(MinimumShouldMatch.Percentage(keys.ElementAtOrDefault(j).Value))
                               )
                           ));
                    j = 0;
                    var Similar = _search.SerTestTT(Filter).Result;

                    if (Similar.Count > 1)
                    {
                        modelInfos.Add(new ModelInfo { Model = _helper.ConvertToDynamic(Names.FirstOrDefault(), Names.ElementAtOrDefault(i), Similar) });
                    }

                    Filter.Clear();
                }

                #endregion

                UserSimilar.Model = modelInfos;

                return await Task.FromResult(Ok(UserSimilar));

                

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /*[HttpPost("uploadall")]
        public async Task<IActionResult> Get([FromForm] IFormFile File)
        {

            try
            {
                _search.Clear();

                if (!string.Equals(File.ContentType.ToLower(), "text/csv", StringComparison.OrdinalIgnoreCase))
                    return BadRequest("type of file !");


                List<string[]> Names = new List<string[]>();
                using (var reader = new System.IO.StreamReader(File.OpenReadStream()))
                {
                    while (!reader.EndOfStream)
                    {

                        Names.Add(reader.ReadLine().Split(','));
                    }

                }



                #region Add To Elastic
                int divarr = Names.Count() % 2 == 0 ? Names.Count() / 2 : (Names.Count() / 2) + 1;
                Task s = Task.Run(() =>
                {
                    for (int i = 0; i < divarr; i++)
                    {

                        _search.Add(ConvertToDynamic(Names.FirstOrDefault(), Names.ElementAtOrDefault(i)));
                    }

                });
                Task s1 = Task.Run(() =>
                {
                    for (int j = divarr; j < Names.Count(); j++)
                    {

                        _search.Add(ConvertToDynamic(Names.FirstOrDefault(), Names.ElementAtOrDefault(j)));
                    }
                });

                Task.WaitAll(new[] { s, s1 });
                #endregion

                #region Get Keys and score
                Dictionary<string, double> keys = new Dictionary<string, double>();
                for (int i = 0; i < Names.FirstOrDefault().Count(); i++)
                {
                    keys.Add(Names.FirstOrDefault().ElementAtOrDefault(i), Convert.ToDouble(Names.Skip(1).FirstOrDefault().ElementAtOrDefault(i)));
                }
                #endregion

                #region Search
                List<Func<QueryContainerDescriptor<dynamic>, QueryContainer>> Filter = new List<Func<QueryContainerDescriptor<dynamic>, QueryContainer>>();
                ElasticServiceDDL.Model.Result UserSimilar = new ElasticServiceDDL.Model.Result() { Header = Names.FirstOrDefault() };
                List<ModelInfo> modelInfos = new List<ModelInfo>();

                int j = 0;
                for (int i = 2; i < Names.Count(); i++)
                {

                    foreach (var Query in Names.ElementAtOrDefault(i))
                        Filter.Add(new Func<QueryContainerDescriptor<dynamic>, QueryContainer>(
                           sh => sh
                               .Match(c => c
                               .Field(keys.ElementAtOrDefault(j).Key)
                               .Query(Names.ElementAtOrDefault(i).ElementAtOrDefault(j++).ToString())
                               .MinimumShouldMatch(MinimumShouldMatch.Percentage(keys.ElementAtOrDefault(j).Value))
                               )
                           ));
                    j = 0;
                    var Similar = _search.SerTestTT(Filter).Result;

                    if (Similar.Count > 1)
                    {
                        dynamic item = new ExpandoObject();
                        var dItem = item as IDictionary<string, object>;
                        for (int buc = 0; buc < Names.FirstOrDefault().Length; buc++)
                            dItem.Add(Names.FirstOrDefault()[buc], Names.ElementAtOrDefault(i).ElementAtOrDefault(buc).ToString());
                        dItem.Add("parent", Similar);
                        modelInfos.Add(new ModelInfo { Model = item });
                    }

                    Filter.Clear();
                }
                #endregion

                UserSimilar.Model = modelInfos;

                return await Task.FromResult(Ok(UserSimilar));

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }*/

    }

}
