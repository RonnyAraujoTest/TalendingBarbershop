﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TalendingBarbershop.Data.DTOs;
using TalendingBarbershop.Data.Models;
using TalendingBarbershop.Data.Responses;
using TalendingBarbershop.Data.Security;

namespace TalendingBarbershop.Services.Quotes
{
    public class Quotes : IQuotes
    {
        private IMapper _mapper;
        private RequestResult _requestResult;
        private TblQuotes _quotes;
        private DbTalendigBarbershopContext _dbContext;

        public Quotes(IMapper mapper)
        {
            _mapper = mapper;
            _requestResult = new RequestResult();
            _dbContext = new DbTalendigBarbershopContext();
            _quotes = new TblQuotes();
        }

        public async Task<TblQuotes> GetQuotesAsync(int id) 
        {
            try
            {
               await _dbContext.TblQuotes.FindAsync(id);
            }
            catch (Exception ex)
            {
            }
            return _quotes;
        }

        public async Task<RequestResult> AddQuote(TblQuotesDTO quote)
        {
            try 
            { 
                _quotes = _mapper.Map<TblQuotes>(quote);
                await _dbContext.TblQuotes.AddAsync(_quotes);
                await _dbContext.SaveChangesAsync();

                _requestResult.Message = "Exito";
                _requestResult.Data = "La cita fue agregado con exito.";
                _requestResult.Response = true;

            } catch(Exception ex) 
            {
                _requestResult.Message = ex.Message;
                _requestResult.Data = null;
                _requestResult.Response = false;
            }

            return _requestResult;
        }

        public async Task<RequestResult> EditQuote(TblQuotesDTO quote)
        {
            
            _quotes = _mapper.Map<TblQuotes>(quote);
            _dbContext.Entry(_quotes).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
                _requestResult.Message = "Exito";
                _requestResult.Data = "La cita fue modificada con exito.";
                _requestResult.Response = true;
            }
            catch (Exception ex)
            {
                _requestResult.Message = ex.Message;
                _requestResult.Data = null;
                _requestResult.Response = false;
            }

            return _requestResult;
        }

       public async Task<RequestResult> DeleteQuote(int id)
       {
            try
            {
                _quotes = await GetQuotesAsync(id);
                if (_quotes != null)
                {
                    _dbContext.TblQuotes.Remove(_quotes);

                    _requestResult.Message = "Exito";
                    _requestResult.Data = "La cita fue eliminada con exito.";
                    _requestResult.Response = true;
                }
            } catch(Exception ex)
            {
                _requestResult.Message = ex.Message;
                _requestResult.Data = null;
                _requestResult.Response = false;
            }
            return null;
       }
    }
}
