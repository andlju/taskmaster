using System;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Petite;
using Taskmaster.Domain;
using Taskmaster.Service.CommandHandlers;
using Taskmaster.Service.Commands;
using Taskmaster.Service.Events;
using Taskmaster.Service.Infrastructure;

namespace Taskmaster.CommandHandlers.Tests.AddTaskItem
{
    [TestClass]
    public class When_Handling_AddTaskItemCommand_With_No_Assigned_User : WithCommandHandlerFor<AddTaskItemCommand>
    {
        private Guid _taskItemAggregateId = Guid.NewGuid();

        protected override ICommandHandler<AddTaskItemCommand> Given(IEventStorage storage)
        {

            return new AddTaskItemCommandHandler(storage);
        }

        protected override AddTaskItemCommand When()
        {
            return new AddTaskItemCommand(_authenticatedUserId, _taskItemAggregateId, "Test title", "Test details", null);
        }

        [TestMethod]
        public void Then_Event_Is_Published()
        {
            Assert.AreEqual(1, Events.Length);
        }

        [TestMethod]
        public void Then_Correct_Event_Is_Published()
        {
            Assert.IsInstanceOfType(Events[0], typeof(TaskItemAddedEvent));
        }

        [TestMethod]
        public void Then_Title_In_Event_Is_Correct()
        {
            Assert.AreEqual("Test title", Event<TaskItemAddedEvent>(0).Title);
        }

        [TestMethod]
        public void Then_Details_In_Event_Is_Correct()
        {
            Assert.AreEqual("Test details", Event<TaskItemAddedEvent>(0).Details);
        }

        [TestMethod]
        public void Then_AssignedUserId_In_Event_Is_Correct()
        {
            Assert.AreEqual(null, Event<TaskItemAddedEvent>(0).AssignedUserAggregateId);
        }

        [TestMethod]
        public void Then_TaskItemId_In_Event_Is_Correct()
        {
            Assert.AreEqual(_taskItemAggregateId, Event<TaskItemAddedEvent>(0).TaskItemAggregateId);
        }

    }
}