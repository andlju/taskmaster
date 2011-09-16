using System;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Petite;
using Taskmaster.Domain;
using Taskmaster.Service.CommandHandlers;
using Taskmaster.Service.Commands;

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

        protected override ICommandHandler<AddUserCommand> Given()
        {
            _userRepository = A.Fake<IUserRepository>();
            _objectContext = A.Fake<IObjectContext>();
            _identityLookup = A.Fake<IIdentityLookup>();

            A.CallTo(() => _userRepository.Add(null)).WithAnyArguments().Invokes(c =>
                                                                                     {
                                                                                         _user = c.GetArgument<User>(0);
                                                                                         _user.UserId = 17;
                                                                                     });

            return new AddUserCommandHandler(_userRepository, _objectContext, _identityLookup);
        }

        protected override AddUserCommand When()
        {
            return new AddUserCommand(_userAggregateId, "Test user");
        }

        [TestMethod]
        public void Then_User_Is_Added()
        {
            Assert.IsNotNull(_user);
        }

        [TestMethod]
        public void Then_User_Name_Is_Correct()
        {
            Assert.AreEqual("Test user", _user.Name);
        }

        [TestMethod]
        public void Then_Changes_Are_Saved_In_ObjectContext()
        {
            A.CallTo(() => _objectContext.SaveChanges()).MustHaveHappened(Repeated.Exactly.Once);
        }

        [TestMethod]
        public void Then_Identity_Is_Mapped()
        {
            A.CallTo(() => _identityLookup.StoreMapping<User>(_userAggregateId, 17)).MustHaveHappened(Repeated.Exactly.Once);
        }
    }
}