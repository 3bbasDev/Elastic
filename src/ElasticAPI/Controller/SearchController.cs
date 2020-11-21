using ElasticApi.Models;
using ElasticServiceDDL.Method;
using ElasticServiceDDL.Model;
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
    public class SearchController : ControllerBase
    {
        private readonly ISearch<User> _search;
        public SearchController(ISearch<User> search)
        {
            _search = search;
        }
        [HttpGet("get/{query}")]
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
        public async Task<IActionResult> Get1([FromBody] RequstSearch<User, User> requstSearch)
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
                            .Fuzziness(Fuzziness.Ratio(75)))
                            ,
                        sh => sh
                            .Match(c => c
                            .Field(p => p.Email)
                            .Query(requstSearch.Model.Email)
                            .Fuzziness(Fuzziness.Ratio(100))
                            )
                            ,
                        sh => sh
                            .Match(c => c
                            .Field(p => p.Phone1)
                            .Query(requstSearch.Model.Phone1)
                            .Fuzziness(Fuzziness.Ratio(100)))
                            ,
                        sh => sh
                            .Match(c => c
                            .Field(p => p.Phone2)
                            .Query(requstSearch.Model.Phone2)
                            .Fuzziness(Fuzziness.Ratio(100)))
                    }

                    ).Result.ToList()));

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RequstSearch<User,User> requstSearch)
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
                            .Fuzziness(Fuzziness.Ratio(75)))
                            ,
                        sh => sh
                            .Match(c => c
                            .Field(p => p.Email)
                            .Query(requstSearch.Model.Email)
                            .Fuzziness(Fuzziness.Ratio(100))
                            )
                            ,
                        sh => sh
                            .Match(c => c
                            .Field(p => p.Phone1)
                            .Query(requstSearch.Model.Phone1)
                            .Fuzziness(Fuzziness.Ratio(100)))
                            ,
                        sh => sh
                            .Match(c => c
                            .Field(p => p.Phone2)
                            .Query(requstSearch.Model.Phone2)
                            .Fuzziness(Fuzziness.Ratio(100)))
                    }

                    );
                if (Check.Any())
                    return BadRequest(Check.ToList());

                await _search.Add( requstSearch.Model );
                return Ok(requstSearch.Model);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("add")]
        public async Task<IActionResult> AddNew()
        {
            try
            {
                _search.Clear() ;
                var Users = System.IO.File.ReadAllLines(@"C:\Users\abbas\Documents\GitHub\Elastic\src\ElasticAPI\Sample.csv")
                                                       .Skip(1)
                                                       .Select(v => Models.User.FromCsv(v))
                                                       .ToList();
                foreach (var User in Users)
                    await _search.Add(User);
                return await Task.FromResult(Ok(Users.Take(10).ToList()));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpGet("test")]
        public async Task<IActionResult> GetTest([FromBody] RequstSearch<User, User> requstSearch)
        {
            try
            {


                var Similar = await _search.SerTest(requstSearch.Query, requstSearch.Page, requstSearch.PageSize, requstSearch.Model, null, null,
                    new List<Func<QueryContainerDescriptor<User>, QueryContainer>>()
                    {
                        sh => sh
                            .Match(c => c
                            .Field(p => p.CustomerName)
                            .Query(requstSearch.Model.CustomerName)
                            .MinimumShouldMatch(MinimumShouldMatch.Percentage(70))
                            )
                            ,
                        sh => sh
                            .Match(c => c
                            .Field(p => p.Email)
                            .Query(requstSearch.Model.Email)
                            .MinimumShouldMatch(MinimumShouldMatch.Percentage(90))
                            )
                            ,
                        sh => sh
                            .Match(c => c
                            .Field(p => p.Phone1)
                            .Query(requstSearch.Model.Phone1)
                            .MinimumShouldMatch(MinimumShouldMatch.Percentage(100))
                            )
                            ,
                        sh => sh
                            .Match(c => c
                            .Field(p => p.Phone2)
                            .Query(requstSearch.Model.Phone2)
                            .MinimumShouldMatch(MinimumShouldMatch.Percentage(100))
                            )
                    }

                    );
                List<Models.User> UserSimilar = new List<User>(); 
                if(Similar.Count>0)
                {
                    Console.WriteLine("1");
                    requstSearch.Model.Parent=Similar;
                    UserSimilar.Add(requstSearch.Model);
                }
                //else
                //{
                //    await _search.Add(requstSearch.Model);
                //}

                return await Task.FromResult(Ok(requstSearch.Model));

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}