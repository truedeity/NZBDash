﻿#region Copyright
// /************************************************************************
//   Copyright (c) 2016 NZBDash
//   File: AlertSettingsControllerTests.cs
//   Created By: Jamie Rees
//  
//   Permission is hereby granted, free of charge, to any person obtaining
//   a copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//  
//   The above copyright notice and this permission notice shall be
//   included in all copies or substantial portions of the Software.
//  
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
//   EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
//   NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
//   LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
//   OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
//   WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// ************************************************************************/
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using Moq;

using NUnit.Framework;

using NZBDash.Common.Interfaces;
using NZBDash.Core.Interfaces;
using NZBDash.Core.Models.Settings;
using NZBDash.UI.Controllers;
using NZBDash.UI.Models.ViewModels.Settings;

using Ploeh.AutoFixture;

namespace NZBDash.UI.Test.Controllers
{
    [TestFixture]
    public class AlertSettingsControllerTests
    {
        private Mock<ILogger> Logger { get; set; }
        private AlertSettingsController _controller { get; set; }
        private Mock<ISettingsService<AlertSettingsDto>> Settings { get; set; }
        private AlertSettingsDto ExpectedDto { get; set; }

        [SetUp]
        public void Setup()
        {
            Logger = new Mock<ILogger>();
            Settings = new Mock<ISettingsService<AlertSettingsDto>>();

            ExpectedDto = new Fixture().Create<AlertSettingsDto>();
            Settings = new Mock<ISettingsService<AlertSettingsDto>>();

            Settings.Setup(x => x.GetSettings()).Returns(ExpectedDto);
            Settings.Setup(x => x.SaveSettings(It.IsAny<AlertSettingsDto>())).Returns(true).Verifiable();

            _controller = new AlertSettingsController(Logger.Object, Settings.Object);
        }

        [Test]
        public void GetAlertSettingsView()
        {
            var result = (ViewResult)_controller.Index();
            var model = (AlertSettingsViewModel)result.Model;

            Assert.That(model.AlertRules.Count, Is.EqualTo(ExpectedDto.AlertRules.Count));
            Assert.That(model.AlertRules[0].ThresholdTime, Is.EqualTo(ExpectedDto.AlertRules[0].ThresholdTime));
            Assert.That(model.AlertRules[0].AlertType.ToString(), Is.EqualTo(ExpectedDto.AlertRules[0].AlertType.ToString()));
            Assert.That(model.AlertRules[0].Enabled, Is.EqualTo(ExpectedDto.AlertRules[0].Enabled));
            Assert.That(model.AlertRules[0].Id, Is.EqualTo(ExpectedDto.AlertRules[0].Id));
            Assert.That(model.AlertRules[0].NicId, Is.EqualTo(ExpectedDto.AlertRules[0].NicId));
            Assert.That(model.AlertRules[0].Percentage, Is.EqualTo(ExpectedDto.AlertRules[0].Percentage));
        }

        [Test]
        public void GetNullAlertSettingsView()
        {
            Settings.Setup(x => x.GetSettings()).Returns(new AlertSettingsDto());
            Settings.Setup(x => x.SaveSettings(It.IsAny<AlertSettingsDto>())).Returns(true).Verifiable();

            _controller = new AlertSettingsController(Logger.Object, Settings.Object);

            var result = (ViewResult)_controller.Index();
            var model = (AlertSettingsViewModel)result.Model;

            Assert.That(model.AlertRules.Count, Is.EqualTo(0));
        }

        [Test]
        public void UpdateCpuAlertGet()
        {
            ExpectedDto.AlertRules[0].AlertType = AlertTypeDto.Cpu;
            Settings.Setup(x => x.GetSettings()).Returns(ExpectedDto);
            Settings.Setup(x => x.SaveSettings(It.IsAny<AlertSettingsDto>())).Returns(true).Verifiable();

            _controller = new AlertSettingsController(Logger.Object, Settings.Object);

            var result = (PartialViewResult)_controller.UpdateAlert(ExpectedDto.AlertRules[0].Id);
            var model = (AlertRules)result.Model;

            var exp = ExpectedDto.AlertRules[0];
            Assert.That(model.Id, Is.EqualTo(exp.Id));
            Assert.That(model.AlertType.ToString(), Is.EqualTo(exp.AlertType.ToString()));
            Assert.That(model.Enabled, Is.EqualTo(exp.Enabled));
            Assert.That(model.Percentage, Is.EqualTo(exp.Percentage));
            Assert.That(model.ThresholdTime, Is.EqualTo(exp.ThresholdTime));
        }

