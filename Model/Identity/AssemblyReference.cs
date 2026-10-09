namespace XrmSync.Model.Identity;

/// <summary>
/// Identifies the plugin assembly to act on, guaranteeing that a <see cref="Name"/> is always
/// available. Managed identity handling only ever needs the name of the assembly as it is
/// registered in Dataverse, so the name can be supplied on its own; when only a local assembly
/// file is known, the name is derived from its file name instead.
/// </summary>
public record AssemblyReference
{
	private AssemblyReference(string name, string? path)
	{
		Name = name;
		Path = path;
	}

	/// <summary>
	/// Name of the plugin assembly as registered in Dataverse. Never null or empty.
	/// </summary>
	public string Name { get; }

	/// <summary>
	/// Path to the local assembly file, or null when the assembly was identified by name alone.
	/// </summary>
	public string? Path { get; }

	public static AssemblyReference FromName(string name) =>
		string.IsNullOrWhiteSpace(name)
			? throw new Exceptions.XrmSyncException("Assembly name is required and cannot be empty.")
			: new AssemblyReference(name.Trim(), null);

	public static AssemblyReference FromPath(string path) =>
		string.IsNullOrWhiteSpace(path)
			? throw new Exceptions.XrmSyncException("Assembly path is required and cannot be empty.")
			: new AssemblyReference(System.IO.Path.GetFileNameWithoutExtension(path), path);

	/// <summary>
	/// Builds a reference from the two interchangeable inputs: an explicit name wins, otherwise the
	/// path supplies one. Callers validate the inputs first (see
	/// <c>XrmSyncConfigurationValidator.ValidateAssemblyReference</c>), so the throw guards an
	/// invariant rather than reporting user error.
	/// </summary>
	public static AssemblyReference Create(string? assemblyName, string? assemblyPath) =>
		!string.IsNullOrWhiteSpace(assemblyName) ? FromName(assemblyName)
		: !string.IsNullOrWhiteSpace(assemblyPath) ? FromPath(assemblyPath)
		: throw new Exceptions.XrmSyncException("Either an assembly name or an assembly path is required.");
}
