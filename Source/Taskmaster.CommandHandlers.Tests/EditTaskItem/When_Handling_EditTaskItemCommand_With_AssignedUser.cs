using System;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Petite;
using Taskmaster.Domain;
using Taskmaster.Service.CommandHandlers;
using Taskmaster.Service.Commands;

namespace Taskmaster.CommandHandlers.Tests.EditTaskItem
{
    [TestClass]
    public class When_Handling_EditTaskItemCommand_With_AssignedUser : WithCommandHandlerFor<EditTaskItemCommand>
    {
        private ITaskItemRepository _taskItemRepository;
        private IIdentityLookup _identityLookup;
        private IObjectContext _objectContext;

        private Guid _taskItemAggregateId = Guid.NewGuid();
        private Guid _assignedUserAggregateId = Guid.NewGuid();

        private TaskItem _taskItem;

        protected override ICommandHandler<EditTaskItemCommand> Given()
        {
            _taskItemRepository = A.Fake<ITaskItemRepository>();
            _identityLookup = A.Fake<IIdentityLookup>();
            _objectContext = A.Fake<IObjectContext>();

            A.CallTo(() => _identityLookup.GetModelId<User>(_assignedUserAggregateId)).Returns(42);
            A.CallTo(() => _identityLookup.GetModelId<TaskItem>(_taskItemAggregateId)).Returns(1337);
            A.CallTo(() => _identityLookup.GetModelId<User>(_authenticatedUserId)).Returns(19);

            _taskItem = new TaskItem()
                            {
                                TaskItemId = 1337,
                                Title = "Test title",
                                Details = "Test details",
                                CreatedByUserId = 17,
                                AssignedToUserId = 1
                            };

            A.CallTo(() => _identityLookup.GetModelId<User>(_assignedUserAggregateId)).Returns(2);

            A.CallTo(() => _taskItemRepository.Get(null)).WithAnyArguments().Returns(_taskItem);

            return new EditTaskItemCommandHandler(_taskItemRepository, _objectContext, _identityLookup);
        }

        protected override EditTaskItemCommand When()
        {
            return new EditTaskItemCommand(_authenticatedUserId, _taskItemAggregateId, "Changed title", "Changed details", _assignedUserAggregateId);
        }

        [TestMethod]
        public void Then_Title_Of_TaskItem_Is_Changed()
        {
            Assert.AreEqual("Changed title", _taskItem.Title);
        }

        [TestMethod]
        public void Then_Details_Of_TaskItem_Is_Changed()
        {
            Assert.AreEqual("Changed details", _taskItem.Details);
        }

        [TestMethod]
        public void Then_AssignedUser_Of_TaskItem_Is_Changed()
        {
            Assert.AreEqual(2, _taskItem.AssignedToUserId);
        }

        [TestMethod]
        public void Then_Changes_Are_Saved_In_ObjectContext()
        {
            A.CallTo(() => _objectContext.SaveChanges()).MustHaveHappened(Repeated.Exactly.Once);
        }

        [TestMethod]
        public void Then_CreatedByUserId_Is_Not_Changed()
        {
            Assert.AreEqual(17, _taskItem.CreatedByUserId);
        }
    }
}