        [Test]
        public void UpdateExistingAlertPost()
        {
            ExpectedDto.AlertRules[0].AlertType = AlertTypeDto.Cpu;
            Settings.Setup(x => x.GetSettings()).Returns(ExpectedDto);
            Settings.Setup(x => x.SaveSettings(It.IsAny<AlertSettingsDto>())).Returns(true).Verifiable();

            _controller = new AlertSettingsController(Logger.Object, Settings.Object);

            var model = new AlertRules { Id = ExpectedDto.AlertRules[0].Id, ThresholdTime = 90, Percentage = 99999, Enabled = true, AlertType = AlertType.Cpu, NicId = 22 };

            _controller.UpdateAlert(model);
            Settings.Verify(x => x.SaveSettings(It.Is<AlertSettingsDto>(c => c.AlertRules.Any(y => y.Percentage == 99999))),Times.Once);
            Settings.Verify(x => x.SaveSettings(It.Is<AlertSettingsDto>(c => c.AlertRules.Any(y => y.ThresholdTime == 90))),Times.Once);
            Settings.Verify(x => x.SaveSettings(It.Is<AlertSettingsDto>(c => c.AlertRules.Any(y => y.NicId == 22))),Times.Once);
            Settings.Verify(x => x.SaveSettings(It.Is<AlertSettingsDto>(c => c.AlertRules.Any(y => y.Enabled))),Times.Once);
        }

        [Test]
        public void UpdateNewAlertPost()
        {
            ExpectedDto.AlertRules[0].AlertType = AlertTypeDto.Cpu;
            Settings.Setup(x => x.GetSettings()).Returns(ExpectedDto);
            Settings.Setup(x => x.SaveSettings(It.IsAny<AlertSettingsDto>())).Returns(true).Verifiable();

            _controller = new AlertSettingsController(Logger.Object, Settings.Object);

            var model = new AlertRules { Id = int.MaxValue, ThresholdTime = 90, Percentage = 92229, Enabled = true, AlertType = AlertType.Cpu, NicId = 22 };

            _controller.UpdateAlert(model);
            Settings.Verify(x => x.SaveSettings(It.Is<AlertSettingsDto>(c => c.AlertRules.Any(y => y.Percentage == 92229))), Times.Once);
            Settings.Verify(x => x.SaveSettings(It.Is<AlertSettingsDto>(c => c.AlertRules.Any(y => y.ThresholdTime == 90))), Times.Once);
            Settings.Verify(x => x.SaveSettings(It.Is<AlertSettingsDto>(c => c.AlertRules.Any(y => y.NicId == 22))), Times.Once);
            Settings.Verify(x => x.SaveSettings(It.Is<AlertSettingsDto>(c => c.AlertRules.Any(y => y.Enabled))), Times.Once);
        }

        [Test]
        public void UpdateAlertPostInvalidModel()
        {
            ExpectedDto.AlertRules[0].AlertType = AlertTypeDto.Cpu;
            Settings.Setup(x => x.GetSettings()).Returns(ExpectedDto);
            Settings.Setup(x => x.SaveSettings(It.IsAny<AlertSettingsDto>())).Returns(true).Verifiable();

            _controller = new AlertSettingsController(Logger.Object, Settings.Object);

            var model = new AlertRules { Id = int.MaxValue, Enabled = true, AlertType = AlertType.Cpu, };

            var jsonResult = (JsonResult)_controller.UpdateAlert(model);
            var error = (Dictionary<string, string>)jsonResult.Data;

            Assert.That(string.IsNullOrEmpty(error[AlertRules.PercentageErrorKey]), Is.False);            
            Assert.That(string.IsNullOrEmpty(error[AlertRules.ThresholdErrorKey]), Is.False);            
            Settings.Verify(x => x.SaveSettings(It.IsAny<AlertSettingsDto>()), Times.Never);
            Settings.Verify(x => x.SaveSettings(It.IsAny<AlertSettingsDto>()), Times.Never);
            Settings.Verify(x => x.SaveSettings(It.IsAny<AlertSettingsDto>()), Times.Never);
            Settings.Verify(x => x.SaveSettings(It.IsAny<AlertSettingsDto>()), Times.Never);
        }
    }
}
