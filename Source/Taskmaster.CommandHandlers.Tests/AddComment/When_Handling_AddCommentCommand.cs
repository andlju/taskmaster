using System;
using System.Collections.ObjectModel;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Petite;
using Taskmaster.Domain;
using Taskmaster.Service.CommandHandlers;
using Taskmaster.Service.Commands;
using Taskmaster.Service.Events;
using Taskmaster.Service.Infrastructure;

namespace Taskmaster.CommandHandlers.Tests.AddComment
{
    
    [TestClass]
    public class When_Handling_AddCommentCommand : WithCommandHandlerFor<AddCommentCommand>
    {
        private Guid _taskItemAggregateId = Guid.NewGuid();
        private Guid _commentId = Guid.NewGuid();

        protected override ICommandHandler<AddCommentCommand> Given(IEventStorage storage)
        {
            return new AddCommentCommandHandler(storage);
        }

        protected override AddCommentCommand When()
        {
            return new AddCommentCommand(_authenticatedUserId, _taskItemAggregateId, _commentId, "My new comment");
        }

        [TestMethod]
        public void Then_Event_Is_Published()
        {
            Assert.AreEqual(1, Events.Length);
        }

        [TestMethod]
        public void Then_Correct_Event_Is_Published()
        {
            Assert.IsInstanceOfType(Events[0], typeof(CommentAddedEvent));
        }

        [TestMethod]
        public void Then_Comment_In_Event_Is_Correct()
        {
            Assert.AreEqual("My new comment", Event<CommentAddedEvent>(0).Comment);
        }

        [TestMethod]
        public void Then_CreatedByUser_In_Event_Is_Correct()
        {
            Assert.AreEqual(_authenticatedUserId, Event<CommentAddedEvent>(0).CreatedByUserId);
        }

        [TestMethod]
        public void Then_TaskItemId_In_Event_Is_Correct()
        {
            Assert.AreEqual(_taskItemAggregateId, Event<CommentAddedEvent>(0).TaskItemAggregateId);
        }
    }
}