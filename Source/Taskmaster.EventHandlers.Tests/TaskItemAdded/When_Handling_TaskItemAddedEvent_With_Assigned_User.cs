using System;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Petite;
using Taskmaster.Domain;
using Taskmaster.Service.EventHandlers;
using Taskmaster.Service.Events;

namespace Taskmaster.EventHandlers.Tests.TaskItemAdded
{
    [TestClass]
    public class When_Handling_TaskItemAddedEvent_With_Assigned_User : WithEventHandlerFor<TaskItemAddedEvent>
    {
        private IIdentityLookup _identityLookup;
        private ITaskItemRepository _taskItemRepository;
        private IObjectContext _objectContext;

        private Guid _taskItemAggregateId = Guid.NewGuid();
        private Guid _assignedUserAggregateId = Guid.NewGuid();

        private TaskItem _storedTaskItem;

        protected override IEventHandler<TaskItemAddedEvent> Given()
        {
            _taskItemRepository = A.Fake<ITaskItemRepository>();
            _identityLookup = A.Fake<IIdentityLookup>();
            _objectContext = A.Fake<IObjectContext>();

            A.CallTo(() => _identityLookup.GetModelId<User>(_assignedUserAggregateId)).Returns(42);
            A.CallTo(() => _identityLookup.GetModelId<User>(_createdByUserId)).Returns(17);

            A.CallTo(() => _taskItemRepository.Add(null)).WithAnyArguments().Invokes(
                c =>
                {
                    _storedTaskItem = c.GetArgument<TaskItem>(0);
                    _storedTaskItem.TaskItemId = 1337;
                });

            return new TaskItemModelHandler(_taskItemRepository, _identityLookup, _objectContext);
        }

        protected override TaskItemAddedEvent When()
        {
            return new TaskItemAddedEvent(_taskItemAggregateId, "Test title", "Test details", _assignedUserAggregateId, _createdByUserId);
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
        public void Then_AssignedToUserId_Of_TaskItem_Is_Correct()
        {
            Assert.AreEqual(42, _storedTaskItem.AssignedToUserId.Value);
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