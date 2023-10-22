using WebApplication1.Repositories;

namespace WebApplication1.Services
{
    public interface IUserContextService
    {
        public List<Guid>? EstablishmentsIds { get; set; }

        public Guid? UserId { get; set; }
    }

    public class UserContextService : IUserContextService
    {
        private readonly IUserRepository _userRepository;

        private List<Guid>? _establishmentsIds = null;

        private Guid? _userId = null;

        public UserContextService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        //public List<Guid>? EstablishmentsIds
        //{
        //    get {
        //        {
        //            if (_userId != null && _establishmentsIds == null)
        //            {
        //                var estab = _userRepository.Get(((Guid) UserId)).Establishment.Id;
        //                return new List<Guid> { estab }; }
        //            else
        //            {
        //                return _establishmentsIds;
        //            };
        //        };
        //    }
        //    set { _establishmentsIds = value; }
        //}

        public List<Guid>? EstablishmentsIds
        {
            get { return _establishmentsIds; }
            set { _establishmentsIds = value; }
        }
        public Guid? UserId
        {
            get { return _userId; }
            set { _userId = value; }
        }
    }


}
