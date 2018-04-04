using MediatR;
using System;
using AlpineSkiHouse.Web.Models;

namespace AlpineSkiHouse.Web.Queries
{
    public class ResolvePass : IRequest<Pass>
    {
        public int CardId { get; set; }

        public int LocationId { get; set; }

        public DateTime DateTime { get; set; }
    }


}
