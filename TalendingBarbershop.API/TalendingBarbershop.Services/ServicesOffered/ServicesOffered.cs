﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TalendingBarbershop.Data.DTOs;
using TalendingBarbershop.Data.Models;
using TalendingBarbershop.Data.Responses;

namespace TalendingBarbershop.Services.ServicesOffered
{
    public class ServicesOffered : IServicesOffered
    {
        private DbTalendigBarbershopContext _dbContext;
        private IMapper _mapper;
        private TblServices _services;
        public ServicesOffered(IMapper mapper)
        {
            _dbContext = new DbTalendigBarbershopContext();
            _mapper = mapper;
        }

        public async Task<TblServices> Add(TblServicesDTO tblServicesDTO)
        {
            _services = _mapper.Map<TblServices>(tblServicesDTO);
            _dbContext.TblServices.Add(_services);
            await _dbContext.SaveChangesAsync();
            return _services;
        }
        public async Task<TblServices> Edit(int id, TblServicesDTO tblServicesDTO)
        {
            _services = _mapper.Map<TblServices>(tblServicesDTO);
            _services.Id = id;

            _dbContext.Entry(_services).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return null;
            }

            return _services;

        }

        public async Task<List<TblServices>> GetAll()
        {
            return await _dbContext.Set<TblServices>()
               .AsQueryable()
               .ToListAsync();
        }
        public async Task<TblServices> Get(int id)
        {
            return await _dbContext.Set<TblServices>()
               .AsQueryable()
               .FirstAsync(x => x.Id == id);
        }
        public async Task<TblServices> Delete(int id)
        {
            var itemToRemove = await _dbContext.TblServices.SingleOrDefaultAsync(x => x.Id == id);
            if (itemToRemove == null) return null;
            var item = _mapper.Map<TblServices>(itemToRemove);

            _dbContext.TblServices.Remove(itemToRemove);
            await _dbContext.SaveChangesAsync();
            return item;
        }
    }
}
