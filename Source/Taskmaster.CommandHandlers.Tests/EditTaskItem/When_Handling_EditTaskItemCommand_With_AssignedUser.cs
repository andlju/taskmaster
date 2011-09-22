using System;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Petite;
using Taskmaster.Domain;
using Taskmaster.Service.CommandHandlers;
using Taskmaster.Service.Commands;
using Taskmaster.Service.Events;
using Taskmaster.Service.Infrastructure;

namespace Taskmaster.CommandHandlers.Tests.EditTaskItem
{
    [TestClass]
    public class When_Handling_EditTaskItemCommand_With_AssignedUser : WithCommandHandlerFor<EditTaskItemCommand>
    {
        private Guid _taskItemAggregateId = Guid.NewGuid();
        private Guid _assignedUserAggregateId = Guid.NewGuid();

        protected override ICommandHandler<EditTaskItemCommand> Given(IEventStorage storage)
        {
            return new EditTaskItemCommandHandler(storage);
        }

        protected override EditTaskItemCommand When()
        {
            return new EditTaskItemCommand(_authenticatedUserId, _taskItemAggregateId, "Changed title", "Changed details", _assignedUserAggregateId);
        }

        [TestMethod]
        public void Then_Event_Is_Published()
        {
            Assert.AreEqual(1, Events.Length);
        }

        [TestMethod]
        public void Then_Correct_Event_Is_Published()
        {
            Assert.IsInstanceOfType(Events[0], typeof(TaskItemEditedEvent));
        }

        [TestMethod]
        public void Then_Title_In_Event_Is_Correct()
        {
            Assert.AreEqual("Changed title", Event<TaskItemEditedEvent>(0).Title);
        }

        [TestMethod]
        public void Then_Details_In_Event_Is_Correct()
        {
            Assert.AreEqual("Changed details", Event<TaskItemEditedEvent>(0).Details);
        }

        [TestMethod]
        public void Then_AssignedUser_In_Event_Is_Correct()
        {
            Assert.AreEqual(_assignedUserAggregateId, Event<TaskItemEditedEvent>(0).AssignedUserAggregateId);
        }

        [TestMethod]
        public void Then_TaskItemId_In_Event_Is_Correct()
        {
            Assert.AreEqual(_taskItemAggregateId, Event<TaskItemEditedEvent>(0).TaskItemAggregateId);
        }

    }
}