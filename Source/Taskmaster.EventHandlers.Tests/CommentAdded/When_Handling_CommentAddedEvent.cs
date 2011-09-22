using System;
using System.Collections.ObjectModel;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Petite;
using Taskmaster.Domain;
using Taskmaster.Service.EventHandlers;
using Taskmaster.Service.Events;

namespace Taskmaster.EventHandlers.Tests.CommentAdded
{
    [TestClass]
    public class When_Handling_CommentAddedEvent : WithEventHandlerFor<CommentAddedEvent>
    {
        private ITaskItemRepository _taskItemRepository;
        private IIdentityLookup _identityLookup;
        private IObjectContext _objectContext;

        private Guid _taskItemAggregateId = Guid.NewGuid();
        private Guid _commentId = Guid.NewGuid();

        private TaskItem _taskItem;

        protected override IEventHandler<CommentAddedEvent> Given()
        {
            _taskItemRepository = A.Fake<ITaskItemRepository>();
            _identityLookup = A.Fake<IIdentityLookup>();
            _objectContext = A.Fake<IObjectContext>();

            A.CallTo(() => _identityLookup.GetModelId<User>(_createdByUserId)).Returns(17);

            _taskItem = new TaskItem()
            {
                Comments = new Collection<TaskComment>()
                                               {
                                                   new TaskComment() { Comment = "Comment 1", CreatedByUserId = 1},
                                                   new TaskComment() { Comment = "Comment 2", CreatedByUserId = 1},
                                               }
            };

            A.CallTo(() => _taskItemRepository.Get(null)).WithAnyArguments().Returns(_taskItem);

            return new TaskItemModelHandler(_taskItemRepository, _identityLookup, _objectContext);
        }

        protected override CommentAddedEvent When()
        {
            return new CommentAddedEvent(_taskItemAggregateId, _commentId, "My new comment", _createdByUserId);
        }

        [TestMethod]
        public void Then_Comment_Is_Added()
        {
            Assert.AreEqual(3, _taskItem.Comments.Count);
        }

        [TestMethod]
        public void Then_Added_Comment_Is_Correct()
        {
            Assert.AreEqual("My new comment", _taskItem.Comments[2].Comment);
        }

        [TestMethod]
        public void Then_Created_User_In_Comment_Is_Correct()
        {
            Assert.AreEqual(17, _taskItem.Comments[2].CreatedByUserId);
        }

        [TestMethod]
        public void Then_Changes_Are_Saved_In_ObjectContext()
        {
            A.CallTo(() => _objectContext.SaveChanges()).MustHaveHappened(Repeated.Exactly.Once);
        }

        [TestMethod]
        public void Then_Identity_Of_Comment_Is_Mapped()
        {
            A.CallTo(() => _identityLookup.StoreMapping<TaskComment>(_commentId, 4711)).WithAnyArguments().MustHaveHappened(Repeated.Exactly.Once);
        }

    }
}