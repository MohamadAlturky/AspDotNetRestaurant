using SharedKernal.Utilities.Result;

namespace Infrastructure.DataAccess.DatabaseSeeding.SeedAdmin;
public interface ISeedAdminService
{
	Task<Result> SeedAdmin();
}
