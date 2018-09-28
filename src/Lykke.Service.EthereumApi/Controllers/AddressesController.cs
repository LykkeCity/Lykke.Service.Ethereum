﻿using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Service.BlockchainApi.Contract.Addresses;
using Lykke.Service.Ethereum.Domain.Services;
using Lykke.Service.Ethereum.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lykke.Service.Ethereum.Controllers
{
    [PublicAPI, Route("/api/addresses")]
    public class AddressesController : Controller
    {
        private readonly IAddressService _addressService;

        public AddressesController(
            IAddressService addressService)
        {
            _addressService = addressService;
        }


        [HttpGet("{address}/validity")]
        public async Task<ActionResult<AddressValidationResponse>> GetAddressValidity(
            string address)
        {
            return new AddressValidationResponse
            {
                IsValid = await _addressService.ValidateAsync(address)
            };
        }

        #region Not Implemented Endpoints
        
        [HttpGet("{address}/balance")]
        public ActionResult<AddressValidationResponse> GetBalance(
            AddressRequest address)
                => StatusCode(StatusCodes.Status501NotImplemented);
        
        [HttpGet("{address}/explorer-url")]
        public ActionResult<AddressValidationResponse> GetExplorerUrl(
            AddressRequest address)
                => StatusCode(StatusCodes.Status501NotImplemented);
        
        [HttpGet("{address}/underlying")]
        public ActionResult<AddressValidationResponse> GetUnderlyingAddress(
            AddressRequest address)
                => StatusCode(StatusCodes.Status501NotImplemented);
        
        [HttpGet("{address}/virtual")]
        public ActionResult<AddressValidationResponse> GetVirtualAddress(
            AddressRequest address)
                => StatusCode(StatusCodes.Status501NotImplemented);
        
        #endregion
    }
}
