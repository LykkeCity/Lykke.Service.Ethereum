using System;
using FluentAssertions;
using Lykke.Service.Ethereum.Core.Blockchain.DTOs;
using Lykke.Service.Ethereum.Core.Blockchain.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Lykke.Service.Ethereum.Core.Blockchain.Extensions
{
    [TestClass]
    public class RpcResponseExtensionsTests
    {
        [TestMethod]
        public void EnsureSuccessResult()
        {
            var response = new RpcResponse
            (
                error: new RpcError(0, "", ""),
                id: "",
                result: null
            );

            Exception expectedException = null;
            
            try
            {
                response.EnsureSuccessfulResult();
            }
            catch (Exception e)
            {
                expectedException = e;
            }

            expectedException
                .Should()
                .BeOfType<RpcResponseException>();
        }

        [TestMethod]
        public void ResultValue()
        {
            var expectedValue = $"{Guid.NewGuid()}";
            
            var actualResponse = new RpcResponse
            (
                error: null,
                id: "",
                result: expectedValue
            );

            actualResponse.ResultValue<string>()
                .Should()
                .Be(expectedValue);
        }
        
        [TestMethod]
        public void ResultValue__Value_Key_Passed()
        {
            var expectedValue = $"{Guid.NewGuid()}";
            
            var actualResponse = new RpcResponse
            (
                error: new RpcError(0, "", null),
                id: "",
                result: expectedValue
            );

            actualResponse.ResultValue<string>("key")
                .Should()
                .Be(expectedValue);
        }
    }
}
