namespace XrmSync.Model.Identity;

public record IdentityCommandOptions(IdentityOperation Operation, AssemblyReference Assembly, string SolutionName, string ClientId, string TenantId);
