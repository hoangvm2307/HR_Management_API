using API.Controllers;
using API.Entities;
using API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControllerNunit.Tests
{
    [TestFixture]
    public class ControllerTest
    {
        private Mock<SwpProjectContext> _contextMock;
        private Mock<IMapper> _mapperMock;
        private Mock<TheCalendarService> _theCalendarServiceMock;
        private TheCalendarController _controller;

        [SetUp]
        public void Setup()
        {
            _contextMock = new Mock<SwpProjectContext>();
            _mapperMock = new Mock<IMapper>();
            _theCalendarServiceMock = new Mock<TheCalendarService>();
            _controller = new TheCalendarController(_contextMock.Object, _mapperMock.Object, _theCalendarServiceMock.Object);
        }

        [Test]
        public async Task GetHolidaysDateByStartDateAndEndDate_ValidDates_ReturnsOkResult()
        {
            // Arrange
            DateTime startDate = DateTime.UtcNow;
            DateTime endDate = DateTime.UtcNow.AddDays(7);
            var calendar = new List<TheCalendar>(); // Add sample data here

            _theCalendarServiceMock.Setup(service => service.GetTypeDates(startDate, endDate))
                .ReturnsAsync(calendar);

            // Act
            var result = await _controller.GetHolidaysDateByStartDateAndEndDate(startDate, endDate);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
        }

        [Test]
        public async Task GetHolidaysDateByStartDateAndEndDate_InvalidDates_ReturnsNotFoundResult()
        {
            // Arrange
            DateTime startDate = DateTime.UtcNow;
            DateTime endDate = DateTime.UtcNow.AddDays(7);
            List<TheCalendar> calendar = null; // Set calendar to null to simulate not found

            _theCalendarServiceMock.Setup(service => service.GetTypeDates(startDate, endDate))
                .ReturnsAsync(calendar);

            // Act
            var result = await _controller.GetHolidaysDateByStartDateAndEndDate(startDate, endDate);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result.Result);
        }

        [Test]
        public async Task GetHolidayDays_ValidDates_ReturnsListOfHolidays()
        {
            // Arrange
            DateTime start = DateTime.UtcNow;
            DateTime end = DateTime.UtcNow.AddDays(7);
            var holidays = new List<TheCalendar>(); // Add sample data here

            _theCalendarServiceMock.Setup(service => service.GetHolidayDays(start, end))
                .ReturnsAsync(holidays);

            // Act
            //var result = await _controller.GetHolidayDays(start, end);

            // Assert
            //Assert.AreEqual(holidays, result.Value);
        }
    }
}
