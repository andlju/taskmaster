using System;
using System.Collections.ObjectModel;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Petite;
using Taskmaster.Domain;
using Taskmaster.Service.CommandHandlers;
using Taskmaster.Service.Commands;

namespace Taskmaster.CommandHandlers.Tests.AddComment
{
    
    [TestClass]
    public class When_Handling_AddCommentCommand : WithCommandHandlerFor<AddCommentCommand>
    {
        private ITaskItemRepository _taskItemRepository;
        private IIdentityLookup _identityLookup;
        private IObjectContext _objectContext;

        private Guid _taskItemAggregateId = Guid.NewGuid();
        private Guid _commentId = Guid.NewGuid();

        private TaskItem _taskItem;

        protected override ICommandHandler<AddCommentCommand> Given()
        {
            _taskItemRepository = A.Fake<ITaskItemRepository>();
            _identityLookup = A.Fake<IIdentityLookup>();
            _objectContext = A.Fake<IObjectContext>();

            _taskItem = new TaskItem()
                            {
                                Comments = new Collection<TaskComment>()
                                               {
                                                   new TaskComment() { Comment = "Comment 1", CreatedByUserId = 1},
                                                   new TaskComment() { Comment = "Comment 2", CreatedByUserId = 1},
                                               }
                            };

            A.CallTo(() => _taskItemRepository.Get(null)).WithAnyArguments().Returns(_taskItem);

            return new AddCommentCommandHandler(_taskItemRepository, _objectContext, _identityLookup);
        }

        protected override AddCommentCommand When()
        {
            return new AddCommentCommand(_taskItemAggregateId, _commentId, "My new comment");
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