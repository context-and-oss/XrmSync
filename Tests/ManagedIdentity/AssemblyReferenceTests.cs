using XrmSync.Model.Exceptions;
using XrmSync.Model.Identity;

namespace Tests.ManagedIdentity;

public class AssemblyReferenceTests
{
	[Fact]
	public void FromPathDerivesTheNameAndKeepsThePath()
	{
		// Act
		var reference = AssemblyReference.FromPath("path/to/MyPlugin.dll");

		// Assert
		Assert.Equal("MyPlugin", reference.Name);
		Assert.Equal("path/to/MyPlugin.dll", reference.Path);
	}

	[Fact]
	public void FromNameLeavesThePathUnset()
	{
		// Act
		var reference = AssemblyReference.FromName("  MyPlugin  ");

		// Assert — the name is normalized, and no local file is implied
		Assert.Equal("MyPlugin", reference.Name);
		Assert.Null(reference.Path);
	}

	[Theory]
	[InlineData("")]
	[InlineData("   ")]
	public void FromNameRejectsBlankNames(string name)
	{
		Assert.Throws<XrmSyncException>(() => AssemblyReference.FromName(name));
	}

	[Fact]
	public void CreatePrefersTheExplicitNameOverThePath()
	{
		// Act
		var reference = AssemblyReference.Create("OverriddenName", "path/to/MyPlugin.dll");

		// Assert — the path is not used to derive the name, and is not carried along
		Assert.Equal("OverriddenName", reference.Name);
		Assert.Null(reference.Path);
	}

	[Fact]
	public void CreateFallsBackToThePathWhenNoNameIsSupplied()
	{
		// Act
		var reference = AssemblyReference.Create(null, "path/to/MyPlugin.dll");

		// Assert
		Assert.Equal("MyPlugin", reference.Name);
	}

	[Fact]
	public void CreateThrowsWhenNeitherIsSupplied()
	{
		// Act & Assert — the invariant callers rely on: a reference always yields a name
		Assert.Throws<XrmSyncException>(() => AssemblyReference.Create(null, null));
		Assert.Throws<XrmSyncException>(() => AssemblyReference.Create(string.Empty, string.Empty));
	}
}
