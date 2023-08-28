using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;

namespace Infrastructure.DataAccess.DBContext;
public class RestaurantContext : DbContext
{
	private readonly ILogger<RestaurantContext> _logger;

	public RestaurantContext(DbContextOptions<RestaurantContext> options, ILogger<RestaurantContext> logger)
			: base(options)
	{
		_logger = logger;
	}
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		if (!optionsBuilder.IsConfigured)
		{
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
			optionsBuilder.UseSqlServer("server=DESKTOP-OO326C9\\SQLEXPRESS;database= Restaurant;Trusted_Connection=True; Encrypt=False;");
		}
	}
	protected override void OnModelCreating(ModelBuilder builder)
	{
		builder.ApplyConfigurationsFromAssembly(typeof(RestaurantContext).Assembly);
	}

	public override DbSet<TEntity> Set<TEntity>()
	{
		try
		{
			return base.Set<TEntity>();
		}
		catch (Exception)
		{
			throw new Exception("حدثت مشكلة عند الاتصال مع قاعدة المعطيات");
		}
	}
	public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		try
		{
			return base.SaveChangesAsync(cancellationToken);
		}
		catch (DBConcurrencyException)
		{
			throw new Exception("حدثت مشكلة في التزامن حاول ثانيةً");
		}
		catch (DbUpdateConcurrencyException)
		{
			throw new Exception("حدثت مشكلة في التزامن عند التعديل حاول ثانيةً");
		}
		catch (Exception)
		{
			throw new Exception("حدثت مشكلة في قاعدة المعطيات حاول ثانيةً");
		}
	}
}
