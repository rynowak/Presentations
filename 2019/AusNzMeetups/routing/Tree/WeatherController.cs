using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Tree
{
    [Route("Weather")]
    public class WeatherController
    {
        //[HttpGet]
        [Route("")]
        public void GetAll()
        {
        }

        //[HttpGet]
        [Route("{id}")]
        public void Get()
        {
        }

        //[HttpGet]
        [Route("{id}/tendayforecast")]
        public void GetTenDay()
        {
        }

        //[HttpPost]
        //[Consumes("application/json")]
        [Route("{id}")]
        public void Update()
        {
        }

        //[HttpPost]
        //[Consumes("application/xml")]
        //[Route("{id}")]
        //public void UpdateXml()
        //{
        //}
    }
}
