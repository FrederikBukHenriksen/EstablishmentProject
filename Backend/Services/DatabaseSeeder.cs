﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Data.DataModels;
using WebApplication1.Models;
using WebApplication1.Repositories;
using WebApplication1.Services;
using Xunit.Abstractions;


namespace WebApplication1.Services
{
    public class DatabaseSeeder
    {
        public IEstablishmentRepository establishmentRepository;

        public DatabaseSeeder(IEstablishmentRepository establishmentRepository)
        {
            this.establishmentRepository = establishmentRepository;
        }

        public void LoadData()
        {
            var Establishment1 = TestDataFactory.CreateEstablishment();
            //for (int i = 0; i < 10; i++)
            //{
            //    Establishment1.Items.Add(TestDataFactory.CreateItem());
            //}
            establishmentRepository.Add(Establishment1);

            //context.Set<Establishment>().Add(Establishment1);
            //context.SaveChanges();


        }
    }
}
