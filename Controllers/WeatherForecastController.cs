﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MiniProfilerNhibernate5.Infra;
using MiniProfilerNhibernate5.Models;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Mapping;
using StackExchange.Profiling;

namespace MiniProfilerNhibernate5.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var query = _unitOfWork.GetSession().CreateSQLQuery("SELECT COUNT(*) FROM TodoCollection");
            
            
            
            // _unitOfWork.GetSession().= new StackExchange.Profiling.Data.ProfiledDbConnection(_unitOfWork.GetSession().Connection, MiniProfiler.Current);

            var count = query.UniqueResult<int>();

            var todos = CreateObjects(500).ToArray();
            
            _unitOfWork.SaveMany(todos);
            
            _unitOfWork.Flush();

            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        private static List<Todo> CreateObjects(int count)
        {
            var todos = new List<Todo>();
            for (var i = 0; i < count; i++)
            {
                todos.Add(new Todo()
                {
                    Id = Guid.NewGuid(),
                    Name = new Random().Next().ToString(),
                    Complete = true
                });  
            }

            return todos;
        }
    }
}
