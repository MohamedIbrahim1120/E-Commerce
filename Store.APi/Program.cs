
using Domain.Contracts;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Data;
using Services;
using Services.Abstractions;
using Shared.ErrorsModels;
using Store.APi.Extensions;
using Store.APi.MiddlesWares;
using System.Threading.Tasks;

namespace Store.APi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.RegisterAllServices(builder.Configuration);

            var app = builder.Build();


            // Configure the HTTP request pipeline.

           await app.ConfigureMiddleWares();

            app.Run();
        }
    }
}