
using System;
using System.Threading.Tasks;
using MartaList.Extensions;
using MartaList.Infrastructure.Repositories;
using MartaList.Models.Responses;

namespace MartaList.Core
{
    public class ApplicationUserService
    {
        private readonly IUserRepository repository;

        public ApplicationUserService(IUserRepository repository)
        {
            this.repository = repository;
        }

        public async Task<ApplicationUserOut> Create(ApplicationUserOut user)
        {
            Validate(user);
            user.Id = Guid.NewGuid().ToString();

            return (await repository.Upsert(user.AsDbModel())).AsOutModel();
        }

        public async Task<ApplicationUserOut> Update(ApplicationUserOut user)
            => (await repository.Upsert(user.AsDbModel())).AsOutModel();

        public async Task<ApplicationUserOut> GetByExternal(string externalId)
        {
            if (externalId.IsEmpty())
                throw new ArgumentException(nameof(externalId));

            return (await repository.GetByExternalId(externalId)).AsOutModel();
        }

        private void Validate(ApplicationUserOut user)
        {
            // TODO: Add validation
        }
    }
}