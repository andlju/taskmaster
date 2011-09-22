using System;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Petite;
using Taskmaster.Domain;
using Taskmaster.Service.CommandHandlers;
using Taskmaster.Service.Commands;
using Taskmaster.Service.Events;
using Taskmaster.Service.Infrastructure;

namespace Taskmaster.CommandHandlers.Tests.AddUser
{
    [TestClass]
    public class When_Handling_AddUserCommand : WithCommandHandlerFor<AddUserCommand>
    {
        private IUserRepository _userRepository;
        private IObjectContext _objectContext;
        private IIdentityLookup _identityLookup;

        private User _user;

        private Guid _userAggregateId = Guid.NewGuid();

        protected override ICommandHandler<AddUserCommand> Given(IEventStorage storage)
        {
            _userRepository = A.Fake<IUserRepository>();
            _objectContext = A.Fake<IObjectContext>();
            _identityLookup = A.Fake<IIdentityLookup>();

            A.CallTo(() => _identityLookup.GetModelId<User>(_authenticatedUserId)).Returns(1);

            A.CallTo(() => _userRepository.Add(null)).WithAnyArguments().Invokes(c =>
                                                                                     {
                                                                                         _user = c.GetArgument<User>(0);
                                                                                         _user.UserId = 17;
                                                                                     });

            return new AddUserCommandHandler(_userRepository, storage);
        }

        protected override AddUserCommand When()
        {
            return new AddUserCommand(_authenticatedUserId, _userAggregateId, "Test user");
        }

        [TestMethod]
        public void Then_Event_Is_Published()
        {
            Assert.AreEqual(1, Events.Length);
        }

        [TestMethod]
        public void Then_Correct_Event_Is_Published()
        {
            Assert.IsInstanceOfType(Events[0],  typeof(UserAddedEvent));
        }

        [TestMethod]
        public void Then_Name_In_Event_Is_Correct()
        {
            Assert.AreEqual("Test user", Event<UserAddedEvent>(0).Name);
        }

        [TestMethod]
        public void Then_UserId_In_Event_Is_Correct()
        {
            Assert.AreEqual(_userAggregateId, Event<UserAddedEvent>(0).UserAggregateId);
        }
    }
}