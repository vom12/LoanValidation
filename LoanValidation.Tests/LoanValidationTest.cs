using LoanValidation.Domain.Models;
using LoanValidation.Services.Config;
using LoanValidation.Services.Interfaces;
using LoanValidation.Services.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace LoanValidation.Tests
{
    [TestFixture]
    public class LoanValidationTest
    {
        private readonly IConfiguration _configuration;

        public LoanValidationTest()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            _configuration = config;
        }

        [Test]
        public async Task TestValidLead()
        {
            // Arrange
            var lead = new Lead
            {
                FirstName = "John",
                LastName = "Doe",
                EmailAddress = "john.doe@example.com",
                PhoneNumber = "0412345678",
                BusinessNumber = "50110219460",
                LoanAmount = 50000,
                CitizenshipStatus = "Citizen",
                TimeTrading = 5,
                CountryCode = "AU",
                Industry = "Industry 1"
            };

            var mockValidationCache = new Mock<ILoanValidationCache>();
            var mockConfig = new Mock<IAppConfig>();
            var mockBusinessNumberService = new Mock<IBusinessNumberService>();

            // Set up mock objects to return expected values
            mockValidationCache.Setup(x => x.TryGetValidationResultAsync(lead))
                .ReturnsAsync((true, new List<ValidationResult>()));

            mockConfig.SetupGet(x => x.MaxLoanAmount).Returns(10000);
            mockConfig.SetupGet(x => x.MinLoanAmount).Returns(1000);
            mockConfig.SetupGet(x => x.AllowedCitizenshipStatuses).Returns("Citizen, Permanent Resident");
            mockConfig.SetupGet(x => x.AllowedCountryCodes).Returns("AU");
            mockConfig.SetupGet(x => x.AllowedIndustries).Returns("Industry 1, Industry 2");
            mockConfig.SetupGet(x => x.BannedIndustries).Returns("Banned Industry 1, Banned Industry 2");
            mockConfig.SetupGet(x => x.MinTimeTrading).Returns(1);
            mockConfig.SetupGet(x => x.MaxTimeTrading).Returns(20);

            mockBusinessNumberService.Setup(x => x.ValidateBusinessNumber(lead.BusinessNumber))
                .ReturnsAsync(true);

            var loanValidationService = new LoanValidationService(
                mockValidationCache.Object,
                mockConfig.Object,
                mockBusinessNumberService.Object
            );

            // Act
            var result = await loanValidationService.ValidateLeadAsync(lead);

            // Assert
            Assert.AreEqual("Qualified", result.Decision);
            Assert.IsNull(result.ValidationResult);
        }

        [Test]
        public async Task TestInvalidLead()
        {
            // Arrange
            var lead = new Lead
            {
                FirstName = "Jane",
                EmailAddress = "jane.doe@example.com",
                BusinessNumber = "1234567801",
                LoanAmount = 500,
                CitizenshipStatus = "Foreigner",
                TimeTrading = 30,
                CountryCode = "US",
                Industry = "Industry 1"
            };

            var mockValidationCache = new Mock<ILoanValidationCache>();
            var mockConfig = new Mock<IAppConfig>();
            var mockBusinessNumberService = new Mock<IBusinessNumberService>();

            // Set up mock objects to return expected values
            mockValidationCache.Setup(x => x.TryGetValidationResultAsync(lead))
                .ReturnsAsync((false, new List<ValidationResult>()));

            mockConfig.SetupGet(x => x.MaxLoanAmount).Returns(10000);
            mockConfig.SetupGet(x => x.MinLoanAmount).Returns(1000);
            mockConfig.SetupGet(x => x.AllowedCitizenshipStatuses).Returns("Citizen, Permanent Resident");
            mockConfig.SetupGet(x => x.AllowedCountryCodes).Returns("AU");
            mockConfig.SetupGet(x => x.AllowedIndustries).Returns("Industry 1, Industry 2");
            mockConfig.SetupGet(x => x.BannedIndustries).Returns("Banned Industry 1, Banned Industry 2");
            mockConfig.SetupGet(x => x.MinTimeTrading).Returns(1);
            mockConfig.SetupGet(x => x.MaxTimeTrading).Returns(20);

            mockBusinessNumberService.Setup(x => x.ValidateBusinessNumber(lead.BusinessNumber))
                .ReturnsAsync(true);

            var loanValidationService = new LoanValidationService(
                mockValidationCache.Object,
                mockConfig.Object,
                mockBusinessNumberService.Object
            );

            // Act
            var result = await loanValidationService.ValidateLeadAsync(lead);

            // Assert
            Assert.AreEqual("Unqualified", result.Decision);
            Assert.NotNull(result.ValidationResult);
            Assert.AreEqual(6, result.ValidationResult.Count);
        }
    }
}
