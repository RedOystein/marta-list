using System.Threading.Tasks;
using GraphQL.Server.Ui.GraphiQL;
using HotChocolate;
using HotChocolate.AspNetCore;
using HotChocolate.AspNetCore.Interceptors;
using HotChocolate.Execution;
using MartaList.Core;
using MartaList.GraphQl;
using MartaList.GraphQl.Mutations;
using MartaList.GraphQl.Queries;
using MartaList.Infrastructure;
using MartaList.Infrastructure.Models;
using MartaList.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MartaList
{
    public class Startup
    {
        public const string UserId = "c044389a-0635-48e3-8299-5f6e5580b0e1";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
    {
      services.AddQueryRequestInterceptor(AuthenticationInterceptor());
      services.AddGraphQL(
          SchemaBuilder.New()
              .AddQueryType<GroupQueryType>()
              .AddMutationType<Mutation>()
              .AddType<GroupType>()
              // .AddType<ListType>()
              .AddType<ItemType>()
              .AddType<UserType>()
              .Create());

      services.AddDiagnosticObserver<DiagnosticObserver>();

      services.AddLogging(builder => builder.AddConsole());
      services.AddHttpContextAccessor();

      services.AddSingleton<ITableClient<List>, TableClient<List>>();
      services.AddSingleton<ITableClient<ListItem>, TableClient<ListItem>>();
      services.AddSingleton<ITableClient<User>, TableClient<User>>();
      services.AddSingleton<ITableClient<Group>, TableClient<Group>>();

      services.AddSingleton<IGroupService, GroupService>();
      services.AddSingleton<GroupQuery>();
      services.AddSingleton<ListQuery>();
      services.AddSingleton<IUserAuthorizationService, UserAuthorizationService>();
      services.AddSingleton<ApplicationUserService>();
      services.AddSingleton<ListService>();

      services.AddSingleton<IListRepository, ListRepository>();
      services.AddSingleton<IItemRepository, ItemRepository>();
      services.AddSingleton<IGroupRepository, GroupRepository>();
      services.AddSingleton<IUserRepository, UserRepository>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseRouting();

            app.UseGraphQL("/graphql");

            app.UseGraphiQLServer(new GraphiQLOptions
            {
                GraphiQLPath = "/graphiql",
                GraphQLEndPoint = "/graphql"
            });

            // app.UseHttpsRedirection();

            // app.UseRouting();

            // app.UseAuthorization();

            // app.UseEndpoints(endpoints =>
            // {
            //     endpoints.MapControllers();
            // });
        }

        private static OnCreateRequestAsync AuthenticationInterceptor()
        {
            return (context, builder, token) =>
            {
                //if (context.GetUser().Identity.IsAuthenticated)
                {
                    builder.SetProperty("currentUser",
                        new CurrentUser(
                            UserId,//Guid.Parse(context.User.FindFirstValue(ClaimTypes.NameIdentifier)),
                            null//context.User.Claims.Select(x => $"{x.Type} : {x.Value}").ToList()
                            ));
                }

                return Task.CompletedTask;
            };
        }
    }
}