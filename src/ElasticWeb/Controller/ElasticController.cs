using ElasticServiceDDL.Method;
using ElasticServiceDDL.Model;
using ElasticWeb.Model;
using Microsoft.AspNetCore.Mvc;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticWeb.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ElasticController : ControllerBase
    {
        private readonly ISearch<User> _search;
        public ElasticController(ISearch<User> search)
        {
            _search = search;
        }
        [HttpGet("{query}")]
        public async Task<IActionResult> Get(string query)
        {
            try
            {
                return Ok(await _search.SearchAll(query));

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet()]
        public async Task<IActionResult> Get1([FromBody] RequstSearch<User, ScoreFillter> requstSearch)
        {
            try
            {
                return await Task.FromResult(Ok(_search.Ser(requstSearch.Query, requstSearch.Page, requstSearch.PageSize, requstSearch.Model, null, null,
                    new List<Func<QueryContainerDescriptor<User>, QueryContainer>>()
                    {
                        sh => sh
                            .Match(c => c
                            .Field(p => p.CustomerName)
                            .Query(requstSearch.Model.CustomerName)
                            .Fuzziness(Fuzziness.Ratio(requstSearch.ScoreFillter.CustomerName)))
                            ,
                        sh => sh
                            .Match(c => c
                            .Field(p => p.Email)
                            .Query(requstSearch.Model.Email)
                            .Fuzziness(Fuzziness.Ratio(requstSearch.ScoreFillter.Email))
                            )
                            ,
                        sh => sh
                            .Match(c => c
                            .Field(p => p.Phone1)
                            .Query(requstSearch.Model.Phone1)
                            .Fuzziness(Fuzziness.Ratio(requstSearch.ScoreFillter.Phone1)))
                            ,
                        sh => sh
                            .Match(c => c
                            .Field(p => p.Phone2)
                            .Query(requstSearch.Model.Phone2)
                            .Fuzziness(Fuzziness.Ratio(requstSearch.ScoreFillter.Phone2)))
                    }

                    ).Result.ToList()));

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RequstSearch<User, ScoreFillter> requstSearch)
        {
            try
            {
                var Check = await _search.Check(requstSearch.Query, requstSearch.Model, null, null,
                    new List<Func<QueryContainerDescriptor<User>, QueryContainer>>()
                    {
                        sh => sh
                            .Match(c => c
                            .Field(p => p.CustomerName)
                            .Query(requstSearch.Model.CustomerName)
                            .Fuzziness(Fuzziness.Ratio(requstSearch.ScoreFillter.CustomerName)))
                            ,
                        sh => sh
                            .Match(c => c
                            .Field(p => p.Email)
                            .Query(requstSearch.Model.Email)
                            .Fuzziness(Fuzziness.Ratio(requstSearch.ScoreFillter.Email))
                            )
                            ,
                        sh => sh
                            .Match(c => c
                            .Field(p => p.Phone1)
                            .Query(requstSearch.Model.Phone1)
                            .Fuzziness(Fuzziness.Ratio(requstSearch.ScoreFillter.Phone1)))
                            ,
                        sh => sh
                            .Match(c => c
                            .Field(p => p.Phone2)
                            .Query(requstSearch.Model.Phone2)
                            .Fuzziness(Fuzziness.Ratio(requstSearch.ScoreFillter.Phone2)))
                    }

                    );
                if (Check.Any())
                    return BadRequest(Check.ToList());

                await _search.Add(requstSearch.Model);
                return Ok(requstSearch.Model);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost(Name = "Clear")]
        public async Task<IActionResult> ClearData()
        {
            try
            {
                await _search.Clear();
                return await Task.FromResult(Ok());
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
