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
        private ITaskItemRepository _taskItemRepository;
        private IIdentityLookup _identityLookup;
        private IObjectContext _objectContext;

        private Guid _taskItemAggregateId = Guid.NewGuid();

        private TaskItem _storedTaskItem;

        protected override ICommandHandler<AddTaskItemCommand> Given(IEventStorage storage)
        {
            _taskItemRepository = A.Fake<ITaskItemRepository>();
            _identityLookup = A.Fake<IIdentityLookup>();
            _objectContext = A.Fake<IObjectContext>();

            A.CallTo(() => _identityLookup.GetModelId<User>(_authenticatedUserId)).Returns(17);

            A.CallTo(() => _taskItemRepository.Add(null)).WithAnyArguments().Invokes(
                c =>
                    {
                        _storedTaskItem = c.GetArgument<TaskItem>(0);
                        _storedTaskItem.TaskItemId = 1337;
                    });

            return new AddTaskItemCommandHandler(_taskItemRepository, _objectContext, _identityLookup, storage);
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


        [TestMethod]
        public void Then_TaskItem_Is_Added()
        {
            Assert.IsNotNull(_storedTaskItem);
        }

        [TestMethod]
        public void Then_Title_Of_TaskItem_Is_Correct()
        {
            Assert.AreEqual("Test title", _storedTaskItem.Title);
        }

        [TestMethod]
        public void Then_Details_Of_TaskItem_Is_Correct()
        {
            Assert.AreEqual("Test details", _storedTaskItem.Details);
        }

        [TestMethod]
        public void Then_AssignedToUserId_Is_Null()
        {
            Assert.IsNull(_storedTaskItem.AssignedToUserId);
        }

        [TestMethod]
        public void Then_Changes_Are_Saved_In_ObjectContext()
        {
            A.CallTo(() => _objectContext.SaveChanges()).MustHaveHappened(Repeated.Exactly.Once);
        }

        [TestMethod]
        public void Then_Identity_Mapping_Is_Stored()
        {
            A.CallTo(() => _identityLookup.StoreMapping<TaskItem>(_taskItemAggregateId, 1337)).MustHaveHappened(Repeated.Exactly.Once);
        }

        [TestMethod]
        public void Then_CreatedByUserId_Is_Correct()
        {
            Assert.AreEqual(17, _storedTaskItem.CreatedByUserId);
        }
    }
}